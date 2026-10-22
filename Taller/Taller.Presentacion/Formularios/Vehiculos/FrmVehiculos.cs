using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Navegacion;
using Microsoft.Extensions.DependencyInjection;

namespace Taller.Presentacion.Formularios.Vehiculos;

/// <summary>
/// Consulta y administra datos persistidos de vehículos y concentra
/// la navegación hacia alta, edición e historial.
/// </summary>
public partial class FrmVehiculos : Form, IFormularioConOperacion
{
    private readonly IServiceProvider _proveedorServicios;
    private readonly List<VehiculoFila> _vehiculos = [];

    private bool _configurando;
    public bool OperacionEnCurso { get; private set; }
    private VehiculoServicio Servicio => _proveedorServicios.GetRequiredService<VehiculoServicio>();

    public FrmVehiculos(IServiceProvider proveedorServicios)
    {
        InitializeComponent();
        _proveedorServicios = proveedorServicios
            ?? throw new ArgumentNullException(nameof(proveedorServicios));

        ConfigurarFormulario();
    }

    private async void Formulario_Load(object? sender, EventArgs e) => await CargarAsync();

    private async Task CargarAsync()
    {
        if (OperacionEnCurso) return;
        try
        {
            OperacionEnCurso = true; Enabled = false;
            var activos = await Servicio.ListarActivosAsync(); var inactivos = await Servicio.ListarInactivosAsync();
            var atenciones = await _proveedorServicios.GetRequiredService<AtencionServicio>().ListarTodasAsync();
            var clientes = atenciones.OrderByDescending(a => a.FechaApertura).GroupBy(a => a.IdVehiculo).ToDictionary(g => g.Key, g => g.First().Cliente.Apellido + ", " + g.First().Cliente.Nombre);
            _vehiculos.Clear(); _vehiculos.AddRange(activos.Concat(inactivos).Select(v => new VehiculoFila(v.IdVehiculo, v.Dominio, v.Modelo.Marca.Nombre + " " + v.Modelo.Nombre, v.Anio, v.Color, clientes.GetValueOrDefault(v.IdVehiculo, "Sin atenciones"), v.Activo, v.FechaAlta)));
            AplicarFiltros();
        }
        catch (Exception ex) { _vehiculos.Clear(); AplicarFiltros(); MessageBox.Show(this, ex.Message, "Cargar vehículos"); }
        finally { OperacionEnCurso = false; Enabled = true; }
    }

    private void ConfigurarFormulario()
    {
        _configurando = true;

        dgvUsuarios.AutoGenerateColumns = false;
        dgvUsuarios.MultiSelect = false;
        dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        colId.DataPropertyName = nameof(VehiculoFila.Id);
        dominio.DataPropertyName = nameof(VehiculoFila.Dominio);
        modelo.DataPropertyName = nameof(VehiculoFila.Modelo);
        año.DataPropertyName = nameof(VehiculoFila.Anio);
        color.DataPropertyName = nameof(VehiculoFila.Color);
        colEstado.DataPropertyName = nameof(VehiculoFila.Estado);
        colFechaAlta.DataPropertyName = nameof(VehiculoFila.FechaAlta);

        btnNuevoUsuario.Text = "＋ Nuevo vehículo";
        btnEditarUsuario.Text = "Editar vehículo";
        btnGestionarCredenciales.Text = "Ver historial";
        btnCambiarEstado.Text = "Deshabilitar vehículo";

        label1.Text = "Consulta de vehículos y antecedentes asociados.";
        lblCantidad.Text = "0 vehículos encontrados";
        txtBuscar.PlaceholderText = "Dominio, modelo o cliente de última atención";

        cboEstado.Items.Clear();
        cboEstado.Items.AddRange(["Todos", "Habilitados", "Deshabilitados"]);
        cboEstado.SelectedIndex = 0;

        _configurando = false;
    }



    private void AplicarFiltros()
    {
        if (_configurando)
            return;

        string busqueda = txtBuscar.Text.Trim();
        IEnumerable<VehiculoFila> consulta = _vehiculos;

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(vehiculo =>
                vehiculo.Dominio.Contains(busqueda, StringComparison.OrdinalIgnoreCase)
                || vehiculo.Modelo.Contains(busqueda, StringComparison.OrdinalIgnoreCase)
                || vehiculo.Propietario.Contains(busqueda, StringComparison.OrdinalIgnoreCase));
        }

        consulta = cboEstado.SelectedIndex switch
        {
            1 => consulta.Where(vehiculo => vehiculo.Activo),
            2 => consulta.Where(vehiculo => !vehiculo.Activo),
            _ => consulta
        };

        List<VehiculoFila> resultado = consulta.OrderByDescending(x => x.Activo).ThenByDescending(x => x.FechaAlta).ThenByDescending(x => x.Id).ToList();

        dgvUsuarios.DataSource = null;
        dgvUsuarios.DataSource = resultado;
        dgvUsuarios.ClearSelection();
        dgvUsuarios.CurrentCell = null;

        lblCantidad.Text = resultado.Count == 1
            ? "1 vehículo encontrado"
            : $"{resultado.Count} vehículos encontrados";

        ActualizarAcciones();
    }

    private VehiculoFila? ObtenerSeleccionado()
    {
        return dgvUsuarios.SelectedRows.Count == 1
            ? dgvUsuarios.SelectedRows[0].DataBoundItem as VehiculoFila
            : null;
    }

    private void ActualizarAcciones()
    {
        VehiculoFila? vehiculo = ObtenerSeleccionado();
        bool haySeleccion = vehiculo is not null;

        btnEditarUsuario.Enabled = haySeleccion;
        btnGestionarCredenciales.Enabled = haySeleccion;
        btnCambiarEstado.Enabled = haySeleccion;

        bool activo = vehiculo?.Activo == true;
        btnCambiarEstado.Text = activo
            ? "Deshabilitar vehículo"
            : "Habilitar vehículo";
        btnCambiarEstado.ForeColor = activo
            ? Color.FromArgb(185, 28, 28)
            : Color.FromArgb(30, 64, 175);
        btnCambiarEstado.FlatAppearance.BorderColor = btnCambiarEstado.ForeColor;
    }

    private async void BtnNuevoVehiculo_Click(object? sender, EventArgs e)
    {
        using FrmAltaVehiculo formulario =
            _proveedorServicios.GetRequiredService<FrmAltaVehiculo>();

        if (formulario.ShowDialog(this) == DialogResult.OK) await CargarAsync();
    }

    private async void BtnEditarVehiculo_Click(object? sender, EventArgs e)
    {
        VehiculoFila? vehiculo = ObtenerSeleccionado();
        if (vehiculo is null)
            return;

        using FrmEditarVehiculo formulario =
            _proveedorServicios.GetRequiredService<FrmEditarVehiculo>();

        string[] marcaModelo = vehiculo.Modelo.Split(
            ' ', 2, StringSplitOptions.RemoveEmptyEntries);

        formulario.PrepararEdicion(
            vehiculo.Id,
            vehiculo.Dominio,
            marcaModelo.ElementAtOrDefault(0) ?? vehiculo.Modelo,
            marcaModelo.ElementAtOrDefault(1) ?? vehiculo.Modelo,
            vehiculo.Anio,
            vehiculo.Color,
            vehiculo.Activo,
            vehiculo.FechaAlta);

        if (formulario.ShowDialog(this) == DialogResult.OK) await CargarAsync();
    }

    private void BtnVerHistorial_Click(object? sender, EventArgs e)
    {
        VehiculoFila? vehiculo = ObtenerSeleccionado();
        if (vehiculo is null)
            return;

        using FrmHistorialVehiculo formulario =
            _proveedorServicios.GetRequiredService<FrmHistorialVehiculo>();

        formulario.PrepararConsulta(
            vehiculo.Dominio,
            vehiculo.Modelo,
            vehiculo.Anio,
            vehiculo.Propietario);

        formulario.ShowDialog(this);
    }

    private async void BtnCambiarEstado_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado(); if (seleccionado is null || OperacionEnCurso) return;
        if (MessageBox.Show(this, "¿Confirma cambiar el estado del registro seleccionado?", "Cambiar estado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try { OperacionEnCurso = true; Enabled = false; if (seleccionado.Activo) await Servicio.DarDeBajaAsync(seleccionado.Id); else await Servicio.ReactivarAsync(seleccionado.Id); }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Cambiar estado"); }
        finally { OperacionEnCurso = false; Enabled = true; }
        await CargarAsync();
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e)
    { await CargarAsync(); }

    private void Filtro_Cambio(object? sender, EventArgs e) => AplicarFiltros();

    private void DgvVehiculos_SelectionChanged(object? sender, EventArgs e) =>
        ActualizarAcciones();

    private void DgvVehiculos_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            BtnVerHistorial_Click(sender, EventArgs.Empty);
    }

    private void DgvVehiculos_CellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0
            || dgvUsuarios.Columns[e.ColumnIndex].Name != "colEstado")
            return;

        bool activo = string.Equals(e.Value?.ToString(), "Habilitado",
            StringComparison.OrdinalIgnoreCase);

        e.CellStyle.ForeColor = activo
            ? Color.FromArgb(22, 101, 52)
            : Color.FromArgb(185, 28, 28);
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
    }

    private sealed class VehiculoFila
    {
        public VehiculoFila(
            int id,
            string dominio,
            string modelo,
            int anio,
            string color,
            string propietario,
            bool activo,
            DateTime fechaAlta)
        {
            Id = id;
            Dominio = dominio;
            Modelo = modelo;
            Anio = anio;
            Color = color;
            Propietario = propietario;
            Activo = activo;
            FechaAlta = fechaAlta;
        }

        public int Id { get; }
        public string Dominio { get; }
        public string Modelo { get; }
        public int Anio { get; }
        public string Color { get; }
        public string Propietario { get; }
        public bool Activo { get; set; }
        public string Estado => Activo ? "Habilitado" : "Deshabilitado";
        public DateTime FechaAlta { get; }
    }
}

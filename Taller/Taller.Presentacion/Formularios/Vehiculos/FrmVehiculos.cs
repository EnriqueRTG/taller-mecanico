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
        _proveedorServicios = proveedorServicios ?? throw new ArgumentNullException(nameof(proveedorServicios));
        ConfigurarFormulario();
    }

    private async void Formulario_Load(object? sender, EventArgs e) => await CargarAsync();
    private async Task CargarAsync()
    {
        if (OperacionEnCurso)
            return;
        try
        {
            OperacionEnCurso = true;
            Enabled = false;
            var registros = await Servicio.ListarAsync();
            var atenciones = await _proveedorServicios.GetRequiredService<AtencionServicio>().ListarTodasAsync();
            var clientes = atenciones.OrderByDescending(a => a.FechaApertura).GroupBy(a => a.IdVehiculo).ToDictionary(g => g.Key, g => g.First().Cliente.Apellido + ", " + g.First().Cliente.Nombre);
            _vehiculos.Clear();
            _vehiculos.AddRange(registros.Select(v => new VehiculoFila(v.IdVehiculo, v.Dominio, v.Modelo.Marca.Nombre + " " + v.Modelo.Nombre, v.Anio, v.Color, clientes.GetValueOrDefault(v.IdVehiculo, "Sin atenciones"), v.Activo, v.FechaAlta, v.Version.ToArray())));
            AplicarFiltros();
        }
        catch (Exception ex)
        {
            _vehiculos.Clear();
            AplicarFiltros();
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Cargar vehículos");
        }
        finally
        {
            OperacionEnCurso = false;
            Enabled = true;
        }
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
            consulta = consulta.Where(vehiculo => Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(vehiculo.Dominio, busqueda) || Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(vehiculo.Modelo, busqueda) || Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(vehiculo.UltimoClienteAtendido, busqueda) || Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(vehiculo.Color, busqueda) || vehiculo.Anio.ToString().Contains(busqueda, StringComparison.OrdinalIgnoreCase));
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
        lblCantidad.Text = resultado.Count == 1 ? "1 vehículo encontrado" : $"{resultado.Count} vehículos encontrados";
        ActualizarAcciones();
    }

    private VehiculoFila? ObtenerSeleccionado()
    {
        return dgvUsuarios.SelectedRows.Count == 1 ? dgvUsuarios.SelectedRows[0].DataBoundItem as VehiculoFila : null;
    }

    private void ActualizarAcciones()
    {
        VehiculoFila? vehiculo = ObtenerSeleccionado();
        bool haySeleccion = vehiculo is not null;
        btnEditarUsuario.Enabled = haySeleccion;
        btnGestionarCredenciales.Enabled = haySeleccion;
        btnCambiarEstado.Enabled = haySeleccion;
        bool activo = vehiculo?.Activo == true;
        btnCambiarEstado.Text = activo ? "Deshabilitar vehículo" : "Habilitar vehículo";
        btnCambiarEstado.ForeColor = activo ? Color.FromArgb(185, 28, 28) : Color.FromArgb(30, 64, 175);
        btnCambiarEstado.FlatAppearance.BorderColor = btnCambiarEstado.ForeColor;
    }

    private async void BtnNuevoVehiculo_Click(object? sender, EventArgs e)
    {
        using FrmAltaVehiculo formulario = _proveedorServicios.GetRequiredService<FrmAltaVehiculo>();
        if (formulario.ShowDialog(this) == DialogResult.OK)
            await CargarAsync();
    }

    private async void BtnEditarVehiculo_Click(object? sender, EventArgs e)
    {
        VehiculoFila? vehiculo = ObtenerSeleccionado();
        if (vehiculo is null)
            return;
        using FrmEditarVehiculo formulario = _proveedorServicios.GetRequiredService<FrmEditarVehiculo>();
        try
        {
            var datos = await Servicio.ObtenerEdicionAsync(vehiculo.Id);
            formulario.PrepararEdicion(datos);
            formulario.CargarModelos(await Servicio.ListarModelosAsync(datos.IdModelo));
            if (formulario.ShowDialog(this) == DialogResult.OK)
                await CargarAsync();
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Abrir edición");
        }
    }

    private void BtnVerHistorial_Click(object? sender, EventArgs e)
    {
        VehiculoFila? vehiculo = ObtenerSeleccionado();
        if (vehiculo is null)
            return;
        using FrmHistorialVehiculo formulario = _proveedorServicios.GetRequiredService<FrmHistorialVehiculo>();
        formulario.PrepararConsulta(vehiculo.Dominio, vehiculo.Modelo, vehiculo.Anio, vehiculo.UltimoClienteAtendido);
        formulario.ShowDialog(this);
    }

    private async void BtnCambiarEstado_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null || OperacionEnCurso)
            return;
        if (MessageBox.Show(this, "¿Confirma cambiar el estado del registro seleccionado?", "Cambiar estado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        try
        {
            OperacionEnCurso = true;
            Enabled = false;
            if (seleccionado.Activo)
                await Servicio.DarDeBajaAsync(seleccionado.Id, seleccionado.Version);
            else
                await Servicio.ReactivarAsync(seleccionado.Id, seleccionado.Version);
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Cambiar estado");
        }
        finally
        {
            OperacionEnCurso = false;
            Enabled = true;
        }

        await CargarAsync();
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e)
    {
        await CargarAsync();
    }

    private void Filtro_Cambio(object? sender, EventArgs e) => AplicarFiltros();
    private void DgvVehiculos_SelectionChanged(object? sender, EventArgs e) => ActualizarAcciones();
    private void DgvVehiculos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            BtnVerHistorial_Click(sender, EventArgs.Empty);
    }

    private void DgvVehiculos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvUsuarios.Columns[e.ColumnIndex].Name != "colEstado")
            return;
        bool activo = string.Equals(e.Value?.ToString(), "Habilitado", StringComparison.OrdinalIgnoreCase);
        e.CellStyle.ForeColor = activo ? Color.FromArgb(22, 101, 52) : Color.FromArgb(185, 28, 28);
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
    }

    private sealed class VehiculoFila
    {
        public VehiculoFila(int id, string dominio, string modelo, int anio, string color, string ultimoClienteAtendido, bool activo, DateTime fechaAlta, byte[] version)
        {
            Id = id;
            Dominio = dominio;
            Modelo = modelo;
            Anio = anio;
            Color = color;
            UltimoClienteAtendido = ultimoClienteAtendido;
            Activo = activo;
            FechaAlta = fechaAlta;
            Version = version.ToArray();
        }

        public byte[] Version { get; }
        public int Id { get; }
        public string Dominio { get; }
        public string Modelo { get; }
        public int Anio { get; }
        public string Color { get; }
        public string UltimoClienteAtendido { get; }
        public bool Activo { get; set; }
        public string Estado => Activo ? "Habilitado" : "Deshabilitado";
        public DateTime FechaAlta { get; }
    }
}

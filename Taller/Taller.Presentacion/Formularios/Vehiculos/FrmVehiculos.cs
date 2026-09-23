using Microsoft.Extensions.DependencyInjection;

namespace Taller.Presentacion.Formularios.Vehiculos;

/// <summary>
/// Simula la consulta y administración de vehículos y concentra
/// la navegación hacia alta, edición e historial.
/// </summary>
public partial class FrmVehiculos : Form
{
    private readonly IServiceProvider _proveedorServicios;
    private readonly List<VehiculoFila> _vehiculos =
    [
        new(1, "AB345CD", "Honda Fit", 2020, "Gris", "Juan Pérez",
            true, new DateTime(2025, 3, 12)),
        new(2, "AC902LM", "Toyota Corolla", 2018, "Blanco", "Juan Pérez",
            true, new DateTime(2025, 7, 9)),
        new(3, "AA123BC", "Ford Fiesta", 2017, "Azul", "Laura Gómez",
            false, new DateTime(2024, 10, 22)),
        new(4, "AE778QP", "Volkswagen Amarok", 2022, "Negro", "Sofía Fernández",
            true, new DateTime(2026, 1, 15))
    ];

    private bool _configurando;

    public FrmVehiculos(IServiceProvider proveedorServicios)
    {
        InitializeComponent();
        _proveedorServicios = proveedorServicios
            ?? throw new ArgumentNullException(nameof(proveedorServicios));

        ConfigurarFormulario();
        ConectarEventos();
        AplicarFiltros();
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
        txtBuscar.PlaceholderText = "Dominio, modelo o propietario";

        cboEstado.Items.Clear();
        cboEstado.Items.AddRange(["Todos", "Habilitados", "Deshabilitados"]);
        cboEstado.SelectedIndex = 0;

        _configurando = false;
    }

    private void ConectarEventos()
    {
        btnNuevoUsuario.Click += BtnNuevoVehiculo_Click;
        btnEditarUsuario.Click += BtnEditarVehiculo_Click;
        btnGestionarCredenciales.Click += BtnVerHistorial_Click;
        btnCambiarEstado.Click += BtnCambiarEstado_Click;
        btnActualizar.Click += BtnActualizar_Click;
        txtBuscar.TextChanged += Filtro_Cambio;
        cboEstado.SelectedIndexChanged += Filtro_Cambio;
        dgvUsuarios.SelectionChanged += DgvVehiculos_SelectionChanged;
        dgvUsuarios.CellDoubleClick += DgvVehiculos_CellDoubleClick;
        dgvUsuarios.CellFormatting += DgvVehiculos_CellFormatting;
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

        List<VehiculoFila> resultado = consulta.ToList();

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

    private void BtnNuevoVehiculo_Click(object? sender, EventArgs e)
    {
        using FrmAltaVehiculo formulario =
            _proveedorServicios.GetRequiredService<FrmAltaVehiculo>();

        formulario.ShowDialog(this);
    }

    private void BtnEditarVehiculo_Click(object? sender, EventArgs e)
    {
        VehiculoFila? vehiculo = ObtenerSeleccionado();
        if (vehiculo is null)
            return;

        using editar_vehiculo formulario =
            _proveedorServicios.GetRequiredService<editar_vehiculo>();

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

        formulario.ShowDialog(this);
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

    private void BtnCambiarEstado_Click(object? sender, EventArgs e)
    {
        VehiculoFila? vehiculo = ObtenerSeleccionado();
        if (vehiculo is null)
            return;

        string accion = vehiculo.Activo ? "deshabilitar" : "habilitar";

        DialogResult respuesta = MessageBox.Show(
            $"¿Está seguro de que desea {accion} el vehículo " +
            $"\"{vehiculo.Dominio}\"?\n\n" +
            "Esta operación es una simulación y no modificará la base de datos.",
            "Confirmar cambio de estado",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes)
            return;

        vehiculo.Activo = !vehiculo.Activo;
        AplicarFiltros();

        MessageBox.Show(
            $"El vehículo fue {(vehiculo.Activo ? "habilitado" : "deshabilitado")} " +
            "en la simulación.",
            "Estado actualizado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void BtnActualizar_Click(object? sender, EventArgs e)
    {
        txtBuscar.Clear();
        cboEstado.SelectedIndex = 0;
        AplicarFiltros();
    }

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

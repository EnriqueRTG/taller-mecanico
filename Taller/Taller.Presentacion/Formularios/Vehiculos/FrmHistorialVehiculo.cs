using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Vehiculos;

/// <summary>
/// Representa la consulta visual del historial de atenciones
/// asociadas a un vehículo.
/// </summary>
public partial class FrmHistorialVehiculo : Form
{
    private string _dominio = "AB345CD";
    private string _modelo = "Honda Fit";
    private int _anio = 2020;
    private string _cliente = "Juan Pérez";

    public FrmHistorialVehiculo()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Recibe los datos de la fila seleccionada antes de abrir el historial.
    /// </summary>
    public void PrepararConsulta(
        string dominio,
        string modelo,
        int anio,
        string cliente)
    {
        _dominio = string.IsNullOrWhiteSpace(dominio) ? "Sin dominio" : dominio;
        _modelo = string.IsNullOrWhiteSpace(modelo) ? "Sin modelo" : modelo;
        _anio = anio;
        _cliente = string.IsNullOrWhiteSpace(cliente) ? "Sin propietario" : cliente;
    }

    private void FrmHistorialVehiculo_Load(object? sender, EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarDatosSimulados();
    }

    private void ConfigurarFormulario()
    {
        Text = $"Historial del vehículo - {_dominio}";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        CancelButton = btnCerrar;
    }

    private void CargarDatosSimulados()
    {
        lblDominioEncabezado.Text = _dominio;
        lblDominioValor.Text = _dominio;
        lblModeloValor.Text = _modelo;
        lblAnioValor.Text = _anio.ToString();
        lblClienteValor.Text = _cliente;

        dgvHistorial.Rows.Clear();

        dgvHistorial.Rows.Add(
            "000001",
            "07/09/2026",
            "Dificultades para arrancar",
            "Falla en el sistema de encendido",
            "Revisión y mantenimiento del sistema",
            "Finalizada");

        dgvHistorial.Rows.Add(
            "000014",
            "15/05/2026",
            "Mantenimiento general",
            "Mantenimiento preventivo",
            "Cambio de aceite y filtros",
            "Finalizada");

        dgvHistorial.Rows.Add(
            "000027",
            "10/02/2026",
            "Ruido en el tren delantero",
            "Desgaste de componentes",
            "Ajuste y reemplazo de bujes",
            "Finalizada");

        lblCantidad.Text = dgvHistorial.Rows.Count == 1
            ? "1 atención registrada"
            : $"{dgvHistorial.Rows.Count} atenciones registradas";

        dgvHistorial.ClearSelection();
        dgvHistorial.CurrentCell = null;
        dgvHistorial.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void AplicarEstilos()
    {
        BackColor = EstilosInterfaz.FondoAplicacion;
        EstilosInterfaz.AplicarBotonSecundario(btnCerrar);
        EstilosInterfaz.AplicarGrilla(dgvHistorial);
    }
}

using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Vehiculos;

/// <summary>
/// Representa la consulta visual del historial de atenciones
/// asociadas a un vehículo.
/// </summary>
/// <remarks>
/// Durante la primera entrega utiliza datos demostrativos y no
/// consulta información adicional de la base de datos.
/// </remarks>
public partial class FrmHistorialVehiculo : Form
{
    #region Inicialización

    /// <summary>
    /// Inicializa una nueva instancia del historial del vehículo.
    /// </summary>
    public FrmHistorialVehiculo()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Configura la ventana, aplica sus estilos y carga
    /// la información demostrativa.
    /// </summary>
    private void FrmHistorialVehiculo_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarDatosSimulados();
    }

    /// <summary>
    /// Configura el comportamiento general del formulario.
    /// </summary>
    private void ConfigurarFormulario()
    {
        Text = "Historial del vehículo";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        CancelButton = btnCerrar;
    }

    #endregion

    #region Datos simulados

    /// <summary>
    /// Carga los datos generales y las atenciones demostrativas
    /// asociadas al vehículo.
    /// </summary>
    private void CargarDatosSimulados()
    {
        lblDominioEncabezado.Text = "AB345CD";

        lblDominioValor.Text = "AB345CD";
        lblModeloValor.Text = "Honda Fit";
        lblAnioValor.Text = "2020";
        lblClienteValor.Text = "Juan Pérez";

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

        lblCantidad.Text =
            $"{dgvHistorial.Rows.Count} atenciones registradas";

        dgvHistorial.ClearSelection();
        dgvHistorial.CurrentCell = null;
        dgvHistorial.AutoResizeRows(
            DataGridViewAutoSizeRowsMode.AllCells);
    }

    #endregion

    #region Eventos

    /// <summary>
    /// Cierra la pantalla de consulta del historial.
    /// </summary>
    private void BtnCerrar_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    #endregion

    #region Estilos

    /// <summary>
    /// Aplica los estilos compartidos a la grilla y al botón.
    /// </summary>
    private void AplicarEstilos()
    {
        BackColor =
            EstilosInterfaz.FondoAplicacion;

        EstilosInterfaz.AplicarBotonSecundario(
            btnCerrar);

        EstilosInterfaz.AplicarGrilla(
            dgvHistorial);
    }

    #endregion
}
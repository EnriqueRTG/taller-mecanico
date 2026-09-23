using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Comprobantes;

/// <summary>
/// Representa la gestión visual del comprobante y de los pagos
/// asociados con una atención.
/// </summary>
/// <remarks>
/// Durante la primera entrega utiliza información demostrativa
/// y conserva los cambios únicamente mientras la ventana permanece abierta.
/// </remarks>
public partial class FrmComprobantePagos : Form
{
    #region Constantes y campos

    private const decimal TotalPresupuesto = 43500m;

    private bool _comprobanteEmitido;
    private decimal _totalAbonado;

    #endregion

    #region Inicialización

    public FrmComprobantePagos()
    {
        InitializeComponent();
    }

    private void FrmComprobantePagos_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarDatosSimulados();
        ActualizarResumenFinanciero();
    }

    private void ConfigurarFormulario()
    {
        Text = "Comprobante y pagos";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        CancelButton = btnCerrar;
    }

    #endregion

    #region Datos simulados

    private void CargarDatosSimulados()
    {
        lblNumeroAtencion.Text =
            "Atención N.º 000001";

        lblClienteValor.Text =
            "Juan Pérez";

        lblVehiculoValor.Text =
            "Honda Fit";

        lblDominioValor.Text =
            "AB345CD";

        lblPresupuestoValor.Text =
            FormatearImporte(TotalPresupuesto);

        _comprobanteEmitido = false;
        _totalAbonado = 0m;

        dgvPagos.Rows.Clear();
        dgvPagos.ClearSelection();
        dgvPagos.CurrentCell = null;
    }

    #endregion

    #region Eventos

    private void BtnEmitirComprobante_Click(
        object? sender,
        EventArgs e)
    {
        SimularEmisionComprobante();
    }

    private void BtnRegistrarPago_Click(
    object? sender,
    EventArgs e)
    {
        decimal saldoPendiente =
            TotalPresupuesto - _totalAbonado;

        if (!_comprobanteEmitido)
        {
            MessageBox.Show(
                this,
                "Primero debe emitirse el comprobante.",
                "Registrar pago",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        if (saldoPendiente <= 0)
        {
            MessageBox.Show(
                this,
                "El comprobante no posee saldo pendiente.",
                "Registrar pago",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        using FrmRegistrarPago formulario =
            new(saldoPendiente);

        if (formulario.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        AgregarPagoSimulado(formulario);
    }

    /// <summary>
    /// Incorpora el pago simulado a la grilla y actualiza
    /// el resumen financiero.
    /// </summary>
    private void AgregarPagoSimulado(
        FrmRegistrarPago formulario)
    {
        string referencia =
            string.IsNullOrWhiteSpace(formulario.Referencia)
                ? "Sin referencia"
                : formulario.Referencia;

        dgvPagos.Rows.Add(
            formulario.FechaPago.ToString("dd/MM/yyyy HH:mm"),
            formulario.MedioPago,
            referencia,
            FormatearImporte(formulario.Importe),
            "Administrativo actual");

        _totalAbonado += formulario.Importe;

        ActualizarResumenFinanciero();
        dgvPagos.ClearSelection();
    }

    private void BtnCerrar_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    #endregion

    #region Simulación del comprobante

    private void SimularEmisionComprobante()
    {
        if (_comprobanteEmitido)
        {
            MessageBox.Show(
                this,
                "El comprobante ya fue emitido durante esta sesión.",
                "Comprobante emitido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        DialogResult respuesta =
            MessageBox.Show(
                this,
                "¿Desea simular la emisión del comprobante " +
                "correspondiente a esta atención?",
                "Emitir comprobante",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes)
        {
            return;
        }

        _comprobanteEmitido = true;

        MessageBox.Show(
            this,
            "El comprobante fue emitido correctamente.",
            "Comprobante emitido",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        ActualizarResumenFinanciero();
    }

    private void ActualizarResumenFinanciero()
    {
        decimal saldo =
            TotalPresupuesto - _totalAbonado;

        lblEstadoValor.Text =
            _comprobanteEmitido
                ? "Emitido"
                : "No emitido";

        lblEstadoValor.ForeColor =
            _comprobanteEmitido
                ? Color.FromArgb(22, 101, 52)
                : Color.FromArgb(180, 83, 9);

        lblNumeroValor.Text =
            _comprobanteEmitido
                ? "C-000001"
                : "Sin asignar";

        lblAbonadoValor.Text =
            FormatearImporte(_totalAbonado);

        lblSaldoValor.Text =
            FormatearImporte(saldo);

        lblSaldoValor.ForeColor =
            saldo <= 0
                ? Color.FromArgb(22, 101, 52)
                : Color.FromArgb(185, 28, 28);

        btnEmitirComprobante.Enabled =
            !_comprobanteEmitido;

        btnRegistrarPago.Enabled =
            _comprobanteEmitido && saldo > 0;

        decimal saldoPendiente =
            TotalPresupuesto - _totalAbonado;

        btnRegistrarPago.Enabled =
            _comprobanteEmitido
            && saldoPendiente > 0;

        btnEmitirComprobante.Enabled =
            !_comprobanteEmitido;
    }

    private static string FormatearImporte(
        decimal importe)
    {
        return importe.ToString(
            "C2",
            new System.Globalization.CultureInfo("es-AR"));
    }

    #endregion

    #region Estilos

    private void AplicarEstilos()
    {
        BackColor =
            EstilosInterfaz.FondoAplicacion;

        EstilosInterfaz.AplicarBotonPrimario(
            btnEmitirComprobante);

        EstilosInterfaz.AplicarBotonPrimario(
            btnRegistrarPago);

        EstilosInterfaz.AplicarBotonSecundario(
            btnCerrar);

        EstilosInterfaz.AplicarGrilla(
            dgvPagos);
    }

    #endregion
}
namespace Taller.Presentacion.Formularios.Comprobantes;

/// <summary>
/// Representa visualmente el registro de un pago asociado
/// a una atención.
///
/// Durante la primera entrega no persiste información
/// en la base de datos.
/// </summary>
public partial class FrmRegistrarPago : Form
{
    #region Campos y propiedades

    private readonly decimal _saldoPendiente;

    /// <summary>
    /// Obtiene la fecha y hora del pago simulado.
    /// </summary>
    public DateTime FechaPago =>
        dtpFechaPago.Value;

    /// <summary>
    /// Obtiene el medio de pago seleccionado.
    /// </summary>
    public string MedioPago =>
        cboMedioPago.SelectedItem?.ToString()
        ?? string.Empty;

    /// <summary>
    /// Obtiene el importe ingresado.
    /// </summary>
    public decimal Importe =>
        nudImporte.Value;

    /// <summary>
    /// Obtiene la referencia ingresada para el pago.
    /// </summary>
    public string Referencia =>
        txtReferencia.Text.Trim();

    /// <summary>
    /// Obtiene las observaciones ingresadas.
    /// </summary>
    public string Observaciones =>
        txtObservaciones.Text.Trim();

    #endregion

    #region Inicialización

    public FrmRegistrarPago(decimal saldoPendiente)
    {
        InitializeComponent();

        _saldoPendiente = saldoPendiente;
    }

    /// <summary>
    /// Configura el formulario y carga sus opciones iniciales.
    /// </summary>
    private void FrmRegistrarPago_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarMediosPago();
    }

    /// <summary>
    /// Establece el comportamiento general de la ventana.
    /// </summary>
    private void ConfigurarFormulario()
    {
        Text = "Registrar pago";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;

        dtpFechaPago.Format = DateTimePickerFormat.Custom;
        dtpFechaPago.CustomFormat = "dd/MM/yyyy HH:mm";
        dtpFechaPago.Value = DateTime.Now;

        cboMedioPago.DropDownStyle =
            ComboBoxStyle.DropDownList;

        nudImporte.DecimalPlaces = 2;
        nudImporte.ThousandsSeparator = true;
        nudImporte.Minimum = 0;
        nudImporte.Maximum = 999999999;
        nudImporte.Increment = 1000;

        txtReferencia.MaxLength = 100;
        txtObservaciones.MaxLength = 500;

        lblSaldoDisponible.Text =
            $"Saldo pendiente: {FormatearImporte(_saldoPendiente)}";
    }

    /// <summary>
    /// Carga los medios de pago disponibles para la simulación.
    /// </summary>
    private void CargarMediosPago()
    {
        cboMedioPago.Items.Clear();

        cboMedioPago.Items.AddRange(
        [
            "Efectivo",
            "Tarjeta de débito",
            "Tarjeta de crédito",
            "Transferencia bancaria",
            "Billetera virtual"
        ]);

        cboMedioPago.SelectedIndex = -1;
    }

    #endregion

    #region Eventos

    /// <summary>
    /// Valida los datos y confirma el registro simulado.
    /// </summary>
    private void BtnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        if (!ValidarFormulario())
        {
            return;
        }

        txtReferencia.Text = txtReferencia.Text.Trim();
        txtObservaciones.Text = txtObservaciones.Text.Trim();

        MessageBox.Show(
            this,
            "El pago fue registrado correctamente.",
            "Registrar pago",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
    }

    /// <summary>
    /// Cancela la operación y cierra la ventana.
    /// </summary>
    private void BtnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    #endregion

    #region Validación

    /// <summary>
    /// Comprueba que el medio de pago y el importe sean válidos.
    /// </summary>
    private bool ValidarFormulario()
    {
        if (cboMedioPago.SelectedIndex < 0)
        {
            MostrarAdvertencia(
                "Seleccioná un medio de pago.",
                cboMedioPago);

            return false;
        }

        if (nudImporte.Value <= 0)
        {
            MostrarAdvertencia(
                "Ingresá un importe mayor que cero.",
                nudImporte);

            return false;
        }

        if (nudImporte.Value > _saldoPendiente)
        {
            MostrarAdvertencia(
                "El importe no puede superar el saldo pendiente.",
                nudImporte);

            return false;
        }

        return true;
    }

    /// <summary>
    /// Muestra una advertencia y posiciona el foco en el control.
    /// </summary>
    private void MostrarAdvertencia(
        string mensaje,
        Control control)
    {
        MessageBox.Show(
            this,
            mensaje,
            "Datos incompletos",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        control.Focus();
    }

    #endregion

    #region Estilos

    /// <summary>
    /// Aplica el estilo visual correspondiente a cada botón.
    /// </summary>
    private void AplicarEstilos()
    {
        ConfigurarBotonPrimario(btnGuardar);
        ConfigurarBotonSecundario(btnCancelar);
    }

    private static void ConfigurarBotonPrimario(
        Button boton)
    {
        boton.BackColor = Color.FromArgb(30, 64, 175);
        boton.ForeColor = Color.White;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.Cursor = Cursors.Hand;
        boton.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
    }

    private static void ConfigurarBotonSecundario(
        Button boton)
    {
        boton.BackColor = Color.White;
        boton.ForeColor = Color.FromArgb(51, 65, 85);
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderColor =
            Color.FromArgb(203, 213, 225);

        boton.FlatAppearance.BorderSize = 1;
        boton.Cursor = Cursors.Hand;
        boton.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Regular);
    }

    private static string FormatearImporte(decimal importe)
    {
        return importe.ToString(
            "C2",
            System.Globalization.CultureInfo.GetCultureInfo("es-AR"));
    }

    #endregion
}
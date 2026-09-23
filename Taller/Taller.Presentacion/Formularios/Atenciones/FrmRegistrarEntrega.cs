namespace Taller.Presentacion.Formularios.Atenciones;

/// <summary>
/// Representa visualmente la entrega de un vehículo
/// correspondiente a una atención finalizada.
///
/// Durante la primera entrega trabaja con información simulada
/// y no realiza operaciones sobre la base de datos.
/// </summary>
public partial class FrmRegistrarEntrega : Form
{
    #region Campos y propiedades

    private const int LongitudMaximaObservaciones = 500;

    private bool _entregaConfirmada;

    /// <summary>
    /// Obtiene la fecha y hora seleccionadas para la entrega.
    /// </summary>
    public DateTime FechaEntrega =>
        dtpFechaEntrega.Value;

    /// <summary>
    /// Obtiene el nombre de la persona que recibe el vehículo.
    /// </summary>
    public string RecibidoPor =>
        txtRecibe.Text.Trim();

    /// <summary>
    /// Obtiene el documento de la persona que recibe el vehículo.
    /// </summary>
    public string DocumentoReceptor =>
        txtDocumento.Text.Trim();

    /// <summary>
    /// Obtiene las observaciones registradas durante la entrega.
    /// </summary>
    public string Observaciones =>
        txtObservaciones.Text.Trim();

    #endregion

    #region Inicialización

    public FrmRegistrarEntrega()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Configura el formulario, aplica los estilos visuales
    /// y carga la información demostrativa.
    /// </summary>
    private void FrmRegistrarEntrega_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarDatosSimulados();
    }

    /// <summary>
    /// Establece el comportamiento general de los controles.
    /// </summary>
    private void ConfigurarFormulario()
    {
        Text = "Registrar entrega";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        AcceptButton = btnConfirmarEntrega;
        CancelButton = btnCancelar;

        dtpFechaEntrega.Format =
            DateTimePickerFormat.Custom;

        dtpFechaEntrega.CustomFormat =
            "dd/MM/yyyy HH:mm";

        dtpFechaEntrega.Value = DateTime.Now;

        txtRecibe.MaxLength = 100;
        txtDocumento.MaxLength = 10;
        txtObservaciones.MaxLength =
            LongitudMaximaObservaciones;
    }

    /// <summary>
    /// Carga información simulada de la atención y del responsable.
    /// </summary>
    private void CargarDatosSimulados()
    {
        lblNumeroAtencion.Text =
            "Atención N.º 000001";

        lblClienteValor.Text =
            "Juan Santos";

        lblVehiculoValor.Text =
            "Citroën Jumper";

        lblDominioValor.Text =
            "AB123CD";

        lblSaldoValor.Text =
            "Pagado";

        lblSaldoValor.Text =
            "$ 0,00";

        txtResponsable.Text =
            "Administrativo actual";

        txtRecibe.Clear();
        txtDocumento.Clear();
        txtObservaciones.Clear();

        chkConfirmarEntrega.Checked = false;
    }

    #endregion

    #region Eventos

    /// <summary>
    /// Posiciona el foco en el primer campo editable.
    /// </summary>
    private void FrmRegistrarEntrega_Shown(
        object? sender,
        EventArgs e)
    {
        txtRecibe.Focus();
    }

    /// <summary>
    /// Valida los datos y confirma visualmente la entrega.
    /// </summary>
    private void BtnConfirmarEntrega_Click(
        object? sender,
        EventArgs e)
    {
        if (!ValidarFormulario())
        {
            return;
        }

        ConfirmarEntregaSimulada();
    }

    /// <summary>
    /// Solicita cerrar el formulario sin confirmar la entrega.
    /// </summary>
    private void BtnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    /// <summary>
    /// Advierte sobre información ingresada que todavía
    /// no fue confirmada.
    /// </summary>
    private void FrmRegistrarEntrega_FormClosing(
        object? sender,
        FormClosingEventArgs e)
    {
        if (_entregaConfirmada || !HayDatosIngresados())
        {
            return;
        }

        DialogResult respuesta = MessageBox.Show(
            this,
            "Hay datos de la entrega que no fueron confirmados. " +
            "¿Desea cerrar igualmente?",
            "Cancelar entrega",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (respuesta == DialogResult.No)
        {
            e.Cancel = true;
            DialogResult = DialogResult.None;
            return;
        }

        if (DialogResult == DialogResult.None)
        {
            DialogResult = DialogResult.Cancel;
        }
    }

    #endregion

    #region Validación

    /// <summary>
    /// Comprueba que se hayan completado los datos obligatorios
    /// y aceptado la confirmación de entrega.
    /// </summary>
    private bool ValidarFormulario()
    {
        string receptor = txtRecibe.Text.Trim();
        string documento = txtDocumento.Text.Trim();

        if (string.IsNullOrWhiteSpace(receptor))
        {
            MostrarAdvertencia(
                "Ingresá el nombre de la persona que recibe el vehículo.",
                txtRecibe);

            return false;
        }

        if (receptor.Length < 3)
        {
            MostrarAdvertencia(
                "El nombre de la persona debe contener al menos 3 caracteres.",
                txtRecibe);

            return false;
        }

        if (string.IsNullOrWhiteSpace(documento))
        {
            MostrarAdvertencia(
                "Ingresá el documento de la persona que recibe el vehículo.",
                txtDocumento);

            return false;
        }

        if (!documento.All(char.IsDigit))
        {
            MostrarAdvertencia(
                "El documento debe contener solamente números.",
                txtDocumento);

            return false;
        }

        if (documento.Length < 7)
        {
            MostrarAdvertencia(
                "El documento debe contener al menos 7 números.",
                txtDocumento);

            return false;
        }

        if (!chkConfirmarEntrega.Checked)
        {
            MessageBox.Show(
                this,
                "Debe confirmar la entrega del vehículo y su documentación.",
                "Confirmación requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            chkConfirmarEntrega.Focus();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determina si el usuario ingresó información que podría perderse.
    /// </summary>
    private bool HayDatosIngresados()
    {
        return !string.IsNullOrWhiteSpace(txtRecibe.Text)
            || !string.IsNullOrWhiteSpace(txtDocumento.Text)
            || !string.IsNullOrWhiteSpace(txtObservaciones.Text)
            || chkConfirmarEntrega.Checked;
    }

    /// <summary>
    /// Muestra una advertencia y devuelve el foco al control indicado.
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

    #region Simulación

    /// <summary>
    /// Simula la confirmación exitosa de la entrega.
    /// </summary>
    private void ConfirmarEntregaSimulada()
    {
        txtRecibe.Text = txtRecibe.Text.Trim();
        txtDocumento.Text = txtDocumento.Text.Trim();
        txtObservaciones.Text =
            txtObservaciones.Text.Trim();

        MessageBox.Show(
            this,
            "La entrega del vehículo fue registrada correctamente.",
            "Entrega registrada",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        _entregaConfirmada = true;
        DialogResult = DialogResult.OK;
    }

    #endregion

    #region Estilos

    /// <summary>
    /// Aplica los estilos utilizados por los formularios del sistema.
    /// </summary>
    private void AplicarEstilos()
    {
        ConfigurarBotonPrimario(
            btnConfirmarEntrega);

        ConfigurarBotonSecundario(
            btnCancelar);

        ConfigurarCampoSoloLectura(
            txtResponsable);
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

    private static void ConfigurarCampoSoloLectura(
        TextBox campo)
    {
        campo.ReadOnly = true;
        campo.BackColor =
            Color.FromArgb(248, 250, 252);

        campo.ForeColor =
            Color.FromArgb(51, 65, 85);

        campo.BorderStyle =
            BorderStyle.FixedSingle;

        campo.TabStop = false;
    }

    #endregion
}
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Presupuestos;

/// <summary>
/// Representa el registro visual de la decisión comunicada
/// por el cliente sobre un presupuesto.
/// </summary>
/// <remarks>
/// Durante la primera entrega utiliza información demostrativa
/// y no modifica datos persistidos.
/// </remarks>
public partial class FrmDecisionPresupuesto : Form
{
    #region Constantes y campos

    private const int LongitudMaximaObservaciones = 1000;

    private bool _formularioInicializado;
    private bool _hayCambios;
    private bool _decisionConfirmada;

    #endregion

    #region Propiedades públicas

    /// <summary>
    /// Obtiene la decisión seleccionada.
    /// </summary>
    public string Decision =>
        cboDecision.SelectedItem?.ToString()
        ?? string.Empty;

    /// <summary>
    /// Obtiene las observaciones ingresadas.
    /// </summary>
    public string Observaciones =>
        txtObservaciones.Text.Trim();

    /// <summary>
    /// Obtiene la fecha y hora indicada para la decisión.
    /// </summary>
    public DateTime FechaDecision =>
        dtpFechaDecision.Value;

    #endregion

    #region Inicialización

    public FrmDecisionPresupuesto()
    {
        InitializeComponent();
    }

    private void FrmDecisionPresupuesto_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarDecisiones();
        CargarDatosSimulados();

        _hayCambios = false;
        _formularioInicializado = true;
    }

    private void FrmDecisionPresupuesto_Shown(
        object? sender,
        EventArgs e)
    {
        cboDecision.Focus();
    }

    private void ConfigurarFormulario()
    {
        Text = "Registrar decisión del presupuesto";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        AcceptButton = btnRegistrar;
        CancelButton = btnCancelar;

        cboDecision.DropDownStyle =
            ComboBoxStyle.DropDownList;

        dtpFechaDecision.Format =
            DateTimePickerFormat.Custom;

        dtpFechaDecision.CustomFormat =
            "dd/MM/yyyy HH:mm";

        txtObservaciones.MaxLength =
            LongitudMaximaObservaciones;
    }

    private void CargarDecisiones()
    {
        cboDecision.BeginUpdate();

        try
        {
            cboDecision.Items.Clear();

            cboDecision.Items.AddRange(
            [
                "Aceptado",
                "Rechazado"
            ]);

            cboDecision.SelectedIndex = -1;
        }
        finally
        {
            cboDecision.EndUpdate();
        }
    }

    #endregion

    #region Datos simulados

    private void CargarDatosSimulados()
    {
        lblNumeroPresupuesto.Text =
            "Presupuesto N.º 000001";

        lblAtencionValor.Text =
            "000001";

        lblClienteValor.Text =
            "Juan Pérez";

        lblVehiculoValor.Text =
            "Honda Fit · AB345CD";

        lblTotalValor.Text =
            "$ 43.500,00";

        cboDecision.SelectedIndex = -1;
        txtObservaciones.Clear();
        dtpFechaDecision.Value = DateTime.Now;
    }

    #endregion

    #region Eventos

    private void CampoEditable_Cambio(
        object? sender,
        EventArgs e)
    {
        if (!_formularioInicializado)
        {
            return;
        }

        _hayCambios = true;

        if (sender is Control control)
        {
            control.BackColor = Color.White;
        }
    }

    private void BtnRegistrar_Click(
        object? sender,
        EventArgs e)
    {
        if (!ValidarFormulario())
        {
            return;
        }

        ConfirmarDecision();
    }

    private void BtnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    private void FrmDecisionPresupuesto_FormClosing(
        object? sender,
        FormClosingEventArgs e)
    {
        if (_decisionConfirmada || !_hayCambios)
        {
            NormalizarResultadoCancelacion();
            return;
        }

        DialogResult respuesta =
            MessageBox.Show(
                this,
                "Hay una decisión ingresada que no fue registrada. " +
                "¿Desea cerrar igualmente?",
                "Registrar decisión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

        if (respuesta == DialogResult.No)
        {
            e.Cancel = true;
            DialogResult = DialogResult.None;
            return;
        }

        NormalizarResultadoCancelacion();
    }

    #endregion

    #region Validación y confirmación

    private bool ValidarFormulario()
    {
        cboDecision.BackColor = Color.White;

        if (cboDecision.SelectedIndex < 0)
        {
            cboDecision.BackColor =
                Color.FromArgb(254, 242, 242);

            MessageBox.Show(
                this,
                "Seleccioná la decisión comunicada por el cliente.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            cboDecision.Focus();
            return false;
        }

        return true;
    }

    private void ConfirmarDecision()
    {
        DialogResult respuesta =
            MessageBox.Show(
                this,
                $"¿Confirma que el presupuesto fue {Decision.ToLowerInvariant()}?",
                "Confirmar decisión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes)
        {
            return;
        }

        txtObservaciones.Text =
            txtObservaciones.Text.Trim();

        MessageBox.Show(
            this,
            "La decisión del presupuesto fue registrada correctamente.",
            "Decisión registrada",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        _decisionConfirmada = true;
        DialogResult = DialogResult.OK;
    }

    private void NormalizarResultadoCancelacion()
    {
        if (!_decisionConfirmada &&
            DialogResult == DialogResult.None)
        {
            DialogResult = DialogResult.Cancel;
        }
    }

    #endregion

    #region Estilos

    private void AplicarEstilos()
    {
        BackColor =
            EstilosInterfaz.FondoAplicacion;

        EstilosInterfaz.AplicarBotonPrimario(
            btnRegistrar);

        EstilosInterfaz.AplicarBotonSecundario(
            btnCancelar);
    }

    #endregion
}
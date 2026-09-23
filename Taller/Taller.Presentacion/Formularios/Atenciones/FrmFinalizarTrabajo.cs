namespace Taller.Presentacion.Formularios.Atenciones;

/// <summary>
/// Representa visualmente la finalización del trabajo técnico
/// correspondiente a una atención.
///
/// Durante la primera entrega utiliza información simulada
/// y no modifica la base de datos.
/// </summary>
public partial class FrmFinalizarTrabajo : Form
{
    #region Constantes y campos

    private const int LongitudMinimaTrabajoRealizado = 10;
    private const int LongitudMaximaTexto = 1000;

    private bool _trabajoFinalizado;

    #endregion

    #region Propiedades de resultado

    /// <summary>
    /// Obtiene la fecha seleccionada para la finalización.
    /// </summary>
    public DateTime FechaFinalizacion =>
        dtpFechaFinalizacion.Value;

    /// <summary>
    /// Obtiene el resultado seleccionado.
    /// </summary>
    public string Resultado =>
        cboResultadoTrabajo.SelectedItem?.ToString()
        ?? string.Empty;

    /// <summary>
    /// Obtiene la descripción del trabajo realizado.
    /// </summary>
    public string TrabajoRealizado =>
        txtTrabajoRealizado.Text.Trim();

    /// <summary>
    /// Obtiene las observaciones técnicas ingresadas.
    /// </summary>
    public string Observaciones =>
        txtObservaciones.Text.Trim();

    /// <summary>
    /// Indica si se confirmó una prueba de funcionamiento.
    /// </summary>
    public bool PruebaFuncionamientoRealizada =>
        chkPruebaFuncionamiento.Checked;

    #endregion

    #region Inicialización

    public FrmFinalizarTrabajo()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Configura el formulario y carga la información simulada.
    /// </summary>
    private void FrmFinalizarTrabajo_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarResultados();
        CargarDatosSimulados();
    }

    /// <summary>
    /// Establece el comportamiento general de la ventana
    /// y sus controles.
    /// </summary>
    private void ConfigurarFormulario()
    {
        Text = "Finalizar trabajo";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        AcceptButton = btnFinalizarTrabajo;
        CancelButton = btnCancelar;

        dtpFechaFinalizacion.Format =
            DateTimePickerFormat.Custom;

        dtpFechaFinalizacion.CustomFormat =
            "dd/MM/yyyy HH:mm";

        dtpFechaFinalizacion.Value =
            DateTime.Now;

        cboResultadoTrabajo.DropDownStyle =
            ComboBoxStyle.DropDownList;

        txtTrabajoRealizado.MaxLength =
            LongitudMaximaTexto;

        txtObservaciones.MaxLength =
            LongitudMaximaTexto;
    }

    /// <summary>
    /// Carga las alternativas disponibles para el resultado
    /// del trabajo técnico.
    /// </summary>
    private void CargarResultados()
    {
        cboResultadoTrabajo.Items.Clear();

        cboResultadoTrabajo.Items.AddRange(
        [
            "Trabajo completado",
            "Completado con observaciones",
            "Requiere trabajo adicional",
            "No fue posible completar el trabajo"
        ]);

        cboResultadoTrabajo.SelectedIndex = -1;
    }

    /// <summary>
    /// Carga información demostrativa de la atención
    /// y los trabajos previamente autorizados.
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

        lblTecnicoValor.Text =
            "Técnico actual";

        lblDiagnosticoValor.Text =
            "Reparación requerida en el sistema de arranque.";

        lblPresupuestoValor.Text =
            "Autorizado · Total: $ 127.700,00";

        dgvTrabajosAutorizados.Rows.Clear();

        dgvTrabajosAutorizados.Rows.Add(
            "SER-002",
            "Servicio",
            "Reemplazo de motor de arranque",
            1,
            "Autorizado");

        dgvTrabajosAutorizados.Rows.Add(
            "REP-001",
            "Repuesto",
            "Motor de arranque",
            1,
            "Autorizado");

        dgvTrabajosAutorizados.Rows.Add(
            "INS-001",
            "Insumo",
            "Limpiador de contactos",
            1,
            "Autorizado");

        txtTrabajoRealizado.Clear();
        txtObservaciones.Clear();

        chkPruebaFuncionamiento.Checked = false;
        chkConfirmarFinalizacion.Checked = false;

        dgvTrabajosAutorizados.ClearSelection();
    }

    #endregion

    #region Eventos

    /// <summary>
    /// Posiciona el foco en el primer campo editable.
    /// </summary>
    private void FrmFinalizarTrabajo_Shown(
        object? sender,
        EventArgs e)
    {
        txtTrabajoRealizado.Focus();
    }

    /// <summary>
    /// Valida y confirma la finalización simulada.
    /// </summary>
    private void BtnFinalizarTrabajo_Click(
        object? sender,
        EventArgs e)
    {
        if (!ValidarFormulario())
        {
            return;
        }

        ConfirmarFinalizacionSimulada();
    }

    /// <summary>
    /// Cancela la operación actual.
    /// </summary>
    private void BtnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    /// <summary>
    /// Restablece el indicador visual cuando cambia el resultado.
    /// </summary>
    private void ResultadoTrabajo_Cambio(
        object? sender,
        EventArgs e)
    {
        cboResultadoTrabajo.BackColor =
            Color.White;
    }

    /// <summary>
    /// Restablece el indicador visual del campo modificado.
    /// </summary>
    private void CampoValidado_Cambio(
        object? sender,
        EventArgs e)
    {
        if (sender is Control control)
        {
            control.BackColor = Color.White;
        }
    }

    /// <summary>
    /// Advierte al usuario cuando intenta cerrar con información
    /// ingresada que todavía no fue confirmada.
    /// </summary>
    private void FrmFinalizarTrabajo_FormClosing(
        object? sender,
        FormClosingEventArgs e)
    {
        if (_trabajoFinalizado ||
            !HayDatosIngresados())
        {
            return;
        }

        DialogResult respuesta = MessageBox.Show(
            this,
            "Hay información del trabajo que no fue confirmada. " +
            "¿Desea cerrar igualmente?",
            "Cancelar finalización",
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
    /// Comprueba que estén completos los datos necesarios
    /// para finalizar el trabajo.
    /// </summary>
    private bool ValidarFormulario()
    {
        RestablecerIndicadoresValidacion();

        string trabajoRealizado =
            txtTrabajoRealizado.Text.Trim();

        if (string.IsNullOrWhiteSpace(
            trabajoRealizado))
        {
            MostrarAdvertencia(
                "Describí el trabajo técnico realizado.",
                txtTrabajoRealizado);

            return false;
        }

        if (trabajoRealizado.Length <
            LongitudMinimaTrabajoRealizado)
        {
            MostrarAdvertencia(
                $"La descripción debe contener al menos " +
                $"{LongitudMinimaTrabajoRealizado} caracteres.",
                txtTrabajoRealizado);

            return false;
        }

        if (cboResultadoTrabajo.SelectedIndex < 0)
        {
            MostrarAdvertencia(
                "Seleccioná el resultado del trabajo.",
                cboResultadoTrabajo);

            return false;
        }

        bool resultadoCompletado =
            Resultado == "Trabajo completado"
            || Resultado ==
                "Completado con observaciones";

        if (resultadoCompletado &&
            !chkPruebaFuncionamiento.Checked)
        {
            MessageBox.Show(
                this,
                "Para indicar que el trabajo fue completado, " +
                "debe confirmarse la prueba de funcionamiento.",
                "Prueba requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            chkPruebaFuncionamiento.Focus();
            return false;
        }

        if (!chkConfirmarFinalizacion.Checked)
        {
            MessageBox.Show(
                this,
                "Debe confirmar la finalización del trabajo técnico.",
                "Confirmación requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            chkConfirmarFinalizacion.Focus();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determina si existe información que podría perderse
    /// al cerrar el formulario.
    /// </summary>
    private bool HayDatosIngresados()
    {
        return !string.IsNullOrWhiteSpace(
                txtTrabajoRealizado.Text)
            || !string.IsNullOrWhiteSpace(
                txtObservaciones.Text)
            || cboResultadoTrabajo.SelectedIndex >= 0
            || chkPruebaFuncionamiento.Checked
            || chkConfirmarFinalizacion.Checked;
    }

    /// <summary>
    /// Restablece los colores utilizados durante la validación.
    /// </summary>
    private void RestablecerIndicadoresValidacion()
    {
        txtTrabajoRealizado.BackColor =
            Color.White;

        cboResultadoTrabajo.BackColor =
            Color.White;
    }

    /// <summary>
    /// Muestra una advertencia y devuelve el foco
    /// al control indicado.
    /// </summary>
    private void MostrarAdvertencia(
        string mensaje,
        Control control)
    {
        control.BackColor =
            Color.FromArgb(254, 242, 242);

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
    /// Simula la finalización exitosa del trabajo técnico.
    /// </summary>
    private void ConfirmarFinalizacionSimulada()
    {
        txtTrabajoRealizado.Text =
            txtTrabajoRealizado.Text.Trim();

        txtObservaciones.Text =
            txtObservaciones.Text.Trim();

        DialogResult respuesta = MessageBox.Show(
            this,
            $"El trabajo se registrará con el resultado " +
            $"\"{Resultado}\". ¿Desea continuar?",
            "Finalizar trabajo",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes)
        {
            return;
        }

        _trabajoFinalizado = true;

        MessageBox.Show(
            this,
            "El trabajo técnico fue finalizado correctamente.",
            "Trabajo finalizado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
    }

    #endregion

    #region Estilos

    /// <summary>
    /// Aplica los estilos visuales del sistema.
    /// </summary>
    private void AplicarEstilos()
    {
        ConfigurarBotonPrimario(
            btnFinalizarTrabajo);

        ConfigurarBotonSecundario(
            btnCancelar);

        ConfigurarGrilla(
            dgvTrabajosAutorizados);
    }

    private static void ConfigurarBotonPrimario(
        Button boton)
    {
        boton.BackColor =
            Color.FromArgb(30, 64, 175);

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

        boton.ForeColor =
            Color.FromArgb(51, 65, 85);

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

    private static void ConfigurarGrilla(
        DataGridView grilla)
    {
        grilla.EnableHeadersVisualStyles = false;
        grilla.BackgroundColor = Color.White;

        grilla.GridColor =
            Color.FromArgb(226, 232, 240);

        grilla.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(241, 245, 249);

        grilla.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.FromArgb(51, 65, 85);

        grilla.ColumnHeadersDefaultCellStyle.Font =
            new Font(
                "Segoe UI",
                9F,
                FontStyle.Bold);

        grilla.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(219, 234, 254);

        grilla.DefaultCellStyle.SelectionForeColor =
            Color.FromArgb(30, 64, 175);

        grilla.DefaultCellStyle.Padding =
            new Padding(4);

        grilla.RowTemplate.Height = 32;
        grilla.ScrollBars = ScrollBars.Both;
    }

    #endregion
}

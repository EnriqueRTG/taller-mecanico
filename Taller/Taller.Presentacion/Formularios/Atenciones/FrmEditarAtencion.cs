using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Atenciones;

/// <summary>
/// Representa la edición visual de los datos administrativos
/// permitidos de una atención.
/// </summary>
/// <remarks>
/// Durante la primera entrega utiliza información simulada y no
/// realiza modificaciones sobre la base de datos.
/// </remarks>
public partial class FrmEditarAtencion : Form
{
    #region Constantes y campos

    private const int LongitudMinimaMotivo = 10;
    private const int LongitudMaximaMotivo = 500;
    private const int LongitudMaximaObservaciones = 1000;

    private bool _formularioInicializado;
    private bool _hayCambios;
    private bool _cambiosConfirmados;

    #endregion

    #region Propiedades públicas

    /// <summary>
    /// Obtiene el motivo de consulta ingresado.
    /// </summary>
    public string Motivo =>
        txtMotivo.Text.Trim();

    /// <summary>
    /// Obtiene las observaciones de recepción ingresadas.
    /// </summary>
    public string Observaciones =>
        txtObservaciones.Text.Trim();

    /// <summary>
    /// Obtiene la prioridad seleccionada.
    /// </summary>
    public string Prioridad =>
        cboPrioridad.SelectedItem?.ToString()
        ?? string.Empty;

    #endregion

    #region Inicialización

    /// <summary>
    /// Inicializa una nueva instancia del formulario.
    /// </summary>
    public FrmEditarAtencion()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Configura el formulario y carga los datos demostrativos.
    /// </summary>
    private void FrmEditarAtencion_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarPrioridades();
        CargarDatosSimulados();

        _hayCambios = false;
        _formularioInicializado = true;
    }

    /// <summary>
    /// Coloca el foco inicial en el motivo de consulta.
    /// </summary>
    private void FrmEditarAtencion_Shown(
        object? sender,
        EventArgs e)
    {
        txtMotivo.Focus();
        txtMotivo.SelectionStart =
            txtMotivo.TextLength;
    }

    /// <summary>
    /// Configura el comportamiento general de la ventana.
    /// </summary>
    private void ConfigurarFormulario()
    {
        Text = "Editar atención";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;

        txtMotivo.MaxLength =
            LongitudMaximaMotivo;

        txtObservaciones.MaxLength =
            LongitudMaximaObservaciones;

        cboPrioridad.DropDownStyle =
            ComboBoxStyle.DropDownList;
    }

    /// <summary>
    /// Carga las prioridades disponibles para la simulación.
    /// </summary>
    private void CargarPrioridades()
    {
        cboPrioridad.BeginUpdate();

        try
        {
            cboPrioridad.Items.Clear();

            cboPrioridad.Items.AddRange(
            [
                "Baja",
                "Normal",
                "Alta",
                "Urgente"
            ]);
        }
        finally
        {
            cboPrioridad.EndUpdate();
        }
    }

    #endregion

    #region Datos simulados

    /// <summary>
    /// Carga información demostrativa de la atención seleccionada.
    /// </summary>
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

        lblFechaIngresoValor.Text =
            "07/09/2026 16:07";

        txtMotivo.Text =
            "Dificultades para arrancar.";

        txtObservaciones.Text =
            "El inconveniente ocurre principalmente durante el primer arranque del día.";

        cboPrioridad.SelectedItem =
            "Normal";
    }

    #endregion

    #region Eventos

    /// <summary>
    /// Marca que uno de los campos editables fue modificado.
    /// </summary>
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

    /// <summary>
    /// Valida y confirma visualmente los cambios ingresados.
    /// </summary>
    private void BtnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        if (!ValidarFormulario())
        {
            return;
        }

        SimularModificacionExitosa();
    }

    /// <summary>
    /// Solicita cerrar el formulario sin confirmar modificaciones.
    /// </summary>
    private void BtnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Advierte al usuario cuando intenta cerrar la ventana
    /// después de modificar datos sin guardarlos.
    /// </summary>
    private void FrmEditarAtencion_FormClosing(
        object? sender,
        FormClosingEventArgs e)
    {
        if (_cambiosConfirmados || !_hayCambios)
        {
            NormalizarResultadoCancelacion();
            return;
        }

        DialogResult respuesta =
            MessageBox.Show(
                this,
                "Hay cambios que no fueron guardados. " +
                "¿Desea cerrar igualmente?",
                "Editar atención",
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

    #region Validación

    /// <summary>
    /// Comprueba que los campos obligatorios contengan
    /// información válida para la simulación.
    /// </summary>
    private bool ValidarFormulario()
    {
        RestablecerIndicadoresValidacion();

        string motivo =
            txtMotivo.Text.Trim();

        if (string.IsNullOrWhiteSpace(motivo))
        {
            MostrarAdvertencia(
                "Ingresá el motivo de consulta.",
                txtMotivo);

            return false;
        }

        if (motivo.Length < LongitudMinimaMotivo)
        {
            MostrarAdvertencia(
                $"El motivo debe contener al menos " +
                $"{LongitudMinimaMotivo} caracteres.",
                txtMotivo);

            return false;
        }

        if (cboPrioridad.SelectedIndex < 0)
        {
            MostrarAdvertencia(
                "Seleccioná la prioridad de la atención.",
                cboPrioridad);

            return false;
        }

        return true;
    }

    /// <summary>
    /// Restablece la apariencia de los campos validados.
    /// </summary>
    private void RestablecerIndicadoresValidacion()
    {
        txtMotivo.BackColor = Color.White;
        cboPrioridad.BackColor = Color.White;
    }

    /// <summary>
    /// Muestra una advertencia y coloca el foco en el control indicado.
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

    #region Simulación y cierre

    /// <summary>
    /// Simula la modificación exitosa y devuelve un resultado afirmativo.
    /// </summary>
    private void SimularModificacionExitosa()
    {
        txtMotivo.Text =
            txtMotivo.Text.Trim();

        txtObservaciones.Text =
            txtObservaciones.Text.Trim();

        MessageBox.Show(
            this,
            "Los datos de la atención fueron modificados correctamente.",
            "Editar atención",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        _cambiosConfirmados = true;
        DialogResult = DialogResult.OK;
    }

    /// <summary>
    /// Devuelve Cancel cuando el formulario se cierra
    /// sin confirmar una modificación.
    /// </summary>
    private void NormalizarResultadoCancelacion()
    {
        if (!_cambiosConfirmados &&
            DialogResult == DialogResult.None)
        {
            DialogResult = DialogResult.Cancel;
        }
    }

    #endregion

    #region Estilos

    /// <summary>
    /// Aplica los estilos visuales compartidos por la aplicación.
    /// </summary>
    private void AplicarEstilos()
    {
        BackColor =
            EstilosInterfaz.FondoAplicacion;

        EstilosInterfaz.AplicarBotonPrimario(
            btnGuardar);

        EstilosInterfaz.AplicarBotonSecundario(
            btnCancelar);
    }

    #endregion
}
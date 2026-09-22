namespace Taller.Presentacion.Formularios.Diagnosticos;

/// <summary>
/// Representa la pantalla de registro de un diagnóstico técnico.
/// </summary>
/// <remarks>
/// En la primera entrega utiliza datos simulados y no realiza
/// operaciones sobre la base de datos. Cuando finaliza correctamente,
/// expone la información ingresada y devuelve
/// <see cref="DialogResult.OK"/> al formulario que la abrió.
/// </remarks>
public partial class FrmRegistrarDiagnostico : Form
{
    #region Campos y propiedades

    private const int LongitudMinimaDiagnostico = 10;
    private const int LongitudMaximaTexto = 1000;

    private bool _registroConfirmado;

    /// <summary>
    /// Obtiene la descripción ingresada para el diagnóstico simulado.
    /// </summary>
    public string Descripcion =>
        txtDescripcion.Text.Trim();

    /// <summary>
    /// Obtiene las observaciones técnicas ingresadas.
    /// </summary>
    public string Observaciones =>
        txtObservaciones.Text.Trim();

    /// <summary>
    /// Obtiene el resultado seleccionado para el diagnóstico.
    /// </summary>
    public string Resultado =>
        cboResultado.SelectedItem?.ToString() ?? string.Empty;

    /// <summary>
    /// Obtiene el nombre del técnico mostrado en el formulario.
    /// </summary>
    public string Tecnico =>
        txtTecnico.Text.Trim();

    /// <summary>
    /// Obtiene la fecha y hora seleccionadas para el diagnóstico.
    /// </summary>
    public DateTime FechaDiagnostico =>
        dtpFechaDiagnostico.Value;

    #endregion

    #region Inicialización
    /// <summary>
    /// Inicializa una nueva instancia del formulario de registro
    /// de diagnóstico.
    /// </summary>
    public FrmRegistrarDiagnostico()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Inicializa la configuración, los estilos y los datos simulados
    /// cuando el formulario termina de cargarse.
    /// </summary>
    /// <param name="sender">
    /// Instancia que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento de carga.
    /// </param>
    private void FrmRegistrarDiagnostico_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarResultados();
        CargarDatosSimulados();
    }

    /// <summary>
    /// Configura el comportamiento general del formulario y sus controles,
    /// incluyendo botones predeterminados, límites de texto, selector de
    /// resultado y formato de fecha.
    /// </summary>
    private void ConfigurarFormulario()
    {
        Text = "Registrar diagnóstico";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;

        cboResultado.DropDownStyle = ComboBoxStyle.DropDownList;

        dtpFechaDiagnostico.Format = DateTimePickerFormat.Custom;
        dtpFechaDiagnostico.CustomFormat = "dd/MM/yyyy HH:mm";
        dtpFechaDiagnostico.Value = DateTime.Now;

        txtDescripcion.MaxLength = LongitudMaximaTexto;
        txtObservaciones.MaxLength = LongitudMaximaTexto;
    }

    /// <summary>
    /// Carga en el selector los posibles resultados simulados
    /// de un diagnóstico técnico.
    /// </summary>
    private void CargarResultados()
    {
        cboResultado.BeginUpdate();

        try
        {
            cboResultado.Items.Clear();

            cboResultado.Items.AddRange(
            [
                "Reparación requerida",
                "Mantenimiento recomendado",
                "Sin fallas detectadas",
                "Requiere evaluación adicional"
            ]);

            cboResultado.SelectedIndex = -1;
        }
        finally
        {
            cboResultado.EndUpdate();
        }
    }
    #endregion

    #region Datos simulados
    /// <summary>
    /// Carga la información simulada de la atención, el cliente,
    /// el vehículo y el técnico responsable.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza exclusivamente para la navegación y
    /// presentación visual de la primera entrega.
    /// </remarks>
    private void CargarDatosSimulados()
    {
        lblNumeroAtencion.Text = "Atención N.º 000001";
        lblClienteValor.Text = "Juan Pérez";
        lblVehiculoValor.Text = "Honda Fit";
        lblDominioValor.Text = "AB345CD";
        lblMotivoValor.Text = "Dificultades para arrancar";

        txtTecnico.Text = "Técnico actual";

        txtDescripcion.Clear();
        txtObservaciones.Clear();
        cboResultado.SelectedIndex = -1;
    }

    /// <summary>
    /// Simula el registro exitoso del diagnóstico, normaliza los textos
    /// ingresados y finaliza el formulario con un resultado satisfactorio.
    /// </summary>
    private void SimularRegistroExitoso()
    {
        txtDescripcion.Text = txtDescripcion.Text.Trim();
        txtObservaciones.Text = txtObservaciones.Text.Trim();

        MessageBox.Show(
            this,
            "El diagnóstico fue registrado correctamente.",
            "Diagnóstico",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        _registroConfirmado = true;
        DialogResult = DialogResult.OK;
    }
    #endregion

    #region Eventos
    /// <summary>
    /// Posiciona el foco inicial en el campo de descripción cuando
    /// el formulario ya se encuentra visible.
    /// </summary>
    /// <param name="sender">
    /// Instancia que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento.
    /// </param>
    private void FrmRegistrarDiagnostico_Shown(
        object? sender,
        EventArgs e)
    {
        txtDescripcion.Focus();
    }

    /// <summary>
    /// Valida los datos ingresados y, si son correctos, ejecuta
    /// el registro simulado del diagnóstico.
    /// </summary>
    /// <param name="sender">
    /// Botón que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento.
    /// </param>
    private void BtnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        if (!ValidarFormulario())
        {
            return;
        }

        SimularRegistroExitoso();
    }

    /// <summary>
    /// Solicita la cancelación del registro y devuelve un resultado
    /// de cancelación al formulario que abrió esta ventana.
    /// </summary>
    /// <param name="sender">
    /// Botón que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento.
    /// </param>
    private void BtnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    /// <summary>
    /// Restablece el color visual de un campo cuando el usuario modifica
    /// su contenido después de una validación fallida.
    /// </summary>
    /// <param name="sender">
    /// Control cuyo contenido o selección fue modificado.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento.
    /// </param>
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
    /// Controla el cierre del formulario y solicita confirmación cuando
    /// existen datos ingresados que todavía no fueron registrados.
    /// </summary>
    /// <param name="sender">
    /// Formulario que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos que permiten continuar o cancelar el cierre.
    /// </param>
    private void FrmRegistrarDiagnostico_FormClosing(
        object? sender,
        FormClosingEventArgs e)
    {
        if (_registroConfirmado)
        {
            return;
        }

        if (HayDatosIngresados())
        {
            DialogResult respuesta = MessageBox.Show(
                this,
                "Hay datos ingresados que no fueron guardados. ¿Desea cerrar igualmente?",
                "Cancelar diagnóstico",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (respuesta == DialogResult.No)
            {
                e.Cancel = true;
                DialogResult = DialogResult.None;
                return;
            }
        }

        if (DialogResult == DialogResult.None)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
    #endregion

    #region Validación
    /// <summary>
    /// Verifica que la descripción cumpla la longitud mínima y que se
    /// haya seleccionado un resultado para el diagnóstico.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> si los datos requeridos son válidos;
    /// en caso contrario, <see langword="false"/>.
    /// </returns>
    private bool ValidarFormulario()
    {
        RestablecerIndicadoresValidacion();

        string descripcion = txtDescripcion.Text.Trim();

        if (string.IsNullOrWhiteSpace(descripcion))
        {
            MostrarAdvertencia(
                "Ingresá una descripción del diagnóstico.",
                txtDescripcion);

            return false;
        }

        if (descripcion.Length < LongitudMinimaDiagnostico)
        {
            MostrarAdvertencia(
                $"La descripción debe contener al menos " +
                $"{LongitudMinimaDiagnostico} caracteres.",
                txtDescripcion);

            return false;
        }

        if (cboResultado.SelectedIndex < 0)
        {
            MostrarAdvertencia(
                "Seleccioná un resultado para el diagnóstico.",
                cboResultado);

            return false;
        }

        return true;
    }

    /// <summary>
    /// Determina si el usuario ingresó información que podría perderse
    /// al cerrar el formulario.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> si existe una descripción, una observación
    /// o un resultado seleccionado; en caso contrario,
    /// <see langword="false"/>.
    /// </returns>
    private bool HayDatosIngresados()
    {
        return !string.IsNullOrWhiteSpace(txtDescripcion.Text)
            || !string.IsNullOrWhiteSpace(txtObservaciones.Text)
            || cboResultado.SelectedIndex >= 0;
    }

    /// <summary>
    /// Restablece la apariencia normal de los controles que participan
    /// en la validación.
    /// </summary>
    private void RestablecerIndicadoresValidacion()
    {
        txtDescripcion.BackColor = Color.White;
        cboResultado.BackColor = Color.White;
    }

    /// <summary>
    /// Muestra una advertencia de validación, resalta el control
    /// relacionado y devuelve el foco a dicho control.
    /// </summary>
    /// <param name="mensaje">
    /// Mensaje que explica el dato faltante o inválido.
    /// </param>
    /// <param name="control">
    /// Control que debe resaltarse y recibir el foco.
    /// </param>
    private void MostrarAdvertencia(
        string mensaje,
        Control control)
    {
        control.BackColor = Color.FromArgb(254, 242, 242);

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
    /// Aplica los estilos visuales correspondientes a los botones
    /// y campos principales del formulario.
    /// </summary>
    private void AplicarEstilos()
    {
        ConfigurarBotonPrimario(btnGuardar);
        ConfigurarBotonSecundario(btnCancelar);

        ConfigurarCampoSoloLectura(txtTecnico);
        ConfigurarCampoEditable(txtDescripcion);
        ConfigurarCampoEditable(txtObservaciones);
    }

    /// <summary>
    /// Aplica el estilo visual definido para una acción principal.
    /// </summary>
    /// <param name="boton">
    /// Botón al que se aplicará el estilo.
    /// </param>
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

    /// <summary>
    /// Aplica el estilo visual definido para una acción secundaria.
    /// </summary>
    /// <param name="boton">
    /// Botón al que se aplicará el estilo.
    /// </param>
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

    /// <summary>
    /// Configura un campo de texto para mostrar información
    /// que el usuario no puede modificar.
    /// </summary>
    /// <param name="campo">
    /// Campo de texto que se configurará como solo lectura.
    /// </param>
    private static void ConfigurarCampoSoloLectura(
        TextBox campo)
    {
        campo.ReadOnly = true;
        campo.BackColor = Color.FromArgb(248, 250, 252);
        campo.ForeColor = Color.FromArgb(51, 65, 85);
        campo.BorderStyle = BorderStyle.FixedSingle;
        campo.TabStop = false;
    }

    /// <summary>
    /// Aplica la apariencia estándar de los campos de texto editables.
    /// </summary>
    /// <param name="campo">
    /// Campo de texto editable al que se aplicará el estilo.
    /// </param>
    private static void ConfigurarCampoEditable(
        TextBox campo)
    {
        campo.ReadOnly = false;
        campo.BackColor = Color.White;
        campo.ForeColor = Color.FromArgb(51, 65, 85);
        campo.BorderStyle = BorderStyle.FixedSingle;
    }
    #endregion
}
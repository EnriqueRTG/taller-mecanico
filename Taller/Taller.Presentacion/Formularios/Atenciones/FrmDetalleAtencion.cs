using Taller.Dominio.Constantes;
using Taller.Presentacion.Formularios.Diagnosticos;
using Taller.Presentacion.Formularios.Vehiculos;
using Taller.Presentacion.Formularios.Presupuestos;
using Taller.Presentacion.Formularios.Comprobantes;

namespace Taller.Presentacion.Formularios.Atenciones;

/// <summary>
/// Representa la pantalla de consulta detallada de una atención
/// y sus diferentes etapas dentro del proceso del taller.
/// </summary>
/// <remarks>
/// Durante la primera entrega utiliza datos simulados, configura las
/// acciones disponibles según el rol y permite navegar hacia otras
/// pantallas del sistema.
/// </remarks>
public partial class FrmDetalleAtencion : Form
{
    #region Campos
    /// <summary>
    /// Identificador del rol utilizado para configurar las acciones
    /// disponibles en el formulario.
    /// </summary>
    private readonly int _rolId;

    /// <summary>
    /// Indica si durante la sesión actual se registró un diagnóstico simulado.
    /// </summary>
    private bool _diagnosticoRegistrado;

    /// <summary>
    /// Indica si durante la sesión actual se registró
    /// una decisión sobre el presupuesto simulado.
    /// </summary>
    private bool _decisionPresupuestoRegistrada;

    private bool _presupuestoGenerado;
    #endregion

    #region Inicialización
    /// <summary>
    /// Inicializa una nueva instancia del detalle de atención para
    /// el rol especificado.
    /// </summary>
    /// <param name="rolId">
    /// Identificador del rol del usuario que accede al formulario.
    /// </param>
    public FrmDetalleAtencion(
        int rolId)
    {
        InitializeComponent();

        _rolId = rolId;
    }

    /// <summary>
    /// Inicializa la configuración, los estilos, los datos simulados
    /// y los permisos visuales del formulario.
    /// </summary>
    /// <param name="sender">
    /// Instancia que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento de carga.
    /// </param>
    private void FrmDetalleAtencion_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        CargarDatosSimulados();
        ConfigurarVistaSegunRol();
        ConfigurarBotonPredeterminado();
    }

    /// <summary>
    /// Configura el título y los controles generales del formulario.
    /// </summary>
    private void ConfigurarFormulario()
    {
        Text = "Detalle de atención";
        CancelButton = btnCerrar;
    }

    /// <summary>
    /// Establece la acción principal como botón predeterminado cuando
    /// se encuentra visible y habilitada.
    /// </summary>
    private void ConfigurarBotonPredeterminado()
    {
        AcceptButton = btnAccionPrincipal.Visible &&
                       btnAccionPrincipal.Enabled
            ? btnAccionPrincipal
            : null;
    }
    #endregion

    #region Datos simulados
    /// <summary>
    /// Carga el estado visual simulado de las diferentes etapas
    /// de la atención.
    /// </summary>
    private void CargarDatosSimulados()
    {
        CargarSeguimientoSimulado();
        CargarDiagnosticoSimulado();
        CargarPresupuestoSimulado();
        CargarPagosSimulados();

        tabDetalle.SelectedTab = tabInformacion;
    }

    /// <summary>
    /// Carga los movimientos iniciales simulados en el historial
    /// de seguimiento de la atención.
    /// </summary>
    private void CargarSeguimientoSimulado()
    {
        dgvSeguimiento.Rows.Clear();

        dgvSeguimiento.Rows.Add(
            "07/09/2026 16:07",
            "Ingreso",
            "Se registró el ingreso del vehículo.",
            "Administrativo Recepción");

        dgvSeguimiento.Rows.Add(
            "07/09/2026 16:10",
            "Ingreso",
            "La atención quedó abierta para evaluación técnica.",
            "Administrativo Recepción");

        dgvSeguimiento.ClearSelection();
    }

    /// <summary>
    /// Configura la pestaña de diagnóstico con su estado inicial pendiente.
    /// </summary>
    private void CargarDiagnosticoSimulado()
    {
        _diagnosticoRegistrado = false;

        lblEstadoDiagnosticoValor.Text = "Pendiente";
        lblEstadoDiagnosticoValor.ForeColor =
            Color.FromArgb(180, 83, 9);

        lblFechaDiagnosticoValor.Text = "Sin registrar";
        lblTecnicoDiagnosticoValor.Text = "Sin asignar";
        lblResultadoDiagnosticoValor.Text = "No disponible";

        txtDescripcionDiagnostico.Text =
            "Todavía no se registró un diagnóstico para esta atención.";

        txtDescripcionDiagnostico.ForeColor =
            Color.FromArgb(100, 116, 139);

        txtDescripcionDiagnostico.Font =
            new Font("Segoe UI", 9F, FontStyle.Italic);

        txtObservacionesDiagnostico.Text =
            "Las observaciones técnicas estarán disponibles cuando el técnico complete la evaluación del vehículo.";

        txtObservacionesDiagnostico.ForeColor =
            Color.FromArgb(100, 116, 139);
    }

    /// <summary>
    /// Configura el presupuesto demostrativo y su estado inicial.
    /// </summary>
    private void CargarPresupuestoSimulado()
    {
        _decisionPresupuestoRegistrada = false;

        dgvDetallePresupuesto.Rows.Clear();

        dgvDetallePresupuesto.Rows.Add(
            "Revisión del sistema de encendido",
            "Servicio",
            "1",
            "$ 25.000,00",
            "$ 25.000,00");

        dgvDetallePresupuesto.Rows.Add(
            "Juego de bujías",
            "Repuesto",
            "1",
            "$ 18.500,00",
            "$ 18.500,00");

        lblEstadoPresupuestoValor.Text =
            "Emitido";

        lblEstadoPresupuestoValor.ForeColor =
            Color.FromArgb(30, 64, 175);

        lblFechaPresupuestoValor.Text =
            DateTime.Today.AddDays(-2)
                .ToString("dd/MM/yyyy");

        lblDecisionValor.Text =
            "Sin registrar";

        lblDecisionValor.ForeColor =
            Color.FromArgb(51, 65, 85);

        lblVigenciaValor.Text =
            DateTime.Today.AddDays(13)
                .ToString("dd/MM/yyyy");

        lblTotalPresupuesto.Text =
            "Total estimado: $ 43.500,00";

        dgvDetallePresupuesto.ClearSelection();
    }

    /// <summary>
    /// Configura el estado inicial simulado de los pagos de la atención.
    /// </summary>
    private void CargarPagosSimulados()
    {
        dgvPagos.Rows.Clear();

        lblResumenPago.Text = "Total abonado: $ 0,00";
        lblSaldoValor.Text = "$ 0,00";

        dgvPagos.ClearSelection();
    }

    /// <summary>
    /// Refleja visualmente el presupuesto generado en el
    /// detalle de la atención.
    /// </summary>
    private void AplicarPresupuestoGenerado(
        FrmGenerarPresupuesto formulario)
    {
        _presupuestoGenerado = true;

        dgvDetallePresupuesto.Rows.Clear();

        foreach (
            FrmGenerarPresupuesto.ConceptoPresupuestoResultado
            concepto in formulario.Conceptos)
        {
            dgvDetallePresupuesto.Rows.Add(
                concepto.Nombre,
                concepto.Categoria,
                concepto.Cantidad,
                FormatearImporte(
                    concepto.PrecioUnitario),
                FormatearImporte(
                    concepto.Subtotal));
        }

        lblEstadoPresupuestoValor.Text =
            "Emitido";

        lblFechaPresupuestoValor.Text =
            formulario.FechaEmision.ToString(
                "dd/MM/yyyy HH:mm");

        lblDecisionValor.Text =
            "Pendiente";

        lblVigenciaValor.Text =
            $"{formulario.VigenciaDias} días";

        lblTotalPresupuesto.Text =
            $"Total estimado: " +
            $"{FormatearImporte(
                formulario.TotalPresupuesto)}";

        lblEtapaPresupuesto.Text =
            "PRESUPUESTO GENERADO";

        pnlEtapaPresupuesto.BackColor =
            Color.FromArgb(219, 234, 254);

        btnAbrirPresupuesto.Enabled = false;

        dgvSeguimiento.Rows.Add(
            formulario.FechaEmision.ToString(
                "dd/MM/yyyy HH:mm"),
            "Presupuesto",
            "Se generó el presupuesto técnico de la atención.",
            "Técnico actual");

        dgvDetallePresupuesto.ClearSelection();
        dgvSeguimiento.ClearSelection();
    }
    private static string FormatearImporte(
    decimal importe)
    {
        return importe.ToString(
            "C2",
            System.Globalization.CultureInfo
                .GetCultureInfo("es-AR"));
    }

    #endregion

    #region Configuración por rol
    /// <summary>
    /// Determina la configuración visual correspondiente al rol del usuario.
    /// </summary>
    private void ConfigurarVistaSegunRol()
    {
        switch (_rolId)
        {
            case RolesSistema.AdministrativoId:
                ConfigurarVistaAdministrativa();
                break;

            case RolesSistema.TecnicoId:
                ConfigurarVistaTecnica();
                break;

            case RolesSistema.AdministradorId:
            default:
                ConfigurarVistaSoloConsulta();
                break;
        }
    }

    /// <summary>
    /// Habilita las acciones administrativas relacionadas con la atención,
    /// el presupuesto, los comprobantes, los pagos y la entrega.
    /// </summary>
    private void ConfigurarVistaAdministrativa()
    {
        lblInformacionRol.Text =
            "Consulte el avance y gestione los datos administrativos de la atención.";

        // Acción administrativa general del detalle.
        btnAccionPrincipal.Visible = true;
        btnAccionPrincipal.Enabled = true;
        btnAccionPrincipal.Text = "Editar datos";

        // El historial se consulta desde la pestaña Diagnóstico.
        btnVerHistorial.Visible = false;
        btnVerHistorialDesdeDiagnostico.Visible = true;

        // El diagnóstico corresponde al Técnico.
        btnRegistrarDiagnostico.Visible = false;

        // El presupuesto técnico corresponde al Técnico.
        btnAbrirPresupuesto.Visible = false;

        // Decisión administrativa del presupuesto.
        btnRegistrarDecision.Visible = true;
        btnRegistrarDecision.Enabled =
            !_decisionPresupuestoRegistrada;

        // Operaciones administrativas posteriores.
        btnGestionarComprobante.Visible = true;
        btnGestionarComprobante.Enabled = true;

        btnRegistrarEntrega.Visible = true;
        btnRegistrarEntrega.Enabled = true;
    }

    /// <summary>
    /// Habilita las acciones técnicas relacionadas con el diagnóstico,
    /// el presupuesto técnico y el historial del vehículo.
    /// </summary>
    private void ConfigurarVistaTecnica()
    {
        lblInformacionRol.Text =
            "Consulte los antecedentes y acceda al diagnóstico y trabajo técnico.";

        // El pie del detalle conserva solamente la acción Cerrar.
        // Las operaciones técnicas se encuentran en sus pestañas.
        btnAccionPrincipal.Visible = false;
        btnVerHistorial.Visible = false;

        // Acciones específicas de la pestaña Diagnóstico.
        btnRegistrarDiagnostico.Visible = true;
        btnRegistrarDiagnostico.Enabled =
            !_diagnosticoRegistrado;

        btnVerHistorialDesdeDiagnostico.Visible = true;

        // Acción específica de la pestaña Presupuesto.
        btnAbrirPresupuesto.Visible = true;
        btnAbrirPresupuesto.Enabled =
            !_presupuestoGenerado;

        // Acciones exclusivas del Administrativo.
        btnRegistrarDecision.Visible = false;
        btnGestionarComprobante.Visible = false;
        btnRegistrarEntrega.Visible = false;
    }

    /// <summary>
    /// Configura una vista segura de solo consulta para roles que no
    /// tienen autorización para gestionar la atención.
    /// </summary>
    private void ConfigurarVistaSoloConsulta()
    {
        lblInformacionRol.Text =
            "Vista de consulta de la atención y sus etapas asociadas.";

        btnAccionPrincipal.Visible = false;
        btnVerHistorial.Visible = false;

        btnRegistrarDiagnostico.Visible = false;
        btnAbrirPresupuesto.Visible = false;
        btnRegistrarDecision.Visible = false;
        btnGestionarComprobante.Visible = false;
        btnRegistrarEntrega.Visible = false;

        btnVerHistorialDesdeDiagnostico.Visible = true;
    }
    #endregion

    #region Eventos
    /// <summary>
    /// Cierra la pantalla de detalle de atención.
    /// </summary>
    /// <param name="sender">
    /// Botón que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento.
    /// </param>
    private void BtnCerrar_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Ejecuta la acción principal correspondiente al rol del usuario.
    /// </summary>
    /// <param name="sender">
    /// Botón que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento.
    /// </param>
    private void BtnAccionPrincipal_Click(
        object? sender,
        EventArgs e)
    {
        switch (_rolId)
        {
            case RolesSistema.TecnicoId:
                AbrirRegistroDiagnostico();
                break;

            case RolesSistema.AdministrativoId:
                AbrirEdicionAtencion();
                break;
        }
    }

    /// <summary>
    /// Abre el historial del vehículo o informa que la pantalla
    /// todavía no está disponible.
    /// </summary>
    private void BtnVerHistorial_Click(
        object? sender,
        EventArgs e)
    {
        AbrirHistorialVehiculo();
    }

    /// <summary>
    /// Abre el formulario de registro de diagnóstico para el usuario técnico.
    /// </summary>
    /// <param name="sender">
    /// Botón que originó el evento.
    /// </param>
    /// <param name="e">
    /// Datos asociados al evento.
    /// </param>
    private void BtnRegistrarDiagnostico_Click(
        object? sender,
        EventArgs e)
    {
        AbrirRegistroDiagnostico();
    }

    /// <summary>
    /// Abre el formulario de generación del presupuesto técnico
    /// para la atención mostrada.
    /// </summary>
    private void BtnAbrirPresupuesto_Click(
        object? sender,
        EventArgs e)
    {
        using FrmGenerarPresupuesto formulario =
            new();

        if (formulario.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        AplicarPresupuestoGenerado(formulario);
    }

    /// <summary>
    /// Abre la pantalla administrativa para registrar la decisión
    /// del cliente sobre el presupuesto.
    /// </summary>
    private void BtnRegistrarDecision_Click(
        object? sender,
        EventArgs e)
    {
        AbrirDecisionPresupuesto();
    }

    /// <summary>
    /// Abre el formulario de registro de entrega para la
    /// atención mostrada actualmente.
    /// </summary>
    private void BtnRegistrarEntrega_Click(
        object? sender,
        EventArgs e)
    {
        using FrmRegistrarEntrega formulario =
            new();

        if (formulario.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        AplicarEntregaSimulada(formulario);
    }

    /// <summary>
    /// Abre la pantalla de gestión del comprobante y sus pagos
    /// correspondientes a la atención seleccionada.
    /// </summary>
    private void BtnGestionarComprobante_Click(
        object? sender,
        EventArgs e)
    {
        using FrmComprobantePagos formulario =
            new();

        formulario.ShowDialog(this);
    }
    #endregion

    #region Navegación
    /// <summary>
    /// Verifica el rol y abre de manera modal el formulario de registro
    /// de diagnóstico.
    /// </summary>
    /// <remarks>
    /// Cuando el formulario devuelve <see cref="DialogResult.OK"/>,
    /// actualiza visualmente el detalle con los datos ingresados.
    /// </remarks>
    private void AbrirRegistroDiagnostico()
    {
        if (_rolId != RolesSistema.TecnicoId)
        {
            MessageBox.Show(
                this,
                "Solamente el personal técnico puede registrar diagnósticos.",
                "Acceso restringido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        if (_diagnosticoRegistrado)
        {
            MessageBox.Show(
                this,
                "La atención ya posee un diagnóstico registrado durante esta sesión.",
                "Diagnóstico registrado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            tabDetalle.SelectedTab = tabDiagnostico;
            return;
        }

        using var formulario = new FrmRegistrarDiagnostico();

        DialogResult resultado = formulario.ShowDialog(this);

        if (resultado != DialogResult.OK)
        {
            return;
        }

        AplicarDiagnosticoSimulado(formulario);
    }

    /// <summary>
    /// Informa que una pantalla de destino todavía no fue incorporada
    /// a la navegación simulada.
    /// </summary>
    /// <param name="nombrePantalla">
    /// Nombre descriptivo de la pantalla pendiente.
    /// </param>
    private void MostrarPantallaPendiente(
        string nombrePantalla)
    {
        MessageBox.Show(
            this,
            $"La pantalla \"{nombrePantalla}\" se incorporará en el siguiente paso de navegación.",
            "Navegación",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    /// <summary>
    /// Abre de manera modal el formulario demostrativo
    /// de edición administrativa de la atención.
    /// </summary>
    private void AbrirEdicionAtencion()
    {
        if (_rolId != RolesSistema.AdministrativoId)
        {
            MessageBox.Show(
                this,
                "Solamente el personal administrativo puede editar los datos de la atención.",
                "Acceso restringido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        using var formulario =
            new FrmEditarAtencion();

        DialogResult resultado =
            formulario.ShowDialog(this);

        if (resultado != DialogResult.OK)
        {
            return;
        }

        // En esta entrega la modificación es solamente visual.
        // La actualización del detalle se incorporará junto con
        // la lógica de negocio definitiva.
    }

    /// <summary>
    /// Abre de manera modal la pantalla demostrativa
    /// del historial de atenciones del vehículo.
    /// </summary>
    private void AbrirHistorialVehiculo()
    {
        using var formulario =
            new FrmHistorialVehiculo();

        formulario.ShowDialog(this);
    }

    /// <summary>
    /// Verifica el rol y abre de manera modal el formulario
    /// demostrativo de decisión del presupuesto.
    /// </summary>
    private void AbrirDecisionPresupuesto()
    {
        if (_rolId != RolesSistema.AdministrativoId)
        {
            MessageBox.Show(
                this,
                "Solamente el personal administrativo puede registrar " +
                "la decisión del cliente.",
                "Acceso restringido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        if (_decisionPresupuestoRegistrada)
        {
            MessageBox.Show(
                this,
                "La decisión del presupuesto ya fue registrada " +
                "durante esta sesión.",
                "Decisión registrada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            tabDetalle.SelectedTab =
                tabPresupuesto;

            return;
        }

        using var formulario =
            new FrmDecisionPresupuesto();

        DialogResult resultado =
            formulario.ShowDialog(this);

        if (resultado != DialogResult.OK)
        {
            return;
        }

        AplicarDecisionPresupuestoSimulada(
            formulario);
    }
    #endregion

    #region Actualización visual
    /// <summary>
    /// Copia los datos del diagnóstico simulado al detalle de la atención
    /// y actualiza el estado visual del proceso.
    /// </summary>
    /// <param name="formulario">
    /// Formulario que contiene los datos del diagnóstico registrado.
    /// </param>
    private void AplicarDiagnosticoSimulado(
        FrmRegistrarDiagnostico formulario)
    {
        _diagnosticoRegistrado = true;

        lblEstadoDiagnosticoValor.Text = "Registrado";
        lblEstadoDiagnosticoValor.ForeColor =
            Color.FromArgb(22, 101, 52);

        lblFechaDiagnosticoValor.Text =
            formulario.FechaDiagnostico.ToString("dd/MM/yyyy HH:mm");

        lblTecnicoDiagnosticoValor.Text =
            formulario.Tecnico;

        lblResultadoDiagnosticoValor.Text =
            formulario.Resultado;

        txtDescripcionDiagnostico.Text =
            formulario.Descripcion;

        txtDescripcionDiagnostico.ForeColor =
            Color.FromArgb(51, 65, 85);

        txtDescripcionDiagnostico.Font =
            new Font("Segoe UI", 9F, FontStyle.Regular);

        txtObservacionesDiagnostico.Text =
            string.IsNullOrWhiteSpace(formulario.Observaciones)
                ? "Sin observaciones técnicas."
                : formulario.Observaciones;

        txtObservacionesDiagnostico.ForeColor =
            Color.FromArgb(51, 65, 85);

        ActualizarEtapaDiagnostico();
        AgregarMovimientoDiagnostico(formulario);
        DeshabilitarRegistroDiagnostico();

        tabDetalle.SelectedTab = tabDiagnostico;
    }

    /// <summary>
    /// Marca visualmente la etapa de diagnóstico como completada.
    /// </summary>
    private void ActualizarEtapaDiagnostico()
    {
        pnlEtapaDiagnostico.BackColor =
            Color.FromArgb(30, 64, 175);

        lblEtapaDiagnostico.Text =
            "✓ DIAGNÓSTICO\nRegistrado";

        lblEtapaDiagnostico.ForeColor = Color.White;
    }

    /// <summary>
    /// Agrega al seguimiento un movimiento correspondiente al diagnóstico
    /// registrado durante la sesión.
    /// </summary>
    /// <param name="formulario">
    /// Formulario que proporciona la fecha, el resultado y el técnico.
    /// </param>
    private void AgregarMovimientoDiagnostico(
        FrmRegistrarDiagnostico formulario)
    {
        dgvSeguimiento.Rows.Add(
            formulario.FechaDiagnostico.ToString("dd/MM/yyyy HH:mm"),
            "Diagnóstico",
            $"Diagnóstico registrado: {formulario.Resultado}.",
            formulario.Tecnico);

        dgvSeguimiento.ClearSelection();
    }

    /// <summary>
    /// Deshabilita las acciones que permitirían registrar nuevamente
    /// el diagnóstico durante la misma sesión.
    /// </summary>
    private void DeshabilitarRegistroDiagnostico()
    {
        btnRegistrarDiagnostico.Enabled = false;
        btnRegistrarDiagnostico.Text = "Diagnóstico registrado";

        AcceptButton = null;
    }

    /// <summary>
    /// Aplica al detalle de atención la decisión simulada
    /// registrada sobre el presupuesto.
    /// </summary>
    /// <param name="formulario">
    /// Formulario que contiene la decisión ingresada.
    /// </param>
    private void AplicarDecisionPresupuestoSimulada(
        FrmDecisionPresupuesto formulario)
    {
        _decisionPresupuestoRegistrada = true;

        bool presupuestoAceptado =
            formulario.Decision == "Aceptado";

        Color colorEstado =
            presupuestoAceptado
                ? Color.FromArgb(22, 101, 52)
                : Color.FromArgb(185, 28, 28);

        lblDecisionValor.Text =
            formulario.Decision;

        lblDecisionValor.ForeColor =
            colorEstado;

        lblEstadoPresupuestoValor.Text =
            presupuestoAceptado
                ? "Aceptado"
                : "Rechazado";

        lblEstadoPresupuestoValor.ForeColor =
            colorEstado;

        ActualizarEtapaPresupuesto(
            formulario.Decision,
            colorEstado);

        AgregarMovimientoDecisionPresupuesto(
            formulario);

        DeshabilitarRegistroDecision();

        tabDetalle.SelectedTab =
            tabPresupuesto;
    }

    /// <summary>
    /// Actualiza visualmente la etapa de presupuesto según
    /// la decisión demostrativa registrada.
    /// </summary>
    private void ActualizarEtapaPresupuesto(
        string decision,
        Color colorEstado)
    {
        pnlEtapaPresupuesto.BackColor =
            colorEstado;

        lblEtapaPresupuesto.Text =
            $"✓ PRESUPUESTO\n{decision}";

        lblEtapaPresupuesto.ForeColor =
            Color.White;
    }

    /// <summary>
    /// Agrega al seguimiento un movimiento demostrativo
    /// asociado con la decisión del presupuesto.
    /// </summary>
    private void AgregarMovimientoDecisionPresupuesto(
        FrmDecisionPresupuesto formulario)
    {
        string descripcion =
            $"El cliente indicó que el presupuesto fue " +
            $"{formulario.Decision.ToLowerInvariant()}.";

        if (!string.IsNullOrWhiteSpace(
            formulario.Observaciones))
        {
            descripcion +=
                $" Observaciones: {formulario.Observaciones}";
        }

        dgvSeguimiento.Rows.Add(
            formulario.FechaDecision
                .ToString("dd/MM/yyyy HH:mm"),
            "Presupuesto",
            descripcion,
            "Administrativo Recepción");

        dgvSeguimiento.ClearSelection();
    }

    /// <summary>
    /// Evita registrar más de una decisión sobre el presupuesto
    /// durante la misma sesión demostrativa.
    /// </summary>
    private void DeshabilitarRegistroDecision()
    {
        btnRegistrarDecision.Enabled = false;
        btnRegistrarDecision.Text =
            "Decisión registrada";
    }
    #endregion

    #region Estilos
    /// <summary>
    /// Aplica los estilos visuales generales a botones y grillas.
    /// </summary>
    private void AplicarEstilos()
    {
        ConfigurarBotonPrimario(btnAccionPrincipal);
        ConfigurarBotonPrimario(btnRegistrarDiagnostico);
        ConfigurarBotonPrimario(btnAbrirPresupuesto);
        ConfigurarBotonPrimario(btnGestionarComprobante);
        ConfigurarBotonPrimario(btnRegistrarEntrega);

        ConfigurarBotonSecundario(btnVerHistorial);
        ConfigurarBotonSecundario(btnCerrar);
        ConfigurarBotonSecundario(btnVerHistorialDesdeDiagnostico);
        ConfigurarBotonSecundario(btnRegistrarDecision);

        ConfigurarGrilla(dgvSeguimiento);
        ConfigurarGrilla(dgvDetallePresupuesto);
        ConfigurarGrilla(dgvPagos);
    }

    /// <summary>
    /// Aplica el estilo correspondiente a una acción principal.
    /// </summary>
    /// <param name="boton">Botón que recibirá el estilo.</param>
    private static void ConfigurarBotonPrimario(
        Button boton)
    {
        boton.BackColor = Color.FromArgb(30, 64, 175);
        boton.ForeColor = Color.White;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.Cursor = Cursors.Hand;
        boton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    /// <summary>
    /// Aplica el estilo correspondiente a una acción secundaria.
    /// </summary>
    /// <param name="boton">Botón que recibirá el estilo.</param>
    private static void ConfigurarBotonSecundario(
        Button boton)
    {
        boton.BackColor = Color.White;
        boton.ForeColor = Color.FromArgb(51, 65, 85);
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        boton.FlatAppearance.BorderSize = 1;
        boton.Cursor = Cursors.Hand;
        boton.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
    }

    /// <summary>
    /// Aplica la apariencia estándar de las grillas de consulta.
    /// </summary>
    /// <param name="grilla">Grilla que recibirá el estilo.</param>
    private static void ConfigurarGrilla(
        DataGridView grilla)
    {
        grilla.EnableHeadersVisualStyles = false;
        grilla.BackgroundColor = Color.White;
        grilla.BorderStyle = BorderStyle.None;
        grilla.GridColor = Color.FromArgb(226, 232, 240);

        grilla.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(241, 245, 249);

        grilla.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.FromArgb(51, 65, 85);

        grilla.ColumnHeadersDefaultCellStyle.Font =
            new Font("Segoe UI", 9F, FontStyle.Bold);

        grilla.DefaultCellStyle.BackColor = Color.White;
        grilla.DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
        grilla.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(219, 234, 254);

        grilla.DefaultCellStyle.SelectionForeColor =
            Color.FromArgb(30, 64, 175);

        grilla.DefaultCellStyle.Font =
            new Font("Segoe UI", 9F, FontStyle.Regular);

        grilla.DefaultCellStyle.Padding = new Padding(4);
        grilla.RowTemplate.Height = 32;
    }
    #endregion

    #region Datos Simulados
    /// <summary>
    /// Actualiza visualmente el detalle de la atención después
    /// de confirmar una entrega simulada.
    /// </summary>
    private void AplicarEntregaSimulada(
        FrmRegistrarEntrega formulario)
    {
        lblEstadoEntregaValor.Text =
            "Entregada";

        lblFechaEntregaValor.Text =
            formulario.FechaEntrega.ToString(
                "dd/MM/yyyy HH:mm");

        lblEntregadoPorValor.Text =
            formulario.RecibidoPor;

        lblEstadoEntregaValor.ForeColor =
            Color.FromArgb(22, 101, 52);

        btnRegistrarEntrega.Enabled = false;

        dgvSeguimiento.Rows.Add(
            formulario.FechaEntrega.ToString(
                "dd/MM/yyyy HH:mm"),
            "Entrega",
            $"El vehículo fue entregado a " +
            $"{formulario.RecibidoPor}.",
            "Administrativo actual");

        dgvSeguimiento.ClearSelection();
    }
    #endregion
    
}
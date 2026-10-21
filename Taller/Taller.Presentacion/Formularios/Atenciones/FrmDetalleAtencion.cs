using Taller.Dominio.Constantes;
using Taller.Presentacion.Formularios.Diagnosticos;
using Taller.Presentacion.Formularios.Vehiculos;
using Taller.Presentacion.Formularios.Presupuestos;
using Taller.Presentacion.Formularios.Comprobantes;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Excepciones;

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

    private bool _guardandoIngreso;

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

    private readonly int _idAtencion;

    private readonly AtencionServicio _atencionServicio;

    private readonly List<ServicioIngresoFila> _serviciosIngreso = [];

    private Atencion? _atencionActual;

    private bool _cargandoServiciosIngreso;
    #endregion

    #region Inicialización
    /// <summary>
    /// Inicializa el detalle de la atención seleccionada.
    /// </summary>
    public FrmDetalleAtencion(
        int idAtencion,
        AtencionServicio atencionServicio,
        SesionUsuario sesionUsuario)
    {
        ArgumentNullException.ThrowIfNull(atencionServicio);
        ArgumentNullException.ThrowIfNull(sesionUsuario);

        if (idAtencion <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idAtencion));
        }

        var usuario = sesionUsuario.UsuarioActual
            ?? throw new InvalidOperationException(
                "No existe una sesión de usuario válida.");

        InitializeComponent();

        _idAtencion = idAtencion;
        _atencionServicio = atencionServicio;
        _rolId = usuario.RolId;
    }

    /// <summary>
    /// Representa un servicio preparado para mostrarse en la grilla.
    /// No es una entidad persistida.
    /// </summary>
    private sealed class ServicioIngresoFila
    {
        public int IdConcepto { get; init; }

        public string Nombre { get; init; } = string.Empty;

        public decimal Cantidad { get; init; }

        public decimal PrecioUnitario { get; init; }

        public decimal Subtotal => decimal.Round(
            Cantidad * PrecioUnitario,
            2,
            MidpointRounding.AwayFromZero);
    }

    private async void FrmDetalleAtencion_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();

        // No mostrar registros demostrativos como si fueran reales.
        dgvSeguimiento.Rows.Clear();
        dgvDetallePresupuesto.Rows.Clear();
        dgvPagos.Rows.Clear();

        tabDetalle.SelectedTab = tabInformacion;

        // Estas operaciones todavía no están conectadas en el detalle.
        btnAccionPrincipal.Visible = false;
        btnVerHistorial.Visible = false;
        btnRegistrarDiagnostico.Visible = false;
        btnVerHistorialDesdeDiagnostico.Visible = false;
        btnAbrirPresupuesto.Visible = false;
        btnRegistrarDecision.Visible = false;
        btnGestionarComprobante.Visible = false;
        btnRegistrarEntrega.Visible = false;
        AcceptButton = null;

        // Dejamos disponibles las pestañas que conectaremos primero.
        // Las restantes conservan su diseño para próximos pasos.
        tabDetalle.TabPages.Remove(tabSeguimiento);
        tabDetalle.TabPages.Remove(tabDiagnostico);
        tabDetalle.TabPages.Remove(tabPresupuesto);
        tabDetalle.TabPages.Remove(tabEntrega);

        grpIngresoDiagnostico.Visible =
            _rolId == RolesSistema.AdministrativoId;

        btnConfirmarIngreso.Enabled = false;
        cboServicioIngreso.Enabled = false;
        nudImporteIngreso.Enabled = false;
        chkAutorizaIngreso.Enabled = false;

        try
        {
            Cursor = Cursors.WaitCursor;
            tabDetalle.Enabled = false;

            await CargarAtencionAsync();

            await CargarServiciosIngresoAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                $"No se pudo cargar la atención.\n\n{ex.Message}",
                "Detalle de atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            Close();
        }
        finally
        {
            Cursor = Cursors.Default;
            tabDetalle.Enabled = true;
        }
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

    /// <summary>
    /// Consulta y muestra los datos reales de la atención.
    /// </summary>
    private async Task CargarAtencionAsync()
    {
        var atencion = await _atencionServicio.ObtenerPorIdAsync(
            _idAtencion);

        if (atencion is null)
        {
            throw new InvalidOperationException(
                "La atención seleccionada ya no existe.");
        }

        _atencionActual = atencion;

        string numero = atencion.IdAtencion.ToString("D6");
        string fecha = atencion.FechaApertura.ToString(
            "dd/MM/yyyy HH:mm");

        string recepcion =
            $"{atencion.UsuarioRecepcion.Nombre} " +
            $"{atencion.UsuarioRecepcion.Apellido}";

        lblTitulo.Text = $"Atención N.º {numero}";
        lblEstado.Text =
            $"Estado: {ObtenerNombreEstado(atencion.Estado)}";

        lblEtapaActual.Text = atencion.IdTecnicoEnActividad is null
            ? "Sin actividad técnica reservada"
            : $"Técnico en actividad: " +
              $"{atencion.TecnicoEnActividad?.Nombre} " +
              $"{atencion.TecnicoEnActividad?.Apellido}";

        lblNombreCliente.Text =
            $"{atencion.Cliente.Apellido}, {atencion.Cliente.Nombre}";

        lblDocumentoCliente.Text =
            $"{atencion.Cliente.TipoDocumento} " +
            $"{atencion.Cliente.Documento}";

        var contactos = new[]
        {
        atencion.Cliente.Telefono,
        atencion.Cliente.Email
    };

        lblContactoCliente.Text = string.Join(
            " · ",
            contactos.Where(c => !string.IsNullOrWhiteSpace(c)));

        if (string.IsNullOrWhiteSpace(lblContactoCliente.Text))
        {
            lblContactoCliente.Text = "Sin contacto registrado";
        }

        lblDominioVehiculo.Text = atencion.Vehiculo.Dominio;

        lblModeloVehiculo.Text =
            $"{atencion.Vehiculo.Modelo.Marca.Nombre} " +
            $"{atencion.Vehiculo.Modelo.Nombre}";

        lblDatosVehiculo.Text =
            $"Año {atencion.Vehiculo.Anio} · {atencion.Vehiculo.Color}";

        lblFechaIngreso.Text = fecha;
        lblUsuarioRecepcion.Text = recepcion;

        lblTituloRecepcion.Text = "REGISTRO DE ATENCIÓN";
        lblTipoIngreso.Text =
            "La apertura no implica autorización de reparación.";

        lblNumeroValor.Text = numero;
        lblIngresoValor.Text = fecha;
        lblRecepcionValor.Text = recepcion;

        txtMotivoConsulta.Text = atencion.MotivoConsulta;
        txtMotivoConsulta.ReadOnly = true;

        lblInformacionRol.Text =
            _rolId == RolesSistema.AdministrativoId
                ? "Recepción: consulte los datos y confirme el ingreso autorizado."
                : "Consulta de los datos de la atención.";

        lblInformacionIngreso.Text =
            atencion.Estado == EstadoAtencion.Abierta
                ? "El cargo inicial se registra al confirmar el ingreso, " +
                  "aunque posteriormente se rechace la reparación."
                : "Esta atención ya no está en la etapa de confirmación de ingreso.";
    }

    /// <summary>
    /// Carga los servicios disponibles para confirmar el ingreso.
    /// </summary>
    private async Task CargarServiciosIngresoAsync()
    {
        if (_rolId != RolesSistema.AdministrativoId
            || _atencionActual?.Estado != EstadoAtencion.Abierta)
        {
            ActualizarControlesIngreso();
            return;
        }

        _cargandoServiciosIngreso = true;
        ActualizarControlesIngreso();

        try
        {
            var servicios = await _atencionServicio
                .ListarServiciosDisponiblesParaIngresoAsync();

            cboServicioIngreso.DisplayMember =
                nameof(ServicioIngresoDto.Nombre);

            cboServicioIngreso.ValueMember =
                nameof(ServicioIngresoDto.IdConcepto);

            cboServicioIngreso.DataSource = servicios;
            cboServicioIngreso.SelectedIndex = -1;

            nudCantidadIngreso.Value = 1m;
            nudImporteIngreso.Value = 0m;
            chkAutorizaIngreso.Checked = false;

            lblInformacionIngreso.Text = servicios.Count == 0
                ? "No hay servicios habilitados para ingreso."
                : "Agregue los servicios y revise el total antes de autorizar.";

            ActualizarGrillaServiciosIngreso();
        }
        finally
        {
            _cargandoServiciosIngreso = false;

            if (!IsDisposed)
            {
                ActualizarControlesIngreso();
            }
        }
    }

    /// <summary>
    /// Completa el importe de referencia al seleccionar un servicio.
    /// </summary>
    private void CboServicioIngreso_SelectedIndexChanged(
        object? sender,
        EventArgs e)
    {
        if (_cargandoServiciosIngreso)
        {
            return;
        }

        chkAutorizaIngreso.Checked = false;
        nudCantidadIngreso.Value = 1m;

        var servicio = cboServicioIngreso.SelectedItem as ServicioIngresoDto;

        decimal? referencia = servicio?.PrecioReferencia;

        bool referenciaUtilizable = referencia.HasValue
            && referencia.Value >= nudImporteIngreso.Minimum
            && referencia.Value <= nudImporteIngreso.Maximum;

        nudImporteIngreso.Value = referenciaUtilizable
            ? referencia!.Value
            : 0m;

        lblInformacionIngreso.Text = servicio is null
            ? "Seleccione un servicio."
            : referenciaUtilizable
                ? "El precio sugerido puede modificarse antes de agregar."
                : "Indique el precio unitario. El cero requiere confirmar que es sin cargo.";

        ActualizarControlesIngreso();

        if (nudImporteIngreso.Enabled)
        {
            nudImporteIngreso.Focus();
            nudImporteIngreso.Select(0, nudImporteIngreso.Text.Length);
        }
    }

    private void NudImporteIngreso_ValueChanged(
        object? sender,
        EventArgs e)
    {
        if (_cargandoServiciosIngreso)
        {
            return;
        }

        chkAutorizaIngreso.Checked = false;
        ActualizarControlesIngreso();
    }

    private void NudImporteIngreso_KeyPress(
        object? sender,
        KeyPressEventArgs e)
    {
        if (!_cargandoServiciosIngreso
            && (!char.IsControl(e.KeyChar) || e.KeyChar == '\b'))
        {
            chkAutorizaIngreso.Checked = false;
        }
    }

    private void ChkAutorizaIngreso_CheckedChanged(
        object? sender,
        EventArgs e)
    {
        ActualizarControlesIngreso();
    }

    /// <summary>
    /// Habilita los controles según los datos y la etapa actual.
    /// </summary>
    private void ActualizarControlesIngreso()
    {
        bool puedeIngresar =
            !_guardandoIngreso
            && !_cargandoServiciosIngreso
            && _rolId == RolesSistema.AdministrativoId
            && _atencionActual is
            {
                Estado: EstadoAtencion.Abierta,
                FechaCierre: null,
                IdTecnicoEnActividad: null,
                ActividadTecnica: null
            };

        bool seleccionado =
            cboServicioIngreso.SelectedItem is ServicioIngresoDto;

        bool hayRenglones = _serviciosIngreso.Count > 0;

        cboServicioIngreso.Enabled =
            puedeIngresar && cboServicioIngreso.Items.Count > 0;

        nudCantidadIngreso.Enabled = puedeIngresar && seleccionado;
        nudImporteIngreso.Enabled = puedeIngresar && seleccionado;

        btnAgregarServicioIngreso.Enabled =
            puedeIngresar && seleccionado;

        dgvServiciosIngreso.Enabled = puedeIngresar;

        btnQuitarServicioIngreso.Enabled =
            puedeIngresar
            && dgvServiciosIngreso.CurrentRow?.DataBoundItem
                is ServicioIngresoFila;

        chkAutorizaIngreso.Enabled = puedeIngresar && hayRenglones;

        btnConfirmarIngreso.Enabled =
            puedeIngresar
            && hayRenglones
            && chkAutorizaIngreso.Checked;

        btnCerrar.Enabled = !_guardandoIngreso;
    }

    private static string ObtenerNombreEstado(
        EstadoAtencion estado)
    {
        return estado switch
        {
            EstadoAtencion.Abierta => "Abierta",
            EstadoAtencion.PendienteDiagnostico => "Pendiente de diagnóstico",
            EstadoAtencion.EnEvaluacionTecnica => "En evaluación técnica",
            EstadoAtencion.PendienteDecision => "Pendiente de decisión",
            EstadoAtencion.EnEjecucion => "En ejecución",
            EstadoAtencion.Pausada => "Pausada",
            EstadoAtencion.TrabajoFinalizado => "Trabajo finalizado",
            EstadoAtencion.PendienteLiquidacion => "Pendiente de liquidación",
            EstadoAtencion.Cerrada => "Cerrada",
            EstadoAtencion.Cancelada => "Cancelada",
            _ => "Estado desconocido"
        };
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

    private void BtnAgregarServicioIngreso_Click(
       object? sender,
       EventArgs e)
    {
        ActualizarControlesIngreso();

        if (!btnAgregarServicioIngreso.Enabled || !ValidateChildren())
        {
            return;
        }

        if (cboServicioIngreso.SelectedItem is not ServicioIngresoDto servicio)
        {
            return;
        }

        if (_serviciosIngreso.Any(
                fila => fila.IdConcepto == servicio.IdConcepto))
        {
            MessageBox.Show(
                this,
                "Ese servicio ya está agregado. Para cambiar sus condiciones, " +
                "quítelo y agréguelo nuevamente.",
                "Servicios de ingreso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        decimal cantidad = nudCantidadIngreso.Value;
        decimal precio = nudImporteIngreso.Value;

        const decimal importeMaximo = 9999999999999999.99m;

        if (cantidad <= 0m
            || precio < 0m
            || precio > importeMaximo / cantidad)
        {
            MessageBox.Show(
                this,
                "Revise la cantidad y el precio del servicio.",
                "Servicios de ingreso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        var filaNueva = new ServicioIngresoFila
        {
            IdConcepto = servicio.IdConcepto,
            Nombre = servicio.Nombre,
            Cantidad = cantidad,
            PrecioUnitario = precio
        };

        decimal totalActual = _serviciosIngreso.Sum(fila => fila.Subtotal);

        if (filaNueva.Subtotal > importeMaximo - totalActual)
        {
            MessageBox.Show(
                this,
                "El total supera el importe permitido.",
                "Servicios de ingreso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        if (precio == 0m)
        {
            var respuesta = MessageBox.Show(
                this,
                $"¿Confirma que «{servicio.Nombre}» se ofrece sin cargo?",
                "Servicio sin cargo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
            {
                nudImporteIngreso.Focus();
                return;
            }
        }

        _serviciosIngreso.Add(filaNueva);

        chkAutorizaIngreso.Checked = false;
        cboServicioIngreso.SelectedIndex = -1;

        ActualizarGrillaServiciosIngreso();

        lblInformacionIngreso.Text =
            "Servicio agregado. Revise los renglones y el total.";

        cboServicioIngreso.Focus();
    }

    private void BtnQuitarServicioIngreso_Click(
        object? sender,
        EventArgs e)
    {
        ActualizarControlesIngreso();

        if (!btnQuitarServicioIngreso.Enabled)
        {
            return;
        }

        if (dgvServiciosIngreso.CurrentRow?.DataBoundItem
            is not ServicioIngresoFila fila)
        {
            return;
        }

        _serviciosIngreso.Remove(fila);
        chkAutorizaIngreso.Checked = false;

        ActualizarGrillaServiciosIngreso();
    }

    /// <summary>
    /// Registra el ingreso autorizado y su cargo inicial.
    /// </summary>
    private async void BtnConfirmarIngreso_Click(
        object? sender,
        EventArgs e)
    {
        if (_guardandoIngreso)
        {
            return;
        }

        ActualizarControlesIngreso();

        if (!btnConfirmarIngreso.Enabled)
        {
            return;
        }

        var datos = new ConfirmarIngresoDto
        {
            IdAtencion = _idAtencion,
            ClienteAutoriza = chkAutorizaIngreso.Checked,
            Servicios = _serviciosIngreso
                .Select(fila => new ServicioIngresoSeleccionadoDto
                {
                    IdConcepto = fila.IdConcepto,
                    Cantidad = fila.Cantidad,
                    PrecioUnitario = fila.PrecioUnitario
                })
                .ToList()
        };

        bool ingresoRegistrado = false;
        bool cerrarDetalle = false;

        try
        {
            _guardandoIngreso = true;
            ActualizarControlesIngreso();
            Cursor = Cursors.WaitCursor;

            await _atencionServicio.ConfirmarIngresoAsync(datos);

            ingresoRegistrado = true;

            await CargarAtencionAsync();

            chkAutorizaIngreso.Checked = false;

            lblInformacionIngreso.Text =
                "Servicios autorizados, pendientes de ejecución.";

            MessageBox.Show(
                this,
                "El ingreso y los servicios autorizados fueron registrados.\n\n" +
                "La atención quedó pendiente de diagnóstico. " +
                "Los servicios todavía no se consideran ejecutados ni pagados.",
                "Ingreso confirmado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (ValidacionException ex)
        {
            chkAutorizaIngreso.Checked = false;

            MessageBox.Show(
                this,
                ex.Message,
                "Revisar ingreso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            cerrarDetalle = true;

            string mensaje = ingresoRegistrado
                ? "El ingreso fue registrado, pero no se pudo actualizar " +
                  "la pantalla. Actualice el listado."
                : "No se pudo confirmar el resultado de la operación. " +
                  "Actualice el listado y consulte el estado antes de reintentar.";

            MessageBox.Show(
                this,
                $"{mensaje}\n\n{ex.Message}",
                "Confirmar ingreso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        finally
        {
            _guardandoIngreso = false;
            Cursor = Cursors.Default;

            if (!IsDisposed)
            {
                ActualizarControlesIngreso();
            }
        }

        if (cerrarDetalle)
        {
            Close();
        }
    }

    private void DgvServiciosIngreso_SelectionChanged(
        object? sender,
        EventArgs e)
    {
        ActualizarControlesIngreso();
    }

    private void ActualizarGrillaServiciosIngreso()
    {
        dgvServiciosIngreso.DataSource = null;
        dgvServiciosIngreso.DataSource = _serviciosIngreso.ToList();

        dgvServiciosIngreso.ClearSelection();
        dgvServiciosIngreso.CurrentCell = null;

        decimal total = _serviciosIngreso.Sum(fila => fila.Subtotal);

        lblTotalServiciosIngreso.Text =
            $"Total autorizado: {total:N2}";

        ActualizarControlesIngreso();
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

        MessageBox.Show(
            this,
            "El acceso al registro real de diagnóstico está pendiente de conexión.",
            "Registrar diagnóstico",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

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
            new FrmEditarAtencion(
                _idAtencion,
                _atencionServicio);

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

    

    /// <summary>
    /// Evita cerrar el detalle mientras se confirma el ingreso.
    /// </summary>
    private void FrmDetalleAtencion_FormClosing(
        object? sender,
        FormClosingEventArgs e)
    {
        if (_guardandoIngreso)
        {
            e.Cancel = true;
        }
    }

   
}
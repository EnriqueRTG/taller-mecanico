using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Estilos;
using Taller.Presentacion.Formularios.Diagnosticos;
using Taller.Presentacion.Formularios.Vehiculos;
using Taller.Presentacion.Seguridad;
using Taller.Presentacion.Navegacion;

namespace Taller.Presentacion.Formularios.Atenciones
{
    public partial class FrmAtenciones : Form
    {
        #region Campos privados
        private readonly AtencionServicio _atencionServicio;
        private readonly SesionUsuario _sesionUsuario;
        private readonly IServiceProvider _serviceProvider;
        private readonly AutorizacionNavegacion _autorizacionNavegacion;
        private List<AtencionFila> _atenciones = [];
        private bool _formularioNuevaAtencionAbierto;
        private bool _tomandoDiagnostico;
        #endregion

        #region Constructor y modelo de presentación
        public FrmAtenciones(
            AtencionServicio atencionServicio,
            SesionUsuario sesionUsuario,
            IServiceProvider serviceProvider,
            AutorizacionNavegacion autorizacionNavegacion)
        {
            InitializeComponent();

            _atencionServicio = atencionServicio;
            _sesionUsuario = sesionUsuario;
            _serviceProvider = serviceProvider;
            _autorizacionNavegacion = autorizacionNavegacion;

            ConfigurarEstilos();
            ConfigurarMargenesAcciones();
        }

        private sealed class AtencionFila
        {
            public int IdAtencion { get; init; }

            public int? IdTecnicoEnActividad { get; init; }

            public ActividadTecnica? ActividadTecnica { get; init; }

            public string Numero =>
                IdAtencion.ToString("D6");

            public DateTime Fecha { get; init; }

            public string FechaApertura =>
                Fecha.ToString("dd/MM/yyyy HH:mm");

            public string Dominio { get; init; } =
                string.Empty;

            public string Vehiculo { get; init; } =
                string.Empty;

            public string Cliente { get; init; } =
                string.Empty;

            public string Documento { get; init; } =
                string.Empty;

            public string Motivo { get; init; } =
                string.Empty;

            public EstadoAtencion EstadoValor { get; init; }

            public string Estado =>
                ObtenerNombreEstado(EstadoValor);

            public string Etapa { get; init; } =
                string.Empty;

            public string Recepcion { get; init; } =
                string.Empty;
        }

        public event EventHandler<DiagnosticoSolicitadoEventArgs>? DiagnosticoSolicitado;
        #endregion

        #region Configuración del formulario
        private void ConfigurarEstilos()
        {
            BackColor = EstilosInterfaz.FondoAplicacion;

            EstilosInterfaz.AplicarBotonPrimario(
                btnNuevaAtencion);

            EstilosInterfaz.AplicarBotonSecundario(
                btnActualizar);

            EstilosInterfaz.AplicarBotonSecundario(
                btnVerDetalle);

            EstilosInterfaz.AplicarBotonSecundario(
                btnEditar);

            EstilosInterfaz.AplicarBotonSecundario(
                btnIniciarProceso);

            EstilosInterfaz.AplicarBotonPeligro(
                btnCancelarAtencion);

            EstilosInterfaz.AplicarBotonSecundario(
                btnRegistrarEntrega);

            EstilosInterfaz.AplicarBotonSecundario(
                btnVerHistorial);

            EstilosInterfaz.AplicarBotonSecundario(
                btnRegistrarDiagnostico);

            EstilosInterfaz.AplicarBotonSecundario(
                btnFinalizarTrabajo);

            EstilosInterfaz.AplicarGrilla(
                dgvAtenciones);
        }

        private void ConfigurarMargenesAcciones()
        {
            Button[] botones =
            [
                btnVerDetalle,
                btnEditar,
                btnIniciarProceso,
                btnCancelarAtencion,
                btnRegistrarEntrega,
                btnVerHistorial,
                btnRegistrarDiagnostico,
                btnFinalizarTrabajo
            ];

            foreach (Button boton in botones)
            {
                boton.AutoSize = false;
                boton.Height = 34;
                boton.Margin = new Padding(5, 2, 5, 2);
            }

            btnVerDetalle.Width = 105;
            btnEditar.Width = 110;
            btnIniciarProceso.Width = 120;
            btnCancelarAtencion.Width = 105;
            btnRegistrarEntrega.Width = 130;

            btnVerHistorial.Width = 110;
            btnRegistrarDiagnostico.Width = 150;
            btnFinalizarTrabajo.Width = 130;

            btnIniciarProceso.Width = 160;
        }

        private void ConfigurarFiltros()
        {
            bool esTecnico =
                _sesionUsuario.UsuarioActual?.RolId == RolesSistema.TecnicoId;

            cboEstado.Items.Clear();

            cboEstado.Items.Add("Todas");
            cboEstado.Items.Add("En curso");

            if (!esTecnico)
            {
                cboEstado.Items.Add("Finalizadas");
            }

            foreach (var estado in Enum.GetValues<EstadoAtencion>())
            {
                if (esTecnico
                    && (estado == EstadoAtencion.Cerrada
                        || estado == EstadoAtencion.Cancelada))
                {
                    continue;
                }

                cboEstado.Items.Add(ObtenerNombreEstado(estado));
            }

            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;

            dtpDesde.Checked = false;
            dtpHasta.Checked = false;

            cboEstado.SelectedItem = esTecnico
                ? "En curso"
                : "Todas";
        }

        private void ConfigurarInterfazSegunRol()
        {
            var usuario =
                _sesionUsuario.UsuarioActual;

            if (usuario is null)
            {
                OcultarAccionesProtegidas();
                DeshabilitarAcciones();

                lblTitulo.Text =
                    "Atenciones";

                lblDescripcion.Text =
                    "No existe una sesión de usuario válida.";

                lblInformacionRol.Text =
                    "No hay operaciones disponibles.";

                return;
            }

            int rolId = usuario.RolId;

            bool puedeGestionar =
                _autorizacionNavegacion.TienePermiso(
                    rolId,
                    PermisoAplicacion.GestionarAtenciones);

            bool puedeConsultarComoTecnico =
                _autorizacionNavegacion.TienePermiso(
                    rolId,
                    PermisoAplicacion.ConsultarAtencionesAsignadas);

            btnNuevaAtencion.Visible =
                puedeGestionar;

            btnEditar.Visible =
                puedeGestionar;

            btnIniciarProceso.Visible =
                puedeGestionar;

            btnCancelarAtencion.Visible =
                puedeGestionar;

            btnRegistrarEntrega.Visible =
                puedeGestionar;

            btnVerHistorial.Visible =
                puedeConsultarComoTecnico;

            btnRegistrarDiagnostico.Visible =
                puedeConsultarComoTecnico;

            btnFinalizarTrabajo.Visible =
                puedeConsultarComoTecnico;

            colRecepcion.Visible =
                puedeGestionar;

            if (puedeConsultarComoTecnico)
            {
                lblTitulo.Text =
                    "Atenciones técnicas";

                lblDescripcion.Text =
                    "Consulta de vehículos que requieren " +
                    "diagnóstico o intervención técnica.";

                lblInformacionRol.Text =
                    "Vista Técnico: consulte antecedentes, registre " +
                    "el diagnóstico y finalice los trabajos autorizados.";
            }
            else if (puedeGestionar)
            {
                lblTitulo.Text =
                    "Gestión de atenciones";

                lblDescripcion.Text =
                    "Registro y seguimiento de los vehículos " +
                    "ingresados al taller.";

                lblInformacionRol.Text =
                    "Vista Administrativo: registre el ingreso, " +
                    "consulte el avance y gestione la entrega del vehículo.";
            }
            else
            {
                OcultarAccionesProtegidas();
                DeshabilitarAcciones();

                lblTitulo.Text =
                    "Atenciones";

                lblDescripcion.Text =
                    "El usuario actual no posee permisos para " +
                    "acceder a la gestión de atenciones.";

                lblInformacionRol.Text =
                    "No hay operaciones disponibles para este perfil.";
            }
        }

        private void OcultarAccionesProtegidas()
        {
            btnNuevaAtencion.Visible = false;

            btnEditar.Visible = false;
            btnIniciarProceso.Visible = false;
            btnCancelarAtencion.Visible = false;
            btnRegistrarEntrega.Visible = false;

            btnVerHistorial.Visible = false;
            btnRegistrarDiagnostico.Visible = false;
            btnFinalizarTrabajo.Visible = false;

            colRecepcion.Visible = false;
        }
        #endregion

        #region Ciclo de vida y carga de datos
        private async void FrmAtenciones_Load(
            object? sender,
            EventArgs e)
        {
            ConfigurarFiltros();
            ConfigurarInterfazSegunRol();
            DeshabilitarAcciones();

            await CargarAtencionesAsync();
        }

        private async Task CargarAtencionesAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                EstablecerEstadoCarga(true);

                List<Atencion> atenciones =
                    await _atencionServicio.ListarTodasAsync();

                _atenciones = atenciones
                    .Select(MapearFila)
                    .ToList();

                AplicarFiltros();
            }
            catch (Exception ex)
            {
                _atenciones = [];

                dgvAtenciones.DataSource = null;

                lblCantidad.Text =
                    "No se pudieron cargar las atenciones";

                DeshabilitarAcciones();

                MessageBox.Show(
                    $"No se pudieron cargar las atenciones.\n\n{ex.Message}",
                    "Atenciones",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                EstablecerEstadoCarga(false);
            }
        }

        private void EstablecerEstadoCarga(
            bool cargando)
        {
            txtBuscar.Enabled = !cargando;
            cboEstado.Enabled = !cargando;
            dtpDesde.Enabled = !cargando;
            dtpHasta.Enabled = !cargando;
            btnActualizar.Enabled = !cargando;

            btnNuevaAtencion.Enabled =
                !cargando
                && btnNuevaAtencion.Visible;

            dgvAtenciones.Enabled =
                !cargando;

            if (cargando)
            {
                DeshabilitarAcciones();
            }
        }
        #endregion

        #region Transformación y presentación de datos
        private static AtencionFila MapearFila(
            Atencion atencion)
        {
            return new AtencionFila
            {
                IdAtencion = atencion.IdAtencion,
                IdTecnicoEnActividad = atencion.IdTecnicoEnActividad,
                Fecha = atencion.FechaApertura,
                Dominio = atencion.Vehiculo.Dominio,
                ActividadTecnica = atencion.ActividadTecnica,

                Vehiculo =
                    $"{atencion.Vehiculo.Modelo.Marca.Nombre} " +
                    atencion.Vehiculo.Modelo.Nombre,

                Cliente =
                    $"{atencion.Cliente.Apellido}, " +
                    atencion.Cliente.Nombre,

                Documento =
                    $"{atencion.Cliente.TipoDocumento} " +
                    atencion.Cliente.Documento,

                Motivo =
                    atencion.MotivoConsulta,

                EstadoValor =
                    atencion.Estado,

                Etapa =
                    ObtenerEtapaTemporal(atencion),

                Recepcion =
                    $"{atencion.UsuarioRecepcion.Nombre} " +
                    atencion.UsuarioRecepcion.Apellido
            };
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

        private static string ObtenerEtapaTemporal(
            Atencion atencion)
        {
            return atencion.Estado switch
            {
                EstadoAtencion.Abierta =>
                    "Ingreso registrado",

                EstadoAtencion.PendienteDiagnostico =>
                    "Espera que un técnico tome el diagnóstico",

                EstadoAtencion.EnEvaluacionTecnica =>
                    "Evaluación técnica y preparación de propuesta",

                EstadoAtencion.PendienteDecision =>
                    "Espera la decisión del cliente",

                EstadoAtencion.EnEjecucion =>
                    "Ejecución de trabajos autorizados",

                EstadoAtencion.Pausada =>
                    "Resolución de un hallazgo adicional",

                EstadoAtencion.TrabajoFinalizado =>
                    "Reparación terminada; resta liquidar y entregar",

                EstadoAtencion.PendienteLiquidacion =>
                    "Actividad técnica terminada; resta liquidar y entregar",

                EstadoAtencion.Cerrada =>
                    "Vehículo entregado y liquidación completada",

                EstadoAtencion.Cancelada =>
                    "Atención cancelada",

                _ => "Sin determinar"
            };
        }
        #endregion

        #region Filtros y selección
        private void AplicarFiltros()
        {
            IEnumerable<AtencionFila> consulta = _atenciones;

            bool esTecnico = _sesionUsuario.UsuarioActual?.RolId == RolesSistema.TecnicoId;

            if (esTecnico)
            {
                consulta = consulta.Where(a =>
                    a.EstadoValor != EstadoAtencion.Cerrada
                    && a.EstadoValor != EstadoAtencion.Cancelada);
            }

            string texto = txtBuscar.Text.Trim();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                consulta = consulta.Where(a =>
                    a.Numero.Contains(
                        texto,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    a.Dominio.Contains(
                        texto,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    a.Cliente.Contains(
                        texto,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    a.Documento.Contains(
                        texto,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    a.Motivo.Contains(
                        texto,
                        StringComparison.OrdinalIgnoreCase));
            }

            string estadoSeleccionado =
                   cboEstado.SelectedItem?.ToString() ?? "Todas";

            switch (estadoSeleccionado)
            {
                case "Todas":
                    break;

                case "En curso":
                    consulta = consulta.Where(a =>
                        a.EstadoValor != EstadoAtencion.Cerrada &&
                        a.EstadoValor != EstadoAtencion.Cancelada);
                    break;

                case "Finalizadas":
                    consulta = consulta.Where(a =>
                        a.EstadoValor == EstadoAtencion.Cerrada ||
                        a.EstadoValor == EstadoAtencion.Cancelada);
                    break;

                default:
                    consulta = consulta.Where(a =>
                        ObtenerNombreEstado(a.EstadoValor) == estadoSeleccionado);
                    break;
            }

            // Cada límite es opcional e independiente.
            bool filtrarDesde = dtpDesde.Checked;
            bool filtrarHasta = dtpHasta.Checked;

            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date;

            if (filtrarDesde && filtrarHasta && desde > hasta)
            {
                dgvAtenciones.DataSource = null;

                lblCantidad.Text =
                    "Rango inválido: Desde debe ser anterior o igual a Hasta.";

                DeshabilitarAcciones();
                return;
            }

            if (filtrarDesde)
            {
                consulta = consulta.Where(a =>
                    a.Fecha.Date >= desde);
            }

            if (filtrarHasta)
            {
                consulta = consulta.Where(a =>
                    a.Fecha.Date <= hasta);
            }

            // Pendientes: grupo operativo y luego mayor antigüedad de apertura.
            var enCurso = consulta
                .Where(a => !EsAtencionFinalizada(a.EstadoValor))
                .OrderBy(ObtenerGrupoOperativo)
                .ThenBy(a => a.Fecha)
                .ThenBy(a => a.IdAtencion);

            // Historial: primero las aperturas más recientes.
            var finalizadas = consulta
                .Where(a => EsAtencionFinalizada(a.EstadoValor))
                .OrderByDescending(a => a.Fecha)
                .ThenByDescending(a => a.IdAtencion);

            List<AtencionFila> resultado = enCurso
                .Concat(finalizadas)
                .ToList();

            dgvAtenciones.DataSource = null;
            dgvAtenciones.DataSource = resultado;

            dgvAtenciones.ClearSelection();
            dgvAtenciones.CurrentCell = null;

            lblCantidad.Text = resultado.Count == 1
                ? "1 atención encontrada"
                : $"{resultado.Count} atenciones encontradas";

            DeshabilitarAcciones();
        }

        private AtencionFila? ObtenerFilaSeleccionada()
        {
            return dgvAtenciones.CurrentRow?.DataBoundItem
                as AtencionFila;
        }

        private void ActualizarAccionesSeleccion()
        {
            AtencionFila? fila =
                ObtenerFilaSeleccionada();

            var usuario =
                _sesionUsuario.UsuarioActual;

            if (fila is null || usuario is null)
            {
                DeshabilitarAcciones();
                return;
            }

            bool puedeGestionar =
                _autorizacionNavegacion.TienePermiso(
                    usuario.RolId,
                    PermisoAplicacion.GestionarAtenciones);

            bool puedeConsultarComoTecnico =
                _autorizacionNavegacion.TienePermiso(
                    usuario.RolId,
                    PermisoAplicacion.ConsultarAtencionesAsignadas);

            // Durante la primera entrega las acciones se habilitan
            // al seleccionar una atención para demostrar el circuito
            // completo de navegación.
            //
            // Las restricciones por estado y etapa se incorporarán
            // junto con la lógica de negocio definitiva.
            btnVerDetalle.Enabled =
                puedeGestionar || puedeConsultarComoTecnico;

            btnEditar.Enabled =
                puedeGestionar &&
                fila.EstadoValor == EstadoAtencion.Abierta;

            btnIniciarProceso.Enabled =
                puedeGestionar &&
                fila.EstadoValor == EstadoAtencion.Abierta;

            btnCancelarAtencion.Enabled =
                puedeGestionar;

            btnRegistrarEntrega.Enabled =
                puedeGestionar;

            btnVerHistorial.Enabled =
                puedeConsultarComoTecnico;

            bool puedeTomarDiagnostico =
                fila.EstadoValor == EstadoAtencion.PendienteDiagnostico
                && fila.IdTecnicoEnActividad is null
                && fila.ActividadTecnica is null;

            bool puedeRegistrarDiagnostico =
                fila.EstadoValor == EstadoAtencion.EnEvaluacionTecnica
                && fila.IdTecnicoEnActividad == usuario.Id
                && fila.ActividadTecnica == ActividadTecnica.Diagnostico;

            btnRegistrarDiagnostico.Text =
                puedeRegistrarDiagnostico
                    ? "Registrar diagnóstico"
                    : "Tomar diagnóstico";

            btnRegistrarDiagnostico.Enabled =
                !_tomandoDiagnostico
                && puedeConsultarComoTecnico
                && (puedeTomarDiagnostico || puedeRegistrarDiagnostico);

            btnFinalizarTrabajo.Enabled =
                puedeConsultarComoTecnico;
        }

        private void DeshabilitarAcciones()
        {
            btnVerDetalle.Enabled = false;
            btnEditar.Enabled = false;
            btnIniciarProceso.Enabled = false;
            btnCancelarAtencion.Enabled = false;
            btnRegistrarEntrega.Enabled = false;

            btnVerHistorial.Enabled = false;
            btnRegistrarDiagnostico.Enabled = false;
            btnFinalizarTrabajo.Enabled = false;
        }
        #endregion

        #region Eventos de filtros y grilla
        private void TxtBuscar_TextChanged(
            object? sender,
            EventArgs e)
        {
            AplicarFiltros();
        }

        private void CboEstado_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            AplicarFiltros();
        }

        private void FiltroFecha_ValueChanged(
            object? sender,
            EventArgs e)
        {
            AplicarFiltros();
        }

        private async void BtnActualizar_Click(
            object? sender,
            EventArgs e)
        {
            await CargarAtencionesAsync();
        }

        private void DgvAtenciones_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            ActualizarAccionesSeleccion();
        }

        private void DgvAtenciones_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvAtenciones.Columns[e.ColumnIndex].Name
                != "colEstado")
            {
                return;
            }

            string estado =
                e.Value?.ToString() ?? string.Empty;

            e.CellStyle.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            switch (estado)
            {
                case "Abierta":
                    e.CellStyle.ForeColor =
                        Color.FromArgb(30, 64, 175);
                    break;

                case "En proceso":
                    e.CellStyle.ForeColor =
                        Color.FromArgb(146, 64, 14);
                    break;

                case "Cerrada":
                    e.CellStyle.ForeColor =
                        Color.FromArgb(22, 101, 52);
                    break;

                case "Cancelada":
                    e.CellStyle.ForeColor =
                        Color.FromArgb(185, 28, 28);
                    break;
            }
        }


        private void DgvAtenciones_CellToolTipTextNeeded(
            object? sender,
            DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvAtenciones.Rows[e.RowIndex].DataBoundItem
                is not AtencionFila fila)
            {
                return;
            }

            string nombreColumna =
                dgvAtenciones.Columns[e.ColumnIndex].Name;

            if (nombreColumna == "colMotivo")
            {
                e.ToolTipText =
                    fila.Motivo;
            }
            else if (nombreColumna == "colEtapa")
            {
                e.ToolTipText =
                    fila.Etapa;
            }
        }
        #endregion

        #region Eventos de acciones administrativas
        private async void BtnNuevaAtencion_Click(
            object? sender,
            EventArgs e)
        {
            if (_formularioNuevaAtencionAbierto)
            {
                return;
            }

            try
            {
                _formularioNuevaAtencionAbierto = true;
                btnNuevaAtencion.Enabled = false;

                using FrmNuevaAtencion formulario =
                    _serviceProvider
                        .GetRequiredService<FrmNuevaAtencion>();

                DialogResult resultado =
                    formulario.ShowDialog(this);

                if (resultado == DialogResult.OK)
                {
                    await CargarAtencionesAsync();
                }
            }
            finally
            {
                _formularioNuevaAtencionAbierto = false;

                btnNuevaAtencion.Enabled =
                    btnNuevaAtencion.Visible;
            }
        }

        private void BtnEditar_Click(
            object? sender,
            EventArgs e)
        {
            AbrirEdicionSeleccionada();
        }

        private void BtnCancelarAtencion_Click(
            object? sender,
            EventArgs e)
        {
            ConfirmarCancelacionAtencion();
        }

        /// <summary>
        /// Abre el formulario de entrega para la atención seleccionada.
        /// </summary>
        private void BtnRegistrarEntrega_Click(
            object? sender,
            EventArgs e)
        {
            AtencionFila? fila = ObtenerFilaSeleccionada();

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "Seleccioná una atención para registrar la entrega.",
                    "Atención no seleccionada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var usuario = _sesionUsuario.UsuarioActual;

            if (usuario is null)
            {
                MessageBox.Show(
                    this,
                    "No se pudo identificar al usuario actual.",
                    "Sesión no disponible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            bool puedeGestionar =
                _autorizacionNavegacion.TienePermiso(
                    usuario.RolId,
                    PermisoAplicacion.GestionarAtenciones);

            if (!puedeGestionar)
            {
                MessageBox.Show(
                    this,
                    "No posee permisos para registrar la entrega.",
                    "Acceso restringido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using FrmRegistrarEntrega formulario =
                new();

            formulario.ShowDialog(this);
        }
        #endregion

        #region Eventos de consulta y acciones técnicas
        private async void BtnVerDetalle_ClickAsync(
            object? sender,
            EventArgs e)
        {
            await AbrirDetalleSeleccionadoAsync();
        }

        private void BtnVerHistorial_Click(
            object? sender,
            EventArgs e)
        {
            AbrirHistorialSeleccionado();
        }

        private void BtnRegistrarDiagnostico_Click(
            object? sender,
            EventArgs e)
        {
            var fila = ObtenerFilaSeleccionada();
            var usuario = _sesionUsuario.UsuarioActual;

            if (_tomandoDiagnostico
                || fila is null
                || usuario?.RolId != RolesSistema.TecnicoId)
            {
                return;
            }

            bool puedeTomar =
                fila.EstadoValor == EstadoAtencion.PendienteDiagnostico
                && fila.IdTecnicoEnActividad is null
                && fila.ActividadTecnica is null;

            bool puedeRetomar =
                fila.EstadoValor == EstadoAtencion.EnEvaluacionTecnica
                && fila.IdTecnicoEnActividad == usuario.Id
                && fila.ActividadTecnica == ActividadTecnica.Diagnostico;

            if (!puedeTomar && !puedeRetomar)
            {
                return;
            }

            DiagnosticoSolicitado?.Invoke(
                this,
                new DiagnosticoSolicitadoEventArgs(fila.IdAtencion));
        }

        /// <summary>
        /// Abre el formulario de finalización del trabajo técnico
        /// para la atención seleccionada.
        /// </summary>
        private void BtnFinalizarTrabajo_Click(
            object? sender,
            EventArgs e)
        {
            AtencionFila? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "Seleccioná una atención para finalizar el trabajo.",
                    "Atención no seleccionada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var usuario =
                _sesionUsuario.UsuarioActual;

            if (usuario is null)
            {
                MessageBox.Show(
                    this,
                    "No se pudo identificar al usuario actual.",
                    "Sesión no disponible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            bool puedeFinalizarTrabajo =
                _autorizacionNavegacion.TienePermiso(
                    usuario.RolId,
                    PermisoAplicacion.ConsultarAtencionesAsignadas);

            if (!puedeFinalizarTrabajo)
            {
                MessageBox.Show(
                    this,
                    "No posee permisos para finalizar trabajos técnicos.",
                    "Acceso restringido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using FrmFinalizarTrabajo formulario =
                new();

            if (formulario.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }

            ActualizarAccionesSeleccion();
        }
        #endregion

        #region Navegación y mensajes auxiliares
        private async Task AbrirDetalleSeleccionadoAsync()
        {
            AtencionFila? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "Seleccioná una atención para consultar su detalle.",
                    "Detalle de atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            Usuario? usuario =
                _sesionUsuario.UsuarioActual;

            if (usuario is null)
            {
                MessageBox.Show(
                    this,
                    "No existe una sesión de usuario válida.",
                    "Detalle de atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using var formulario =
                new FrmDetalleAtencion(
                        fila.IdAtencion,
                        _atencionServicio,
                        _sesionUsuario);

            formulario.ShowDialog(this);

            await CargarAtencionesAsync();
        }

        /// <summary>
        /// Abre directamente el formulario demostrativo de registro
        /// de diagnóstico para la atención seleccionada.
        /// </summary>
        private void AbrirRegistroDiagnosticoSeleccionado()
        {
            AtencionFila? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "Seleccioná una atención para registrar el diagnóstico.",
                    "Registrar diagnóstico",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            Usuario? usuario =
                _sesionUsuario.UsuarioActual;

            if (usuario is null)
            {
                MessageBox.Show(
                    this,
                    "No existe una sesión de usuario válida.",
                    "Registrar diagnóstico",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            bool puedeRegistrar =
                _autorizacionNavegacion.TienePermiso(
                    usuario.RolId,
                    PermisoAplicacion.GestionarDiagnosticos);

            if (!puedeRegistrar)
            {
                MessageBox.Show(
                    this,
                    "Solamente el personal técnico puede registrar diagnósticos.",
                    "Acceso restringido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using var formulario =
                new FrmRegistrarDiagnostico();

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado != DialogResult.OK)
            {
                return;
            }

            MessageBox.Show(
                this,
                "El diagnóstico se registró de manera simulada.\n\n" +
                "La persistencia y actualización definitiva de la atención " +
                "se incorporarán en una entrega posterior.",
                "Diagnóstico registrado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Abre de manera modal la pantalla demostrativa de edición
        /// para la atención seleccionada.
        /// </summary>
        private async Task AbrirEdicionSeleccionada()
        {
            AtencionFila? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "Seleccioná una atención para editar sus datos.",
                    "Editar atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var atencion = await _atencionServicio.ObtenerPorIdAsync(
                fila.IdAtencion);

            if (atencion is null
                || atencion.Estado != EstadoAtencion.Abierta
                || atencion.FechaCierre is not null
                || atencion.IdTecnicoEnActividad is not null
                || atencion.ActividadTecnica is not null)
            {
                MessageBox.Show(
                    this,
                    "La atención ya no está disponible para editar sus datos. " +
                    "Se actualizará el listado.",
                    "Editar atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await CargarAtencionesAsync();
                return;
            }

            using var formulario =
                new FrmEditarAtencion(
                    fila.IdAtencion,
                    _atencionServicio);

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado != DialogResult.OK)
            {
                return;
            }

            await CargarAtencionesAsync();
        }

        private void MostrarFuncionEnDesarrollo(
            string nombreFuncion)
        {
            MessageBox.Show(
                $"La opción \"{nombreFuncion}\" forma parte del flujo " +
                "previsto del sistema y será conectada con su lógica " +
                "de negocio en la siguiente etapa.",
                nombreFuncion,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Solicita confirmación para simular el inicio del proceso
        /// de la atención seleccionada.
        /// </summary>
        private void ConfirmarInicioProceso()
        {
            AtencionFila? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "Seleccioná una atención para iniciar su proceso.",
                    "Iniciar proceso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult respuesta =
                MessageBox.Show(
                    this,
                    $"¿Desea iniciar el proceso de la atención N.º {fila.Numero}?\n\n" +
                    "En la implementación definitiva, la atención avanzará " +
                    "a la etapa de evaluación técnica.",
                    "Iniciar proceso",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            MessageBox.Show(
                this,
                "El inicio del proceso fue simulado correctamente.\n\n" +
                "La atención quedaría disponible para la evaluación técnica.",
                "Proceso iniciado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Solicita confirmación para simular la cancelación lógica
        /// de la atención seleccionada.
        /// </summary>
        private void ConfirmarCancelacionAtencion()
        {
            AtencionFila? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "Seleccioná una atención para cancelarla.",
                    "Cancelar atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult respuesta =
                MessageBox.Show(
                    this,
                    $"¿Confirma la cancelación de la atención N.º {fila.Numero}?\n\n" +
                    "La cancelación definitiva será lógica: la atención " +
                    "permanecerá registrada en el historial del sistema.",
                    "Cancelar atención",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            MessageBox.Show(
                this,
                "La cancelación de la atención fue simulada correctamente.",
                "Atención cancelada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Abre el historial demostrativo del vehículo correspondiente
        /// a la atención seleccionada.
        /// </summary>
        private void AbrirHistorialSeleccionado()
        {
            AtencionFila? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "Seleccioná una atención para consultar el historial del vehículo.",
                    "Historial del vehículo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using var formulario =
                new FrmHistorialVehiculo();

            formulario.ShowDialog(this);
        }
        #endregion

        private void DgvAtenciones_CellDoubleClickAsync(
            object sender, 
            DataGridViewCellEventArgs e)
        {

        }

        private async void BtnIniciarProceso_ClickAsync(
            object sender, 
            EventArgs e)
        {
            await AbrirDetalleSeleccionadoAsync();
        }

        private static bool EsAtencionFinalizada(
            EstadoAtencion estado)
        {
            return estado == EstadoAtencion.Cerrada
                || estado == EstadoAtencion.Cancelada;
        }

        /// <summary>
        /// Determina el grupo de presentación según el rol.
        /// No concede permisos ni reemplaza validaciones del servicio.
        /// </summary>
        private int ObtenerGrupoOperativo(
            AtencionFila fila)
        {
            if (EsAtencionFinalizada(fila.EstadoValor))
            {
                return 9;
            }

            var usuario = _sesionUsuario.UsuarioActual;

            if (usuario is null)
            {
                return 8;
            }

            if (usuario.RolId == RolesSistema.AdministrativoId)
            {
                return fila.EstadoValor switch
                {
                    EstadoAtencion.PendienteDecision => 0,

                    EstadoAtencion.TrabajoFinalizado
                        or EstadoAtencion.PendienteLiquidacion => 1,

                    EstadoAtencion.Abierta => 2,

                    _ => 3
                };
            }

            if (usuario.RolId == RolesSistema.TecnicoId)
            {
                // Primero, el trabajo que este técnico ya tomó.
                if (fila.IdTecnicoEnActividad == usuario.Id)
                {
                    return 0;
                }

                // Luego, diagnósticos disponibles para tomar.
                if (fila.EstadoValor == EstadoAtencion.PendienteDiagnostico
                    && fila.IdTecnicoEnActividad is null)
                {
                    return 1;
                }

                // Etapas técnicas sin reserva.
                // Su operación concreta seguirá validándose en los servicios.
                if (fila.IdTecnicoEnActividad is null)
                {
                    return fila.EstadoValor switch
                    {
                        EstadoAtencion.EnEvaluacionTecnica => 2,
                        EstadoAtencion.EnEjecucion => 3,
                        EstadoAtencion.Pausada => 4,
                        _ => 5
                    };
                }

                // Actividades reservadas por otro técnico: seguimiento.
                return 5;
            }

            return 8;
        }
    }
}

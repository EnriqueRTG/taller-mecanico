using Taller.Dominio.Tiempo;
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
    public partial class FrmAtenciones : Form, IFormularioConOperacion
    {
#region Campos privados
        private readonly AtencionServicio _atencionServicio;
        private readonly SesionUsuario _sesionUsuario;
        private readonly IServiceProvider _serviceProvider;
        private readonly AutorizacionNavegacion _autorizacionNavegacion;
        private List<AtencionFila> _atenciones = [];
        private bool _formularioNuevaAtencionAbierto;
        private bool _tomandoDiagnostico = false;
        private bool _cargandoDatos;
        public bool OperacionEnCurso => _cargandoDatos;

#endregion
#region Constructor y modelo de presentación
        public FrmAtenciones(AtencionServicio atencionServicio, SesionUsuario sesionUsuario, IServiceProvider serviceProvider, AutorizacionNavegacion autorizacionNavegacion)
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
            public bool TieneSolicitudes { get; init; }
            public bool EvaluacionFinalizada { get; init; }
            public int? IdTecnicoEnActividad { get; init; }
            public ActividadTecnica? ActividadTecnica { get; init; }
            public string Numero => IdAtencion.ToString("D6");
            public DateTime Fecha { get; init; }
            public string FechaApertura => Fecha.ToString("dd/MM/yyyy HH:mm");
            public string Dominio { get; init; } = string.Empty;
            public string Vehiculo { get; init; } = string.Empty;
            public string Cliente { get; init; } = string.Empty;
            public string Documento { get; init; } = string.Empty;
            public string Motivo { get; init; } = string.Empty;
            public TipoSolicitudAtencion TipoSolicitud { get; init; }
            public EstadoAtencion EstadoValor { get; init; }
            public string Estado => ObtenerNombreEstado(EstadoValor);
            public string Etapa { get; init; } = string.Empty;
            public string Recepcion { get; init; } = string.Empty;
        }

        public event EventHandler<DiagnosticoSolicitadoEventArgs>? DiagnosticoSolicitado;
#endregion
#region Configuración del formulario
        private void ConfigurarEstilos()
        {
            BackColor = EstilosInterfaz.FondoAplicacion;
            EstilosInterfaz.AplicarBotonPrimario(btnNuevaAtencion);
            EstilosInterfaz.AplicarBotonSecundario(btnActualizar);
            EstilosInterfaz.AplicarBotonSecundario(btnVerDetalle);
            EstilosInterfaz.AplicarBotonSecundario(btnEditar);
            EstilosInterfaz.AplicarBotonSecundario(btnIniciarProceso);
            EstilosInterfaz.AplicarBotonPeligro(btnCancelarAtencion);
            EstilosInterfaz.AplicarBotonSecundario(btnRegistrarEntrega);
            EstilosInterfaz.AplicarBotonSecundario(btnVerHistorial);
            EstilosInterfaz.AplicarBotonSecundario(btnRegistrarDiagnostico);
            EstilosInterfaz.AplicarBotonSecundario(btnFinalizarTrabajo);
            EstilosInterfaz.AplicarGrilla(dgvAtenciones);
        }

        private void ConfigurarMargenesAcciones()
        {
            Button[] botones = [btnVerDetalle, btnEditar, btnIniciarProceso, btnCancelarAtencion, btnRegistrarEntrega, btnVerHistorial, btnRegistrarDiagnostico, btnFinalizarTrabajo];
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
            bool esTecnico = _sesionUsuario.UsuarioActual?.RolId == RolesSistema.TecnicoId;
            cboEstado.Items.Clear();
            cboEstado.Items.Add("Todas");
            cboEstado.Items.Add("En curso");
            if (!esTecnico)
            {
                cboEstado.Items.Add("Finalizadas");
            }

            foreach (var estado in Enum.GetValues<EstadoAtencion>())
            {
                if (esTecnico && (estado == EstadoAtencion.Cerrada || estado == EstadoAtencion.Cancelada))
                {
                    continue;
                }

                cboEstado.Items.Add(ObtenerNombreEstado(estado));
            }

            dtpDesde.Value = FechaHoraTaller.Hoy.AddMonths(-1);
            dtpHasta.Value = FechaHoraTaller.Hoy;
            dtpDesde.Checked = false;
            dtpHasta.Checked = false;
            cboEstado.SelectedItem = esTecnico ? "En curso" : "Todas";
        }

        private void ConfigurarInterfazSegunRol()
        {
            var usuario = _sesionUsuario.UsuarioActual;
            if (usuario is null)
            {
                OcultarAccionesProtegidas();
                DeshabilitarAcciones();
                lblTitulo.Text = "Atenciones";
                lblDescripcion.Text = "No existe una sesión de usuario válida.";
                lblInformacionRol.Text = "No hay operaciones disponibles.";
                return;
            }

            int rolId = usuario.RolId;
            bool puedeGestionar = _autorizacionNavegacion.TienePermiso(rolId, PermisoAplicacion.GestionarAtenciones);
            bool puedeConsultarComoTecnico = _autorizacionNavegacion.TienePermiso(rolId, PermisoAplicacion.ConsultarAtencionesAsignadas);
            btnNuevaAtencion.Visible = puedeGestionar;
            btnEditar.Visible = puedeGestionar;
            btnIniciarProceso.Visible = puedeGestionar;
            btnCancelarAtencion.Visible = puedeGestionar;
            btnRegistrarEntrega.Visible = puedeGestionar;
            btnVerHistorial.Visible = puedeGestionar || puedeConsultarComoTecnico;
            btnRegistrarDiagnostico.Visible = puedeConsultarComoTecnico;
            btnFinalizarTrabajo.Visible = puedeConsultarComoTecnico;
            colRecepcion.Visible = puedeGestionar;
            if (puedeConsultarComoTecnico)
            {
                lblTitulo.Text = "Atenciones técnicas";
                lblDescripcion.Text = "Consulta de vehículos que requieren " + "diagnóstico o intervención técnica.";
                lblInformacionRol.Text = "Vista Técnico: consulte antecedentes, registre " + "el diagnóstico y finalice los trabajos autorizados.";
            }
            else if (puedeGestionar)
            {
                lblTitulo.Text = "Gestión de atenciones";
                lblDescripcion.Text = "Registro y seguimiento de los vehículos " + "ingresados al taller.";
                lblInformacionRol.Text = "Vista Administrativo: registre el ingreso, " + "consulte el avance y gestione la entrega del vehículo.";
            }
            else
            {
                OcultarAccionesProtegidas();
                DeshabilitarAcciones();
                lblTitulo.Text = "Atenciones";
                lblDescripcion.Text = "El usuario actual no posee permisos para " + "acceder a la gestión de atenciones.";
                lblInformacionRol.Text = "No hay operaciones disponibles para este perfil.";
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
        private async void FrmAtenciones_Load(object? sender, EventArgs e)
        {
            ConfigurarFiltros();
            ConfigurarInterfazSegunRol();
            DeshabilitarAcciones();
            await CargarAtencionesAsync();
        }

        private async Task CargarAtencionesAsync()
        {
            if (_cargandoDatos)
                return;
            _cargandoDatos = true;
            try
            {
                Cursor = Cursors.WaitCursor;
                EstablecerEstadoCarga(true);
                List<Atencion> atenciones = await _atencionServicio.ListarTodasAsync();
                _atenciones = atenciones.Select(MapearFila).ToList();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                _atenciones = [];
                dgvAtenciones.DataSource = null;
                lblCantidad.Text = "No se pudieron cargar las atenciones";
                DeshabilitarAcciones();
                Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Atenciones");
            }
            finally
            {
                Cursor = Cursors.Default;
                EstablecerEstadoCarga(false);
                _cargandoDatos = false;
            }
        }

        private void EstablecerEstadoCarga(bool cargando)
        {
            txtBuscar.Enabled = !cargando;
            cboEstado.Enabled = !cargando;
            dtpDesde.Enabled = !cargando;
            dtpHasta.Enabled = !cargando;
            btnActualizar.Enabled = !cargando;
            btnLimpiarFiltros.Enabled = !cargando;
            btnNuevaAtencion.Enabled = !cargando && btnNuevaAtencion.Visible;
            dgvAtenciones.Enabled = !cargando;
            if (cargando)
            {
                DeshabilitarAcciones();
            }
        }

#endregion
#region Transformación y presentación de datos
        private static AtencionFila MapearFila(Atencion atencion)
        {
            return new AtencionFila
            {
                IdAtencion = atencion.IdAtencion,
                TieneSolicitudes = atencion.Solicitudes.Count > 0,
                EvaluacionFinalizada = atencion.Evaluacion?.Finalizada == true,
                IdTecnicoEnActividad = atencion.IdTecnicoEnActividad,
                Fecha = atencion.FechaApertura,
                Dominio = atencion.Vehiculo.Dominio,
                ActividadTecnica = atencion.ActividadTecnica,
                Vehiculo = $"{atencion.Vehiculo.Modelo.Marca.Nombre} " + atencion.Vehiculo.Modelo.Nombre,
                Cliente = $"{atencion.Cliente.Apellido}, " + atencion.Cliente.Nombre,
                Documento = $"{atencion.Cliente.TipoDocumento} " + atencion.Cliente.Documento,
                Motivo = atencion.Solicitudes.Count == 0 ? atencion.MotivoConsulta : string.Join("; ", atencion.Solicitudes.OrderBy(s => s.Orden).Select(s => s.Descripcion)),
                TipoSolicitud = atencion.TipoSolicitud,
                EstadoValor = atencion.Estado,
                Etapa = ObtenerEtapaTemporal(atencion),
                Recepcion = $"{atencion.UsuarioRecepcion.Nombre} " + atencion.UsuarioRecepcion.Apellido
            };
        }

        private static string ObtenerNombreEstado(EstadoAtencion estado)
        {
            return estado switch
            {
                EstadoAtencion.Abierta => "Abierta",
                EstadoAtencion.PendienteDiagnostico => "Pendiente de diagnóstico",
                EstadoAtencion.EnEvaluacionTecnica => "En preparación técnica",
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

        private static string ObtenerEtapaTemporal(Atencion atencion)
        {
            if (atencion.Pausas.Any(p => p.FechaResolucion == null))
                return atencion.Pausas.Any(p => p.FechaResolucion == null && p.IdDetalle == null) ? "Intervención pausada: consultar seguimiento" : "Hay tareas pausadas; otras pueden continuar";
            return atencion.Estado switch
            {
                EstadoAtencion.Abierta => "Ingreso registrado",
                EstadoAtencion.PendienteDiagnostico => "Espera que un técnico tome el diagnóstico",
                EstadoAtencion.EnEvaluacionTecnica when atencion.Evaluacion?.Finalizada == true => "Presupuesto preparado: recepción debe informar al cliente",
                EstadoAtencion.EnEvaluacionTecnica when atencion.IdTecnicoEnActividad == null => "Pendiente de preparación técnica",
                EstadoAtencion.EnEvaluacionTecnica => "Técnico preparando el alcance y presupuesto",
                EstadoAtencion.PendienteDecision => "Espera la decisión del cliente",
                EstadoAtencion.EnEjecucion => "Ejecución de trabajos autorizados",
                EstadoAtencion.Pausada => "Resolución de un hallazgo adicional",
                EstadoAtencion.TrabajoFinalizado => "Reparación terminada; resta liquidar y entregar",
                EstadoAtencion.PendienteLiquidacion => "Actividad técnica terminada; resta liquidar y entregar",
                EstadoAtencion.Cerrada => "Vehículo entregado y liquidación completada",
                EstadoAtencion.Cancelada => "Atención cancelada",
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
                consulta = consulta.Where(a => a.EstadoValor != EstadoAtencion.Cerrada && a.EstadoValor != EstadoAtencion.Cancelada);
            }

            string texto = txtBuscar.Text.Trim();
            if (!string.IsNullOrWhiteSpace(texto))
            {
                consulta = consulta.Where(a => a.Numero.Contains(texto, StringComparison.OrdinalIgnoreCase) || a.Dominio.Contains(texto, StringComparison.OrdinalIgnoreCase) || a.Cliente.Contains(texto, StringComparison.OrdinalIgnoreCase) || a.Documento.Contains(texto, StringComparison.OrdinalIgnoreCase) || a.Motivo.Contains(texto, StringComparison.OrdinalIgnoreCase));
            }

            string estadoSeleccionado = cboEstado.SelectedItem?.ToString() ?? "Todas";
            switch (estadoSeleccionado)
            {
                case "Todas":
                    break;
                case "En curso":
                    consulta = consulta.Where(a => a.EstadoValor != EstadoAtencion.Cerrada && a.EstadoValor != EstadoAtencion.Cancelada);
                    break;
                case "Finalizadas":
                    consulta = consulta.Where(a => a.EstadoValor == EstadoAtencion.Cerrada || a.EstadoValor == EstadoAtencion.Cancelada);
                    break;
                default:
                    consulta = consulta.Where(a => ObtenerNombreEstado(a.EstadoValor) == estadoSeleccionado);
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
                lblCantidad.Text = "Rango inválido: Desde debe ser anterior o igual a Hasta.";
                DeshabilitarAcciones();
                return;
            }

            if (filtrarDesde)
            {
                consulta = consulta.Where(a => a.Fecha.Date >= desde);
            }

            if (filtrarHasta)
            {
                consulta = consulta.Where(a => a.Fecha.Date <= hasta);
            }

            // Pendientes: grupo operativo y luego mayor antigüedad de apertura.
            var enCurso = consulta.Where(a => !EsAtencionFinalizada(a.EstadoValor)).OrderBy(ObtenerGrupoOperativo).ThenBy(a => a.Fecha).ThenBy(a => a.IdAtencion);
            // Historial: primero las aperturas más recientes.
            var finalizadas = consulta.Where(a => EsAtencionFinalizada(a.EstadoValor)).OrderByDescending(a => a.Fecha).ThenByDescending(a => a.IdAtencion);
            List<AtencionFila> resultado = enCurso.Concat(finalizadas).ToList();
            dgvAtenciones.DataSource = null;
            dgvAtenciones.DataSource = resultado;
            dgvAtenciones.ClearSelection();
            dgvAtenciones.CurrentCell = null;
            lblCantidad.Text = resultado.Count == 1 ? "1 atención encontrada" : $"{resultado.Count} atenciones encontradas";
            DeshabilitarAcciones();
        }

        private AtencionFila? ObtenerFilaSeleccionada()
        {
            return dgvAtenciones.CurrentRow?.DataBoundItem as AtencionFila;
        }

        private void ActualizarAccionesSeleccion()
        {
            AtencionFila? fila = ObtenerFilaSeleccionada();
            var usuario = _sesionUsuario.UsuarioActual;
            if (fila is null || usuario is null)
            {
                DeshabilitarAcciones();
                return;
            }

            bool puedeGestionar = _autorizacionNavegacion.TienePermiso(usuario.RolId, PermisoAplicacion.GestionarAtenciones);
            bool puedeConsultarComoTecnico = _autorizacionNavegacion.TienePermiso(usuario.RolId, PermisoAplicacion.ConsultarAtencionesAsignadas);
            btnVerDetalle.Enabled = puedeGestionar || puedeConsultarComoTecnico;
            btnSeguimiento.Enabled = fila.EstadoValor != EstadoAtencion.Abierta && (puedeGestionar || puedeConsultarComoTecnico);
            btnEditar.Enabled = puedeGestionar && fila.EstadoValor == EstadoAtencion.Abierta && !fila.TieneSolicitudes;
            btnIniciarProceso.Enabled = puedeGestionar && fila.EstadoValor == EstadoAtencion.Abierta;
            btnCancelarAtencion.Enabled = puedeGestionar && fila.EstadoValor == EstadoAtencion.Abierta;
            btnRegistrarEntrega.Enabled = puedeGestionar && fila.EstadoValor is EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion or EstadoAtencion.Cerrada;
            btnVerHistorial.Enabled = puedeGestionar || puedeConsultarComoTecnico;
            bool puedeTomarDiagnostico = !fila.EvaluacionFinalizada && (fila.EstadoValor == EstadoAtencion.PendienteDiagnostico || fila.EstadoValor == EstadoAtencion.EnEvaluacionTecnica) && fila.IdTecnicoEnActividad is null && fila.ActividadTecnica is null;
            bool puedeRegistrarDiagnostico = fila.EstadoValor == EstadoAtencion.EnEvaluacionTecnica && fila.IdTecnicoEnActividad == usuario.Id && fila.ActividadTecnica is ActividadTecnica.Diagnostico or ActividadTecnica.Presupuesto;
            btnRegistrarDiagnostico.Text = puedeRegistrarDiagnostico ? "Continuar" : fila.TipoSolicitud == TipoSolicitudAtencion.ServicioDefinido ? "Preparar presupuesto" : "Diagnosticar y presupuestar";
            btnRegistrarDiagnostico.Enabled = !_tomandoDiagnostico && puedeConsultarComoTecnico && (puedeTomarDiagnostico || puedeRegistrarDiagnostico);
            btnFinalizarTrabajo.Enabled = puedeConsultarComoTecnico && fila.EstadoValor == EstadoAtencion.EnEjecucion && (fila.IdTecnicoEnActividad == null || fila.IdTecnicoEnActividad == usuario.Id);
        }

        private void DeshabilitarAcciones()
        {
            btnSeguimiento.Enabled = false;
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
        private void TxtBuscar_TextChanged(object? sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void CboEstado_SelectedIndexChanged(object? sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void FiltroFecha_ValueChanged(object? sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void LimpiarFiltros_Click(object? sender, EventArgs e)
        {
            if (btnActualizar.Enabled == false) return;
            txtBuscar.Clear();
            ConfigurarFiltros();
            AplicarFiltros();
            txtBuscar.Focus();
        }

        private async void BtnActualizar_Click(object? sender, EventArgs e)
        {
            await CargarAtencionesAsync();
        }

        private void DgvAtenciones_SelectionChanged(object? sender, EventArgs e)
        {
            ActualizarAccionesSeleccion();
        }

        private void DgvAtenciones_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvAtenciones.Columns[e.ColumnIndex].Name != "colEstado")
            {
                return;
            }

            string estado = e.Value?.ToString() ?? string.Empty;
            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            switch (estado)
            {
                case "Abierta":
                    e.CellStyle.ForeColor = Color.FromArgb(30, 64, 175);
                    break;
                case "En proceso":
                    e.CellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                    break;
                case "Cerrada":
                    e.CellStyle.ForeColor = Color.FromArgb(22, 101, 52);
                    break;
                case "Cancelada":
                    e.CellStyle.ForeColor = Color.FromArgb(185, 28, 28);
                    break;
            }
        }

        private void DgvAtenciones_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvAtenciones.Rows[e.RowIndex].DataBoundItem is not AtencionFila fila)
            {
                return;
            }

            string nombreColumna = dgvAtenciones.Columns[e.ColumnIndex].Name;
            if (nombreColumna == "colMotivo")
            {
                e.ToolTipText = fila.Motivo;
            }
            else if (nombreColumna == "colEtapa")
            {
                e.ToolTipText = fila.Etapa;
            }
        }

#endregion
#region Eventos de acciones administrativas
        private async void BtnNuevaAtencion_Click(object? sender, EventArgs e)
        {
            if (_formularioNuevaAtencionAbierto)
            {
                return;
            }

            try
            {
                _formularioNuevaAtencionAbierto = true;
                btnNuevaAtencion.Enabled = false;
                using FrmNuevaAtencion formulario = _serviceProvider.GetRequiredService<FrmNuevaAtencion>();
                DialogResult resultado = formulario.ShowDialog(this);
                if (resultado == DialogResult.OK)
                {
                    await CargarAtencionesAsync();
                }
            }
            finally
            {
                _formularioNuevaAtencionAbierto = false;
                btnNuevaAtencion.Enabled = btnNuevaAtencion.Visible;
            }
        }

        private async void BtnEditar_Click(object? sender, EventArgs e)
        {
            await AbrirEdicionSeleccionada();
        }

        private async void BtnCancelarAtencion_Click(object? sender, EventArgs e)
        {
            await CancelarAperturaSeleccionadaAsync();
        }

        /// <summary>
        /// Abre el formulario de entrega para la atención seleccionada.
        /// </summary>
        private async void BtnRegistrarEntrega_Click(object? sender, EventArgs e)
        {
            var fila = ObtenerFilaSeleccionada();
            if (fila == null || _sesionUsuario.UsuarioActual?.RolId != RolesSistema.AdministrativoId)
                return;
            using var formulario = new Taller.Presentacion.Formularios.Comprobantes.FrmComprobantePagos(fila.IdAtencion, _serviceProvider.GetRequiredService<CobroServicio>(), _serviceProvider.GetRequiredService<Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto>());
            formulario.ShowDialog(this);
            await CargarAtencionesAsync();
        }

#endregion
#region Eventos de consulta y acciones técnicas
        private async void VerSeguimiento(object? sender, EventArgs e)
        {
            if (_cargandoDatos || _tomandoDiagnostico || ObtenerFilaSeleccionada()is not { } fila)
                return;
            _tomandoDiagnostico = true;
            try
            {
                using var f = new FrmSeguimientoTrabajo(fila.IdAtencion, _serviceProvider.GetRequiredService<SeguimientoTrabajoServicio>(), _serviceProvider.GetRequiredService<EvaluacionServicio>(), _serviceProvider.GetRequiredService<PresupuestoServicio>(), _serviceProvider.GetRequiredService<Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto>());
                f.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Seguimiento de atención");
            }
            finally
            {
                _tomandoDiagnostico = false;
                await CargarAtencionesAsync();
            }
        }

        private async void BtnVerDetalle_ClickAsync(object? sender, EventArgs e)
        {
            await AbrirDetalleSeleccionadoAsync();
        }

        private void BtnVerHistorial_Click(object? sender, EventArgs e)
        {
            AbrirHistorialSeleccionado();
        }

        private void BtnRegistrarDiagnostico_Click(object? sender, EventArgs e)
        {
            var fila = ObtenerFilaSeleccionada();
            var usuario = _sesionUsuario.UsuarioActual;
            if (_tomandoDiagnostico || fila is null || usuario?.RolId != RolesSistema.TecnicoId)
            {
                return;
            }

            bool puedeTomar = !fila.EvaluacionFinalizada && (fila.EstadoValor == EstadoAtencion.PendienteDiagnostico || fila.EstadoValor == EstadoAtencion.EnEvaluacionTecnica) && fila.IdTecnicoEnActividad is null && fila.ActividadTecnica is null;
            bool puedeRetomar = fila.EstadoValor == EstadoAtencion.EnEvaluacionTecnica && fila.IdTecnicoEnActividad == usuario.Id && fila.ActividadTecnica is ActividadTecnica.Diagnostico or ActividadTecnica.Presupuesto;
            if (!puedeTomar && !puedeRetomar)
            {
                return;
            }

            DiagnosticoSolicitado?.Invoke(this, new DiagnosticoSolicitadoEventArgs(fila.IdAtencion));
        }

        /// <summary>
        /// Abre el formulario de finalización del trabajo técnico
        /// para la atención seleccionada.
        /// </summary>
        private async void BtnFinalizarTrabajo_Click(object? sender, EventArgs e)
        {
            var fila = ObtenerFilaSeleccionada();
            if (fila == null || _sesionUsuario.UsuarioActual?.RolId != RolesSistema.TecnicoId)
                return;
            using var formulario = new FrmEjecutarAtencion(fila.IdAtencion, _serviceProvider.GetRequiredService<EjecucionServicio>(), _sesionUsuario, _serviceProvider);
            formulario.ShowDialog(this);
            await CargarAtencionesAsync();
        }

#endregion
#region Navegación y mensajes auxiliares
        private async Task AbrirDetalleSeleccionadoAsync()
        {
            AtencionFila? fila = ObtenerFilaSeleccionada();
            if (fila is null)
            {
                MessageBox.Show(this, "Seleccioná una atención para consultar su detalle.", "Detalle de atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario? usuario = _sesionUsuario.UsuarioActual;
            if (usuario is null)
            {
                MessageBox.Show(this, "No existe una sesión de usuario válida.", "Detalle de atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var formulario = new FrmDetalleAtencion(fila.IdAtencion, _atencionServicio, _sesionUsuario, _serviceProvider.GetRequiredService<EvaluacionServicio>(), _serviceProvider);
            formulario.ShowDialog(this);
            await CargarAtencionesAsync();
        }

        /// <summary>
        /// Abre directamente el formulario de registro
        /// de diagnóstico para la atención seleccionada.
        /// </summary>
        private async Task AbrirEdicionSeleccionada()
        {
            AtencionFila? fila = ObtenerFilaSeleccionada();
            if (fila is null)
            {
                MessageBox.Show(this, "Seleccioná una atención para editar sus datos.", "Editar atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var atencion = await _atencionServicio.ObtenerPorIdAsync(fila.IdAtencion);
            if (atencion is null || atencion.Estado != EstadoAtencion.Abierta || atencion.FechaCierre is not null || atencion.IdTecnicoEnActividad is not null || atencion.ActividadTecnica is not null)
            {
                MessageBox.Show(this, "La atención ya no está disponible para editar sus datos. " + "Se actualizará el listado.", "Editar atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarAtencionesAsync();
                return;
            }

            using var formulario = new FrmEditarAtencion(fila.IdAtencion, _atencionServicio);
            DialogResult resultado = formulario.ShowDialog(this);
            if (resultado != DialogResult.OK)
            {
                return;
            }

            await CargarAtencionesAsync();
        }

        private void MostrarFuncionEnDesarrollo(string nombreFuncion)
        {
            MessageBox.Show($"La opción \"{nombreFuncion}\" forma parte del flujo " + "previsto del sistema y será conectada con su lógica " + "de negocio en la siguiente etapa.", nombreFuncion, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task CancelarAperturaSeleccionadaAsync()
        {
            var fila = ObtenerFilaSeleccionada();
            if (fila is null || _cargandoDatos)
                return;
            using var motivo = new Taller.Presentacion.Formularios.Compartidos.FrmSolicitarMotivo("Cancelar apertura", "Solo se cancela una atención abierta que no haya ingresado al circuito técnico. Indique el motivo.");
            if (motivo.ShowDialog(this) != DialogResult.OK)
                return;
            try
            {
                _cargandoDatos = true;
                DeshabilitarAcciones();
                var a = await _atencionServicio.ObtenerPorIdAsync(fila.IdAtencion) ?? throw new InvalidOperationException("La atención ya no existe.");
                await _serviceProvider.GetRequiredService<EvaluacionServicio>().CancelarAperturaAsync(a.IdAtencion, a.Version, motivo.Motivo);
            }
            catch (Exception ex)
            {
                Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Cancelar apertura");
            }
            finally
            {
                _cargandoDatos = false;
            }

            await CargarAtencionesAsync();
        }

        /// <summary>
        /// Abre el historial real del vehículo correspondiente
        /// a la atención seleccionada.
        /// </summary>
        private void AbrirHistorialSeleccionado()
        {
            AtencionFila? fila = ObtenerFilaSeleccionada();
            if (fila is null)
            {
                MessageBox.Show(this, "Seleccioná una atención para consultar el historial del vehículo.", "Historial del vehículo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var formulario = _serviceProvider.GetRequiredService<FrmHistorialVehiculo>();
            formulario.PrepararConsulta(fila.Dominio);
            formulario.ShowDialog(this);
        }

#endregion
        private void DgvAtenciones_CellDoubleClickAsync(object sender, DataGridViewCellEventArgs e)
        {
        }

        private async void BtnIniciarProceso_ClickAsync(object sender, EventArgs e)
        {
            await AbrirDetalleSeleccionadoAsync();
        }

        private static bool EsAtencionFinalizada(EstadoAtencion estado)
        {
            return estado == EstadoAtencion.Cerrada || estado == EstadoAtencion.Cancelada;
        }

        /// <summary>
        /// Determina el grupo de presentación según el rol.
        /// No concede permisos ni reemplaza validaciones del servicio.
        /// </summary>
        private int ObtenerGrupoOperativo(AtencionFila fila)
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
                    EstadoAtencion.EnEvaluacionTecnica when fila.EvaluacionFinalizada => 0,
                    EstadoAtencion.PendienteDecision => 0,
                    EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion => 1,
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
                if (fila.EstadoValor == EstadoAtencion.PendienteDiagnostico && fila.IdTecnicoEnActividad is null)
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

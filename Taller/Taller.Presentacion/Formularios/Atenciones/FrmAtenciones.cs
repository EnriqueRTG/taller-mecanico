using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Estilos;
using Taller.Presentacion.Seguridad;

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
            ConfigurarEventos();
        }

        private sealed class AtencionFila
        {
            public int IdAtencion { get; init; }

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
        }

        private void ConfigurarEventos()
        {
            Load += FrmAtenciones_Load;

            btnNuevaAtencion.Click +=
                btnNuevaAtencion_Click;

            btnActualizar.Click +=
                btnActualizar_Click;

            txtBuscar.TextChanged +=
                txtBuscar_TextChanged;

            cboEstado.SelectedIndexChanged +=
                cboEstado_SelectedIndexChanged;

            dtpDesde.ValueChanged +=
                filtroFecha_ValueChanged;

            dtpHasta.ValueChanged +=
                filtroFecha_ValueChanged;

            dgvAtenciones.SelectionChanged +=
                dgvAtenciones_SelectionChanged;

            dgvAtenciones.CellDoubleClick +=
                dgvAtenciones_CellDoubleClick;

            dgvAtenciones.CellFormatting +=
                dgvAtenciones_CellFormatting;

            dgvAtenciones.CellToolTipTextNeeded +=
                dgvAtenciones_CellToolTipTextNeeded;

            btnVerDetalle.Click +=
                btnVerDetalle_Click;

            btnEditar.Click +=
                btnEditar_Click;

            btnIniciarProceso.Click +=
                btnIniciarProceso_Click;

            btnCancelarAtencion.Click +=
                btnCancelarAtencion_Click;

            btnRegistrarEntrega.Click +=
                btnRegistrarEntrega_Click;

            btnVerHistorial.Click +=
                btnVerHistorial_Click;

            btnRegistrarDiagnostico.Click +=
                btnRegistrarDiagnostico_Click;

            btnFinalizarTrabajo.Click +=
                btnFinalizarTrabajo_Click;
        }

        private void ConfigurarFiltros()
        {
            cboEstado.Items.Clear();

            cboEstado.Items.Add("Activas");
            cboEstado.Items.Add("Todas");
            cboEstado.Items.Add("Abierta");
            cboEstado.Items.Add("En proceso");
            cboEstado.Items.Add("Cerrada");
            cboEstado.Items.Add("Cancelada");

            dtpDesde.Value =
                DateTime.Today.AddMonths(-1);

            dtpHasta.Value =
                DateTime.Today;

            cboEstado.SelectedIndex = 0;
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
                    await _atencionServicio.ListarActivasAsync();

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
                Fecha = atencion.FechaApertura,
                Dominio = atencion.Vehiculo.Dominio,

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
                EstadoAtencion.Abierta =>
                    "Abierta",

                EstadoAtencion.EnProceso =>
                    "En proceso",

                EstadoAtencion.Cerrada =>
                    "Cerrada",

                EstadoAtencion.Cancelada =>
                    "Cancelada",

                _ =>
                    "Desconocido"
            };
        }

        private static string ObtenerEtapaTemporal(
            Atencion atencion)
        {
            return atencion.Estado switch
            {
                EstadoAtencion.Abierta =>
                    "Ingreso registrado",

                EstadoAtencion.EnProceso =>
                    "Pendiente de diagnóstico",

                EstadoAtencion.Cerrada =>
                    "Vehículo entregado",

                EstadoAtencion.Cancelada =>
                    "Atención cancelada",

                _ =>
                    "Sin determinar"
            };
        }
        #endregion

        #region Filtros y selección
        private void AplicarFiltros()
        {
            IEnumerable<AtencionFila> consulta =
                _atenciones;

            string texto =
                txtBuscar.Text.Trim();

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

            string estado =
                cboEstado.SelectedItem?.ToString()
                ?? "Activas";

            switch (estado)
            {
                case "Abierta":
                    consulta = consulta.Where(a =>
                        a.EstadoValor == EstadoAtencion.Abierta);
                    break;

                case "En proceso":
                    consulta = consulta.Where(a =>
                        a.EstadoValor == EstadoAtencion.EnProceso);
                    break;

                case "Cerrada":
                    consulta = consulta.Where(a =>
                        a.EstadoValor == EstadoAtencion.Cerrada);
                    break;

                case "Cancelada":
                    consulta = consulta.Where(a =>
                        a.EstadoValor == EstadoAtencion.Cancelada);
                    break;

                case "Activas":
                    consulta = consulta.Where(a =>
                        a.EstadoValor == EstadoAtencion.Abierta
                        ||
                        a.EstadoValor == EstadoAtencion.EnProceso);
                    break;
            }

            DateTime desde =
                dtpDesde.Value.Date;

            DateTime hasta =
                dtpHasta.Value.Date.AddDays(1);

            consulta = consulta.Where(a =>
                a.Fecha >= desde
                &&
                a.Fecha < hasta);

            List<AtencionFila> resultado =
                consulta
                    .OrderByDescending(a => a.Fecha)
                    .ToList();

            dgvAtenciones.DataSource = null;
            dgvAtenciones.DataSource = resultado;

            dgvAtenciones.ClearSelection();
            dgvAtenciones.CurrentCell = null;

            lblCantidad.Text =
                resultado.Count == 1
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

            bool estaActiva =
                fila.EstadoValor == EstadoAtencion.Abierta
                ||
                fila.EstadoValor == EstadoAtencion.EnProceso;

            btnVerDetalle.Enabled =
                puedeGestionar
                || puedeConsultarComoTecnico;

            btnEditar.Enabled =
                puedeGestionar
                && estaActiva;

            btnIniciarProceso.Enabled =
                puedeGestionar
                && fila.EstadoValor == EstadoAtencion.Abierta;

            btnCancelarAtencion.Enabled =
                puedeGestionar
                && estaActiva;

            btnRegistrarEntrega.Enabled =
                puedeGestionar
                && fila.Etapa == "Preparada para entrega";

            btnVerHistorial.Enabled =
                puedeConsultarComoTecnico;

            btnRegistrarDiagnostico.Enabled =
                puedeConsultarComoTecnico
                && fila.Etapa == "Pendiente de diagnóstico";

            btnFinalizarTrabajo.Enabled =
                puedeConsultarComoTecnico
                && fila.Etapa == "Trabajo autorizado";
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
        private void txtBuscar_TextChanged(
            object? sender,
            EventArgs e)
        {
            AplicarFiltros();
        }

        private void cboEstado_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            AplicarFiltros();
        }

        private void filtroFecha_ValueChanged(
            object? sender,
            EventArgs e)
        {
            AplicarFiltros();
        }

        private async void btnActualizar_Click(
            object? sender,
            EventArgs e)
        {
            await CargarAtencionesAsync();
        }

        private void dgvAtenciones_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            ActualizarAccionesSeleccion();
        }

        private void dgvAtenciones_CellDoubleClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            AbrirDetalleSeleccionado();
        }

        private void dgvAtenciones_CellFormatting(
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


        private void dgvAtenciones_CellToolTipTextNeeded(
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
        private async void btnNuevaAtencion_Click(
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

        private void btnEditar_Click(
            object? sender,
            EventArgs e)
        {
            MostrarFuncionEnDesarrollo(
                "Editar atención");
        }

        private void btnIniciarProceso_Click(
            object? sender,
            EventArgs e)
        {
            MostrarFuncionEnDesarrollo(
                "Iniciar proceso");
        }

        private void btnCancelarAtencion_Click(
            object? sender,
            EventArgs e)
        {
            MostrarFuncionEnDesarrollo(
                "Cancelar atención");
        }

        private void btnRegistrarEntrega_Click(
            object? sender,
            EventArgs e)
        {
            MostrarFuncionEnDesarrollo(
                "Registrar entrega");
        }
        #endregion

        #region Eventos de consulta y acciones técnicas
        private void btnVerDetalle_Click(
            object? sender,
            EventArgs e)
        {
            AbrirDetalleSeleccionado();
        }

        private void btnVerHistorial_Click(
            object? sender,
            EventArgs e)
        {
            MostrarFuncionEnDesarrollo(
                "Historial del vehículo");
        }

        private void btnRegistrarDiagnostico_Click(
            object? sender,
            EventArgs e)
        {
            AbrirDetalleSeleccionado();
        }

        private void btnFinalizarTrabajo_Click(
            object? sender,
            EventArgs e)
        {
            MostrarFuncionEnDesarrollo(
                "Finalizar trabajo");
        }
        #endregion

        #region Navegación y mensajes auxiliares
        private void AbrirDetalleSeleccionado()
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
                new FrmDetalleAtencion(usuario.RolId);

            formulario.ShowDialog(this);
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
        #endregion
    }
}

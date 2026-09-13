using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Formularios.Usuarios;
using Taller.Presentacion.Seguridad;


namespace Taller.Presentacion.Formularios.Principal;

/// <summary>
/// Representa la ventana principal del sistema después
/// de que el usuario inicia sesión correctamente.
/// </summary>
public partial class FrmPrincipal : Form
{
    private readonly SesionUsuario _sesionUsuario;
    public event EventHandler? CerrarSesionSolicitada;
    public event EventHandler? SalirSolicitado;
    private readonly IServiceProvider _serviceProvider;
    private readonly AutorizacionNavegacion _autorizacionNavegacion;
    private Form? _formularioActivo;
    /// <summary>
    /// Inicializa el formulario principal utilizando
    /// la sesión del usuario autenticado.
    /// </summary>
    public FrmPrincipal(
    SesionUsuario sesionUsuario,
    IServiceProvider serviceProvider,
    AutorizacionNavegacion autorizacionNavegacion)
    {
        InitializeComponent();

        _sesionUsuario = sesionUsuario;
        _serviceProvider = serviceProvider;
        _autorizacionNavegacion = autorizacionNavegacion;
    }

    /// <summary>
    /// Configura el formulario utilizando los datos
    /// del usuario actualmente autenticado.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        MostrarDatosSesion();

        if (!_sesionUsuario.EstaAutenticado)
        {
            return;
        }

        ConfigurarMenuSegunRol();

        AbrirFormulario<FrmInicio>("Inicio");

    }

    protected override void OnFormClosed(
    FormClosedEventArgs e)
    {
        _formularioActivo?.Close();
        _formularioActivo?.Dispose();
        _formularioActivo = null;

        base.OnFormClosed(e);
    }

    /// <summary>
    /// 
    /// </summary>
    private void MostrarDatosSesion()
    {
        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            MessageBox.Show(
                "No existe un usuario autenticado.",
                "Sesión no válida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            Close();
            return;
        }

        lblNombreUsuario.Text = $"{usuario.Nombre} {usuario.Apellido}";

        lblRol.Text = usuario.Rol.Nombre;
    }

    private void AbrirFormulario<TFormulario>(
    string titulo)
    where TFormulario : Form
    {
        if (_formularioActivo?.GetType() == typeof(TFormulario))
        {
            return;
        }

        _formularioActivo?.Close();
        _formularioActivo?.Dispose();

        pnlContenido.Controls.Clear();

        var formulario =
            _serviceProvider
                .GetRequiredService<TFormulario>();

        formulario.TopLevel = false;
        formulario.FormBorderStyle = FormBorderStyle.None;
        formulario.Dock = DockStyle.Fill;

        pnlContenido.Controls.Add(formulario);

        _formularioActivo = formulario;

        lblTituloSeccion.Text = titulo;

        formulario.Show();
        formulario.BringToFront();
    }

    private void ConfigurarMenuSegunRol()
    {
        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            OcultarOpcionesProtegidas();
            return;
        }

        var rolId = usuario.RolId;

        btnUsuarios.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarUsuarios);

        btnClientes.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarClientes);

        btnVehiculos.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarVehiculos);

        btnAtenciones.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarAtenciones)
            ||
            TienePermiso(
                rolId,
                PermisoAplicacion.ConsultarAtencionesAsignadas);

        btnDiagnosticos.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.ConsultarDiagnosticos)
            ||
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarDiagnosticos);

        btnPresupuestos.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarDecisionPresupuesto)
            ||
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarPresupuestosTecnicos);

        btnTrabajos.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.RegistrarEjecucionTrabajo);

        btnComprobantes.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarComprobantesPagos);

        btnEntregas.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarEntregas);

        btnReportes.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.ConsultarReportesGenerales)
            ||
            TienePermiso(
                rolId,
                PermisoAplicacion.ConsultarReportesOperativos)
            ||
            TienePermiso(
                rolId,
                PermisoAplicacion.ConsultarReportesTecnicos);

        btnRespaldo.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarRespaldo);
    }

    private bool TienePermiso(
    int rolId,
    PermisoAplicacion permiso)
    {
        return _autorizacionNavegacion
            .TienePermiso(rolId, permiso);
    }

    private void OcultarOpcionesProtegidas()
    {
        btnUsuarios.Visible = false;
        btnClientes.Visible = false;
        btnVehiculos.Visible = false;
        btnAtenciones.Visible = false;
        btnDiagnosticos.Visible = false;
        btnPresupuestos.Visible = false;
        btnTrabajos.Visible = false;
        btnComprobantes.Visible = false;
        btnEntregas.Visible = false;
        btnReportes.Visible = false;
        btnRespaldo.Visible = false;
    }

    private void btnInicio_Click(
    object sender,
    EventArgs e)
{
    AbrirFormulario<FrmInicio>("Inicio");
    MarcarBotonActivo(btnInicio);
}

    private void btnCerrarSesion_Click(
    object sender,
    EventArgs e)
    {
        var respuesta = MessageBox.Show(
            "¿Desea cerrar la sesión actual?",
            "Cerrar sesión",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (respuesta == DialogResult.Yes)
        {
            CerrarSesionSolicitada?.Invoke(
                this,
                EventArgs.Empty);
        }
    }

    private void btnSalir_Click(
        object sender,
        EventArgs e)
    {
        var respuesta = MessageBox.Show(
            "¿Desea salir de la aplicación?",
            "Salir",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (respuesta == DialogResult.Yes)
        {
            SalirSolicitado?.Invoke(
                this,
                EventArgs.Empty);
        }
    }

    private void btnUsuarios_Click(
    object sender,
    EventArgs e)
    {
        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            MessageBox.Show(
                "No existe una sesión de usuario válida.",
                "Sesión no válida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        if (!TienePermiso(
            usuario.RolId,
            PermisoAplicacion.GestionarUsuarios))
        {
            MessageBox.Show(
                "No tiene permisos para acceder a la gestión de usuarios.",
                "Acceso restringido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        AbrirFormulario<FrmUsuarios>("Usuarios");
        MarcarBotonActivo(btnUsuarios);
    }

    private void MarcarBotonActivo(Button botonActivo)
    {
        foreach (Control control in pnlOpciones.Controls)
        {
            if (control is not Button boton)
                continue;

            boton.BackColor = Color.FromArgb(15, 23, 42);
            boton.ForeColor = Color.White;
            boton.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        }

        botonActivo.BackColor = Color.FromArgb(30, 64, 175);
        botonActivo.ForeColor = Color.White;
        botonActivo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
    }
}

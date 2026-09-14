using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Formularios.Atenciones;
using Taller.Presentacion.Formularios.Clientes;
using Taller.Presentacion.Formularios.Comprobantes;
using Taller.Presentacion.Formularios.Diagnosticos;
using Taller.Presentacion.Formularios.Presupuestos;
using Taller.Presentacion.Formularios.Reportes;
using Taller.Presentacion.Formularios.Sistema;
using Taller.Presentacion.Formularios.Usuarios;
using Taller.Presentacion.Formularios.Vehiculos;
using Taller.Presentacion.Seguridad;

namespace Taller.Presentacion.Formularios.Principal;

/// <summary>
/// Representa la ventana principal del sistema después
/// de que el usuario inicia sesión correctamente.
/// </summary>
public partial class FrmPrincipal : Form
{
    private readonly SesionUsuario _sesionUsuario;
    private readonly IServiceProvider _serviceProvider;
    private readonly AutorizacionNavegacion _autorizacionNavegacion;

    private Form? _formularioActivo;

    public event EventHandler? CerrarSesionSolicitada;
    public event EventHandler? SalirSolicitado;

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
        MarcarBotonActivo(btnInicio);
    }

    /// <summary>
    /// Libera el formulario interno que se encuentre abierto.
    /// </summary>
    protected override void OnFormClosed(
        FormClosedEventArgs e)
    {
        _formularioActivo?.Close();
        _formularioActivo?.Dispose();
        _formularioActivo = null;

        base.OnFormClosed(e);
    }

    /// <summary>
    /// Muestra los datos del usuario autenticado.
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

        lblNombreUsuario.Text =
            $"{usuario.Nombre} {usuario.Apellido}";

        lblRol.Text = usuario.Rol.Nombre;
    }

    /// <summary>
    /// Abre un formulario dentro del panel principal.
    /// </summary>
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
        _formularioActivo = null;

        pnlContenido.Controls.Clear();

        var formulario = _serviceProvider
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

    /// <summary>
    /// Abre un formulario únicamente cuando el usuario posee
    /// al menos uno de los permisos requeridos.
    /// </summary>
    private void AbrirFormularioAutorizado<TFormulario>(
        string titulo,
        Button botonActivo,
        string mensajeAccesoRestringido,
        params PermisoAplicacion[] permisos)
        where TFormulario : Form
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

        bool autorizado = permisos.Any(
            permiso => TienePermiso(
                usuario.RolId,
                permiso));

        if (!autorizado)
        {
            MessageBox.Show(
                mensajeAccesoRestringido,
                "Acceso restringido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        AbrirFormulario<TFormulario>(titulo);
        MarcarBotonActivo(botonActivo);
    }

    /// <summary>
    /// Configura la visibilidad de las opciones según el rol.
    /// </summary>
    private void ConfigurarMenuSegunRol()
    {
        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            OcultarOpcionesProtegidas();
            return;
        }

        int rolId = usuario.RolId;

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

        btnComprobantes.Visible =
            TienePermiso(
                rolId,
                PermisoAplicacion.GestionarComprobantesPagos);

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

    /// <summary>
    /// Comprueba si un rol posee el permiso solicitado.
    /// </summary>
    private bool TienePermiso(
        int rolId,
        PermisoAplicacion permiso)
    {
        return _autorizacionNavegacion
            .TienePermiso(rolId, permiso);
    }

    /// <summary>
    /// Oculta todas las opciones que requieren autorización.
    /// </summary>
    private void OcultarOpcionesProtegidas()
    {
        btnUsuarios.Visible = false;
        btnClientes.Visible = false;
        btnVehiculos.Visible = false;
        btnAtenciones.Visible = false;
        btnDiagnosticos.Visible = false;
        btnPresupuestos.Visible = false;
        btnComprobantes.Visible = false;
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

    private void btnUsuarios_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmUsuarios>(
            "Usuarios",
            btnUsuarios,
            "No tiene permisos para acceder a la gestión de usuarios.",
            PermisoAplicacion.GestionarUsuarios);
    }

    private void btnClientes_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmClientes>(
            "Clientes",
            btnClientes,
            "No tiene permisos para acceder a la gestión de clientes.",
            PermisoAplicacion.GestionarClientes);
    }

    private void btnVehiculos_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmVehiculos>(
            "Vehículos",
            btnVehiculos,
            "No tiene permisos para acceder a la gestión de vehículos.",
            PermisoAplicacion.GestionarVehiculos);
    }

    private void btnAtenciones_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmAtenciones>(
            "Atenciones",
            btnAtenciones,
            "No tiene permisos para acceder a las atenciones.",
            PermisoAplicacion.GestionarAtenciones,
            PermisoAplicacion.ConsultarAtencionesAsignadas);
    }

    private void btnDiagnosticos_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmDiagnosticos>(
            "Diagnósticos",
            btnDiagnosticos,
            "No tiene permisos para acceder a los diagnósticos.",
            PermisoAplicacion.ConsultarDiagnosticos,
            PermisoAplicacion.GestionarDiagnosticos);
    }

    private void btnPresupuestos_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmPresupuestos>(
            "Presupuestos",
            btnPresupuestos,
            "No tiene permisos para acceder a los presupuestos.",
            PermisoAplicacion.GestionarDecisionPresupuesto,
            PermisoAplicacion.GestionarPresupuestosTecnicos);
    }

    private void btnComprobantes_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmComprobantes>(
            "Comprobantes y pagos",
            btnComprobantes,
            "No tiene permisos para acceder a comprobantes y pagos.",
            PermisoAplicacion.GestionarComprobantesPagos);
    }

    private void btnReportes_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmReportes>(
            "Reportes",
            btnReportes,
            "No tiene permisos para acceder a los reportes.",
            PermisoAplicacion.ConsultarReportesGenerales,
            PermisoAplicacion.ConsultarReportesOperativos,
            PermisoAplicacion.ConsultarReportesTecnicos);
    }

    private void btnRespaldo_Click(
        object sender,
        EventArgs e)
    {
        AbrirFormularioAutorizado<FrmRespaldoRestauracion>(
            "Respaldo y restauración",
            btnRespaldo,
            "No tiene permisos para acceder a respaldo y restauración.",
            PermisoAplicacion.GestionarRespaldo);
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

    /// <summary>
    /// Resalta visualmente la opción actualmente seleccionada.
    /// </summary>
    private void MarcarBotonActivo(
        Button botonActivo)
    {
        foreach (Control control in pnlOpciones.Controls)
        {
            if (control is not Button boton)
            {
                continue;
            }

            boton.BackColor =
                Color.FromArgb(15, 23, 42);

            boton.ForeColor = Color.White;

            boton.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Regular);
        }

        botonActivo.BackColor =
            Color.FromArgb(30, 64, 175);

        botonActivo.ForeColor = Color.White;

        botonActivo.Font = new Font(
            "Segoe UI",
            9.5F,
            FontStyle.Bold);
    }
}
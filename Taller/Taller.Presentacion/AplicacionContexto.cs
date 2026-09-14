using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Formularios.Autenticacion;
using Taller.Presentacion.Formularios.Principal;

namespace Taller.Presentacion;

/// <summary>
/// Controla el ciclo de vida general de la interfaz de usuario:
/// autenticación, apertura del formulario principal, cierre de
/// sesión y finalización de la aplicación.
/// </summary>
/// <remarks>
/// También administra los alcances del contenedor de dependencias.
/// Cada formulario de acceso utiliza un alcance temporal, mientras
/// que el formulario principal mantiene su propio alcance durante
/// toda la sesión visual del usuario.
/// </remarks>
public sealed class AplicacionContexto : ApplicationContext
{
    private readonly IServiceProvider _proveedorServicios;
    private readonly SesionUsuario _sesionUsuario;

    private IServiceScope? _alcanceFormularioPrincipal;
    private FrmPrincipal? _formularioPrincipal;

    /// <summary>
    /// Inicializa el contexto general de la aplicación.
    /// </summary>
    /// <param name="proveedorServicios">
    /// Proveedor raíz utilizado para crear los alcances
    /// de inyección de dependencias.
    /// </param>
    /// <param name="sesionUsuario">
    /// Servicio que conserva al usuario autenticado.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando alguna dependencia requerida es nula.
    /// </exception>
    public AplicacionContexto(
        IServiceProvider proveedorServicios,
        SesionUsuario sesionUsuario)
    {
        _proveedorServicios =
            proveedorServicios
            ?? throw new ArgumentNullException(
                nameof(proveedorServicios));

        _sesionUsuario =
            sesionUsuario
            ?? throw new ArgumentNullException(
                nameof(sesionUsuario));

        Application.Idle += Application_Idle;
    }

    /// <summary>
    /// Inicia el flujo de la interfaz cuando la aplicación
    /// queda disponible para procesar eventos.
    /// </summary>
    private void Application_Idle(
        object? sender,
        EventArgs e)
    {
        Application.Idle -= Application_Idle;

        MostrarLogin();
    }

    /// <summary>
    /// Muestra el formulario de acceso utilizando un alcance
    /// temporal de dependencias.
    /// </summary>
    /// <remarks>
    /// Si el usuario cancela o cierra el formulario sin
    /// autenticarse, finaliza la aplicación.
    /// </remarks>
    private void MostrarLogin()
    {
        using IServiceScope alcanceLogin =
            _proveedorServicios.CreateScope();

        using FrmLogin login =
            alcanceLogin.ServiceProvider
                .GetRequiredService<FrmLogin>();

        DialogResult resultado =
            login.ShowDialog();

        if (resultado != DialogResult.OK
            || !_sesionUsuario.EstaAutenticado)
        {
            _sesionUsuario.Cerrar();
            ExitThread();
            return;
        }

        MostrarFormularioPrincipal();
    }

    /// <summary>
    /// Crea y muestra el formulario principal dentro de
    /// un nuevo alcance de dependencias.
    /// </summary>
    private void MostrarFormularioPrincipal()
    {
        LiberarFormularioPrincipal();

        _alcanceFormularioPrincipal =
            _proveedorServicios.CreateScope();

        _formularioPrincipal =
            _alcanceFormularioPrincipal
                .ServiceProvider
                .GetRequiredService<FrmPrincipal>();

        _formularioPrincipal.CerrarSesionSolicitada +=
            FormularioPrincipal_CerrarSesionSolicitada;

        _formularioPrincipal.SalirSolicitado +=
            FormularioPrincipal_SalirSolicitado;

        _formularioPrincipal.FormClosed +=
            FormularioPrincipal_FormClosed;

        _formularioPrincipal.Show();
    }

    /// <summary>
    /// Atiende la solicitud de cierre de sesión generada
    /// desde el formulario principal.
    /// </summary>
    private void FormularioPrincipal_CerrarSesionSolicitada(
        object? sender,
        EventArgs e)
    {
        CerrarFormularioPrincipal();

        _sesionUsuario.Cerrar();

        MostrarLogin();
    }

    /// <summary>
    /// Atiende la solicitud de finalización completa
    /// de la aplicación.
    /// </summary>
    private void FormularioPrincipal_SalirSolicitado(
        object? sender,
        EventArgs e)
    {
        CerrarFormularioPrincipal();

        _sesionUsuario.Cerrar();

        ExitThread();
    }

    /// <summary>
    /// Finaliza la aplicación cuando el formulario principal
    /// se cierra directamente, por ejemplo mediante el botón
    /// de cierre de la ventana.
    /// </summary>
    private void FormularioPrincipal_FormClosed(
        object? sender,
        FormClosedEventArgs e)
    {
        LiberarFormularioPrincipal();

        _sesionUsuario.Cerrar();

        ExitThread();
    }

    /// <summary>
    /// Cierra el formulario principal evitando que su evento
    /// de cierre finalice involuntariamente la aplicación.
    /// </summary>
    private void CerrarFormularioPrincipal()
    {
        if (_formularioPrincipal is null)
        {
            return;
        }

        _formularioPrincipal.FormClosed -=
            FormularioPrincipal_FormClosed;

        _formularioPrincipal.Close();

        LiberarFormularioPrincipal();
    }

    /// <summary>
    /// Desvincula los eventos y libera el formulario principal
    /// junto con su alcance de dependencias.
    /// </summary>
    private void LiberarFormularioPrincipal()
    {
        if (_formularioPrincipal is not null)
        {
            _formularioPrincipal.CerrarSesionSolicitada -=
                FormularioPrincipal_CerrarSesionSolicitada;

            _formularioPrincipal.SalirSolicitado -=
                FormularioPrincipal_SalirSolicitado;

            _formularioPrincipal.FormClosed -=
                FormularioPrincipal_FormClosed;

            _formularioPrincipal.Dispose();
            _formularioPrincipal = null;
        }

        _alcanceFormularioPrincipal?.Dispose();
        _alcanceFormularioPrincipal = null;
    }

    /// <summary>
    /// Libera los formularios, la sesión y los alcances
    /// pendientes antes de terminar el hilo principal.
    /// </summary>
    protected override void ExitThreadCore()
    {
        Application.Idle -= Application_Idle;

        LiberarFormularioPrincipal();
        _sesionUsuario.Cerrar();

        base.ExitThreadCore();
    }
}
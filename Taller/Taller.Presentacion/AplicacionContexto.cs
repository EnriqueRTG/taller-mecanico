using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Formularios.Autenticacion;
using Taller.Presentacion.Formularios.Principal;

namespace Taller.Presentacion;

/// <summary>
/// Controla el ciclo de vida general de la interfaz:
/// inicio de sesión, formulario principal y cierre de sesión.
/// </summary>
public sealed class AplicacionContexto : ApplicationContext
{
    private readonly IServiceProvider _proveedorServicio;
    private readonly SesionUsuario _sesionUsuario;

    private FrmPrincipal? _formularioPrincipal;

    public AplicacionContexto(
        IServiceProvider serviceProvider,
        SesionUsuario sesionUsuario)
    {
        _proveedorServicio = serviceProvider;
        _sesionUsuario = sesionUsuario;

        Application.Idle += Application_Idle;
    }

    private void Application_Idle(object? sender, EventArgs e)
    {
        Application.Idle -= Application_Idle;

        MostrarLogin();
    }

    private void MostrarLogin()
    {
        using var login =
            _proveedorServicio.GetRequiredService<FrmLogin>();

        var resultado = login.ShowDialog();

        if (resultado != DialogResult.OK)
        {
            ExitThread();
            return;
        }

        MostrarFormularioPrincipal();
    }

    private void MostrarFormularioPrincipal()
    {
        _formularioPrincipal = _proveedorServicio.GetRequiredService<FrmPrincipal>();

        _formularioPrincipal.CerrarSesionSolicitada
            += FormularioPrincipal_CerrarSesionSolicitada;

        _formularioPrincipal.SalirSolicitado
            += FormularioPrincipal_SalirSolicitado;

        _formularioPrincipal.FormClosed
            += FormularioPrincipal_FormClosed;

        _formularioPrincipal.Show();
    }

    private void FormularioPrincipal_CerrarSesionSolicitada(
        object? sender,
        EventArgs e)
    {
        CerrarFormularioPrincipal();

        _sesionUsuario.Cerrar();

        MostrarLogin();
    }

    private void FormularioPrincipal_SalirSolicitado(
        object? sender,
        EventArgs e)
    {
        CerrarFormularioPrincipal();

        _sesionUsuario.Cerrar();

        ExitThread();
    }

    private void FormularioPrincipal_FormClosed(
        object? sender,
        FormClosedEventArgs e)
    {
        LiberarFormularioPrincipal();

        _sesionUsuario.Cerrar();

        ExitThread();
    }

    private void CerrarFormularioPrincipal()
    {
        if (_formularioPrincipal is null)
        {
            return;
        }

        _formularioPrincipal.FormClosed
            -= FormularioPrincipal_FormClosed;

        _formularioPrincipal.Close();

        LiberarFormularioPrincipal();
    }

    private void LiberarFormularioPrincipal()
    {
        if (_formularioPrincipal is null)
        {
            return;
        }

        _formularioPrincipal.CerrarSesionSolicitada
            -= FormularioPrincipal_CerrarSesionSolicitada;

        _formularioPrincipal.SalirSolicitado
            -= FormularioPrincipal_SalirSolicitado;

        _formularioPrincipal.FormClosed
            -= FormularioPrincipal_FormClosed;

        _formularioPrincipal.Dispose();
        _formularioPrincipal = null;
    }
}
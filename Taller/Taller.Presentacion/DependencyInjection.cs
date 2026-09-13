using Microsoft.Extensions.DependencyInjection;
using Taller.Presentacion.Formularios.Atenciones;
using Taller.Presentacion.Formularios.Autenticacion;
using Taller.Presentacion.Formularios.Clientes;
using Taller.Presentacion.Formularios.Principal;
using Taller.Presentacion.Formularios.Usuarios;
using Taller.Presentacion.Formularios.Vehiculos;
using Taller.Presentacion.Seguridad;

namespace Taller.Presentacion;

/// <summary>
/// Registra los formularios y servicios pertenecientes
/// a la capa de Presentación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega al contenedor de dependencias los componentes
    /// utilizados por la interfaz de usuario.
    /// </summary>
    public static IServiceCollection AddPresentacion(
        this IServiceCollection services)
    {
        services.AddTransient<FrmLogin>();

        services.AddTransient<FrmPrincipal>();
        services.AddTransient<FrmInicio>();

        services.AddTransient<FrmUsuarios>();
        services.AddTransient<FrmAltaUsuario>();

        services.AddTransient<FrmNuevaAtencion>();
        services.AddTransient<FrmAltaCliente>();
        services.AddTransient<FrmAltaVehiculo>();

        services.AddSingleton<AutorizacionNavegacion>();


        services.AddTransient<AplicacionContexto>();


        return services;
    }
}
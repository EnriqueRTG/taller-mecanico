using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;

namespace Taller.Aplicacion;

/// <summary>
/// Clase de extensión para registrar los servicios de la capa de aplicación en el contenedor de dependencias.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de la capa de aplicación en el contenedor de dependencias.
    /// </summary>
    /// <param name="services"></param>
    /// <returns>El contenedor de dependencias con los servicios registrados.</returns>
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        services.AddScoped<UsuarioServicio>();
        services.AddScoped<AutenticacionServicio>();
        services.AddScoped<ClienteServicio>();
        services.AddScoped<VehiculoServicio>();
        services.AddScoped<AtencionServicio>();

        return services;
    }

}
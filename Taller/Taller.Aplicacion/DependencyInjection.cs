using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;

namespace Taller.Aplicacion;

public static class DependencyInjection
{
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        services.AddScoped<UsuarioServicio>();
        services.AddScoped<AutenticacionServicio>();

        return services;
    }

}
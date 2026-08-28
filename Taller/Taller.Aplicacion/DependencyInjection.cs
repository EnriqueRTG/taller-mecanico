using Microsoft.Extensions.DependencyInjection;

namespace Taller.Aplicacion;

public static class DependencyInjection
{
    public static IServiceCollection AddAplicacion(
        this IServiceCollection services)
    {
        return services;
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taller.Infraestructura.Persistencia;

namespace Taller.Infraestructura;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructura(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'Default'.");

        services.AddDbContext<TallerDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Inicializacion;
using Taller.Infraestructura.Persistencia.Repositorios;
using Taller.Infraestructura.Seguridad;

namespace Taller.Infraestructura;

/// <summary>
/// Clase de extensión para registrar los servicios de la capa de infraestructura en el contenedor de dependencias.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de la capa de infraestructura en el contenedor de dependencias.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns>El contenedor de dependencias con los servicios registrados.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection AddInfraestructura(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Obtiene la conexión desde una variable de entorno, si existe, o desde los archivos de configuración de la aplicación.
        var connectionString =
            Environment.GetEnvironmentVariable("SGTM_CONN")
            ?? configuration.GetConnectionString("Default");

        // Evita iniciar la aplicación con una cadena nula, vacía o compuesta únicamente por espacios.
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                            "No se encontró una cadena de conexión válida para SGTM.");
        }

        // Configura Entity Framework Core para trabajar con SQL Server mediante TallerDbContext.
        services.AddDbContext<TallerDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Registra el repositorio concreto utilizado para
        // las operaciones de persistencia de usuarios.
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

        // Registra el servicio encargado de generar
        // y verificar hashes de contraseñas.
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Registra el servicio concreto utilizado para las operaciones de persistencia de clientes.
        services.AddScoped<IClienteRepositorio, ClienteRepositorio>();

        // Registra el servicio responsable de crear los datos
        // mínimos necesarios para el primer inicio del sistema.
        services.AddScoped<InicializadorDatos>();


        return services;
    }
}
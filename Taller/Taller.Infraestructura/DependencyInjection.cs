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
    /// <param name = "services"></param>
    /// <param name = "configuration"></param>
    /// <returns>El contenedor de dependencias con los servicios registrados.</returns>
    /// <exception cref = "InvalidOperationException"></exception>
    public static IServiceCollection AddInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        // Obtiene la conexión desde una variable de entorno, si existe, o desde los archivos de configuración de la aplicación.
        var connectionString = Environment.GetEnvironmentVariable("SGTM_CONN") ?? configuration.GetConnectionString("Default");
        // Evita iniciar la aplicación con una cadena nula, vacía o compuesta únicamente por espacios.
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("No se encontró una cadena de conexión válida para SGTM.");
        }

        // Configura Entity Framework Core para trabajar con SQL Server mediante TallerDbContext.
        var respaldos = Taller.Infraestructura.Respaldos.OpcionesRespaldos.Desde(configuration, connectionString);
        services.AddSingleton(respaldos);
        services.AddSingleton<Taller.Infraestructura.Respaldos.GeneracionMantenimiento>();
        services.AddSingleton<Taller.Infraestructura.Respaldos.IntercepcionMantenimiento>();
        services.AddSingleton<Taller.Infraestructura.Respaldos.RegistroRespaldosSql>();
        services.AddSingleton<IArchivoRespaldoNube, Taller.Infraestructura.Respaldos.ArchivoRespaldoAzure>();
        services.AddSingleton<IRespaldoMotor, Taller.Infraestructura.Respaldos.RespaldoSqlServer>();
        services.AddDbContext<TallerDbContext>((proveedor, options) =>
        {
            // Cerrar una operación debe cerrar también la sesión física: el pool dejaría
            // conexiones dormidas que impedirían entrar a SINGLE_USER sin forzarlas.
            var conexionNegocio = respaldos.Habilitado ? new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString)
            {
                Pooling = false
            }.ConnectionString : connectionString;
            options.UseSqlServer(conexionNegocio);
            options.UsarSemillaCatalogo();
            if (respaldos.Habilitado)
                options.AddInterceptors(proveedor.GetRequiredService<Taller.Infraestructura.Respaldos.IntercepcionMantenimiento>());
        });
        // Registra el repositorio concreto utilizado para
        // las operaciones de persistencia de usuarios.
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        // Registra el servicio encargado de generar
        // y verificar hashes de contraseñas.
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IProteccionAcceso, ProteccionAccesoSql>();
        // Registra el servicio concreto utilizado para las operaciones de persistencia de clientes.
        services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
        // Registra el servicio concreto utilizado para las operaciones de persistencia de marcas.
        services.AddScoped<IMarcaRepositorio, MarcaRepositorio>();
        // Registra el servicio concreto utilizado para las operaciones de persistencia de modelos.
        services.AddScoped<IModeloRepositorio, ModeloRepositorio>();
        // Registra el servicio concreto utilizado para las operaciones de persistencia de vehículos.
        services.AddScoped<IVehiculoRepositorio, VehiculoRepositorio>();
        // Registrar el servicio concreto utilizado para las operaciones de persistencia de atenciones.
        services.AddScoped<IAtencionRepositorio, AtencionRepositorio>();
        // Registrar el servicio concreto utilizado para las operaciones de persistencia de Roles.
        services.AddScoped<IRolRepositorio, RolRepositorio>();
        // Registrar el servicio concreto utilizado para las operaciones de persistencia de categorías de conceptos.
        services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
        // Registrar el servicio concreto utilizado para las operaciones de persistencia de conceptos.
        services.AddScoped<IConceptoRepositorio, ConceptoRepositorio>();
        // Registrar el servicio concreto utilizado para las operaciones de persistencia de diagnósticos.
        services.AddScoped<IDiagnosticoRepositorio, DiagnosticoRepositorio>();
        // Registrar el servicio concreto utilizado para las operaciones de persistencia de unidad de trabajo.
        services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
        services.AddScoped<IEvaluacionRepositorio, EvaluacionRepositorio>();
        services.AddScoped<ICircuitoRepositorio, CircuitoRepositorio>();
        services.AddScoped<IReporteRepositorio, ReporteRepositorio>();
        services.AddScoped<IInicioRepositorio, InicioRepositorio>();
        services.AddScoped<IAuditoriaAccesoRepositorio, AuditoriaAccesoRepositorio>();
        // Registra el servicio responsable de crear los datos
        // mínimos necesarios para el primer inicio del sistema.
        services.AddScoped<InicializadorDatos>();
        return services;
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Taller.Infraestructura.Persistencia;

/// <summary>
/// Fábrica utilizada por las herramientas de Entity Framework Core
/// para crear una instancia de TallerDbContext en tiempo de diseño.
/// Se utiliza, por ejemplo, al crear migraciones o actualizar la base de datos.
/// </summary>
public sealed class TallerDbContextFactory
    : IDesignTimeDbContextFactory<TallerDbContext>
{
    /// <summary>
    /// Crea y configura una instancia de TallerDbContext para que
    /// las herramientas de EF Core puedan trabajar sin iniciar
    /// completamente la aplicación WinForms.
    /// </summary>
    public TallerDbContext CreateDbContext(string[] args)
    {
        // Obtiene la carpeta desde la cual se está ejecutando actualmente
        // el comando de Entity Framework Core.
        var currentDirectory = Directory.GetCurrentDirectory();

        // Determina dónde se encuentra el proyecto Taller.Presentacion,
        // ya que allí están los archivos appsettings utilizados para
        // obtener la cadena de conexión.
        //
        // Si EF Core ya se está ejecutando desde Taller.Presentacion,
        // se utiliza directamente la carpeta actual.
        //
        // En caso contrario, se construye la ruta partiendo desde
        // la raíz del repositorio:
        // Taller/Taller.Presentacion.
        var presentationPath =
            Path.GetFileName(currentDirectory)
                .Equals(
                    "Taller.Presentacion",
                    StringComparison.OrdinalIgnoreCase)
            ? currentDirectory
            : Path.GetFullPath(
                Path.Combine(
                    currentDirectory,
                    "Taller",
                    "Taller.Presentacion"));

        // Construye la configuración que utilizará EF Core
        // durante las operaciones realizadas en tiempo de diseño.
        var configuration = new ConfigurationBuilder()
            // Indica la carpeta base donde se buscarán los archivos
            // de configuración.
            .SetBasePath(presentationPath)
            // Carga la configuración general del proyecto.
            // Es obligatorio que este archivo exista.
            .AddJsonFile(
                "appsettings.json",
                optional: false)
            // Carga, si existe, la configuración local.
            // Este archivo puede contener la cadena de conexión
            // específica de cada computadora y no debe versionarse.
            .AddJsonFile(
                "appsettings.Local.json",
                optional: true)
            // También permite obtener valores definidos mediante
            // variables de entorno.
            .AddEnvironmentVariables()
            // Construye finalmente el objeto IConfiguration.
            .Build();

        // Busca la cadena de conexión utilizando dos alternativas.
        //
        // Primero intenta obtenerla desde la variable de entorno SGTM_CONN.
        // Si no existe, utiliza ConnectionStrings:Default de los appsettings.
        //
        // Si ninguna de las dos contiene una conexión válida,
        // se produce una excepción indicando el problema.
        var connectionString =
            Environment.GetEnvironmentVariable("SGTM_CONN")
            ?? configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "No se encontró una cadena de conexión para SGTM.");

        // Crea un constructor de opciones específico para TallerDbContext.
        // Estas opciones indican cómo debe funcionar el contexto.
        var optionsBuilder =
            new DbContextOptionsBuilder<TallerDbContext>();

        // Configura Entity Framework Core para utilizar SQL Server
        // y la cadena de conexión obtenida anteriormente.
        optionsBuilder.UseSqlServer(connectionString);

        // Crea y devuelve finalmente una instancia de TallerDbContext
        // utilizando las opciones ya configuradas.
        return new TallerDbContext(optionsBuilder.Options);
    }
}
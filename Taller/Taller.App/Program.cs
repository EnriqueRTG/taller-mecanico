// Program.cs (Taller.App)
// -----------------------------------------------------------------------------
// Punto de entrada de la aplicación WinForms utilizando HostBuilder para:
// - Configuración (appsettings.json + overrides locales)
// - Inversión de Dependencias (DI)
// - Logging centralizado con Serilog
// - Inicialización de persistencia (migraciones + seed de datos)
//
// Paquetes NuGet requeridos en Taller.App:
// - Microsoft.Extensions.Hosting
// - Microsoft.Extensions.Configuration.Json
// - Serilog
// - Serilog.Settings.Configuration
// - Serilog.Extensions.Logging
// - Serilog.Sinks.File
// - (solo para herramientas de diseño) Microsoft.EntityFrameworkCore.Design
//
// Notas de arquitectura:
// - El DbContext (AppDbContext) vive en Infrastructure y se registra aquí.
// - Las migraciones se aplican al inicio en DEV para acelerar el ciclo de trabajo.
//   En PROD conviene aplicarlas vía script/CI y dejar el seeding controlado.
// -----------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Taller.Infrastructure.Persistence;
using Taller.Infrastructure.Persistence.Seeders;

namespace Taller.App
{
    /// <summary>
    /// Clase estática de arranque. Orquesta la creación del Host, el registro de
    /// dependencias, la aplicación de migraciones/seed y el lanzamiento del formulario principal.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada de la aplicación WinForms.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos (reservado para futuros usos).</param>
        [STAThread]
        static void Main(string[] args)
        {
            // 0) Inicializa estilos modernos de WinForms (DPI/Fonts) antes de cualquier UI.
            ApplicationConfiguration.Initialize();

            // 1) Crear Host genérico .NET: base para DI + Config + Logging
            var builder = Host.CreateApplicationBuilder(args);

            // 1.1) Configuración: archivo base y overrides locales (no versionados)
            // appsettings.Local.json permite credenciales/cadenas de conexión locales sin comprometer el repo.
            builder.Configuration
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

            // 1.2) Logging: Serilog como proveedor único, configurado desde appsettings
            // Se reemplazan los proveedores por defecto para mantener trazas homogéneas (archivo, template, niveles).
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(Log.Logger, dispose: true);

            try
            {
                // 2) REGISTRO DE DEPENDENCIAS (DI)
                // 2.1) Cadena de conexión: "Default" (requerida)
                var connectionString = builder.Configuration.GetConnectionString("Default")
                                        ?? throw new InvalidOperationException(
                                            "No se encontró 'ConnectionStrings:Default' en appsettings.");

                // 2.2) DbContext: EF Core con SQL Server
                // El provider (Microsoft.EntityFrameworkCore.SqlServer) está referenciado en Infrastructure.
                builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

                // 2.3) Formularios/UI (inyectables). Agregar aquí otros forms a medida que aparecen.
                builder.Services.AddTransient<MainForm>();
                builder.Services.AddTransient<ClientesForm>();
                builder.Services.AddTransient<ClienteEditForm>();
                //builder.Services.AddTransient<>();

                // 3) Construcción del Host: congela la configuración y arma el ServiceProvider
                using var host = builder.Build();

                // 4) Alcance (scope) de DI para resolver servicios con lifetime Scoped/Transient
                using var scope = host.Services.CreateScope();
                var sp = scope.ServiceProvider;

                // 4.1) Logger robusto para etapa de inicio (evita problemas con ILogger<Program>)
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var startupLogger = loggerFactory.CreateLogger("Startup");

                // 4.2) Inicialización de persistencia
                //    - Aplica migraciones pendientes (en DEV)
                //    - Ejecuta seeders idempotentes (no duplican por Código)
                // Si se requiere no bloquear, convertir Main a async o usar Task.Run + Wait.
                var db = sp.GetRequiredService<AppDbContext>();
                DatabaseSeeder.EnsureSeededAsync(db, startupLogger).GetAwaiter().GetResult();

                // 5) Lanzamiento de la UI
                var mainForm = sp.GetRequiredService<MainForm>();
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                // Fallos críticos de arranque: siempre registrar y notificar al usuario
                Log.Fatal(ex, "Fallo no controlado durante el inicio de la aplicación.");
                MessageBox.Show(
                    $"Se produjo un error al iniciar la aplicación:\n{ex.Message}",
                    "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Importante para flush de buffers (archivo) y liberar recursos de Serilog
                Log.CloseAndFlush();
            }
        }
    }
}

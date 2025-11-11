// Program.cs en Taller.App
// Punto de entrada de la aplicación WinForms con HostBuilder para DI, Config y Logging.
// Requiere NuGet en Taller.App: 
// - Microsoft.Extensions.Hosting
// - Microsoft.Extensions.Configuration.Json
// - Serilog
// - Serilog.Settings.Configuration
// - Serilog.Extensions.Logging
// - Serilog.Sinks.File

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Taller.App
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada. Inicializa configuración de UI, crea el Host (DI, Config, Logging),
        /// registra servicios y ejecuta el formulario principal.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Inicializa estilos modernos de WinForms (High DPI, fuentes, etc.)
            ApplicationConfiguration.Initialize();

            // Host genérico .NET: contenedor de DI + Config + Logging para toda la app.
            var builder = Host.CreateApplicationBuilder(args);

            // 1) Configuración: appsettings.json (obligatorio) + appsettings.Local.json (opcional para secretos locales)
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

            // 2) Logging: Serilog lee su configuración del JSON (nivel, destinos, template)
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)   // habilita bloque "Serilog" en appsettings
                .Enrich.FromLogContext()                        // agrega metadatos útiles a cada evento
                .CreateLogger();

            // Reemplaza proveedores por defecto por Serilog (centralizado)
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(Log.Logger, dispose: true);

            // 3) Registro de dependencias (DI)
            // - DbContext y servicios se agregan más abajo cuando existan.
            // - Registramos el formulario principal (DI-friendly).
            builder.Services.AddTransient<MainForm>();

            // 4) Build del host: congela la configuración y prepara el ServiceProvider
            var host = builder.Build();

            // 5) Scope de DI: ciclo de vida de objetos (transient/scoped/singleton)
            using var scope = host.Services.CreateScope();
            var mainForm = scope.ServiceProvider.GetRequiredService<MainForm>();

            // 6) Arranque de la UI
            Application.Run(mainForm);
        }
    }
}

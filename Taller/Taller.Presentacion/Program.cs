using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taller.Aplicacion;
using Taller.Infraestructura;
using Taller.Infraestructura.Persistencia.Inicializacion;

// Luego mover para ID en Presentacion
using Taller.Presentacion.Formularios.Autenticacion;

namespace Taller.Presentacion;

internal static class Program
{
    [STAThread]
    static async Task Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration
            .AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: true)
            .AddJsonFile(
                "appsettings.Local.json",
                optional: true,
                reloadOnChange: true);

        // Registra los servicios pertenecientes a la capa de Aplicación.
        builder.Services.AddAplicacion();

        // Registra los servicios tecnicos de Infraestructura, incluyendo persistencia, repositorios y seguridad
        builder.Services.AddInfraestructura(
            builder.Configuration);

        // Luego mover para ID en Presentacion
        builder.Services.AddScoped<Login>();

        using var host = builder.Build();

        // Crea un alcance de servicios para ejecutar la inicializacion de datos al comenzar la apliacion
        using var scope = host.Services.CreateScope();

        var inicializador = scope.ServiceProvider.GetRequiredService<InicializadorDatos>();

        await inicializador.InicializarAsync();

        var login = scope.ServiceProvider
        .GetRequiredService<Login>();

        Application.Run(login);
    }
}
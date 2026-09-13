using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taller.Aplicacion;
using Taller.Infraestructura;
using Taller.Infraestructura.Persistencia.Inicializacion;
using Taller.Presentacion.Formularios.Autenticacion;
using Taller.Presentacion.Formularios.Principal;

namespace Taller.Presentacion;

internal static class Program
{
    [STAThread]
    private static async Task Main(string[] args)
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

        // Registra los servicios de la capa de Aplicación.
        builder.Services.AddAplicacion();

        // Registra persistencia, repositorios y seguridad.
        builder.Services.AddInfraestructura(
            builder.Configuration);

        // Registra formularios y componentes de Presentación.
        builder.Services.AddPresentacion();

        using var host = builder.Build();
        using var scope = host.Services.CreateScope();

        var inicializador =
            scope.ServiceProvider
                .GetRequiredService<InicializadorDatos>();

        await inicializador.InicializarAsync();

        var aplicacionContexto =
            scope.ServiceProvider
                .GetRequiredService<AplicacionContexto>();

        Application.Run(aplicacionContexto);
    }
}
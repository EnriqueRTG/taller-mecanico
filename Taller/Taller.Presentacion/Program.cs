using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Taller.Aplicacion;
using Taller.Infraestructura;

namespace Taller.Presentacion;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
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

        builder.Services.AddAplicacion();

        builder.Services.AddInfraestructura(
            builder.Configuration);

        using var host = builder.Build();
    }
}
using Serilog;
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
    private static void Main(string[] args)
    {
        var carpetaLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SGTM", "Logs");
        Serilog.Log.Logger = new Serilog.LoggerConfiguration().WriteTo.File(Path.Combine(carpetaLog, "sgtm-.log"), rollingInterval: Serilog.RollingInterval.Day, retainedFileCountLimit: 7, fileSizeLimitBytes: 2_000_000, rollOnFileSizeLimit: true).CreateLogger();
        ApplicationConfiguration.Initialize();
        try
        {
            Ejecutar(args);
        }
        catch (Exception ex)
        {
            Log.Error("Inicio fallido: {Tipo}", ex.GetType().Name);
            MessageBox.Show("SGTM no pudo iniciar. Revise la configuración de conexión y mantenimiento con el personal técnico.\n\n" + "Consulte el registro local con el responsable técnico.", "Inicio de SGTM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            Serilog.Log.CloseAndFlush();
        }
    }

    private static void Ejecutar(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
        // Registra los servicios de la capa de Aplicación.
        builder.Services.AddAplicacion();
        // Registra persistencia, repositorios y seguridad.
        builder.Services.AddInfraestructura(builder.Configuration);
        // Registra formularios y componentes de Presentación.
        builder.Services.AddPresentacion();
        using var host = builder.Build();
        using var scope = host.Services.CreateScope();
        var inicializador = scope.ServiceProvider.GetRequiredService<InicializadorDatos>();
        // La inicialización debe completarse sobre el mismo hilo STA
        // que luego ejecutará el bucle principal de Windows Forms.
        // Un Main async puede continuar en un hilo MTA después del await
        // y provocar errores al abrir OpenFileDialog o SaveFileDialog.
        try
        {
            inicializador.InicializarAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Log.Error("Inicio fallido: {Tipo}", ex.GetType().Name);
            MessageBox.Show("No se pudo iniciar SGTM. Puede haber una recuperación en curso o un problema de conexión.\n\n" + "Consulte el registro local con el responsable técnico." + "\n\nSi el mantenimiento se interrumpió, contacte al personal técnico; no intente operar con otra copia de la base.", "Inicio de SGTM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var aplicacionContexto = scope.ServiceProvider.GetRequiredService<AplicacionContexto>();
        Application.Run(aplicacionContexto);
    }
}

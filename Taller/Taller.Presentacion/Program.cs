using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taller.Aplicacion;
using Taller.Infraestructura;
using Taller.Infraestructura.Persistencia.Inicializacion;
using Taller.Presentacion.Formularios.Autenticacion;
using Taller.Presentacion.Formularios.Principal;
using System.Diagnostics;

namespace Taller.Presentacion;
internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        // El instalador usa este modo para preparar la conexión antes de abrir SGTM.
        if (args.Length > 0 && args[0] == "--preparar-base")
        {
            Environment.ExitCode = PrepararBase(args);
            return;
        }

        var carpetaLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SGTM", "Logs");
        Serilog.Log.Logger = new Serilog.LoggerConfiguration().WriteTo.File(Path.Combine(carpetaLog, "sgtm-.log"), rollingInterval: Serilog.RollingInterval.Day, retainedFileCountLimit: 7, fileSizeLimitBytes: 2_000_000, rollOnFileSizeLimit: true).CreateLogger();
        ApplicationConfiguration.Initialize();
        try
        {
            Ejecutar(args);
        }
        catch (Exception ex)
        {
            var referencia = Guid.NewGuid().ToString("N")[..10];
            Log.Error(ex, "Inicio fallido de SGTM; referencia {Referencia}", referencia);
            MessageBox.Show("SGTM no pudo iniciar. Revise la configuración de conexión y mantenimiento con el personal técnico.\n\n" + "Consulte el registro local con el responsable técnico. Referencia: " + referencia, "Inicio de SGTM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            Serilog.Log.CloseAndFlush();
        }
    }

    private static int PrepararBase(string[] args)
    {
        // Esta primera integración solo acepta una base existente indicada
        // explícitamente. El nombre de la base de prueba se pasa desde afuera.
        if (args.Length != 3
            || (args[1] != "--usar-base-existente" && args[1] != "--crear-base")
            || string.IsNullOrWhiteSpace(args[2]))
        {
            return 2;
        }

        // Busca el script junto al ejecutable, aunque el proceso se inicie
        // desde otro directorio.
        var carpetaAplicacion = AppContext.BaseDirectory;
        var rutaScript = Path.Combine(carpetaAplicacion, "Preparar-BaseSGTM.ps1");

        if (!File.Exists(rutaScript))
        {
            return 3;
        }

        // Usa Windows PowerShell, el mismo que se probó manualmente.
        var rutaPowerShell = Path.Combine(
            Environment.SystemDirectory, "WindowsPowerShell", "v1.0", "powershell.exe");

        var inicio = new ProcessStartInfo
        {
            FileName = rutaPowerShell,
            WorkingDirectory = carpetaAplicacion,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true
        };

        // ArgumentList conserva cada argumento por separado, incluso si
        // la ruta de instalación contiene espacios.
        foreach (var argumento in new[]
        {
            "-NoProfile", "-NonInteractive", "-ExecutionPolicy", "RemoteSigned",
            "-File", rutaScript,
            "-Base", args[2],
            "-CarpetaPreparacion", carpetaAplicacion
        })
        {
            inicio.ArgumentList.Add(argumento);
        }

        // El script reutiliza la base solo cuando recibe este modificador.
        if (args[1] == "--usar-base-existente")
        {
            inicio.ArgumentList.Add("-UsarBaseExistente");
        }

        try
        {
            using var proceso = Process.Start(inicio);
            if (proceso is null)
            {
                return 4;
            }

            
            // Lee el error de PowerShell mientras el proceso termina.
            var errorPowerShell = proceso.StandardError.ReadToEnd();
            proceso.WaitForExit();

            if (proceso.ExitCode != 0)
            {
                // Guarda el diagnóstico fuera de la carpeta que el MSI puede revertir.
                var identidad = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                var rutaLog = Path.Combine(Path.GetTempPath(), "SGTM-PrepararBase-error.log");
                File.WriteAllText(
                    rutaLog,
                    $"Cuenta: {identidad}{Environment.NewLine}" +
                    $"Código: {proceso.ExitCode}{Environment.NewLine}" +
                    errorPowerShell);
            }

            return proceso.ExitCode;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            return 4;
        }
    }

    private static void Ejecutar(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args, ContentRootPath = AppContext.BaseDirectory });
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
        catch (Taller.Aplicacion.Excepciones.ValidacionException ex)
        {
            Log.Warning(ex, "Instalación incompatible con la versión de SGTM");
            MessageBox.Show(ex.Message, "Verificar versión de la base", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        catch (Exception ex)
        {
            var referencia = Guid.NewGuid().ToString("N")[..10];
            Log.Error(ex, "Inicio fallido de SGTM; referencia {Referencia}", referencia);
            MessageBox.Show("No se pudo iniciar SGTM. Puede haber una recuperación en curso o un problema de conexión.\n\n" + "Consulte el registro local con el responsable técnico. Referencia: " + referencia + "\n\nSi el mantenimiento se interrumpió, contacte al personal técnico; no intente operar con otra copia de la base.", "Inicio de SGTM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var aplicacionContexto = scope.ServiceProvider.GetRequiredService<AplicacionContexto>();
        var ayuda = new Ayuda.AyudaContextual();
        Application.AddMessageFilter(ayuda);
        try
        {
            Application.Run(aplicacionContexto);
        }
        finally
        {
            Application.RemoveMessageFilter(ayuda);
        }
    }
}

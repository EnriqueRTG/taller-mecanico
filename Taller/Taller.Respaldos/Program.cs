using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taller.Aplicacion;
using Taller.Infraestructura;
using Taller.Infraestructura.Respaldos;
using Taller.Respaldos;

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args.Where(a => a != "--instalar-control" && a != "--estado-control" && a != "--verificar-base").ToArray(), ContentRootPath = AppContext.BaseDirectory });
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false).AddEnvironmentVariables();
builder.Services.AddAplicacion();
builder.Services.AddInfraestructura(builder.Configuration);
if (args.Contains("--verificar-base", StringComparer.Ordinal))
{
    using var verificacion = builder.Build();
    await verificacion.Services.GetRequiredService<RegistroRespaldosSql>().VerificarBaseAsync();
    Console.WriteLine("Base SGTM accesible y estructura inicial presente. No se modificaron datos. La copia real verificará los permisos de respaldo sobre SQL y archivos.");
    return;
}

if (args.Contains("--instalar-control", StringComparer.Ordinal))
{
    using var instalador = builder.Build();
    await instalador.Services.GetRequiredService<RegistroRespaldosSql>().InstalarAsync();
    Console.WriteLine("Base de control instalada. Configure permisos y active Respaldos:Habilitado en todos los clientes antes de restaurar.");
    return;
}

if (args.Contains("--estado-control", StringComparer.Ordinal))
{
    using var diagnostico = builder.Build();
    var estado = await diagnostico.Services.GetRequiredService<Taller.Aplicacion.Abstracciones.Persistencia.IRespaldoMotor>().ConsultarAsync();
    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(estado, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
    return;
}

builder.Services.AddWindowsService(o => o.ServiceName = "SGTMRespaldos");
builder.Services.AddHostedService<AgenteRespaldos>();
using var host = builder.Build();
await host.RunAsync();

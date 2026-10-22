using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taller.Aplicacion;
using Taller.Infraestructura;
using Taller.Infraestructura.Respaldos;
using Taller.Respaldos;

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args.Where(a => a != "--instalar-control" && a != "--estado-control").ToArray(), ContentRootPath = AppContext.BaseDirectory });
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false);
builder.Services.AddAplicacion();
builder.Services.AddInfraestructura(builder.Configuration);
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

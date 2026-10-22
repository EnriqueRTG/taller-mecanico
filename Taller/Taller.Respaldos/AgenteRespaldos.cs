using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Taller.Aplicacion.Abstracciones.Persistencia;
namespace Taller.Respaldos;
/// <summary>Agente independiente de las sesiones visuales. Instalar en un único equipo siempre encendido con acceso al servidor SQL.</summary>
public sealed class AgenteRespaldos(IRespaldoMotor motor,ILogger<AgenteRespaldos> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if(!motor.Habilitado){log.LogError("Respaldos está deshabilitado; el agente no ejecutará copias. Configure la instalación y reinicie el servicio.");return;}
        using var reloj=new PeriodicTimer(TimeSpan.FromMinutes(1));
        do
        {
            try{await motor.EjecutarProgramacionAsync(stoppingToken);}
            catch(OperationCanceledException)when(stoppingToken.IsCancellationRequested){break;}
            catch(Exception e){log.LogError("El agente no pudo ejecutar su comprobación: {Error}",e.Message);}
        }while(await reloj.WaitForNextTickAsync(stoppingToken));
    }
}

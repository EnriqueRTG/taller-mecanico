using System.Collections.Concurrent;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace Taller.Infraestructura.Respaldos;
/// <summary>Cada conexión de negocio conserva un bloqueo compartido externo hasta cerrarse. Restaurar espera el exclusivo y bloquea nuevos accesos.</summary>
public sealed class IntercepcionMantenimiento(OpcionesRespaldos opciones,GeneracionMantenimiento generacion) : DbConnectionInterceptor
{
    private readonly ConcurrentDictionary<DbConnection,SqlConnection> _permisos=new();
    private async Task EntrarAsync(DbConnection negocio,CancellationToken ct)
    {
        var control=await SqlRespaldos.AbrirAsync(opciones.Control(false),ct);
        try
        {
            using var cmd=SqlRespaldos.Comando(control,"DECLARE @r int; EXEC @r=sys.sp_getapplock @Resource=@recurso,@LockMode='Shared',@LockOwner='Session',@LockTimeout=0,@DbPrincipal='public'; SELECT @r;",("@recurso",opciones.RecursoMantenimiento));
            if(Convert.ToInt32(await cmd.ExecuteScalarAsync(ct))<0)throw new InvalidOperationException("El sistema está en mantenimiento. Espere a que finalice la restauración.");
            var estado=await new RegistroRespaldosSql(opciones).ComprobarAsync(control,ct);
            if(estado.Mantenimiento)throw new InvalidOperationException("El sistema está en mantenimiento: "+estado.Detalle+". Si la recuperación se interrumpió, contacte al personal técnico.");
            generacion.Validar(estado.Generacion);
            if(!_permisos.TryAdd(negocio,control))throw new InvalidOperationException("La conexión ya tiene una operación en curso.");
        }
        catch{await control.DisposeAsync();throw;}
    }
    private void Salir(DbConnection c){if(_permisos.TryRemove(c,out var permiso))permiso.Dispose();}
    public override InterceptionResult ConnectionOpening(DbConnection c,ConnectionEventData e,InterceptionResult r){EntrarAsync(c,default).GetAwaiter().GetResult();return r;}
    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection c,ConnectionEventData e,InterceptionResult r,CancellationToken ct=default){await EntrarAsync(c,ct);return r;}
    public override void ConnectionClosed(DbConnection c,ConnectionEndEventData e)=>Salir(c);
    public override Task ConnectionClosedAsync(DbConnection c,ConnectionEndEventData e){Salir(c);return Task.CompletedTask;}
    public override void ConnectionDisposed(DbConnection c,ConnectionEndEventData e)=>Salir(c);
    public override Task ConnectionDisposedAsync(DbConnection c,ConnectionEndEventData e){Salir(c);return Task.CompletedTask;}
    public override void ConnectionFailed(DbConnection c,ConnectionErrorEventData e)=>Salir(c);
    public override Task ConnectionFailedAsync(DbConnection c,ConnectionErrorEventData e,CancellationToken ct=default){Salir(c);return Task.CompletedTask;}
}

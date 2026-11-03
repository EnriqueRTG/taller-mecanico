using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Taller.Infraestructura.Respaldos;

/// <summary>
/// Protege cada apertura hasta su cierre real. Las sesiones físicas se reutilizan,
/// pero quedan en master mientras están ociosas para permitir restaurar negocio.
/// No conserva permisos ni el estado de mantenimiento en caché.
/// </summary>
public sealed class IntercepcionMantenimiento(OpcionesRespaldos opciones, GeneracionMantenimiento generacion) : DbConnectionInterceptor
{
    private readonly ConcurrentDictionary<DbConnection, SqlConnection> _permisos = new();

    private async Task EntrarAsync(DbConnection negocio, CancellationToken ct)
    {
        var control = await SqlRespaldos.AbrirAsync(opciones.ControlCliente(), ct).ConfigureAwait(false);
        try
        {
            // Un solo viaje adquiere el permiso y lee el estado duradero. Restaurar
            // necesita el exclusivo y sigue esperando a que termine esta operación.
            using var cmd = SqlRespaldos.Comando(control, """
                DECLARE @r int;
                EXEC @r=sys.sp_getapplock @Resource=@recurso,@LockMode='Shared',
                    @LockOwner='Session',@LockTimeout=0,@DbPrincipal='public';
                IF @r<0 THROW 51144,'El sistema está en mantenimiento.',1;
                SELECT BaseDatos,VersionEsquema,Mantenimiento,Generacion,Detalle FROM dbo.EstadoSGTM WHERE Id=1;
                """, ("@recurso", opciones.RecursoMantenimiento));
            cmd.CommandTimeout = 15;
            using (var rd = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                if (!await rd.ReadAsync(ct).ConfigureAwait(false)
                    || !string.Equals(rd.GetString(0), opciones.BaseDatos, StringComparison.OrdinalIgnoreCase) || rd.GetInt32(1) != 1)
                    throw new InvalidOperationException("La base de control no corresponde a esta instalación o requiere actualización técnica.");
                if (rd.GetBoolean(2))
                    throw new InvalidOperationException("El sistema está en mantenimiento: " + rd.GetString(4));
                generacion.Validar(rd.GetInt64(3));
            }
            if (!_permisos.TryAdd(negocio, control))
                throw new InvalidOperationException("La conexión ya tiene una operación en curso.");
        }
        catch (SqlException e) when (e.Number == 51144)
        {
            await LiberarAsync(control).ConfigureAwait(false);
            throw new InvalidOperationException("El sistema está en mantenimiento. Espere a que finalice la restauración.", e);
        }
        catch
        {
            await LiberarAsync(control).ConfigureAwait(false);
            throw;
        }
    }

    /// <summary>El applock de sesión se libera explícitamente antes de devolver control al pool.</summary>
    private async Task LiberarAsync(SqlConnection control)
    {
        try
        {
            if (control.State == ConnectionState.Open)
            {
                using var cmd = SqlRespaldos.Comando(control,
                    "EXEC sys.sp_releaseapplock @Resource=@recurso,@LockOwner='Session',@DbPrincipal='public';",
                    ("@recurso", opciones.RecursoMantenimiento));
                cmd.CommandTimeout = 15;
                await cmd.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
        catch { SqlConnection.ClearPool(control); }
        finally { await control.DisposeAsync().ConfigureAwait(false); }
    }

    private void Salir(DbConnection c)
    {
        if (_permisos.TryRemove(c, out var permiso)) LiberarAsync(permiso).GetAwaiter().GetResult();
    }
    private Task SalirAsync(DbConnection c) => _permisos.TryRemove(c, out var permiso)
        ? LiberarAsync(permiso) : Task.CompletedTask;

    /// <summary>
    /// Estaciona negocio ANTES de cerrar y liberar su permiso. SqlClient restablece
    /// el catálogo original al reutilizar la conexión. Si falla, descarta esa sesión
    /// al cerrarla; no deja una conexión dormida dentro de la base restaurable.
    /// </summary>
    private async Task EstacionarAsync(DbConnection c)
    {
        if (c is not SqlConnection sql || !new SqlConnectionStringBuilder(sql.ConnectionString).Pooling) return;
        if (sql.State != ConnectionState.Open)
        {
            // Dispose del contexto puede cerrar internamente antes de este evento.
            // Si aún conservamos el permiso, no pasó por ConnectionClosing: se
            // descarta ese pool para retirar la sesión que no pudo estacionarse.
            // Una disposición posterior a consultas normales no tiene permiso y
            // conserva el pool; la ruta frecuente sigue reutilizando conexiones.
            if (_permisos.ContainsKey(c)) SqlConnection.ClearPool(sql);
            return;
        }
        try { await sql.ChangeDatabaseAsync("master", CancellationToken.None).ConfigureAwait(false); }
        catch { SqlConnection.ClearPool(sql); }
    }

    public override InterceptionResult ConnectionOpening(DbConnection c, ConnectionEventData e, InterceptionResult r)
    {
        EntrarAsync(c, default).GetAwaiter().GetResult();
        return r;
    }
    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection c, ConnectionEventData e, InterceptionResult r, CancellationToken ct = default)
    {
        await EntrarAsync(c, ct).ConfigureAwait(false);
        return r;
    }
    public override InterceptionResult ConnectionClosing(DbConnection c, ConnectionEventData e, InterceptionResult r)
    {
        EstacionarAsync(c).GetAwaiter().GetResult();
        return r;
    }
    public override async ValueTask<InterceptionResult> ConnectionClosingAsync(DbConnection c, ConnectionEventData e, InterceptionResult r)
    {
        await EstacionarAsync(c).ConfigureAwait(false);
        return r;
    }
    public override InterceptionResult ConnectionDisposing(DbConnection c, ConnectionEventData e, InterceptionResult r)
    {
        EstacionarAsync(c).GetAwaiter().GetResult();
        return r;
    }
    public override async ValueTask<InterceptionResult> ConnectionDisposingAsync(DbConnection c, ConnectionEventData e, InterceptionResult r)
    {
        await EstacionarAsync(c).ConfigureAwait(false);
        return r;
    }
    public override void ConnectionClosed(DbConnection c, ConnectionEndEventData e) => Salir(c);
    public override Task ConnectionClosedAsync(DbConnection c, ConnectionEndEventData e) => SalirAsync(c);
    public override void ConnectionDisposed(DbConnection c, ConnectionEndEventData e) => Salir(c);
    public override Task ConnectionDisposedAsync(DbConnection c, ConnectionEndEventData e) => SalirAsync(c);
    public override void ConnectionFailed(DbConnection c, ConnectionErrorEventData e) => Salir(c);
    public override Task ConnectionFailedAsync(DbConnection c, ConnectionErrorEventData e, CancellationToken ct = default) => SalirAsync(c);
}

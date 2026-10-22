using Microsoft.Data.SqlClient;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Respaldos;

namespace Taller.Infraestructura.Respaldos;
public sealed partial class RespaldoSqlServer
{
    /// <summary>Solo se invoca desde el agente instalado. Un bloqueo SQL evita ejecuciones simultáneas entre agentes y copias manuales.</summary>
    public async Task EjecutarProgramacionAsync(CancellationToken cancelacion = default)
    {
        if (!Habilitado)
            return;
        await using var control = await ControlAsync(cancelacion);
        await SqlRespaldos.EjecutarAsync(control, "UPDATE dbo.ProgramaSGTM SET SenalUtc=SYSUTCDATETIME() WHERE Id=1", cancelacion);
        if ((await _registro.ComprobarAsync(control, cancelacion)).Mantenimiento)
            return;
        await using var bloqueo = await SqlRespaldos.BloquearAsync(opciones, opciones.RecursoCopias, "Exclusive", 0, cancelacion);
        var p = await _registro.ProgramaAsync(control, cancelacion);
        var ahora = reloj.GetUtcNow();
        if (!p.Activo || p.ProximaUtc is null || p.ProximaUtc > ahora)
            return;
        await using var acceso = await AccesoNegocioAsync(cancelacion);
        await using var master = await SqlRespaldos.AbrirAsync(opciones.Master(), cancelacion);
        try
        {
            int autorizado = await SqlRespaldos.EscalarAsync<int>(master, $"SELECT COUNT(*) FROM {Db}.dbo.Usuarios u JOIN {Db}.dbo.Roles r ON r.Id=u.RolId WHERE u.Id=@id AND u.Activo=1 AND u.RolId=1 AND r.Activo=1 AND r.Nombre=N'Administrador'", cancelacion, ("@id", p.IdAdministrador));
            if (autorizado != 1)
                throw new InvalidOperationException("El administrador que autorizó la programación ya no está habilitado. Otro administrador debe revisar y guardar el plan.");
            var familia = await FamiliaAsync(master, cancelacion);
            var esquema = await EsquemaAsync(master, opciones.BaseDatos, cancelacion);
            var completa = (await _registro.CopiasAsync(control, cancelacion)).FirstOrDefault(x => x.Tipo == TipoRespaldo.Completa && !x.SoloCopia && x.Familia == familia && x.EsquemaHash == esquema);
            var tipo = PoliticaRespaldos.TipoProgramado(p, ahora, completa is not null && completa.RegistradaUtc > ahora.AddDays(-7) && await EsBaseActualAsync(master, completa, cancelacion));
            var copia = await CrearNativaAsync(master, control, tipo, p.CarpetaServidor, "Agente · autorizado por " + p.Autor, false, null, cancelacion);
            var proxima = PoliticaRespaldos.Proxima(p, reloj.GetUtcNow());
            await _registro.GuardarProgramaAsync(control, p with { ProximaUtc = proxima }, cancelacion);
            await SqlRespaldos.EjecutarAsync(control, "UPDATE dbo.ProgramaSGTM SET Resultado=@resultado WHERE Id=1", cancelacion, ("@resultado", $"{tipo} verificada: {copia.FechaServidor:dd/MM/yyyy HH:mm:ss} (servidor SQL). {copia.Advertencia}"));
        }
        catch (Exception e)when (e is not OperationCanceledException)
        {
            // Evita una tormenta de intentos y conserva la programación. Un horario perdido se ejecuta una vez al reiniciar el agente.
            await _registro.GuardarProgramaAsync(control, p with { ProximaUtc = reloj.GetUtcNow().AddMinutes(15) }, CancellationToken.None);
            await SqlRespaldos.EjecutarAsync(control, "UPDATE dbo.ProgramaSGTM SET Resultado=@resultado WHERE Id=1", CancellationToken.None, ("@resultado", "Falló el respaldo automático; reintento en 15 minutos: " + e.Message));
            await _registro.EventoAsync(control, "CopiaProgramada", "Fallida", p.Autor, "", e.Message, CancellationToken.None);
        }
    }
}

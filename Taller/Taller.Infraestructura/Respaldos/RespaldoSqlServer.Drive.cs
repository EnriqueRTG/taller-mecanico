using Microsoft.Data.SqlClient;
using Taller.Aplicacion.DTOs.Respaldos;

namespace Taller.Infraestructura.Respaldos;
public sealed partial class RespaldoSqlServer
{
    private static Task ActualizarCopiaAsync(SqlConnection c, CopiaRespaldoDto copia, CancellationToken ct) =>
        SqlRespaldos.EjecutarAsync(c, "UPDATE dbo.CopiasSGTM SET Documento=@json WHERE Id=@id", ct,
            ("@json", System.Text.Json.JsonSerializer.Serialize(copia)), ("@id", copia.Id));

    // Called while holding the copy and maintenance locks. Never creates another SQL backup on retry.
    private async Task<CopiaRespaldoDto> ExportarDriveAsync(SqlConnection control, CopiaRespaldoDto copia, string destino, CancellationToken ct)
    {
        copia = copia with { UltimoIntentoDriveUtc = reloj.GetUtcNow(), EstadoDrive = "Pendiente de exportación" };
        await ActualizarCopiaAsync(control, copia, ct);
        try
        {
            await ExigirEquipoPaquetesAsync(ct);
            var archivos = new List<CopiaRespaldoDto>();
            if (copia.BaseId is { } id)
                archivos.Add((await _registro.CopiasAsync(control, ct)).Single(x => x.Id == id));
            archivos.Add(copia);
            var ruta = await PaqueteRespaldo.ExportarAsync(destino, copia.Id, archivos, ct);
            copia = copia with { RutaPaqueteDrive = ruta, EstadoDrive = "Paquete verificado; comprobar sincronización en Drive" };
            await ActualizarCopiaAsync(control, copia, ct);
            await _registro.EventoAsync(control, "PaqueteExportado", "Verificado; sincronización externa", copia.Autor, "", ruta, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            copia = copia with { EstadoDrive = "Pendiente de reintento; copia local conservada" };
            await ActualizarCopiaAsync(control, copia, CancellationToken.None);
            await _registro.EventoAsync(control, "ExportacionDrive", "Pendiente", copia.Autor, "", ex.Message, CancellationToken.None);
        }
        return copia;
    }
}

using Microsoft.Data.SqlClient;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Respaldos;

namespace Taller.Infraestructura.Respaldos;
public sealed partial class RespaldoSqlServer
{
    private IArchivoRespaldoNube ExigirNube() => nube ?? throw new InvalidOperationException("No está instalado el proveedor Azure.");
    private async Task<CopiaRespaldoDto> ReplicarAsync(SqlConnection c, CopiaRespaldoDto copia, CancellationToken ct)
    {
        copia = copia with
        {
            AzureSolicitado = true,
            EstadoAzure = "Pendiente",
            UltimoIntentoAzureUtc = reloj.GetUtcNow()
        };
        await ActualizarCopiaAsync(c, copia, ct);
        try
        {
            // Una diferencial requiere que su completa base esté disponible también en la nube.
            if (copia.BaseId is { } id)
            {
                var baseCompleta = (await _registro.CopiasAsync(c, ct)).Single(x => x.Id == id);
                if (baseCompleta.EstadoAzure != "Verificada")
                    await ReplicarAsync(c, baseCompleta, ct);
                baseCompleta = (await _registro.CopiasAsync(c, ct)).Single(x => x.Id == id);
                if (baseCompleta.EstadoAzure != "Verificada")
                    throw new InvalidOperationException("Falta replicar la completa base.");
            }

            copia = await ExigirNube().SubirAsync(copia, ct);
            await ActualizarCopiaAsync(c, copia, ct);
            await _registro.EventoAsync(c, "ReplicaAzure", "Verificada", copia.Autor, "", copia.Id.ToString(), ct);
        }
        catch (Exception ex)when (ex is not OperationCanceledException)
        {
            copia = copia with
            {
                EstadoAzure = "Pendiente de reintento",
                AdvertenciaAzure = "La copia local está verificada; la réplica Azure no se completó. Revise conexión, identidad y acceso a la carpeta, y reintente desde Copias."
            };
            await ActualizarCopiaAsync(c, copia, CancellationToken.None);
            // No persistir excepciones del SDK: pueden contener URLs y datos de autenticación.
            await _registro.EventoAsync(c, "ReplicaAzure", "Pendiente", copia.Autor, "", copia.Id + " · " + ex.GetType().Name, CancellationToken.None);
        }

        return copia;
    }

    private static Task ActualizarCopiaAsync(SqlConnection c, CopiaRespaldoDto copia, CancellationToken ct) => SqlRespaldos.EjecutarAsync(c, "UPDATE dbo.CopiasSGTM SET Documento=@json WHERE Id=@id", ct, ("@json", System.Text.Json.JsonSerializer.Serialize(copia)), ("@id", copia.Id));
    public async Task<CopiaRespaldoDto> CrearConDestinoAsync(TipoRespaldo tipo, string carpeta, string autor, bool azure, IProgress<string>? progreso = null, CancellationToken ct = default)
    {
        if (azure)
            ExigirNube().ValidarConfiguracion();
        // Mantener bloqueos de mantenimiento/copias hasta registrar y replicar el archivo.
        await using var control = await ControlAsync(ct);
        await using var bloqueo = await SqlRespaldos.BloquearAsync(opciones, opciones.RecursoCopias, "Exclusive", 0, ct);
        await using var acceso = await AccesoNegocioAsync(ct);
        await using var master = await SqlRespaldos.AbrirAsync(opciones.Master(), ct);
        Taller.Aplicacion.Respaldos.PoliticaRespaldos.Tipo(tipo);
        carpeta = Taller.Aplicacion.Respaldos.PoliticaRespaldos.Carpeta(carpeta);
        var copia = await CrearNativaAsync(master, control, tipo, carpeta, autor, false, progreso, ct);
        return azure ? await ReplicarAsync(control, copia, ct) : copia;
    }

    public async Task ReintentarAzureAsync(Guid id, string autor, CancellationToken ct = default)
    {
        await using var control = await ControlAsync(ct);
        await using var bloqueo = await SqlRespaldos.BloquearAsync(opciones, opciones.RecursoCopias, "Exclusive", 0, ct);
        await using var acceso = await AccesoNegocioAsync(ct);
        var copia = (await _registro.CopiasAsync(control, ct)).Single(x => x.Id == id);
        if (!copia.AzureSolicitado)
            throw new InvalidOperationException("Esta copia no solicitó réplica Azure.");
        await _registro.EventoAsync(control, "ReintentoAzure", "Solicitado", autor, "", id.ToString(), ct);
        await ReplicarAsync(control, copia, ct);
    }
}

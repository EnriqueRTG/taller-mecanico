using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Respaldos;

namespace Taller.Infraestructura.Respaldos;
public sealed partial class RespaldoSqlServer
{
    private async Task ExigirEquipoPaquetesAsync(CancellationToken ct)
    {
        if (!(await ConsultarAsync(ct)).PuedeExplorarCarpetaLocal)
            throw new InvalidOperationException("Abra SGTM en el mismo equipo Windows donde está SQL Server para exportar o recuperar paquetes de Drive.");
    }
    public async Task<string> ExportarPaqueteAsync(Guid copia, string destino, string autor, CancellationToken ct = default)
    {
        await ExigirEquipoPaquetesAsync(ct);
        await using var control = await ControlAsync(ct);
        await using var bloqueo = await SqlRespaldos.BloquearAsync(opciones, opciones.RecursoCopias, "Exclusive", 0, ct);
        await using var acceso = await AccesoNegocioAsync(ct);
        await using var master = await SqlRespaldos.AbrirAsync(opciones.Master(), ct);
        var plan = await ComprobarPlanAsync(master, control, copia, ct);
        var archivos = new List<CopiaRespaldoDto> { plan.Completa };
        if (plan.Diferencial != null) archivos.Add(plan.Diferencial);
        var ruta = await PaqueteRespaldo.ExportarAsync(destino, copia, archivos, ct);
        var exportada = (plan.Diferencial ?? plan.Completa) with { RutaPaqueteDrive = ruta, EstadoDrive = "Paquete verificado; comprobar sincronización en Drive", UltimoIntentoDriveUtc = reloj.GetUtcNow() };
        await ActualizarCopiaAsync(control, exportada, ct);
        await _registro.EventoAsync(control, "PaqueteExportado", "Archivo verificado; sincronización externa", autor, "", ruta, ct);
        return ruta;
    }
    public async Task<Guid> ImportarPaqueteAsync(string origen, string carpetaServidor, string autor, CancellationToken ct = default)
    {
        await ExigirEquipoPaquetesAsync(ct);
        carpetaServidor = PoliticaRespaldos.Carpeta(carpetaServidor);
        if (!Directory.Exists(carpetaServidor)) throw new InvalidOperationException("La carpeta local de recuperación no existe.");
        var paquete = await PaqueteRespaldo.LeerAsync(origen, ct);
        await using var control = await ControlAsync(ct);
        await using var bloqueo = await SqlRespaldos.BloquearAsync(opciones, opciones.RecursoCopias, "Exclusive", 0, ct);
        await using var acceso = await AccesoNegocioAsync(ct);
        await using var master = await SqlRespaldos.AbrirAsync(opciones.Master(), ct);
        var familia = await FamiliaAsync(master, ct);
        var esquema = await EsquemaAsync(master, opciones.BaseDatos, ct);
        if (paquete.Archivos.Any(a => a.Copia.Familia != familia || a.Copia.EsquemaHash != esquema))
            throw new InvalidOperationException("La copia corresponde a otra base o versión. Para reconstruir una instalación perdida se requiere recuperación técnica en una base separada.");
        var conocidas = await _registro.CopiasAsync(control, ct);
        var preparadas = new List<CopiaRespaldoDto>();
        var creadas = new List<string>();
        try
        {
            foreach (var archivo in paquete.Archivos.OrderBy(a => a.Copia.Tipo))
            {
                var ruta = Path.Combine(carpetaServidor, "importada-" + Guid.NewGuid().ToString("N") + ".bak");
                creadas.Add(ruta);
                await PaqueteRespaldo.CopiarAsync(Path.Combine(origen, archivo.Nombre), ruta, ct);
                if (new FileInfo(ruta).Length != archivo.Longitud || await PaqueteRespaldo.Hash(ruta, ct) != archivo.Sha256)
                    throw new InvalidOperationException("El archivo cambió durante la recuperación. No se registró el paquete.");
                var copia = archivo.Copia with { RutaServidor = ruta, RutaControl = "", Advertencia = "Recuperada de paquete transportable; auxiliar de control no incluido." };
                await VerificarArchivoAsync(master, copia, ct);
                preparadas.Add(copia);
            }
            var completa = preparadas.Single(c => c.Tipo == TipoRespaldo.Completa);
            var diferencial = preparadas.SingleOrDefault(c => c.Tipo == TipoRespaldo.Diferencial);
            if (diferencial != null && (completa.SoloCopia || (await LeerCabeceraAsync(master, diferencial.RutaServidor, ct)).Base != completa.BackupSetGuid))
                throw new InvalidOperationException("La diferencial no depende de la completa incluida.");
            // Registrar toda la cadena o ninguna de sus partes.
            using var transaccion = control.BeginTransaction();
            foreach (var copia in preparadas)
            {
                var existente = conocidas.SingleOrDefault(c => c.Id == copia.Id);
                if (existente != null && existente.BackupSetGuid != copia.BackupSetGuid) throw new InvalidOperationException("Identificador de copia incompatible con el catálogo.");
                using var cmd = control.CreateCommand();
                cmd.Transaction = transaccion;
                cmd.CommandText = existente == null
                    ? "INSERT dbo.CopiasSGTM(Id,BackupSet,FechaUtc,Documento) VALUES(@id,@set,@fecha,@json)"
                    : "UPDATE dbo.CopiasSGTM SET Documento=@json WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", copia.Id);
                cmd.Parameters.AddWithValue("@set", copia.BackupSetGuid);
                cmd.Parameters.AddWithValue("@fecha", copia.RegistradaUtc);
                cmd.Parameters.AddWithValue("@json", System.Text.Json.JsonSerializer.Serialize(copia));
                await cmd.ExecuteNonQueryAsync(ct);
            }
            transaccion.Commit();
            creadas.Clear(); // Los archivos ya pertenecen al catálogo durable.
            await _registro.EventoAsync(control, "PaqueteImportado", "Verificado; pendiente de ensayo", autor, "", paquete.Seleccionada.ToString(), ct);
            return paquete.Seleccionada;
        }
        finally
        {
            foreach (var ruta in creadas) if (File.Exists(ruta)) File.Delete(ruta);
        }
    }
}

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Respaldos;

namespace Taller.Infraestructura.Respaldos;
public sealed partial class RespaldoSqlServer
{
    private sealed record Cabecera(Guid Set, Guid Familia, Guid? Base, string BaseDatos, int Tipo, bool Checksum, bool Danada, bool SoloCopia, DateTime Fecha, int VersionSql);
    private sealed record ArchivoSql(string Nombre, string Ruta, int Tipo);
    private async Task<string> EsquemaAsync(SqlConnection master, string db, CancellationToken ct = default)
    {
        using var cmd = SqlRespaldos.Comando(master, $"SELECT MigrationId FROM {SqlRespaldos.Identificador(db)}.dbo.__EFMigrationsHistory ORDER BY MigrationId");
        using var rd = await cmd.ExecuteReaderAsync(ct);
        var ids = new List<string>();
        while (await rd.ReadAsync(ct))
            ids.Add(rd.GetString(0));
        if (ids.Count == 0)
            throw new InvalidOperationException("La base no posee un esquema SGTM migrado.");
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join("\n", ids))));
    }

    private async Task<Guid> FamiliaAsync(SqlConnection master, CancellationToken ct = default)
    {
        using var cmd = SqlRespaldos.Comando(master, "SELECT family_guid FROM sys.database_recovery_status WHERE database_id=DB_ID(@db)", ("@db", opciones.BaseDatos));
        var value = await cmd.ExecuteScalarAsync(ct);
        return value is Guid guid ? guid : throw new InvalidOperationException("La base no está disponible para comprobar su identidad.");
    }

    private async Task<Cabecera> LeerCabeceraAsync(SqlConnection master, string ruta, CancellationToken ct = default)
    {
        using var cmd = SqlRespaldos.Comando(master, "RESTORE HEADERONLY FROM DISK=@ruta", ("@ruta", ruta));
        using var rd = await cmd.ExecuteReaderAsync(ct);
        if (!await rd.ReadAsync(ct))
            throw new InvalidOperationException("El archivo no contiene una copia nativa de SQL Server.");
        T Valor<T>(string n) => (T)rd[n];
        var cab = new Cabecera(Valor<Guid>("BackupSetGUID"), Valor<Guid>("FamilyGUID"), rd["DifferentialBaseGUID"] is Guid g ? g : null, Valor<string>("DatabaseName"), Convert.ToInt32(rd["BackupType"]), Valor<bool>("HasBackupChecksums"), Valor<bool>("IsDamaged"), Valor<bool>("IsCopyOnly"), Valor<DateTime>("BackupFinishDate"), Convert.ToInt32(rd["SoftwareVersionMajor"]));
        if (await rd.ReadAsync(ct))
            throw new InvalidOperationException("El dispositivo contiene varios conjuntos. Se requieren archivos individuales, uno por copia.");
        return cab;
    }

    private async Task VerificarArchivoAsync(SqlConnection master, CopiaRespaldoDto copia, CancellationToken ct = default)
    {
        var h = await LeerCabeceraAsync(master, copia.RutaServidor, ct);
        int version = await SqlRespaldos.EscalarAsync<int>(master, "SELECT CONVERT(int,SERVERPROPERTY('ProductMajorVersion'))", ct);
        if (h.Set != copia.BackupSetGuid || h.Familia != copia.Familia || !string.Equals(h.BaseDatos, opciones.BaseDatos, StringComparison.OrdinalIgnoreCase) || h.Danada || !h.Checksum || h.VersionSql > version || h.Tipo != (copia.Tipo == TipoRespaldo.Completa ? 1 : 5) || h.SoloCopia != copia.SoloCopia)
            throw new InvalidOperationException("El archivo no coincide con el catálogo, pertenece a otra base, está dañado o no es compatible con este servidor.");
        await SqlRespaldos.EjecutarAsync(master, "RESTORE VERIFYONLY FROM DISK=@ruta WITH FILE=1,CHECKSUM", ct, ("@ruta", copia.RutaServidor));
    }

    private async Task<bool> EsBaseActualAsync(SqlConnection master, CopiaRespaldoDto? copia, CancellationToken ct = default)
    {
        if (copia is null || copia.SoloCopia || copia.Tipo != TipoRespaldo.Completa)
            return false;
        int cantidad = await SqlRespaldos.EscalarAsync<int>(master, $"SELECT COUNT(*) FROM {Db}.sys.database_files WHERE type=0 AND (differential_base_guid IS NULL OR differential_base_guid<>@base)", ct, ("@base", copia.BackupSetGuid));
        return cantidad == 0;
    }

    private async Task<CopiaRespaldoDto> CrearNativaAsync(SqlConnection master, SqlConnection control, TipoRespaldo tipo, string carpeta, string autor, bool protectora, IProgress<string>? progreso, CancellationToken ct)
    {
        var id = Guid.NewGuid();
        var ruta = PoliticaRespaldos.Archivo(carpeta, $"SGTM_{(protectora ? "PROTECTORA" : tipo.ToString().ToUpperInvariant())}_{reloj.GetUtcNow():yyyyMMdd_HHmmss}_{id:N}.bak");
        string esquema = await EsquemaAsync(master, opciones.BaseDatos, ct);
        Guid familia = await FamiliaAsync(master, ct);
        CopiaRespaldoDto? completa = null;
        if (tipo == TipoRespaldo.Diferencial)
        {
            completa = (await _registro.CopiasAsync(control, ct)).FirstOrDefault(x => x.Tipo == TipoRespaldo.Completa && !x.SoloCopia && x.Familia == familia && x.EsquemaHash == esquema);
            if (!await EsBaseActualAsync(master, completa, ct))
                throw new InvalidOperationException("No hay una copia completa base registrada y vigente. Cree una completa antes de solicitar la diferencial.");
            await VerificarArchivoAsync(master, completa!, ct);
        }

        progreso?.Report(protectora ? "SQL Server está creando la copia protectora durante el mantenimiento." : "SQL Server está creando la copia " + tipo + ". La aplicación puede continuar operando.");
        await _registro.EventoAsync(control, "CopiaIniciada", "En curso", autor, "", ruta, ct);
        string opcionesCopia = protectora ? ",COPY_ONLY" : tipo == TipoRespaldo.Diferencial ? ",DIFFERENTIAL" : "";
        await SqlRespaldos.EjecutarAsync(master, $"BACKUP DATABASE {Db} TO DISK=@ruta WITH NOINIT,CHECKSUM,NAME=@nombre{opcionesCopia}", ct, ("@ruta", ruta), ("@nombre", "SGTM-" + id));
        var h = await LeerCabeceraAsync(master, ruta, ct);
        if (h.Familia != familia || h.Tipo != (tipo == TipoRespaldo.Completa ? 1 : 5) || completa != null && h.Base != completa.BackupSetGuid)
            throw new InvalidOperationException("La copia no coincide con la familia o la base diferencial esperada. Se conserva el archivo para revisión técnica.");
        var rutaControl = PoliticaRespaldos.Archivo(carpeta, $"SGTM_CONTROL_{id:N}.bak");
        var copia = new CopiaRespaldoDto
        {
            Id = id,
            BackupSetGuid = h.Set,
            BaseId = completa?.Id,
            Tipo = tipo,
            RutaServidor = ruta,
            RutaControl = rutaControl,
            FechaServidor = h.Fecha,
            RegistradaUtc = reloj.GetUtcNow(),
            EsquemaHash = esquema,
            Familia = familia,
            Autor = autor,
            SoloCopia = protectora
        };
        progreso?.Report("Verificando checksum y lectura de la copia.");
        await VerificarArchivoAsync(master, copia, ct);
        await _registro.CopiaAsync(control, copia, ct);
        await _registro.EventoAsync(control, "CopiaVerificada", "Correcto", autor, "", ruta, ct);
        try
        {
            await SqlRespaldos.EjecutarAsync(master, $"BACKUP DATABASE {SqlRespaldos.Identificador(opciones.BaseControl)} TO DISK=@ruta WITH NOINIT,COPY_ONLY,CHECKSUM,NAME=@nombre", ct, ("@ruta", rutaControl), ("@nombre", "SGTM-Control-" + id));
            await SqlRespaldos.EjecutarAsync(master, "RESTORE VERIFYONLY FROM DISK=@ruta WITH FILE=1,CHECKSUM", ct, ("@ruta", rutaControl));
        }
        catch (Exception e)
        {
            await _registro.EventoAsync(control, "CopiaControl", "Fallida", autor, "", $"La copia de negocio está verificada; falló su copia auxiliar de catálogo: {e.Message}", CancellationToken.None);
            copia = copia with
            {
                Advertencia = "La copia principal está verificada, pero falló la copia auxiliar de control. Revise el historial y contacte al técnico."
            };
            await SqlRespaldos.EjecutarAsync(control, "UPDATE dbo.CopiasSGTM SET Documento=@json WHERE Id=@id", CancellationToken.None, ("@id", copia.Id), ("@json", JsonSerializer.Serialize(copia)));
        }

        return copia;
    }

    private async Task<List<ArchivoSql>> ArchivosActualesAsync(SqlConnection master, CancellationToken ct = default)
    {
        using var cmd = SqlRespaldos.Comando(master, "SELECT name,physical_name,type FROM sys.master_files WHERE database_id=DB_ID(@db) ORDER BY file_id", ("@db", opciones.BaseDatos));
        using var rd = await cmd.ExecuteReaderAsync(ct);
        var archivos = new List<ArchivoSql>();
        while (await rd.ReadAsync(ct))
            archivos.Add(new(rd.GetString(0), rd.GetString(1), Convert.ToInt32(rd[2])));
        if (archivos.Count < 2 || archivos.Any(a => a.Tipo is not (0 or 1)))
            throw new InvalidOperationException("Esta recuperación admite archivos de datos y log convencionales. FILESTREAM u otros diseños requieren personal especializado.");
        return archivos;
    }

    private async Task ComprobarArchivosAsync(SqlConnection master, CopiaRespaldoDto copia, List<ArchivoSql> actuales, CancellationToken ct = default)
    {
        using var cmd = SqlRespaldos.Comando(master, "RESTORE FILELISTONLY FROM DISK=@ruta WITH FILE=1", ("@ruta", copia.RutaServidor));
        using var rd = await cmd.ExecuteReaderAsync(ct);
        var originales = new List<(string, string)>();
        while (await rd.ReadAsync(ct))
            originales.Add(((string)rd["LogicalName"], (string)rd["Type"]));
        if (originales.Count != actuales.Count || originales.Any(a => !actuales.Any(b => b.Nombre == a.Item1 && (b.Tipo == 0 ? "D" : "L") == a.Item2)))
            throw new InvalidOperationException("La estructura de archivos cambió. Se requiere recuperar en otra base y revisar con personal especializado.");
    }

    private async Task RestaurarNativaAsync(SqlConnection master, string destino, CopiaRespaldoDto copia, List<ArchivoSql> archivos, bool recuperar, CancellationToken ct = default)
    {
        string movimientos = string.Join(",", archivos.Select(a => "MOVE " + SqlRespaldos.Literal(a.Nombre) + " TO " + SqlRespaldos.Literal(a.Ruta)));
        await SqlRespaldos.EjecutarAsync(master, $"RESTORE DATABASE {SqlRespaldos.Identificador(destino)} FROM DISK=@ruta WITH FILE=1,{movimientos},REPLACE,CHECKSUM,{(recuperar ? "RECOVERY" : "NORECOVERY")}", ct, ("@ruta", copia.RutaServidor));
    }

    private async Task IntegridadAsync(SqlConnection master, string db, CancellationToken ct = default)
    {
        using var cmd = SqlRespaldos.Comando(master, $"DBCC CHECKDB({SqlRespaldos.Identificador(db)}) WITH TABLERESULTS,NO_INFOMSGS,ALL_ERRORMSGS");
        using var rd = await cmd.ExecuteReaderAsync(ct);
        do
        {
            if (await rd.ReadAsync(ct))
                throw new InvalidOperationException("CHECKDB informó errores. No se aplicaron reparaciones ni pérdida de datos automática; contacte al personal técnico.");
        }
        while (await rd.NextResultAsync(ct));
    }
}

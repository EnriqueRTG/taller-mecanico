using System.Text.Json;
using Microsoft.Data.SqlClient;
using Taller.Aplicacion.DTOs.Respaldos;
namespace Taller.Infraestructura.Respaldos;
/// <summary>Catálogo y auditoría duraderos en una base separada. Restaurar negocio no revierte aprobaciones ni planificación.</summary>
public sealed class RegistroRespaldosSql(OpcionesRespaldos opciones)
{
    internal const string Esquema = """
        IF OBJECT_ID(N'dbo.EstadoSGTM') IS NULL CREATE TABLE dbo.EstadoSGTM(Id int NOT NULL PRIMARY KEY CHECK(Id=1), BaseDatos nvarchar(128) NOT NULL, VersionEsquema int NOT NULL, Generacion bigint NOT NULL, Mantenimiento bit NOT NULL, Operacion uniqueidentifier NULL, Detalle nvarchar(2000) NOT NULL);
        IF OBJECT_ID(N'dbo.ProgramaSGTM') IS NULL CREATE TABLE dbo.ProgramaSGTM(Id int NOT NULL PRIMARY KEY CHECK(Id=1), Documento nvarchar(max) NOT NULL CHECK(ISJSON(Documento)=1), Version bigint NOT NULL, SenalUtc datetimeoffset NULL, Resultado nvarchar(2000) NOT NULL);
        IF OBJECT_ID(N'dbo.CopiasSGTM') IS NULL CREATE TABLE dbo.CopiasSGTM(Id uniqueidentifier NOT NULL PRIMARY KEY, BackupSet uniqueidentifier NOT NULL UNIQUE, FechaUtc datetimeoffset NOT NULL, Documento nvarchar(max) NOT NULL CHECK(ISJSON(Documento)=1));
        IF OBJECT_ID(N'dbo.EventosSGTM') IS NULL CREATE TABLE dbo.EventosSGTM(Id bigint IDENTITY PRIMARY KEY, FechaUtc datetimeoffset NOT NULL DEFAULT(SYSUTCDATETIME()), Accion nvarchar(100) NOT NULL, Resultado nvarchar(100) NOT NULL, Autor nvarchar(250) NOT NULL, Segundo nvarchar(250) NOT NULL, Detalle nvarchar(2000) NOT NULL, Operacion uniqueidentifier NULL);
        IF COL_LENGTH(N'dbo.EventosSGTM',N'Operacion') IS NULL ALTER TABLE dbo.EventosSGTM ADD Operacion uniqueidentifier NULL;
        IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name=N'UX_EventosSGTM_Operacion' AND object_id=OBJECT_ID(N'dbo.EventosSGTM')) CREATE UNIQUE INDEX UX_EventosSGTM_Operacion ON dbo.EventosSGTM(Operacion) WHERE Operacion IS NOT NULL;
        """;
    /// <summary>Instalación explícita por personal con permisos SQL. Nunca se ejecuta al iniciar un cliente.</summary>
    public async Task InstalarAsync(CancellationToken ct=default)
    {
        await using(var master=await SqlRespaldos.AbrirAsync(opciones.Master(),ct))
        {
            int existe=await SqlRespaldos.EscalarAsync<int>(master,"SELECT COUNT(*) FROM sys.databases WHERE name=@db",ct,("@db",opciones.BaseControl));
            if(existe==0)await SqlRespaldos.EjecutarAsync(master,"CREATE DATABASE "+SqlRespaldos.Identificador(opciones.BaseControl),ct);
        }
        await using var c=await SqlRespaldos.AbrirAsync(opciones.Control(),ct);
        await SqlRespaldos.EjecutarAsync(c,Esquema,ct);
        await SqlRespaldos.EjecutarAsync(c,"IF NOT EXISTS(SELECT 1 FROM dbo.EstadoSGTM WHERE Id=1) INSERT dbo.EstadoSGTM VALUES(1,@db,1,1,0,NULL,N'Instalación preparada');",ct,("@db",opciones.BaseDatos));
        await ComprobarAsync(c,ct);
        string json=JsonSerializer.Serialize(new ProgramaRespaldoDto());
        await SqlRespaldos.EjecutarAsync(c,"IF NOT EXISTS(SELECT 1 FROM dbo.ProgramaSGTM WHERE Id=1) INSERT dbo.ProgramaSGTM VALUES(1,@json,0,NULL,N'Sin ejecuciones');",ct,("@json",json));
    }
    internal async Task<(bool Mantenimiento,long Generacion,string Detalle)> ComprobarAsync(SqlConnection c,CancellationToken ct=default)
    {
        using var cmd=SqlRespaldos.Comando(c,"SELECT BaseDatos,VersionEsquema,Mantenimiento,Generacion,Detalle FROM dbo.EstadoSGTM WHERE Id=1");
        using var rd=await cmd.ExecuteReaderAsync(ct);
        if(!await rd.ReadAsync(ct)||!string.Equals(rd.GetString(0),opciones.BaseDatos,StringComparison.OrdinalIgnoreCase)||rd.GetInt32(1)!=1)throw new InvalidOperationException("La base de control no corresponde a esta instalación o requiere actualización técnica.");
        return(rd.GetBoolean(2),rd.GetInt64(3),rd.GetString(4));
    }
    internal async Task<List<CopiaRespaldoDto>> CopiasAsync(SqlConnection c,CancellationToken ct=default)
    {
        using var cmd=SqlRespaldos.Comando(c,"SELECT Documento FROM dbo.CopiasSGTM ORDER BY FechaUtc DESC");using var rd=await cmd.ExecuteReaderAsync(ct);var resultado=new List<CopiaRespaldoDto>();
        while(await rd.ReadAsync(ct))resultado.Add(JsonSerializer.Deserialize<CopiaRespaldoDto>(rd.GetString(0))??throw new InvalidOperationException("Catálogo inválido."));return resultado;
    }
    internal async Task CopiaAsync(SqlConnection c,CopiaRespaldoDto copia,CancellationToken ct=default)=>await SqlRespaldos.EjecutarAsync(c,"INSERT dbo.CopiasSGTM(Id,BackupSet,FechaUtc,Documento) VALUES(@id,@set,@fecha,@json)",ct,("@id",copia.Id),("@set",copia.BackupSetGuid),("@fecha",copia.RegistradaUtc),("@json",JsonSerializer.Serialize(copia)));
    internal async Task EventoAsync(SqlConnection c,string accion,string resultado,string autor,string segundo,string detalle,CancellationToken ct=default)
    {
        await SqlRespaldos.EjecutarAsync(c,"INSERT dbo.EventosSGTM(Accion,Resultado,Autor,Segundo,Detalle) VALUES(@accion,@resultado,@autor,@segundo,@detalle)",ct,("@accion",accion),("@resultado",resultado),("@autor",autor),("@segundo",segundo),("@detalle",detalle.Length>2000?detalle[..2000]:detalle));
    }
    internal async Task<List<EventoRespaldoDto>> EventosAsync(SqlConnection c,CancellationToken ct=default)
    {
        using var cmd=SqlRespaldos.Comando(c,"SELECT TOP(200) FechaUtc,Accion,Resultado,Autor,Segundo,Detalle FROM dbo.EventosSGTM ORDER BY Id DESC");using var rd=await cmd.ExecuteReaderAsync(ct);var eventos=new List<EventoRespaldoDto>();
        while(await rd.ReadAsync(ct))eventos.Add(new(rd.GetFieldValue<DateTimeOffset>(0),rd.GetString(1),rd.GetString(2),rd.GetString(3),rd.GetString(4),rd.GetString(5)));return eventos;
    }
    internal async Task<ProgramaRespaldoDto> ProgramaAsync(SqlConnection c,CancellationToken ct=default)
    {
        using var cmd=SqlRespaldos.Comando(c,"SELECT Documento,Version,SenalUtc,Resultado FROM dbo.ProgramaSGTM WHERE Id=1");using var rd=await cmd.ExecuteReaderAsync(ct);
        if(!await rd.ReadAsync(ct))throw new InvalidOperationException("Falta instalar la programación de respaldos.");
        var p=JsonSerializer.Deserialize<ProgramaRespaldoDto>(rd.GetString(0))??throw new InvalidOperationException("Configuración inválida.");
        return p with {Version=rd.GetInt64(1),UltimaSenalAgenteUtc=rd.IsDBNull(2)?null:rd.GetFieldValue<DateTimeOffset>(2),UltimoResultado=rd.GetString(3)};
    }
    internal async Task GuardarProgramaAsync(SqlConnection c,ProgramaRespaldoDto p,CancellationToken ct=default)
    {
        using var cmd=SqlRespaldos.Comando(c,"UPDATE dbo.ProgramaSGTM SET Documento=@json,Version=Version+1 WHERE Id=1 AND Version=@version",("@json",JsonSerializer.Serialize(p)),("@version",p.Version));
        if(await cmd.ExecuteNonQueryAsync(ct)!=1)throw new InvalidOperationException("La programación cambió en otra sesión. Actualice antes de guardar.");
    }
    internal Task ConfirmarFinAsync(SqlConnection c,Guid operacion,string resultado,string autor,string segundo,string detalle,string estado)
    {
        const string sql="""
            SET XACT_ABORT ON;
            BEGIN TRANSACTION;
            IF NOT EXISTS(SELECT 1 FROM dbo.EventosSGTM WITH(UPDLOCK,HOLDLOCK) WHERE Operacion=@op)
            BEGIN
                IF NOT EXISTS(SELECT 1 FROM dbo.EstadoSGTM WITH(UPDLOCK,HOLDLOCK) WHERE Id=1 AND Operacion=@op AND Mantenimiento=1)
                    THROW 51110,'La operación de recuperación no coincide con el mantenimiento registrado.',1;
                INSERT dbo.EventosSGTM(Accion,Resultado,Autor,Segundo,Detalle,Operacion) VALUES(N'Restauracion',@resultado,@autor,@segundo,@detalle,@op);
                UPDATE dbo.EstadoSGTM SET Mantenimiento=0,Generacion=Generacion+1,Operacion=NULL,Detalle=@estado WHERE Id=1 AND Operacion=@op;
            END;
            COMMIT;
            """;
        return SqlRespaldos.EjecutarAsync(c,sql,default,("@op",operacion),("@resultado",resultado),("@autor",autor),("@segundo",segundo),("@detalle",detalle.Length>2000?detalle[..2000]:detalle),("@estado",estado));
    }
    internal Task EstadoAsync(SqlConnection c,bool mantenimiento,string detalle,bool incrementar=false,Guid? operacion=null,CancellationToken ct=default)=>SqlRespaldos.EjecutarAsync(c,"UPDATE dbo.EstadoSGTM SET Mantenimiento=@m,Detalle=@d,Operacion=@op,Generacion=Generacion+@inc WHERE Id=1",ct,("@m",mantenimiento),("@d",detalle),("@op",operacion),("@inc",incrementar?1:0));
}

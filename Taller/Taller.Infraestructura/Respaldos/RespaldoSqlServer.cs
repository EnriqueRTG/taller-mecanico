using Microsoft.Data.SqlClient;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Respaldos;
namespace Taller.Infraestructura.Respaldos;
/// <summary>Coordina copias verificadas y recuperación nativa; usa conexiones dedicadas a master/control, nunca el contexto de negocio.</summary>
public sealed partial class RespaldoSqlServer(OpcionesRespaldos opciones,TimeProvider reloj) : IRespaldoMotor
{
    private readonly RegistroRespaldosSql _registro=new(opciones);
    public bool Habilitado=>opciones.Habilitado;
    private string Db=>SqlRespaldos.Identificador(opciones.BaseDatos);
    private async Task<SqlConnection> ControlAsync(CancellationToken ct=default)
    { opciones.ExigirHabilitado();var c=await SqlRespaldos.AbrirAsync(opciones.Control(),ct);try{await _registro.ComprobarAsync(c,ct);return c;}catch{await c.DisposeAsync();throw;} }
    private async Task<SqlConnection> AccesoNegocioAsync(CancellationToken ct=default)
    {
        var c=await SqlRespaldos.BloquearAsync(opciones,opciones.RecursoMantenimiento,"Shared",0,ct);
        try{if((await _registro.ComprobarAsync(c,ct)).Mantenimiento)throw new InvalidOperationException("No se puede iniciar una copia o verificación mientras hay mantenimiento pendiente.");return c;}catch{await c.DisposeAsync();throw;}
    }
    public async Task<EstadoRespaldosDto> ConsultarAsync(CancellationToken cancelacion=default)
    {
        await using var c=await ControlAsync(cancelacion);var e=await _registro.ComprobarAsync(c,cancelacion);
        return new(opciones.BaseDatos,e.Mantenimiento,e.Generacion,e.Detalle,await _registro.CopiasAsync(c,cancelacion),await _registro.EventosAsync(c,cancelacion));
    }
    public async Task<ProgramaRespaldoDto> ObtenerProgramaAsync(CancellationToken cancelacion=default)
    {await using var c=await ControlAsync(cancelacion);return await _registro.ProgramaAsync(c,cancelacion);}
    public async Task GuardarProgramaAsync(ProgramaRespaldoDto p,CancellationToken cancelacion=default)
    {
        PoliticaRespaldos.Zona(p);await using var c=await ControlAsync(cancelacion);
        await using var bloqueo=await SqlRespaldos.BloquearAsync(opciones,opciones.RecursoCopias,"Exclusive",0,cancelacion);
        if((await _registro.ComprobarAsync(c,cancelacion)).Mantenimiento)throw new InvalidOperationException("La programación no se modifica durante una recuperación.");
        await _registro.GuardarProgramaAsync(c,p,cancelacion);await _registro.EventoAsync(c,"ProgramaModificado","Correcto",p.Autor,"",p.Activo?$"Próxima ejecución UTC: {p.ProximaUtc:O}. Zona: {p.ZonaHoraria}. Carpeta: {p.CarpetaServidor}":"Programación desactivada",cancelacion);
    }
    public async Task<CopiaRespaldoDto> CrearAsync(TipoRespaldo tipo,string carpeta,string autor,IProgress<string>? progreso=null,CancellationToken cancelacion=default)
    {
        opciones.ExigirHabilitado();PoliticaRespaldos.Tipo(tipo);carpeta=PoliticaRespaldos.Carpeta(carpeta);
        await using var control=await ControlAsync(cancelacion);
        await using var bloqueo=await SqlRespaldos.BloquearAsync(opciones,opciones.RecursoCopias,"Exclusive",0,cancelacion);
        await using var acceso=await AccesoNegocioAsync(cancelacion);
        await using var master=await SqlRespaldos.AbrirAsync(opciones.Master(),cancelacion);
        try{return await CrearNativaAsync(master,control,tipo,carpeta,autor,false,progreso,cancelacion);}
        catch(Exception e){await _registro.EventoAsync(control,"CopiaSolicitada","Fallida",autor,"",e.Message,CancellationToken.None);throw;}
    }
}

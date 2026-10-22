using System.Data;
using Microsoft.Data.SqlClient;
namespace Taller.Infraestructura.Respaldos;
/// <summary>Infraestructura SQL compartida: valores parametrizados e identificadores/literales escapados donde la gramática no admite parámetros.</summary>
public static class SqlRespaldos
{
    public static string Identificador(string nombre)
    {
        if(string.IsNullOrWhiteSpace(nombre)||nombre.Length>128||nombre.Any(char.IsControl)||new[]{"master","model","msdb","tempdb"}.Contains(nombre,StringComparer.OrdinalIgnoreCase))throw new InvalidOperationException("La operación requiere una base de usuario con nombre válido; no admite bases del sistema.");
        return "["+nombre.Replace("]","]]")+"]";
    }
    public static string Literal(string valor)=>"N'"+valor.Replace("'","''")+"'";
    internal static SqlCommand Comando(SqlConnection c,string sql,params (string Nombre,object? Valor)[] parametros)
    {
        var cmd=new SqlCommand(sql,c) { CommandTimeout=3600 };
        foreach(var p in parametros)cmd.Parameters.AddWithValue(p.Nombre,p.Valor??DBNull.Value);
        return cmd;
    }
    internal static async Task<SqlConnection> AbrirAsync(string cadena,CancellationToken ct=default)
    { var c=new SqlConnection(cadena);try{await c.OpenAsync(ct);return c;}catch{await c.DisposeAsync();throw;} }
    internal static async Task EjecutarAsync(SqlConnection c,string sql,CancellationToken ct=default,params (string,object?)[] parametros)
    { using var cmd=Comando(c,sql,parametros);await cmd.ExecuteNonQueryAsync(ct); }
    internal static async Task<T> EscalarAsync<T>(SqlConnection c,string sql,CancellationToken ct=default,params (string,object?)[] parametros)
    { using var cmd=Comando(c,sql,parametros);var v=await cmd.ExecuteScalarAsync(ct);return v is null or DBNull ? default! : (T)Convert.ChangeType(v,typeof(T)); }
    internal static async Task<SqlConnection> BloquearAsync(OpcionesRespaldos o,string recurso,string modo,int esperaMs,CancellationToken ct=default)
    {
        var c=await AbrirAsync(o.Control(),ct);
        try
        {
            using var cmd=Comando(c,"DECLARE @r int; EXEC @r=sys.sp_getapplock @Resource=@recurso,@LockMode=@modo,@LockOwner='Session',@LockTimeout=@espera,@DbPrincipal='public'; SELECT @r;",("@recurso",recurso),("@modo",modo),("@espera",esperaMs));cmd.CommandTimeout=Math.Max(20,esperaMs/1000+5);
            int resultado=Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));if(resultado<0)throw new InvalidOperationException("Hay otra operación o actividad en curso. Espere a que termine; no se desconectaron usuarios ni se revirtieron transacciones.");
            return c;
        }catch{await c.DisposeAsync();throw;}
    }
}

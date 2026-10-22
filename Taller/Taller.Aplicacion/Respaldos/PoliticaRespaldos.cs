using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;

namespace Taller.Aplicacion.Respaldos;
/// <summary>Reglas puras de tipos, rutas del servidor, calendario y doble aprobación.</summary>
public static class PoliticaRespaldos
{
    public static string Carpeta(string? valor)
    {
        var p = (valor ?? "").Trim().TrimEnd('\\', '/');
        if (p.Length == 0 || p.Length > 145 || p.Any(char.IsControl) || p.Split(['/', '\\']).Any(s => s == "..") || !(p.StartsWith('/') || p.StartsWith("\\\\") || Regex.IsMatch(p, @"^[A-Za-z]:\\")) || p.IndexOfAny(['*', '?', '"', '<', '>', '|']) >= 0)
            throw new ValidacionException("Ingrese una carpeta absoluta del servidor SQL, sin .., comodines ni controles y de hasta 145 caracteres. Debe existir y permitir escritura a SQL Server.");
        return p;
    }

    public static string Archivo(string carpeta, string nombre) => Carpeta(carpeta) + (carpeta.StartsWith('/') ? "/" : "\\") + nombre;
    public static void Tipo(TipoRespaldo tipo)
    {
        if (!Enum.IsDefined(tipo))
            throw new ValidacionException("Seleccione completa o diferencial.");
    }

    public static TimeZoneInfo Zona(ProgramaRespaldoDto p)
    {
        Carpeta(p.CarpetaServidor);
        if (p.MinutoDelDia is < 0 or >= 1440 || !Enum.IsDefined(p.DiaCompleta))
            throw new ValidacionException("Revise el horario y el día de la copia completa.");
        try
        {
            return ResolverZona(p.ZonaHoraria);
        }
        catch (Exception e)when (e is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            throw new ValidacionException("La zona horaria configurada no está disponible.");
        }
    }

    public static TimeZoneInfo ResolverZona(string id)
    {
        var candidatos = id is "Argentina Standard Time" or "America/Argentina/Buenos_Aires" or "America/Buenos_Aires" ? new[]
        {
            id,
            "America/Argentina/Buenos_Aires",
            "Argentina Standard Time",
            "America/Buenos_Aires"
        }

        : new[]
        {
            id
        };
        foreach (var candidato in candidatos)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(candidato);
            }
            catch (TimeZoneNotFoundException)
            {
            }
        }

        throw new TimeZoneNotFoundException("Zona horaria no disponible: " + id);
    }

    public static DateTimeOffset Proxima(ProgramaRespaldoDto p, DateTimeOffset ahora)
    {
        var zona = Zona(p);
        var local = TimeZoneInfo.ConvertTime(ahora, zona).Date;
        for (int i = 0; i < 370; i++)
        {
            var candidato = DateTime.SpecifyKind(local.AddDays(i).AddMinutes(p.MinutoDelDia), DateTimeKind.Unspecified);
            if (zona.IsInvalidTime(candidato))
                continue;
            var utc = new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(candidato, zona), TimeSpan.Zero);
            if (utc > ahora)
                return utc;
        }

        throw new ValidacionException("No se pudo determinar la próxima ejecución.");
    }

    public static TipoRespaldo TipoProgramado(ProgramaRespaldoDto p, DateTimeOffset ahora, bool baseDisponible) => !p.Diferenciales || !baseDisponible || TimeZoneInfo.ConvertTime(ahora, Zona(p)).DayOfWeek == p.DiaCompleta ? TipoRespaldo.Completa : TipoRespaldo.Diferencial;
    public static string Huella(string hash) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(hash)));
    public static void Aprobadores(AprobadorRestauracionDto primero, AprobadorRestauracionDto segundo)
    {
        if (primero.Id <= 0 || segundo.Id <= 0 || primero.Id == segundo.Id || primero.RolId != RolesSistema.AdministradorId || segundo.RolId is not (RolesSistema.AdministradorId or RolesSistema.AdministrativoId))
            throw new ValidacionException("Se requiere el administrador solicitante y otra persona distinta: administrador o administrativo, ambos activos y autenticados.");
    }
}

namespace Taller.Aplicacion.DTOs.Respaldos;
/// <summary>Plan diario en una zona explícita. Permite completas diarias o completa semanal con diferenciales diarias.</summary>
public sealed record ProgramaRespaldoDto
{
    public bool Activo { get; init; }
    public bool Diferenciales { get; init; } = true;
    public DayOfWeek DiaCompleta { get; init; } = DayOfWeek.Sunday;
    public int MinutoDelDia { get; init; } = 120;
    public string ZonaHoraria { get; init; } = "Argentina Standard Time";
    public string CarpetaServidor { get; init; } = "";
    public int IdAdministrador { get; init; }
    public string Autor { get; init; } = "";
    public DateTimeOffset? ProximaUtc { get; init; }
    public DateTimeOffset? UltimaSenalAgenteUtc { get; init; }
    public string UltimoResultado { get; init; } = "Sin ejecuciones";
    public long Version { get; init; }
}

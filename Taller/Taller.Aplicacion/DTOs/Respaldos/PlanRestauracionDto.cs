namespace Taller.Aplicacion.DTOs.Respaldos;
/// <summary>Plan verificado mediante restauración temporal y CHECKDB, ligado a un usuario y con caducidad.</summary>
public sealed record PlanRestauracionDto
{
    public Guid Token { get; init; }
    public int IdSolicitante { get; init; }
    public DateTimeOffset VenceUtc { get; init; }
    public CopiaRespaldoDto Completa { get; init; } = new();
    public CopiaRespaldoDto? Diferencial { get; init; }
    public string Resumen { get; init; } = "";
    public string EsquemaEsperado { get; init; } = "";
}

namespace Taller.Aplicacion.DTOs.Respaldos;
/// <summary>Catálogo externo a la base recuperable; rutas pertenecen al servidor SQL, no al equipo cliente.</summary>
public sealed record CopiaRespaldoDto
{
    public Guid Id { get; init; }
    public Guid BackupSetGuid { get; init; }
    public Guid? BaseId { get; init; }
    public TipoRespaldo Tipo { get; init; }
    public string RutaServidor { get; init; } = "";
    public string RutaControl { get; init; } = "";
    public DateTime FechaServidor { get; init; }
    public DateTimeOffset RegistradaUtc { get; init; }
    public string EsquemaHash { get; init; } = "";
    public Guid Familia { get; init; }
    public string Autor { get; init; } = "";
    public bool SoloCopia { get; init; }
    public string Advertencia { get; init; } = "";
}

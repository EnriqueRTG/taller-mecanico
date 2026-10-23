namespace Taller.Aplicacion.DTOs.Respaldos;
/// <summary>Catálogo externo a la base recuperable; rutas pertenecen al servidor SQL, no al equipo cliente.</summary>
public sealed record CopiaRespaldoDto
{
    public DateTimeOffset? UltimoIntentoAzureUtc { get; init; }
    public bool AzureSolicitado { get; init; }
    public string EstadoAzure { get; init; } = "No solicitado";
    public string ContenedorAzure { get; init; } = "";
    public string BlobAzure { get; init; } = "";
    public string Sha256 { get; init; } = "";
    public string ETagAzure { get; init; } = "";
    public long Longitud { get; init; }
    public DateTimeOffset? VerificadaAzureUtc { get; init; }
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

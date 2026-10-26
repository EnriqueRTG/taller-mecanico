using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Diagnosticos;

/// <summary>
/// Contiene los datos de consulta de un informe técnico registrado.
/// </summary>
public sealed class DiagnosticoDetalleDto
{
    public int IdDiagnostico { get; init; }
    public int IdAtencion { get; init; }
    public DateTime FechaDiagnostico { get; init; }

    public string TecnicoAutor { get; init; } = string.Empty;
    public string FallaDetectada { get; init; } = string.Empty;
    public string TrabajoRecomendado { get; init; } = string.Empty;

    public int? Kilometraje { get; init; }
    public string? Observaciones { get; init; }

}

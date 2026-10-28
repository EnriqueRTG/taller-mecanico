namespace Taller.Aplicacion.DTOs.Reportes;
/// <summary>Documento generado e inmutable por convención: filtros, indicadores, gráfico, interpretación y detalle de una misma consulta.</summary>
public sealed class ReporteDocumentoDto
{
    /// <summary>Opciones válidas del tipo de reporte, independientes de las filas y de los filtros.</summary>
    public IReadOnlyList<string> EstadosDisponibles { get; init; } = [];
    public string Titulo { get; init; } = "";
    public string EmitidoPor { get; init; } = "";
    public DateTime Generado { get; init; }
    public string Alcance { get; init; } = "";
    public string Filtros { get; init; } = "";
    public string Metodologia { get; init; } = "";
    public string Recomendacion { get; init; } = "";
    public string UnidadGrafico { get; init; } = "";
    public IReadOnlyList<IndicadorReporteDto> Indicadores { get; init; } = [];
    public IReadOnlyList<PuntoReporteDto> Grafico { get; init; } = [];
    public IReadOnlyList<ReporteRegistroDto> Registros { get; init; } = [];
}

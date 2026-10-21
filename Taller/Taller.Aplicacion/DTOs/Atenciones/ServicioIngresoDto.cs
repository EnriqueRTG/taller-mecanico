namespace Taller.Aplicacion.DTOs.Atenciones;

/// <summary>
/// Presenta un servicio disponible para seleccionar en el ingreso.
/// </summary>
public sealed class ServicioIngresoDto
{
    public int IdConcepto { get; init; }

    public string Nombre { get; init; } = string.Empty;

    public string? Descripcion { get; init; }

    public string Categoria { get; init; } = string.Empty;

    /// <summary>
    /// Importe sugerido. Si es null, debe establecerse
    /// expresamente al preparar el ingreso.
    /// </summary>
    public decimal? PrecioReferencia { get; init; }
}
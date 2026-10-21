namespace Taller.Aplicacion.DTOs.Atenciones;

/// <summary>
/// Contiene las condiciones propuestas para un servicio de ingreso.
/// </summary>
public sealed class ServicioIngresoSeleccionadoDto
{
    public int IdConcepto { get; init; }

    public decimal Cantidad { get; init; } = 1m;

    public decimal PrecioUnitario { get; init; }
}
using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;
/// <summary>Datos del circuito técnico y su propuesta económica.</summary>
public sealed class ServicioEvaluadoDto
{
    public int IdDetalle { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public EstadoEjecucionDetalle Estado { get; set; }
    public string? Observaciones { get; set; }
}

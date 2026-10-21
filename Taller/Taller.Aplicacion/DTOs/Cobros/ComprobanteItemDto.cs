using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Cobros;
/// <summary>Contrato de datos para cobros, independiente de los controles de presentación.</summary>
public sealed class ComprobanteItemDto
{
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

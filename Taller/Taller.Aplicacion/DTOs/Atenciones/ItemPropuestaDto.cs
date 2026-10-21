using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;
/// <summary>Datos del circuito técnico y su propuesta económica.</summary>
public sealed class ItemPropuestaDto
{
    public int IdDetalle { get; set; }
    public bool? AceptadoPorCliente { get; set; }
    public int IdConcepto { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; } = 1;
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => decimal.Round(Cantidad * PrecioUnitario, 2, MidpointRounding.AwayFromZero);
}

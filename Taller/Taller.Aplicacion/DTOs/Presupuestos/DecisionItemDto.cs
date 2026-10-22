using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Presupuestos;
/// <summary>Contrato de datos para presupuestos, independiente de los controles de presentación.</summary>
public sealed class DecisionItemDto
{
    public int IdConcepto { get; set; }
    public OrigenMaterial OrigenMaterial { get; set; }
    public int IdDetalle { get; set; }
    public bool Aceptar { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => decimal.Round(Cantidad * PrecioUnitario, 2, MidpointRounding.AwayFromZero);
}

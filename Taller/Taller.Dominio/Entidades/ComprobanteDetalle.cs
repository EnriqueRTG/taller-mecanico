namespace Taller.Dominio.Entidades;
/// <summary>Registro persistente del circuito de liquidación y cobros internos, sin integración fiscal.</summary>
public sealed class ComprobanteDetalle
{
    public int IdComprobanteDetalle { get; set; }
    public int IdComprobante { get; set; }
    public Comprobante Comprobante { get; set; } = null!;
    public int IdPresupuestoDetalle { get; set; }
    public PresupuestoDetalle Origen { get; set; } = null!;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

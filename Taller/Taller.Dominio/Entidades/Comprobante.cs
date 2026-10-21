namespace Taller.Dominio.Entidades;
/// <summary>Registro persistente del circuito de liquidación y cobros internos, sin integración fiscal.</summary>
public sealed class Comprobante
{
    public int IdComprobante { get; set; }
    public int IdAtencion { get; set; }
    public Atencion Atencion { get; set; } = null!;
    public int IdUsuarioEmision { get; set; }
    public Usuario UsuarioEmision { get; set; } = null!;
    public DateTime FechaEmision { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string DocumentoCliente { get; set; } = string.Empty;
    public string Dominio { get; set; } = string.Empty;
    public string Moneda { get; set; } = "ARS";
    public decimal Total { get; set; }
    public decimal TotalPagado { get; set; }
    public string? Observaciones { get; set; }
    public byte[] Version { get; set; } = [];
    public ICollection<ComprobanteDetalle> Detalles { get; set; } = new List<ComprobanteDetalle>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}

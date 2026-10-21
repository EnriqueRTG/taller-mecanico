namespace Taller.Dominio.Entidades;
/// <summary>Registro persistente del circuito de liquidación y cobros internos, sin integración fiscal.</summary>
public sealed class PagoMedio
{
    public int IdPagoMedio { get; set; }
    public int IdPago { get; set; }
    public Pago Pago { get; set; } = null!;
    public string NombreMedio { get; set; } = string.Empty;
    public int Orden { get; set; }
    public int IdMedioPago { get; set; }
    public MedioPago MedioPago { get; set; } = null!;
    public decimal Importe { get; set; }
    public string? Referencia { get; set; }
}

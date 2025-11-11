namespace Taller.Domain.Entities
{
    /// <summary>
    /// Pago asociado a una factura (permite pagos parciales).
    /// </summary>
    public sealed class Pago
    {
        public long Id { get; init; }
        public long FacturaId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Medio { get; set; } = "Efectivo";
        public decimal Monto { get; set; }
        public string? Ref { get; set; }
    }
}

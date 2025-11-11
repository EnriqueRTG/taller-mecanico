namespace Taller.Domain.Entities
{
    /// <summary>
    /// Factura emitida por una OT. Total debería reflejar la suma de ítems.
    /// </summary>
    public sealed class Factura
    {
        public long Id { get; init; }
        public string Nro { get; set; } = null!;
        public long OrdenId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public string? Observaciones { get; set; }
    }
}

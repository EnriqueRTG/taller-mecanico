using Taller.Domain.Enums;

namespace Taller.Domain.Entities
{
    /// <summary>
    /// Ítem de una OT. Si Tipo=REP, ReferenciaId apunta a Repuesto.Id; si Tipo=MO, a Servicio.Id.
    /// </summary>
    public sealed class OrdenItem
    {
        public long Id { get; init; }
        public long OrdenId { get; set; }
        public ItemTipo Tipo { get; set; }
        public long? ReferenciaId { get; set; }   // Repuesto.Id o Servicio.Id según Tipo
        public string Descripcion { get; set; } = null!;
        public decimal Cantidad { get; set; }     // horas (MO) o unidades (REP)
        public decimal PrecioUnit { get; set; }
        public decimal Subtotal => Math.Round(Cantidad * PrecioUnit, 2);
    }
}

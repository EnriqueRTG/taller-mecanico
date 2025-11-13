using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Domain.Entities;

namespace Taller.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración para <see cref="OrdenItem"/>: MO/REP, cantidades y precios.
    /// </summary>
    public sealed class OrdenItemConfig : IEntityTypeConfiguration<OrdenItem>
    {
        public void Configure(EntityTypeBuilder<OrdenItem> e)
        {
            e.ToTable("ordenes_items");
            e.HasKey(x => x.Id);

            // Tipo (enum) como int (definido en Enums.ItemTipo)
            e.Property(x => x.Tipo).HasConversion<int>();

            e.Property(x => x.Descripcion)
             .IsRequired()
             .HasMaxLength(200);

            // Precisión monetaria y cantidades
            e.Property(x => x.Cantidad).HasPrecision(10, 2);
            e.Property(x => x.PrecioUnit).HasPrecision(18, 2);

            // ⚠️ Subtotal es calculado en el dominio (get-only); NO se mapea a la DB.
            e.Ignore(x => x.Subtotal);

            // Si se elimina la Orden, los ítems sí deben irse (cascada)
            e.HasOne<Orden>()
             .WithMany()
             .HasForeignKey(x => x.OrdenId)
             .OnDelete(DeleteBehavior.Cascade);

            // Si usás ReferenciaId para Servicio/Repuesto, no hay FK directa (polimorfismo simple)
            // Podés agregar índices para acelerar búsquedas por tipo+referencia:
            e.HasIndex(x => new { x.Tipo, x.ReferenciaId });
        }
    }
}

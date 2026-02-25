using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones
{
    /// <summary>
    /// Configuración para <see cref="Factura"/>: correlativo, total y vínculo con Orden.
    /// </summary>
    public sealed class FacturaConfiguracion : IEntityTypeConfiguration<Factura>
    {
        public void Configure(EntityTypeBuilder<Factura> e)
        {
            e.ToTable("facturas");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nro)
             .IsRequired()
             .HasMaxLength(30);

            e.HasIndex(x => x.Nro).IsUnique();

            e.Property(x => x.Total).HasPrecision(18, 2);
            e.Property(x => x.Fecha).IsRequired();

            // No cascada: no queremos borrar facturas si se borra la orden
            e.HasOne<Orden>()
             .WithMany()
             .HasForeignKey(x => x.OrdenId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

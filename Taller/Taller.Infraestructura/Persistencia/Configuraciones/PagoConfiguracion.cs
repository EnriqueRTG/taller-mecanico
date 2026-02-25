using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones
{
    /// <summary>
    /// Configuración para <see cref="Pago"/>: monto, medio y relación con Factura.
    /// </summary>
    public sealed class PagoConfiguracion : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> e)
        {
            e.ToTable("pagos");
            e.HasKey(x => x.Id);

            e.Property(x => x.Monto).HasPrecision(18, 2);
            e.Property(x => x.Fecha).IsRequired();

            e.Property(x => x.Medio).HasMaxLength(30);
            e.Property(x => x.Ref).HasMaxLength(80);

            // Cascada: si se borra la factura, sus pagos asociados dejan de tener sentido
            e.HasOne<Factura>()
             .WithMany()
             .HasForeignKey(x => x.FacturaId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

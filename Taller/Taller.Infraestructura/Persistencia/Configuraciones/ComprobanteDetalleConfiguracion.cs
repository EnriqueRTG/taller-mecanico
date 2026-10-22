using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>Configura integridad, relaciones e importes del circuito económico.</summary>
public sealed class ComprobanteDetalleConfiguracion : IEntityTypeConfiguration<ComprobanteDetalle>
{
    public void Configure(EntityTypeBuilder<ComprobanteDetalle> b)
    {
        b.ToTable("ComprobanteDetalles", t => t.HasCheckConstraint("CK_ComprobanteDetalles_Importes", "[Cantidad] > 0 AND [PrecioUnitario] >= 0 AND [Subtotal] = ROUND([Cantidad] * [PrecioUnitario], 2)"));
        b.HasKey(x => x.IdComprobanteDetalle);
        b.Property(x => x.Descripcion).HasMaxLength(500).IsRequired();
        b.Property(x => x.Cantidad).HasPrecision(18, 3);
        b.Property(x => x.PrecioUnitario).HasPrecision(18, 2);
        b.Property(x => x.Subtotal).HasPrecision(18, 2);
        b.HasIndex(x => x.IdPresupuestoDetalle).IsUnique();
        b.HasOne(x => x.Comprobante).WithMany(x => x.Detalles).HasForeignKey(x => x.IdComprobante).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Origen).WithMany().HasForeignKey(x => x.IdPresupuestoDetalle).OnDelete(DeleteBehavior.Restrict);
    }
}

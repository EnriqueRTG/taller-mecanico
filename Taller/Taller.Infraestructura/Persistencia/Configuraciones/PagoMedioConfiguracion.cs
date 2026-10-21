using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>Configura integridad, relaciones e importes del circuito económico.</summary>
public sealed class PagoMedioConfiguracion : IEntityTypeConfiguration<PagoMedio>
{
    public void Configure(EntityTypeBuilder<PagoMedio> b)
    {
        b.ToTable("PagoMedios", t => t.HasCheckConstraint("CK_PagoMedios_Importe", "[Importe] > 0 AND [Orden] >= 0"));
        b.Property(x => x.NombreMedio).HasMaxLength(60).IsRequired();
        b.HasKey(x => x.IdPagoMedio); b.HasIndex(x => new { x.IdPago, x.Orden }).IsUnique();
        b.Property(x => x.Importe).HasPrecision(18, 2); b.Property(x => x.Referencia).HasMaxLength(200);
        b.HasOne(x => x.Pago).WithMany(x => x.Medios).HasForeignKey(x => x.IdPago).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.MedioPago).WithMany().HasForeignKey(x => x.IdMedioPago).OnDelete(DeleteBehavior.Restrict);
    }
}

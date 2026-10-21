using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>Configura integridad, relaciones e importes del circuito económico.</summary>
public sealed class ComprobanteConfiguracion : IEntityTypeConfiguration<Comprobante>
{
    public void Configure(EntityTypeBuilder<Comprobante> b)
    {
        b.ToTable("Comprobantes", t =>
        {
            t.HasCheckConstraint("CK_Comprobantes_Importes", "[Total] >= 0 AND [TotalPagado] >= 0 AND [TotalPagado] <= [Total]");
            t.HasCheckConstraint("CK_Comprobantes_SinCargo", "[Total] > 0 OR LEN(LTRIM(RTRIM(COALESCE([Observaciones], N'')))) > 0");
        });
        b.Property(x => x.Moneda).HasMaxLength(3).IsRequired();
        b.HasKey(x => x.IdComprobante);
        b.HasIndex(x => x.IdAtencion).IsUnique();
        b.Property(x => x.Cliente).HasMaxLength(200).IsRequired();
        b.Property(x => x.DocumentoCliente).HasMaxLength(60).IsRequired();
        b.Property(x => x.Dominio).HasMaxLength(20).IsRequired();
        b.Property(x => x.Observaciones).HasMaxLength(1000);
        b.Property(x => x.Total).HasPrecision(18, 2);
        b.Property(x => x.TotalPagado).HasPrecision(18, 2);
        b.Property(x => x.Version).IsRowVersion();
        b.HasOne(x => x.Atencion).WithMany().HasForeignKey(x => x.IdAtencion).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.UsuarioEmision).WithMany().HasForeignKey(x => x.IdUsuarioEmision).OnDelete(DeleteBehavior.Restrict);
    }
}

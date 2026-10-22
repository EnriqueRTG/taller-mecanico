using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>Configura integridad, relaciones e importes del circuito económico.</summary>
public sealed class PagoConfiguracion : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> b)
    {
        b.ToTable("Pagos", t =>
        {
            t.HasCheckConstraint("CK_Pagos_Importe", "[Importe] > 0 AND [SaldoPosterior] >= 0");
            t.HasCheckConstraint("CK_Pagos_Anulacion", "([Anulado] = 0 AND [MotivoAnulacion] IS NULL AND [FechaAnulacion] IS NULL AND [IdUsuarioAnulacion] IS NULL) OR ([Anulado] = 1 AND LEN(LTRIM(RTRIM(COALESCE([MotivoAnulacion], N'')))) > 0 AND [FechaAnulacion] IS NOT NULL AND [IdUsuarioAnulacion] IS NOT NULL)");
        });
        b.Property(x => x.UsuarioRegistroNombre).HasMaxLength(200).IsRequired();
        b.HasKey(x => x.IdPago); b.HasIndex(x => x.IdOperacion).IsUnique();
        b.Property(x => x.Importe).HasPrecision(18, 2); b.Property(x => x.SaldoPosterior).HasPrecision(18, 2); b.Property(x => x.MotivoAnulacion).HasMaxLength(1000);
        b.HasOne(x => x.Comprobante).WithMany(x => x.Pagos).HasForeignKey(x => x.IdComprobante).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.UsuarioRegistro).WithMany().HasForeignKey(x => x.IdUsuarioRegistro).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.UsuarioAnulacion).WithMany().HasForeignKey(x => x.IdUsuarioAnulacion).OnDelete(DeleteBehavior.Restrict);
    }
}

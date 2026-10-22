using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Configuraciones;
public sealed class EvaluacionAtencionConfiguracion : IEntityTypeConfiguration<EvaluacionAtencion>
{
    public void Configure(EntityTypeBuilder<EvaluacionAtencion> b)
    {
        b.ToTable("EvaluacionesAtencion", t => t.HasCheckConstraint("CK_EvaluacionesAtencion_Kilometraje", "[Kilometraje] IS NULL OR [Kilometraje] >= 0"));
        b.HasKey(e => e.IdAtencion);
        b.Property(e => e.IdAtencion).ValueGeneratedNever();
        b.Property(e => e.FallaDetectada).HasMaxLength(1000).IsRequired();
        b.Property(e => e.TrabajoRecomendado).HasMaxLength(1000).IsRequired();
        b.Property(e => e.Observaciones).HasMaxLength(1000);
        b.Property(e => e.Version).IsRowVersion();
        b.HasOne(e => e.Atencion).WithOne(a => a.Evaluacion).HasForeignKey<EvaluacionAtencion>(e => e.IdAtencion).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.TecnicoUltimaEdicion).WithMany().HasForeignKey(e => e.IdTecnicoUltimaEdicion).OnDelete(DeleteBehavior.Restrict);
    }
}

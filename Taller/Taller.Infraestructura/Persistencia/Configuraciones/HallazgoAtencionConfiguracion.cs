using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>Define restricciones e integridad referencial del seguimiento operativo.</summary>
public sealed class HallazgoAtencionConfiguracion : IEntityTypeConfiguration<HallazgoAtencion>
{
    public void Configure(EntityTypeBuilder<HallazgoAtencion> b)
    {
        b.ToTable("HallazgosAtencion", t => t.HasCheckConstraint("CK_Hallazgos_Textos", "LEN(LTRIM(RTRIM([Descripcion]))) > 0 AND LEN(LTRIM(RTRIM([Recomendacion]))) > 0"));
        b.HasKey(x => x.IdHallazgoAtencion);
        b.Property(x => x.Descripcion).HasMaxLength(1000).IsRequired();
        b.Property(x => x.Recomendacion).HasMaxLength(1000).IsRequired();
        b.HasOne(x => x.Atencion).WithMany(x => x.Hallazgos).HasForeignKey(x => x.IdAtencion).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Tecnico).WithMany().HasForeignKey(x => x.IdTecnico).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.DetalleAfectado).WithMany().HasForeignKey(x => x.IdDetalleAfectado).OnDelete(DeleteBehavior.Restrict);
    }
}

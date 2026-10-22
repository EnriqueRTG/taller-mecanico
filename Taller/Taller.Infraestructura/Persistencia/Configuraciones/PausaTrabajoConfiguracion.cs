using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>Define restricciones e integridad referencial del seguimiento operativo.</summary>
public sealed class PausaTrabajoConfiguracion : IEntityTypeConfiguration<PausaTrabajo>
{
    public void Configure(EntityTypeBuilder<PausaTrabajo> b)
    {
        b.ToTable("PausasTrabajo", t =>
        {
            t.HasCheckConstraint("CK_Pausas_Motivo", "LEN(LTRIM(RTRIM([Motivo]))) > 0");
            t.HasCheckConstraint("CK_Pausas_Resolucion", "([FechaResolucion] IS NULL AND [IdTecnicoResolucion] IS NULL AND [Resolucion] IS NULL) OR ([FechaResolucion] >= [Fecha] AND [IdTecnicoResolucion] IS NOT NULL AND LEN(LTRIM(RTRIM([Resolucion]))) > 0)");
        });
        b.HasKey(x => x.IdPausaTrabajo);
        b.Property(x => x.Motivo).HasMaxLength(1000).IsRequired();
        b.Property(x => x.Resolucion).HasMaxLength(1000);
        b.HasOne(x => x.Atencion).WithMany(x => x.Pausas).HasForeignKey(x => x.IdAtencion).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Detalle).WithMany().HasForeignKey(x => x.IdDetalle).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.TecnicoResolucion).WithMany().HasForeignKey(x => x.IdTecnicoResolucion).OnDelete(DeleteBehavior.Restrict);
    }
}

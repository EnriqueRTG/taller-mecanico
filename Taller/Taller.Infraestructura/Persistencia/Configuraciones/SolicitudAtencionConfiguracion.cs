using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>Protege la identidad, orden y contenido de los pedidos de cada visita.</summary>
public sealed class SolicitudAtencionConfiguracion : IEntityTypeConfiguration<SolicitudAtencion>
{
    public void Configure(EntityTypeBuilder<SolicitudAtencion> b)
    {
        b.ToTable("SolicitudesAtencion", t => {
            t.HasCheckConstraint("CK_SolicitudesAtencion_Tipo", "[Tipo] IN (N'TrabajoSolicitado', N'ProblemaInformado')");
            t.HasCheckConstraint("CK_SolicitudesAtencion_Descripcion", "LEN(LTRIM(RTRIM([Descripcion]))) > 0");
            t.HasCheckConstraint("CK_SolicitudesAtencion_Orden", "[Orden] > 0");
        });
        b.HasKey(s => s.IdSolicitudAtencion);
        b.Property(s => s.Tipo).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(s => s.Descripcion).HasMaxLength(500).IsRequired();
        b.HasIndex(s => new { s.IdAtencion, s.Orden }).IsUnique();
        b.HasOne(s => s.Atencion).WithMany(a => a.Solicitudes).HasForeignKey(s => s.IdAtencion).OnDelete(DeleteBehavior.Restrict);
    }
}

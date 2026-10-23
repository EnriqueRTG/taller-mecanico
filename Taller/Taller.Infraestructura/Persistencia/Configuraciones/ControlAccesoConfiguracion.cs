using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;
public sealed class ControlAccesoConfiguracion : IEntityTypeConfiguration<ControlAcceso>
{
    public void Configure(EntityTypeBuilder<ControlAcceso> b)
    {
        b.ToTable("ControlesAcceso");
        b.HasKey(x => x.Clave);
        b.Property(x => x.Clave).HasMaxLength(64).IsUnicode(false);
    }
}

public sealed class EventoAccesoConfiguracion : IEntityTypeConfiguration<EventoAcceso>
{
    public void Configure(EntityTypeBuilder<EventoAcceso> b)
    {
        b.ToTable("EventosAcceso");
        b.HasKey(x => x.Id);
        b.Property(x => x.Clave).HasMaxLength(64).IsUnicode(false);
        b.HasIndex(x => x.FechaUtc);
    }
}

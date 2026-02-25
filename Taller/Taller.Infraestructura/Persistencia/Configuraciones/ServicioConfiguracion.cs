using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones
{
    /// <summary>
    /// Configuración para <see cref="Servicio"/>: catálogo de mano de obra/servicios.
    /// </summary>
    public sealed class ServicioConfiguracion : IEntityTypeConfiguration<Servicio>
    {
        public void Configure(EntityTypeBuilder<Servicio> e)
        {
            e.ToTable("servicios");
            e.HasKey(x => x.Id);

            e.Property(x => x.Codigo)
             .IsRequired()
             .HasMaxLength(40);

            e.HasIndex(x => x.Codigo).IsUnique();

            e.Property(x => x.Descripcion)
             .IsRequired()
             .HasMaxLength(200);

            e.Property(x => x.TarifaHora).HasPrecision(18, 2);
        }
    }
}

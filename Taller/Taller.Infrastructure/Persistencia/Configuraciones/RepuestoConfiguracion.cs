using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones
{
    /// <summary>
    /// Configuración para <see cref="Repuesto"/>: catálogo con stock y precios.
    /// </summary>
    public sealed class RepuestoConfiguracion : IEntityTypeConfiguration<Repuesto>
    {
        public void Configure(EntityTypeBuilder<Repuesto> e)
        {
            e.ToTable("repuestos");
            e.HasKey(x => x.Id);

            e.Property(x => x.Codigo)
             .IsRequired()
             .HasMaxLength(40);

            e.HasIndex(x => x.Codigo).IsUnique();

            e.Property(x => x.Descripcion)
             .IsRequired()
             .HasMaxLength(200);

            e.Property(x => x.Costo).HasPrecision(18, 2);
            e.Property(x => x.Precio).HasPrecision(18, 2);

            // Si en tu entidad Stock/StockMin son int, no seteamos precisión.
            // Si necesitás fracciones, cambia el tipo a decimal y agrega precisión:
            // e.Property(x => x.Stock).HasPrecision(18, 3);
            // e.Property(x => x.StockMin).HasPrecision(18, 3);
        }
    }
}

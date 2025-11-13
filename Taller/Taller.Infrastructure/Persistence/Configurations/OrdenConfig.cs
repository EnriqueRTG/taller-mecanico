using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Domain.Entities;

namespace Taller.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Config de cabecera de <see cref="Orden"/>: correlativo único, estados y FKs.
    /// </summary>
    public sealed class OrdenConfig : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> e)
        {
            e.ToTable("ordenes");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nro)
             .IsRequired()
             .HasMaxLength(30);

            e.HasIndex(x => x.Nro).IsUnique();          // correlativo legible único

            // Enum -> int para performance/compactación
            e.Property(x => x.Estado).HasConversion<int>();

            e.Property(x => x.AperturaEn).IsRequired();

            e.Property(x => x.Diagnostico).HasMaxLength(1000);
            e.Property(x => x.Observaciones).HasMaxLength(1000);

            // FKs (sin cascada para evitar borrar accidentalmente cabeceras)
            e.HasOne<Cliente>()
             .WithMany()
             .HasForeignKey(x => x.ClienteId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<Vehiculo>()
             .WithMany()
             .HasForeignKey(x => x.VehiculoId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

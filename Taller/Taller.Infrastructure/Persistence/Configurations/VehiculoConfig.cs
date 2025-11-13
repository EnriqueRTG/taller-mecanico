using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Domain.Entities;

namespace Taller.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración para <see cref="Vehiculo"/>: unicidad por Patente y FK a Cliente.
    /// </summary>
    public sealed class VehiculoConfig : IEntityTypeConfiguration<Vehiculo>
    {
        public void Configure(EntityTypeBuilder<Vehiculo> e)
        {
            e.ToTable("vehiculos");
            e.HasKey(x => x.Id);

            e.Property(x => x.Patente)
             .IsRequired()
             .HasMaxLength(15);

            e.HasIndex(x => x.Patente).IsUnique();      // no permitir patentes duplicadas

            e.Property(x => x.Marca).HasMaxLength(60);
            e.Property(x => x.Modelo).HasMaxLength(80);

            // VIN opcional; si lo usás, solemos limitar longitud
            e.Property(x => x.Vin).HasMaxLength(30);

            // Relación N:1 con Cliente (sin borrado en cascada)
            e.HasOne<Cliente>()
             .WithMany()
             .HasForeignKey(x => x.ClienteId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

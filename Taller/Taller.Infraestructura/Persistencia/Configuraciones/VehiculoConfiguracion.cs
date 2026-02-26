using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones
{
    /// <summary>
    /// Configuración para <see cref="Vehiculo"/>: unicidad por Patente y FK a Cliente.
    /// </summary>
    public sealed class VehiculoConfiguracion : IEntityTypeConfiguration<Vehiculo>
    {
        public void Configure(EntityTypeBuilder<Vehiculo> e)
        {
            e.ToTable("vehiculos");

            e.HasKey(v => v.Id);

            e.Property(v => v.Patente)
             .IsRequired()
             .HasMaxLength(10);

            e.Property(v => v.Marca)
                .IsRequired()
                .HasMaxLength(60);

            e.Property(v => v.Modelo)
                .HasMaxLength(60);

            e.Property(v => v.Color)
                .HasMaxLength(40);

            e.Property(v => v.Vin)
                .HasMaxLength(30);

            e.Property(v => v.Observaciones)
                .HasMaxLength(500);

            e.Property(v => v.Activo)
                .HasDefaultValue(true);

            // Indice de patente único para evitar duplicados
            e.HasIndex(v => v.Patente)
                .IsUnique();

            // Relación N:1 con Cliente (sin borrado en cascada)
            e.HasOne(v => v.Cliente)
             .WithMany(c => c.Vehiculos)
             .HasForeignKey(v => v.ClienteId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

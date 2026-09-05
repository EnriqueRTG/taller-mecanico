using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura las propiedades de la entidad Vehiculo en la base de datos.
/// </summary>
public sealed class VehiculoConfiguracion : IEntityTypeConfiguration<Vehiculo>
{
    /// <summary>
    /// Configura la entidad Vehiculo en el modelo de datos.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Vehiculo> builder)
    {
        // Nombre de la tabla
        builder.ToTable("Vehiculos");

        // Clave primaria
        builder.HasKey(v => v.IdVehiculo);

        // Configuración de la propiedad IdVehiculo como autoincremental
        builder.Property(v => v.IdVehiculo)
            .ValueGeneratedOnAdd();

        // Dominio o patente del vehículo
        builder.Property(v => v.Dominio)
            .IsRequired()
            .HasMaxLength(10);

        // El dominio debe ser único en la base de datos
        builder.HasIndex(v => v.Dominio)
            .IsUnique();

        // Año del vehículo
        builder.Property(v => v.Anio)
            .IsRequired();

        // Color del vehículo
        builder.Property(v => v.Color)
            .IsRequired()
            .HasMaxLength(50);

        // Baja lógica del vehículo (activo o inactivo)
        builder.Property(v => v.Activo)
            .IsRequired();

        // Fecha de alta del vehículo
        builder.Property(v => v.FechaAlta)
            .IsRequired();

        // Modelo asociado
        builder.Property(v => v.IdModelo)
            .IsRequired();

        // Relación Modelo 1:N Vehiculos
        builder.HasOne(v => v.Modelo)
            .WithMany(m => m.Vehiculos)
            .HasForeignKey(v => v.IdModelo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

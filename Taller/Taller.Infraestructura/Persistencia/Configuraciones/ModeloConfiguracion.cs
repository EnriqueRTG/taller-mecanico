using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura las propiedades de la entidad Modelo en la base de datos.
/// </summary>
public sealed class ModeloConfiguracion : IEntityTypeConfiguration<Modelo>
{
    /// <summary>
    /// Configura las propiedades de la entidad Modelo en la base de datos.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Modelo> builder)
    {
        // Nombre de la tabla en la base de datos
        builder.ToTable("Modelos");

        // Clave primaria
        builder.HasKey(m => m.IdModelo);

        // Configuración de la propiedad IdModelo autoincrementable
        builder.Property(m => m.IdModelo)
            .ValueGeneratedOnAdd();

        // Nombre del modelo
        builder.Property(m => m.Nombre)
            .IsRequired()
            .HasMaxLength(50);

        // Baja logica del modelo (Activo/No Activo)
        builder.Property(m => m.Activo)
            .IsRequired();

        // Marca asociada al modelo
        builder.Property(m => m.IdMarca)
            .IsRequired();

        // Un modelo no puede repetirse para la misma marca
        // Se crea un índice único compuesto por IdMarca y Nombre
        builder.HasIndex(m => new 
            { 
                m.IdMarca, 
                m.Nombre 
            })
            .IsUnique();

        // Realcion Marca 1:N Modelos
        builder.HasOne(m => m.Marca)
            .WithMany(m => m.Modelos)
            .HasForeignKey(m => m.IdMarca)
            .OnDelete(DeleteBehavior.Restrict);


    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura las propiedades de la entidad Marca en la base de datos.
/// </summary>
public sealed class MarcaConfiguracion : IEntityTypeConfiguration<Marca>
{
    /// <summary>
    /// Configura las propiedades de la entidad Marca en la base de datos.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Marca> builder)
    {
        // Nombre de la tabla en la base de datos
        builder.ToTable("Marcas");

        // Clave primaria
        builder.HasKey(m => m.IdMarca);

        // Configuracion de la propiedad IdMarca autoincrementable
        builder.Property(m => m.IdMarca)
            .ValueGeneratedOnAdd();

        // Nombre de la marca
        builder.Property(m => m.Nombre)
            .IsRequired()
            .HasMaxLength(50);

        // Baja lógica
        builder.Property(m => m.Activo)
            .IsRequired();

        // El nombre de la marca debe ser único
        builder.HasIndex(m => m.Nombre)
            .IsUnique();
    }
}
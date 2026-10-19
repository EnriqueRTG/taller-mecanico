using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura las categorías utilizadas para agrupar conceptos.
/// </summary>
public sealed class CategoriaConfiguracion : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias", tabla =>
        {
            // Rechaza nombres vacíos o compuestos solo por espacios.
            tabla.HasCheckConstraint("CK_Categorias_Nombre", 
                "LEN(LTRIM(RTRIM(Nombre))) > 0");
        });

        builder.HasKey(c => c.IdCategoria);

        builder.Property(c => c.IdCategoria)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(c => c.Descripcion)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(c => c.Activo)
            .IsRequired()
            .HasDefaultValue(true);

        // La unicidad incluye categorías inactivas:
        // se reactiva la existente en lugar de crear otra igual.
        builder.HasIndex(c => c.Nombre)
            .HasDatabaseName("UX_Categorias_Nombre")
            .IsUnique();
    }
}

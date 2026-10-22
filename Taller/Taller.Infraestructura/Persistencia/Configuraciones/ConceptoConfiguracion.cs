using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura el catálogo de servicios, repuestos y consumibles.
/// </summary>
public sealed class ConceptoConfiguracion : IEntityTypeConfiguration<Concepto>
{
    public void Configure(EntityTypeBuilder<Concepto> builder)
    {
        builder.ToTable("Conceptos", tabla =>
        {
            // Rechaza nombres vacíos o compuestos solo por espacios.
            tabla.HasCheckConstraint("CK_Conceptos_Nombre", 
                "LEN(LTRIM(RTRIM(Nombre))) > 0");

            tabla.HasCheckConstraint("CK_Conceptos_Tipo",
                """
                [Tipo] IN (
                    N'Servicio', 
                    N'Repuesto', 
                    N'Consumible')
                """); // Asegura que el tipo sea válido 

            tabla.HasCheckConstraint("CK_Conceptos_PrecioReferencia",
                """
                [PrecioReferencia] IS NULL OR 
                [PrecioReferencia] >= 0
                """); // Asegura que el precio de referencia sea nulo o no negativo

            // Los repuestos y consumibles no se ofrecen como servicios de ingreso.
            tabla.HasCheckConstraint(
                "CK_Conceptos_DisponibleParaIngreso",
                """
                [DisponibleParaIngreso] = 0
                OR [Tipo] = N'Servicio'
                """);
        });

        builder.HasKey(c => c.IdConcepto);

        builder.Property(c => c.IdConcepto)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.IdCategoria)
            .IsRequired();

        builder.Property(c => c.Tipo)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Descripcion)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(c => c.PrecioReferencia)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(c => c.Activo)
            .IsRequired();

        builder.Property(c => c.DisponibleParaIngreso)
            .IsRequired()
            .HasDefaultValue(false);

        // Facilita listar conceptos de una categoría específica.
        builder.HasIndex(c => c.IdCategoria)
            .HasDatabaseName("IX_Conceptos_IdCategoria");

        // Evita repetir un concepto del mismo tipo dentro de una categoría.
        // Incluye los conceptos inactivos para conservar su identidad.
        builder.HasIndex(c => new
        {
            c.IdCategoria,
            c.Tipo,
            c.Nombre
        })
            .HasDatabaseName("UX_Conceptos_Categoria_Tipo_Nombre")
            .IsUnique();

        builder.HasOne(c => c.Categoria)
            .WithMany(cat => cat.Conceptos)
            .HasForeignKey(c => c.IdCategoria)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

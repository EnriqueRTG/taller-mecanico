using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura los informes técnicos iniciales y adicionales.
/// </summary>
public sealed class DiagnosticoConfiguracion : IEntityTypeConfiguration<Diagnostico>
{
    public void Configure(EntityTypeBuilder<Diagnostico> builder)
    {
        builder.ToTable("Diagnosticos", tabla =>
        {
            tabla.HasCheckConstraint(
                "CK_Diagnosticos_Tipo",
                "Tipo IN (N'Inicial', N'Adicional')");

            tabla.HasCheckConstraint(
                "CK_Diagnosticos_Kilometraje",
                "[Kilometraje] IS NULL OR [Kilometraje] >= 0");

            // El bloqueo de trabajos anteriores solo corresponde a
            // un hallazgo adicional.
            tabla.HasCheckConstraint(
                "CK_Diagnosticos_ImpideContinuar",
                "[Tipo] <> N'Inicial' OR [ImpideContinuar] = 0");

            tabla.HasCheckConstraint(
                "CK_Diagnosticos_FallaDetectada",
                "LEN(LTRIM(RTRIM([FallaDetectada]))) > 0");

            tabla.HasCheckConstraint(
                "CK_Diagnosticos_TrabajoRecomendado",
                "LEN(LTRIM(RTRIM([TrabajoRecomendado]))) > 0");
        });

        builder.HasKey(d => d.IdDiagnostico);

        builder.Property(d => d.IdDiagnostico)
            .ValueGeneratedOnAdd();

        builder.Property(d => d.IdAtencion)
            .IsRequired();

        builder.Property(d => d.IdTecnicoAutor)
            .IsRequired();

        builder.Property(d => d.FechaDiagnostico)
            .IsRequired()
            .HasColumnType("datetime2(0)");

        builder.Property(d => d.FallaDetectada)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.TrabajoRecomendado)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.Kilometraje)
            .IsRequired(false);

        builder.Property(d => d.Observaciones)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(d => d.Tipo)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(d => d.ImpideContinuar)
            .IsRequired()
            .HasDefaultValue(false);

        // Solo el informe inicial debe ser único por atención.
        // Los adicionales no quedan limitados por este índice.
        builder.HasIndex(d => d.IdAtencion)
            .HasDatabaseName("UX_Diagnosticos_InicialPorAtencion")
            .IsUnique()
            .HasFilter("[Tipo] = N'Inicial'");

        builder.HasOne(d => d.Atencion)
            .WithMany(a => a.Diagnosticos)
            .HasForeignKey(d => d.IdAtencion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.TecnicoAutor)
            .WithMany()
            .HasForeignKey(d => d.IdTecnicoAutor)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

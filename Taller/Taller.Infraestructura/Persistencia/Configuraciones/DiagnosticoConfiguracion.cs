using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura los informes técnicos de la evaluación técnica.
/// </summary>
public sealed class DiagnosticoConfiguracion : IEntityTypeConfiguration<Diagnostico>
{
    public void Configure(EntityTypeBuilder<Diagnostico> builder)
    {
        builder.ToTable("Diagnosticos", tabla =>
        {

            tabla.HasCheckConstraint(
                "CK_Diagnosticos_Kilometraje",
                "[Kilometraje] IS NULL OR [Kilometraje] >= 0");


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



        // Un informe definitivo de evaluación por atención; los hallazgos tienen su propia entidad.
        builder.HasIndex(d => d.IdAtencion)
            .HasDatabaseName("UX_Diagnosticos_Atencion")
            .IsUnique();

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

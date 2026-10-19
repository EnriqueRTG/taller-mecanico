using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

public sealed class HistorialAtencionConfiguracion : IEntityTypeConfiguration<HistorialAtencion>
{
    public void Configure(EntityTypeBuilder<HistorialAtencion> builder)
    {
        builder.ToTable("HistorialAtenciones", tabla =>
        {
            tabla.HasCheckConstraint("CK_HistorialAtenciones_Accion", "LEN(LTRIM(RTRIM([Accion]))) > 0");

            tabla.HasCheckConstraint(
                "CK_HistorialAtenciones_EstadoAnterior",
                """
                [EstadoAnterior] IS NULL
                OR [EstadoAnterior] IN (
                    N'Abierta',
                    N'PendienteDiagnostico',
                    N'EnEvaluacionTecnica',
                    N'PendienteDecision',
                    N'EnEjecucion',
                    N'Pausada',
                    N'TrabajoFinalizado',
                    N'PendienteLiquidacion',
                    N'Cerrada',
                    N'Cancelada'
                )
                """);

            tabla.HasCheckConstraint(
                "CK_HistorialAtenciones_EstadoNuevo",
                """
                [EstadoNuevo] IN (
                    N'Abierta',
                    N'PendienteDiagnostico',
                    N'EnEvaluacionTecnica',
                    N'PendienteDecision',
                    N'EnEjecucion',
                    N'Pausada',
                    N'TrabajoFinalizado',
                    N'PendienteLiquidacion',
                    N'Cerrada',
                    N'Cancelada'
                )
                """);

            builder.HasKey(h => h.IdHistorialAtencion);

            builder.Property(h => h.IdHistorialAtencion)
                .ValueGeneratedOnAdd();

            builder.Property(h => h.IdAtencion)
                .IsRequired();

            builder.Property(h => h.IdUsuario)
                .IsRequired();

            builder.Property(h => h.IdDiagnostico)
                .IsRequired(false);

            builder.Property(h => h.Fecha)
                .IsRequired()
                .HasColumnType("datetime2(0)");

            builder.Property(h => h.Accion)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(h => h.EstadoAnterior)
                .HasConversion<string>()
                .IsRequired(false)
                .HasMaxLength(30);

            builder.Property(h => h.EstadoNuevo)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(h => h.Observaciones)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.HasIndex(h => new
            {
                h.IdAtencion,
                h.Fecha,
                h.IdHistorialAtencion
            })
            .HasDatabaseName("IX_HistorialAtenciones_Atencion_Fecha_Id");

            builder.HasOne(h => h.Atencion)
                .WithMany(a => a.Historial)
                .HasForeignKey(h => h.IdAtencion)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Diagnostico)
                .WithMany()
                .HasForeignKey(h => h.IdDiagnostico)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });
    }
}

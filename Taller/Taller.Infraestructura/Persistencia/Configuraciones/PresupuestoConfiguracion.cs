using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>
/// Configura los presupuestos, sus relaciones y restricciones.
/// </summary>
public sealed class PresupuestoConfiguracion : IEntityTypeConfiguration<Presupuesto>
{
    public void Configure(EntityTypeBuilder<Presupuesto> builder)
    {
        builder.ToTable("Presupuestos", tabla =>
        {
            tabla.HasCheckConstraint("CK_Presupuestos_Adicional", "([EsInspeccion] = 0 OR [EsAdicional] = 1) AND ([EsAdicional] = 0 OR [Tipo] = N'Reparacion') AND ([IdHallazgoAtencion] IS NULL OR [EsAdicional] = 1)");
            tabla.HasCheckConstraint("CK_Presupuestos_Origen", "[Tipo] IN (N'Diagnostico', N'Reparacion') AND ([Tipo] <> N'Diagnostico' OR [IdDiagnostico] IS NULL)");
            tabla.HasCheckConstraint("CK_Presupuestos_Estado", """
                [Estado] IN (
                    N'Borrador',
                    N'PendientePresentacion',
                    N'PendienteDecision',
                    N'Aceptado',
                    N'Rechazado', N'PendienteRevisionTecnica'
                )
                """);
            // Una decisión requiere usuario y fecha.
            // Mientras está pendiente, ninguno debe estar cargado.
            tabla.HasCheckConstraint("CK_Presupuestos_Decision", """
                (
                    [Estado] IN (
                        N'Borrador',
                    N'PendientePresentacion',
                        N'PendienteDecision'
                    )
                    AND [IdUsuarioDecision] IS NULL
                    AND [FechaDecision] IS NULL
                )
                OR
                (
                    [Estado] IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica')
                    AND [IdUsuarioDecision] IS NOT NULL
                    AND [FechaDecision] IS NOT NULL
                )
                """);
            tabla.HasCheckConstraint("CK_Presupuestos_Presentacion", """
                (
                    [Estado] IN (N'Borrador', N'PendientePresentacion')
                    AND [FechaPresentacion] IS NULL
                )
                OR
                (
                    [Estado] IN (
                        N'PendienteDecision',
                        N'Aceptado',
                        N'Rechazado', N'PendienteRevisionTecnica'
                    )
                    AND [FechaPresentacion] IS NOT NULL
                )
                """);
            tabla.HasCheckConstraint("CK_Presupuestos_Fechas", """
                (
                    [FechaPresentacion] IS NULL
                    OR [FechaPresentacion] >= [FechaCreacion]
                )
                AND
                (
                    [FechaDecision] IS NULL
                    OR (
                        [FechaPresentacion] IS NOT NULL
                        AND [FechaDecision] >= [FechaPresentacion]
                    )
                )
                """);
            // El servicio inicial se registra con autorización previa.
            tabla.HasCheckConstraint("CK_Presupuestos_DiagnosticoAceptado", "[Tipo] <> N'Diagnostico' OR [Estado] = N'Aceptado'");
        });
        builder.HasOne(p => p.Hallazgo).WithMany().HasForeignKey(p => p.IdHallazgoAtencion).OnDelete(DeleteBehavior.Restrict);
        builder.HasKey(p => p.IdPresupuesto);
        builder.Property(p => p.Vigente).IsRequired();
        builder.Property(p => p.MotivoRevision).HasMaxLength(1000);
        builder.Property(p => p.ObservacionesDecision).HasMaxLength(1000);
        builder.HasOne(p => p.PresupuestoAnterior).WithMany().HasForeignKey(p => p.IdPresupuestoAnterior).OnDelete(DeleteBehavior.Restrict);
        builder.Property(p => p.IdPresupuesto).ValueGeneratedOnAdd();
        builder.Property(p => p.Tipo).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(p => p.IdAtencion).IsRequired();
        builder.Property(p => p.IdDiagnostico).IsRequired(false);
        builder.Property(p => p.IdUsuarioCreacion).IsRequired();
        builder.Property(p => p.FechaCreacion).HasColumnType("datetime2(0)").IsRequired();
        builder.Property(p => p.Estado).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(p => p.FechaPresentacion).HasColumnType("datetime2(0)").IsRequired(false);
        builder.Property(p => p.IdUsuarioDecision).IsRequired(false);
        builder.Property(p => p.FechaDecision).HasColumnType("datetime2(0)").IsRequired(false);
        builder.Property(p => p.Observaciones).HasMaxLength(1000).IsRequired(false);
        builder.Property(p => p.Version).IsRowVersion();
        // Solo un presupuesto del cargo inicial por atención.
        builder.HasIndex(p => p.IdAtencion, "UX_Presupuestos_DiagnosticoPorAtencion").HasDatabaseName("UX_Presupuestos_DiagnosticoPorAtencion").IsUnique().HasFilter("[Tipo] = N'Diagnostico'");
        builder.HasIndex(p => p.IdAtencion).HasDatabaseName("IX_Presupuestos_Atencion").IsUnique(false);
        builder.HasIndex(p => p.IdAtencion, "UX_Presupuestos_BorradorPorAtencion").IsUnique().HasFilter("[Tipo] = N'Reparacion' AND [Estado] = N'Borrador' AND [Vigente] = 1 AND [EsAdicional] = 0");
        // Un diagnóstico puede fundamentar varias propuestas.
        builder.HasIndex(p => p.IdDiagnostico).HasDatabaseName("IX_Presupuestos_IdDiagnostico");
        builder.HasOne(p => p.Atencion).WithMany().HasForeignKey(p => p.IdAtencion).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.Diagnostico).WithMany().HasForeignKey(p => new { p.IdDiagnostico, p.IdAtencion }).HasPrincipalKey(d => new { d.IdDiagnostico, d.IdAtencion }).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.UsuarioCreacion).WithMany().HasForeignKey(p => p.IdUsuarioCreacion).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.UsuarioDecision).WithMany().HasForeignKey(p => p.IdUsuarioDecision).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
    }
}

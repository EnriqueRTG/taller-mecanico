using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>
/// Configura los ítems presupuestados y su seguimiento de ejecución.
/// </summary>
public sealed class PresupuestoDetalleConfiguracion : IEntityTypeConfiguration<PresupuestoDetalle>
{
    public void Configure(EntityTypeBuilder<PresupuestoDetalle> builder)
    {
        builder.ToTable("PresupuestoDetalles", tabla =>
        {
            tabla.HasCheckConstraint("CK_Detalles_MaterialCliente", "[OrigenMaterial] IN (N'Taller', N'Cliente') AND [CantidadRecibidaCliente] >= 0 AND [CantidadRecibidaCliente] <= [Cantidad] AND ([OrigenMaterial] <> N'Cliente' OR [PrecioUnitario] = 0) AND ([MaterialClienteConforme] = 0 OR ([OrigenMaterial] = N'Cliente' AND [CantidadRecibidaCliente] > 0 AND [IdTecnicoMaterial] IS NOT NULL AND [FechaRecepcionMaterial] IS NOT NULL))");
            tabla.HasCheckConstraint("CK_PresupuestoDetalles_Descripcion", "LEN(LTRIM(RTRIM([Descripcion]))) > 0");
            tabla.HasCheckConstraint("CK_PresupuestoDetalles_Cantidad", "[Cantidad] > 0");
            tabla.HasCheckConstraint("CK_PresupuestoDetalles_PrecioUnitario", "[PrecioUnitario] >= 0");
            tabla.HasCheckConstraint("CK_PresupuestoDetalles_CantidadEjecutada", """
                [CantidadEjecutada] >= 0
                AND [CantidadEjecutada] <= [Cantidad]
                """);
            tabla.HasCheckConstraint("CK_PresupuestoDetalles_EstadoEjecucion", """
                [EstadoEjecucion] IN (
                    N'NoIniciado',
                    N'EnCurso',
                    N'Completado'
                )
                """);
            // Mantiene coherencia entre el avance y las cantidades.
            tabla.HasCheckConstraint("CK_PresupuestoDetalles_Avance", """
                (
                    [EstadoEjecucion] = N'NoIniciado'
                    AND [CantidadEjecutada] = 0
                )
                OR
                (
                    [EstadoEjecucion] = N'EnCurso'
                    AND [CantidadEjecutada] < [Cantidad]
                )
                OR
                (
                    [EstadoEjecucion] = N'Completado'
                    AND [CantidadEjecutada] = [Cantidad]
                )
                """);
            tabla.HasCheckConstraint("CK_PresupuestoDetalles_RegistroTecnico", """
                (
                    [IdTecnicoRegistro] IS NULL
                    AND [FechaRegistroEjecucion] IS NULL
                )
                OR
                (
                    [IdTecnicoRegistro] IS NOT NULL
                    AND [FechaRegistroEjecucion] IS NOT NULL
                )
                """);
        });
        builder.Property(d => d.OrigenMaterial).HasConversion<string>().HasMaxLength(20).HasDefaultValue(Taller.Dominio.Enumeraciones.OrigenMaterial.Taller);
        builder.Property(d => d.CantidadRecibidaCliente).HasPrecision(18, 3);
        builder.Property(d => d.ObservacionesMaterial).HasMaxLength(1000);
        builder.HasOne(d => d.TecnicoMaterial).WithMany().HasForeignKey(d => d.IdTecnicoMaterial).OnDelete(DeleteBehavior.Restrict);
        builder.Property(d => d.AceptadoPorCliente).IsRequired(false);
        builder.HasOne<SolicitudAtencion>().WithMany().HasForeignKey(d => d.IdSolicitudAtencion).OnDelete(DeleteBehavior.Restrict);
        builder.HasKey(d => d.IdPresupuestoDetalle);
        builder.Property(d => d.IdPresupuestoDetalle).ValueGeneratedOnAdd();
        builder.Property(d => d.IdPresupuesto).IsRequired();
        builder.Property(d => d.IdConcepto).IsRequired();
        builder.Property(d => d.Descripcion).HasMaxLength(500).IsRequired();
        builder.Property(d => d.Cantidad).HasPrecision(18, 3).IsRequired();
        builder.Property(d => d.PrecioUnitario).HasPrecision(18, 2).IsRequired();
        builder.Property(d => d.EstadoEjecucion).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(d => d.CantidadEjecutada).HasPrecision(18, 3).IsRequired();
        builder.Property(d => d.IdTecnicoRegistro).IsRequired(false);
        builder.Property(d => d.FechaRegistroEjecucion).HasColumnType("datetime2(0)").IsRequired(false);
        builder.Property(d => d.ObservacionesEjecucion).HasMaxLength(1000).IsRequired(false);
        // Presupuesto al que pertenece el ítem.
        builder.HasOne(d => d.Presupuesto).WithMany(p => p.Detalles).HasForeignKey(d => d.IdPresupuesto).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.Concepto).WithMany().HasForeignKey(d => d.IdConcepto).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.TecnicoRegistro).WithMany().HasForeignKey(d => d.IdTecnicoRegistro).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura la persistencia, las relaciones y las restricciones
/// de integridad de las atenciones.
/// </summary>
public sealed class AtencionConfiguracion : IEntityTypeConfiguration<Atencion>
{
    public void Configure(
        EntityTypeBuilder<Atencion> builder)
    {
        builder.ToTable("Atenciones", tabla =>
        {
            // Impide guardar estados ajenos al ciclo definido.
            tabla.HasCheckConstraint(
                "CK_Atenciones_Estado",
                """
                [Estado] IN (
                    N'Abierta',
                    N'PendienteDiagnostico',
                    N'EnEvaluacionTecnica',
                    N'PendienteDecision',
                    N'EnEjecucion',
                    N'TrabajoFinalizado',
                    N'Cerrada',
                    N'Cancelada'
                )
                """);
        });

        builder.HasKey(a => a.IdAtencion);

        builder.Property(a => a.IdAtencion)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.IdCliente)
            .IsRequired();

        builder.Property(a => a.IdVehiculo)
            .IsRequired();

        builder.Property(a => a.IdUsuarioRecepcion)
            .IsRequired();

        builder.Property(a => a.FechaApertura) 
            .IsRequired();

        builder.Property(a => a.MotivoConsulta) 
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.FechaCierre)
            .IsRequired(false);

        // SQL Server genera una nueva versión en cada actualización.
        // EF la utiliza para detectar modificaciones concurrentes.
        builder.Property(a => a.Version)
            .IsRowVersion();

        // Permite varias atenciones históricas por vehículo,
        // pero únicamente una que no esté cerrada ni cancelada.
        builder.HasIndex(a => a.IdVehiculo)
            .HasDatabaseName("UX_Atenciones_VehiculoActivo")
            .IsUnique()
            .HasFilter(
                "[Estado] <> N'Cerrada' AND [Estado] <> N'Cancelada'");

        // Cliente 1:N Atenciones
        builder.HasOne(a => a.Cliente)
            .WithMany(c => c.Atenciones)
            .HasForeignKey(a => a.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        // Vehiculo 1:N Atenciones
        builder.HasOne(a => a.Vehiculo)
            .WithMany(v => v.Atenciones)
            .HasForeignKey(a => a.IdVehiculo)
            .OnDelete(DeleteBehavior.Restrict);

        // Usuario 1:N Atenciones
        builder.HasOne(a => a.UsuarioRecepcion)
            .WithMany(u => u.AtencionesRecepcionadas)
            .HasForeignKey(a => a.IdUsuarioRecepcion)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

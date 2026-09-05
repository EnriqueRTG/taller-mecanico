using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura las propiedades y relaciones de la entidad Atención
/// en la base de datos.
/// </summary>
public sealed class AtencionConfiguracion : IEntityTypeConfiguration<Atencion>
{
    public void Configure(EntityTypeBuilder<Atencion> builder)
    {
        builder.ToTable("Atenciones");

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

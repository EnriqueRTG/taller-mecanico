using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Configura las propiedades de la entidad Cliente en la base de datos.
/// </summary>
public sealed class ClienteConfiguracion : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        // Nombre de la tabla en la base de datos
        builder.ToTable("Clientes");

        // Clave primaria
        builder.HasKey(c => c.IdCliente);

        // Configuración de la propiedad IdCliente autoincremental
        builder.Property(c => c.IdCliente)
            .ValueGeneratedOnAdd();

        // Numero de documento del cliente
        builder.Property(c => c.Documento)
            .IsRequired()
            .HasMaxLength(20);

        // Tipo de documento del cliente
        builder.Property(c => c.TipoDocumento)
            .IsRequired()
            .HasMaxLength(20);

        // Nombre del cliente
        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(80);

        // Apellido del cliente
        builder.Property(c => c.Apellido)
            .IsRequired()
            .HasMaxLength(80);

        // Teléfono del cliente
        builder.Property(c => c.Telefono)
            .HasMaxLength(30);

        // Email del cliente
        builder.Property(c => c.Email)
            .HasMaxLength(120);

        // Dirección del cliente
        builder.Property(c => c.Direccion)
            .HasMaxLength(200);

        // Baja lógica del cliente
        builder.Property(c => c.Activo)
            .IsRequired();

        // Fecha de alta del cliente
        builder.Property(c => c.FechaAlta)
            .IsRequired();

        // No permitir duplicados en la combinación de TipoDocumento y Documento
        builder.HasIndex(c => new
        {
            c.TipoDocumento,
            c.Documento
        })
        .IsUnique();
    }
}
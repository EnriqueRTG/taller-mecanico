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

        // Configuración de la propiedad IdCliente para que se genere automáticamente al agregar un nuevo cliente
        builder.Property(c => c.IdCliente)
            .ValueGeneratedOnAdd();

        // Configuración de la propiedad Documento
        builder.Property(c => c.Documento)
            .IsRequired()
            .HasMaxLength(20);
        
        // Configuración de la propiedad TipoDocumento
        builder.Property(c => c.TipoDocumento)
            .IsRequired()
            .HasMaxLength(20);

        // Configuración de la propiedad Nombre
        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(80);

        // Configuración de la propiedad Apellido
        builder.Property(c => c.Apellido)
            .IsRequired()
            .HasMaxLength(80);

        // Configuración de la propiedad Telefono
        builder.Property(c => c.Telefono)
            .HasMaxLength(30);

        // Configuración de la propiedad Email
        builder.Property(c => c.Email)
            .HasMaxLength(120);

        // Configuración de la propiedad Direccion
        builder.Property(c => c.Direccion)
            .HasMaxLength(200);

        // Configuración de la propiedad Activo
        builder.Property(c => c.Activo)
            .IsRequired();

        // Configuración de la propiedad FechaAlta
        builder.Property(c => c.FechaAlta)
            .IsRequired();

        // Configuración de la restricción de unicidad para Documento y TipoDocumento
        builder.HasIndex(c => new
        {
            c.TipoDocumento,
            c.Documento
        })
        .IsUnique();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones
{
    /// <summary>
    /// Configuración de la entidad Cliente para EF Core.
    /// </summary>
    public sealed class ClienteConfiguracion : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> e)
        {
            e.ToTable("clientes");

            e.HasKey(x => x.Id);

            // ===== Campos obligatorios =====
            e.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(60);

            e.Property(x => x.Apellido)
                .IsRequired()
                .HasMaxLength(60);

            e.Property(x => x.DocTipo)
                .IsRequired()
                .HasMaxLength(10);

            e.Property(x => x.DocNro)
                .IsRequired()
                .HasMaxLength(30);

            e.Property(x => x.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // ===== Campos opcionales =====
            // (Correo, teléfono y dirección pueden ser null)
            e.Property(x => x.Email)
                .HasMaxLength(120);

            e.Property(x => x.Telefono)
                .HasMaxLength(40);

            e.Property(x => x.Direccion)
                .HasMaxLength(200);

            // ===== Índices útiles =====
            e.HasIndex(x => new { x.Apellido, x.Nombre });

            e.HasIndex(x => x.Email);

            // No forzamos unicidad por si hay errores de carga
            e.HasIndex(x => new { x.DocTipo, x.DocNro })
                .IsUnique(false);

            // Sólo clientes activos por defecto en las consultas
            e.HasQueryFilter(c => c.Activo);
        }
    }
}
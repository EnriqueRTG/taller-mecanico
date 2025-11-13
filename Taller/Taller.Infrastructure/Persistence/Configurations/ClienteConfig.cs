using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Domain.Entities;

namespace Taller.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de mapeo para la entidad <see cref="Cliente"/>.
    /// Mantener aquí TODAS las restricciones que deben cumplirse en la base de datos.
    /// </summary>
    public sealed class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> e)
        {
            e.ToTable("clientes");                 // nombre de tabla
            e.HasKey(x => x.Id);                   // PK

            e.Property(x => x.Nombre)
             .IsRequired()
             .HasMaxLength(120);

            e.Property(x => x.Email)
             .HasMaxLength(120);

            e.Property(x => x.Telefono)
             .HasMaxLength(40);

            e.Property(x => x.DocTipo)
             .HasMaxLength(10);

            e.Property(x => x.DocNro)
             .HasMaxLength(30);

            e.Property(x => x.Direccion)
             .HasMaxLength(200);

            // Índices útiles para búsqueda/validación
            e.HasIndex(x => x.Nombre);
            e.HasIndex(x => x.Email);
            // Documento (tipo+nro) suele ser único en la práctica
            e.HasIndex(x => new { x.DocTipo, x.DocNro }).IsUnique(false);
        }
    }
}

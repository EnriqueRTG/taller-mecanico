using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones
{
    /// <summary>   
    /// 
    public sealed class ClienteConfiguracion : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> e)
        {
            e.ToTable("clientes");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
             .IsRequired()
             .HasMaxLength(60);

            e.Property(x => x.Apellido)
             .IsRequired()
             .HasMaxLength(60);

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

            // Índices útiles
            e.HasIndex(x => new { x.Apellido, x.Nombre });
            e.HasIndex(x => x.Email);
            e.HasIndex(x => new { x.DocTipo, x.DocNro }).IsUnique(false);

            e.Property(x => x.Activo).HasDefaultValue(true);

            // Sólo clientes activos por defecto
            e.HasQueryFilter(c => c.Activo);
        }
    }


}


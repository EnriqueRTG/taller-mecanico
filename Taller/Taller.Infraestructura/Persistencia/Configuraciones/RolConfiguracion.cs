using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

public sealed class RolConfiguracion : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Nombre)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(r => r.Descripcion)
            .HasMaxLength(150);
        builder.Property(r=> r.Activo)
            .IsRequired();
        builder.HasIndex(r => r.Nombre)
            .IsUnique();

        // Datos iniciales necesarios para el funcionamiento del sistema.
        builder.HasData(
            new Rol
            {
                Id = 1,
                Nombre = "Administrador",
                Descripcion = "Administración general del sistema",
                Activo = true
            },
            new Rol
            {
                Id = 2,
                Nombre = "Administrativo",
                Descripcion = "Gestión administrativa y atención al cliente",
                Activo = true
            },
            new Rol
            {
                Id = 3,
                Nombre = "Técnico",
                Descripcion = "Gestión técnica de los trabajos del taller",
                Activo = true
            }
        );
    }
}
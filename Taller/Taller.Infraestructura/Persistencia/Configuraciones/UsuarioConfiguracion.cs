using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;


namespace Taller.Infraestructura.Persistencia.Configuraciones;

public sealed class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // Nombre de la tabla en la base de datos
        builder.ToTable("Usuarios");

        // Clave primaria
        builder.HasKey(u => u.Id);

        // Nombre utilizado por el usuario para iniciar sesión
        builder.Property(u => u.NombreUsuario)
            .IsRequired()
            .HasMaxLength(50);

        // Hash de la contraseña del usuario
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        // Datos identificatorios del usuario
        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.Apellido)
            .IsRequired()
            .HasMaxLength(100);

        // Permite realizar una baja logica del usuario
        builder.Property(u => u.Activo)
            .IsRequired();

        // Fecha de registro del usuario en el sistema
        builder.Property(u => u.FechaAlta)
            .IsRequired();

        // No puede haber dos usuarios con el mismo nombre de acceso
        builder.HasIndex(u => u.NombreUsuario)
            .IsUnique();

        // Relacion
        // un Rol puede tener muchos Usuarios, pero cada Usuario pertenece a un unico Rol
        builder.HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RolId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Configuraciones;

/// <summary>
/// Define la configuración de persistencia de la entidad
/// <see cref="Usuario"/> mediante Entity Framework Core.
/// </summary>
public sealed class UsuarioConfiguracion
    : IEntityTypeConfiguration<Usuario>
{
    /// <summary>
    /// Configura la tabla, propiedades, restricciones, índices
    /// y relaciones correspondientes a los usuarios.
    /// </summary>
    /// <param name="builder">
    /// Constructor utilizado por Entity Framework Core para
    /// configurar la entidad <see cref="Usuario"/>.
    /// </param>
    public void Configure(
        EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.NombreUsuario)
            .IsRequired()
            .HasMaxLength(
                UsuarioRestricciones.NombreUsuarioMaximo);

        builder.Property(usuario => usuario.PasswordHash)
            .IsRequired()
            .HasMaxLength(
                UsuarioRestricciones.PasswordHashMaximo);

        builder.Property(usuario => usuario.Nombre)
            .IsRequired()
            .HasMaxLength(
                UsuarioRestricciones.NombreMaximo);

        builder.Property(usuario => usuario.Apellido)
            .IsRequired()
            .HasMaxLength(
                UsuarioRestricciones.ApellidoMaximo);

        builder.Property(usuario => usuario.Activo)
            .IsRequired();

        builder.Property(usuario => usuario.FechaAlta)
            .IsRequired();

        builder.HasIndex(usuario => usuario.NombreUsuario)
            .IsUnique();

        builder.HasOne(usuario => usuario.Rol)
            .WithMany(rol => rol.Usuarios)
            .HasForeignKey(usuario => usuario.RolId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
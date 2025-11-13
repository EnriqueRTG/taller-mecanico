using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Domain.Entities;

namespace Taller.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración para <see cref="Usuario"/>: credenciales y rol.
    /// </summary>
    public sealed class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> e)
        {
            e.ToTable("usuarios");
            e.HasKey(x => x.Id);

            e.Property(x => x.Username)
             .IsRequired()
             .HasMaxLength(40);

            e.HasIndex(x => x.Username).IsUnique();

            e.Property(x => x.PassHash)
             .IsRequired();

            e.Property(x => x.Rol)
             .IsRequired()
             .HasMaxLength(20);

            e.Property(x => x.CreadoEn).IsRequired();
            e.Property(x => x.Activo).IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia;

/// <summary>
/// Contexto principal de Entity Framework Core para el sistema.
/// Representa la conexión entre las entidades del dominio y la base de datos.
/// </summary>
public sealed class TallerDbContext : DbContext
{
    /// <summary>
    /// Inicializa el contexto utilizando las opciones configuradas
    /// mediante inyección de dependencias (proveedor SQL Server,
    /// cadena de conexión, etc.).
    /// </summary>
    public TallerDbContext(DbContextOptions<TallerDbContext> options)
        : base(options)
    {
    }

    // Conjunto de usuarios administrados por Entity Framework Core.
    // Permite realizar consultas y operaciones sobre la entidad Usuario.
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    // Conjunto de roles administrados por Entity Framework Core.
    public DbSet<Rol> Roles => Set<Rol>();

    // Conjunto de clientes administrados por Entity Framework Core.
    public DbSet<Cliente> Clientes => Set<Cliente>();

    /// <summary>
    /// Configura el modelo de datos que utilizará Entity Framework Core.
    /// Este método se ejecuta cuando EF Core construye el modelo.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Conserva la configuración base proporcionada por DbContext.
        base.OnModelCreating(modelBuilder);

        // Busca y aplica automáticamente todas las clases que implementen
        // IEntityTypeConfiguration<T> dentro de este mismo ensamblado.
        //
        // Por ejemplo:
        // - UsuarioConfiguracion
        // - RolConfiguracion
        // - ClienteConfiguracion
        // De esta manera no es necesario registrar manualmente cada
        // configuración mediante ApplyConfiguration(...).
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TallerDbContext).Assembly);
    }
}
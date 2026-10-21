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

    // Conjunto de marcas administradas por Entity Framework Core.
    public DbSet<Marca> Marcas => Set<Marca>();

    // Conjunto de modelos administrados por Entity Framework Core.
    public DbSet<Modelo> Modelos => Set<Modelo>();

    // Conjunto de vehículos administrados por Entity Framework Core.
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();

    // Conjunto de atenciones administrados por Entity Framework Core.
    public DbSet<Atencion> Atenciones => Set<Atencion>();

    // Conjunto de diagnósticos administrados por Entity Framework Core.
    public DbSet<Diagnostico> Diagnosticos => Set<Diagnostico>();

    // Conjunto de historial de atenciones administrados por Entity Framework Core.
    public DbSet<HistorialAtencion> HistorialAtenciones => Set<HistorialAtencion>();

    // Conjunto de categorías administrados por Entity Framework Core.
    public DbSet<Categoria> Categorias => Set<Categoria>();

    // Conjunto de conceptos administrados por Entity Framework Core.
    public DbSet<Concepto> Conceptos => Set<Concepto>();

    public DbSet<Comprobante> Comprobantes => Set<Comprobante>();
    public DbSet<ComprobanteDetalle> ComprobanteDetalles => Set<ComprobanteDetalle>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<PagoMedio> PagoMedios => Set<PagoMedio>();
    public DbSet<MedioPago> MediosPago => Set<MedioPago>();

    public DbSet<Presupuesto> Presupuestos =>
    Set<Presupuesto>();

    public DbSet<PresupuestoDetalle> PresupuestoDetalles =>
        Set<PresupuestoDetalle>();

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
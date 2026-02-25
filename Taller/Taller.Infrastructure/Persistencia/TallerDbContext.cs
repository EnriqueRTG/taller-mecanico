using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia
{
    /// <summary>
    /// Contexto principal de Entity Framework Core para el sistema de gestión del taller mecánico (SGTM).
    /// Se encarga de exponer los conjuntos de entidades (DbSets) y aplicar las configuraciones
    /// de mapeo hacia la base de datos SQL Server.
    /// </summary>
    /// <remarks>
    /// Este contexto se registra en tiempo de ejecución mediante inyección de dependencias:
    /// <code>
    /// services.AddDbContext&lt;AppDbContext&gt;(options =>
    ///     options.UseSqlServer(configuration.GetConnectionString("Default")));
    /// </code>
    /// 
    /// En diseño (design-time), EF Core usa <see cref="TallerDbContextFactory"/> para generar migraciones.
    /// 
    /// Mantener esta clase libre de lógica de negocio. Su único propósito es la persistencia.
    /// </remarks>
    public sealed class TallerDbContext : DbContext
    {
        public TallerDbContext(DbContextOptions<TallerDbContext> options) : base(options) { }

        // ========= DbSets (tablas representadas en el modelo) =========

        /// <summary>Clientes registrados en el sistema.</summary>
        public DbSet<Cliente> Clientes => Set<Cliente>();

        /// <summary>Vehículos asociados a los clientes.</summary>
        public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();

        /// <summary>Órdenes de trabajo de reparación o mantenimiento.</summary>
        public DbSet<Orden> Ordenes => Set<Orden>();

        /// <summary>Detalles (ítems) pertenecientes a cada orden.</summary>
        public DbSet<OrdenItem> OrdenesItems => Set<OrdenItem>();

        /// <summary>Repuestos gestionados por el taller.</summary>
        public DbSet<Repuesto> Repuestos => Set<Repuesto>();

        /// <summary>Servicios ofrecidos (catálogo de tareas).</summary>
        public DbSet<Servicio> Servicios => Set<Servicio>();

        /// <summary>Facturas generadas a partir de órdenes completadas.</summary>
        public DbSet<Factura> Facturas => Set<Factura>();

        /// <summary>Pagos aplicados a facturas emitidas.</summary>
        public DbSet<Pago> Pagos => Set<Pago>();

        /// <summary>Usuarios (operadores o administradores del sistema).</summary>
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        // ========= Convenciones globales =========

        /// <summary>
        /// Configura convenciones globales aplicadas a todo el modelo.
        /// Ideal para definir reglas por tipo de dato (decimal, string, DateTime, etc.).
        /// </summary>
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<decimal>().HavePrecision(18, 2);
            builder.Properties<string>().HaveMaxLength(255);
            builder.Properties<DateTime>().HavePrecision(0);
        }

        // ========= Configuración del modelo =========

        /// <summary>
        /// Aplica las configuraciones definidas en clases <see cref="IEntityTypeConfiguration{TEntity}"/>.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // (Opcional) definir un esquema por defecto (evita usar dbo)
            // modelBuilder.HasDefaultSchema("taller");

            // Carga todas las configuraciones de entidad contenidas en este ensamblado
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

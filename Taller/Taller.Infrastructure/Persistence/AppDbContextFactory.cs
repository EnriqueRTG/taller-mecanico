using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Taller.Infrastructure.Persistence
{
    /// <summary>
    /// Factory de diseño para EF Core (CLI/PMC). Permite crear <see cref="AppDbContext"/>
    /// sin levantar la UI (necesaria para Add-Migration / Update-Database).
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Crea el contexto con una cadena de conexión de diseño.
        /// Prioriza variable de entorno "SGTM_CONN", luego usa un valor por defecto.
        /// </summary>
        public AppDbContext CreateDbContext(string[] args)
        {
            // 1) Tomar cadena desde variable de entorno si está definida (útil en CI)
            var conn = System.Environment.GetEnvironmentVariable("SGTM_CONN")
                       ?? "Server=.\\SQLEXPRESS;Database=SGTM_DB;Trusted_Connection=True;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(conn);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

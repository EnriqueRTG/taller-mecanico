using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Taller.Infraestructura.Persistencia.Semillas
{
    /// <summary>
    /// Orquestador principal de la siembra de datos.
    /// 
    /// 🔹 Responsabilidades:
    /// - Aplicar migraciones pendientes (solo en entornos de desarrollo o prueba).
    /// - Ejecutar los seeders definidos (Servicios, Repuestos, etc.) de forma idempotente.
    /// - Registrar cada paso del proceso en el logger para trazabilidad.
    ///
    /// En producción, las migraciones deberían aplicarse manualmente o mediante CI/CD,
    /// dejando aquí únicamente la ejecución controlada de seeders.
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Aplica las migraciones pendientes (si las hubiera) y ejecuta los seeders de forma segura.
        /// </summary>
        /// <param name="db">Instancia del <see cref="TallerDbContext"/> conectada a la base de datos.</param>
        /// <param name="logger">Instancia de <see cref="ILogger"/> para registro del proceso.</param>
        /// <param name="ct">Cancellation token opcional (permite cancelar operaciones async largas).</param>
        public static async Task EnsureSeededAsync(TallerDbContext db, ILogger logger, CancellationToken ct = default)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            try
            {
                // 1️⃣ Aplicar migraciones pendientes (solo entorno DEV o pruebas)
                logger.LogInformation("📦 Verificando migraciones pendientes...");
                await db.Database.MigrateAsync(ct);
                logger.LogInformation("✅ Migraciones aplicadas correctamente (si existían pendientes).");

                // 2️⃣ Ejecutar seeders definidos (idempotentes)
                logger.LogInformation("🌱 Iniciando seeding de datos base...");

                await ServicioSemillas.SeedAsync(db, logger, ct);
                await RepuestoSemillas.SeedAsync(db, logger, ct);
                await ClienteSemillas.SeedAsync(db, logger, ct);

                logger.LogInformation("✅ Seeding completado exitosamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Error durante el proceso de migración o seeding: {Mensaje}", ex.Message);
                throw; // se relanza para que Program.cs lo capture y muestre correctamente
            }
        }
    }
}

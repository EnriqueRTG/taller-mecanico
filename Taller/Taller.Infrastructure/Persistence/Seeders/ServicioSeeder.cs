using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taller.Domain.Entities;

namespace Taller.Infrastructure.Persistence.Seeders
{
    /// <summary>
    /// Siembra datos base para la tabla <see cref="Servicio"/>.
    /// Implementación idempotente por Código y eficiente (1 query para existentes + AddRange).
    /// </summary>
    public static class ServicioSeeder
    {
        public static async Task SeedAsync(AppDbContext db, ILogger logger, CancellationToken ct = default)
        {
            // 1) Catálogo base
            var seed = new List<Servicio>
            {
                new() { Codigo = "MO-ACEITE",  Descripcion = "Cambio de aceite (hora taller)",   TarifaHora = 15000m, Activo = true },
                new() { Codigo = "MO-FRENOS",  Descripcion = "Revisión y ajuste de frenos",      TarifaHora = 17000m, Activo = true },
                new() { Codigo = "MO-ALINBAL", Descripcion = "Alineación y balanceo",            TarifaHora = 16000m, Activo = true },
                new() { Codigo = "MO-DIAG",    Descripcion = "Diagnóstico general",              TarifaHora = 14000m, Activo = true },
            };

            // 2) Normalización y reglas mínimas
            foreach (var s in seed)
            {
                s.Codigo = s.Codigo.Trim().ToUpperInvariant();
                s.Descripcion = s.Descripcion.Trim();
                if (s.TarifaHora <= 0)
                {
                    logger.LogWarning("ServicioSeeder: TarifaHora <= 0 para {Codigo}. Ajustando a 1.", s.Codigo);
                    s.TarifaHora = 1m;
                }
            }

            // 3) Traer códigos existentes (1 sola query) y convertir a HashSet con comparador case-insensitive
            var existentes = (await db.Servicios
                .AsNoTracking()
                .Select(x => x.Codigo)
                .ToListAsync(ct))
                .ToHashSet(System.StringComparer.OrdinalIgnoreCase);

            // 4) Filtrar los que faltan
            var nuevos = seed.Where(s => !existentes.Contains(s.Codigo)).ToList();

            if (nuevos.Count == 0)
            {
                logger.LogInformation("ServicioSeeder: sin cambios (todos los códigos ya existen).");
                return;
            }

            // 5) Insertar en lote con ligera optimización
            var prev = db.ChangeTracker.AutoDetectChangesEnabled;
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                await db.Servicios.AddRangeAsync(nuevos, ct);
                await db.SaveChangesAsync(ct);
            }
            finally
            {
                db.ChangeTracker.AutoDetectChangesEnabled = prev;
            }

            // 6) Logging de detalle + resumen
            foreach (var s in nuevos)
                logger.LogInformation("Seed Servicio agregado -> {Codigo} - {Desc}", s.Codigo, s.Descripcion);

            logger.LogInformation("ServicioSeeder: agregados {Cantidad} nuevos (existentes {Existentes}).",
                nuevos.Count, existentes.Count);
        }
    }
}

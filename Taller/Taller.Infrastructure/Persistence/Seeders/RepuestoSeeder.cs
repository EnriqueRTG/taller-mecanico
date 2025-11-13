using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taller.Domain.Entities;

namespace Taller.Infrastructure.Persistence.Seeders
{
    /// <summary>
    /// Siembra datos base para la tabla <see cref="Repuesto"/>.
    /// Idempotente por Código y eficiente (1 query para existentes + AddRange).
    /// </summary>
    public static class RepuestoSeeder
    {
        /// <summary>
        /// Inserta repuestos base si no existen (comparando por <see cref="Repuesto.Codigo"/>).
        /// </summary>
        /// <param name="db">DbContext de la aplicación.</param>
        /// <param name="logger">Logger para trazabilidad del proceso.</param>
        /// <param name="ct">Cancellation token opcional.</param>
        public static async Task SeedAsync(AppDbContext db, ILogger logger, CancellationToken ct = default)
        {
            // 1) Catálogo base
            var seed = new List<Repuesto>
            {
                new() { Codigo = "RP-FILT-ACE", Descripcion = "Filtro de aceite",   Stock = 50,  StockMin = 10, Costo = 1800m, Precio = 2500m, Activo = true },
                new() { Codigo = "RP-BUJ-NGK",  Descripcion = "Bujía NGK",          Stock = 100, StockMin = 20, Costo = 1200m, Precio = 1800m, Activo = true },
                new() { Codigo = "RP-PAST-FRN", Descripcion = "Pastillas de freno", Stock = 30,  StockMin = 6,  Costo = 6500m, Precio = 9000m, Activo = true },
                new() { Codigo = "RP-FILT-AIR", Descripcion = "Filtro de aire",     Stock = 40,  StockMin = 8,  Costo = 2200m, Precio = 3200m, Activo = true },
            };

            // 2) Normalización & reglas defensivas
            foreach (var r in seed)
            {
                r.Codigo = r.Codigo.Trim().ToUpperInvariant();
                r.Descripcion = r.Descripcion.Trim();

                if (r.Precio < r.Costo)
                {
                    logger.LogWarning("RepuestoSeeder: Precio < Costo para {Codigo}. Ajustando Precio={Precio} a Costo={Costo}.",
                        r.Codigo, r.Precio, r.Costo);
                    r.Precio = r.Costo; // o un margen p.ej. r.Costo * 1.2m
                }

                if (r.StockMin > r.Stock)
                {
                    logger.LogWarning("RepuestoSeeder: StockMin > Stock para {Codigo}. Ajustando StockMin={StockMin} a Stock={Stock}.",
                        r.Codigo, r.StockMin, r.Stock);
                    r.StockMin = r.Stock;
                }
            }

            // 3) Traer códigos existentes (1 query) y armar HashSet case-insensitive en memoria
            var existentes = (await db.Repuestos
                    .AsNoTracking()
                    .Select(x => x.Codigo)
                    .ToListAsync(ct))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 4) Filtrar sólo los que faltan
            var nuevos = seed.Where(r => !existentes.Contains(r.Codigo)).ToList();
            if (nuevos.Count == 0)
            {
                logger.LogInformation("Seeder Repuestos: sin cambios (todos los códigos ya existen).");
                return;
            }

            // 5) Insertar en lote con ligera optimización
            var prev = db.ChangeTracker.AutoDetectChangesEnabled;
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                await db.Repuestos.AddRangeAsync(nuevos, ct);
                await db.SaveChangesAsync(ct);
            }
            finally
            {
                db.ChangeTracker.AutoDetectChangesEnabled = prev;
            }

            // 6) Logs de detalle + resumen
            foreach (var r in nuevos)
                logger.LogInformation("Seed Repuesto agregado -> {Codigo} - {Desc}", r.Codigo, r.Descripcion);

            logger.LogInformation("Seeder Repuestos: agregados {Cantidad} nuevos (existentes {Existentes}).",
                nuevos.Count, existentes.Count);
        }
    }
}

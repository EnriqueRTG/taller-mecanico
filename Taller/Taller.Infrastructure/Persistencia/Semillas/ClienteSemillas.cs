using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Semillas
{
    /// <summary>
    /// Seed de clientes de prueba.
    /// </summary>
    public static class ClienteSemillas
    {
        public static async Task SeedAsync(TallerDbContext db, ILogger logger, CancellationToken ct = default)
        {
            var seed = new List<Cliente>
            {
                new()
                {
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Email = "juan.perez@test.com",
                    Telefono = "3794-111111",
                    Direccion = "San Martín 123", 
                    DocTipo = "DNI", 
                    DocNro = "30123456"
                },
                new()
                {
                    Nombre = "María",
                    Apellido = "Gómez",
                    Email = "maria.gomez@test.com",
                    Telefono = "3794-222222",
                    Direccion = "Belgrano 456",
                    DocTipo = "DNI", 
                    DocNro = "28999888"
                },
                new()
                {
                    Nombre = "Carlos",
                    Apellido = "López",
                    Email = "carlos.lopez@test.com",
                    Telefono = "3794-333333",
                    Direccion = "Rivadavia 789",
                    DocTipo = "DNI", 
                    DocNro = "27555444"
                },
            };

            // Normalización mínima (evita null warnings con ?.Trim())
            foreach (var c in seed)
            {
                c.Nombre = c.Nombre.Trim();
                c.Apellido = c.Apellido.Trim();
                c.Email = c.Email?.Trim();
                c.Telefono = c.Telefono?.Trim();
                c.Direccion = c.Direccion?.Trim();
                c.DocTipo = c.DocTipo?.Trim();
            }

            // Evitar duplicados por Nombre+Apellido (ejemplo simple)
            var existentes = await db.Clientes
                .AsNoTracking()
                .Select(x => new { x.Nombre, x.Apellido })
                .ToListAsync(ct);

            var nuevos = seed
                .Where(s => !existentes.Any(e =>
                    e.Nombre == s.Nombre && e.Apellido == s.Apellido))
                .ToList();

            if (nuevos.Count == 0)
            {
                logger.LogInformation("ClienteSeeder: sin cambios (todos los clientes seed ya existen).");
                return;
            }

            await db.Clientes.AddRangeAsync(nuevos, ct);
            await db.SaveChangesAsync(ct);

            logger.LogInformation("ClienteSeeder: agregados {Cantidad} clientes de prueba.", nuevos.Count);
        }
    }
}


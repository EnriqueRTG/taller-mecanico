using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Semillas
{
    public static class VehiculoSemillas
    {
        public static async Task SeedAsync(
            TallerDbContext db,
            ILogger logger,
            CancellationToken ct = default)
        {
            // si ya hay vehículos, podes salir o verificar uno a uno.
            if (await db.Vehiculos.AnyAsync(ct))
            {
                logger.LogInformation("VehiculoSemillas: ya existen vehiculos, no se agregan nuevos.");
                return;
            }

            // Buscar los clientes por documento (los que sembramos antes)
            var clientes = await db.Clientes
                .Where(c =>
                (c.DocTipo == "DNI" && c.DocNro == "30123456") || // Juan
                (c.DocTipo == "DNI" && c.DocNro == "28999888") || // María
                (c.DocTipo == "DNI" && c.DocNro == "27555444"))   // Carlos
                .ToListAsync(ct);

            var clienteJuan = clientes.FirstOrDefault(c => c.DocNro == "30123456" && c.DocTipo == "DNI");
            var clienteMaria = clientes.FirstOrDefault(c => c.DocNro == "28999888" && c.DocTipo == "DNI");
            var clienteCarlos = clientes.FirstOrDefault(c => c.DocNro == "27555444" && c.DocTipo == "DNI");

            var vehiculos = new List<Vehiculo>();

            if (clienteJuan is not null)
            {
                vehiculos.Add(new Vehiculo
                {
                    ClienteId = clienteJuan.Id,
                    Patente = "AA123BB",
                    Marca = "Fiat",
                    Modelo = "Palio Attractive",
                    Anio = 2014,
                    Color = "Rojo",
                    Vin = "FIATPALIOVIN001",
                    Observaciones = "Auto de uso diario del cliente Juan.",
                    Activo = true
                });
            }

            if (clienteMaria is not null)
            {
                vehiculos.Add(new Vehiculo
                {
                    ClienteId = clienteMaria.Id,
                    Patente = "AB456CD",
                    Marca = "Chevrolet",
                    Modelo = "Onix",
                    Anio = 2018,
                    Color = "Gris",
                    Vin = "CHEVONIXVIN001",
                    Observaciones = "Vehículo principal de María.",
                    Activo = true
                });
            }

            if (clienteCarlos is not null)
            {
                vehiculos.Add(new Vehiculo
                {
                    ClienteId = clienteCarlos.Id,
                    Patente = "AB123AJ",
                    Marca = "Toyota",
                    Modelo = "Corolla",
                    Anio = 2019,
                    Color = "Gris",
                    Vin = "TOYOCOROLLAVIN001",
                    Observaciones = "Auto utilizado como UBER por Carlos.",
                    Activo = true
                });
            }

            // Idempotencia extra por patente (si cambia logica implementada arriba)
            var patentesExistentes = await db.Vehiculos
                .AsNoTracking()
                .Select(v => v.Patente)
                .ToListAsync(ct);

            var nuevos = vehiculos
                .Where(v => !patentesExistentes.Contains(v.Patente))
                .ToList();

            if (nuevos.Count == 0)
            {
                logger.LogInformation("VehiculoSemillas: sin cambios (todas las patentes seed ya existen).");
                return;
            }

            await db.Vehiculos.AddRangeAsync(nuevos, ct);
            await db.SaveChangesAsync(ct);

            logger.LogInformation("VehiculoSemillas: {Count} vehiculos de prueba agregados.", nuevos.Count);

        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Dominio.Entidades;
using Taller.Dominio.Repositorios;

namespace Taller.Infraestructura.Persistencia.Repositorios
{
    public sealed class VehiculoRepositorio : IVehiculoRepositorio
    {
        public readonly TallerDbContext _db;

        public VehiculoRepositorio(TallerDbContext db)
        {
            _db = db;
        }   

        public async Task ActualizarAsync(Vehiculo vehiculo)
        {
            _db.Vehiculos.Update(vehiculo);
            await _db.SaveChangesAsync();
        }

        public async Task AgregarAsync(Vehiculo vehiculo)
        {
            _db.Vehiculos.Add(vehiculo);
            await _db.SaveChangesAsync();
        }

        public async Task EliminarAsync(Vehiculo vehiculo)
        {
            _db.Vehiculos.Remove(vehiculo);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistePorPatenteAsync(string patente, long? excluirId = null)
        {
            var query = _db.Vehiculos.AsQueryable()
                .Where(v => v.Patente == patente);

            if (excluirId is not null)
                query = query.Where(v => v.Id != excluirId.Value);

            return await query.AnyAsync();
        }

        public async Task<IReadOnlyList<Vehiculo>> ListarAsync()
            => await _db.Vehiculos
            .AsNoTracking()
            .Include(v => v.Cliente)
            .OrderBy(v => v.Patente)
            .ToListAsync();

        public async Task<IReadOnlyList<Vehiculo>> ListarPorClienteAsync(long clienteId)
            => await _db.Vehiculos
            .AsNoTracking()
            .Where(v => v.ClienteId == clienteId)
            .Include(v => v.Cliente)
            .OrderBy(v => v.Patente)
            .ToListAsync();

        public async Task<Vehiculo?> ObtenerPorIdAsync(long id)
            => await _db.Vehiculos
            .Include(v => v.Cliente)
            .FirstOrDefaultAsync(v => v.Id == id);
    }


}

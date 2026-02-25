using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taller.Dominio.Entidades;
using Taller.Dominio.Repositorios;
using Taller.Infraestructura.Persistencia;

namespace Taller.Infraestructura.Persistencia.Repositorios;

public class ClienteRepositorio : IClienteRepositorio
{
    private readonly TallerDbContext _contexto;

    public ClienteRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Cliente?> ObtenerPorIdAsync(long id)
    {
        return await _contexto.Clientes.FindAsync(id);
    }

    public async Task<IReadOnlyList<Cliente>> ListarAsync()
    {
        return await _contexto.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task AgregarAsync(Cliente cliente)
    {
        _contexto.Clientes.Add(cliente);
        await _contexto.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Cliente cliente)
    {
        _contexto.Clientes.Update(cliente);
        await _contexto.SaveChangesAsync();
    }

    public async Task EliminarAsync(Cliente cliente)
    {
        _contexto.Clientes.Remove(cliente);
        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> ExistePorDocumentoAsync(string documento, long? excluirId = null)
    {
        var query = _contexto.Clientes.AsQueryable()
            .Where(c => c.DocNro == documento);

        if (excluirId.HasValue)
            query = query.Where(c => c.Id != excluirId.Value);

        return await query.AnyAsync();
    }
}

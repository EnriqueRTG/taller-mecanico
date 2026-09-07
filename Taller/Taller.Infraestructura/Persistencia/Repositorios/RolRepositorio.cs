using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las operaciones de consulta de roles
/// mediante Entity Framework Core.
/// </summary>
public sealed class RolRepositorio : IRolRepositorio
{
    private readonly TallerDbContext _contexto;

    public RolRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Rol>> ListarActivosAsync()
    {
        return await _contexto.Roles
            .Where(r => r.Activo)
            .OrderBy(r => r.Nombre)
            .ToListAsync();
    }

    public async Task<Rol?> ObtenerPorIdAsync(int idRol)
    {
        return await _contexto.Roles
            .FirstOrDefaultAsync(r => r.Id == idRol);
    }
}
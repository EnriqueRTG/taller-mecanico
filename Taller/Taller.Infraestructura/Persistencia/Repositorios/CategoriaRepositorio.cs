using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las operaciones de consulta de categorias
/// mediante Entity Framework Core.
/// </summary>
public sealed class CategoriaRepositorio : ICategoriaRepositorio
{
    private readonly TallerDbContext _contexto;

    public CategoriaRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Categoria>> ListarActivosAsync()
    {
        return await _contexto.Categorias
            .Where(r => r.Activo)
            .OrderBy(r => r.Nombre)
            .ToListAsync();
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int idCategoria)
    {
        return await _contexto.Categorias
            .FirstOrDefaultAsync(r => r.IdCategoria == idCategoria);
    }
}

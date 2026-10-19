using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

public sealed class CategoriaRepositorio : ICategoriaRepositorio
{
    private readonly TallerDbContext _contexto;
    
    public CategoriaRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteNombreAsync(
        string nombreCategoria)
    {
        return await _contexto.Categorias
            .AsNoTracking()
            .AnyAsync(
                categoria =>
                    categoria.Nombre == nombreCategoria);
    }

    public async Task<List<Categoria>> ListarAsync()
    {
        return await _contexto.Categorias
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(
                categoria =>
                    categoria.IdCategoria == id);
    }

    public async Task<Categoria?> ObtenerPorNombreAsync(string nombre)
    {
        return await _contexto.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(
                categoria =>
                    categoria.Nombre == nombre);
    }
}

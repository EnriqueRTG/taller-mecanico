using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;

public class CategoriaServicio
{
    private readonly ICategoriaRepositorio _categoriaRepositorio;

    public CategoriaServicio(ICategoriaRepositorio categoriaRepositorio)
    {
        _categoriaRepositorio = categoriaRepositorio;
    }

    public async Task<List<Categoria>> ListarAsync()
    {
        return await _categoriaRepositorio.ListarActivosAsync();
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        return await _categoriaRepositorio.ObtenerPorIdAsync(id);
    }
}
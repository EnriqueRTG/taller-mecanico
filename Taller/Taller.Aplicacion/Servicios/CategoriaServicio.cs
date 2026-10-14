using Taller.Dominio.Entidades;
using Taller.Aplicacion.Abstracciones.Persistencia;

namespace Taller.Aplicacion.Servicios;

public sealed class CategoriaServicio
{
    private readonly ICategoriaRepositorio _categoriaRepositorio;

    public CategoriaServicio(ICategoriaRepositorio categoriaRepositorio)
    {
        _categoriaRepositorio = categoriaRepositorio;
    }

    public async Task<Categoria?> ObtenerPorIdAsync(
        int id)
    {
        ValidarId(id);

        return await _categoriaRepositorio.ObtenerPorIdAsync(id);
    }

    /// <summary>
    /// Verifica que el identificador sea mayor que cero.
    /// </summary>
    private static void ValidarId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "El identificador de la categoría no es válido.",
                nameof(id));
        }
    }
}

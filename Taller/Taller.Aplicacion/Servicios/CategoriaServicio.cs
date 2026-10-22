using Taller.Dominio.Entidades;
using Taller.Aplicacion.Abstracciones.Persistencia;

namespace Taller.Aplicacion.Servicios;
public sealed class CategoriaServicio
{
    private readonly ICategoriaRepositorio _categoriaRepositorio;
    private readonly Taller.Aplicacion.Seguridad.AutorizacionOperativa _acceso;
    public CategoriaServicio(ICategoriaRepositorio categoriaRepositorio, Taller.Aplicacion.Seguridad.AutorizacionOperativa acceso)
    {
        _categoriaRepositorio = categoriaRepositorio;
        _acceso = acceso;
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        await _acceso.ExigirAsync();
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
            throw new ArgumentException("El identificador de la categoría no es válido.", nameof(id));
        }
    }
}

using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Aplicacion.Excepciones;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Proporciona las operaciones de negocio relacionadas
/// con la consulta y gestión de conceptos del catálogo.
/// </summary>
public sealed class ConceptoServicio
{
    private readonly IConceptoRepositorio _conceptoRepositorio;
    private readonly ICategoriaRepositorio _categoriaRepositorio;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de conceptos.
    /// </summary>
    public ConceptoServicio(
        IConceptoRepositorio conceptoRepositorio,
        ICategoriaRepositorio categoriaRepositorio)
    {
        _conceptoRepositorio =
            conceptoRepositorio
            ?? throw new ArgumentNullException(
                nameof(conceptoRepositorio));

        _categoriaRepositorio =
            categoriaRepositorio
            ?? throw new ArgumentNullException(
                nameof(categoriaRepositorio));
    }

    /// <summary>
    /// Obtiene todos los conceptos activos registrados en el catálogo.
    /// </summary>
    public async Task<List<Concepto>> ListarAsync()
    {
        return await _conceptoRepositorio.ListarActivosAsync();
    }

    /// <summary>
    /// Obtiene las categorías activas que pueden asignarse
    /// a los conceptos del sistema.
    /// </summary>
    public async Task<List<Categoria>> ListarCategoriasDisponiblesAsync()
    {
        List<Categoria> categoriasRegistradas =
            await _categoriaRepositorio.ListarActivosAsync();

        return categoriasRegistradas
            .Where(EsCategoriaDisponible)
            .OrderBy(categoria => categoria.IdCategoria)
            .ToList();
    }
    /// <summary>
    /// Lista los conceptos por categoria
    /// </summary>
    public async Task<List<Concepto>> ListarPorCategoriaAsync(int idCategoria)
    {
        if (idCategoria <= 0)
        {
            throw new ValidacionException("El identificador de la categoría no es válido.", nameof(idCategoria));
        }

        var conceptos = await _conceptoRepositorio.ListarActivosAsync();
        return conceptos.Where(c => c.IdCategoria == idCategoria).OrderBy(c => c.Nombre).ToList();
    }

    /// <summary>
    /// Obtiene un concepto mediante su identificador.
    /// </summary>
    public async Task<Concepto?> ObtenerPorIdAsync(int idConcepto)
    {
        ValidarId(idConcepto);

        return await _conceptoRepositorio.ObtenerPorIdAsync(idConcepto);
    }

    /// <summary>
    /// Comprueba que una categoría coincida con una de las categorías
    /// habilitadas por la aplicación.
    /// </summary>
    private static bool EsCategoriaDisponible(
        Categoria categoria)
    {
        if (!categoria.Activo
            || !CategoriaSistema.EsCategoriaValida(categoria.IdCategoria))
        {
            return false;
        }

        string? nombreEsperado =
            CategoriaSistema.ObtenerNombreEsperado(categoria.IdCategoria);

        return nombreEsperado is not null
            && !string.IsNullOrWhiteSpace(categoria.Nombre)
            && string.Equals(
                categoria.Nombre,
                nombreEsperado,
                StringComparison.Ordinal);
    }

    /// <summary>
    /// Valida que un identificador de concepto sea mayor que cero.
    /// </summary>
    private static void ValidarId(int idConcepto)
    {
        if (idConcepto <= 0)
        {
            throw new ValidacionException(
                "El identificador del concepto no es válido.",
                nameof(idConcepto));
        }
    }
}
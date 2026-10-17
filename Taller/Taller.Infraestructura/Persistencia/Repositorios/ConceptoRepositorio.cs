using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

public sealed class ConceptoRepositorio : IConceptoRepositorio
{
    private readonly TallerDbContext _contexto;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio.
    /// </summary>
    /// <param name="contexto">
    /// Contexto de Entity Framework Core utilizado para acceder
    /// a la base de datos.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando el contexto recibido es nulo.
    /// </exception>
    public ConceptoRepositorio(
        TallerDbContext contexto)
    {
        _contexto =
            contexto
            ?? throw new ArgumentNullException(
                nameof(contexto));
    }

    /// <summary>
    /// Obtiene un concepto mediante su identificador,
    /// incluyendo la información de su categoría.
    /// </summary>
    public async Task<Concepto?> ObtenerPorIdAsync(int idConcepto)
    {
        return await _contexto.Conceptos
            .AsNoTracking()
            .Include(concepto => concepto.Categoria)
            .SingleOrDefaultAsync(
                concepto => concepto.IdConcepto == idConcepto);
    }

    /// <summary>
    /// Obtiene un concepto mediante su nombre,
    /// incluyendo la información de su categoría.
    /// </summary>
    public async Task<Concepto?> ObtenerPorNombreConceptoAsync(string nombreConcepto)
    {
        return await _contexto.Conceptos
            .AsNoTracking()
            .Include(concepto => concepto.Categoria)
            .SingleOrDefaultAsync(
                concepto => concepto.Nombre == nombreConcepto);
    }

    /// <summary>
    /// Obtiene un concepto mediante su tipo y nombre.
    /// </summary>
    public async Task<Concepto?> ObtenerPorTipoAsync(string tipo, string nombre)
    {
        // Nota: Si 'tipo' en tu lógica representa el enum convertido a string o texto, 
        // puedes adaptarlo aquí según cómo lo guardes en base de datos.
        return await _contexto.Conceptos
            .AsNoTracking()
            .Include(concepto => concepto.Categoria)
            .SingleOrDefaultAsync(
                concepto => concepto.Nombre == nombre);
    }

    /// <summary>
    /// Obtiene todos los conceptos activos junto con sus categorías,
    /// ordenados por tipo y nombre.
    /// </summary>
    public async Task<List<Concepto>> ListarActivosAsync()
    {
        return await _contexto.Conceptos
            .AsNoTracking()
            .Include(concepto => concepto.Categoria)
            .Where(concepto => concepto.Activo)
            .OrderBy(concepto => concepto.Tipo)
            .ThenBy(concepto => concepto.Nombre)
            .ToListAsync();
    }
}
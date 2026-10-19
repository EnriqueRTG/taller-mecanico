using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las consultas de persistencia de conceptos.
/// </summary>
public sealed class ConceptoRepositorio : IConceptoRepositorio
{
    private readonly TallerDbContext _contexto;

    public ConceptoRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto
            ?? throw new ArgumentNullException(nameof(contexto));
    }

    public Task<Concepto?> ObtenerPorIdAsync(int idConcepto)
    {
        return _contexto.Conceptos
            .AsNoTracking()
            .Include(c => c.Categoria)
            .FirstOrDefaultAsync(c => c.IdConcepto == idConcepto);
    }

    public Task<List<Concepto>> ListarServiciosActivosAsync()
    {
        return _contexto.Conceptos
            .AsNoTracking()
            .Include(c => c.Categoria)
            .Where(c =>
                c.Activo &&
                c.Categoria.Activo &&
                c.Tipo == TipoConcepto.Servicio)
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.IdConcepto)
            .ToListAsync();
    }
}
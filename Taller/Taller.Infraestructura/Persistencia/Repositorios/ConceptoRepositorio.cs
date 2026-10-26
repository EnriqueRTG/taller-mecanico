using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Repositorios;
/// <summary>Lee la oferta real y su disponibilidad de catálogo; no representa inventario.</summary>
public sealed class ConceptoRepositorio(TallerDbContext contexto) : IConceptoRepositorio
{
    public Task<List<Concepto>> ListarCatalogoAsync() => contexto.Conceptos.AsNoTracking().Include(c => c.Categoria)
        .OrderBy(c => c.Categoria.Nombre).ThenBy(c => c.Nombre).ThenBy(c => c.IdConcepto).ToListAsync();
}

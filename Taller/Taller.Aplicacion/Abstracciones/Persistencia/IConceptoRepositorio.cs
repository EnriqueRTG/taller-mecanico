using Taller.Dominio.Entidades;
namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>Consulta el catálogo persistido, incluidos conceptos inactivos para consulta.</summary>
public interface IConceptoRepositorio
{
    Task<List<Concepto>> ListarCatalogoAsync();
}

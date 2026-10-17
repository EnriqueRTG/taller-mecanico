using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para consultar los conceptos del sistema.
/// </summary>
public interface IConceptoRepositorio
{
    Task<Concepto?> ObtenerPorIdAsync(int idConcepto);
    Task<Concepto?> ObtenerPorNombreConceptoAsync(string nombreConcepto);
    Task<Concepto?> ObtenerPorTipoAsync(string tipo, string nombre);
    Task<List<Concepto>> ListarActivosAsync();         
}
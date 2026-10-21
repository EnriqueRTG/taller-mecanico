using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las consultas de persistencia del catálogo de conceptos.
/// </summary>
public interface IConceptoRepositorio
{
    /// <summary>
    /// Obtiene un concepto y su categoría.
    /// Devuelve null si no existe.
    /// </summary>
    Task<Concepto?> ObtenerPorIdAsync(int idConcepto);

    /// <summary>
    /// Lista servicios activos pertenecientes a categorías activas.
    /// </summary>
    Task<List<Concepto>> ListarServiciosActivosAsync();

    /// <summary>
    /// Lista los servicios habilitados para ingreso que estén activos
    /// y pertenezcan a categorías activas.
    /// </summary>
    Task<List<Concepto>> ListarServiciosDisponiblesParaIngresoAsync();
}
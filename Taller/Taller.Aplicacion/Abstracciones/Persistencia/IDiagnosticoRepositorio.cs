using Taller.Dominio.Entidades;
using Taller.Aplicacion.DTOs.Diagnosticos;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia de informes técnicos.
/// </summary>
public interface IDiagnosticoRepositorio
{
    /// <summary>
    /// Obtiene un diagnóstico sin seguimiento.
    /// Devuelve null si no existe.
    /// </summary>
    Task<Diagnostico?> ObtenerPorIdAsync(
        int idDiagnostico);

    /// <summary>
    /// Obtiene los datos de consulta de un informe registrado.
    /// Devuelve null si no existe.
    /// </summary>
    Task<DiagnosticoDetalleDto?> ObtenerDetalleAsync(int idDiagnostico);

    /// <summary>
    /// Lista los informes de una atención en orden cronológico.
    /// </summary>
    Task<List<Diagnostico>> ListarPorAtencionAsync(
        int idAtencion);

    /// <summary>
    /// Lista las atenciones pendientes o en actividad de diagnóstico
    /// y aquellas que ya cuentan con un informe inicial.
    /// </summary>
    Task<List<DiagnosticoBandejaDto>> ListarBandejaAsync();

    /// <summary>
    /// Comprueba si la atención ya tiene un informe inicial.
    /// </summary>
    Task<bool> ExisteInicialAsync(
        int idAtencion);

    /// <summary>
    /// Incorpora un diagnóstico nuevo al contexto.
    /// No confirma los cambios en la base de datos.
    /// </summary>
    void Agregar(
        Diagnostico diagnostico);

}
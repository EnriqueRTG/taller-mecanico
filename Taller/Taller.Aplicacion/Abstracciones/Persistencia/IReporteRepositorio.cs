using Taller.Aplicacion.DTOs.Reportes;
namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>Proyecta registros reales sin seguimiento. El servicio decide el alcance de usuario antes de consultar.</summary>
public interface IReporteRepositorio
{
    Task<List<ReporteRegistroDto>> ListarCargaAsync(int? tecnico);
    Task<List<ReporteRegistroDto>> ListarAtencionesAsync(DateTime desde, DateTime hastaExclusivo);
    Task<List<ReporteRegistroDto>> ListarPresupuestosAsync(DateTime desde, DateTime hastaExclusivo, int? tecnico);
    Task<List<ReporteRegistroDto>> ListarMovimientosAsync(DateTime desde, DateTime hastaExclusivo);
    Task<List<ReporteRegistroDto>> ListarSaldosAsync();
    Task<List<ReporteRegistroDto>> ListarActividadAsync(DateTime desde, DateTime hastaExclusivo, int? tecnico);
    Task<List<ReporteRegistroDto>> ListarConceptosAsync(DateTime desde, DateTime hastaExclusivo, int? tecnico);
}

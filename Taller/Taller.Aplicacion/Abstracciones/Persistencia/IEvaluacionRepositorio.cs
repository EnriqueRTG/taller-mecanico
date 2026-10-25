using Taller.Dominio.Entidades;
using Taller.Aplicacion.DTOs.Atenciones;
namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>Consultas y preparación de cambios del expediente técnico. No confirma guardados.</summary>
public interface IEvaluacionRepositorio
{
    Task<Atencion?> ObtenerAtencionAsync(int id);
    Task<EvaluacionAtencion?> ObtenerAvanceAsync(int id);
    Task<List<Presupuesto>> ObtenerPresupuestosAsync(int id);
    Task<List<Diagnostico>> ObtenerDiagnosticosAsync(int id);
    Task<List<Concepto>> ListarCatalogoAsync();
    Task<List<EvaluacionBandejaDto>> ListarAsync();
    Task<List<Taller.Aplicacion.DTOs.Ejecuciones.TrabajoEnCursoDto>> ListarEjecucionesAsync();
    Task<List<PresupuestoConsultaDto>> ListarPresupuestosAsync();
    void PrepararVersion(Atencion atencion, byte[] version);
    void EliminarBorrador(Presupuesto presupuesto);
    void AgregarAvance(EvaluacionAtencion avance);
    void AgregarPresupuesto(Presupuesto presupuesto);
    void AgregarDiagnostico(Diagnostico diagnostico);
    void AgregarHistorial(HistorialAtencion historial);
    void ReemplazarDetalles(Presupuesto presupuesto, IEnumerable<PresupuestoDetalle> detalles);
}

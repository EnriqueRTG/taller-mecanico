using Taller.Aplicacion.DTOs.Diagnosticos;
namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>Consulta el informe definitivo; Evaluación persiste informe y propuesta conjuntamente.</summary>
public interface IDiagnosticoRepositorio
{
    Task<DiagnosticoDetalleDto?> ObtenerDetalleAsync(int idDiagnostico);
}

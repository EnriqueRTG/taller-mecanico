using Taller.Aplicacion.DTOs.Respaldos;
namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>Operaciones nativas y coordinación duradera de recuperación; no utiliza el DbContext de negocio.</summary>
public interface IRespaldoMotor
{
    bool Habilitado { get; }
    Task<EstadoRespaldosDto> ConsultarAsync(CancellationToken cancelacion = default);
    Task<ProgramaRespaldoDto> ObtenerProgramaAsync(CancellationToken cancelacion = default);
    Task GuardarProgramaAsync(ProgramaRespaldoDto programa, CancellationToken cancelacion = default);
    Task<CopiaRespaldoDto> CrearAsync(TipoRespaldo tipo, string carpeta, string autor, IProgress<string>? progreso = null, CancellationToken cancelacion = default);
    Task<PlanRestauracionDto> ProbarRestauracionAsync(Guid copiaId, int solicitante, IProgress<string>? progreso = null, CancellationToken cancelacion = default);
    Task<ResultadoRestauracionDto> RestaurarAsync(PlanRestauracionDto plan, AprobadorRestauracionDto primero, AprobadorRestauracionDto segundo, string motivo, IProgress<string>? progreso = null);
    Task EjecutarProgramacionAsync(CancellationToken cancelacion = default);
}

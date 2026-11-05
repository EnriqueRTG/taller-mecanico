namespace Taller.Aplicacion.Consultas;
/// <summary>Selecciona hitos útiles para seguimiento técnico; no representa una auditoría exhaustiva del historial.</summary>
/// <remarks>CorreccionAdicionalPreparada permanece en el expediente, sin agregar un filtro o una magnitud de negocio al reporte.</remarks>
public static class ActividadTecnicaReporte
{
    public static IReadOnlyList<string> Acciones { get; } = Array.AsReadOnly(new[]
    {
        "EvaluacionTomada",
        "EvaluacionLiberada",
        "EvaluacionAvanceGuardado",
        "EvaluacionFinalizada",
        "EjecucionTomada",
        "EjecucionLiberada",
        "EjecucionAvanceGuardado",
        "TrabajoFinalizado",
        "HallazgoRegistrado",
        "PausaResuelta",
        "MaterialClienteRevisado",
        "PresupuestoAdicionalPreparado",
        "SeleccionParcialRevisada",
        "SeleccionClienteValidada"
    });
    /// <summary>Hitos ofrecidos como filtros individuales. No limita los eventos consultados con «Todos».</summary>
    public static IReadOnlyList<string> AccionesFiltrables { get; } = Array.AsReadOnly(new[]
    {
        "EvaluacionTomada",
        "EvaluacionFinalizada",
        "EjecucionTomada",
        "TrabajoFinalizado",
        "HallazgoRegistrado",
        "SeleccionClienteValidada",
        "SeleccionParcialRevisada"
    });
}

namespace Taller.Aplicacion.Consultas;
/// <summary>Define las acciones del historial incluidas en el reporte de actividad técnica.</summary>
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

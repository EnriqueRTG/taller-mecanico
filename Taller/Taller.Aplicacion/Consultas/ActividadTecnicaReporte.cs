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
}

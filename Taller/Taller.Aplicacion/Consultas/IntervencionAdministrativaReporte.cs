namespace Taller.Aplicacion.Consultas;

public static class IntervencionAdministrativaReporte
{
    public static IReadOnlyList<string> Acciones { get; } = Array.AsReadOnly(new[]
    {
        "RecepcionRegistrada", "PresupuestoPresentado", "DecisionPresupuestoRegistrada",
        "LiquidacionEmitida", "PagoRegistrado", "PagoAnulado", "VehiculoEntregado"
    });
}

namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Identifica el servicio o los trabajos que contempla un presupuesto.
/// </summary>
public enum TipoPresupuesto
{
    /// <summary>
    /// Registra el importe del servicio inicial autorizado
    /// al enviar el vehículo a diagnóstico.
    /// Se vincula directamente con la atención, porque todavía
    /// no existe un informe técnico.
    /// </summary>
    Diagnostico = 1,

    /// <summary>
    /// Contempla trabajos propuestos a partir de un diagnóstico.
    /// Requiere la aceptación del cliente antes de ejecutarse.
    /// Incluye tanto la propuesta inicial como las adicionales.
    /// </summary>
    Reparacion = 2
}
using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;

/// <summary>Etiquetas de consulta; no cambia los valores persistidos ni las transiciones.</summary>
public static class TextoAtencion
{
    public static string Estado(EstadoAtencion estado) => estado switch
    {
        EstadoAtencion.EnEvaluacionTecnica => "En preparación técnica",
        EstadoAtencion.PendienteDecision => "Pendiente de decisión",
        EstadoAtencion.EnEjecucion => "En ejecución",
        EstadoAtencion.TrabajoFinalizado => "Trabajo finalizado",
        EstadoAtencion.PendienteLiquidacion => "Pendiente de liquidación",
        EstadoAtencion.Cerrada => "Cerrada",
        EstadoAtencion.Cancelada => "Cancelada",
        _ => "Estado desconocido"
    };
    public static string Solicitud(TipoSolicitudAtencion tipo) => tipo switch
    {
        TipoSolicitudAtencion.Problema => "Problema a diagnosticar",
        TipoSolicitudAtencion.ServicioDefinido => "Servicio definido",
        TipoSolicitudAtencion.Mixta => "Mixta",
        _ => "Solicitud desconocida"
    };
}

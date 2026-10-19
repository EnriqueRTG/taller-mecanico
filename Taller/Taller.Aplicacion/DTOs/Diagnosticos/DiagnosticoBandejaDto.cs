using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Diagnosticos;

/// <summary>
/// Representa una atención y su diagnóstico inicial
/// en la bandeja de trabajo técnico.
/// </summary>
/// <remarks>
/// Puede representar una actividad pendiente o en curso
/// aunque todavía no exista un informe registrado.
/// </remarks>
public sealed class DiagnosticoBandejaDto
{
    public int IdAtencion { get; init; }
    public int? IdDiagnostico { get; init; }

    public string Cliente { get; init; } = string.Empty;
    public string Vehiculo { get; init; } = string.Empty;
    public string Dominio { get; init; } = string.Empty;
    public string MotivoConsulta { get; init; } = string.Empty;

    public EstadoAtencion EstadoAtencion { get; init; }

    public int? IdTecnicoEnActividad { get; init; }
    public ActividadTecnica? ActividadTecnica { get; init; }

    public string? TecnicoEnActividad { get; init; }

    public int? IdTecnicoAutor { get; init; }
    public string? TecnicoAutor { get; init; }

    public DateTime FechaApertura { get; init; }

    /// <summary>
    /// Fecha de la última toma de diagnóstico
    /// correspondiente a la reserva actual.
    /// </summary>
    public DateTime? FechaTomaDiagnostico { get; init; }

    public DateTime? FechaDiagnostico { get; init; }
}
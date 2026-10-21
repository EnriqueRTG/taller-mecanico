namespace Taller.Dominio.Entidades;
/// <summary>Avance recuperable de la evaluación y preparación de una propuesta técnica.</summary>
/// <remarks>El informe definitivo se conserva en Diagnostico al finalizar la evaluación.</remarks>
public sealed class EvaluacionAtencion
{
    public int IdAtencion { get; set; }
    public bool IncluyeDiagnostico { get; set; }
    public string FallaDetectada { get; set; } = string.Empty;
    public string TrabajoRecomendado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public int? Kilometraje { get; set; }
    public bool SoloEvaluacion { get; set; }
    public bool Finalizada { get; set; }
    public int IdTecnicoUltimaEdicion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public byte[] Version { get; set; } = [];
    public Atencion Atencion { get; set; } = null!;
    public Usuario TecnicoUltimaEdicion { get; set; } = null!;
}

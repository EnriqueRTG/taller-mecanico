namespace Taller.Aplicacion.DTOs.Diagnosticos;

/// <summary>
/// Transporta los datos necesarios para registrar
/// el diagnóstico inicial de una atención.
/// </summary>
/// <remarks>
/// El servicio de aplicación valida estos datos y determina
/// el técnico autor, la fecha y el tipo de diagnóstico.
/// </remarks>
public sealed class RegistrarDiagnosticoInicialDto
{
    public int IdAtencion { get; init; }

    public string FallaDetectada { get; init; } = string.Empty;

    public string TrabajoRecomendado { get; init; } = string.Empty;

    public int? Kilometraje { get; init; }

    public string? Observaciones { get; init; }
}
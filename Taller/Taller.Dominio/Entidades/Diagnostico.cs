using Taller.Dominio.Enumeraciones;

namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa un informe técnico completo de una atención.
/// </summary>
/// <remarks>
/// Los hallazgos posteriores se registran mediante HallazgoAtencion.
/// </remarks>
public sealed class Diagnostico
{
    public int IdDiagnostico { get; set; }
    public int IdAtencion { get; set; }

    /// <summary>
    /// Técnico que elaboró el informe.
    /// Su autoría se conserva aunque otro técnico
    /// prepare el presupuesto o ejecute los trabajos.
    /// </summary>
    public int IdTecnicoAutor { get; set; }
    public DateTime FechaDiagnostico { get; set; }
    public string FallaDetectada { get; set; } = null!;
    public string TrabajoRecomendado { get; set; } = null!;
    public int? Kilometraje { get; set; }
    public string? Observaciones { get; set; }


    public Atencion Atencion { get; set; } = null!;

    public Usuario TecnicoAutor { get; set; } = null!;
}

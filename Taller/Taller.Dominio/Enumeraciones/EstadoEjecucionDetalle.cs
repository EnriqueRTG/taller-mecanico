namespace Taller.Dominio.Enumeraciones;

/// <summary>Avance del ítem autorizado. Una cantidad parcial permanece EnCurso; no es un cierre.</summary>
public enum EstadoEjecucionDetalle
{
    NoIniciado = 1,
    EnCurso = 2,
    // Conserva su identificador: no reutilizar los valores de estados retirados.
    Completado = 4
}

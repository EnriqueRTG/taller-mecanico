using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.Consultas;
/// <summary>Distingue el circuito vigente de los valores persistidos del circuito anterior, sin reinterpretar historiales.</summary>
public static class EstadosAtencionConsulta
{
    public static bool EsCompatibilidad(EstadoAtencion estado) => estado is EstadoAtencion.Abierta or EstadoAtencion.PendienteDiagnostico or EstadoAtencion.Pausada;
    public static IReadOnlyList<EstadoAtencion> Listar(IEnumerable<EstadoAtencion> existentes)
    {
        var presentes = existentes.ToHashSet();
        return Enum.GetValues<EstadoAtencion>().Where(e => !EsCompatibilidad(e) || presentes.Contains(e)).ToList();
    }
}

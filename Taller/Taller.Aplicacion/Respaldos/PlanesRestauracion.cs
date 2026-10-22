using System.Collections.Concurrent;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Excepciones;
namespace Taller.Aplicacion.Respaldos;
/// <summary>Planes de corta duración, de un solo uso. El formulario nunca decide qué rutas o datos se restauran.</summary>
public sealed class PlanesRestauracion(TimeProvider reloj)
{
    private readonly ConcurrentDictionary<Guid, PlanRestauracionDto> _planes = new();
    public PlanRestauracionDto Agregar(PlanRestauracionDto plan)
    {
        foreach(var viejo in _planes.Where(x=>x.Value.VenceUtc<=reloj.GetUtcNow())) _planes.TryRemove(viejo.Key,out _);
        var emitido=plan with { Token=Guid.NewGuid(), VenceUtc=reloj.GetUtcNow().AddMinutes(10) };
        _planes[emitido.Token]=emitido;return emitido;
    }
    public PlanRestauracionDto Obtener(Guid token,int usuario)
    {
        if(!_planes.TryGetValue(token,out var p)||p.IdSolicitante!=usuario||p.VenceUtc<=reloj.GetUtcNow()) throw new ValidacionException("El plan no pertenece a esta sesión o venció. Valide nuevamente la copia.");
        return p;
    }
    public PlanRestauracionDto Consumir(Guid token,int usuario)
    {
        var p=Obtener(token,usuario);
        if(!_planes.TryRemove(token,out _))throw new ValidacionException("El plan ya se utilizó.");return p;
    }
}

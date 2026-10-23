using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Inicio;
using Taller.Aplicacion.Seguridad;
using Taller.Dominio.Constantes;

namespace Taller.Aplicacion.Servicios;
public sealed class InicioServicio(IInicioRepositorio repositorio, AutorizacionOperativa acceso)
{
    public async Task<List<PendienteInicioDto>> ListarPendientesAsync()
    {
        var u = await acceso.ExigirRolesAsync(RolesSistema.AdministradorId, RolesSistema.AdministrativoId, RolesSistema.TecnicoId);
        return await repositorio.ListarPendientesAsync(u.RolId, u.Id);
    }
}

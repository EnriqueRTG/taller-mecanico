using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Inicio;
using Taller.Aplicacion.Seguridad;
using Taller.Dominio.Constantes;

namespace Taller.Aplicacion.Servicios;
public sealed class InicioServicio(IInicioRepositorio repositorio, AutorizacionOperativa acceso, ReporteServicio reportes)
{
    /// <summary>Autoriza una vez y prepara indicadores y pendientes para la misma carga de Inicio.</summary>
    public async Task<(List<Taller.Aplicacion.DTOs.Reportes.ReporteDocumentoDto> Tarjetas, List<PendienteInicioDto> Pendientes)> CargarAsync()
    {
        var u = await acceso.ExigirRolesAsync(RolesSistema.AdministradorId, RolesSistema.AdministrativoId, RolesSistema.TecnicoId);
        var tarjetas = await reportes.ResumirInicioAsync(u);
        var pendientes = await repositorio.ListarPendientesAsync(u.RolId, u.Id);
        return (tarjetas, pendientes);
    }

    public async Task<List<PendienteInicioDto>> ListarPendientesAsync()
    {
        var u = await acceso.ExigirRolesAsync(RolesSistema.AdministradorId, RolesSistema.AdministrativoId, RolesSistema.TecnicoId);
        return await repositorio.ListarPendientesAsync(u.RolId, u.Id);
    }
}

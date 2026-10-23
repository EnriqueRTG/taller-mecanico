using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Seguridad;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Dominio.Constantes;

namespace Taller.Aplicacion.Servicios;
public sealed class AuditoriaAccesoServicio(IAuditoriaAccesoRepositorio repositorio, AutorizacionOperativa acceso)
{
    public async Task<List<AccesoAuditoriaDto>> ConsultarAsync(DateTime desde, DateTime hasta, bool? correcto)
    {
        await acceso.ExigirRolesAsync(RolesSistema.AdministradorId);
        if (desde.Date > hasta.Date || hasta.Date > DateTime.Today || (hasta.Date - desde.Date).TotalDays > 366)
            throw new ValidacionException("Seleccione hasta un año de fechas, sin fechas futuras.");
        return await repositorio.ConsultarAsync(new DateTimeOffset(desde.Date), new DateTimeOffset(hasta.Date.AddDays(1)), correcto);
    }
}

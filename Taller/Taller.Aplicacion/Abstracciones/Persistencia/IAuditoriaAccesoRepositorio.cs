using Taller.Aplicacion.DTOs.Seguridad;

namespace Taller.Aplicacion.Abstracciones.Persistencia;
public interface IAuditoriaAccesoRepositorio
{
    Task<List<AccesoAuditoriaDto>> ConsultarAsync(DateTimeOffset desde, DateTimeOffset hasta, bool? correcto);
}

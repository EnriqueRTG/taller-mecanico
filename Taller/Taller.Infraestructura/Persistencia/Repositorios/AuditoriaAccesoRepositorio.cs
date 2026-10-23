using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Seguridad;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;
public sealed class AuditoriaAccesoRepositorio(TallerDbContext db) : IAuditoriaAccesoRepositorio
{
    public Task<List<AccesoAuditoriaDto>> ConsultarAsync(DateTimeOffset desde, DateTimeOffset hasta, bool? correcto) => db.Set<EventoAcceso>().AsNoTracking().Where(e => e.FechaUtc >= desde && e.FechaUtc < hasta && (!correcto.HasValue || e.Correcto == correcto)).OrderByDescending(e => e.Id).Take(500).Select(e => new AccesoAuditoriaDto(e.Id, e.FechaUtc, db.Usuarios.Where(u => u.Id == e.IdUsuario).Select(u => u.NombreUsuario).FirstOrDefault() ?? "Identidad no reconocida", e.Correcto)).ToListAsync();
}

using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Inicio;
using Taller.Dominio.Constantes;
using Taller.Dominio.Enumeraciones;

namespace Taller.Infraestructura.Persistencia.Repositorios;
public sealed class InicioRepositorio(TallerDbContext db) : IInicioRepositorio
{
    public async Task<List<PendienteInicioDto>> ListarPendientesAsync(int rol, int usuario)
    {
        if (rol == RolesSistema.AdministradorId)
        {
            var desde = DateTimeOffset.UtcNow.AddDays(-7);
            var fallos = await db.Set<Taller.Dominio.Entidades.EventoAcceso>().AsNoTracking().Where(x => !x.Correcto && x.FechaUtc >= desde).OrderByDescending(x => x.FechaUtc).Take(20).ToListAsync();
            return fallos.Select(x => new PendienteInicioDto("ACC-" + x.Id, Taller.Dominio.Tiempo.FechaHoraTaller.DesdeUtc(x.FechaUtc).DateTime, "Revisar acceso rechazado · " + (x.IdUsuario.HasValue ? "Usuario " + x.IdUsuario : "Identidad no reconocida"), "Consultar auditoría de accesos", 1)).ToList();
        }

        var q = db.Atenciones.AsNoTracking().Where(a => a.Estado != EstadoAtencion.Cerrada && a.Estado != EstadoAtencion.Cancelada);
        if (rol == RolesSistema.TecnicoId)
            q = q.Where(a => a.Estado == EstadoAtencion.EnEvaluacionTecnica || a.Estado == EstadoAtencion.EnEjecucion || db.Presupuestos.Any(p => p.IdAtencion == a.IdAtencion && p.Vigente && p.Estado == EstadoPresupuesto.PendienteRevisionTecnica));
        var datos = await q.Select(a => new { a.IdAtencion, a.Estado, a.IdTecnicoEnActividad, Fecha = db.HistorialAtenciones.Where(h => h.IdAtencion == a.IdAtencion).Select(h => (DateTime? )h.Fecha).Max() ?? a.FechaApertura, Descripcion = a.Vehiculo.Dominio + " · " + a.Cliente.Apellido + ", " + a.Cliente.Nombre + " · " + a.MotivoConsulta, Revision = db.Presupuestos.Any(p => p.IdAtencion == a.IdAtencion && p.Vigente && p.Estado == EstadoPresupuesto.PendienteRevisionTecnica), Presentar = db.Presupuestos.Any(p => p.IdAtencion == a.IdAtencion && p.Vigente && p.Estado == EstadoPresupuesto.PendientePresentacion), Pausa = a.Pausas.Any(p => p.FechaResolucion == null) }).ToListAsync();
        return datos.Select(a =>
        {
            var propio = a.IdTecnicoEnActividad == usuario;
            var tecnico = rol == RolesSistema.TecnicoId;
            string accion = tecnico ? a.Revision ? "Revisar selección parcial" : a.Pausa ? "Revisar pausa y causa" : propio ? "Continuar trabajo propio" : a.IdTecnicoEnActividad == null ? "Disponible para tomar" : "Asignada a otro técnico" : a.Presentar ? "Comunicar presupuesto al cliente" : a.Estado == EstadoAtencion.PendienteDecision ? "Registrar decisión del cliente" : a.Estado is EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion ? "Gestionar liquidación, saldo y entrega" : a.Estado == EstadoAtencion.EnEvaluacionTecnica ? "Esperar preparación técnica" : "Consultar seguimiento técnico";
            int prioridad = tecnico ? a.Revision ? 0 : propio ? 1 : a.IdTecnicoEnActividad == null ? 2 : 9 : a.Presentar ? 0 : a.Estado == EstadoAtencion.PendienteDecision ? 1 : a.Estado is EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion ? 2 : a.Estado == EstadoAtencion.EnEvaluacionTecnica ? 3 : 9;
            return new PendienteInicioDto("A-" + a.IdAtencion, a.Fecha, a.Descripcion, accion, prioridad);
        }).OrderBy(x => x.Prioridad).ThenByDescending(x => x.Fecha).ThenBy(x => x.Codigo).Take(30).ToList();
    }
}

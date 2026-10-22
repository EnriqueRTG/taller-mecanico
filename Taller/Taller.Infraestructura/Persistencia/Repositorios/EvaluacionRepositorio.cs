using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Infraestructura.Persistencia.Repositorios;
/// <summary>Carga entidades con seguimiento para operaciones coordinadas por la unidad de trabajo.</summary>
public sealed class EvaluacionRepositorio(TallerDbContext contexto) : IEvaluacionRepositorio
{
    public Task<Atencion?> ObtenerAtencionAsync(int id) => contexto.Atenciones.Include(a => a.Hallazgos).Include(a => a.Pausas).AsSplitQuery().Include(a => a.Cliente).Include(a => a.Vehiculo).ThenInclude(v => v.Modelo).ThenInclude(m => m.Marca).SingleOrDefaultAsync(a => a.IdAtencion == id);
    public Task<EvaluacionAtencion?> ObtenerAvanceAsync(int id) => contexto.Set<EvaluacionAtencion>().SingleOrDefaultAsync(e => e.IdAtencion == id);
    public Task<List<Presupuesto>> ObtenerPresupuestosAsync(int id) => contexto.Presupuestos.Include(p => p.Detalles).Where(p => p.IdAtencion == id).ToListAsync();
    public Task<List<Diagnostico>> ObtenerDiagnosticosAsync(int id) => contexto.Diagnosticos.Where(d => d.IdAtencion == id).ToListAsync();
    public Task<List<Concepto>> ListarCatalogoAsync() => contexto.Conceptos.AsNoTracking().Include(c => c.Categoria).Where(c => c.Activo && c.Categoria.Activo).OrderBy(c => c.Nombre).ToListAsync();
    public Task<List<EvaluacionBandejaDto>> ListarAsync() => contexto.Atenciones.AsNoTracking().Where(a => a.Estado != EstadoAtencion.Cerrada && a.Estado != EstadoAtencion.Cancelada && (a.Estado == EstadoAtencion.PendienteDiagnostico || a.Estado == EstadoAtencion.EnEvaluacionTecnica || contexto.Set<EvaluacionAtencion>().Any(e => e.IdAtencion == a.IdAtencion && e.Finalizada))).Select(a => new EvaluacionBandejaDto { IdAtencion = a.IdAtencion, Cliente = a.Cliente.Apellido + ", " + a.Cliente.Nombre, Vehiculo = a.Vehiculo.Dominio + " · " + a.Vehiculo.Modelo.Nombre, Motivo = a.MotivoConsulta, TipoSolicitud = a.TipoSolicitud, Fecha = a.Historial.Select(h => (DateTime? )h.Fecha).Max() ?? a.FechaApertura, IdTecnico = a.IdTecnicoEnActividad, Tecnico = a.TecnicoEnActividad == null ? "Disponible" : a.TecnicoEnActividad.Apellido + ", " + a.TecnicoEnActividad.Nombre, Finalizada = contexto.Set<EvaluacionAtencion>().Any(e => e.IdAtencion == a.IdAtencion && e.Finalizada), IdDiagnostico = a.Diagnosticos.Where(d => d.Tipo == TipoDiagnostico.Inicial).Select(d => (int? )d.IdDiagnostico).FirstOrDefault() }).ToListAsync();
    public async Task<List<PresupuestoConsultaDto>> ListarPresupuestosAsync()
    {
        var datos = await contexto.Presupuestos.AsNoTracking().Include(p => p.Atencion).ThenInclude(a => a.Cliente).Include(p => p.Atencion).ThenInclude(a => a.Vehiculo).Include(p => p.Detalles).ToListAsync();
        return datos.Select(p => new PresupuestoConsultaDto { EsAdicional = p.EsAdicional, IdPresupuesto = p.IdPresupuesto, IdAtencion = p.IdAtencion, Cliente = p.Atencion.Cliente.Apellido + ", " + p.Atencion.Cliente.Nombre, Vehiculo = p.Atencion.Vehiculo.Dominio, Fecha = p.FechaDecision ?? p.FechaPresentacion ?? p.FechaCreacion, Tipo = p.Tipo, Estado = p.Estado, Vigente = p.Vigente, IdPresupuestoAnterior = p.IdPresupuestoAnterior, TotalAceptado = p.Estado != EstadoPresupuesto.Aceptado ? 0 : p.Detalles.Where(d => d.AceptadoPorCliente == true).Sum(d => decimal.Round(d.Cantidad * d.PrecioUnitario, 2, MidpointRounding.AwayFromZero)), Version = p.Version, Observaciones = p.Observaciones, Items = p.Detalles.Select(d => new ItemPropuestaDto { OrigenMaterial = d.OrigenMaterial, IdDetalle = d.IdPresupuestoDetalle, AceptadoPorCliente = d.AceptadoPorCliente, IdConcepto = d.IdConcepto, Descripcion = d.Descripcion, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario }).ToList() }).ToList();
    }

    public void PrepararVersion(Atencion a, byte[] version)
    {
        var entrada = contexto.Entry(a);
        entrada.Property(x => x.Version).OriginalValue = version.ToArray();
        entrada.Property(x => x.Estado).IsModified = true;
    }

    public void EliminarBorrador(Presupuesto p)
    {
        contexto.PresupuestoDetalles.RemoveRange(p.Detalles);
        contexto.Presupuestos.Remove(p);
    }

    public void AgregarAvance(EvaluacionAtencion e) => contexto.Add(e);
    public void AgregarPresupuesto(Presupuesto p) => contexto.Add(p);
    public void AgregarDiagnostico(Diagnostico d) => contexto.Add(d);
    public void AgregarHistorial(HistorialAtencion h) => contexto.Add(h);
    public void ReemplazarDetalles(Presupuesto p, IEnumerable<PresupuestoDetalle> detalles)
    {
        contexto.PresupuestoDetalles.RemoveRange(p.Detalles);
        p.Detalles = detalles.ToList();
    }
}

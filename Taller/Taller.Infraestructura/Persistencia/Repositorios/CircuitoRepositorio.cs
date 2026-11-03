using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
namespace Taller.Infraestructura.Persistencia.Repositorios;
/// <summary>Implementa el acceso al expediente económico y su seguimiento de pagos.</summary>
public sealed class CircuitoRepositorio(TallerDbContext contexto) : ICircuitoRepositorio
{
    /// <summary>Una sola consulta mantiene coherentes fecha, total y saldo sin cargar pagos completos.</summary>
    public Task<List<Taller.Aplicacion.DTOs.Cobros.AtencionCobroDto>> ListarResumenCobrosAsync() => contexto.Atenciones.AsNoTracking()
        .Where(a => a.Estado == EstadoAtencion.TrabajoFinalizado || a.Estado == EstadoAtencion.PendienteLiquidacion || a.Estado == EstadoAtencion.Cerrada)
        .Select(a => new { Atencion = a, Documento = contexto.Comprobantes.Where(c => c.IdAtencion == a.IdAtencion).FirstOrDefault() })
        .Select(x => new Taller.Aplicacion.DTOs.Cobros.AtencionCobroDto {
            IdAtencion = x.Atencion.IdAtencion, Cliente = x.Atencion.Cliente.Apellido + ", " + x.Atencion.Cliente.Nombre,
            Dominio = x.Atencion.Vehiculo.Dominio, Estado = x.Atencion.Estado,
            IdComprobante = x.Documento == null ? null : x.Documento.IdComprobante,
            Total = x.Documento == null ? null : x.Documento.Total, Pagado = x.Documento == null ? null : x.Documento.TotalPagado,
            Fecha = x.Documento == null ? x.Atencion.FechaApertura
                : (x.Documento.Pagos.Select(p => (DateTime?)(p.FechaAnulacion ?? p.Fecha)).Max() ?? x.Documento.FechaEmision)
                    > x.Documento.FechaEmision
                    ? x.Documento.Pagos.Select(p => (DateTime?)(p.FechaAnulacion ?? p.Fecha)).Max()!.Value : x.Documento.FechaEmision
        }).ToListAsync();

    public Task<bool> ExisteLiquidacionAsync(int idAtencion) => contexto.Comprobantes.AnyAsync(c => c.IdAtencion == idAtencion);
    public Task<Presupuesto?> ObtenerPresupuestoAsync(int id) => contexto.Presupuestos.Include(x => x.Detalles)
        .Include(x => x.Atencion).ThenInclude(x => x.Solicitudes).Include(x => x.Atencion).ThenInclude(x => x.Cliente).Include(x => x.Atencion).ThenInclude(x => x.Vehiculo)
        .SingleOrDefaultAsync(x => x.IdPresupuesto == id);
    private IQueryable<Comprobante> Documentos() => contexto.Set<Comprobante>()
        .Include(x => x.Detalles).Include(x => x.Pagos).ThenInclude(x => x.Medios).ThenInclude(x => x.MedioPago)
        .Include(x => x.Pagos).ThenInclude(x => x.UsuarioRegistro).Include(x => x.Atencion).AsSplitQuery();
    public Task<Comprobante?> ObtenerComprobanteAsync(int id) => Documentos().SingleOrDefaultAsync(x => x.IdComprobante == id);
    public Task<Comprobante?> ObtenerPorAtencionAsync(int idAtencion) => Documentos().SingleOrDefaultAsync(x => x.IdAtencion == idAtencion);
    public Task<Pago?> ObtenerPagoPorOperacionAsync(Guid id) => contexto.Set<Pago>().AsNoTracking().Include(x => x.Medios).SingleOrDefaultAsync(x => x.IdOperacion == id);
    public Task<List<MedioPago>> ListarMediosAsync() => contexto.Set<MedioPago>().AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre).ToListAsync();
    public Task<List<Atencion>> ListarAtencionesParaCobrosAsync() => contexto.Atenciones.AsNoTracking().Include(x => x.Cliente).Include(x => x.Vehiculo)
        .Where(x => x.Estado == EstadoAtencion.TrabajoFinalizado || x.Estado == EstadoAtencion.PendienteLiquidacion || x.Estado == EstadoAtencion.Cerrada).ToListAsync();
    public Task<List<Comprobante>> ListarComprobantesAsync() => Documentos().AsNoTracking().ToListAsync();
    public void AgregarComprobante(Comprobante d) => contexto.Add(d);
    public void AgregarPago(Pago p) => contexto.Add(p);
    public void PrepararVersion(Comprobante d, byte[] version)
    {
        contexto.Entry(d).Property(x => x.Version).OriginalValue = version.ToArray();
        contexto.Entry(d).Property(x => x.TotalPagado).IsModified = true;
    }
}

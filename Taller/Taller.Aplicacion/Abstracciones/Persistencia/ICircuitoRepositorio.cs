using Taller.Dominio.Entidades;
namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>Carga documentos con seguimiento para operaciones atómicas; no confirma cambios.</summary>
public interface ICircuitoRepositorio
{
    /// <summary>Precondición económica sin cargar el documento ni su historial de pagos.</summary>
    /// <summary>Resumen económico de bandeja, sin cargar detalles de comprobantes.</summary>
    Task<List<Taller.Aplicacion.DTOs.Cobros.AtencionCobroDto>> ListarResumenCobrosAsync();
    Task<bool> ExisteLiquidacionAsync(int idAtencion);
    Task<Presupuesto?> ObtenerPresupuestoAsync(int id);
    Task<Comprobante?> ObtenerComprobanteAsync(int id);
    Task<Comprobante?> ObtenerPorAtencionAsync(int idAtencion);
    Task<Pago?> ObtenerPagoPorOperacionAsync(Guid id);
    Task<List<MedioPago>> ListarMediosAsync();
    Task<List<Atencion>> ListarAtencionesParaCobrosAsync();
    Task<List<Comprobante>> ListarComprobantesAsync();
    void AgregarComprobante(Comprobante documento);
    void AgregarPago(Pago pago);
    void PrepararVersion(Comprobante documento, byte[] version);
}

using Taller.Dominio.Entidades;
namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>Carga documentos con seguimiento para operaciones atómicas; no confirma cambios.</summary>
public interface ICircuitoRepositorio
{
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

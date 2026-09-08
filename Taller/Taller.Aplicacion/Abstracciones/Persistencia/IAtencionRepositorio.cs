

using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para trabajar con las atenciones dentro de la aplicación.
/// </summary>
public interface IAtencionRepositorio
{
    Task<Atencion?> ObtenerPorIdAsync(int idAtencion);
    Task<List<Atencion>> ListarActivasAsync();
    Task<List<Atencion>> ListarPorClienteAsync(int idCliente);
    Task<List<Atencion>> ListarPorVehiculoAsync(int idVehiculo);
    Task AgregarAsync(Atencion atencion);
    Task ActualizarAsync(Atencion atencion);
    Task<bool> ExisteAtencionActivaParaVehiculoAsync(int idVehiculo);
}

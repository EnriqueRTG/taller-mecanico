using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para trabajar con vehículos dentro de la aplicación.
/// </summary>
public interface IVehiculoRepositorio 
{
    Task<Vehiculo?> ObtenerPorIdAsync(int idVehiculo);
    Task<Vehiculo?> ObtenerPorDominioAsync(string dominio);
    Task<List<Vehiculo>> ListarActivosAsync();
    Task<List<Vehiculo>> ListarInactivosAsync();
    Task AgregarAsync(Vehiculo vehiculo);
    Task ActualizarAsync(Vehiculo vehiculo);
    Task<bool> ExisteDominioAsync(string dominio, int? excluirIdVehiculo = null);
}

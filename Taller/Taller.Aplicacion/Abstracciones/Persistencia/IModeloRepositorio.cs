using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para consultar los modelos de vehículos.
/// </summary>
public interface IModeloRepositorio
{
    Task<Modelo?> ObtenerPorIdAsync(int idModelo);
    Task<List<Modelo>> ListarActivosPorMarcaAsync(int idMarca);
}

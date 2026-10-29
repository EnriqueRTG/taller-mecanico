using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>
/// Define las operaciones de persistencia necesarias
/// para consultar los modelos de vehículos.
/// </summary>
public interface IModeloRepositorio
{
    Task<List<Modelo>> ListarParaEdicionAsync(int actual);
    Task<Modelo?> ObtenerPorIdAsync(int idModelo);

}

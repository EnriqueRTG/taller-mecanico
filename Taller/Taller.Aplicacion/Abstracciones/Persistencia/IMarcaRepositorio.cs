using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para consultar las marcas de vehículos.
/// </summary>
public interface IMarcaRepositorio
{
    Task<Marca?> ObtenerPorIdAsync(int idMarca);
    Task<List<Marca>> ListarActivosAsync();
}

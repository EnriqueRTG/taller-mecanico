using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>
/// Define las operaciones de persistencia necesarias
/// para consultar los modelos de vehículos.
/// </summary>
public interface IModeloRepositorio
{
    /// <summary>Marcas activas que disponen de algún modelo activo para dar de alta un vehículo.</summary>
    Task<List<Marca>> ListarMarcasDisponiblesAsync();
    /// <summary>Modelos activos de una sola marca activa; el filtro se ejecuta en persistencia.</summary>
    Task<List<Modelo>> ListarDisponiblesPorMarcaAsync(int idMarca);
    Task<List<Modelo>> ListarParaEdicionAsync(int actual);
    Task<Modelo?> ObtenerPorIdAsync(int idModelo);

}

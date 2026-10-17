using Taller.Dominio.Entidades;
namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para consultar las categorías del sistema.
/// </summary>
public interface ICategoriaRepositorio
{    
    Task<Categoria?> ObtenerPorIdAsync(int IdCategoria);
    Task<List<Categoria>> ListarActivosAsync();
}
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

public interface ICategoriaRepositorio
{
    Task<Categoria?> ObtenerPorIdAsync(
        int id);

    Task<Categoria?> ObtenerPorNombreAsync(
        string nombre);


    Task<List<Categoria>> ListarAsync();

    Task<bool> ExisteNombreAsync(
        string nombre);
}

using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para consultar los roles del sistema.
/// </summary>
public interface IRolRepositorio
{
    Task<Rol?> ObtenerPorIdAsync(int idRol);

    Task<List<Rol>> ListarActivosAsync();
}
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para trabajar con usuarios dentro de la aplicación.
/// </summary>
public interface IUsuarioRepositorio
{
    Task<Usuario?>ObtenerPorIdAsync(int id);
    Task<Usuario?>ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<List<Usuario>> ListarAsync();
    Task AgregarAsync(Usuario usuario);
    Task ActualizarAsync(Usuario usuario);
    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);
}
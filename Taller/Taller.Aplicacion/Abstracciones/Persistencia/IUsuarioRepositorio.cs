using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para trabajar con usuarios dentro de la aplicación.
/// </summary>
public interface IUsuarioRepositorio
{
    Task<Usuario?> ObtenerPorIdAsync(
        int id);

    Task<Usuario?> ObtenerPorNombreUsuarioAsync(
        string nombreUsuario);

    Task<List<Usuario>> ListarAsync();

    Task<bool> ExisteNombreUsuarioAsync(
        string nombreUsuario);

    Task AgregarAsync(
        Usuario usuario);

    Task ActualizarDatosAsync(
        int id,
        string nombre,
        string apellido,
        int rolId);

    Task ActualizarCredencialesAsync(
        int id,
        string nombreUsuario,
        string? passwordHash);

    Task CambiarEstadoAsync(
        int id,
        bool activo);
}
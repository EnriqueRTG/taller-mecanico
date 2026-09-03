using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para trabajar con clientes dentro de la aplicación.
/// </summary>
public interface IClienteRepositorio
{
    Task<Cliente?> ObtenerPorIdAsync(int id);
    Task<Cliente?> ObtenerPorDocumentoAsync(string tipoDocumento, string documento);
    Task<List<Cliente>> ListarActivosAsync();
    Task<List<Cliente>> ListarInactivosAsync();
    Task AgregarAsync(Cliente cliente);
    Task ActualizarAsync(Cliente cliente);
    Task<bool> ExisteDocumentoAsync(string tipoDocumento, string documento, int? excluirIdCliente = null);
}

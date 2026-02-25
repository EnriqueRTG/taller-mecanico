using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Dominio.Entidades;

namespace Taller.Dominio.Repositorios;

public interface IClienteRepositorio
{
    Task<Cliente?> ObtenerPorIdAsync(long id);
    Task<IReadOnlyList<Cliente>> ListarAsync();

    Task AgregarAsync(Cliente cliente);
    Task ActualizarAsync(Cliente cliente);
    Task EliminarAsync(Cliente cliente); // o BajaLogica más adelante

    Task<bool> ExistePorDocumentoAsync(string documento, long? excluirId = null);
}
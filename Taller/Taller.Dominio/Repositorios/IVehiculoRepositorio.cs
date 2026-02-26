using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Dominio.Entidades;

namespace Taller.Dominio.Repositorios
{
    public interface IVehiculoRepositorio
    {
        Task<IReadOnlyList<Vehiculo>> ListarAsync();
        Task<IReadOnlyList<Vehiculo>> ListarPorClienteAsync(long clienteId);
        
        Task<Vehiculo?> ObtenerPorIdAsync(long id);

        Task AgregarAsync(Vehiculo vehiculo);
        Task ActualizarAsync(Vehiculo vehiculo);
        Task EliminarAsync(Vehiculo vehiculo);

        // Control de unidad de patente
        Task<bool> ExistePorPatenteAsync(string patente, long? excluirId = null);
    }
}

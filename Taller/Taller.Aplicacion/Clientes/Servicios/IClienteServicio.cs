using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Taller.Aplicacion.Clientes.Dtos;

namespace Taller.Aplicacion.Clientes.Servicios;

public interface IClienteServicio
{
    Task<IReadOnlyList<ClienteDto>> ListarAsync();
    Task<ClienteDto?> ObtenerPorIdAsync(long id);
    Task<long> CrearAsync(ClienteDto dto);
    Task ActualizarAsync(ClienteDto dto);
    Task EliminarAsync(long id);
}
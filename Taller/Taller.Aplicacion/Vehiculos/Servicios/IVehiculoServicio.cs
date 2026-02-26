using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Aplicacion.Vehiculos.Dtos;

namespace Taller.Aplicacion.Vehiculos.Servicios
{
    public interface IVehiculoServicio
    {
        Task<IReadOnlyList<VehiculoDto>> ListarAsync();
        Task<IReadOnlyList<VehiculoDto>> ListarPorClienteIdAsync(long clienteId);
        
        Task<VehiculoEdicionDto?> ObtenerParaEdiciondAsync(long id);

        Task<long> CrearAsync(VehiculoEdicionDto dto);
        Task ActualizarAsync(VehiculoEdicionDto dto);
        Task EliminarAsync(long id);
    }
}

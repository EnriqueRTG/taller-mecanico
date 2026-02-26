using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taller.Aplicacion.Vehiculos.Dtos
{
    public sealed class VehiculoDto
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string ClienteNombreCompleto { get; set; } = null!;
        public string Patente { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string? Modelo { get; set; }
        public int? Anio { get; set; }
        public string? Color { get; set; }
        public string? Vin { get; set; } 
        public string? Observaciones { get; set; }
        public bool Activo { get; set; }
    }
}

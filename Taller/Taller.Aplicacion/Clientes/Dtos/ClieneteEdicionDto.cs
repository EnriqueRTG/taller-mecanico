using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taller.Aplicacion.Clientes.Dtos
{
    public class ClienteEdicionDto
    {
        public long? Id { get; set; }   // null en alta, valor en edición

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string DocTipo { get; set; } = string.Empty;
        public string DocNro { get; set; } = string.Empty;

        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }

        public bool Activo { get; set; } = true;
    }
}

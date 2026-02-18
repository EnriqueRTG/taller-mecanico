using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taller.Aplicacion.Clientes.Dtos;

public class ClienteDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string DocTipo { get; set; } = null!;
    public string DocNro { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public bool Activo { get; set; }
}
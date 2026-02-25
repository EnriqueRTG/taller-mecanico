using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taller.Aplicacion.Clientes.Dtos;

public class ClienteDto
{
    public long Id { get; set; }

    // Para la UI, nunca null: si falta algo se muestra vacío
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DocTipo { get; set; } = string.Empty;
    public string DocNro { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;

    public bool Activo { get; set; }
}
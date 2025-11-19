using System.ComponentModel.DataAnnotations.Schema;

namespace Taller.Domain.Entities
{
    /// <summary>
    /// Cliente del taller mecánico.
    /// </summary>
    public sealed class Cliente
    {
        public long Id { get; init; }

        /// <summary>Nombre de pila del cliente.</summary>
        public string Nombre { get; set; } = null!;

        /// <summary>Apellido del cliente.</summary>
        public string Apellido { get; set; } = null!;

        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? DocTipo { get; set; }
        public string? DocNro { get; set; }

        /// <summary>Dirección postal del cliente (libre).</summary>
        public string? Direccion { get; set; }

        /// <summary>Baja lógica. True = activo, False = dado de baja.</summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Nombre completo calculado, sólo de lectura.
        /// No se mapea a la base de datos.
        /// </summary>
        [NotMapped]
        public string NombreCompleto =>
            string.IsNullOrWhiteSpace(Apellido)
                ? Nombre
                : $"{Apellido}, {Nombre}";
    }
}

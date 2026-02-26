using System.ComponentModel.DataAnnotations.Schema;

namespace Taller.Dominio.Entidades
{
    /// <summary>
    /// Cliente del taller mecánico.
    /// </summary>
    public sealed class Cliente
    {
        public long Id { get; set; }

        // Campos obligatorios (NO nullables)
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string DocTipo { get; set; } = null!;
        public string DocNro { get; set; } = null!;

        // Campos opcionales (pueden ser null)
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }

        // Relacion con Vehículos (un cliente puede tener varios vehículos)
        public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();

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

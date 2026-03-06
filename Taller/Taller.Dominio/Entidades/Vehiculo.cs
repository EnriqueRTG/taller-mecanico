namespace Taller.Dominio.Entidades
{
    /// <summary>
    /// Vehículo perteneciente a un cliente. Patente debe ser única.
    /// </summary>
    public sealed class Vehiculo
    {
        public long Id { get; set; }

        // Relacion con Cliente (dueño del vehículo)
        public long ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public string Patente { get; set; } = null!; // Obligatorio y único
        public string Marca { get; set; } = null!; // Obligatorio
        public string? Modelo { get; set; }
        public int? Anio { get; set; }
        public string? Color { get; set; }
        public string? Vin { get; set; } // Numero de chasis, opcional
        public string? Observaciones { get; set; }

        public bool? Activo { get; set; } = true;

    }
}

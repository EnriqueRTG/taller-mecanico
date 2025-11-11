namespace Taller.Domain.Entities
{
    /// <summary>
    /// Vehículo perteneciente a un cliente. Patente debe ser única.
    /// </summary>
    public sealed class Vehiculo
    {
        public long Id { get; init; }
        public long ClienteId { get; set; }
        public string Patente { get; set; } = null!;
        public string? Vin { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int? Anio { get; set; }
    }
}

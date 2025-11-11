using Taller.Domain.Enums;

namespace Taller.Domain.Entities
{
    /// <summary>
    /// Orden de Trabajo (OT): asocia Cliente y Vehículo, con diagnóstico y estado.
    /// </summary>
    public sealed class Orden
    {
        public long Id { get; init; }
        public string Nro { get; set; } = null!;
        public long ClienteId { get; set; }
        public long VehiculoId { get; set; }
        public long? TecnicoId { get; set; }
        public EstadoOT Estado { get; set; } = EstadoOT.Abierta;
        public string? Diagnostico { get; set; }
        public string? Observaciones { get; set; }
        public DateTime AperturaEn { get; set; } = DateTime.UtcNow;
        public DateTime? CierreEn { get; set; }
    }
}

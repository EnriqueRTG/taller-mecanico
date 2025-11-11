namespace Taller.Domain.Entities
{
    /// <summary>
    /// Servicio de mano de obra tarifado por hora.
    /// </summary>
    public sealed class Servicio
    {
        public long Id { get; init; }
        public string Codigo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal TarifaHora { get; set; }
        public bool Activo { get; set; } = true;
    }
}

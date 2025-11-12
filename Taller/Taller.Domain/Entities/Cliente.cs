namespace Taller.Domain.Entities
{
    /// <summary>
    /// Cliente del taller. Datos de contacto e identificación.
    /// </summary>
    public sealed class Cliente
    {
        public long Id { get; init; }
        public string Nombre { get; set; } = null!;
        public string? DocTipo { get; set; }
        public string? DocNro { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
    }
}

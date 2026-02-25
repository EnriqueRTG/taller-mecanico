namespace Taller.Dominio.Entidades
{
    /// <summary>
    /// Repuesto en inventario (control de stock y precios).
    /// </summary>
    public sealed class Repuesto
    {
        public long Id { get; init; }
        public string Codigo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; } = true;
    }
}

    using Taller.Dominio.Enumeraciones;

    namespace Taller.Dominio.Entidades;

    /// <summary>
    /// Representa un servicio, repuesto o consumible del catálogo.
    /// Sus datos actuales no modifican los valores históricos
    /// guardados en presupuestos y comprobantes.
    /// </summary>
    public sealed class Concepto
    {
        public int IdConcepto { get; set; }
        public int IdCategoria { get; set; }
        public TipoConcepto Tipo { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal? PrecioReferencia { get; set; }
        public bool Activo { get; set; } = true;
        /// <summary>
        /// Indica si el servicio puede incluirse entre las prestaciones
        /// autorizadas por el cliente al ingresar al circuito técnico.
        /// </summary>
        /// <remarks>
        /// Solo puede habilitarse para conceptos de tipo Servicio.
        /// No representa que esté autorizado, ejecutado ni cobrado.
        /// </remarks>
        public bool DisponibleParaIngreso { get; set; }

        // Relacion con Categoria
        public Categoria Categoria { get; set; } = null!;
    }
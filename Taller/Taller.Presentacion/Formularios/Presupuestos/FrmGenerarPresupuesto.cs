using System.Data;

namespace Taller.Presentacion.Formularios.Presupuestos;

public partial class FrmGenerarPresupuesto : Form
{
    private readonly List<ConceptoCatalogo> _catalogo = [];
    private readonly List<ConceptoPresupuesto> _detalle = [];

    private ConceptoCatalogo? _conceptoSeleccionado;
    private ConceptoPresupuesto? _conceptoEnEdicion;

    private bool _presupuestoGenerado;
    private bool _inicializandoFormulario;

    public decimal TotalPresupuesto =>
    _detalle.Sum(concepto => concepto.Subtotal);

    public DateTime FechaEmision =>
        dtpFechaEmision.Value;

    public int VigenciaDias =>
        decimal.ToInt32(nudVigenciaDias.Value);

    public string Observaciones =>
        txtObservaciones.Text.Trim();

    /// <summary>
    /// Representa un concepto incluido en el presupuesto generado.
    /// </summary>
    public sealed record ConceptoPresupuestoResultado(
        string Codigo,
        string Tipo,
        string Nombre,
        string Categoria,
        decimal Cantidad,
        decimal PrecioUnitario,
        decimal Subtotal);

    /// <summary>
    /// Obtiene una copia de los conceptos incluidos
    /// en el presupuesto simulado.
    /// </summary>
    public IReadOnlyList<ConceptoPresupuestoResultado> Conceptos =>
        _detalle
            .Select(concepto =>
                new ConceptoPresupuestoResultado(
                    concepto.Codigo,
                    concepto.Tipo,
                    concepto.Nombre,
                    concepto.Categoria,
                    concepto.Cantidad,
                    concepto.PrecioUnitario,
                    concepto.Subtotal))
            .ToList();


    public FrmGenerarPresupuesto()
    {
        InitializeComponent();
    }

    private void FrmGenerarPresupuesto_Load(
    object? sender,
    EventArgs e)
    {
        _inicializandoFormulario = true;

        try
        {
            ConfigurarFormulario();
            AplicarEstilos();
            CargarFiltros();
            CargarCatalogoSimulado();
            CargarDatosAtencionSimulados();
            AplicarFiltrosCatalogo();
            ActualizarDetallePresupuesto();
        }
        finally
        {
            _inicializandoFormulario = false;
        }
    }

    private void ConfigurarFormulario()
    {
        Text = "Generar presupuesto";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        CancelButton = btnCancelar;

        dtpFechaEmision.Format =
            DateTimePickerFormat.Custom;

        dtpFechaEmision.CustomFormat =
            "dd/MM/yyyy HH:mm";

        dtpFechaEmision.Value = DateTime.Now;

        cboTipoConcepto.DropDownStyle =
            ComboBoxStyle.DropDownList;

        cboCategoriaConcepto.DropDownStyle =
            ComboBoxStyle.DropDownList;

        nudCantidad.Minimum = 1;
        nudCantidad.Maximum = 100;
        nudCantidad.Value = 1;

        nudPrecioUnitario.DecimalPlaces = 2;
        nudPrecioUnitario.ThousandsSeparator = true;
        nudPrecioUnitario.Maximum = 999999999;

        nudVigenciaDias.Minimum = 1;
        nudVigenciaDias.Maximum = 90;
        nudVigenciaDias.Value = 15;

        txtObservaciones.MaxLength = 500;

        RestablecerConceptoSeleccionado();
    }

    private void CargarFiltros()
    {
        cboTipoConcepto.Items.Clear();
        cboTipoConcepto.Items.AddRange(
        [
            "Todos",
        "Servicio",
        "Repuesto",
        "Insumo"
        ]);

        cboCategoriaConcepto.Items.Clear();
        cboCategoriaConcepto.Items.AddRange(
        [
            "Todas",
        "Motor",
        "Electricidad",
        "Frenos",
        "Transmisión",
        "Lubricación",
        "Mano de obra"
        ]);

        cboTipoConcepto.SelectedIndex = 0;
        cboCategoriaConcepto.SelectedIndex = 0;
    }

    private void CargarCatalogoSimulado()
    {
        _catalogo.Clear();

        _catalogo.AddRange(
        [
            new(
            "SER-001",
            "Servicio",
            "Diagnóstico eléctrico",
            "Electricidad",
            "Servicio",
            15000m),

        new(
            "SER-002",
            "Servicio",
            "Reemplazo de motor de arranque",
            "Mano de obra",
            "Hora",
            22000m),

        new(
            "SER-003",
            "Servicio",
            "Limpieza del sistema de frenos",
            "Frenos",
            "Servicio",
            18000m),

        new(
            "REP-001",
            "Repuesto",
            "Motor de arranque",
            "Electricidad",
            "Unidad",
            85000m),

        new(
            "REP-002",
            "Repuesto",
            "Juego de escobillas",
            "Electricidad",
            "Juego",
            18500m),

        new(
            "REP-003",
            "Repuesto",
            "Pastillas de freno delanteras",
            "Frenos",
            "Juego",
            32000m),

        new(
            "INS-001",
            "Insumo",
            "Limpiador de contactos",
            "Electricidad",
            "Unidad",
            6500m),

        new(
            "INS-002",
            "Insumo",
            "Grasa dieléctrica",
            "Lubricación",
            "Unidad",
            4200m),

        new(
            "INS-003",
            "Insumo",
            "Aceite lubricante",
            "Lubricación",
            "Litro",
            12500m)
        ]);
    }

    private void CargarDatosAtencionSimulados()
    {
        lblClienteValor.Text = "Juan Santos";
        lblVehiculoValor.Text = "Citroën Jumper";
        lblDominioValor.Text = "AB123CD";
        lblDiagnosticoValor.Text =
            "Reparación requerida en el sistema de arranque.";
    }

    private void FiltroCatalogo_Cambio(
    object? sender,
    EventArgs e)
    {
        if (_inicializandoFormulario)
        {
            return;
        }

        AplicarFiltrosCatalogo();
    }

    private void BtnBuscarConceptos_Click(
        object? sender,
        EventArgs e)
    {
        AplicarFiltrosCatalogo();
    }

    private void BtnLimpiarFiltros_Click(
        object? sender,
        EventArgs e)
    {
        _inicializandoFormulario = true;

        try
        {
            txtBuscarConcepto.Clear();
            cboTipoConcepto.SelectedIndex = 0;
            cboCategoriaConcepto.SelectedIndex = 0;
        }
        finally
        {
            _inicializandoFormulario = false;
        }

        AplicarFiltrosCatalogo();
        txtBuscarConcepto.Focus();
    }

    private void AplicarFiltrosCatalogo()
    {
        string busqueda =
            txtBuscarConcepto.Text.Trim();

        string tipo =
            cboTipoConcepto.SelectedItem?.ToString()
            ?? "Todos";

        string categoria =
            cboCategoriaConcepto.SelectedItem?.ToString()
            ?? "Todas";

        IEnumerable<ConceptoCatalogo> resultados =
            _catalogo;

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            resultados = resultados.Where(concepto =>
                concepto.Codigo.Contains(
                    busqueda,
                    StringComparison.OrdinalIgnoreCase)
                || concepto.Nombre.Contains(
                    busqueda,
                    StringComparison.OrdinalIgnoreCase)
                || concepto.Categoria.Contains(
                    busqueda,
                    StringComparison.OrdinalIgnoreCase)
                || concepto.Tipo.Contains(
                    busqueda,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (tipo != "Todos")
        {
            resultados = resultados.Where(
                concepto => concepto.Tipo == tipo);
        }

        if (categoria != "Todas")
        {
            resultados = resultados.Where(
                concepto =>
                    concepto.Categoria == categoria);
        }

        MostrarResultadosCatalogo(resultados);
    }

    private void MostrarResultadosCatalogo(
    IEnumerable<ConceptoCatalogo> resultados)
    {
        dgvCatalogoConceptos.Rows.Clear();

        foreach (ConceptoCatalogo concepto in resultados)
        {
            int indice = dgvCatalogoConceptos.Rows.Add(
                concepto.Codigo,
                concepto.Tipo,
                concepto.Nombre,
                concepto.Categoria,
                concepto.Unidad,
                FormatearImporte(
                    concepto.PrecioReferencia));

            dgvCatalogoConceptos.Rows[indice].Tag =
                concepto;
        }

        dgvCatalogoConceptos.ClearSelection();
        RestablecerConceptoSeleccionado();
    }

    private void DgvCatalogoConceptos_SelectionChanged(
    object? sender,
    EventArgs e)
    {
        if (dgvCatalogoConceptos.CurrentRow?.Tag
            is not ConceptoCatalogo concepto)
        {
            return;
        }

        SeleccionarConcepto(concepto);
    }

    private void SeleccionarConcepto(
        ConceptoCatalogo concepto)
    {
        _conceptoSeleccionado = concepto;
        _conceptoEnEdicion = null;

        lblConceptoSeleccionado.Text =
            $"{concepto.Codigo} - {concepto.Nombre}";

        nudCantidad.Value = 1;
        nudPrecioUnitario.Value =
            concepto.PrecioReferencia;

        btnAgregarConcepto.Text = "Agregar";
        btnAgregarConcepto.Enabled = true;
    }

    private void BtnAgregarConcepto_Click(
    object? sender,
    EventArgs e)
    {
        AgregarOActualizarConcepto();
    }

    private void AgregarOActualizarConcepto()
    {
        if (_conceptoSeleccionado is null)
        {
            MessageBox.Show(
                this,
                "Seleccioná un concepto del catálogo.",
                "Concepto no seleccionado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        if (nudPrecioUnitario.Value <= 0)
        {
            MessageBox.Show(
                this,
                "El precio unitario debe ser mayor que cero.",
                "Precio inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            nudPrecioUnitario.Focus();
            return;
        }

        if (_conceptoEnEdicion is not null)
        {
            _conceptoEnEdicion.Cantidad =
                nudCantidad.Value;

            _conceptoEnEdicion.PrecioUnitario =
                nudPrecioUnitario.Value;
        }
        else
        {
            ConceptoPresupuesto? existente =
                _detalle.FirstOrDefault(concepto =>
                    concepto.Codigo ==
                    _conceptoSeleccionado.Codigo);

            if (existente is not null)
            {
                existente.Cantidad +=
                    nudCantidad.Value;

                existente.PrecioUnitario =
                    nudPrecioUnitario.Value;
            }
            else
            {
                _detalle.Add(
                    new ConceptoPresupuesto
                    {
                        Codigo =
                            _conceptoSeleccionado.Codigo,

                        Tipo =
                            _conceptoSeleccionado.Tipo,

                        Nombre =
                            _conceptoSeleccionado.Nombre,

                        Categoria =
                            _conceptoSeleccionado.Categoria,

                        Cantidad =
                            nudCantidad.Value,

                        PrecioUnitario =
                            nudPrecioUnitario.Value
                    });
            }
        }

        ActualizarDetallePresupuesto();
        RestablecerConceptoSeleccionado();
    }

    private void ActualizarDetallePresupuesto()
    {
        dgvConceptosPresupuesto.Rows.Clear();

        foreach (ConceptoPresupuesto concepto
            in _detalle)
        {
            int indice =
                dgvConceptosPresupuesto.Rows.Add(
                    concepto.Codigo,
                    concepto.Tipo,
                    concepto.Nombre,
                    concepto.Cantidad,
                    FormatearImporte(
                        concepto.PrecioUnitario),
                    FormatearImporte(
                        concepto.Subtotal));

            dgvConceptosPresupuesto.Rows[indice].Tag =
                concepto;
        }

        dgvConceptosPresupuesto.ClearSelection();

        lblCantidadConceptos.Text =
            _detalle.Count == 1
                ? "1 concepto agregado"
                : $"{_detalle.Count} conceptos agregados";

        lblTotalPresupuesto.Text =
            $"Total estimado: " +
            $"{FormatearImporte(TotalPresupuesto)}";

        btnGenerarPresupuesto.Enabled =
            _detalle.Count > 0;

        btnModificarCantidad.Enabled = false;
        btnQuitarConcepto.Enabled = false;
    }

    private void DgvConceptosPresupuesto_SelectionChanged(
    object? sender,
    EventArgs e)
    {
        bool haySeleccion =
            dgvConceptosPresupuesto.CurrentRow?.Tag
            is ConceptoPresupuesto;

        btnModificarCantidad.Enabled = haySeleccion;
        btnQuitarConcepto.Enabled = haySeleccion;
    }

    private void BtnModificarCantidad_Click(
        object? sender,
        EventArgs e)
    {
        if (dgvConceptosPresupuesto.CurrentRow?.Tag
            is not ConceptoPresupuesto concepto)
        {
            return;
        }

        ConceptoCatalogo? conceptoCatalogo =
            _catalogo.FirstOrDefault(item =>
                item.Codigo == concepto.Codigo);

        if (conceptoCatalogo is null)
        {
            return;
        }

        _conceptoSeleccionado = conceptoCatalogo;
        _conceptoEnEdicion = concepto;

        lblConceptoSeleccionado.Text =
            $"{concepto.Codigo} - {concepto.Nombre}";

        nudCantidad.Value = concepto.Cantidad;
        nudPrecioUnitario.Value =
            concepto.PrecioUnitario;

        btnAgregarConcepto.Text = "Actualizar";
        btnAgregarConcepto.Enabled = true;

        nudCantidad.Focus();
    }

    private void BtnQuitarConcepto_Click(
    object? sender,
    EventArgs e)
    {
        if (dgvConceptosPresupuesto.CurrentRow?.Tag
            is not ConceptoPresupuesto concepto)
        {
            return;
        }

        DialogResult respuesta = MessageBox.Show(
            this,
            $"¿Desea quitar \"{concepto.Nombre}\" " +
            $"del presupuesto?",
            "Quitar concepto",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes)
        {
            return;
        }

        _detalle.Remove(concepto);

        ActualizarDetallePresupuesto();
        RestablecerConceptoSeleccionado();
    }

    private void RestablecerConceptoSeleccionado()
    {
        _conceptoSeleccionado = null;
        _conceptoEnEdicion = null;

        lblConceptoSeleccionado.Text =
            "Ningún concepto seleccionado";

        nudCantidad.Value = 1;
        nudPrecioUnitario.Value = 0;

        btnAgregarConcepto.Text = "Agregar";
        btnAgregarConcepto.Enabled = false;
    }

    private void DgvCatalogoConceptos_CellDoubleClick(
    object? sender,
    DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        if (dgvCatalogoConceptos.Rows[e.RowIndex].Tag
            is not ConceptoCatalogo concepto)
        {
            return;
        }

        SeleccionarConcepto(concepto);
        AgregarOActualizarConcepto();
    }

    private void BtnGenerarPresupuesto_Click(
    object? sender,
    EventArgs e)
    {
        if (_detalle.Count == 0)
        {
            MessageBox.Show(
                this,
                "Agregá al menos un concepto al presupuesto.",
                "Presupuesto incompleto",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        DialogResult respuesta = MessageBox.Show(
            this,
            $"Se generará un presupuesto por " +
            $"{FormatearImporte(TotalPresupuesto)}. " +
            $"¿Desea continuar?",
            "Generar presupuesto",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes)
        {
            return;
        }

        _presupuestoGenerado = true;

        MessageBox.Show(
            this,
            "El presupuesto fue generado correctamente.",
            "Presupuesto generado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
    }

    private void BtnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private void FrmGenerarPresupuesto_FormClosing(
    object? sender,
    FormClosingEventArgs e)
    {
        bool hayDatos =
            _detalle.Count > 0
            || !string.IsNullOrWhiteSpace(
                txtObservaciones.Text);

        if (_presupuestoGenerado || !hayDatos)
        {
            return;
        }

        DialogResult respuesta = MessageBox.Show(
            this,
            "Hay datos del presupuesto que no fueron guardados. " +
            "¿Desea cerrar igualmente?",
            "Cancelar presupuesto",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (respuesta == DialogResult.No)
        {
            e.Cancel = true;
            DialogResult = DialogResult.None;
        }
    }

    private static string FormatearImporte(
    decimal importe)
    {
        return importe.ToString(
            "C2",
            System.Globalization.CultureInfo
                .GetCultureInfo("es-AR"));
    }

    #region Estilos

    /// <summary>
    /// Aplica el estilo visual correspondiente a los controles
    /// utilizados en la generación del presupuesto.
    /// </summary>
    private void AplicarEstilos()
    {
        ConfigurarBotonPrimario(
            btnBuscarConceptos);

        ConfigurarBotonPrimario(
            btnAgregarConcepto);

        ConfigurarBotonPrimario(
            btnGenerarPresupuesto);

        ConfigurarBotonSecundario(
            btnLimpiarFiltros);

        ConfigurarBotonSecundario(
            btnModificarCantidad);

        ConfigurarBotonSecundario(
            btnQuitarConcepto);

        ConfigurarBotonSecundario(
            btnCancelar);

        ConfigurarGrilla(
            dgvCatalogoConceptos);

        ConfigurarGrilla(
            dgvConceptosPresupuesto);
    }

    /// <summary>
    /// Aplica el estilo utilizado para las acciones principales.
    /// </summary>
    private static void ConfigurarBotonPrimario(
        Button boton)
    {
        boton.BackColor =
            Color.FromArgb(30, 64, 175);

        boton.ForeColor = Color.White;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.Cursor = Cursors.Hand;

        boton.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
    }

    /// <summary>
    /// Aplica el estilo utilizado para las acciones secundarias.
    /// </summary>
    private static void ConfigurarBotonSecundario(
        Button boton)
    {
        boton.BackColor = Color.White;

        boton.ForeColor =
            Color.FromArgb(51, 65, 85);

        boton.FlatStyle = FlatStyle.Flat;

        boton.FlatAppearance.BorderColor =
            Color.FromArgb(203, 213, 225);

        boton.FlatAppearance.BorderSize = 1;
        boton.Cursor = Cursors.Hand;

        boton.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Regular);
    }

    /// <summary>
    /// Configura la presentación visual y el comportamiento
    /// general de una grilla de solo lectura.
    /// </summary>
    private static void ConfigurarGrilla(
        DataGridView grilla)
    {
        grilla.EnableHeadersVisualStyles = false;
        grilla.BackgroundColor = Color.White;
        grilla.BorderStyle = BorderStyle.FixedSingle;

        grilla.GridColor =
            Color.FromArgb(226, 232, 240);

        grilla.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(241, 245, 249);

        grilla.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.FromArgb(51, 65, 85);

        grilla.ColumnHeadersDefaultCellStyle.Font =
            new Font(
                "Segoe UI",
                9F,
                FontStyle.Bold);

        grilla.ColumnHeadersDefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleLeft;

        grilla.DefaultCellStyle.BackColor =
            Color.White;

        grilla.DefaultCellStyle.ForeColor =
            Color.FromArgb(51, 65, 85);

        grilla.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(219, 234, 254);

        grilla.DefaultCellStyle.SelectionForeColor =
            Color.FromArgb(30, 64, 175);

        grilla.DefaultCellStyle.Font =
            new Font(
                "Segoe UI",
                9F,
                FontStyle.Regular);

        grilla.DefaultCellStyle.Padding =
            new Padding(4);

        grilla.RowTemplate.Height = 32;

        grilla.ReadOnly = true;
        grilla.MultiSelect = false;
        grilla.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        grilla.AllowUserToAddRows = false;
        grilla.AllowUserToDeleteRows = false;
        grilla.AllowUserToResizeRows = false;
        grilla.RowHeadersVisible = false;

        grilla.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.None;

        grilla.ScrollBars =
            ScrollBars.Both;
    }

    #endregion

    private sealed record ConceptoCatalogo(
    string Codigo,
    string Tipo,
    string Nombre,
    string Categoria,
    string Unidad,
    decimal PrecioReferencia);

    private sealed class ConceptoPresupuesto
    {
        public required string Codigo { get; init; }

        public required string Tipo { get; init; }

        public required string Nombre { get; init; }

        public required string Categoria { get; init; }

        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal =>
            Cantidad * PrecioUnitario;
    }


}

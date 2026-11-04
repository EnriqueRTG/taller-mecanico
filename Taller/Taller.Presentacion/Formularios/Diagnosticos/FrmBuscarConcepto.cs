using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Diagnosticos;

/// <summary>Selecciona un concepto del catálogo ya cargado, sin modificarlo ni agregar ítems.</summary>
public sealed class FrmBuscarConcepto : Form
{
    private readonly IReadOnlyList<ConceptoPropuestaDto> _catalogo;
    private readonly TextBox _buscar = new() { Name = "txtBuscar", Dock = DockStyle.Fill, PlaceholderText = "Nombre, descripción o categoría" };
    private readonly ComboBox _tipo = new() { Name = "cboTipo", Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _categoria = new() { Name = "cboCategoria", Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DataGridView _lista = new() { Name = "dgvConceptos", Dock = DockStyle.Fill, AutoGenerateColumns = false,
        ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, MultiSelect = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect, RowHeadersVisible = false, BackgroundColor = Color.White,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly TextBox _descripcion = new() { Name = "txtDescripcion", Dock = DockStyle.Fill, ReadOnly = true, Multiline = true, ScrollBars = ScrollBars.Vertical, TabStop = false };
    private readonly Label _resumen = new() { Dock = DockStyle.Fill, AutoSize = true };
    private readonly Button _seleccionar = new() { Name = "btnSeleccionar", Text = "Seleccionar", AutoSize = true, Height = 34 };
    public ConceptoPropuestaDto? ConceptoSeleccionado { get; private set; }

    public FrmBuscarConcepto(IReadOnlyList<ConceptoPropuestaDto> catalogo, string busqueda = "")
    {
        _catalogo = catalogo;
        Text = "Buscar concepto del catálogo";
        Font = new Font("Segoe UI", 10F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1060, 620);
        MinimumSize = new Size(760, 480);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(248, 250, 252);
        MinimizeBox = false;
        ShowInTaskbar = false;
        var principal = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 1, RowCount = 5 };
        principal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        foreach (var altura in new[] { 64, 0, 90, 28, 44 }) principal.RowStyles.Add(new RowStyle(altura == 0 ? SizeType.Percent : SizeType.Absolute, altura == 0 ? 100 : altura));
        Controls.Add(principal);
        var filtros = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2 };
        filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
        filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        filtros.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        filtros.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        string[] etiquetas = ["Buscar", "Tipo", "Categoría"];
        Control[] editores = [_buscar, _tipo, _categoria];
        for (int i = 0; i < etiquetas.Length; i++)
        {
            filtros.Controls.Add(new Label { Text = etiquetas[i], Dock = DockStyle.Fill }, i, 0);
            editores[i].AccessibleName = etiquetas[i];
            filtros.Controls.Add(editores[i], i, 1);
        }
        principal.Controls.Add(filtros, 0, 0);
        principal.Controls.Add(_lista, 0, 1);
        principal.Controls.Add(_descripcion, 0, 2);
        principal.Controls.Add(_resumen, 0, 3);
        var acciones = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        var cancelar = new Button { Text = "Cancelar", AutoSize = true, Height = 34, DialogResult = DialogResult.Cancel };
        EstilosInterfaz.AplicarBotonSecundario(cancelar);
        EstilosInterfaz.AplicarBotonSecundario(_seleccionar);
        acciones.Controls.Add(cancelar);
        acciones.Controls.Add(_seleccionar);
        principal.Controls.Add(acciones, 0, 4);
        CancelButton = cancelar;
        AcceptButton = _seleccionar;
        AgregarColumna("Nombre", nameof(ConceptoPropuestaDto.Nombre), 230, 180);
        AgregarColumna("Tipo", nameof(ConceptoPropuestaDto.Tipo), 90, 95);
        AgregarColumna("Categoría", nameof(ConceptoPropuestaDto.Categoria), 150, 140);
        AgregarColumna("Precio de referencia ARS", nameof(ConceptoPropuestaDto.PrecioReferencia), 110, 150);
        _lista.Columns[3].DefaultCellStyle.Format = "N2";
        _lista.Columns[3].DefaultCellStyle.NullValue = "Sin referencia";
        _lista.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _tipo.Items.Add("Todos los tipos");
        foreach (var tipo in Enum.GetValues<TipoConcepto>()) _tipo.Items.Add(tipo);
        _categoria.Items.Add("Todas las categorías");
        foreach (var categoria in catalogo.Select(c => c.Categoria).Distinct().OrderBy(c => c)) _categoria.Items.Add(categoria);
        _tipo.SelectedIndex = _categoria.SelectedIndex = 0;
        _buscar.Text = busqueda;
        _buscar.TextChanged += (_, _) => Filtrar();
        _tipo.SelectedIndexChanged += (_, _) => Filtrar();
        _categoria.SelectedIndexChanged += (_, _) => Filtrar();
        _lista.SelectionChanged += (_, _) => ActualizarSeleccion();
        _lista.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Seleccionar(); };
        _seleccionar.Click += (_, _) => Seleccionar();
        Load += (_, _) => { DisposicionVentanas.AjustarAlAreaDisponible(this); _buscar.Focus(); };
        Filtrar();
    }

    private void AgregarColumna(string titulo, string propiedad, float peso, int minimo) =>
        _lista.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = titulo, DataPropertyName = propiedad,
            FillWeight = peso, MinimumWidth = minimo, SortMode = DataGridViewColumnSortMode.NotSortable });

    private void Filtrar()
    {
        var q = _buscar.Text.Trim();
        _lista.DataSource = _catalogo.Where(c =>
            Taller.Aplicacion.Consultas.BusquedaTexto.Coincide($"{c.Nombre} {c.Descripcion} {c.Categoria}", q)
            && (_tipo.SelectedItem is not TipoConcepto tipo || c.Tipo == tipo)
            && (_categoria.SelectedIndex == 0 || c.Categoria == (string?)_categoria.SelectedItem)).ToList();
        _resumen.Text = $"{_lista.RowCount} conceptos · Precio orientativo: revise su vigencia antes de presupuestar.";
        ActualizarSeleccion();
    }

    private void ActualizarSeleccion()
    {
        var concepto = _lista.CurrentRow?.DataBoundItem as ConceptoPropuestaDto;
        _seleccionar.Enabled = concepto is not null;
        _descripcion.Text = concepto is null ? "Sin resultados para los filtros indicados." :
            concepto.Nombre + Environment.NewLine + (concepto.Descripcion ?? "Sin descripción adicional.");
    }

    private void Seleccionar()
    {
        if (_lista.CurrentRow?.DataBoundItem is not ConceptoPropuestaDto concepto) return;
        ConceptoSeleccionado = concepto;
        DialogResult = DialogResult.OK;
        Close();
    }
}

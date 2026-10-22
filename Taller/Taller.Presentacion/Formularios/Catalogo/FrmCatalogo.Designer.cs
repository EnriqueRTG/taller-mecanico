#nullable enable
namespace Taller.Presentacion.Formularios.Catalogo;
partial class FrmCatalogo
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpPrincipal = new TableLayoutPanel();
        lblTitulo = new Label();
        lblDescripcion = new Label();
        flpFiltros = new FlowLayoutPanel();
        txtBuscar = new TextBox();
        cmbCategorias = new ComboBox();
        cmbTiposConcepto = new ComboBox();
        cboDisponibilidad = new ComboBox();
        btnActualizar = new Button();
        btnLimpiar = new Button();
        dgvConceptos = new DataGridView();
        txtDetalle = new TextBox();
        lblCantidad = new Label();
        SuspendLayout();
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 9F);
        ClientSize = new Size(1030, 580);
        MinimumSize = new Size(760, 480);
        Text = "Catálogo de conceptos";
        BackColor = Color.FromArgb(244, 247, 251);
        tlpPrincipal.Name = "tlpPrincipal";
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Padding = new Padding(12);
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.RowCount = 6;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 86));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        Controls.Add(tlpPrincipal);
        tlpPrincipal.Controls.Add(lblTitulo, 0, 0);
        tlpPrincipal.Controls.Add(lblDescripcion, 0, 1);
        tlpPrincipal.Controls.Add(flpFiltros, 0, 2);
        tlpPrincipal.Controls.Add(dgvConceptos, 0, 3);
        tlpPrincipal.Controls.Add(txtDetalle, 0, 4);
        tlpPrincipal.Controls.Add(lblCantidad, 0, 5);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Text = "Catálogo de servicios, repuestos y consumibles";
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Font = new Font("Segoe UI", 17, FontStyle.Bold);
        lblDescripcion.Name = "lblDescripcion";
        lblDescripcion.Dock = DockStyle.Fill;
        lblDescripcion.Text = "Oferta del negocio y precios de referencia en ARS. Disponible significa habilitado en catálogo; no confirma existencias de stock. Seleccione una fila para leer su descripción.";
        flpFiltros.Name = "flpFiltros";
        flpFiltros.Dock = DockStyle.Fill;
        flpFiltros.AutoScroll = true;
        flpFiltros.WrapContents = true;
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Width = 270;
        txtBuscar.PlaceholderText = "Código, nombre, descripción o categoría";
        txtBuscar.MaxLength = 150;
        cmbCategorias.Name = "cmbCategorias";
        cmbCategorias.Width = 210;
        cmbCategorias.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTiposConcepto.Name = "cmbTiposConcepto";
        cmbTiposConcepto.Width = 140;
        cmbTiposConcepto.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDisponibilidad.Name = "cboDisponibilidad";
        cboDisponibilidad.Width = 185;
        cboDisponibilidad.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDisponibilidad.Items.AddRange(["Toda disponibilidad", "Disponibles en catálogo", "No disponibles", "Servicios para ingreso"]);
        btnActualizar.Name = "btnActualizar";
        btnActualizar.Text = "Actualizar";
        btnActualizar.AutoSize = true;
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Text = "Limpiar filtros";
        btnLimpiar.AutoSize = true;
        flpFiltros.Controls.AddRange([txtBuscar, cmbCategorias, cmbTiposConcepto, cboDisponibilidad, btnActualizar, btnLimpiar]);
        dgvConceptos.Name = "dgvConceptos";
        dgvConceptos.Dock = DockStyle.Fill;
        dgvConceptos.ColumnHeadersHeightSizeMode=DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvConceptos.AutoGenerateColumns = false;
        dgvConceptos.ReadOnly = true;
        dgvConceptos.AllowUserToAddRows = false;
        dgvConceptos.AllowUserToDeleteRows = false;
        dgvConceptos.AllowUserToResizeColumns = true;
        dgvConceptos.MultiSelect = false;
        dgvConceptos.RowHeadersVisible = false;
        dgvConceptos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvConceptos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvConceptos.BackgroundColor = Color.White;
        colIdConcepto = new DataGridViewTextBoxColumn
        {
            Name = "colIdConcepto",
            DataPropertyName = "IdConcepto",
            HeaderText = "Código",
            MinimumWidth = 60,
            FillWeight = 55
        };
        dgvConceptos.Columns.Add(colIdConcepto);
        colCategoria = new DataGridViewTextBoxColumn
        {
            Name = "colCategoria",
            DataPropertyName = "Categoria",
            HeaderText = "Categoría",
            MinimumWidth = 120,
            FillWeight = 145
        };
        dgvConceptos.Columns.Add(colCategoria);
        colTipo = new DataGridViewTextBoxColumn
        {
            Name = "colTipo",
            DataPropertyName = "Tipo",
            HeaderText = "Tipo",
            MinimumWidth = 85,
            FillWeight = 80
        };
        dgvConceptos.Columns.Add(colTipo);
        colNombre = new DataGridViewTextBoxColumn
        {
            Name = "colNombre",
            DataPropertyName = "Nombre",
            HeaderText = "Concepto",
            MinimumWidth = 170,
            FillWeight = 200
        };
        dgvConceptos.Columns.Add(colNombre);
        colPrecio = new DataGridViewTextBoxColumn
        {
            Name = "colPrecio",
            DataPropertyName = "Precio",
            HeaderText = "Referencia ARS",
            MinimumWidth = 100,
            FillWeight = 100
        };
        dgvConceptos.Columns.Add(colPrecio);
        colEstado = new DataGridViewTextBoxColumn
        {
            Name = "colEstado",
            DataPropertyName = "Estado",
            HeaderText = "Disponibilidad",
            MinimumWidth = 110,
            FillWeight = 120
        };
        dgvConceptos.Columns.Add(colEstado);
        colPrecio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        txtDetalle.Name = "txtDetalle";
        txtDetalle.Dock = DockStyle.Fill;
        txtDetalle.Multiline = true;
        txtDetalle.ReadOnly = true;
        txtDetalle.ScrollBars = ScrollBars.Vertical;
        txtDetalle.BackColor = Color.White;
        lblCantidad.Name = "lblCantidad";
        lblCantidad.Dock = DockStyle.Fill;
        Load += FormCatalogo_Load;
        FormClosing += Formulario_FormClosing;
        txtBuscar.TextChanged += Filtro_Cambio;
        cmbCategorias.SelectedIndexChanged += Filtro_Cambio;
        cmbTiposConcepto.SelectedIndexChanged += Filtro_Cambio;
        cboDisponibilidad.SelectedIndexChanged += Filtro_Cambio;
        dgvConceptos.SelectionChanged += Seleccion_Cambio;
        btnActualizar.Click += Actualizar_Click;
        btnLimpiar.Click += Limpiar_Click;
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpPrincipal = null!;
    private FlowLayoutPanel flpFiltros = null!;
    private Label lblTitulo = null!, lblDescripcion = null!, lblCantidad = null!;
    private TextBox txtBuscar = null!, txtDetalle = null!;
    private ComboBox cmbCategorias = null!, cmbTiposConcepto = null!, cboDisponibilidad = null!;
    private Button btnActualizar = null!, btnLimpiar = null!;
    private DataGridView dgvConceptos = null!;
    private DataGridViewTextBoxColumn colIdConcepto = null!;
    private DataGridViewTextBoxColumn colCategoria = null!;
    private DataGridViewTextBoxColumn colTipo = null!;
    private DataGridViewTextBoxColumn colNombre = null!;
    private DataGridViewTextBoxColumn colPrecio = null!;
    private DataGridViewTextBoxColumn colEstado = null!;
}

#nullable enable
namespace Taller.Presentacion.Formularios.Presupuestos;
partial class FrmPrepararPropuesta
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblAyuda = null!, lblTotal = null!;
    private TextBox txtBuscar = null!, txtDescripcion = null!, txtMotivo = null!;
    private ComboBox cboConcepto = null!, cboOrigen = null!;
    private NumericUpDown nudCantidad = null!, nudPrecio = null!;
    private DataGridView dgvItems = null!;
    private Button btnAgregar = null!, btnQuitar = null!, btnGuardar = null!, btnCerrar = null!;
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpPrincipal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 7,
            Padding = new Padding(12)
        };
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        lblAyuda = new Label
        {
            Dock = DockStyle.Fill
        };
        txtBuscar = new TextBox
        {
            Dock = DockStyle.Fill,
            PlaceholderText = "Buscar concepto, categoría o descripción"
        };
        txtBuscar.TextChanged += Filtrar;
        var entrada = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 2
        };
        entrada.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        entrada.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
        entrada.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
        entrada.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
        cboConcepto = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cboConcepto.SelectedIndexChanged += SeleccionarConcepto;
        txtDescripcion = new TextBox
        {
            Dock = DockStyle.Fill,
            MaxLength = 500,
            PlaceholderText = "Descripción acordada"
        };
        cboOrigen = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cboOrigen.Items.AddRange(new object[] { "Suministra taller", "Aporta cliente" });
        cboOrigen.SelectedIndex = 0;
        cboOrigen.SelectedIndexChanged += CambiarOrigen;
        nudCantidad = new NumericUpDown
        {
            Dock = DockStyle.Fill,
            DecimalPlaces = 3,
            Minimum = 0.001m,
            Maximum = 1000000,
            Value = 1
        };
        nudPrecio = new NumericUpDown
        {
            Dock = DockStyle.Fill,
            DecimalPlaces = 2,
            Maximum = 100000000,
            ThousandsSeparator = true
        };
        btnAgregar = new Button
        {
            Text = "Agregar ítem",
            AutoSize = true
        };
        btnAgregar.Click += Agregar;
        btnQuitar = new Button
        {
            Text = "Quitar seleccionado",
            AutoSize = true
        };
        btnQuitar.Click += Quitar;
        entrada.Controls.Add(cboConcepto, 0, 0);
        entrada.SetColumnSpan(cboConcepto, 2);
        entrada.Controls.Add(cboOrigen, 2, 0);
        entrada.Controls.Add(btnAgregar, 3, 0);
        entrada.Controls.Add(txtDescripcion, 0, 1);
        entrada.Controls.Add(nudCantidad, 1, 1);
        entrada.Controls.Add(nudPrecio, 2, 1);
        entrada.Controls.Add(btnQuitar, 3, 1);
        dgvItems = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeColumns = true,
            RowHeadersVisible = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White
        };
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescripcion", DataPropertyName = "Descripcion", HeaderText = "Concepto / alcance", FillWeight = 220, MinimumWidth = 210 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colOrigen", DataPropertyName = "OrigenMaterial", HeaderText = "Aporte", MinimumWidth = 90 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCantidad", DataPropertyName = "Cantidad", HeaderText = "Cantidad", MinimumWidth = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrecio", DataPropertyName = "PrecioUnitario", HeaderText = "Precio unitario", MinimumWidth = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubtotal", DataPropertyName = "Subtotal", HeaderText = "Subtotal", MinimumWidth = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        lblTotal = new Label
        {
            Dock = DockStyle.Fill
        };
        txtMotivo = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            MaxLength = 1000,
            PlaceholderText = "Justificación técnica de viabilidad, condiciones y motivo de la propuesta (obligatorio)"
        };
        var acciones = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };
        btnGuardar = new Button
        {
            Text = "Confirmar propuesta para recepción",
            AutoSize = true,
            Height = 35
        };
        btnGuardar.Click += Guardar;
        btnCerrar = new Button
        {
            Text = "Cancelar",
            AutoSize = true,
            Height = 35
        };
        btnCerrar.Click += Cerrar;
        acciones.Controls.AddRange(new Control[] { btnGuardar, btnCerrar });
        tlpPrincipal.Controls.Add(lblAyuda, 0, 0);
        tlpPrincipal.Controls.Add(txtBuscar, 0, 1);
        tlpPrincipal.Controls.Add(entrada, 0, 2);
        tlpPrincipal.Controls.Add(dgvItems, 0, 3);
        tlpPrincipal.Controls.Add(lblTotal, 0, 4);
        tlpPrincipal.Controls.Add(txtMotivo, 0, 5);
        tlpPrincipal.Controls.Add(acciones, 0, 6);
        Controls.Add(tlpPrincipal);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 10);
        Text = "Preparar propuesta técnica";
        ClientSize = new Size(1000, 650);
        MinimumSize = new Size(820, 580);
        StartPosition = FormStartPosition.CenterParent;
        Load += Cargar;
        FormClosing += AlCerrar;
    }
}

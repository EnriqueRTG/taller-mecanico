namespace Taller.Presentacion.Formularios.Atenciones;
partial class EditorSolicitudes
{
    private TableLayoutPanel tabla = null!;
    private ComboBox cboTipo = null!;
    private TextBox txtSolicitud = null!, txtObservaciones = null!;
    private DataGridView dgvSolicitudes = null!;
    private Button btnAgregar = null!, btnQuitar = null!;
    private CheckBox chkKm = null!;
    private NumericUpDown nudKm = null!;
    private void InitializeComponent()
    {
        tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(4) };
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        var acciones = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        cboTipo = new ComboBox { Name = "cboTipo", DropDownStyle = ComboBoxStyle.DropDownList, Width = 230 };
        cboTipo.Items.AddRange(["Trabajo que el cliente solicita", "Problema o síntoma que informa"]);
        btnAgregar = new Button { Name = "btnAgregarSolicitud", Text = "Agregar solicitud", AutoSize = true, FlatStyle = FlatStyle.Flat, ForeColor = Color.FromArgb(30,64,175), BackColor = Color.White };
        btnQuitar = new Button { Name = "btnQuitarSolicitud", Text = "Quitar seleccionada", AutoSize = true, FlatStyle = FlatStyle.Flat, ForeColor = Color.FromArgb(30,64,175), BackColor = Color.White };
        acciones.Controls.AddRange([cboTipo, btnAgregar, btnQuitar]);
        txtSolicitud = new TextBox { Name = "txtSolicitud", Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical, MaxLength = 500, PlaceholderText = "Describa cada pedido por separado y presione Agregar solicitud." };
        dgvSolicitudes = new DataGridView { Name = "dgvSolicitudes", Dock = DockStyle.Fill, AutoGenerateColumns = false, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false, MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells, BackgroundColor = Color.White };
        dgvSolicitudes.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvSolicitudes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSolicitud", HeaderText = "Solicitudes de esta visita", DataPropertyName = "Etiqueta", MinimumWidth = 200 });
        var ingreso = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        chkKm = new CheckBox { Name = "chkKmRecepcion", Text = "Kilometraje de ingreso", AutoSize = true };
        nudKm = new NumericUpDown { Name = "nudKmRecepcion", Maximum = int.MaxValue, Enabled = false, Width = 120, ThousandsSeparator = true };
        ingreso.Controls.AddRange([chkKm,nudKm]);
        txtObservaciones = new TextBox { Name = "txtObservacionesRecepcion", Dock = DockStyle.Fill, MaxLength = 1000, Multiline = true, ScrollBars = ScrollBars.Vertical, PlaceholderText = "Observaciones de recepción: daños visibles, testigos, condiciones de ingreso (opcional)." };
        tabla.Controls.Add(acciones,0,0); tabla.Controls.Add(txtSolicitud,0,1); tabla.Controls.Add(dgvSolicitudes,0,2); tabla.Controls.Add(ingreso,0,3); tabla.Controls.Add(txtObservaciones,0,4);
        Controls.Add(tabla); Name = "editorSolicitudes"; BackColor = Color.White; MinimumSize = new Size(300,320); Size = new Size(750,340);
        btnAgregar.Click += Agregar; btnQuitar.Click += Quitar; chkKm.CheckedChanged += KilometrajeCambiado;
    }
}

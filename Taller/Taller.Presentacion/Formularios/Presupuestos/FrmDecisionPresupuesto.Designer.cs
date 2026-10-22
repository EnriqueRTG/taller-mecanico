#nullable enable
namespace Taller.Presentacion.Formularios.Presupuestos;
partial class FrmDecisionPresupuesto
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
        tlpPrincipal = new TableLayoutPanel();
        tlpPrincipal.Name = "tlpPrincipal";
        lblResumen = new Label();
        lblResumen.Name = "lblResumen";
        dgvItems = new DataGridView();
        dgvItems.Name = "dgvItems";
        lblTotal = new Label();
        lblTotal.Name = "lblTotal";
        txtMotivo = new TextBox();
        txtMotivo.Name = "txtMotivo";
        flpAcciones = new FlowLayoutPanel();
        flpAcciones.Name = "flpAcciones";
        btnRechazar = new Button();
        btnRechazar.Name = "btnRechazar";
        btnGuardar = new Button();
        btnGuardar.Name = "btnGuardar";
        btnCerrar = new Button();
        btnCerrar.Name = "btnCerrar";
        components = new System.ComponentModel.Container();
        SuspendLayout();
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 10F);
        ClientSize = new Size(1020, 620);
        MinimumSize = new Size(820, 520);
        StartPosition = FormStartPosition.CenterParent;
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Padding = new Padding(12);
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowCount = 5;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        Controls.Add(tlpPrincipal);
        Text = "Decisión del cliente por ítem";
        tlpPrincipal.Controls.Add(lblResumen, 0, 0);
        lblResumen.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(dgvItems, 0, 1);
        dgvItems.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(lblTotal, 0, 2);
        lblTotal.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(txtMotivo, 0, 3);
        txtMotivo.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(flpAcciones, 0, 4);
        flpAcciones.Dock = DockStyle.Fill;
        dgvItems.AutoGenerateColumns = false;
        dgvItems.AllowUserToAddRows = false;
        dgvItems.AllowUserToDeleteRows = false;
        dgvItems.AllowUserToResizeColumns = true;
        dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvItems.MultiSelect = false;
        dgvItems.ReadOnly = true;
        dgvItems.RowHeadersVisible = false;
        dgvItems.BackgroundColor = Color.White;
        dgvItems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvItems.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescripcion", DataPropertyName = "Descripcion", HeaderText = "Concepto propuesto", FillWeight = 240, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCantidad", DataPropertyName = "Cantidad", HeaderText = "Cantidad", FillWeight = 70, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colOrigen", DataPropertyName = "OrigenMaterial", HeaderText = "Aporta", MinimumWidth = 90 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrecioUnitario", DataPropertyName = "PrecioUnitario", HeaderText = "Precio unitario", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubtotal", DataPropertyName = "Subtotal", HeaderText = "Subtotal", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        dgvItems.ReadOnly = false;
        dgvItems.Columns[0].ReadOnly = true;
        dgvItems.Columns[1].ReadOnly = true;
        dgvItems.Columns[2].ReadOnly = true;
        dgvItems.Columns[3].ReadOnly = true;
        dgvItems.Columns.Insert(0, new DataGridViewCheckBoxColumn { Name = "colAceptar", DataPropertyName = "Aceptar", HeaderText = "Acepta", FillWeight = 60 });
        dgvItems.CellValueChanged += Cambio;
        dgvItems.CurrentCellDirtyStateChanged += CommitCelda;
        txtMotivo.TextChanged += TextoCambiado;
        txtMotivo.Multiline = true;
        txtMotivo.MaxLength = 1000;
        txtMotivo.ScrollBars = ScrollBars.Vertical;
        txtMotivo.PlaceholderText = "Motivo de los ítems excluidos o del rechazo. Obligatorio si no se acepta todo.";
        btnRechazar.Text = "Rechazar todos";
        btnRechazar.AutoSize = true;
        btnRechazar.Height = 34;
        btnRechazar.Click += RechazarTodo;
        flpAcciones.Controls.Add(btnRechazar);
        btnGuardar.Text = "Registrar decisión";
        btnGuardar.AutoSize = true;
        btnGuardar.Height = 34;
        btnGuardar.Click += Guardar;
        flpAcciones.Controls.Add(btnGuardar);
        btnCerrar.Text = "Cerrar";
        btnCerrar.AutoSize = true;
        btnCerrar.Height = 34;
        btnCerrar.Click += Cerrar;
        flpAcciones.Controls.Add(btnCerrar);
        Load += Cargar;
        FormClosing += AlCerrar;
        BackColor = Color.FromArgb(248, 250, 252);
        btnGuardar.BackColor = Color.FromArgb(30, 64, 175);
        btnGuardar.ForeColor = Color.White;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        CancelButton = btnCerrar;
        btnCerrar.DialogResult = DialogResult.None;
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblResumen = null!;
    private DataGridView dgvItems = null!;
    private Label lblTotal = null!;
    private TextBox txtMotivo = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private Button btnRechazar = null!;
    private Button btnGuardar = null!;
    private Button btnCerrar = null!;
}

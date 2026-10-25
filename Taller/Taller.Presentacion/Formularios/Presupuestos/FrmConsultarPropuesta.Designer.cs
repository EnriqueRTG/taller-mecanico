#nullable enable
namespace Taller.Presentacion.Formularios.Presupuestos;
partial class FrmConsultarPropuesta
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
        tlpPrincipal.Name = "tlpPrincipal";
        lblResumen = new Label();
        lblResumen.Name = "lblResumen";
        dgvItems = new DataGridView();
        dgvItems.Name = "dgvItems";
        txtObservaciones = new TextBox();
        txtObservaciones.Name = "txtObservaciones";
        lblTotal = new Label();
        lblTotal.Name = "lblTotal";
        flpAcciones = new FlowLayoutPanel();
        flpAcciones.Name = "flpAcciones";
        btnCerrar = new Button();
        btnCerrar.Name = "btnCerrar";
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
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        Controls.Add(tlpPrincipal);
        Text = "Consultar presupuesto";
        tlpPrincipal.Controls.Add(lblResumen, 0, 0);
        lblResumen.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(dgvItems, 0, 1);
        dgvItems.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(txtObservaciones, 0, 2);
        txtObservaciones.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(lblTotal, 0, 3);
        lblTotal.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(flpAcciones, 0, 4);
        flpAcciones.Dock = DockStyle.Fill;
        txtObservaciones.ReadOnly = true;
        txtObservaciones.Multiline = true;
        txtObservaciones.ScrollBars = ScrollBars.Vertical;
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
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescripcion", DataPropertyName = "Descripcion", HeaderText = "Descripción", FillWeight = 240, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCantidad", DataPropertyName = "Cantidad", HeaderText = "Cantidad", FillWeight = 70, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colOrigen", DataPropertyName = "OrigenMaterial", HeaderText = "Aporta", MinimumWidth = 90 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrecioUnitario", DataPropertyName = "PrecioUnitario", HeaderText = "Precio unitario", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubtotal", DataPropertyName = "Subtotal", HeaderText = "Subtotal", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        btnCerrar.Text = "Cerrar";
        btnCerrar.AutoSize = true;
        btnCerrar.Height = 34;
        btnCerrar.Click += Cerrar;
        btnExportar = new Button { Name = "btnExportar", Text = "Exportar HTML", AutoSize = true, Height = 34 };
        btnExportar.Click += Exportar;
        btnImprimir = new Button { Name = "btnImprimir", Text = "Imprimir / PDF", AutoSize = true, Height = 34 };
        btnImprimir.Click += Imprimir;
        flpAcciones.Controls.AddRange(new Control[] { btnExportar, btnImprimir, btnCerrar });
        BackColor = Color.FromArgb(248, 250, 252);
        CancelButton = btnCerrar;
        btnCerrar.DialogResult = DialogResult.None;
        dgvItems.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colAceptado", DataPropertyName = "AceptadoPorCliente", HeaderText = "Cliente acepta", ThreeState = true, FillWeight = 80, ReadOnly = true });
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblResumen = null!;
    private DataGridView dgvItems = null!;
    private TextBox txtObservaciones = null!;
    private Label lblTotal = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private Button btnExportar = null!, btnImprimir = null!;
    private Button btnCerrar = null!;
}

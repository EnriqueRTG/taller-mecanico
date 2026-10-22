#nullable enable
namespace Taller.Presentacion.Formularios.Comprobantes;
partial class FrmComprobantes
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        tlpPrincipal = new TableLayoutPanel();
        tlpPrincipal.Name = "tlpPrincipal";
        lblTitulo = new Label();
        lblTitulo.Name = "lblTitulo";
        txtBuscar = new TextBox();
        txtBuscar.Name = "txtBuscar";
        cboEstado = new ComboBox();
        cboEstado.Name = "cboEstado";
        dgvDatos = new DataGridView();
        dgvDatos.Name = "dgvDatos";
        lblAyuda = new Label();
        lblAyuda.Name = "lblAyuda";
        flpAcciones = new FlowLayoutPanel();
        flpAcciones.Name = "flpAcciones";
        btnGestionar = new Button();
        btnGestionar.Name = "btnGestionar";
        btnActualizar = new Button();
        btnActualizar.Name = "btnActualizar";
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
        tlpPrincipal.RowCount = 6;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        Controls.Add(tlpPrincipal);Text="Liquidaciones, cobros y entregas";
        tlpPrincipal.Controls.Add(lblTitulo, 0, 0);
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Text = "Liquidaciones, cobros y entregas";
        tlpPrincipal.Controls.Add(txtBuscar, 0, 1);
        txtBuscar.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(cboEstado, 0, 2);
        cboEstado.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(dgvDatos, 0, 3);
        dgvDatos.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(lblAyuda, 0, 4);
        lblAyuda.Dock = DockStyle.Fill;
        lblAyuda.Text = "Saldo pendiente primero. Se permiten cobros parciales y distintos medios; la entrega exige saldo cero. Documentos internos sin validez fiscal.";
        tlpPrincipal.Controls.Add(flpAcciones, 0, 5);
        flpAcciones.Dock = DockStyle.Fill;
        txtBuscar.PlaceholderText="Buscar atención, liquidación, cliente o dominio";txtBuscar.TextChanged+=Filtrar;
        cboEstado.DropDownStyle=ComboBoxStyle.DropDownList;cboEstado.Items.AddRange(new object[]{"Todas","Por liquidar","Con saldo pendiente","Listas para entregar","Entregadas"});cboEstado.SelectedIndexChanged+=Filtrar;
        dgvDatos.SelectionChanged+=Seleccion;
        dgvDatos.AutoGenerateColumns = false;
        dgvDatos.AllowUserToAddRows = false;
        dgvDatos.AllowUserToDeleteRows = false;
        dgvDatos.AllowUserToResizeColumns = true;
        dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDatos.MultiSelect = false;
        dgvDatos.ReadOnly = true;
        dgvDatos.RowHeadersVisible = false;
        dgvDatos.BackgroundColor = Color.White;
        dgvDatos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvDatos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colIdAtencion", DataPropertyName = "IdAtencion", HeaderText = "Atención", FillWeight = 70, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCliente", DataPropertyName = "Cliente", HeaderText = "Cliente", FillWeight = 160, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDominio", DataPropertyName = "Dominio", HeaderText = "Dominio", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSituacion", DataPropertyName = "Situacion", HeaderText = "Situación", FillWeight = 140, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", DataPropertyName = "Estado", HeaderText = "Etapa", FillWeight = 130, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotal", DataPropertyName = "Total", HeaderText = "Liquidado", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPagado", DataPropertyName = "Pagado", HeaderText = "Pagado", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSaldo", DataPropertyName = "Saldo", HeaderText = "Saldo", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        btnGestionar.Text = "Gestionar atención y cobros";
        btnGestionar.AutoSize = true;
        btnGestionar.Height = 34;
        btnGestionar.Click += Gestionar;
        flpAcciones.Controls.Add(btnGestionar);
        btnActualizar.Text = "Actualizar";
        btnActualizar.AutoSize = true;
        btnActualizar.Height = 34;
        btnActualizar.Click += Actualizar;
        flpAcciones.Controls.Add(btnActualizar);

        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn{Name="colFecha",DataPropertyName="Fecha",HeaderText="Último movimiento",FillWeight=110,DefaultCellStyle=new DataGridViewCellStyle{Format="dd/MM/yyyy HH:mm"}});
        Load += Cargar;
        FormClosing += AlCerrar;
        BackColor=Color.FromArgb(248,250,252);
        btnGestionar.BackColor=Color.FromArgb(30,64,175);btnGestionar.ForeColor=Color.White;btnGestionar.FlatStyle=FlatStyle.Flat;
        ResumeLayout(false);
    }
    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblTitulo = null!;
    private TextBox txtBuscar = null!;
    private ComboBox cboEstado = null!;
    private DataGridView dgvDatos = null!;
    private Label lblAyuda = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private Button btnGestionar = null!;
    private Button btnActualizar = null!;
}

#nullable enable
namespace Taller.Presentacion.Formularios.Presupuestos;
partial class FrmPresupuestos
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
        btnDecidir = new Button();
        btnRevision = new Button();
        btnVer = new Button();
        btnVer.Name = "btnVer";
        btnPresentar = new Button();
        btnPresentar.Name = "btnPresentar";
        btnActualizar = new Button();
        btnActualizar.Name = "btnActualizar";
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
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 86));
        btnDecidir.Name = "btnDecidir";
        btnDecidir.Text = "Registrar decisión";
        btnDecidir.AutoSize = true;
        btnDecidir.Height = 34;
        btnDecidir.Click += Decidir;
        btnRevision.Name = "btnRevision";
        btnRevision.Text = "Solicitar revisión";
        btnRevision.AutoSize = true;
        btnRevision.Height = 34;
        btnRevision.Click += Revision;
        flpAcciones.Controls.Add(btnDecidir);
        flpAcciones.Controls.Add(btnRevision);
        Controls.Add(tlpPrincipal);
        Text = "Presupuestos";
        tlpPrincipal.Controls.Add(lblTitulo, 0, 0);
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Text = "Presupuestos · propuestas y servicios iniciales";
        tlpPrincipal.Controls.Add(txtBuscar, 0, 1);
        txtBuscar.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(cboEstado, 0, 2);
        cboEstado.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(dgvDatos, 0, 3);
        dgvDatos.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(lblAyuda, 0, 4);
        lblAyuda.Dock = DockStyle.Fill;
        lblAyuda.Text = "Pendientes de presentación primero. Borradores se continúan desde Evaluaciones técnicas. La autorización inicial no implica ejecución ni pago.";
        tlpPrincipal.Controls.Add(flpAcciones, 0, 5);
        flpAcciones.Dock = DockStyle.Fill;
        txtBuscar.PlaceholderText = "Buscar presupuesto, atención, cliente o dominio";
        txtBuscar.TextChanged += Filtrar;
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.Items.AddRange(new object[] { "Todos", "Por presentar", "Borradores", "Esperando decisión", "Revisión técnica de selección" });
        cboEstado.SelectedIndexChanged += Filtrar;
        dgvDatos.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colVigente", DataPropertyName = "Vigente", HeaderText = "Vigente", FillWeight = 60, ReadOnly = true });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAceptado", DataPropertyName = "TotalAceptado", HeaderText = "Aceptado", FillWeight = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        dgvDatos.SelectionChanged += Seleccion;
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
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colIdPresupuesto", DataPropertyName = "IdPresupuesto", HeaderText = "N.º", FillWeight = 60, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colIdAtencion", DataPropertyName = "IdAtencion", HeaderText = "Atención", FillWeight = 60, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCliente", DataPropertyName = "Cliente", HeaderText = "Cliente", FillWeight = 150, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colVehiculo", DataPropertyName = "Vehiculo", HeaderText = "Vehículo", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTipo", DataPropertyName = "TipoDescripcion", HeaderText = "Tipo", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", DataPropertyName = "EstadoDescripcion", HeaderText = "Estado", FillWeight = 150, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", DataPropertyName = "Fecha", HeaderText = "Último movimiento", FillWeight = 115, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotal", DataPropertyName = "Total", HeaderText = "Total", FillWeight = 90, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        btnValidarSeleccion = new Button
        {
            Name = "btnValidarSeleccion",
            Text = "Revisar selección parcial",
            AutoSize = true,
            Height = 34
        };
        btnValidarSeleccion.Click += ValidarSeleccion;
        flpAcciones.Controls.Add(btnValidarSeleccion);
        btnSeguimiento = new Button
        {
            Name = "btnSeguimiento",
            Text = "Seguimiento y hallazgos",
            AutoSize = true,
            Height = 34
        };
        btnSeguimiento.Click += Seguimiento;
        flpAcciones.Controls.Add(btnSeguimiento);
        flpAcciones.AutoScroll = true;
        btnVer.Text = "Ver documento";
        btnVer.AutoSize = true;
        btnVer.Height = 34;
        btnVer.Click += Ver;
        flpAcciones.Controls.Add(btnVer);
        btnPresentar.Text = "Registrar comunicación al cliente";
        btnPresentar.AutoSize = true;
        btnPresentar.Height = 34;
        btnPresentar.Click += Presentar;
        flpAcciones.Controls.Add(btnPresentar);
        btnActualizar.Text = "Actualizar";
        btnActualizar.AutoSize = true;
        btnActualizar.Height = 34;
        btnActualizar.Click += Actualizar;
        flpAcciones.Controls.Add(btnActualizar);
        Load += Cargar;
        FormClosing += AlCerrar;
        BackColor = Color.FromArgb(248, 250, 252);
        btnPresentar.BackColor = Color.FromArgb(30, 64, 175);
        btnPresentar.ForeColor = Color.White;
        btnPresentar.FlatStyle = FlatStyle.Flat;
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblTitulo = null!;
    private TextBox txtBuscar = null!;
    private ComboBox cboEstado = null!;
    private DataGridView dgvDatos = null!;
    private Label lblAyuda = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private Button btnDecidir = null!;
    private Button btnRevision = null!;
    private Button btnValidarSeleccion = null!;
    private Button btnSeguimiento = null!;
    private Button btnVer = null!;
    private Button btnPresentar = null!;
    private Button btnActualizar = null!;
}

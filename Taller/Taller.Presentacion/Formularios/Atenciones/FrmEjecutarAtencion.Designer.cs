#nullable enable
namespace Taller.Presentacion.Formularios.Atenciones;
partial class FrmEjecutarAtencion
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
        txtResumen = new TextBox();
        txtResumen.Name = "txtResumen";
        lblAviso = new Label();
        lblAviso.Name = "lblAviso";
        dgvItems = new DataGridView();
        dgvItems.Name = "dgvItems";
        txtObservaciones = new TextBox();
        txtObservaciones.Name = "txtObservaciones";
        flpAcciones = new FlowLayoutPanel();
        flpAcciones.Name = "flpAcciones";
        btnTomar = new Button();
        btnTomar.Name = "btnTomar";
        btnGuardar = new Button();
        btnGuardar.Name = "btnGuardar";
        btnLiberar = new Button();
        btnLiberar.Name = "btnLiberar";
        btnFinalizar = new Button();
        btnFinalizar.Name = "btnFinalizar";
        btnInterrumpir = new Button();
        btnInterrumpir.Name = "btnInterrumpir";
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
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 68));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 86));
        Controls.Add(tlpPrincipal);
        Text = "Ejecución de trabajos autorizados";
        tlpPrincipal.Controls.Add(txtResumen, 0, 0);
        txtResumen.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(lblAviso, 0, 1);
        lblAviso.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(dgvItems, 0, 2);
        dgvItems.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(txtObservaciones, 0, 3);
        txtObservaciones.Dock = DockStyle.Fill;
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
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescripcion", DataPropertyName = "Descripcion", HeaderText = "Trabajo autorizado", FillWeight = 200, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCantidadAutorizada", DataPropertyName = "CantidadAutorizada", HeaderText = "Autorizado", FillWeight = 80, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCantidadEjecutada", DataPropertyName = "CantidadEjecutada", HeaderText = "Realizado", FillWeight = 80, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colObservaciones", DataPropertyName = "Observaciones", HeaderText = "Motivo / registro técnico", FillWeight = 190, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvItems.ReadOnly = false;
        dgvItems.Columns[0].ReadOnly = true;
        dgvItems.Columns[1].ReadOnly = true;
        dgvItems.Columns.Insert(3, new DataGridViewComboBoxColumn { Name = "colEstado", DataPropertyName = "Estado", HeaderText = "Estado", DataSource = Enum.GetValues<Taller.Dominio.Enumeraciones.EstadoEjecucionDetalle>(), FillWeight = 95 });
        dgvItems.CellValueChanged += CeldaCambiada;
        dgvItems.DataError += ErrorCelda;
        txtObservaciones.Multiline = true;
        txtObservaciones.MaxLength = 1000;
        txtObservaciones.ScrollBars = ScrollBars.Vertical;
        txtObservaciones.PlaceholderText = "Revisión del alcance, resultado del trabajo o motivo de interrupción.";
        txtObservaciones.TextChanged += TextoCambiado;
        btnTomar.Text = "Iniciar ejecución";
        btnTomar.AutoSize = true;
        btnTomar.Height = 34;
        btnTomar.Click += Tomar;
        flpAcciones.Controls.Add(btnTomar);
        btnGuardar.Text = "Guardar avance";
        btnGuardar.AutoSize = true;
        btnGuardar.Height = 34;
        btnGuardar.Click += Guardar;
        flpAcciones.Controls.Add(btnGuardar);
        btnCompletar = new Button { Name = "btnCompletarSeleccionada", Text = "Completar fila seleccionada", AutoSize = true, FlatStyle = FlatStyle.Flat };
        btnCompletar.Click += CompletarSeleccionada;
        flpAcciones.Controls.Add(btnCompletar);
        btnLiberar.Text = "Liberar actividad";
        btnLiberar.AutoSize = true;
        btnLiberar.Height = 34;
        btnLiberar.Click += Liberar;
        flpAcciones.Controls.Add(btnLiberar);
        btnFinalizar.Text = "Finalizar trabajo";
        btnFinalizar.AutoSize = true;
        btnFinalizar.Height = 34;
        btnFinalizar.Click += Finalizar;
        flpAcciones.Controls.Add(btnFinalizar);
        btnInterrumpir.Text = "Terminar con interrupción";
        btnInterrumpir.AutoSize = true;
        btnInterrumpir.Height = 34;
        btnInterrumpir.Click += Interrumpir;
        flpAcciones.Controls.Add(btnInterrumpir);
        btnSeguimiento = new Button
        {
            Name = "btnSeguimiento",
            Text = "Hallazgos, pausas y materiales",
            AutoSize = true,
            Height = 34
        };
        btnSeguimiento.Click += Seguimiento;
        chkConformidad = new CheckBox { Name = "chkConformidadTecnica", Text = "Comprobé el resultado y doy conformidad técnica", AutoSize = true };
        flpAcciones.Controls.Add(chkConformidad);
        flpAcciones.AutoSize = true;
        flpAcciones.WrapContents = true;
        tlpPrincipal.RowStyles[4].SizeType = SizeType.AutoSize;
        flpAcciones.Controls.Add(btnSeguimiento);
        btnCerrar.Text = "Cerrar";
        btnCerrar.AutoSize = true;
        btnCerrar.Height = 34;
        btnCerrar.Click += Cerrar;
        flpAcciones.Controls.Add(btnCerrar);
        Load += Cargar;
        FormClosing += AlCerrar;
        BackColor = Color.FromArgb(248, 250, 252);
        btnFinalizar.BackColor = Color.FromArgb(30, 64, 175);
        btnFinalizar.ForeColor = Color.White;
        btnFinalizar.FlatStyle = FlatStyle.Flat;
        CancelButton = btnCerrar;
        btnCerrar.DialogResult = DialogResult.None;
        txtResumen.ReadOnly = true;
        txtResumen.Multiline = true;
        txtResumen.ScrollBars = ScrollBars.Vertical;
        txtResumen.TabStop = false;
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpPrincipal = null!;
    private TextBox txtResumen = null!;
    private Label lblAviso = null!;
    private DataGridView dgvItems = null!;
    private TextBox txtObservaciones = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private CheckBox chkConformidad = null!;
    private Button btnCompletar = null!;
    private Button btnTomar = null!;
    private Button btnGuardar = null!;
    private Button btnLiberar = null!;
    private Button btnFinalizar = null!;
    private Button btnInterrumpir = null!;
    private Button btnSeguimiento = null!;
    private Button btnCerrar = null!;
}

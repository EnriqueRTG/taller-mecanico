namespace Taller.Presentacion.Formularios.Diagnosticos;

partial class FrmDiagnosticos
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tlpPrincipal = new TableLayoutPanel();
        pnlCabecera = new Panel();
        btnRegistrar = new Button();
        lblResumen = new Label();
        lblDescripcion = new Label();
        lblTitulo = new Label();
        pnlFiltros = new Panel();
        tlpFiltros = new TableLayoutPanel();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        lblTecnico = new Label();
        cboTecnico = new ComboBox();
        lblEspacio = new Label();
        btnLimpiar = new Button();
        pnlGrilla = new Panel();
        dgvDiagnosticos = new DataGridView();
        colAtencion = new DataGridViewTextBoxColumn();
        colFecha = new DataGridViewTextBoxColumn();
        colCliente = new DataGridViewTextBoxColumn();
        colVehiculo = new DataGridViewTextBoxColumn();
        colMotivo = new DataGridViewTextBoxColumn();
        colTecnico = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlPie = new Panel();
        flpAcciones = new FlowLayoutPanel();
        btnVerAtencion = new Button();
        btnRegistrarSeleccionado = new Button();
        btnActualizar = new Button();
        lblCantidad = new Label();
        pnlInformacion = new Panel();
        lblInformacion = new Label();
        tlpPrincipal.SuspendLayout();
        pnlCabecera.SuspendLayout();
        pnlFiltros.SuspendLayout();
        tlpFiltros.SuspendLayout();
        pnlGrilla.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDiagnosticos).BeginInit();
        pnlPie.SuspendLayout();
        flpAcciones.SuspendLayout();
        pnlInformacion.SuspendLayout();
        SuspendLayout();
        // principal
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
        tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
        tlpPrincipal.Controls.Add(pnlGrilla, 0, 2);
        tlpPrincipal.Controls.Add(pnlPie, 0, 3);
        tlpPrincipal.Controls.Add(pnlInformacion, 0, 4);
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.RowCount = 5;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        // cabecera
        pnlCabecera.BackColor = Color.White;
        pnlCabecera.Controls.Add(btnRegistrar);
        pnlCabecera.Controls.Add(lblResumen);
        pnlCabecera.Controls.Add(lblDescripcion);
        pnlCabecera.Controls.Add(lblTitulo);
        pnlCabecera.Dock = DockStyle.Fill;
        pnlCabecera.Margin = new Padding(0, 0, 0, 10);
        btnRegistrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRegistrar.Location = new Point(754, 12);
        btnRegistrar.Size = new Size(180, 40);
        btnRegistrar.Text = "＋ Registrar diagnóstico";
        btnRegistrar.Click += BtnRegistrar_Click;
        lblResumen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblResumen.ForeColor = Color.FromArgb(71, 85, 105);
        lblResumen.Location = new Point(510, 21);
        lblResumen.Size = new Size(225, 22);
        lblResumen.TextAlign = ContentAlignment.MiddleRight;
        lblDescripcion.AutoSize = true;
        lblDescripcion.Font = new Font("Segoe UI", 9.5F);
        lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
        lblDescripcion.Location = new Point(20, 43);
        lblDescripcion.Text = "Consulta y registro de diagnósticos asociados a las atenciones.";
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitulo.Location = new Point(18, 8);
        lblTitulo.Text = "Gestión de diagnósticos";
        // filtros
        pnlFiltros.BackColor = Color.White;
        pnlFiltros.Controls.Add(tlpFiltros);
        pnlFiltros.Dock = DockStyle.Fill;
        pnlFiltros.Margin = new Padding(0, 0, 0, 10);
        pnlFiltros.Padding = new Padding(14, 8, 14, 8);
        tlpFiltros.ColumnCount = 4;
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        tlpFiltros.Controls.Add(lblBuscar, 0, 0);
        tlpFiltros.Controls.Add(txtBuscar, 0, 1);
        tlpFiltros.Controls.Add(lblEstado, 1, 0);
        tlpFiltros.Controls.Add(cboEstado, 1, 1);
        tlpFiltros.Controls.Add(lblTecnico, 2, 0);
        tlpFiltros.Controls.Add(cboTecnico, 2, 1);
        tlpFiltros.Controls.Add(lblEspacio, 3, 0);
        tlpFiltros.Controls.Add(btnLimpiar, 3, 1);
        tlpFiltros.Dock = DockStyle.Fill;
        tlpFiltros.RowCount = 2;
        tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        ConfigurarEtiqueta(lblBuscar, "Buscar");
        ConfigurarEtiqueta(lblEstado, "Estado");
        ConfigurarEtiqueta(lblTecnico, "Técnico");
        ConfigurarFiltro(txtBuscar);
        txtBuscar.PlaceholderText = "Atención, dominio, cliente o motivo";
        txtBuscar.TextChanged += Filtros_Changed;
        ConfigurarFiltro(cboEstado);
        ConfigurarFiltro(cboTecnico);
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTecnico.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.SelectedIndexChanged += Filtros_Changed;
        cboTecnico.SelectedIndexChanged += Filtros_Changed;
        btnLimpiar.Dock = DockStyle.Fill;
        btnLimpiar.Margin = new Padding(5, 0, 5, 5);
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.Click += BtnLimpiar_Click;
        // grilla
        pnlGrilla.BackColor = Color.White;
        pnlGrilla.Controls.Add(dgvDiagnosticos);
        pnlGrilla.Dock = DockStyle.Fill;
        pnlGrilla.Margin = new Padding(0, 0, 0, 10);
        pnlGrilla.Padding = new Padding(14);
        dgvDiagnosticos.Columns.AddRange(colAtencion, colFecha, colCliente, colVehiculo, colMotivo, colTecnico, colEstado);
        dgvDiagnosticos.Dock = DockStyle.Fill;
        dgvDiagnosticos.SelectionChanged += DgvDiagnosticos_SelectionChanged;
        dgvDiagnosticos.CellDoubleClick += DgvDiagnosticos_CellDoubleClick;
        ConfigurarColumna(colAtencion, "Atención", 90);
        ConfigurarColumna(colFecha, "Ingreso", 90);
        ConfigurarColumna(colCliente, "Cliente", 145);
        ConfigurarColumna(colVehiculo, "Vehículo", 170);
        ConfigurarColumna(colMotivo, "Motivo", 220);
        ConfigurarColumna(colTecnico, "Técnico", 125);
        ConfigurarColumna(colEstado, "Diagnóstico", 120);
        colMotivo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        // pie
        pnlPie.BackColor = Color.White;
        pnlPie.Controls.Add(flpAcciones);
        pnlPie.Controls.Add(lblCantidad);
        pnlPie.Dock = DockStyle.Fill;
        pnlPie.Margin = new Padding(0, 0, 0, 10);
        pnlPie.Padding = new Padding(12, 8, 12, 8);
        flpAcciones.Controls.Add(btnVerAtencion);
        flpAcciones.Controls.Add(btnRegistrarSeleccionado);
        flpAcciones.Controls.Add(btnActualizar);
        flpAcciones.Dock = DockStyle.Left;
        flpAcciones.Size = new Size(520, 40);
        ConfigurarBoton(btnVerAtencion, "Ver atención", 120);
        ConfigurarBoton(btnRegistrarSeleccionado, "Registrar diagnóstico", 160);
        ConfigurarBoton(btnActualizar, "Actualizar", 105);
        btnVerAtencion.Click += BtnVerAtencion_Click;
        btnRegistrarSeleccionado.Click += BtnRegistrar_Click;
        btnActualizar.Click += BtnActualizar_Click;
        lblCantidad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCantidad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblCantidad.ForeColor = Color.FromArgb(71, 85, 105);
        lblCantidad.Location = new Point(742, 16);
        lblCantidad.Size = new Size(190, 24);
        lblCantidad.TextAlign = ContentAlignment.MiddleRight;
        // información
        pnlInformacion.BackColor = Color.FromArgb(239, 246, 255);
        pnlInformacion.Controls.Add(lblInformacion);
        pnlInformacion.Dock = DockStyle.Fill;
        lblInformacion.Dock = DockStyle.Fill;
        lblInformacion.Font = new Font("Segoe UI", 9F);
        lblInformacion.ForeColor = Color.FromArgb(30, 64, 175);
        lblInformacion.Padding = new Padding(16, 0, 12, 0);
        lblInformacion.Text = "El diagnóstico se registra desde una atención asignada al técnico.";
        lblInformacion.TextAlign = ContentAlignment.MiddleLeft;
        // formulario
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(tlpPrincipal);
        FormBorderStyle = FormBorderStyle.None;
        MinimumSize = new Size(760, 500);
        Padding = new Padding(24, 20, 24, 20);
        Text = "Gestión de diagnósticos";
        Load += FrmDiagnosticos_Load;
        tlpPrincipal.ResumeLayout(false);
        pnlCabecera.ResumeLayout(false);
        pnlCabecera.PerformLayout();
        pnlFiltros.ResumeLayout(false);
        tlpFiltros.ResumeLayout(false);
        tlpFiltros.PerformLayout();
        pnlGrilla.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvDiagnosticos).EndInit();
        pnlPie.ResumeLayout(false);
        flpAcciones.ResumeLayout(false);
        pnlInformacion.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigurarEtiqueta(Label c, string texto)
    { c.Dock = DockStyle.Fill; c.Font = new Font("Segoe UI", 9F, FontStyle.Bold); c.Margin = new Padding(5, 0, 5, 0); c.Text = texto; c.TextAlign = ContentAlignment.BottomLeft; }
    private static void ConfigurarFiltro(Control c)
    { c.Dock = DockStyle.Fill; c.Font = new Font("Segoe UI", 9.75F); c.Margin = new Padding(5, 0, 5, 5); }
    private static void ConfigurarColumna(DataGridViewColumn c, string texto, int ancho)
    { c.HeaderText = texto; c.Width = ancho; }
    private static void ConfigurarBoton(Button c, string texto, int ancho)
    { c.Margin = new Padding(3); c.Size = new Size(ancho, 34); c.Text = texto; }

    private TableLayoutPanel tlpPrincipal;
    private Panel pnlCabecera;
    private Button btnRegistrar;
    private Label lblResumen;
    private Label lblDescripcion;
    private Label lblTitulo;
    private Panel pnlFiltros;
    private TableLayoutPanel tlpFiltros;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Label lblEstado;
    private ComboBox cboEstado;
    private Label lblTecnico;
    private ComboBox cboTecnico;
    private Label lblEspacio;
    private Button btnLimpiar;
    private Panel pnlGrilla;
    private DataGridView dgvDiagnosticos;
    private DataGridViewTextBoxColumn colAtencion;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colCliente;
    private DataGridViewTextBoxColumn colVehiculo;
    private DataGridViewTextBoxColumn colMotivo;
    private DataGridViewTextBoxColumn colTecnico;
    private DataGridViewTextBoxColumn colEstado;
    private Panel pnlPie;
    private FlowLayoutPanel flpAcciones;
    private Button btnVerAtencion;
    private Button btnRegistrarSeleccionado;
    private Button btnActualizar;
    private Label lblCantidad;
    private Panel pnlInformacion;
    private Label lblInformacion;
}

namespace Taller.Presentacion.Formularios.Comprobantes;

partial class FrmComprobantes
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        tlpPrincipal = new TableLayoutPanel();
        pnlCabecera = new Panel();
        btnGestionar = new Button();
        lblResumen = new Label();
        lblDescripcion = new Label();
        lblTitulo = new Label();
        pnlFiltros = new Panel();
        tlpFiltros = new TableLayoutPanel();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        lblSaldo = new Label();
        cboSaldo = new ComboBox();
        lblEspacio = new Label();
        btnLimpiar = new Button();
        pnlGrilla = new Panel();
        dgvComprobantes = new DataGridView();
        colComprobante = new DataGridViewTextBoxColumn();
        colAtencion = new DataGridViewTextBoxColumn();
        colCliente = new DataGridViewTextBoxColumn();
        colVehiculo = new DataGridViewTextBoxColumn();
        colTotal = new DataGridViewTextBoxColumn();
        colAbonado = new DataGridViewTextBoxColumn();
        colSaldo = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlPie = new Panel();
        flpAcciones = new FlowLayoutPanel();
        btnVerPagos = new Button();
        btnRegistrarEntrega = new Button();
        btnActualizar = new Button();
        lblCantidad = new Label();
        pnlInformacion = new Panel();
        lblInformacion = new Label();
        tlpPrincipal.SuspendLayout(); pnlCabecera.SuspendLayout(); pnlFiltros.SuspendLayout(); tlpFiltros.SuspendLayout();
        pnlGrilla.SuspendLayout(); ((System.ComponentModel.ISupportInitialize)dgvComprobantes).BeginInit();
        pnlPie.SuspendLayout(); flpAcciones.SuspendLayout(); pnlInformacion.SuspendLayout(); SuspendLayout();
        // principal
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.Controls.Add(pnlCabecera, 0, 0); tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
        tlpPrincipal.Controls.Add(pnlGrilla, 0, 2); tlpPrincipal.Controls.Add(pnlPie, 0, 3); tlpPrincipal.Controls.Add(pnlInformacion, 0, 4);
        tlpPrincipal.Dock = DockStyle.Fill; tlpPrincipal.RowCount = 5;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        // cabecera
        pnlCabecera.BackColor = Color.White; pnlCabecera.Dock = DockStyle.Fill; pnlCabecera.Margin = new Padding(0, 0, 0, 10);
        pnlCabecera.Controls.Add(btnGestionar); pnlCabecera.Controls.Add(lblResumen); pnlCabecera.Controls.Add(lblDescripcion); pnlCabecera.Controls.Add(lblTitulo);
        btnGestionar.Anchor = AnchorStyles.Top | AnchorStyles.Right; btnGestionar.Location = new Point(754, 12); btnGestionar.Size = new Size(180, 40);
        btnGestionar.Text = "Gestionar comprobante"; btnGestionar.Click += BtnGestionar_Click;
        lblResumen.Anchor = AnchorStyles.Top | AnchorStyles.Right; lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblResumen.ForeColor = Color.FromArgb(71, 85, 105); lblResumen.Location = new Point(510, 21); lblResumen.Size = new Size(225, 22); lblResumen.TextAlign = ContentAlignment.MiddleRight;
        lblDescripcion.AutoSize = true; lblDescripcion.Font = new Font("Segoe UI", 9.5F); lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
        lblDescripcion.Location = new Point(20, 43); lblDescripcion.Text = "Emisión, pagos parciales, saldos y entrega de vehículos.";
        lblTitulo.AutoSize = true; lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold); lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitulo.Location = new Point(18, 8); lblTitulo.Text = "Comprobantes y pagos";
        // filtros
        pnlFiltros.BackColor = Color.White; pnlFiltros.Controls.Add(tlpFiltros); pnlFiltros.Dock = DockStyle.Fill; pnlFiltros.Margin = new Padding(0, 0, 0, 10); pnlFiltros.Padding = new Padding(14, 8, 14, 8);
        tlpFiltros.ColumnCount = 4; tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F)); tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F)); tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F)); tlpFiltros.Dock = DockStyle.Fill;
        tlpFiltros.RowCount = 2; tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F)); tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpFiltros.Controls.Add(lblBuscar, 0, 0); tlpFiltros.Controls.Add(txtBuscar, 0, 1); tlpFiltros.Controls.Add(lblEstado, 1, 0); tlpFiltros.Controls.Add(cboEstado, 1, 1);
        tlpFiltros.Controls.Add(lblSaldo, 2, 0); tlpFiltros.Controls.Add(cboSaldo, 2, 1); tlpFiltros.Controls.Add(lblEspacio, 3, 0); tlpFiltros.Controls.Add(btnLimpiar, 3, 1);
        ConfigurarEtiqueta(lblBuscar, "Buscar"); ConfigurarEtiqueta(lblEstado, "Estado"); ConfigurarEtiqueta(lblSaldo, "Situación del saldo");
        ConfigurarFiltro(txtBuscar); txtBuscar.PlaceholderText = "Comprobante, atención, cliente o dominio"; txtBuscar.TextChanged += Filtros_Changed;
        ConfigurarFiltro(cboEstado); ConfigurarFiltro(cboSaldo); cboEstado.DropDownStyle = ComboBoxStyle.DropDownList; cboSaldo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.SelectedIndexChanged += Filtros_Changed; cboSaldo.SelectedIndexChanged += Filtros_Changed;
        btnLimpiar.Dock = DockStyle.Fill; btnLimpiar.Margin = new Padding(5, 0, 5, 5); btnLimpiar.Text = "Limpiar"; btnLimpiar.Click += BtnLimpiar_Click;
        // grilla
        pnlGrilla.BackColor = Color.White; pnlGrilla.Controls.Add(dgvComprobantes); pnlGrilla.Dock = DockStyle.Fill; pnlGrilla.Margin = new Padding(0, 0, 0, 10); pnlGrilla.Padding = new Padding(14);
        dgvComprobantes.Columns.AddRange(colComprobante, colAtencion, colCliente, colVehiculo, colTotal, colAbonado, colSaldo, colEstado);
        dgvComprobantes.Dock = DockStyle.Fill; dgvComprobantes.SelectionChanged += DgvComprobantes_SelectionChanged; dgvComprobantes.CellDoubleClick += DgvComprobantes_CellDoubleClick;
        ConfigurarColumna(colComprobante, "Comprobante", 105); ConfigurarColumna(colAtencion, "Atención", 85); ConfigurarColumna(colCliente, "Cliente", 145);
        ConfigurarColumna(colVehiculo, "Vehículo", 165); ConfigurarColumna(colTotal, "Total", 105); ConfigurarColumna(colAbonado, "Abonado", 105);
        ConfigurarColumna(colSaldo, "Saldo", 105); ConfigurarColumna(colEstado, "Estado", 120); colCliente.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; colVehiculo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        // pie
        pnlPie.BackColor = Color.White; pnlPie.Controls.Add(flpAcciones); pnlPie.Controls.Add(lblCantidad); pnlPie.Dock = DockStyle.Fill; pnlPie.Margin = new Padding(0, 0, 0, 10); pnlPie.Padding = new Padding(12, 8, 12, 8);
        flpAcciones.Controls.Add(btnVerPagos); flpAcciones.Controls.Add(btnRegistrarEntrega); flpAcciones.Controls.Add(btnActualizar); flpAcciones.Dock = DockStyle.Left; flpAcciones.Size = new Size(500, 40);
        ConfigurarBoton(btnVerPagos, "Comprobante y pagos", 165); ConfigurarBoton(btnRegistrarEntrega, "Registrar entrega", 145); ConfigurarBoton(btnActualizar, "Actualizar", 105);
        btnVerPagos.Click += BtnGestionar_Click; btnRegistrarEntrega.Click += BtnRegistrarEntrega_Click; btnActualizar.Click += BtnActualizar_Click;
        lblCantidad.Anchor = AnchorStyles.Top | AnchorStyles.Right; lblCantidad.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblCantidad.ForeColor = Color.FromArgb(71, 85, 105);
        lblCantidad.Location = new Point(742, 16); lblCantidad.Size = new Size(190, 24); lblCantidad.TextAlign = ContentAlignment.MiddleRight;
        // información
        pnlInformacion.BackColor = Color.FromArgb(239, 246, 255); pnlInformacion.Controls.Add(lblInformacion); pnlInformacion.Dock = DockStyle.Fill;
        lblInformacion.Dock = DockStyle.Fill; lblInformacion.Font = new Font("Segoe UI", 9F); lblInformacion.ForeColor = Color.FromArgb(30, 64, 175);
        lblInformacion.Padding = new Padding(16, 0, 12, 0); lblInformacion.Text = "Los pagos se administran dentro del comprobante; la entrega se habilita al cancelar el saldo."; lblInformacion.TextAlign = ContentAlignment.MiddleLeft;
        // formulario
        AutoScaleDimensions = new SizeF(7F, 15F); AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(245, 247, 250); ClientSize = new Size(1000, 650);
        Controls.Add(tlpPrincipal); FormBorderStyle = FormBorderStyle.None; MinimumSize = new Size(760, 500); Padding = new Padding(24, 20, 24, 20); Text = "Comprobantes y pagos";
        Load += FrmComprobantes_Load;
        tlpPrincipal.ResumeLayout(false); pnlCabecera.ResumeLayout(false); pnlCabecera.PerformLayout(); pnlFiltros.ResumeLayout(false); tlpFiltros.ResumeLayout(false); tlpFiltros.PerformLayout();
        pnlGrilla.ResumeLayout(false); ((System.ComponentModel.ISupportInitialize)dgvComprobantes).EndInit(); pnlPie.ResumeLayout(false); flpAcciones.ResumeLayout(false); pnlInformacion.ResumeLayout(false); ResumeLayout(false);
    }

    private static void ConfigurarEtiqueta(Label c, string texto) { c.Dock = DockStyle.Fill; c.Font = new Font("Segoe UI", 9F, FontStyle.Bold); c.Margin = new Padding(5, 0, 5, 0); c.Text = texto; c.TextAlign = ContentAlignment.BottomLeft; }
    private static void ConfigurarFiltro(Control c) { c.Dock = DockStyle.Fill; c.Font = new Font("Segoe UI", 9.75F); c.Margin = new Padding(5, 0, 5, 5); }
    private static void ConfigurarColumna(DataGridViewColumn c, string texto, int ancho) { c.HeaderText = texto; c.Width = ancho; }
    private static void ConfigurarBoton(Button c, string texto, int ancho) { c.Margin = new Padding(3); c.Size = new Size(ancho, 34); c.Text = texto; }

    private TableLayoutPanel tlpPrincipal; private Panel pnlCabecera; private Button btnGestionar; private Label lblResumen; private Label lblDescripcion; private Label lblTitulo;
    private Panel pnlFiltros; private TableLayoutPanel tlpFiltros; private Label lblBuscar; private TextBox txtBuscar; private Label lblEstado; private ComboBox cboEstado;
    private Label lblSaldo; private ComboBox cboSaldo; private Label lblEspacio; private Button btnLimpiar; private Panel pnlGrilla; private DataGridView dgvComprobantes;
    private DataGridViewTextBoxColumn colComprobante; private DataGridViewTextBoxColumn colAtencion; private DataGridViewTextBoxColumn colCliente; private DataGridViewTextBoxColumn colVehiculo;
    private DataGridViewTextBoxColumn colTotal; private DataGridViewTextBoxColumn colAbonado; private DataGridViewTextBoxColumn colSaldo; private DataGridViewTextBoxColumn colEstado;
    private Panel pnlPie; private FlowLayoutPanel flpAcciones; private Button btnVerPagos; private Button btnRegistrarEntrega; private Button btnActualizar; private Label lblCantidad;
    private Panel pnlInformacion; private Label lblInformacion;
}

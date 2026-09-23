namespace Taller.Presentacion.Formularios.Presupuestos;

partial class FrmPresupuestos
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        tlpPrincipal = new TableLayoutPanel();
        pnlCabecera = new Panel();
        lblResumen = new Label();
        btnGenerar = new Button();
        lblDescripcion = new Label();
        lblTitulo = new Label();
        pnlFiltros = new Panel();
        tlpFiltros = new TableLayoutPanel();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        lblDesde = new Label();
        dtpDesde = new DateTimePicker();
        lblHasta = new Label();
        dtpHasta = new DateTimePicker();
        lblEspacio = new Label();
        btnLimpiar = new Button();
        pnlGrilla = new Panel();
        dgvPresupuestos = new DataGridView();
        colNumero = new DataGridViewTextBoxColumn();
        colFecha = new DataGridViewTextBoxColumn();
        colAtencion = new DataGridViewTextBoxColumn();
        colCliente = new DataGridViewTextBoxColumn();
        colVehiculo = new DataGridViewTextBoxColumn();
        colTotal = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlPie = new Panel();
        tlpPie = new TableLayoutPanel();
        flpAcciones = new FlowLayoutPanel();
        btnVerDetalle = new Button();
        btnRegistrarDecision = new Button();
        btnActualizar = new Button();
        lblCantidad = new Label();
        pnlInformacion = new Panel();
        lblInformacion = new Label();
        pnlIndicador = new Panel();
        tlpPrincipal.SuspendLayout();
        pnlCabecera.SuspendLayout();
        pnlFiltros.SuspendLayout();
        tlpFiltros.SuspendLayout();
        pnlGrilla.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPresupuestos).BeginInit();
        pnlPie.SuspendLayout();
        tlpPie.SuspendLayout();
        flpAcciones.SuspendLayout();
        pnlInformacion.SuspendLayout();
        SuspendLayout();
        // tlpPrincipal
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
        tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
        tlpPrincipal.Controls.Add(pnlGrilla, 0, 2);
        tlpPrincipal.Controls.Add(pnlPie, 0, 3);
        tlpPrincipal.Controls.Add(pnlInformacion, 0, 4);
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Location = new Point(24, 20);
        tlpPrincipal.Margin = new Padding(0);
        tlpPrincipal.RowCount = 5;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tlpPrincipal.Size = new Size(952, 610);
        // pnlCabecera
        pnlCabecera.BackColor = Color.White;
        pnlCabecera.Controls.Add(lblResumen);
        pnlCabecera.Controls.Add(btnGenerar);
        pnlCabecera.Controls.Add(lblDescripcion);
        pnlCabecera.Controls.Add(lblTitulo);
        pnlCabecera.Dock = DockStyle.Fill;
        pnlCabecera.Margin = new Padding(0, 0, 0, 10);
        pnlCabecera.Padding = new Padding(18, 8, 18, 8);
        // lblResumen
        lblResumen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblResumen.ForeColor = Color.FromArgb(71, 85, 105);
        lblResumen.Location = new Point(510, 21);
        lblResumen.Size = new Size(230, 22);
        lblResumen.TextAlign = ContentAlignment.MiddleRight;
        // btnGenerar
        btnGenerar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGenerar.Location = new Point(758, 11);
        btnGenerar.Size = new Size(176, 40);
        btnGenerar.Text = "＋ Generar presupuesto";
        btnGenerar.Click += BtnGenerar_Click;
        // lblDescripcion
        lblDescripcion.AutoSize = true;
        lblDescripcion.Font = new Font("Segoe UI", 9.5F);
        lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
        lblDescripcion.Location = new Point(20, 42);
        lblDescripcion.Text = "Elaboración, consulta y decisión de presupuestos del taller.";
        // lblTitulo
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitulo.Location = new Point(18, 8);
        lblTitulo.Text = "Gestión de presupuestos";
        // pnlFiltros
        pnlFiltros.BackColor = Color.White;
        pnlFiltros.Controls.Add(tlpFiltros);
        pnlFiltros.Dock = DockStyle.Fill;
        pnlFiltros.Margin = new Padding(0, 0, 0, 10);
        pnlFiltros.Padding = new Padding(14, 8, 14, 8);
        // tlpFiltros
        tlpFiltros.ColumnCount = 5;
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        tlpFiltros.Controls.Add(lblBuscar, 0, 0);
        tlpFiltros.Controls.Add(txtBuscar, 0, 1);
        tlpFiltros.Controls.Add(lblEstado, 1, 0);
        tlpFiltros.Controls.Add(cboEstado, 1, 1);
        tlpFiltros.Controls.Add(lblDesde, 2, 0);
        tlpFiltros.Controls.Add(dtpDesde, 2, 1);
        tlpFiltros.Controls.Add(lblHasta, 3, 0);
        tlpFiltros.Controls.Add(dtpHasta, 3, 1);
        tlpFiltros.Controls.Add(lblEspacio, 4, 0);
        tlpFiltros.Controls.Add(btnLimpiar, 4, 1);
        tlpFiltros.Dock = DockStyle.Fill;
        tlpFiltros.RowCount = 2;
        tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // labels de filtros
        ConfigurarEtiquetaFiltro(lblBuscar, "Buscar");
        ConfigurarEtiquetaFiltro(lblEstado, "Estado");
        ConfigurarEtiquetaFiltro(lblDesde, "Desde");
        ConfigurarEtiquetaFiltro(lblHasta, "Hasta");
        // txtBuscar
        txtBuscar.Dock = DockStyle.Fill;
        txtBuscar.Font = new Font("Segoe UI", 9.75F);
        txtBuscar.Margin = new Padding(5, 0, 5, 5);
        txtBuscar.PlaceholderText = "N.º, atención, dominio o cliente";
        txtBuscar.TextChanged += Filtros_Changed;
        // cboEstado
        cboEstado.Dock = DockStyle.Fill;
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.Font = new Font("Segoe UI", 9.75F);
        cboEstado.Margin = new Padding(5, 0, 5, 5);
        cboEstado.SelectedIndexChanged += Filtros_Changed;
        // fechas
        ConfigurarFecha(dtpDesde);
        ConfigurarFecha(dtpHasta);
        dtpDesde.ValueChanged += Filtros_Changed;
        dtpHasta.ValueChanged += Filtros_Changed;
        // btnLimpiar
        btnLimpiar.Dock = DockStyle.Fill;
        btnLimpiar.Margin = new Padding(5, 0, 5, 5);
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.Click += BtnLimpiar_Click;
        // pnlGrilla
        pnlGrilla.BackColor = Color.White;
        pnlGrilla.Controls.Add(dgvPresupuestos);
        pnlGrilla.Dock = DockStyle.Fill;
        pnlGrilla.Margin = new Padding(0, 0, 0, 10);
        pnlGrilla.Padding = new Padding(14);
        // dgvPresupuestos
        dgvPresupuestos.Columns.AddRange(colNumero, colFecha, colAtencion, colCliente, colVehiculo, colTotal, colEstado);
        dgvPresupuestos.Dock = DockStyle.Fill;
        dgvPresupuestos.SelectionChanged += DgvPresupuestos_SelectionChanged;
        dgvPresupuestos.CellDoubleClick += DgvPresupuestos_CellDoubleClick;
        ConfigurarColumna(colNumero, "Presupuesto", 105);
        ConfigurarColumna(colFecha, "Fecha", 90);
        ConfigurarColumna(colAtencion, "Atención", 85);
        ConfigurarColumna(colCliente, "Cliente", 155);
        ConfigurarColumna(colVehiculo, "Vehículo", 190);
        ConfigurarColumna(colTotal, "Total", 105);
        ConfigurarColumna(colEstado, "Estado", 155);
        colCliente.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colVehiculo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        // pnlPie
        pnlPie.BackColor = Color.White;
        pnlPie.Controls.Add(tlpPie);
        pnlPie.Dock = DockStyle.Fill;
        pnlPie.Margin = new Padding(0, 0, 0, 10);
        pnlPie.Padding = new Padding(12, 8, 12, 8);
        // tlpPie
        tlpPie.ColumnCount = 2;
        tlpPie.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPie.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
        tlpPie.Controls.Add(flpAcciones, 0, 0);
        tlpPie.Controls.Add(lblCantidad, 1, 0);
        tlpPie.Dock = DockStyle.Fill;
        // flpAcciones
        flpAcciones.Controls.Add(btnVerDetalle);
        flpAcciones.Controls.Add(btnRegistrarDecision);
        flpAcciones.Controls.Add(btnActualizar);
        flpAcciones.Dock = DockStyle.Fill;
        ConfigurarBotonAccion(btnVerDetalle, "Ver detalle", 120);
        ConfigurarBotonAccion(btnRegistrarDecision, "Registrar decisión", 150);
        ConfigurarBotonAccion(btnActualizar, "Actualizar", 105);
        btnVerDetalle.Click += BtnVerDetalle_Click;
        btnRegistrarDecision.Click += BtnRegistrarDecision_Click;
        btnActualizar.Click += BtnActualizar_Click;
        // lblCantidad
        lblCantidad.Dock = DockStyle.Fill;
        lblCantidad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblCantidad.ForeColor = Color.FromArgb(71, 85, 105);
        lblCantidad.TextAlign = ContentAlignment.MiddleRight;
        // pnlInformacion
        pnlInformacion.BackColor = Color.FromArgb(239, 246, 255);
        pnlInformacion.Controls.Add(lblInformacion);
        pnlInformacion.Controls.Add(pnlIndicador);
        pnlInformacion.Dock = DockStyle.Fill;
        // lblInformacion
        lblInformacion.Dock = DockStyle.Fill;
        lblInformacion.Font = new Font("Segoe UI", 9F);
        lblInformacion.ForeColor = Color.FromArgb(30, 64, 175);
        lblInformacion.Padding = new Padding(12, 0, 12, 0);
        lblInformacion.Text = "Seleccione un presupuesto para consultar su detalle o registrar la decisión informada por el cliente.";
        lblInformacion.TextAlign = ContentAlignment.MiddleLeft;
        // pnlIndicador
        pnlIndicador.BackColor = Color.FromArgb(30, 64, 175);
        pnlIndicador.Dock = DockStyle.Left;
        pnlIndicador.Size = new Size(5, 48);
        // FrmPresupuestos
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(tlpPrincipal);
        FormBorderStyle = FormBorderStyle.None;
        MinimumSize = new Size(760, 500);
        Padding = new Padding(24, 20, 24, 20);
        Text = "Gestión de presupuestos";
        Load += FrmPresupuestos_Load;
        tlpPrincipal.ResumeLayout(false);
        pnlCabecera.ResumeLayout(false);
        pnlCabecera.PerformLayout();
        pnlFiltros.ResumeLayout(false);
        tlpFiltros.ResumeLayout(false);
        tlpFiltros.PerformLayout();
        pnlGrilla.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvPresupuestos).EndInit();
        pnlPie.ResumeLayout(false);
        tlpPie.ResumeLayout(false);
        flpAcciones.ResumeLayout(false);
        pnlInformacion.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigurarEtiquetaFiltro(Label etiqueta, string texto)
    {
        etiqueta.Dock = DockStyle.Fill;
        etiqueta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        etiqueta.Margin = new Padding(5, 0, 5, 0);
        etiqueta.Text = texto;
        etiqueta.TextAlign = ContentAlignment.BottomLeft;
    }

    private static void ConfigurarFecha(DateTimePicker selector)
    {
        selector.Dock = DockStyle.Fill;
        selector.Format = DateTimePickerFormat.Short;
        selector.Margin = new Padding(5, 0, 5, 5);
    }

    private static void ConfigurarColumna(DataGridViewColumn columna, string encabezado, int ancho)
    {
        columna.HeaderText = encabezado;
        columna.MinimumWidth = Math.Min(ancho, 90);
        columna.Width = ancho;
    }

    private static void ConfigurarBotonAccion(Button boton, string texto, int ancho)
    {
        boton.Margin = new Padding(3);
        boton.Size = new Size(ancho, 34);
        boton.Text = texto;
    }

    #endregion

    private TableLayoutPanel tlpPrincipal;
    private Panel pnlCabecera;
    private Label lblResumen;
    private Button btnGenerar;
    private Label lblDescripcion;
    private Label lblTitulo;
    private Panel pnlFiltros;
    private TableLayoutPanel tlpFiltros;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Label lblEstado;
    private ComboBox cboEstado;
    private Label lblDesde;
    private DateTimePicker dtpDesde;
    private Label lblHasta;
    private DateTimePicker dtpHasta;
    private Label lblEspacio;
    private Button btnLimpiar;
    private Panel pnlGrilla;
    private DataGridView dgvPresupuestos;
    private DataGridViewTextBoxColumn colNumero;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colAtencion;
    private DataGridViewTextBoxColumn colCliente;
    private DataGridViewTextBoxColumn colVehiculo;
    private DataGridViewTextBoxColumn colTotal;
    private DataGridViewTextBoxColumn colEstado;
    private Panel pnlPie;
    private TableLayoutPanel tlpPie;
    private FlowLayoutPanel flpAcciones;
    private Button btnVerDetalle;
    private Button btnRegistrarDecision;
    private Button btnActualizar;
    private Label lblCantidad;
    private Panel pnlInformacion;
    private Label lblInformacion;
    private Panel pnlIndicador;
}

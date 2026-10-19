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
        btnVerDiagnostico = new Button();
        btnRegistrar = new Button();
        btnLiberarDiagnostico = new Button();
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
        // 
        // tlpPrincipal
        // 
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
        tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
        tlpPrincipal.Controls.Add(pnlGrilla, 0, 2);
        tlpPrincipal.Controls.Add(pnlPie, 0, 3);
        tlpPrincipal.Controls.Add(pnlInformacion, 0, 4);
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Location = new Point(24, 20);
        tlpPrincipal.Name = "tlpPrincipal";
        tlpPrincipal.RowCount = 5;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tlpPrincipal.Size = new Size(952, 610);
        tlpPrincipal.TabIndex = 0;
        // 
        // pnlCabecera
        // 
        pnlCabecera.BackColor = Color.White;
        pnlCabecera.Controls.Add(lblResumen);
        pnlCabecera.Controls.Add(lblDescripcion);
        pnlCabecera.Controls.Add(lblTitulo);
        pnlCabecera.Dock = DockStyle.Fill;
        pnlCabecera.Location = new Point(0, 0);
        pnlCabecera.Margin = new Padding(0, 0, 0, 10);
        pnlCabecera.Name = "pnlCabecera";
        pnlCabecera.Size = new Size(952, 65);
        pnlCabecera.TabIndex = 0;
        // 
        // lblResumen
        // 
        lblResumen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblResumen.ForeColor = Color.FromArgb(71, 85, 105);
        lblResumen.Location = new Point(1262, 21);
        lblResumen.Name = "lblResumen";
        lblResumen.Size = new Size(225, 22);
        lblResumen.TabIndex = 1;
        lblResumen.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDescripcion
        // 
        lblDescripcion.AutoSize = true;
        lblDescripcion.Font = new Font("Segoe UI", 9.5F);
        lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
        lblDescripcion.Location = new Point(20, 43);
        lblDescripcion.Name = "lblDescripcion";
        lblDescripcion.Size = new Size(378, 17);
        lblDescripcion.TabIndex = 2;
        lblDescripcion.Text = "Consulta y registro de diagnósticos asociados a las atenciones.";
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitulo.Location = new Point(18, 8);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(248, 30);
        lblTitulo.TabIndex = 3;
        lblTitulo.Text = "Gestión de diagnósticos";
        // 
        // pnlFiltros
        // 
        pnlFiltros.BackColor = Color.White;
        pnlFiltros.Controls.Add(tlpFiltros);
        pnlFiltros.Dock = DockStyle.Fill;
        pnlFiltros.Location = new Point(0, 75);
        pnlFiltros.Margin = new Padding(0, 0, 0, 10);
        pnlFiltros.Name = "pnlFiltros";
        pnlFiltros.Padding = new Padding(14, 8, 14, 8);
        pnlFiltros.Size = new Size(952, 75);
        pnlFiltros.TabIndex = 1;
        // 
        // tlpFiltros
        // 
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
        tlpFiltros.Location = new Point(14, 8);
        tlpFiltros.Name = "tlpFiltros";
        tlpFiltros.RowCount = 2;
        tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpFiltros.Size = new Size(924, 59);
        tlpFiltros.TabIndex = 0;
        // 
        // lblBuscar
        // 
        lblBuscar.Location = new Point(3, 0);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(100, 22);
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Buscar";
        // 
        // txtBuscar
        // 
        txtBuscar.Dock = DockStyle.Fill;
        txtBuscar.Location = new Point(3, 25);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.PlaceholderText = "Atención, dominio, cliente o motivo";
        txtBuscar.Size = new Size(382, 23);
        txtBuscar.TabIndex = 1;
        txtBuscar.TextChanged += Filtros_Changed;
        // 
        // lblEstado
        // 
        lblEstado.Location = new Point(391, 0);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(100, 22);
        lblEstado.TabIndex = 2;
        lblEstado.Text = "Estado";
        // 
        // cboEstado
        // 
        cboEstado.Dock = DockStyle.Fill;
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.Location = new Point(391, 25);
        cboEstado.Name = "cboEstado";
        cboEstado.Size = new Size(197, 23);
        cboEstado.TabIndex = 3;
        cboEstado.SelectedIndexChanged += Filtros_Changed;
        // 
        // lblTecnico
        // 
        lblTecnico.Location = new Point(594, 0);
        lblTecnico.Name = "lblTecnico";
        lblTecnico.Size = new Size(100, 22);
        lblTecnico.TabIndex = 4;
        lblTecnico.Text = "Técnico asociado";
        // 
        // cboTecnico
        // 
        cboTecnico.Dock = DockStyle.Fill;
        cboTecnico.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTecnico.Location = new Point(594, 25);
        cboTecnico.Name = "cboTecnico";
        cboTecnico.Size = new Size(215, 23);
        cboTecnico.TabIndex = 5;
        cboTecnico.SelectedIndexChanged += Filtros_Changed;
        // 
        // lblEspacio
        // 
        lblEspacio.Location = new Point(815, 0);
        lblEspacio.Name = "lblEspacio";
        lblEspacio.Size = new Size(100, 22);
        lblEspacio.TabIndex = 6;
        // 
        // btnLimpiar
        // 
        btnLimpiar.Dock = DockStyle.Fill;
        btnLimpiar.Location = new Point(817, 22);
        btnLimpiar.Margin = new Padding(5, 0, 5, 5);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(102, 32);
        btnLimpiar.TabIndex = 7;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.Click += BtnLimpiar_Click;
        // 
        // pnlGrilla
        // 
        pnlGrilla.BackColor = Color.White;
        pnlGrilla.Controls.Add(dgvDiagnosticos);
        pnlGrilla.Dock = DockStyle.Fill;
        pnlGrilla.Location = new Point(0, 160);
        pnlGrilla.Margin = new Padding(0, 0, 0, 10);
        pnlGrilla.Name = "pnlGrilla";
        pnlGrilla.Padding = new Padding(14);
        pnlGrilla.Size = new Size(952, 326);
        pnlGrilla.TabIndex = 2;
        // 
        // dgvDiagnosticos
        // 
        dgvDiagnosticos.Columns.AddRange(new DataGridViewColumn[] { colAtencion, colFecha, colCliente, colVehiculo, colMotivo, colTecnico, colEstado });
        dgvDiagnosticos.Dock = DockStyle.Fill;
        dgvDiagnosticos.Location = new Point(14, 14);
        dgvDiagnosticos.Name = "dgvDiagnosticos";
        dgvDiagnosticos.Size = new Size(924, 298);
        dgvDiagnosticos.TabIndex = 0;
        dgvDiagnosticos.CellDoubleClick += DgvDiagnosticos_CellDoubleClick;
        dgvDiagnosticos.SelectionChanged += DgvDiagnosticos_SelectionChanged;
        // 
        // colAtencion
        // 
        colAtencion.HeaderText = "Atención";
        colAtencion.Name = "colAtencion";
        // 
        // colFecha
        // 
        colFecha.HeaderText = "Fecha de referencia";
        colFecha.Name = "colFecha";
        // 
        // colCliente
        // 
        colCliente.HeaderText = "Cliente";
        colCliente.Name = "colCliente";
        // 
        // colVehiculo
        // 
        colVehiculo.HeaderText = "Vehículo";
        colVehiculo.Name = "colVehiculo";
        // 
        // colMotivo
        // 
        colMotivo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colMotivo.HeaderText = "Motivo";
        colMotivo.Name = "colMotivo";
        // 
        // colTecnico
        // 
        colTecnico.HeaderText = "Técnico";
        colTecnico.Name = "colTecnico";
        // 
        // colEstado
        // 
        colEstado.HeaderText = "Diagnóstico";
        colEstado.Name = "colEstado";
        // 
        // pnlPie
        // 
        pnlPie.BackColor = Color.White;
        pnlPie.Controls.Add(flpAcciones);
        pnlPie.Controls.Add(lblCantidad);
        pnlPie.Dock = DockStyle.Fill;
        pnlPie.Location = new Point(0, 496);
        pnlPie.Margin = new Padding(0, 0, 0, 10);
        pnlPie.Name = "pnlPie";
        pnlPie.Padding = new Padding(12, 8, 12, 8);
        pnlPie.Size = new Size(952, 56);
        pnlPie.TabIndex = 3;
        // 
        // flpAcciones
        // 
        flpAcciones.Controls.Add(btnVerAtencion);
        flpAcciones.Controls.Add(btnVerDiagnostico);
        flpAcciones.Controls.Add(btnRegistrar);
        flpAcciones.Controls.Add(btnLiberarDiagnostico);
        flpAcciones.Controls.Add(btnActualizar);
        flpAcciones.Dock = DockStyle.Fill;
        flpAcciones.Location = new Point(12, 8);
        flpAcciones.Name = "flpAcciones";
        flpAcciones.Size = new Size(928, 40);
        flpAcciones.TabIndex = 0;
        flpAcciones.WrapContents = false;
        // 
        // btnVerAtencion
        // 
        btnVerAtencion.Location = new Point(4, 4);
        btnVerAtencion.Margin = new Padding(4);
        btnVerAtencion.Name = "btnVerAtencion";
        btnVerAtencion.Size = new Size(160, 34);
        btnVerAtencion.TabIndex = 0;
        btnVerAtencion.Text = "Ver atención";
        btnVerAtencion.Click += BtnVerAtencion_Click;
        // 
        // btnVerDiagnostico
        // 
        btnVerDiagnostico.Enabled = false;
        btnVerDiagnostico.Location = new Point(172, 4);
        btnVerDiagnostico.Margin = new Padding(4);
        btnVerDiagnostico.Name = "btnVerDiagnostico";
        btnVerDiagnostico.Size = new Size(150, 34);
        btnVerDiagnostico.TabIndex = 3;
        btnVerDiagnostico.Text = "Ver diagnóstico";
        btnVerDiagnostico.UseVisualStyleBackColor = true;
        btnVerDiagnostico.Click += BtnVerDiagnostico_Click;
        // 
        // btnRegistrar
        // 
        btnRegistrar.Location = new Point(330, 4);
        btnRegistrar.Margin = new Padding(4);
        btnRegistrar.Name = "btnRegistrar";
        btnRegistrar.Size = new Size(170, 34);
        btnRegistrar.TabIndex = 0;
        btnRegistrar.Text = "Tomar diagnóstico";
        btnRegistrar.Click += BtnRegistrar_Click;
        // 
        // btnLiberarDiagnostico
        // 
        btnLiberarDiagnostico.Location = new Point(508, 4);
        btnLiberarDiagnostico.Margin = new Padding(4);
        btnLiberarDiagnostico.Name = "btnLiberarDiagnostico";
        btnLiberarDiagnostico.Size = new Size(170, 34);
        btnLiberarDiagnostico.TabIndex = 1;
        btnLiberarDiagnostico.Text = "Liberar diagnóstico";
        btnLiberarDiagnostico.Click += BtnLiberar_Click;
        // 
        // btnActualizar
        // 
        btnActualizar.Location = new Point(686, 4);
        btnActualizar.Margin = new Padding(4);
        btnActualizar.Name = "btnActualizar";
        btnActualizar.Size = new Size(110, 34);
        btnActualizar.TabIndex = 2;
        btnActualizar.Text = "Actualizar";
        btnActualizar.Click += BtnActualizar_Click;
        // 
        // lblCantidad
        // 
        lblCantidad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCantidad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblCantidad.ForeColor = Color.FromArgb(71, 85, 105);
        lblCantidad.Location = new Point(1494, 16);
        lblCantidad.Name = "lblCantidad";
        lblCantidad.Size = new Size(190, 24);
        lblCantidad.TabIndex = 1;
        lblCantidad.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlInformacion
        // 
        pnlInformacion.BackColor = Color.FromArgb(239, 246, 255);
        pnlInformacion.Controls.Add(lblInformacion);
        pnlInformacion.Dock = DockStyle.Fill;
        pnlInformacion.Location = new Point(3, 565);
        pnlInformacion.Name = "pnlInformacion";
        pnlInformacion.Size = new Size(946, 42);
        pnlInformacion.TabIndex = 4;
        // 
        // lblInformacion
        // 
        lblInformacion.Dock = DockStyle.Fill;
        lblInformacion.Font = new Font("Segoe UI", 9F);
        lblInformacion.ForeColor = Color.FromArgb(30, 64, 175);
        lblInformacion.Location = new Point(0, 0);
        lblInformacion.Name = "lblInformacion";
        lblInformacion.Padding = new Padding(16, 0, 12, 0);
        lblInformacion.Size = new Size(946, 42);
        lblInformacion.TabIndex = 0;
        lblInformacion.Text = "El diagnóstico se registra desde una atención asignada al técnico.";
        lblInformacion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // FrmDiagnosticos
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(tlpPrincipal);
        FormBorderStyle = FormBorderStyle.None;
        MinimumSize = new Size(760, 500);
        Name = "FrmDiagnosticos";
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
    private Panel pnlPie;
    private FlowLayoutPanel flpAcciones;
    private Button btnVerAtencion;
    private Button btnLiberarDiagnostico;
    private Button btnActualizar;
    private Label lblCantidad;
    private Panel pnlInformacion;
    private Label lblInformacion;
    private DataGridViewTextBoxColumn colAtencion;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colCliente;
    private DataGridViewTextBoxColumn colVehiculo;
    private DataGridViewTextBoxColumn colMotivo;
    private DataGridViewTextBoxColumn colTecnico;
    private DataGridViewTextBoxColumn colEstado;
    private Button btnVerDiagnostico;
}

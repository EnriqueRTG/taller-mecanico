namespace Taller.Presentacion.Formularios.Sistema;

partial class FrmRespaldoRestauracion
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        tlpPrincipal = new TableLayoutPanel();
        pnlCabecera = new Panel();
        lblUltimoRespaldo = new Label();
        lblDescripcion = new Label();
        lblTitulo = new Label();
        tabOperaciones = new TabControl();
        tabRespaldo = new TabPage();
        tlpRespaldo = new TableLayoutPanel();
        lblBaseDatosTitulo = new Label();
        txtBaseDatos = new TextBox();
        lblCarpetaTitulo = new Label();
        pnlCarpeta = new Panel();
        btnExaminarCarpeta = new Button();
        txtCarpeta = new TextBox();
        lblNombreTitulo = new Label();
        txtNombreArchivo = new TextBox();
        lblAyudaRespaldo = new Label();
        pnlAccionRespaldo = new Panel();
        btnCrearRespaldo = new Button();
        tabRestauracion = new TabPage();
        tlpRestauracion = new TableLayoutPanel();
        lblArchivoTitulo = new Label();
        pnlArchivo = new Panel();
        btnExaminarArchivo = new Button();
        txtArchivoRestaurar = new TextBox();
        lblDetalleArchivoTitulo = new Label();
        lblDetalleArchivo = new Label();
        pnlAdvertencia = new Panel();
        lblAdvertencia = new Label();
        pnlAccionRestauracion = new Panel();
        btnValidarArchivo = new Button();
        btnRestaurar = new Button();
        pnlHistorial = new Panel();
        dgvHistorial = new DataGridView();
        colFecha = new DataGridViewTextBoxColumn();
        colTipo = new DataGridViewTextBoxColumn();
        colArchivo = new DataGridViewTextBoxColumn();
        colUsuario = new DataGridViewTextBoxColumn();
        colResultado = new DataGridViewTextBoxColumn();
        lblHistorial = new Label();
        pnlPie = new Panel();
        lblEstado = new Label();
        tlpPrincipal.SuspendLayout(); pnlCabecera.SuspendLayout(); tabOperaciones.SuspendLayout(); tabRespaldo.SuspendLayout();
        tlpRespaldo.SuspendLayout(); pnlCarpeta.SuspendLayout(); pnlAccionRespaldo.SuspendLayout(); tabRestauracion.SuspendLayout();
        tlpRestauracion.SuspendLayout(); pnlArchivo.SuspendLayout(); pnlAdvertencia.SuspendLayout(); pnlAccionRestauracion.SuspendLayout();
        pnlHistorial.SuspendLayout(); ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit(); pnlPie.SuspendLayout(); SuspendLayout();
        // principal
        tlpPrincipal.ColumnCount = 1; tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.Controls.Add(pnlCabecera, 0, 0); tlpPrincipal.Controls.Add(tabOperaciones, 0, 1); tlpPrincipal.Controls.Add(pnlHistorial, 0, 2); tlpPrincipal.Controls.Add(pnlPie, 0, 3);
        tlpPrincipal.Dock = DockStyle.Fill; tlpPrincipal.RowCount = 4; tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 270F)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        // cabecera
        pnlCabecera.BackColor = Color.White; pnlCabecera.Controls.Add(lblUltimoRespaldo); pnlCabecera.Controls.Add(lblDescripcion); pnlCabecera.Controls.Add(lblTitulo);
        pnlCabecera.Dock = DockStyle.Fill; pnlCabecera.Margin = new Padding(0, 0, 0, 10);
        lblUltimoRespaldo.Anchor = AnchorStyles.Top | AnchorStyles.Right; lblUltimoRespaldo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUltimoRespaldo.ForeColor = Color.FromArgb(71, 85, 105); lblUltimoRespaldo.Location = new Point(610, 24); lblUltimoRespaldo.Size = new Size(320, 22); lblUltimoRespaldo.TextAlign = ContentAlignment.MiddleRight;
        lblDescripcion.AutoSize = true; lblDescripcion.Font = new Font("Segoe UI", 9.5F); lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
        lblDescripcion.Location = new Point(20, 44); lblDescripcion.Text = "Administración simulada de copias de seguridad de la base de datos.";
        lblTitulo.AutoSize = true; lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold); lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitulo.Location = new Point(18, 8); lblTitulo.Text = "Respaldo y restauración";
        // tabs
        tabOperaciones.Controls.Add(tabRespaldo); tabOperaciones.Controls.Add(tabRestauracion); tabOperaciones.Dock = DockStyle.Fill;
        tabOperaciones.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); tabOperaciones.Margin = new Padding(0, 0, 0, 10);
        tabRespaldo.BackColor = Color.White; tabRespaldo.Controls.Add(tlpRespaldo); tabRespaldo.Padding = new Padding(14); tabRespaldo.Text = "Crear respaldo";
        tabRestauracion.BackColor = Color.White; tabRestauracion.Controls.Add(tlpRestauracion); tabRestauracion.Padding = new Padding(14); tabRestauracion.Text = "Restaurar respaldo";
        // tabla respaldo
        tlpRespaldo.ColumnCount = 2; tlpRespaldo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F)); tlpRespaldo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRespaldo.Controls.Add(lblBaseDatosTitulo, 0, 0); tlpRespaldo.Controls.Add(txtBaseDatos, 1, 0); tlpRespaldo.Controls.Add(lblCarpetaTitulo, 0, 1);
        tlpRespaldo.Controls.Add(pnlCarpeta, 1, 1); tlpRespaldo.Controls.Add(lblNombreTitulo, 0, 2); tlpRespaldo.Controls.Add(txtNombreArchivo, 1, 2);
        tlpRespaldo.Controls.Add(lblAyudaRespaldo, 1, 3); tlpRespaldo.Controls.Add(pnlAccionRespaldo, 1, 4); tlpRespaldo.Dock = DockStyle.Fill; tlpRespaldo.RowCount = 5;
        tlpRespaldo.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); tlpRespaldo.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); tlpRespaldo.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tlpRespaldo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); tlpRespaldo.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        ConfigurarEtiqueta(lblBaseDatosTitulo, "Base de datos"); ConfigurarEtiqueta(lblCarpetaTitulo, "Carpeta de destino"); ConfigurarEtiqueta(lblNombreTitulo, "Nombre del archivo");
        ConfigurarTexto(txtBaseDatos); txtBaseDatos.ReadOnly = true; ConfigurarTexto(txtNombreArchivo);
        pnlCarpeta.Controls.Add(txtCarpeta); pnlCarpeta.Controls.Add(btnExaminarCarpeta); pnlCarpeta.Dock = DockStyle.Fill;
        txtCarpeta.Dock = DockStyle.Fill; txtCarpeta.Font = new Font("Segoe UI", 9.5F); txtCarpeta.ReadOnly = true;
        btnExaminarCarpeta.Dock = DockStyle.Right; btnExaminarCarpeta.Size = new Size(110, 40); btnExaminarCarpeta.Text = "Examinar..."; btnExaminarCarpeta.Click += BtnExaminarCarpeta_Click;
        lblAyudaRespaldo.Dock = DockStyle.Fill; lblAyudaRespaldo.Font = new Font("Segoe UI", 8.75F); lblAyudaRespaldo.ForeColor = Color.FromArgb(100, 116, 139);
        lblAyudaRespaldo.Text = "Se generará un archivo .bak. Esta pantalla simula la operación y no modifica SQL Server.";
        pnlAccionRespaldo.Controls.Add(btnCrearRespaldo); pnlAccionRespaldo.Dock = DockStyle.Fill;
        btnCrearRespaldo.Anchor = AnchorStyles.Top | AnchorStyles.Right; btnCrearRespaldo.Location = new Point(558, 4); btnCrearRespaldo.Size = new Size(170, 36);
        btnCrearRespaldo.Text = "Crear respaldo"; btnCrearRespaldo.Click += BtnCrearRespaldo_Click;
        // tabla restauración
        tlpRestauracion.ColumnCount = 2; tlpRestauracion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F)); tlpRestauracion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRestauracion.Controls.Add(lblArchivoTitulo, 0, 0); tlpRestauracion.Controls.Add(pnlArchivo, 1, 0); tlpRestauracion.Controls.Add(lblDetalleArchivoTitulo, 0, 1);
        tlpRestauracion.Controls.Add(lblDetalleArchivo, 1, 1); tlpRestauracion.Controls.Add(pnlAdvertencia, 0, 2); tlpRestauracion.SetColumnSpan(pnlAdvertencia, 2);
        tlpRestauracion.Controls.Add(pnlAccionRestauracion, 1, 3); tlpRestauracion.Dock = DockStyle.Fill; tlpRestauracion.RowCount = 4;
        tlpRestauracion.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F)); tlpRestauracion.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tlpRestauracion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); tlpRestauracion.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        ConfigurarEtiqueta(lblArchivoTitulo, "Archivo de respaldo"); ConfigurarEtiqueta(lblDetalleArchivoTitulo, "Archivo seleccionado");
        pnlArchivo.Controls.Add(txtArchivoRestaurar); pnlArchivo.Controls.Add(btnExaminarArchivo); pnlArchivo.Dock = DockStyle.Fill;
        txtArchivoRestaurar.Dock = DockStyle.Fill; txtArchivoRestaurar.Font = new Font("Segoe UI", 9.5F); txtArchivoRestaurar.ReadOnly = true;
        btnExaminarArchivo.Dock = DockStyle.Right; btnExaminarArchivo.Size = new Size(110, 44); btnExaminarArchivo.Text = "Examinar..."; btnExaminarArchivo.Click += BtnExaminarArchivo_Click;
        lblDetalleArchivo.Dock = DockStyle.Fill; lblDetalleArchivo.Font = new Font("Segoe UI", 9F); lblDetalleArchivo.ForeColor = Color.FromArgb(71, 85, 105); lblDetalleArchivo.Text = "Ningún archivo seleccionado.";
        pnlAdvertencia.BackColor = Color.FromArgb(254, 242, 242); pnlAdvertencia.Controls.Add(lblAdvertencia); pnlAdvertencia.Dock = DockStyle.Fill; pnlAdvertencia.Margin = new Padding(0, 4, 0, 4);
        lblAdvertencia.Dock = DockStyle.Fill; lblAdvertencia.Font = new Font("Segoe UI", 9F); lblAdvertencia.ForeColor = Color.FromArgb(153, 27, 27); lblAdvertencia.Padding = new Padding(12, 0, 12, 0);
        lblAdvertencia.Text = "Advertencia: restaurar una copia reemplazaría la información actual. Debe confirmarse expresamente la operación."; lblAdvertencia.TextAlign = ContentAlignment.MiddleLeft;
        pnlAccionRestauracion.Controls.Add(btnValidarArchivo); pnlAccionRestauracion.Controls.Add(btnRestaurar); pnlAccionRestauracion.Dock = DockStyle.Fill;
        btnValidarArchivo.Anchor = AnchorStyles.Top | AnchorStyles.Right; btnValidarArchivo.Location = new Point(408, 4); btnValidarArchivo.Size = new Size(145, 36); btnValidarArchivo.Text = "Validar archivo"; btnValidarArchivo.Click += BtnValidarArchivo_Click;
        btnRestaurar.Anchor = AnchorStyles.Top | AnchorStyles.Right; btnRestaurar.Location = new Point(563, 4); btnRestaurar.Size = new Size(165, 36); btnRestaurar.Text = "Restaurar respaldo"; btnRestaurar.Click += BtnRestaurar_Click;
        // historial
        pnlHistorial.BackColor = Color.White; pnlHistorial.Controls.Add(dgvHistorial); pnlHistorial.Controls.Add(lblHistorial); pnlHistorial.Dock = DockStyle.Fill; pnlHistorial.Margin = new Padding(0, 0, 0, 10); pnlHistorial.Padding = new Padding(14, 42, 14, 14);
        lblHistorial.AutoSize = true; lblHistorial.Font = new Font("Segoe UI", 10F, FontStyle.Bold); lblHistorial.Location = new Point(14, 12); lblHistorial.Text = "Historial reciente";
        dgvHistorial.Columns.AddRange(colFecha, colTipo, colArchivo, colUsuario, colResultado); dgvHistorial.Dock = DockStyle.Fill;
        ConfigurarColumna(colFecha, "Fecha y hora", 145); ConfigurarColumna(colTipo, "Operación", 105); ConfigurarColumna(colArchivo, "Archivo", 300);
        ConfigurarColumna(colUsuario, "Usuario", 145); ConfigurarColumna(colResultado, "Resultado", 110); colArchivo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        // pie
        pnlPie.BackColor = Color.FromArgb(239, 246, 255); pnlPie.Controls.Add(lblEstado); pnlPie.Dock = DockStyle.Fill;
        lblEstado.Dock = DockStyle.Fill; lblEstado.Font = new Font("Segoe UI", 9F); lblEstado.ForeColor = Color.FromArgb(30, 64, 175); lblEstado.Padding = new Padding(14, 0, 14, 0);
        lblEstado.Text = "Listo para administrar copias de seguridad."; lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        // formulario
        AutoScaleDimensions = new SizeF(7F, 15F); AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(245, 247, 250); ClientSize = new Size(1000, 700);
        Controls.Add(tlpPrincipal); FormBorderStyle = FormBorderStyle.None; MinimumSize = new Size(780, 560); Padding = new Padding(24, 20, 24, 20); Text = "Respaldo y restauración";
        Load += FrmRespaldoRestauracion_Load;
        tlpPrincipal.ResumeLayout(false); pnlCabecera.ResumeLayout(false); pnlCabecera.PerformLayout(); tabOperaciones.ResumeLayout(false); tabRespaldo.ResumeLayout(false);
        tlpRespaldo.ResumeLayout(false); tlpRespaldo.PerformLayout(); pnlCarpeta.ResumeLayout(false); pnlCarpeta.PerformLayout(); pnlAccionRespaldo.ResumeLayout(false);
        tabRestauracion.ResumeLayout(false); tlpRestauracion.ResumeLayout(false); tlpRestauracion.PerformLayout(); pnlArchivo.ResumeLayout(false); pnlArchivo.PerformLayout();
        pnlAdvertencia.ResumeLayout(false); pnlAccionRestauracion.ResumeLayout(false); pnlHistorial.ResumeLayout(false); pnlHistorial.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit(); pnlPie.ResumeLayout(false); ResumeLayout(false);
    }

    private static void ConfigurarEtiqueta(Label c, string texto) { c.Dock = DockStyle.Fill; c.Font = new Font("Segoe UI", 9F, FontStyle.Bold); c.ForeColor = Color.FromArgb(51, 65, 85); c.Padding = new Padding(0, 8, 8, 0); c.Text = texto; }
    private static void ConfigurarTexto(TextBox c) { c.Dock = DockStyle.Fill; c.Font = new Font("Segoe UI", 9.5F); c.Margin = new Padding(3, 4, 3, 7); }
    private static void ConfigurarColumna(DataGridViewColumn c, string texto, int ancho) { c.HeaderText = texto; c.Width = ancho; }

    private TableLayoutPanel tlpPrincipal; private Panel pnlCabecera; private Label lblUltimoRespaldo; private Label lblDescripcion; private Label lblTitulo;
    private TabControl tabOperaciones; private TabPage tabRespaldo; private TableLayoutPanel tlpRespaldo; private Label lblBaseDatosTitulo; private TextBox txtBaseDatos;
    private Label lblCarpetaTitulo; private Panel pnlCarpeta; private Button btnExaminarCarpeta; private TextBox txtCarpeta; private Label lblNombreTitulo; private TextBox txtNombreArchivo;
    private Label lblAyudaRespaldo; private Panel pnlAccionRespaldo; private Button btnCrearRespaldo; private TabPage tabRestauracion; private TableLayoutPanel tlpRestauracion;
    private Label lblArchivoTitulo; private Panel pnlArchivo; private Button btnExaminarArchivo; private TextBox txtArchivoRestaurar; private Label lblDetalleArchivoTitulo; private Label lblDetalleArchivo;
    private Panel pnlAdvertencia; private Label lblAdvertencia; private Panel pnlAccionRestauracion; private Button btnValidarArchivo; private Button btnRestaurar;
    private Panel pnlHistorial; private DataGridView dgvHistorial; private DataGridViewTextBoxColumn colFecha; private DataGridViewTextBoxColumn colTipo; private DataGridViewTextBoxColumn colArchivo;
    private DataGridViewTextBoxColumn colUsuario; private DataGridViewTextBoxColumn colResultado; private Label lblHistorial; private Panel pnlPie; private Label lblEstado;
}

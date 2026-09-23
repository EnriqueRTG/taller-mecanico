namespace Taller.Presentacion.Formularios.Presupuestos;

partial class FrmDetallePresupuesto
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
        lblEstado = new Label();
        lblSubtitulo = new Label();
        lblTitulo = new Label();
        pnlDatos = new Panel();
        tlpDatos = new TableLayoutPanel();
        lblAtencionTitulo = new Label();
        lblAtencion = new Label();
        lblFechaTitulo = new Label();
        lblFecha = new Label();
        lblClienteTitulo = new Label();
        lblCliente = new Label();
        lblVehiculoTitulo = new Label();
        lblVehiculo = new Label();
        pnlDetalle = new Panel();
        dgvConceptos = new DataGridView();
        colTipo = new DataGridViewTextBoxColumn();
        colConcepto = new DataGridViewTextBoxColumn();
        colCantidad = new DataGridViewTextBoxColumn();
        colPrecio = new DataGridViewTextBoxColumn();
        colSubtotal = new DataGridViewTextBoxColumn();
        lblDetalle = new Label();
        pnlResumen = new Panel();
        lblObservaciones = new Label();
        lblObservacionesTitulo = new Label();
        lblTotal = new Label();
        lblTotalTitulo = new Label();
        lblSubtotal = new Label();
        lblSubtotalTitulo = new Label();
        pnlPie = new Panel();
        btnCerrar = new Button();
        btnImprimir = new Button();
        tlpPrincipal.SuspendLayout();
        pnlCabecera.SuspendLayout();
        pnlDatos.SuspendLayout();
        tlpDatos.SuspendLayout();
        pnlDetalle.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvConceptos).BeginInit();
        pnlResumen.SuspendLayout();
        pnlPie.SuspendLayout();
        SuspendLayout();
        // tlpPrincipal
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
        tlpPrincipal.Controls.Add(pnlDatos, 0, 1);
        tlpPrincipal.Controls.Add(pnlDetalle, 0, 2);
        tlpPrincipal.Controls.Add(pnlResumen, 0, 3);
        tlpPrincipal.Controls.Add(pnlPie, 0, 4);
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Padding = new Padding(20);
        tlpPrincipal.RowCount = 5;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 74F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 104F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
        // pnlCabecera
        pnlCabecera.BackColor = Color.White;
        pnlCabecera.Controls.Add(lblEstado);
        pnlCabecera.Controls.Add(lblSubtitulo);
        pnlCabecera.Controls.Add(lblTitulo);
        pnlCabecera.Dock = DockStyle.Fill;
        pnlCabecera.Margin = new Padding(0, 0, 0, 10);
        pnlCabecera.Padding = new Padding(16, 8, 16, 8);
        // lblEstado
        lblEstado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblEstado.AutoSize = true;
        lblEstado.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblEstado.Location = new Point(704, 25);
        lblEstado.Text = "ESTADO";
        // lblSubtitulo
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
        lblSubtitulo.ForeColor = Color.FromArgb(100, 116, 139);
        lblSubtitulo.Location = new Point(18, 43);
        lblSubtitulo.Text = "Consulta detallada de conceptos e importes presupuestados.";
        // lblTitulo
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitulo.Location = new Point(16, 8);
        lblTitulo.Text = "Presupuesto P-000000";
        // pnlDatos
        pnlDatos.BackColor = Color.White;
        pnlDatos.Controls.Add(tlpDatos);
        pnlDatos.Dock = DockStyle.Fill;
        pnlDatos.Margin = new Padding(0, 0, 0, 10);
        pnlDatos.Padding = new Padding(14, 8, 14, 8);
        // tlpDatos
        tlpDatos.ColumnCount = 4;
        tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpDatos.Controls.Add(lblAtencionTitulo, 0, 0);
        tlpDatos.Controls.Add(lblFechaTitulo, 1, 0);
        tlpDatos.Controls.Add(lblClienteTitulo, 2, 0);
        tlpDatos.Controls.Add(lblVehiculoTitulo, 3, 0);
        tlpDatos.Controls.Add(lblAtencion, 0, 1);
        tlpDatos.Controls.Add(lblFecha, 1, 1);
        tlpDatos.Controls.Add(lblCliente, 2, 1);
        tlpDatos.Controls.Add(lblVehiculo, 3, 1);
        tlpDatos.Dock = DockStyle.Fill;
        tlpDatos.RowCount = 2;
        tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tlpDatos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        ConfigurarTituloDato(lblAtencionTitulo, "Atención");
        ConfigurarTituloDato(lblFechaTitulo, "Fecha de emisión");
        ConfigurarTituloDato(lblClienteTitulo, "Cliente");
        ConfigurarTituloDato(lblVehiculoTitulo, "Vehículo");
        ConfigurarValorDato(lblAtencion);
        ConfigurarValorDato(lblFecha);
        ConfigurarValorDato(lblCliente);
        ConfigurarValorDato(lblVehiculo);
        // pnlDetalle
        pnlDetalle.BackColor = Color.White;
        pnlDetalle.Controls.Add(dgvConceptos);
        pnlDetalle.Controls.Add(lblDetalle);
        pnlDetalle.Dock = DockStyle.Fill;
        pnlDetalle.Margin = new Padding(0, 0, 0, 10);
        pnlDetalle.Padding = new Padding(14, 42, 14, 14);
        // lblDetalle
        lblDetalle.AutoSize = true;
        lblDetalle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDetalle.ForeColor = Color.FromArgb(15, 23, 42);
        lblDetalle.Location = new Point(14, 12);
        lblDetalle.Text = "Conceptos presupuestados";
        // dgvConceptos
        dgvConceptos.Columns.AddRange(colTipo, colConcepto, colCantidad, colPrecio, colSubtotal);
        dgvConceptos.Dock = DockStyle.Fill;
        ConfigurarColumna(colTipo, "Tipo", 115);
        ConfigurarColumna(colConcepto, "Descripción", 300);
        ConfigurarColumna(colCantidad, "Cantidad", 90);
        ConfigurarColumna(colPrecio, "Precio unitario", 125);
        ConfigurarColumna(colSubtotal, "Subtotal", 125);
        colConcepto.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        // pnlResumen
        pnlResumen.BackColor = Color.White;
        pnlResumen.Controls.Add(lblObservaciones);
        pnlResumen.Controls.Add(lblObservacionesTitulo);
        pnlResumen.Controls.Add(lblTotal);
        pnlResumen.Controls.Add(lblTotalTitulo);
        pnlResumen.Controls.Add(lblSubtotal);
        pnlResumen.Controls.Add(lblSubtotalTitulo);
        pnlResumen.Dock = DockStyle.Fill;
        pnlResumen.Margin = new Padding(0, 0, 0, 10);
        lblObservacionesTitulo.AutoSize = true;
        lblObservacionesTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblObservacionesTitulo.Location = new Point(16, 12);
        lblObservacionesTitulo.Text = "Observaciones";
        lblObservaciones.Font = new Font("Segoe UI", 9F);
        lblObservaciones.ForeColor = Color.FromArgb(71, 85, 105);
        lblObservaciones.Location = new Point(16, 37);
        lblObservaciones.Size = new Size(520, 58);
        lblObservaciones.Text = "Valores estimados sujetos a la aprobación del cliente.";
        lblSubtotalTitulo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblSubtotalTitulo.AutoSize = true;
        lblSubtotalTitulo.Location = new Point(650, 19);
        lblSubtotalTitulo.Text = "Subtotal:";
        lblSubtotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblSubtotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblSubtotal.Location = new Point(740, 16);
        lblSubtotal.Size = new Size(130, 24);
        lblSubtotal.TextAlign = ContentAlignment.MiddleRight;
        lblTotalTitulo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTotalTitulo.AutoSize = true;
        lblTotalTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTotalTitulo.Location = new Point(650, 58);
        lblTotalTitulo.Text = "TOTAL:";
        lblTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTotal.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTotal.ForeColor = Color.FromArgb(30, 64, 175);
        lblTotal.Location = new Point(730, 51);
        lblTotal.Size = new Size(140, 34);
        lblTotal.TextAlign = ContentAlignment.MiddleRight;
        // pnlPie
        pnlPie.Controls.Add(btnCerrar);
        pnlPie.Controls.Add(btnImprimir);
        pnlPie.Dock = DockStyle.Fill;
        pnlPie.Margin = new Padding(0);
        btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCerrar.Location = new Point(748, 8);
        btnCerrar.Size = new Size(122, 38);
        btnCerrar.Text = "Cerrar";
        btnCerrar.Click += BtnCerrar_Click;
        btnImprimir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnImprimir.Location = new Point(600, 8);
        btnImprimir.Size = new Size(136, 38);
        btnImprimir.Text = "Vista de impresión";
        btnImprimir.Click += BtnImprimir_Click;
        // FrmDetallePresupuesto
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(920, 650);
        Controls.Add(tlpPrincipal);
        MinimumSize = new Size(820, 580);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Detalle del presupuesto";
        Load += FrmDetallePresupuesto_Load;
        tlpPrincipal.ResumeLayout(false);
        pnlCabecera.ResumeLayout(false);
        pnlCabecera.PerformLayout();
        pnlDatos.ResumeLayout(false);
        tlpDatos.ResumeLayout(false);
        tlpDatos.PerformLayout();
        pnlDetalle.ResumeLayout(false);
        pnlDetalle.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvConceptos).EndInit();
        pnlResumen.ResumeLayout(false);
        pnlResumen.PerformLayout();
        pnlPie.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigurarTituloDato(Label label, string texto)
    {
        label.Dock = DockStyle.Fill;
        label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        label.ForeColor = Color.FromArgb(71, 85, 105);
        label.Text = texto;
    }

    private static void ConfigurarValorDato(Label label)
    {
        label.Dock = DockStyle.Fill;
        label.Font = new Font("Segoe UI", 9.5F);
        label.ForeColor = Color.FromArgb(15, 23, 42);
    }

    private static void ConfigurarColumna(DataGridViewColumn columna, string texto, int ancho)
    {
        columna.HeaderText = texto;
        columna.Width = ancho;
    }

    private TableLayoutPanel tlpPrincipal;
    private Panel pnlCabecera;
    private Label lblEstado;
    private Label lblSubtitulo;
    private Label lblTitulo;
    private Panel pnlDatos;
    private TableLayoutPanel tlpDatos;
    private Label lblAtencionTitulo;
    private Label lblAtencion;
    private Label lblFechaTitulo;
    private Label lblFecha;
    private Label lblClienteTitulo;
    private Label lblCliente;
    private Label lblVehiculoTitulo;
    private Label lblVehiculo;
    private Panel pnlDetalle;
    private DataGridView dgvConceptos;
    private DataGridViewTextBoxColumn colTipo;
    private DataGridViewTextBoxColumn colConcepto;
    private DataGridViewTextBoxColumn colCantidad;
    private DataGridViewTextBoxColumn colPrecio;
    private DataGridViewTextBoxColumn colSubtotal;
    private Label lblDetalle;
    private Panel pnlResumen;
    private Label lblObservaciones;
    private Label lblObservacionesTitulo;
    private Label lblTotal;
    private Label lblTotalTitulo;
    private Label lblSubtotal;
    private Label lblSubtotalTitulo;
    private Panel pnlPie;
    private Button btnCerrar;
    private Button btnImprimir;
}

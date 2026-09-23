namespace Taller.Presentacion.Formularios.Presupuestos
{
    partial class FrmGenerarPresupuesto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            tlpPrincipal = new TableLayoutPanel();
            pnlEncabezado = new Panel();
            tlpEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            lblNumeroAtencion = new Label();
            tlpContenido = new TableLayoutPanel();
            grpAtencion = new GroupBox();
            tlpResumenAtencion = new TableLayoutPanel();
            lblCliente = new Label();
            lblClienteValor = new Label();
            lblVehiculo = new Label();
            lblVehiculoValor = new Label();
            lblDominio = new Label();
            lblDominioValor = new Label();
            lblDiagnostico = new Label();
            lblDiagnosticoValor = new Label();
            splitPresupuesto = new SplitContainer();
            grpCatalogo = new GroupBox();
            tlpCatalogo = new TableLayoutPanel();
            tlpFiltrosCatalogo = new TableLayoutPanel();
            lblBuscar = new Label();
            lblTipo = new Label();
            lblCategoria = new Label();
            label4 = new Label();
            txtBuscarConcepto = new TextBox();
            cboTipoConcepto = new ComboBox();
            cboCategoriaConcepto = new ComboBox();
            btnBuscarConceptos = new Button();
            dgvCatalogoConceptos = new DataGridView();
            colCatalogoCodigo = new DataGridViewTextBoxColumn();
            colCatalogoTipo = new DataGridViewTextBoxColumn();
            colCatalogoNombre = new DataGridViewTextBoxColumn();
            colCatalogoCategoria = new DataGridViewTextBoxColumn();
            colCatalogoUnidad = new DataGridViewTextBoxColumn();
            colCatalogoPrecio = new DataGridViewTextBoxColumn();
            pnlConceptoSeleccionado = new Panel();
            tlpConceptoSeleccionado = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            lblConceptoSeleccionado = new Label();
            nudCantidad = new NumericUpDown();
            nudPrecioUnitario = new NumericUpDown();
            btnAgregarConcepto = new Button();
            btnLimpiarFiltros = new Button();
            grpDetallePresupuesto = new GroupBox();
            tlpDetallePresupuesto = new TableLayoutPanel();
            dgvConceptosPresupuesto = new DataGridView();
            colDetalleCodigo = new DataGridViewTextBoxColumn();
            colDetalleTipo = new DataGridViewTextBoxColumn();
            colDetalleConcepto = new DataGridViewTextBoxColumn();
            colDetalleCantidad = new DataGridViewTextBoxColumn();
            colDetallePrecioUnitario = new DataGridViewTextBoxColumn();
            colDetalleSubtotal = new DataGridViewTextBoxColumn();
            flpAccionesDetalle = new FlowLayoutPanel();
            btnQuitarConcepto = new Button();
            btnModificarCantidad = new Button();
            tlpResumenPresupuesto = new TableLayoutPanel();
            lblTotalPresupuesto = new Label();
            lblCantidadConceptos = new Label();
            grpDatosGenerales = new GroupBox();
            tlpDatosGenerales = new TableLayoutPanel();
            label5 = new Label();
            dtpFechaEmision = new DateTimePicker();
            label6 = new Label();
            nudVigenciaDias = new NumericUpDown();
            label7 = new Label();
            txtObservaciones = new TextBox();
            flpBotones = new FlowLayoutPanel();
            btnGenerarPresupuesto = new Button();
            btnCancelar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            tlpContenido.SuspendLayout();
            grpAtencion.SuspendLayout();
            tlpResumenAtencion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitPresupuesto).BeginInit();
            splitPresupuesto.Panel1.SuspendLayout();
            splitPresupuesto.Panel2.SuspendLayout();
            splitPresupuesto.SuspendLayout();
            grpCatalogo.SuspendLayout();
            tlpCatalogo.SuspendLayout();
            tlpFiltrosCatalogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCatalogoConceptos).BeginInit();
            pnlConceptoSeleccionado.SuspendLayout();
            tlpConceptoSeleccionado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecioUnitario).BeginInit();
            grpDetallePresupuesto.SuspendLayout();
            tlpDetallePresupuesto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvConceptosPresupuesto).BeginInit();
            flpAccionesDetalle.SuspendLayout();
            tlpResumenPresupuesto.SuspendLayout();
            grpDatosGenerales.SuspendLayout();
            tlpDatosGenerales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudVigenciaDias).BeginInit();
            flpBotones.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(tlpContenido, 0, 1);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 2;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPrincipal.Size = new Size(1084, 721);
            tlpPrincipal.TabIndex = 0;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tlpEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 10);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(20, 5, 20, 5);
            pnlEncabezado.Size = new Size(1084, 65);
            pnlEncabezado.TabIndex = 1;
            // 
            // tlpEncabezado
            // 
            tlpEncabezado.ColumnCount = 2;
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpEncabezado.Controls.Add(lblTitulo, 0, 0);
            tlpEncabezado.Controls.Add(lblDescripcion, 0, 1);
            tlpEncabezado.Controls.Add(lblNumeroAtencion, 1, 0);
            tlpEncabezado.Dock = DockStyle.Fill;
            tlpEncabezado.Location = new Point(20, 5);
            tlpEncabezado.Margin = new Padding(0);
            tlpEncabezado.Name = "tlpEncabezado";
            tlpEncabezado.RowCount = 2;
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
            tlpEncabezado.Size = new Size(1044, 55);
            tlpEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.ImageAlign = ContentAlignment.TopLeft;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Margin = new Padding(0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(730, 31);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Generar presupuesto técnico";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.ImageAlign = ContentAlignment.BottomLeft;
            lblDescripcion.Location = new Point(0, 31);
            lblDescripcion.Margin = new Padding(0);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(730, 24);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Detalle los trabajos y conceptos necesarios para la reparación.";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNumeroAtencion
            // 
            lblNumeroAtencion.Dock = DockStyle.Fill;
            lblNumeroAtencion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumeroAtencion.ForeColor = Color.FromArgb(30, 64, 175);
            lblNumeroAtencion.Location = new Point(730, 0);
            lblNumeroAtencion.Margin = new Padding(0);
            lblNumeroAtencion.Name = "lblNumeroAtencion";
            lblNumeroAtencion.Size = new Size(314, 31);
            lblNumeroAtencion.TabIndex = 2;
            lblNumeroAtencion.Text = "Atención N.º 000001";
            lblNumeroAtencion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tlpContenido
            // 
            tlpContenido.ColumnCount = 1;
            tlpContenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContenido.Controls.Add(grpAtencion, 0, 0);
            tlpContenido.Controls.Add(splitPresupuesto, 0, 1);
            tlpContenido.Controls.Add(flpBotones, 0, 3);
            tlpContenido.Dock = DockStyle.Fill;
            tlpContenido.Location = new Point(3, 78);
            tlpContenido.Name = "tlpContenido";
            tlpContenido.Padding = new Padding(16);
            tlpContenido.RowCount = 4;
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tlpContenido.Size = new Size(1078, 640);
            tlpContenido.TabIndex = 2;
            // 
            // grpAtencion
            // 
            grpAtencion.BackColor = Color.White;
            grpAtencion.Controls.Add(tlpResumenAtencion);
            grpAtencion.Dock = DockStyle.Fill;
            grpAtencion.Location = new Point(16, 16);
            grpAtencion.Margin = new Padding(0, 0, 0, 8);
            grpAtencion.Name = "grpAtencion";
            grpAtencion.Size = new Size(1046, 82);
            grpAtencion.TabIndex = 0;
            grpAtencion.TabStop = false;
            grpAtencion.Text = "Atención seleccionada";
            // 
            // tlpResumenAtencion
            // 
            tlpResumenAtencion.ColumnCount = 4;
            tlpResumenAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tlpResumenAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumenAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tlpResumenAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 39F));
            tlpResumenAtencion.Controls.Add(lblCliente, 0, 0);
            tlpResumenAtencion.Controls.Add(lblClienteValor, 1, 0);
            tlpResumenAtencion.Controls.Add(lblVehiculo, 2, 0);
            tlpResumenAtencion.Controls.Add(lblVehiculoValor, 3, 0);
            tlpResumenAtencion.Controls.Add(lblDominio, 0, 1);
            tlpResumenAtencion.Controls.Add(lblDominioValor, 1, 1);
            tlpResumenAtencion.Controls.Add(lblDiagnostico, 2, 1);
            tlpResumenAtencion.Controls.Add(lblDiagnosticoValor, 3, 1);
            tlpResumenAtencion.Dock = DockStyle.Fill;
            tlpResumenAtencion.Location = new Point(3, 19);
            tlpResumenAtencion.Name = "tlpResumenAtencion";
            tlpResumenAtencion.Padding = new Padding(10);
            tlpResumenAtencion.RowCount = 2;
            tlpResumenAtencion.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumenAtencion.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumenAtencion.Size = new Size(1040, 60);
            tlpResumenAtencion.TabIndex = 0;
            // 
            // lblCliente
            // 
            lblCliente.AutoSize = true;
            lblCliente.Dock = DockStyle.Fill;
            lblCliente.Location = new Point(13, 10);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(177, 20);
            lblCliente.TabIndex = 0;
            lblCliente.Text = "Cliente:";
            lblCliente.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblClienteValor
            // 
            lblClienteValor.AutoSize = true;
            lblClienteValor.Dock = DockStyle.Fill;
            lblClienteValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblClienteValor.Location = new Point(196, 10);
            lblClienteValor.Name = "lblClienteValor";
            lblClienteValor.Size = new Size(249, 20);
            lblClienteValor.TabIndex = 1;
            lblClienteValor.Text = "Juan Santos";
            lblClienteValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculo
            // 
            lblVehiculo.AutoSize = true;
            lblVehiculo.Dock = DockStyle.Fill;
            lblVehiculo.Location = new Point(451, 10);
            lblVehiculo.Name = "lblVehiculo";
            lblVehiculo.Size = new Size(177, 20);
            lblVehiculo.TabIndex = 2;
            lblVehiculo.Text = "Vehículo:";
            lblVehiculo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblVehiculoValor
            // 
            lblVehiculoValor.AutoSize = true;
            lblVehiculoValor.Dock = DockStyle.Fill;
            lblVehiculoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVehiculoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblVehiculoValor.Location = new Point(634, 10);
            lblVehiculoValor.Name = "lblVehiculoValor";
            lblVehiculoValor.Size = new Size(393, 20);
            lblVehiculoValor.TabIndex = 3;
            lblVehiculoValor.Text = "Citroën Jumper";
            lblVehiculoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominio
            // 
            lblDominio.AutoSize = true;
            lblDominio.Dock = DockStyle.Fill;
            lblDominio.Location = new Point(13, 30);
            lblDominio.Name = "lblDominio";
            lblDominio.Size = new Size(177, 20);
            lblDominio.TabIndex = 4;
            lblDominio.Text = "Dominio:";
            lblDominio.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDominioValor
            // 
            lblDominioValor.AutoSize = true;
            lblDominioValor.Dock = DockStyle.Fill;
            lblDominioValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDominioValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblDominioValor.Location = new Point(196, 30);
            lblDominioValor.Name = "lblDominioValor";
            lblDominioValor.Size = new Size(249, 20);
            lblDominioValor.TabIndex = 5;
            lblDominioValor.Text = "AB123CD";
            lblDominioValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiagnostico
            // 
            lblDiagnostico.AutoSize = true;
            lblDiagnostico.Dock = DockStyle.Fill;
            lblDiagnostico.Location = new Point(451, 30);
            lblDiagnostico.Name = "lblDiagnostico";
            lblDiagnostico.Size = new Size(177, 20);
            lblDiagnostico.TabIndex = 6;
            lblDiagnostico.Text = "Diagnóstico:";
            lblDiagnostico.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDiagnosticoValor
            // 
            lblDiagnosticoValor.AutoSize = true;
            lblDiagnosticoValor.Dock = DockStyle.Fill;
            lblDiagnosticoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDiagnosticoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblDiagnosticoValor.Location = new Point(634, 30);
            lblDiagnosticoValor.Name = "lblDiagnosticoValor";
            lblDiagnosticoValor.Size = new Size(393, 20);
            lblDiagnosticoValor.TabIndex = 7;
            lblDiagnosticoValor.Text = "Reparación requerida";
            lblDiagnosticoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // splitPresupuesto
            // 
            splitPresupuesto.BackColor = Color.FromArgb(226, 232, 240);
            splitPresupuesto.Dock = DockStyle.Fill;
            splitPresupuesto.Location = new Point(19, 109);
            splitPresupuesto.Name = "splitPresupuesto";
            // 
            // splitPresupuesto.Panel1
            // 
            splitPresupuesto.Panel1.Controls.Add(grpCatalogo);
            // 
            // splitPresupuesto.Panel2
            // 
            splitPresupuesto.Panel2.Controls.Add(grpDetallePresupuesto);
            splitPresupuesto.Size = new Size(1040, 442);
            splitPresupuesto.SplitterDistance = 610;
            splitPresupuesto.SplitterWidth = 8;
            splitPresupuesto.TabIndex = 1;
            // 
            // grpCatalogo
            // 
            grpCatalogo.BackColor = Color.White;
            grpCatalogo.Controls.Add(tlpCatalogo);
            grpCatalogo.Dock = DockStyle.Fill;
            grpCatalogo.Location = new Point(0, 0);
            grpCatalogo.Name = "grpCatalogo";
            grpCatalogo.Padding = new Padding(10);
            grpCatalogo.Size = new Size(610, 442);
            grpCatalogo.TabIndex = 0;
            grpCatalogo.TabStop = false;
            grpCatalogo.Text = "Catálogo de conceptos";
            // 
            // tlpCatalogo
            // 
            tlpCatalogo.ColumnCount = 1;
            tlpCatalogo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCatalogo.Controls.Add(tlpFiltrosCatalogo, 0, 0);
            tlpCatalogo.Controls.Add(dgvCatalogoConceptos, 0, 1);
            tlpCatalogo.Controls.Add(pnlConceptoSeleccionado, 0, 2);
            tlpCatalogo.Controls.Add(btnLimpiarFiltros, 0, 3);
            tlpCatalogo.Dock = DockStyle.Fill;
            tlpCatalogo.Location = new Point(10, 26);
            tlpCatalogo.Name = "tlpCatalogo";
            tlpCatalogo.RowCount = 4;
            tlpCatalogo.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));
            tlpCatalogo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCatalogo.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tlpCatalogo.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpCatalogo.Size = new Size(590, 406);
            tlpCatalogo.TabIndex = 0;
            // 
            // tlpFiltrosCatalogo
            // 
            tlpFiltrosCatalogo.ColumnCount = 4;
            tlpFiltrosCatalogo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpFiltrosCatalogo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tlpFiltrosCatalogo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            tlpFiltrosCatalogo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpFiltrosCatalogo.Controls.Add(lblBuscar, 0, 0);
            tlpFiltrosCatalogo.Controls.Add(lblTipo, 1, 0);
            tlpFiltrosCatalogo.Controls.Add(lblCategoria, 2, 0);
            tlpFiltrosCatalogo.Controls.Add(label4, 3, 0);
            tlpFiltrosCatalogo.Controls.Add(txtBuscarConcepto, 0, 1);
            tlpFiltrosCatalogo.Controls.Add(cboTipoConcepto, 1, 1);
            tlpFiltrosCatalogo.Controls.Add(cboCategoriaConcepto, 2, 1);
            tlpFiltrosCatalogo.Controls.Add(btnBuscarConceptos, 3, 1);
            tlpFiltrosCatalogo.Dock = DockStyle.Fill;
            tlpFiltrosCatalogo.Location = new Point(3, 3);
            tlpFiltrosCatalogo.Name = "tlpFiltrosCatalogo";
            tlpFiltrosCatalogo.RowCount = 2;
            tlpFiltrosCatalogo.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpFiltrosCatalogo.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tlpFiltrosCatalogo.Size = new Size(584, 72);
            tlpFiltrosCatalogo.TabIndex = 0;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Dock = DockStyle.Fill;
            lblBuscar.Location = new Point(3, 0);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(227, 28);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar";
            lblBuscar.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Dock = DockStyle.Fill;
            lblTipo.Location = new Point(236, 0);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(122, 28);
            lblTipo.TabIndex = 1;
            lblTipo.Text = "Tipo";
            lblTipo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Dock = DockStyle.Fill;
            lblCategoria.Location = new Point(364, 0);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(128, 28);
            lblCategoria.TabIndex = 2;
            lblCategoria.Text = "Categoría";
            lblCategoria.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(498, 0);
            label4.Name = "label4";
            label4.Size = new Size(0, 15);
            label4.TabIndex = 3;
            // 
            // txtBuscarConcepto
            // 
            txtBuscarConcepto.Dock = DockStyle.Fill;
            txtBuscarConcepto.Location = new Point(3, 31);
            txtBuscarConcepto.Name = "txtBuscarConcepto";
            txtBuscarConcepto.PlaceholderText = "Código, nombre o palabra clave";
            txtBuscarConcepto.Size = new Size(227, 23);
            txtBuscarConcepto.TabIndex = 0;
            txtBuscarConcepto.TextChanged += FiltroCatalogo_Cambio;
            // 
            // cboTipoConcepto
            // 
            cboTipoConcepto.Dock = DockStyle.Fill;
            cboTipoConcepto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoConcepto.FormattingEnabled = true;
            cboTipoConcepto.Location = new Point(236, 31);
            cboTipoConcepto.Name = "cboTipoConcepto";
            cboTipoConcepto.Size = new Size(122, 23);
            cboTipoConcepto.TabIndex = 1;
            cboTipoConcepto.SelectedIndexChanged += FiltroCatalogo_Cambio;
            // 
            // cboCategoriaConcepto
            // 
            cboCategoriaConcepto.Dock = DockStyle.Fill;
            cboCategoriaConcepto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoriaConcepto.FormattingEnabled = true;
            cboCategoriaConcepto.Location = new Point(364, 31);
            cboCategoriaConcepto.Name = "cboCategoriaConcepto";
            cboCategoriaConcepto.Size = new Size(128, 23);
            cboCategoriaConcepto.TabIndex = 2;
            cboCategoriaConcepto.SelectedIndexChanged += FiltroCatalogo_Cambio;
            // 
            // btnBuscarConceptos
            // 
            btnBuscarConceptos.Dock = DockStyle.Fill;
            btnBuscarConceptos.Location = new Point(499, 32);
            btnBuscarConceptos.Margin = new Padding(4);
            btnBuscarConceptos.Name = "btnBuscarConceptos";
            btnBuscarConceptos.Size = new Size(81, 36);
            btnBuscarConceptos.TabIndex = 3;
            btnBuscarConceptos.Text = "Buscar";
            btnBuscarConceptos.UseVisualStyleBackColor = true;
            btnBuscarConceptos.Click += BtnBuscarConceptos_Click;
            // 
            // dgvCatalogoConceptos
            // 
            dgvCatalogoConceptos.AllowUserToAddRows = false;
            dgvCatalogoConceptos.AllowUserToDeleteRows = false;
            dgvCatalogoConceptos.AllowUserToResizeRows = false;
            dgvCatalogoConceptos.BackgroundColor = Color.White;
            dgvCatalogoConceptos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCatalogoConceptos.Columns.AddRange(new DataGridViewColumn[] { colCatalogoCodigo, colCatalogoTipo, colCatalogoNombre, colCatalogoCategoria, colCatalogoUnidad, colCatalogoPrecio });
            dgvCatalogoConceptos.Dock = DockStyle.Fill;
            dgvCatalogoConceptos.Location = new Point(3, 86);
            dgvCatalogoConceptos.Margin = new Padding(3, 8, 3, 8);
            dgvCatalogoConceptos.MultiSelect = false;
            dgvCatalogoConceptos.Name = "dgvCatalogoConceptos";
            dgvCatalogoConceptos.ReadOnly = true;
            dgvCatalogoConceptos.RowHeadersVisible = false;
            dgvCatalogoConceptos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCatalogoConceptos.Size = new Size(584, 178);
            dgvCatalogoConceptos.TabIndex = 4;
            dgvCatalogoConceptos.CellDoubleClick += DgvCatalogoConceptos_CellDoubleClick;
            dgvCatalogoConceptos.SelectionChanged += DgvCatalogoConceptos_SelectionChanged;
            // 
            // colCatalogoCodigo
            // 
            colCatalogoCodigo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCatalogoCodigo.HeaderText = "Código";
            colCatalogoCodigo.Name = "colCatalogoCodigo";
            colCatalogoCodigo.ReadOnly = true;
            colCatalogoCodigo.Width = 85;
            // 
            // colCatalogoTipo
            // 
            colCatalogoTipo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCatalogoTipo.HeaderText = "Tipo";
            colCatalogoTipo.Name = "colCatalogoTipo";
            colCatalogoTipo.ReadOnly = true;
            colCatalogoTipo.Width = 85;
            // 
            // colCatalogoNombre
            // 
            colCatalogoNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCatalogoNombre.HeaderText = "Nombre";
            colCatalogoNombre.Name = "colCatalogoNombre";
            colCatalogoNombre.ReadOnly = true;
            colCatalogoNombre.Width = 210;
            // 
            // colCatalogoCategoria
            // 
            colCatalogoCategoria.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCatalogoCategoria.HeaderText = "Categoría";
            colCatalogoCategoria.Name = "colCatalogoCategoria";
            colCatalogoCategoria.ReadOnly = true;
            colCatalogoCategoria.Width = 120;
            // 
            // colCatalogoUnidad
            // 
            colCatalogoUnidad.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCatalogoUnidad.HeaderText = "Unidad";
            colCatalogoUnidad.Name = "colCatalogoUnidad";
            colCatalogoUnidad.ReadOnly = true;
            colCatalogoUnidad.Width = 80;
            // 
            // colCatalogoPrecio
            // 
            colCatalogoPrecio.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCatalogoPrecio.DefaultCellStyle = dataGridViewCellStyle1;
            colCatalogoPrecio.HeaderText = "Precio";
            colCatalogoPrecio.Name = "colCatalogoPrecio";
            colCatalogoPrecio.ReadOnly = true;
            colCatalogoPrecio.Width = 110;
            // 
            // pnlConceptoSeleccionado
            // 
            pnlConceptoSeleccionado.BackColor = Color.FromArgb(241, 245, 249);
            pnlConceptoSeleccionado.Controls.Add(tlpConceptoSeleccionado);
            pnlConceptoSeleccionado.Dock = DockStyle.Fill;
            pnlConceptoSeleccionado.Location = new Point(3, 275);
            pnlConceptoSeleccionado.Name = "pnlConceptoSeleccionado";
            pnlConceptoSeleccionado.Padding = new Padding(8);
            pnlConceptoSeleccionado.Size = new Size(584, 86);
            pnlConceptoSeleccionado.TabIndex = 2;
            // 
            // tlpConceptoSeleccionado
            // 
            tlpConceptoSeleccionado.ColumnCount = 4;
            tlpConceptoSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52.6315842F));
            tlpConceptoSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.7894754F));
            tlpConceptoSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21.0526333F));
            tlpConceptoSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.5263166F));
            tlpConceptoSeleccionado.Controls.Add(label1, 0, 0);
            tlpConceptoSeleccionado.Controls.Add(label2, 1, 0);
            tlpConceptoSeleccionado.Controls.Add(label3, 2, 0);
            tlpConceptoSeleccionado.Controls.Add(lblConceptoSeleccionado, 0, 1);
            tlpConceptoSeleccionado.Controls.Add(nudCantidad, 1, 1);
            tlpConceptoSeleccionado.Controls.Add(nudPrecioUnitario, 2, 1);
            tlpConceptoSeleccionado.Controls.Add(btnAgregarConcepto, 3, 1);
            tlpConceptoSeleccionado.Dock = DockStyle.Fill;
            tlpConceptoSeleccionado.Location = new Point(8, 8);
            tlpConceptoSeleccionado.Name = "tlpConceptoSeleccionado";
            tlpConceptoSeleccionado.RowCount = 2;
            tlpConceptoSeleccionado.RowStyles.Add(new RowStyle(SizeType.Percent, 41.6666679F));
            tlpConceptoSeleccionado.RowStyles.Add(new RowStyle(SizeType.Percent, 58.3333321F));
            tlpConceptoSeleccionado.Size = new Size(568, 70);
            tlpConceptoSeleccionado.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(292, 29);
            label1.TabIndex = 0;
            label1.Text = "Concepto seleccionado";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(301, 0);
            label2.Name = "label2";
            label2.Size = new Size(83, 29);
            label2.TabIndex = 1;
            label2.Text = "Cantidad";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(390, 0);
            label3.Name = "label3";
            label3.Size = new Size(113, 29);
            label3.TabIndex = 2;
            label3.Text = "Precio unitario";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblConceptoSeleccionado
            // 
            lblConceptoSeleccionado.AutoSize = true;
            lblConceptoSeleccionado.Dock = DockStyle.Fill;
            lblConceptoSeleccionado.Location = new Point(3, 29);
            lblConceptoSeleccionado.Name = "lblConceptoSeleccionado";
            lblConceptoSeleccionado.Size = new Size(292, 41);
            lblConceptoSeleccionado.TabIndex = 3;
            lblConceptoSeleccionado.Text = "Ningún concepto seleccionado";
            lblConceptoSeleccionado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudCantidad
            // 
            nudCantidad.Dock = DockStyle.Fill;
            nudCantidad.Location = new Point(301, 32);
            nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidad.Name = "nudCantidad";
            nudCantidad.Size = new Size(83, 23);
            nudCantidad.TabIndex = 5;
            nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // nudPrecioUnitario
            // 
            nudPrecioUnitario.DecimalPlaces = 2;
            nudPrecioUnitario.Dock = DockStyle.Fill;
            nudPrecioUnitario.Location = new Point(390, 32);
            nudPrecioUnitario.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            nudPrecioUnitario.Name = "nudPrecioUnitario";
            nudPrecioUnitario.Size = new Size(113, 23);
            nudPrecioUnitario.TabIndex = 6;
            nudPrecioUnitario.ThousandsSeparator = true;
            // 
            // btnAgregarConcepto
            // 
            btnAgregarConcepto.Dock = DockStyle.Fill;
            btnAgregarConcepto.Enabled = false;
            btnAgregarConcepto.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAgregarConcepto.Location = new Point(509, 32);
            btnAgregarConcepto.Name = "btnAgregarConcepto";
            btnAgregarConcepto.Size = new Size(56, 35);
            btnAgregarConcepto.TabIndex = 7;
            btnAgregarConcepto.Text = "Agregar";
            btnAgregarConcepto.UseVisualStyleBackColor = true;
            btnAgregarConcepto.Click += BtnAgregarConcepto_Click;
            // 
            // btnLimpiarFiltros
            // 
            btnLimpiarFiltros.Anchor = AnchorStyles.Left;
            btnLimpiarFiltros.Location = new Point(3, 369);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(130, 32);
            btnLimpiarFiltros.TabIndex = 3;
            btnLimpiarFiltros.Text = "Limpiar filtros";
            btnLimpiarFiltros.UseVisualStyleBackColor = true;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            // 
            // grpDetallePresupuesto
            // 
            grpDetallePresupuesto.BackColor = Color.White;
            grpDetallePresupuesto.Controls.Add(tlpDetallePresupuesto);
            grpDetallePresupuesto.Dock = DockStyle.Fill;
            grpDetallePresupuesto.Location = new Point(0, 0);
            grpDetallePresupuesto.Name = "grpDetallePresupuesto";
            grpDetallePresupuesto.Padding = new Padding(10);
            grpDetallePresupuesto.Size = new Size(422, 442);
            grpDetallePresupuesto.TabIndex = 0;
            grpDetallePresupuesto.TabStop = false;
            grpDetallePresupuesto.Text = "Presupuesto en preparación";
            // 
            // tlpDetallePresupuesto
            // 
            tlpDetallePresupuesto.ColumnCount = 1;
            tlpDetallePresupuesto.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDetallePresupuesto.Controls.Add(dgvConceptosPresupuesto, 0, 0);
            tlpDetallePresupuesto.Controls.Add(flpAccionesDetalle, 0, 1);
            tlpDetallePresupuesto.Controls.Add(tlpResumenPresupuesto, 0, 2);
            tlpDetallePresupuesto.Controls.Add(grpDatosGenerales, 0, 3);
            tlpDetallePresupuesto.Dock = DockStyle.Fill;
            tlpDetallePresupuesto.Location = new Point(10, 26);
            tlpDetallePresupuesto.Name = "tlpDetallePresupuesto";
            tlpDetallePresupuesto.RowCount = 4;
            tlpDetallePresupuesto.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDetallePresupuesto.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpDetallePresupuesto.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpDetallePresupuesto.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            tlpDetallePresupuesto.Size = new Size(402, 406);
            tlpDetallePresupuesto.TabIndex = 0;
            // 
            // dgvConceptosPresupuesto
            // 
            dgvConceptosPresupuesto.AllowUserToAddRows = false;
            dgvConceptosPresupuesto.AllowUserToDeleteRows = false;
            dgvConceptosPresupuesto.AllowUserToResizeRows = false;
            dgvConceptosPresupuesto.BackgroundColor = Color.White;
            dgvConceptosPresupuesto.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvConceptosPresupuesto.Columns.AddRange(new DataGridViewColumn[] { colDetalleCodigo, colDetalleTipo, colDetalleConcepto, colDetalleCantidad, colDetallePrecioUnitario, colDetalleSubtotal });
            dgvConceptosPresupuesto.Dock = DockStyle.Fill;
            dgvConceptosPresupuesto.Location = new Point(3, 3);
            dgvConceptosPresupuesto.MultiSelect = false;
            dgvConceptosPresupuesto.Name = "dgvConceptosPresupuesto";
            dgvConceptosPresupuesto.ReadOnly = true;
            dgvConceptosPresupuesto.RowHeadersVisible = false;
            dgvConceptosPresupuesto.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvConceptosPresupuesto.Size = new Size(396, 176);
            dgvConceptosPresupuesto.TabIndex = 8;
            dgvConceptosPresupuesto.SelectionChanged += DgvConceptosPresupuesto_SelectionChanged;
            // 
            // colDetalleCodigo
            // 
            colDetalleCodigo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDetalleCodigo.HeaderText = "Código";
            colDetalleCodigo.Name = "colDetalleCodigo";
            colDetalleCodigo.ReadOnly = true;
            colDetalleCodigo.Width = 85;
            // 
            // colDetalleTipo
            // 
            colDetalleTipo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDetalleTipo.HeaderText = "Tipo";
            colDetalleTipo.Name = "colDetalleTipo";
            colDetalleTipo.ReadOnly = true;
            colDetalleTipo.Width = 80;
            // 
            // colDetalleConcepto
            // 
            colDetalleConcepto.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDetalleConcepto.HeaderText = "Concepto";
            colDetalleConcepto.Name = "colDetalleConcepto";
            colDetalleConcepto.ReadOnly = true;
            colDetalleConcepto.Width = 220;
            // 
            // colDetalleCantidad
            // 
            colDetalleCantidad.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDetalleCantidad.HeaderText = "Cant.";
            colDetalleCantidad.Name = "colDetalleCantidad";
            colDetalleCantidad.ReadOnly = true;
            colDetalleCantidad.Width = 65;
            // 
            // colDetallePrecioUnitario
            // 
            colDetallePrecioUnitario.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDetallePrecioUnitario.DefaultCellStyle = dataGridViewCellStyle2;
            colDetallePrecioUnitario.HeaderText = "Precio";
            colDetallePrecioUnitario.Name = "colDetallePrecioUnitario";
            colDetallePrecioUnitario.ReadOnly = true;
            colDetallePrecioUnitario.Width = 105;
            // 
            // colDetalleSubtotal
            // 
            colDetalleSubtotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDetalleSubtotal.DefaultCellStyle = dataGridViewCellStyle3;
            colDetalleSubtotal.HeaderText = "Subtotal";
            colDetalleSubtotal.Name = "colDetalleSubtotal";
            colDetalleSubtotal.ReadOnly = true;
            colDetalleSubtotal.Width = 110;
            // 
            // flpAccionesDetalle
            // 
            flpAccionesDetalle.Controls.Add(btnQuitarConcepto);
            flpAccionesDetalle.Controls.Add(btnModificarCantidad);
            flpAccionesDetalle.Dock = DockStyle.Fill;
            flpAccionesDetalle.FlowDirection = FlowDirection.RightToLeft;
            flpAccionesDetalle.Location = new Point(3, 185);
            flpAccionesDetalle.Name = "flpAccionesDetalle";
            flpAccionesDetalle.Padding = new Padding(0, 5, 0, 3);
            flpAccionesDetalle.Size = new Size(396, 38);
            flpAccionesDetalle.TabIndex = 1;
            flpAccionesDetalle.WrapContents = false;
            // 
            // btnQuitarConcepto
            // 
            btnQuitarConcepto.Enabled = false;
            btnQuitarConcepto.Location = new Point(293, 8);
            btnQuitarConcepto.Name = "btnQuitarConcepto";
            btnQuitarConcepto.Size = new Size(100, 28);
            btnQuitarConcepto.TabIndex = 10;
            btnQuitarConcepto.Text = "Quitar";
            btnQuitarConcepto.UseVisualStyleBackColor = true;
            btnQuitarConcepto.Click += BtnQuitarConcepto_Click;
            // 
            // btnModificarCantidad
            // 
            btnModificarCantidad.Enabled = false;
            btnModificarCantidad.Location = new Point(147, 8);
            btnModificarCantidad.Name = "btnModificarCantidad";
            btnModificarCantidad.Size = new Size(140, 28);
            btnModificarCantidad.TabIndex = 9;
            btnModificarCantidad.Text = "Modificar cantidad";
            btnModificarCantidad.UseVisualStyleBackColor = true;
            btnModificarCantidad.Click += BtnModificarCantidad_Click;
            // 
            // tlpResumenPresupuesto
            // 
            tlpResumenPresupuesto.ColumnCount = 2;
            tlpResumenPresupuesto.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpResumenPresupuesto.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpResumenPresupuesto.Controls.Add(lblTotalPresupuesto, 1, 0);
            tlpResumenPresupuesto.Controls.Add(lblCantidadConceptos, 0, 0);
            tlpResumenPresupuesto.Dock = DockStyle.Fill;
            tlpResumenPresupuesto.Location = new Point(3, 229);
            tlpResumenPresupuesto.Name = "tlpResumenPresupuesto";
            tlpResumenPresupuesto.RowCount = 1;
            tlpResumenPresupuesto.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumenPresupuesto.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumenPresupuesto.Size = new Size(396, 44);
            tlpResumenPresupuesto.TabIndex = 2;
            // 
            // lblTotalPresupuesto
            // 
            lblTotalPresupuesto.AutoSize = true;
            lblTotalPresupuesto.Dock = DockStyle.Fill;
            lblTotalPresupuesto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalPresupuesto.Location = new Point(201, 0);
            lblTotalPresupuesto.Name = "lblTotalPresupuesto";
            lblTotalPresupuesto.Size = new Size(192, 44);
            lblTotalPresupuesto.TabIndex = 2;
            lblTotalPresupuesto.Text = "Total estimado: $ 0,00";
            lblTotalPresupuesto.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCantidadConceptos
            // 
            lblCantidadConceptos.AutoSize = true;
            lblCantidadConceptos.Dock = DockStyle.Fill;
            lblCantidadConceptos.Location = new Point(3, 0);
            lblCantidadConceptos.Name = "lblCantidadConceptos";
            lblCantidadConceptos.Size = new Size(192, 44);
            lblCantidadConceptos.TabIndex = 0;
            lblCantidadConceptos.Text = "0 conceptos agregados";
            lblCantidadConceptos.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpDatosGenerales
            // 
            grpDatosGenerales.Controls.Add(tlpDatosGenerales);
            grpDatosGenerales.Dock = DockStyle.Fill;
            grpDatosGenerales.Location = new Point(3, 279);
            grpDatosGenerales.Name = "grpDatosGenerales";
            grpDatosGenerales.Size = new Size(396, 124);
            grpDatosGenerales.TabIndex = 3;
            grpDatosGenerales.TabStop = false;
            grpDatosGenerales.Text = "Datos generales";
            // 
            // tlpDatosGenerales
            // 
            tlpDatosGenerales.ColumnCount = 4;
            tlpDatosGenerales.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tlpDatosGenerales.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            tlpDatosGenerales.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tlpDatosGenerales.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            tlpDatosGenerales.Controls.Add(label5, 0, 0);
            tlpDatosGenerales.Controls.Add(dtpFechaEmision, 1, 0);
            tlpDatosGenerales.Controls.Add(label6, 2, 0);
            tlpDatosGenerales.Controls.Add(nudVigenciaDias, 3, 0);
            tlpDatosGenerales.Controls.Add(label7, 0, 1);
            tlpDatosGenerales.Controls.Add(txtObservaciones, 1, 1);
            tlpDatosGenerales.Dock = DockStyle.Fill;
            tlpDatosGenerales.Location = new Point(3, 19);
            tlpDatosGenerales.Name = "tlpDatosGenerales";
            tlpDatosGenerales.Padding = new Padding(6);
            tlpDatosGenerales.RowCount = 2;
            tlpDatosGenerales.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpDatosGenerales.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpDatosGenerales.Size = new Size(390, 102);
            tlpDatosGenerales.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(9, 6);
            label5.Name = "label5";
            label5.Size = new Size(77, 45);
            label5.TabIndex = 0;
            label5.Text = "Fecha de emisión";
            // 
            // dtpFechaEmision
            // 
            dtpFechaEmision.Dock = DockStyle.Fill;
            dtpFechaEmision.Format = DateTimePickerFormat.Custom;
            dtpFechaEmision.Location = new Point(92, 9);
            dtpFechaEmision.Name = "dtpFechaEmision";
            dtpFechaEmision.Size = new Size(99, 23);
            dtpFechaEmision.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(197, 6);
            label6.Name = "label6";
            label6.Size = new Size(77, 45);
            label6.TabIndex = 2;
            label6.Text = "Vigencia";
            // 
            // nudVigenciaDias
            // 
            nudVigenciaDias.Dock = DockStyle.Fill;
            nudVigenciaDias.Location = new Point(280, 9);
            nudVigenciaDias.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            nudVigenciaDias.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudVigenciaDias.Name = "nudVigenciaDias";
            nudVigenciaDias.Size = new Size(101, 23);
            nudVigenciaDias.TabIndex = 12;
            nudVigenciaDias.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(9, 51);
            label7.Name = "label7";
            label7.Size = new Size(77, 45);
            label7.TabIndex = 4;
            label7.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            tlpDatosGenerales.SetColumnSpan(txtObservaciones, 3);
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Location = new Point(92, 54);
            txtObservaciones.MaxLength = 500;
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(289, 39);
            txtObservaciones.TabIndex = 13;
            // 
            // flpBotones
            // 
            flpBotones.Controls.Add(btnGenerarPresupuesto);
            flpBotones.Controls.Add(btnCancelar);
            flpBotones.Dock = DockStyle.Fill;
            flpBotones.FlowDirection = FlowDirection.RightToLeft;
            flpBotones.Location = new Point(19, 567);
            flpBotones.Name = "flpBotones";
            flpBotones.Padding = new Padding(0, 10, 0, 0);
            flpBotones.Size = new Size(1040, 54);
            flpBotones.TabIndex = 2;
            // 
            // btnGenerarPresupuesto
            // 
            btnGenerarPresupuesto.Enabled = false;
            btnGenerarPresupuesto.Location = new Point(857, 13);
            btnGenerarPresupuesto.Name = "btnGenerarPresupuesto";
            btnGenerarPresupuesto.Size = new Size(180, 38);
            btnGenerarPresupuesto.TabIndex = 15;
            btnGenerarPresupuesto.Text = "Generar presupuesto";
            btnGenerarPresupuesto.UseVisualStyleBackColor = true;
            btnGenerarPresupuesto.Click += BtnGenerarPresupuesto_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(731, 13);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.TabIndex = 14;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // FrmGenerarPresupuesto
            // 
            AcceptButton = btnGenerarPresupuesto;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(248, 250, 252);
            CancelButton = btnCancelar;
            ClientSize = new Size(1084, 721);
            Controls.Add(tlpPrincipal);
            MinimizeBox = false;
            MinimumSize = new Size(1000, 700);
            Name = "FrmGenerarPresupuesto";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Generar presupuesto";
            FormClosing += FrmGenerarPresupuesto_FormClosing;
            Load += FrmGenerarPresupuesto_Load;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            tlpContenido.ResumeLayout(false);
            grpAtencion.ResumeLayout(false);
            tlpResumenAtencion.ResumeLayout(false);
            tlpResumenAtencion.PerformLayout();
            splitPresupuesto.Panel1.ResumeLayout(false);
            splitPresupuesto.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitPresupuesto).EndInit();
            splitPresupuesto.ResumeLayout(false);
            grpCatalogo.ResumeLayout(false);
            tlpCatalogo.ResumeLayout(false);
            tlpFiltrosCatalogo.ResumeLayout(false);
            tlpFiltrosCatalogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCatalogoConceptos).EndInit();
            pnlConceptoSeleccionado.ResumeLayout(false);
            tlpConceptoSeleccionado.ResumeLayout(false);
            tlpConceptoSeleccionado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecioUnitario).EndInit();
            grpDetallePresupuesto.ResumeLayout(false);
            tlpDetallePresupuesto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvConceptosPresupuesto).EndInit();
            flpAccionesDetalle.ResumeLayout(false);
            tlpResumenPresupuesto.ResumeLayout(false);
            tlpResumenPresupuesto.PerformLayout();
            grpDatosGenerales.ResumeLayout(false);
            tlpDatosGenerales.ResumeLayout(false);
            tlpDatosGenerales.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudVigenciaDias).EndInit();
            flpBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private TableLayoutPanel tlpEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblNumeroAtencion;
        private TableLayoutPanel tlpContenido;
        private GroupBox grpAtencion;
        private TableLayoutPanel tlpResumenAtencion;
        private Label lblCliente;
        private Label lblClienteValor;
        private Label lblVehiculo;
        private Label lblVehiculoValor;
        private Label lblDominio;
        private Label lblDominioValor;
        private Label lblDiagnostico;
        private Label lblDiagnosticoValor;
        private SplitContainer splitPresupuesto;
        private GroupBox grpCatalogo;
        private TableLayoutPanel tlpCatalogo;
        private TableLayoutPanel tlpFiltrosCatalogo;
        private Label lblBuscar;
        private Label lblTipo;
        private Label lblCategoria;
        private Label label4;
        private TextBox txtBuscarConcepto;
        private ComboBox cboTipoConcepto;
        private ComboBox cboCategoriaConcepto;
        private Button btnBuscarConceptos;
        private DataGridView dgvCatalogoConceptos;
        private DataGridViewTextBoxColumn colCatalogoCodigo;
        private DataGridViewTextBoxColumn colCatalogoTipo;
        private DataGridViewTextBoxColumn colCatalogoNombre;
        private DataGridViewTextBoxColumn colCatalogoCategoria;
        private DataGridViewTextBoxColumn colCatalogoUnidad;
        private DataGridViewTextBoxColumn colCatalogoPrecio;
        private Panel pnlConceptoSeleccionado;
        private TableLayoutPanel tlpConceptoSeleccionado;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lblConceptoSeleccionado;
        private NumericUpDown nudCantidad;
        private NumericUpDown nudPrecioUnitario;
        private Button btnAgregarConcepto;
        private Button btnLimpiarFiltros;
        private GroupBox grpDetallePresupuesto;
        private TableLayoutPanel tlpDetallePresupuesto;
        private DataGridView dgvConceptosPresupuesto;
        private DataGridViewTextBoxColumn colDetalleCodigo;
        private DataGridViewTextBoxColumn colDetalleTipo;
        private DataGridViewTextBoxColumn colDetalleConcepto;
        private DataGridViewTextBoxColumn colDetalleCantidad;
        private DataGridViewTextBoxColumn colDetallePrecioUnitario;
        private DataGridViewTextBoxColumn colDetalleSubtotal;
        private FlowLayoutPanel flpAccionesDetalle;
        private Button btnQuitarConcepto;
        private Button btnModificarCantidad;
        private TableLayoutPanel tlpResumenPresupuesto;
        private Label lblCantidadConceptos;
        private Label lblTotalPresupuesto;
        private GroupBox grpDatosGenerales;
        private TableLayoutPanel tlpDatosGenerales;
        private Label label5;
        private DateTimePicker dtpFechaEmision;
        private Label label6;
        private NumericUpDown nudVigenciaDias;
        private Label label7;
        private TextBox txtObservaciones;
        private FlowLayoutPanel flpBotones;
        private Button btnGenerarPresupuesto;
        private Button btnCancelar;
    }
}
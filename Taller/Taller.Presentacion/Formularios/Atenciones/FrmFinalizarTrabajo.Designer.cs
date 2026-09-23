namespace Taller.Presentacion.Formularios.Atenciones
{
    partial class FrmFinalizarTrabajo
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
            tlpPrincipal = new TableLayoutPanel();
            pnlEncabezado = new Panel();
            tlpEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            lblNumeroAtencion = new Label();
            grpAtencion = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblClienteTitulo = new Label();
            lblClienteValor = new Label();
            lblVehiculoTitulo = new Label();
            lblVehiculoValor = new Label();
            lblDominioTitulo = new Label();
            lblDominioValor = new Label();
            lblTecnicoTitulo = new Label();
            lblTecnicoValor = new Label();
            lblDiagnosticoTitulo = new Label();
            lblDiagnosticoValor = new Label();
            lblPresupuestoTitulo = new Label();
            lblPresupuestoValor = new Label();
            grpTrabajosAutorizados = new GroupBox();
            dgvTrabajosAutorizados = new DataGridView();
            colCodigo = new DataGridViewTextBoxColumn();
            colTipo = new DataGridViewTextBoxColumn();
            colConcepto = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            grpResultadoTrabajo = new GroupBox();
            tlpResultado = new TableLayoutPanel();
            label1 = new Label();
            dtpFechaFinalizacion = new DateTimePicker();
            label2 = new Label();
            cboResultadoTrabajo = new ComboBox();
            lblTrabajoRealizadoTitulo = new Label();
            txtTrabajoRealizado = new TextBox();
            lblObservacionesTitulo = new Label();
            txtObservaciones = new TextBox();
            flpConfirmaciones = new FlowLayoutPanel();
            chkConfirmarFinalizacion = new CheckBox();
            chkPruebaFuncionamiento = new CheckBox();
            flpBotones = new FlowLayoutPanel();
            btnFinalizarTrabajo = new Button();
            btnCancelar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            grpAtencion.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            grpTrabajosAutorizados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTrabajosAutorizados).BeginInit();
            grpResultadoTrabajo.SuspendLayout();
            tlpResultado.SuspendLayout();
            flpConfirmaciones.SuspendLayout();
            flpBotones.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(grpAtencion, 0, 1);
            tlpPrincipal.Controls.Add(grpTrabajosAutorizados, 0, 2);
            tlpPrincipal.Controls.Add(grpResultadoTrabajo, 0, 3);
            tlpPrincipal.Controls.Add(flpBotones, 0, 4);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(16);
            tlpPrincipal.RowCount = 5;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 105F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 62F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tlpPrincipal.Size = new Size(884, 641);
            tlpPrincipal.TabIndex = 0;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tlpEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(16, 16);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 10);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(20, 5, 20, 5);
            pnlEncabezado.Size = new Size(852, 65);
            pnlEncabezado.TabIndex = 2;
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
            tlpEncabezado.Size = new Size(812, 55);
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
            lblTitulo.Size = new Size(568, 31);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Finalizar trabajo técnico";
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
            lblDescripcion.Size = new Size(568, 24);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Registre el resultado de los trabajos realizados sobre el vehículo.";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNumeroAtencion
            // 
            lblNumeroAtencion.Dock = DockStyle.Fill;
            lblNumeroAtencion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumeroAtencion.ForeColor = Color.FromArgb(30, 64, 175);
            lblNumeroAtencion.Location = new Point(568, 0);
            lblNumeroAtencion.Margin = new Padding(0);
            lblNumeroAtencion.Name = "lblNumeroAtencion";
            lblNumeroAtencion.Size = new Size(244, 31);
            lblNumeroAtencion.TabIndex = 2;
            lblNumeroAtencion.Text = "Atención N.º 000001";
            lblNumeroAtencion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // grpAtencion
            // 
            grpAtencion.BackColor = Color.White;
            grpAtencion.Controls.Add(tableLayoutPanel1);
            grpAtencion.Dock = DockStyle.Fill;
            grpAtencion.Location = new Point(19, 94);
            grpAtencion.Name = "grpAtencion";
            grpAtencion.Size = new Size(846, 99);
            grpAtencion.TabIndex = 3;
            grpAtencion.TabStop = false;
            grpAtencion.Text = "Atención seleccionada";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel1.Controls.Add(lblClienteTitulo, 0, 0);
            tableLayoutPanel1.Controls.Add(lblClienteValor, 1, 0);
            tableLayoutPanel1.Controls.Add(lblVehiculoTitulo, 2, 0);
            tableLayoutPanel1.Controls.Add(lblVehiculoValor, 3, 0);
            tableLayoutPanel1.Controls.Add(lblDominioTitulo, 0, 1);
            tableLayoutPanel1.Controls.Add(lblDominioValor, 1, 1);
            tableLayoutPanel1.Controls.Add(lblTecnicoTitulo, 2, 1);
            tableLayoutPanel1.Controls.Add(lblTecnicoValor, 3, 1);
            tableLayoutPanel1.Controls.Add(lblDiagnosticoTitulo, 0, 2);
            tableLayoutPanel1.Controls.Add(lblDiagnosticoValor, 1, 2);
            tableLayoutPanel1.Controls.Add(lblPresupuestoTitulo, 2, 2);
            tableLayoutPanel1.Controls.Add(lblPresupuestoValor, 3, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(840, 77);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.Dock = DockStyle.Fill;
            lblClienteTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblClienteTitulo.Location = new Point(4, 4);
            lblClienteTitulo.Margin = new Padding(4);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.Size = new Size(134, 17);
            lblClienteTitulo.TabIndex = 0;
            lblClienteTitulo.Text = "Cliente:";
            lblClienteTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClienteValor
            // 
            lblClienteValor.Dock = DockStyle.Fill;
            lblClienteValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteValor.ForeColor = Color.FromArgb(15, 23, 42);
            lblClienteValor.Location = new Point(146, 4);
            lblClienteValor.Margin = new Padding(4);
            lblClienteValor.Name = "lblClienteValor";
            lblClienteValor.Size = new Size(269, 17);
            lblClienteValor.TabIndex = 1;
            lblClienteValor.Text = "Juan Santos";
            lblClienteValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculoTitulo
            // 
            lblVehiculoTitulo.Dock = DockStyle.Fill;
            lblVehiculoTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblVehiculoTitulo.Location = new Point(423, 4);
            lblVehiculoTitulo.Margin = new Padding(4);
            lblVehiculoTitulo.Name = "lblVehiculoTitulo";
            lblVehiculoTitulo.Size = new Size(134, 17);
            lblVehiculoTitulo.TabIndex = 2;
            lblVehiculoTitulo.Text = "Vehículo:";
            lblVehiculoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculoValor
            // 
            lblVehiculoValor.Dock = DockStyle.Fill;
            lblVehiculoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVehiculoValor.ForeColor = Color.FromArgb(15, 23, 42);
            lblVehiculoValor.Location = new Point(565, 4);
            lblVehiculoValor.Margin = new Padding(4);
            lblVehiculoValor.Name = "lblVehiculoValor";
            lblVehiculoValor.Size = new Size(271, 17);
            lblVehiculoValor.TabIndex = 3;
            lblVehiculoValor.Text = "Citroën Jumper";
            lblVehiculoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioTitulo
            // 
            lblDominioTitulo.Dock = DockStyle.Fill;
            lblDominioTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblDominioTitulo.Location = new Point(4, 29);
            lblDominioTitulo.Margin = new Padding(4);
            lblDominioTitulo.Name = "lblDominioTitulo";
            lblDominioTitulo.Size = new Size(134, 17);
            lblDominioTitulo.TabIndex = 4;
            lblDominioTitulo.Text = "Dominio:";
            lblDominioTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioValor
            // 
            lblDominioValor.Dock = DockStyle.Fill;
            lblDominioValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDominioValor.ForeColor = Color.FromArgb(15, 23, 42);
            lblDominioValor.Location = new Point(146, 29);
            lblDominioValor.Margin = new Padding(4);
            lblDominioValor.Name = "lblDominioValor";
            lblDominioValor.Size = new Size(269, 17);
            lblDominioValor.TabIndex = 5;
            lblDominioValor.Text = "AB123CD";
            lblDominioValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTecnicoTitulo
            // 
            lblTecnicoTitulo.Dock = DockStyle.Fill;
            lblTecnicoTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblTecnicoTitulo.Location = new Point(423, 29);
            lblTecnicoTitulo.Margin = new Padding(4);
            lblTecnicoTitulo.Name = "lblTecnicoTitulo";
            lblTecnicoTitulo.Size = new Size(134, 17);
            lblTecnicoTitulo.TabIndex = 6;
            lblTecnicoTitulo.Text = "Técnico:";
            lblTecnicoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTecnicoValor
            // 
            lblTecnicoValor.Dock = DockStyle.Fill;
            lblTecnicoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTecnicoValor.ForeColor = Color.FromArgb(15, 23, 42);
            lblTecnicoValor.Location = new Point(565, 29);
            lblTecnicoValor.Margin = new Padding(4);
            lblTecnicoValor.Name = "lblTecnicoValor";
            lblTecnicoValor.Size = new Size(271, 17);
            lblTecnicoValor.TabIndex = 7;
            lblTecnicoValor.Text = "Técnico actual";
            lblTecnicoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiagnosticoTitulo
            // 
            lblDiagnosticoTitulo.Dock = DockStyle.Fill;
            lblDiagnosticoTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblDiagnosticoTitulo.Location = new Point(4, 54);
            lblDiagnosticoTitulo.Margin = new Padding(4);
            lblDiagnosticoTitulo.Name = "lblDiagnosticoTitulo";
            lblDiagnosticoTitulo.Size = new Size(134, 19);
            lblDiagnosticoTitulo.TabIndex = 8;
            lblDiagnosticoTitulo.Text = "Diagnóstico:";
            lblDiagnosticoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiagnosticoValor
            // 
            lblDiagnosticoValor.Dock = DockStyle.Fill;
            lblDiagnosticoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDiagnosticoValor.ForeColor = Color.FromArgb(15, 23, 42);
            lblDiagnosticoValor.Location = new Point(146, 54);
            lblDiagnosticoValor.Margin = new Padding(4);
            lblDiagnosticoValor.Name = "lblDiagnosticoValor";
            lblDiagnosticoValor.Size = new Size(269, 19);
            lblDiagnosticoValor.TabIndex = 9;
            lblDiagnosticoValor.Text = "Reparación requerida";
            lblDiagnosticoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPresupuestoTitulo
            // 
            lblPresupuestoTitulo.Dock = DockStyle.Fill;
            lblPresupuestoTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblPresupuestoTitulo.Location = new Point(423, 54);
            lblPresupuestoTitulo.Margin = new Padding(4);
            lblPresupuestoTitulo.Name = "lblPresupuestoTitulo";
            lblPresupuestoTitulo.Size = new Size(134, 19);
            lblPresupuestoTitulo.TabIndex = 10;
            lblPresupuestoTitulo.Text = "Presupuesto:";
            lblPresupuestoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPresupuestoValor
            // 
            lblPresupuestoValor.Dock = DockStyle.Fill;
            lblPresupuestoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPresupuestoValor.ForeColor = Color.FromArgb(15, 23, 42);
            lblPresupuestoValor.Location = new Point(565, 54);
            lblPresupuestoValor.Margin = new Padding(4);
            lblPresupuestoValor.Name = "lblPresupuestoValor";
            lblPresupuestoValor.Size = new Size(271, 19);
            lblPresupuestoValor.TabIndex = 11;
            lblPresupuestoValor.Text = "Autorizado";
            lblPresupuestoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpTrabajosAutorizados
            // 
            grpTrabajosAutorizados.BackColor = Color.White;
            grpTrabajosAutorizados.Controls.Add(dgvTrabajosAutorizados);
            grpTrabajosAutorizados.Dock = DockStyle.Fill;
            grpTrabajosAutorizados.Location = new Point(16, 196);
            grpTrabajosAutorizados.Margin = new Padding(0, 0, 0, 8);
            grpTrabajosAutorizados.Name = "grpTrabajosAutorizados";
            grpTrabajosAutorizados.Padding = new Padding(10);
            grpTrabajosAutorizados.Size = new Size(852, 132);
            grpTrabajosAutorizados.TabIndex = 4;
            grpTrabajosAutorizados.TabStop = false;
            grpTrabajosAutorizados.Text = "Trabajos y conceptos autorizados";
            // 
            // dgvTrabajosAutorizados
            // 
            dgvTrabajosAutorizados.AllowUserToAddRows = false;
            dgvTrabajosAutorizados.AllowUserToDeleteRows = false;
            dgvTrabajosAutorizados.AllowUserToResizeRows = false;
            dgvTrabajosAutorizados.BackgroundColor = Color.White;
            dgvTrabajosAutorizados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTrabajosAutorizados.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colTipo, colConcepto, colCantidad, colEstado });
            dgvTrabajosAutorizados.Dock = DockStyle.Fill;
            dgvTrabajosAutorizados.Location = new Point(10, 26);
            dgvTrabajosAutorizados.Margin = new Padding(6);
            dgvTrabajosAutorizados.MultiSelect = false;
            dgvTrabajosAutorizados.Name = "dgvTrabajosAutorizados";
            dgvTrabajosAutorizados.ReadOnly = true;
            dgvTrabajosAutorizados.RowHeadersVisible = false;
            dgvTrabajosAutorizados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrabajosAutorizados.Size = new Size(832, 96);
            dgvTrabajosAutorizados.TabIndex = 0;
            // 
            // colCodigo
            // 
            colCodigo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCodigo.HeaderText = "Código";
            colCodigo.Name = "colCodigo";
            colCodigo.ReadOnly = true;
            // 
            // colTipo
            // 
            colTipo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colTipo.HeaderText = "Tipo";
            colTipo.Name = "colTipo";
            colTipo.ReadOnly = true;
            colTipo.Width = 110;
            // 
            // colConcepto
            // 
            colConcepto.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colConcepto.HeaderText = "Trabajo o concepto";
            colConcepto.MinimumWidth = 250;
            colConcepto.Name = "colConcepto";
            colConcepto.ReadOnly = true;
            // 
            // colCantidad
            // 
            colCantidad.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCantidad.DefaultCellStyle = dataGridViewCellStyle1;
            colCantidad.HeaderText = "Cantidad";
            colCantidad.Name = "colCantidad";
            colCantidad.ReadOnly = true;
            // 
            // colEstado
            // 
            colEstado.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEstado.DefaultCellStyle = dataGridViewCellStyle2;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            colEstado.Width = 150;
            // 
            // grpResultadoTrabajo
            // 
            grpResultadoTrabajo.BackColor = Color.White;
            grpResultadoTrabajo.Controls.Add(tlpResultado);
            grpResultadoTrabajo.Dock = DockStyle.Fill;
            grpResultadoTrabajo.Location = new Point(16, 336);
            grpResultadoTrabajo.Margin = new Padding(0, 0, 0, 8);
            grpResultadoTrabajo.Name = "grpResultadoTrabajo";
            grpResultadoTrabajo.Padding = new Padding(10);
            grpResultadoTrabajo.Size = new Size(852, 220);
            grpResultadoTrabajo.TabIndex = 5;
            grpResultadoTrabajo.TabStop = false;
            grpResultadoTrabajo.Text = "Resultado del trabajo";
            // 
            // tlpResultado
            // 
            tlpResultado.ColumnCount = 4;
            tlpResultado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tlpResultado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tlpResultado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tlpResultado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tlpResultado.Controls.Add(label1, 0, 0);
            tlpResultado.Controls.Add(dtpFechaFinalizacion, 1, 0);
            tlpResultado.Controls.Add(label2, 2, 0);
            tlpResultado.Controls.Add(cboResultadoTrabajo, 3, 0);
            tlpResultado.Controls.Add(lblTrabajoRealizadoTitulo, 0, 1);
            tlpResultado.Controls.Add(txtTrabajoRealizado, 0, 2);
            tlpResultado.Controls.Add(lblObservacionesTitulo, 0, 3);
            tlpResultado.Controls.Add(txtObservaciones, 0, 4);
            tlpResultado.Controls.Add(flpConfirmaciones, 0, 5);
            tlpResultado.Dock = DockStyle.Fill;
            tlpResultado.Location = new Point(10, 26);
            tlpResultado.Name = "tlpResultado";
            tlpResultado.Padding = new Padding(6);
            tlpResultado.RowCount = 6;
            tlpResultado.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpResultado.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpResultado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResultado.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpResultado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResultado.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tlpResultado.Size = new Size(832, 184);
            tlpResultado.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(9, 6);
            label1.Name = "label1";
            label1.Size = new Size(141, 36);
            label1.TabIndex = 0;
            label1.Text = "Fecha de finalización:";
            // 
            // dtpFechaFinalizacion
            // 
            dtpFechaFinalizacion.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpFechaFinalizacion.Dock = DockStyle.Fill;
            dtpFechaFinalizacion.Format = DateTimePickerFormat.Custom;
            dtpFechaFinalizacion.Location = new Point(156, 9);
            dtpFechaFinalizacion.Name = "dtpFechaFinalizacion";
            dtpFechaFinalizacion.Size = new Size(256, 23);
            dtpFechaFinalizacion.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(418, 6);
            label2.Name = "label2";
            label2.Size = new Size(141, 36);
            label2.TabIndex = 2;
            label2.Text = "Resultado:";
            // 
            // cboResultadoTrabajo
            // 
            cboResultadoTrabajo.Dock = DockStyle.Fill;
            cboResultadoTrabajo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboResultadoTrabajo.FormattingEnabled = true;
            cboResultadoTrabajo.Location = new Point(565, 9);
            cboResultadoTrabajo.Name = "cboResultadoTrabajo";
            cboResultadoTrabajo.Size = new Size(258, 23);
            cboResultadoTrabajo.TabIndex = 3;
            cboResultadoTrabajo.SelectedIndexChanged += ResultadoTrabajo_Cambio;
            // 
            // lblTrabajoRealizadoTitulo
            // 
            lblTrabajoRealizadoTitulo.AutoSize = true;
            tlpResultado.SetColumnSpan(lblTrabajoRealizadoTitulo, 4);
            lblTrabajoRealizadoTitulo.Dock = DockStyle.Fill;
            lblTrabajoRealizadoTitulo.Location = new Point(9, 42);
            lblTrabajoRealizadoTitulo.Name = "lblTrabajoRealizadoTitulo";
            lblTrabajoRealizadoTitulo.Size = new Size(814, 22);
            lblTrabajoRealizadoTitulo.TabIndex = 4;
            lblTrabajoRealizadoTitulo.Text = "Trabajo realizado:";
            lblTrabajoRealizadoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTrabajoRealizado
            // 
            txtTrabajoRealizado.BorderStyle = BorderStyle.FixedSingle;
            tlpResultado.SetColumnSpan(txtTrabajoRealizado, 4);
            txtTrabajoRealizado.Dock = DockStyle.Fill;
            txtTrabajoRealizado.Location = new Point(9, 67);
            txtTrabajoRealizado.MaxLength = 1000;
            txtTrabajoRealizado.Multiline = true;
            txtTrabajoRealizado.Name = "txtTrabajoRealizado";
            txtTrabajoRealizado.ScrollBars = ScrollBars.Vertical;
            txtTrabajoRealizado.Size = new Size(814, 21);
            txtTrabajoRealizado.TabIndex = 5;
            txtTrabajoRealizado.TextChanged += CampoValidado_Cambio;
            // 
            // lblObservacionesTitulo
            // 
            lblObservacionesTitulo.AutoSize = true;
            tlpResultado.SetColumnSpan(lblObservacionesTitulo, 4);
            lblObservacionesTitulo.Dock = DockStyle.Fill;
            lblObservacionesTitulo.Location = new Point(9, 91);
            lblObservacionesTitulo.Name = "lblObservacionesTitulo";
            lblObservacionesTitulo.Size = new Size(814, 22);
            lblObservacionesTitulo.TabIndex = 6;
            lblObservacionesTitulo.Text = "Observaciones técnicas:";
            lblObservacionesTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtObservaciones
            // 
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            tlpResultado.SetColumnSpan(txtObservaciones, 4);
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Location = new Point(9, 116);
            txtObservaciones.MaxLength = 1000;
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(814, 21);
            txtObservaciones.TabIndex = 7;
            // 
            // flpConfirmaciones
            // 
            flpConfirmaciones.AutoScroll = true;
            tlpResultado.SetColumnSpan(flpConfirmaciones, 4);
            flpConfirmaciones.Controls.Add(chkConfirmarFinalizacion);
            flpConfirmaciones.Controls.Add(chkPruebaFuncionamiento);
            flpConfirmaciones.Dock = DockStyle.Fill;
            flpConfirmaciones.Location = new Point(9, 143);
            flpConfirmaciones.Name = "flpConfirmaciones";
            flpConfirmaciones.Size = new Size(814, 32);
            flpConfirmaciones.TabIndex = 8;
            flpConfirmaciones.WrapContents = false;
            // 
            // chkConfirmarFinalizacion
            // 
            chkConfirmarFinalizacion.AutoSize = true;
            chkConfirmarFinalizacion.Location = new Point(3, 7);
            chkConfirmarFinalizacion.Margin = new Padding(3, 7, 3, 3);
            chkConfirmarFinalizacion.Name = "chkConfirmarFinalizacion";
            chkConfirmarFinalizacion.Size = new Size(336, 19);
            chkConfirmarFinalizacion.TabIndex = 0;
            chkConfirmarFinalizacion.Text = "Confirmo que el trabajo técnico puede darse por finalizado";
            chkConfirmarFinalizacion.UseVisualStyleBackColor = true;
            // 
            // chkPruebaFuncionamiento
            // 
            chkPruebaFuncionamiento.AutoSize = true;
            chkPruebaFuncionamiento.Location = new Point(345, 7);
            chkPruebaFuncionamiento.Margin = new Padding(3, 7, 25, 3);
            chkPruebaFuncionamiento.Name = "chkPruebaFuncionamiento";
            chkPruebaFuncionamiento.Size = new Size(242, 19);
            chkPruebaFuncionamiento.TabIndex = 1;
            chkPruebaFuncionamiento.Text = "Se realizó una prueba de funcionamiento";
            chkPruebaFuncionamiento.UseVisualStyleBackColor = true;
            // 
            // flpBotones
            // 
            flpBotones.Controls.Add(btnFinalizarTrabajo);
            flpBotones.Controls.Add(btnCancelar);
            flpBotones.Dock = DockStyle.Fill;
            flpBotones.FlowDirection = FlowDirection.RightToLeft;
            flpBotones.Location = new Point(16, 564);
            flpBotones.Margin = new Padding(0);
            flpBotones.Name = "flpBotones";
            flpBotones.Padding = new Padding(0, 10, 0, 0);
            flpBotones.Size = new Size(852, 61);
            flpBotones.TabIndex = 6;
            flpBotones.WrapContents = false;
            // 
            // btnFinalizarTrabajo
            // 
            btnFinalizarTrabajo.Location = new Point(682, 10);
            btnFinalizarTrabajo.Margin = new Padding(8, 0, 0, 0);
            btnFinalizarTrabajo.Name = "btnFinalizarTrabajo";
            btnFinalizarTrabajo.Size = new Size(170, 38);
            btnFinalizarTrabajo.TabIndex = 0;
            btnFinalizarTrabajo.Text = "Finalizar trabajo";
            btnFinalizarTrabajo.UseVisualStyleBackColor = true;
            btnFinalizarTrabajo.Click += BtnFinalizarTrabajo_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(554, 10);
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // FrmFinalizarTrabajo
            // 
            AcceptButton = btnFinalizarTrabajo;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            CancelButton = btnCancelar;
            ClientSize = new Size(884, 641);
            Controls.Add(tlpPrincipal);
            MinimizeBox = false;
            MinimumSize = new Size(820, 620);
            Name = "FrmFinalizarTrabajo";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Finalizar trabajo";
            FormClosing += FrmFinalizarTrabajo_FormClosing;
            Load += FrmFinalizarTrabajo_Load;
            Shown += FrmFinalizarTrabajo_Shown;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            grpAtencion.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            grpTrabajosAutorizados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTrabajosAutorizados).EndInit();
            grpResultadoTrabajo.ResumeLayout(false);
            tlpResultado.ResumeLayout(false);
            tlpResultado.PerformLayout();
            flpConfirmaciones.ResumeLayout(false);
            flpConfirmaciones.PerformLayout();
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
        private GroupBox grpAtencion;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblClienteTitulo;
        private Label lblClienteValor;
        private Label lblVehiculoTitulo;
        private Label lblVehiculoValor;
        private Label lblDominioTitulo;
        private Label lblDominioValor;
        private Label lblTecnicoTitulo;
        private Label lblTecnicoValor;
        private Label lblDiagnosticoTitulo;
        private Label lblDiagnosticoValor;
        private Label lblPresupuestoTitulo;
        private Label lblPresupuestoValor;
        private GroupBox grpTrabajosAutorizados;
        private DataGridView dgvTrabajosAutorizados;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colTipo;
        private DataGridViewTextBoxColumn colConcepto;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colEstado;
        private GroupBox grpResultadoTrabajo;
        private TableLayoutPanel tlpResultado;
        private Label label1;
        private DateTimePicker dtpFechaFinalizacion;
        private Label label2;
        private ComboBox cboResultadoTrabajo;
        private Label lblTrabajoRealizadoTitulo;
        private TextBox txtTrabajoRealizado;
        private Label lblObservacionesTitulo;
        private TextBox txtObservaciones;
        private FlowLayoutPanel flpConfirmaciones;
        private CheckBox chkConfirmarFinalizacion;
        private CheckBox chkPruebaFuncionamiento;
        private FlowLayoutPanel flpBotones;
        private Button btnFinalizarTrabajo;
        private Button btnCancelar;
    }
}
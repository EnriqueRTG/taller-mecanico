namespace Taller.Presentacion.Formularios.Comprobantes
{
    partial class FrmComprobantePagos
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tlpPrincipal = new TableLayoutPanel();
            pnlAcciones = new Panel();
            flpAcciones = new FlowLayoutPanel();
            btnCerrar = new Button();
            btnEmitirComprobante = new Button();
            btnRegistrarPago = new Button();
            grpPagos = new GroupBox();
            dgvPagos = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colMedioPago = new DataGridViewTextBoxColumn();
            colReferencia = new DataGridViewTextBoxColumn();
            colImporte = new DataGridViewTextBoxColumn();
            colRegistradoPor = new DataGridViewTextBoxColumn();
            grpResumenFinanciero = new GroupBox();
            tlpResumenFinanciero = new TableLayoutPanel();
            lblEstadoTitulo = new Label();
            lblNumeroTitulo = new Label();
            lblAbonadoTitulo = new Label();
            lblSaldoTitulo = new Label();
            lblEstadoValor = new Label();
            lblNumeroValor = new Label();
            lblAbonadoValor = new Label();
            lblSaldoValor = new Label();
            grpAtencion = new GroupBox();
            tlpAtencion = new TableLayoutPanel();
            lblClienteTitulo = new Label();
            lblVehiculoTitulo = new Label();
            lblDominioTitulo = new Label();
            lblPresupuestoTitulo = new Label();
            lblClienteValor = new Label();
            lblVehiculoValor = new Label();
            lblDominioValor = new Label();
            lblPresupuestoValor = new Label();
            pnlEncabezado = new Panel();
            tplEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            lblNumeroAtencion = new Label();
            tlpPrincipal.SuspendLayout();
            pnlAcciones.SuspendLayout();
            flpAcciones.SuspendLayout();
            grpPagos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            grpResumenFinanciero.SuspendLayout();
            tlpResumenFinanciero.SuspendLayout();
            grpAtencion.SuspendLayout();
            tlpAtencion.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tplEncabezado.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlAcciones, 0, 4);
            tlpPrincipal.Controls.Add(grpPagos, 0, 3);
            tlpPrincipal.Controls.Add(grpResumenFinanciero, 0, 2);
            tlpPrincipal.Controls.Add(grpAtencion, 0, 1);
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(18);
            tlpPrincipal.RowCount = 5;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tlpPrincipal.Size = new Size(884, 571);
            tlpPrincipal.TabIndex = 0;
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.White;
            pnlAcciones.Controls.Add(flpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(21, 491);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Padding = new Padding(8);
            pnlAcciones.Size = new Size(842, 59);
            pnlAcciones.TabIndex = 5;
            // 
            // flpAcciones
            // 
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnCerrar);
            flpAcciones.Controls.Add(btnEmitirComprobante);
            flpAcciones.Controls.Add(btnRegistrarPago);
            flpAcciones.Dock = DockStyle.Right;
            flpAcciones.Location = new Point(401, 8);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Size = new Size(433, 43);
            flpAcciones.TabIndex = 0;
            flpAcciones.WrapContents = false;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(3, 3);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.TabIndex = 0;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += BtnCerrar_Click;
            // 
            // btnEmitirComprobante
            // 
            btnEmitirComprobante.Location = new Point(119, 3);
            btnEmitirComprobante.Name = "btnEmitirComprobante";
            btnEmitirComprobante.Size = new Size(160, 36);
            btnEmitirComprobante.TabIndex = 1;
            btnEmitirComprobante.Text = "Emitir comprobante";
            btnEmitirComprobante.UseVisualStyleBackColor = true;
            btnEmitirComprobante.Click += BtnEmitirComprobante_Click;
            // 
            // btnRegistrarPago
            // 
            btnRegistrarPago.Location = new Point(285, 3);
            btnRegistrarPago.Name = "btnRegistrarPago";
            btnRegistrarPago.Size = new Size(145, 36);
            btnRegistrarPago.TabIndex = 2;
            btnRegistrarPago.Text = "Registrar pago";
            btnRegistrarPago.UseVisualStyleBackColor = true;
            btnRegistrarPago.Click += BtnRegistrarPago_Click;
            // 
            // grpPagos
            // 
            grpPagos.BackColor = Color.White;
            grpPagos.Controls.Add(dgvPagos);
            grpPagos.Dock = DockStyle.Fill;
            grpPagos.Location = new Point(18, 333);
            grpPagos.Margin = new Padding(0, 0, 0, 10);
            grpPagos.Name = "grpPagos";
            grpPagos.Padding = new Padding(12);
            grpPagos.Size = new Size(848, 145);
            grpPagos.TabIndex = 4;
            grpPagos.TabStop = false;
            grpPagos.Text = "Pagos registrados";
            // 
            // dgvPagos
            // 
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dgvPagos.AllowUserToResizeRows = false;
            dgvPagos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvPagos.BackgroundColor = Color.White;
            dgvPagos.BorderStyle = BorderStyle.None;
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPagos.Columns.AddRange(new DataGridViewColumn[] { colFecha, colMedioPago, colReferencia, colImporte, colRegistradoPor });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvPagos.DefaultCellStyle = dataGridViewCellStyle4;
            dgvPagos.Dock = DockStyle.Fill;
            dgvPagos.Location = new Point(12, 28);
            dgvPagos.MultiSelect = false;
            dgvPagos.Name = "dgvPagos";
            dgvPagos.ReadOnly = true;
            dgvPagos.RowHeadersVisible = false;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(824, 105);
            dgvPagos.TabIndex = 0;
            // 
            // colFecha
            // 
            colFecha.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colFecha.FillWeight = 258.88324F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            colFecha.Width = 150;
            // 
            // colMedioPago
            // 
            colMedioPago.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colMedioPago.FillWeight = 297.8554F;
            colMedioPago.HeaderText = "Medio de pago";
            colMedioPago.Name = "colMedioPago";
            colMedioPago.ReadOnly = true;
            colMedioPago.Width = 150;
            // 
            // colReferencia
            // 
            colReferencia.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReferencia.FillWeight = 37.7518158F;
            colReferencia.HeaderText = "Referencia";
            colReferencia.Name = "colReferencia";
            colReferencia.ReadOnly = true;
            colReferencia.Width = 175;
            // 
            // colImporte
            // 
            colImporte.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colImporte.FillWeight = 5.18014765F;
            colImporte.HeaderText = "Importe";
            colImporte.Name = "colImporte";
            colImporte.ReadOnly = true;
            colImporte.Width = 150;
            // 
            // colRegistradoPor
            // 
            colRegistradoPor.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colRegistradoPor.FillWeight = 0.315886974F;
            colRegistradoPor.HeaderText = "Registrado por";
            colRegistradoPor.Name = "colRegistradoPor";
            colRegistradoPor.ReadOnly = true;
            colRegistradoPor.Width = 250;
            // 
            // grpResumenFinanciero
            // 
            grpResumenFinanciero.BackColor = Color.White;
            grpResumenFinanciero.Controls.Add(tlpResumenFinanciero);
            grpResumenFinanciero.Dock = DockStyle.Fill;
            grpResumenFinanciero.Location = new Point(18, 213);
            grpResumenFinanciero.Margin = new Padding(0, 0, 0, 10);
            grpResumenFinanciero.Name = "grpResumenFinanciero";
            grpResumenFinanciero.Padding = new Padding(12);
            grpResumenFinanciero.Size = new Size(848, 110);
            grpResumenFinanciero.TabIndex = 3;
            grpResumenFinanciero.TabStop = false;
            grpResumenFinanciero.Text = "Estado del comprobante";
            // 
            // tlpResumenFinanciero
            // 
            tlpResumenFinanciero.ColumnCount = 4;
            tlpResumenFinanciero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumenFinanciero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumenFinanciero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumenFinanciero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumenFinanciero.Controls.Add(lblEstadoTitulo, 0, 0);
            tlpResumenFinanciero.Controls.Add(lblNumeroTitulo, 1, 0);
            tlpResumenFinanciero.Controls.Add(lblAbonadoTitulo, 2, 0);
            tlpResumenFinanciero.Controls.Add(lblSaldoTitulo, 3, 0);
            tlpResumenFinanciero.Controls.Add(lblEstadoValor, 0, 1);
            tlpResumenFinanciero.Controls.Add(lblNumeroValor, 1, 1);
            tlpResumenFinanciero.Controls.Add(lblAbonadoValor, 2, 1);
            tlpResumenFinanciero.Controls.Add(lblSaldoValor, 3, 1);
            tlpResumenFinanciero.Dock = DockStyle.Fill;
            tlpResumenFinanciero.Location = new Point(12, 28);
            tlpResumenFinanciero.Name = "tlpResumenFinanciero";
            tlpResumenFinanciero.Padding = new Padding(8);
            tlpResumenFinanciero.RowCount = 2;
            tlpResumenFinanciero.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumenFinanciero.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumenFinanciero.Size = new Size(824, 70);
            tlpResumenFinanciero.TabIndex = 0;
            // 
            // lblEstadoTitulo
            // 
            lblEstadoTitulo.AutoSize = true;
            lblEstadoTitulo.Dock = DockStyle.Fill;
            lblEstadoTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblEstadoTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblEstadoTitulo.Location = new Point(11, 8);
            lblEstadoTitulo.Name = "lblEstadoTitulo";
            lblEstadoTitulo.Size = new Size(196, 27);
            lblEstadoTitulo.TabIndex = 0;
            lblEstadoTitulo.Text = "COMPROBANTE";
            lblEstadoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNumeroTitulo
            // 
            lblNumeroTitulo.AutoSize = true;
            lblNumeroTitulo.Dock = DockStyle.Fill;
            lblNumeroTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblNumeroTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblNumeroTitulo.Location = new Point(213, 8);
            lblNumeroTitulo.Name = "lblNumeroTitulo";
            lblNumeroTitulo.Size = new Size(196, 27);
            lblNumeroTitulo.TabIndex = 1;
            lblNumeroTitulo.Text = "NÚMERO";
            lblNumeroTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAbonadoTitulo
            // 
            lblAbonadoTitulo.AutoSize = true;
            lblAbonadoTitulo.Dock = DockStyle.Fill;
            lblAbonadoTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblAbonadoTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblAbonadoTitulo.Location = new Point(415, 8);
            lblAbonadoTitulo.Name = "lblAbonadoTitulo";
            lblAbonadoTitulo.Size = new Size(196, 27);
            lblAbonadoTitulo.TabIndex = 2;
            lblAbonadoTitulo.Text = "TOTAL ABONADO";
            lblAbonadoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSaldoTitulo
            // 
            lblSaldoTitulo.AutoSize = true;
            lblSaldoTitulo.Dock = DockStyle.Fill;
            lblSaldoTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblSaldoTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblSaldoTitulo.Location = new Point(617, 8);
            lblSaldoTitulo.Name = "lblSaldoTitulo";
            lblSaldoTitulo.Size = new Size(196, 27);
            lblSaldoTitulo.TabIndex = 3;
            lblSaldoTitulo.Text = "SALDO PENDIENTE";
            lblSaldoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEstadoValor
            // 
            lblEstadoValor.AutoSize = true;
            lblEstadoValor.Dock = DockStyle.Fill;
            lblEstadoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEstadoValor.ForeColor = Color.FromArgb(180, 83, 9);
            lblEstadoValor.Location = new Point(11, 35);
            lblEstadoValor.Name = "lblEstadoValor";
            lblEstadoValor.Size = new Size(196, 27);
            lblEstadoValor.TabIndex = 4;
            lblEstadoValor.Text = "No emitido";
            // 
            // lblNumeroValor
            // 
            lblNumeroValor.AutoSize = true;
            lblNumeroValor.Dock = DockStyle.Fill;
            lblNumeroValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNumeroValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblNumeroValor.Location = new Point(213, 35);
            lblNumeroValor.Name = "lblNumeroValor";
            lblNumeroValor.Size = new Size(196, 27);
            lblNumeroValor.TabIndex = 5;
            lblNumeroValor.Text = "Sin asignar";
            // 
            // lblAbonadoValor
            // 
            lblAbonadoValor.AutoSize = true;
            lblAbonadoValor.Dock = DockStyle.Fill;
            lblAbonadoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAbonadoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblAbonadoValor.Location = new Point(415, 35);
            lblAbonadoValor.Name = "lblAbonadoValor";
            lblAbonadoValor.Size = new Size(196, 27);
            lblAbonadoValor.TabIndex = 6;
            lblAbonadoValor.Text = "$ 0,00";
            // 
            // lblSaldoValor
            // 
            lblSaldoValor.AutoSize = true;
            lblSaldoValor.Dock = DockStyle.Fill;
            lblSaldoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSaldoValor.ForeColor = Color.FromArgb(185, 28, 28);
            lblSaldoValor.Location = new Point(617, 35);
            lblSaldoValor.Name = "lblSaldoValor";
            lblSaldoValor.Size = new Size(196, 27);
            lblSaldoValor.TabIndex = 7;
            lblSaldoValor.Text = "$ 43.500,00";
            // 
            // grpAtencion
            // 
            grpAtencion.BackColor = Color.White;
            grpAtencion.Controls.Add(tlpAtencion);
            grpAtencion.Dock = DockStyle.Fill;
            grpAtencion.Location = new Point(18, 103);
            grpAtencion.Margin = new Padding(0, 0, 0, 10);
            grpAtencion.Name = "grpAtencion";
            grpAtencion.Padding = new Padding(12);
            grpAtencion.Size = new Size(848, 100);
            grpAtencion.TabIndex = 2;
            grpAtencion.TabStop = false;
            grpAtencion.Text = "Atención seleccionada";
            // 
            // tlpAtencion
            // 
            tlpAtencion.ColumnCount = 4;
            tlpAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpAtencion.Controls.Add(lblClienteTitulo, 0, 0);
            tlpAtencion.Controls.Add(lblVehiculoTitulo, 1, 0);
            tlpAtencion.Controls.Add(lblDominioTitulo, 2, 0);
            tlpAtencion.Controls.Add(lblPresupuestoTitulo, 3, 0);
            tlpAtencion.Controls.Add(lblClienteValor, 0, 1);
            tlpAtencion.Controls.Add(lblVehiculoValor, 1, 1);
            tlpAtencion.Controls.Add(lblDominioValor, 2, 1);
            tlpAtencion.Controls.Add(lblPresupuestoValor, 3, 1);
            tlpAtencion.Dock = DockStyle.Fill;
            tlpAtencion.Location = new Point(12, 28);
            tlpAtencion.Name = "tlpAtencion";
            tlpAtencion.Padding = new Padding(8);
            tlpAtencion.RowCount = 2;
            tlpAtencion.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpAtencion.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpAtencion.Size = new Size(824, 60);
            tlpAtencion.TabIndex = 0;
            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.AutoSize = true;
            lblClienteTitulo.Dock = DockStyle.Fill;
            lblClienteTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblClienteTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblClienteTitulo.Location = new Point(11, 8);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.Size = new Size(196, 22);
            lblClienteTitulo.TabIndex = 0;
            lblClienteTitulo.Text = "CLIENTE";
            lblClienteTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculoTitulo
            // 
            lblVehiculoTitulo.AutoSize = true;
            lblVehiculoTitulo.Dock = DockStyle.Fill;
            lblVehiculoTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblVehiculoTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblVehiculoTitulo.Location = new Point(213, 8);
            lblVehiculoTitulo.Name = "lblVehiculoTitulo";
            lblVehiculoTitulo.Size = new Size(196, 22);
            lblVehiculoTitulo.TabIndex = 1;
            lblVehiculoTitulo.Text = "VEHÍCULO";
            lblVehiculoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioTitulo
            // 
            lblDominioTitulo.AutoSize = true;
            lblDominioTitulo.Dock = DockStyle.Fill;
            lblDominioTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblDominioTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblDominioTitulo.Location = new Point(415, 8);
            lblDominioTitulo.Name = "lblDominioTitulo";
            lblDominioTitulo.Size = new Size(196, 22);
            lblDominioTitulo.TabIndex = 2;
            lblDominioTitulo.Text = "DOMINIO";
            lblDominioTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPresupuestoTitulo
            // 
            lblPresupuestoTitulo.AutoSize = true;
            lblPresupuestoTitulo.Dock = DockStyle.Fill;
            lblPresupuestoTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblPresupuestoTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblPresupuestoTitulo.Location = new Point(617, 8);
            lblPresupuestoTitulo.Name = "lblPresupuestoTitulo";
            lblPresupuestoTitulo.Size = new Size(196, 22);
            lblPresupuestoTitulo.TabIndex = 3;
            lblPresupuestoTitulo.Text = "PRESUPUESTO";
            lblPresupuestoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClienteValor
            // 
            lblClienteValor.AutoSize = true;
            lblClienteValor.Dock = DockStyle.Fill;
            lblClienteValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblClienteValor.Location = new Point(11, 30);
            lblClienteValor.Name = "lblClienteValor";
            lblClienteValor.Size = new Size(196, 22);
            lblClienteValor.TabIndex = 4;
            lblClienteValor.Text = "Juan Pérez";
            // 
            // lblVehiculoValor
            // 
            lblVehiculoValor.AutoSize = true;
            lblVehiculoValor.Dock = DockStyle.Fill;
            lblVehiculoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVehiculoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblVehiculoValor.Location = new Point(213, 30);
            lblVehiculoValor.Name = "lblVehiculoValor";
            lblVehiculoValor.Size = new Size(196, 22);
            lblVehiculoValor.TabIndex = 5;
            lblVehiculoValor.Text = "Honda Fit";
            // 
            // lblDominioValor
            // 
            lblDominioValor.AutoSize = true;
            lblDominioValor.Dock = DockStyle.Fill;
            lblDominioValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDominioValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblDominioValor.Location = new Point(415, 30);
            lblDominioValor.Name = "lblDominioValor";
            lblDominioValor.Size = new Size(196, 22);
            lblDominioValor.TabIndex = 6;
            lblDominioValor.Text = "AB345CD";
            // 
            // lblPresupuestoValor
            // 
            lblPresupuestoValor.AutoSize = true;
            lblPresupuestoValor.Dock = DockStyle.Fill;
            lblPresupuestoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPresupuestoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblPresupuestoValor.Location = new Point(617, 30);
            lblPresupuestoValor.Name = "lblPresupuestoValor";
            lblPresupuestoValor.Size = new Size(196, 22);
            lblPresupuestoValor.TabIndex = 7;
            lblPresupuestoValor.Text = "$ 43.500,00";
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tplEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(18, 18);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 10);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(848, 75);
            pnlEncabezado.TabIndex = 1;
            // 
            // tplEncabezado
            // 
            tplEncabezado.ColumnCount = 2;
            tplEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tplEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tplEncabezado.Controls.Add(lblTitulo, 0, 0);
            tplEncabezado.Controls.Add(lblDescripcion, 0, 1);
            tplEncabezado.Controls.Add(lblNumeroAtencion, 1, 0);
            tplEncabezado.Dock = DockStyle.Fill;
            tplEncabezado.Location = new Point(0, 0);
            tplEncabezado.Name = "tplEncabezado";
            tplEncabezado.RowCount = 2;
            tplEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tplEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tplEncabezado.Size = new Size(848, 75);
            tplEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(3, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(587, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Comprobante y pagos";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(3, 37);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(587, 38);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Consulte el comprobante y registre los pagos de la atención.";
            // 
            // lblNumeroAtencion
            // 
            lblNumeroAtencion.AutoSize = true;
            lblNumeroAtencion.Dock = DockStyle.Fill;
            lblNumeroAtencion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumeroAtencion.ForeColor = Color.FromArgb(30, 64, 175);
            lblNumeroAtencion.Location = new Point(596, 0);
            lblNumeroAtencion.Name = "lblNumeroAtencion";
            lblNumeroAtencion.Size = new Size(249, 37);
            lblNumeroAtencion.TabIndex = 2;
            lblNumeroAtencion.Text = "Atención N.º 000001";
            lblNumeroAtencion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // FrmComprobantePagos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(884, 571);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmComprobantePagos";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Comprobante y pagos";
            Load += FrmComprobantePagos_Load;
            tlpPrincipal.ResumeLayout(false);
            pnlAcciones.ResumeLayout(false);
            pnlAcciones.PerformLayout();
            flpAcciones.ResumeLayout(false);
            grpPagos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            grpResumenFinanciero.ResumeLayout(false);
            tlpResumenFinanciero.ResumeLayout(false);
            tlpResumenFinanciero.PerformLayout();
            grpAtencion.ResumeLayout(false);
            tlpAtencion.ResumeLayout(false);
            tlpAtencion.PerformLayout();
            pnlEncabezado.ResumeLayout(false);
            tplEncabezado.ResumeLayout(false);
            tplEncabezado.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private TableLayoutPanel tplEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblNumeroAtencion;
        private GroupBox grpAtencion;
        private TableLayoutPanel tlpAtencion;
        private Label lblClienteTitulo;
        private Label lblVehiculoTitulo;
        private Label lblDominioTitulo;
        private Label lblPresupuestoTitulo;
        private Label lblClienteValor;
        private Label lblVehiculoValor;
        private Label lblDominioValor;
        private Label lblPresupuestoValor;
        private GroupBox grpResumenFinanciero;
        private TableLayoutPanel tlpResumenFinanciero;
        private Label lblEstadoTitulo;
        private Label lblNumeroTitulo;
        private Label lblAbonadoTitulo;
        private Label lblSaldoTitulo;
        private Label lblEstadoValor;
        private Label lblNumeroValor;
        private Label lblAbonadoValor;
        private Label lblSaldoValor;
        private GroupBox grpPagos;
        private DataGridView dgvPagos;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colMedioPago;
        private DataGridViewTextBoxColumn colReferencia;
        private DataGridViewTextBoxColumn colImporte;
        private DataGridViewTextBoxColumn colRegistradoPor;
        private Panel pnlAcciones;
        private FlowLayoutPanel flpAcciones;
        private Button btnCerrar;
        private Button btnEmitirComprobante;
        private Button btnRegistrarPago;
    }
}
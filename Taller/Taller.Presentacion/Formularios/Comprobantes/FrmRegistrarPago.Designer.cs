namespace Taller.Presentacion.Formularios.Comprobantes
{
    partial class FrmRegistrarPago
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
            tlpPrincipal = new TableLayoutPanel();
            grpDatosPagos = new GroupBox();
            tlpResumen = new TableLayoutPanel();
            pnlEncabezado = new Panel();
            tlpEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            lblSaldoDisponible = new Label();
            pnlAcciones = new Panel();
            flpAcciones = new FlowLayoutPanel();
            btnGuardar = new Button();
            btnCancelar = new Button();
            lblFechaHora = new Label();
            lblMedioPago = new Label();
            lblImporte = new Label();
            lblReferencia = new Label();
            lblObservaciones = new Label();
            dtpFechaPago = new DateTimePicker();
            cboMedioPago = new ComboBox();
            nudImporte = new NumericUpDown();
            txtReferencia = new TextBox();
            txtObservaciones = new TextBox();
            tlpPrincipal.SuspendLayout();
            grpDatosPagos.SuspendLayout();
            tlpResumen.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            pnlAcciones.SuspendLayout();
            flpAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudImporte).BeginInit();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(grpDatosPagos, 0, 1);
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(pnlAcciones, 0, 2);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 3;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tlpPrincipal.Size = new Size(684, 461);
            tlpPrincipal.TabIndex = 0;
            // 
            // grpDatosPagos
            // 
            grpDatosPagos.BackColor = Color.White;
            grpDatosPagos.Controls.Add(tlpResumen);
            grpDatosPagos.Dock = DockStyle.Fill;
            grpDatosPagos.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpDatosPagos.ForeColor = Color.FromArgb(15, 23, 42);
            grpDatosPagos.Location = new Point(0, 82);
            grpDatosPagos.Margin = new Padding(0, 0, 0, 10);
            grpDatosPagos.Name = "grpDatosPagos";
            grpDatosPagos.Padding = new Padding(12);
            grpDatosPagos.Size = new Size(684, 299);
            grpDatosPagos.TabIndex = 2;
            grpDatosPagos.TabStop = false;
            grpDatosPagos.Text = "Datos del pago";
            // 
            // tlpResumen
            // 
            tlpResumen.ColumnCount = 2;
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tlpResumen.Controls.Add(lblFechaHora, 0, 0);
            tlpResumen.Controls.Add(lblMedioPago, 0, 1);
            tlpResumen.Controls.Add(lblImporte, 0, 2);
            tlpResumen.Controls.Add(lblReferencia, 0, 3);
            tlpResumen.Controls.Add(lblObservaciones, 0, 4);
            tlpResumen.Controls.Add(dtpFechaPago, 1, 0);
            tlpResumen.Controls.Add(cboMedioPago, 1, 1);
            tlpResumen.Controls.Add(nudImporte, 1, 2);
            tlpResumen.Controls.Add(txtReferencia, 1, 3);
            tlpResumen.Controls.Add(txtObservaciones, 1, 4);
            tlpResumen.Dock = DockStyle.Fill;
            tlpResumen.Location = new Point(12, 28);
            tlpResumen.Margin = new Padding(0);
            tlpResumen.Name = "tlpResumen";
            tlpResumen.Padding = new Padding(8);
            tlpResumen.RowCount = 5;
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpResumen.Size = new Size(660, 259);
            tlpResumen.TabIndex = 0;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tlpEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 10);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(18, 12, 18, 12);
            pnlEncabezado.Size = new Size(684, 72);
            pnlEncabezado.TabIndex = 1;
            // 
            // tlpEncabezado
            // 
            tlpEncabezado.ColumnCount = 2;
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpEncabezado.Controls.Add(lblTitulo, 0, 0);
            tlpEncabezado.Controls.Add(lblDescripcion, 0, 1);
            tlpEncabezado.Controls.Add(lblSaldoDisponible, 1, 0);
            tlpEncabezado.Dock = DockStyle.Fill;
            tlpEncabezado.Location = new Point(18, 12);
            tlpEncabezado.Margin = new Padding(0);
            tlpEncabezado.Name = "tlpEncabezado";
            tlpEncabezado.RowCount = 2;
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpEncabezado.Size = new Size(648, 48);
            tlpEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(3, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(447, 24);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registrar pago";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(3, 24);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(447, 24);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Ingrese los datos del pago recibido.";
            // 
            // lblSaldoDisponible
            // 
            lblSaldoDisponible.AutoSize = true;
            lblSaldoDisponible.Dock = DockStyle.Fill;
            lblSaldoDisponible.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSaldoDisponible.ForeColor = Color.FromArgb(30, 64, 175);
            lblSaldoDisponible.Location = new Point(456, 0);
            lblSaldoDisponible.Name = "lblSaldoDisponible";
            lblSaldoDisponible.Size = new Size(189, 24);
            lblSaldoDisponible.TabIndex = 2;
            lblSaldoDisponible.Text = "$ SALDO";
            lblSaldoDisponible.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlAcciones
            // 
            pnlAcciones.Controls.Add(flpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(3, 394);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(678, 64);
            pnlAcciones.TabIndex = 3;
            // 
            // flpAcciones
            // 
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Dock = DockStyle.Right;
            flpAcciones.FlowDirection = FlowDirection.RightToLeft;
            flpAcciones.Location = new Point(411, 0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Padding = new Padding(0, 10, 0, 0);
            flpAcciones.Size = new Size(267, 64);
            flpAcciones.TabIndex = 4;
            flpAcciones.WrapContents = false;
            // 
            // btnGuardar
            // 
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.Location = new Point(119, 13);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(145, 36);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Registrar pago";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.Location = new Point(3, 13);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 36);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // lblFechaHora
            // 
            lblFechaHora.AutoSize = true;
            lblFechaHora.Dock = DockStyle.Fill;
            lblFechaHora.Location = new Point(11, 8);
            lblFechaHora.Name = "lblFechaHora";
            lblFechaHora.Size = new Size(122, 35);
            lblFechaHora.TabIndex = 0;
            lblFechaHora.Text = "Fecha y hora:";
            // 
            // lblMedioPago
            // 
            lblMedioPago.AutoSize = true;
            lblMedioPago.Dock = DockStyle.Fill;
            lblMedioPago.Location = new Point(11, 43);
            lblMedioPago.Name = "lblMedioPago";
            lblMedioPago.Size = new Size(122, 35);
            lblMedioPago.TabIndex = 1;
            lblMedioPago.Text = "Medio de pago:";
            // 
            // lblImporte
            // 
            lblImporte.AutoSize = true;
            lblImporte.Dock = DockStyle.Fill;
            lblImporte.Location = new Point(11, 78);
            lblImporte.Name = "lblImporte";
            lblImporte.Size = new Size(122, 35);
            lblImporte.TabIndex = 2;
            lblImporte.Text = "Importe:";
            // 
            // lblReferencia
            // 
            lblReferencia.AutoSize = true;
            lblReferencia.Dock = DockStyle.Fill;
            lblReferencia.Location = new Point(11, 113);
            lblReferencia.Name = "lblReferencia";
            lblReferencia.Size = new Size(122, 35);
            lblReferencia.TabIndex = 3;
            lblReferencia.Text = "Referencia:";
            // 
            // lblObservaciones
            // 
            lblObservaciones.AutoSize = true;
            lblObservaciones.Dock = DockStyle.Fill;
            lblObservaciones.Location = new Point(11, 148);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(122, 103);
            lblObservaciones.TabIndex = 4;
            lblObservaciones.Text = "Observaciones:";
            // 
            // dtpFechaPago
            // 
            dtpFechaPago.Format = DateTimePickerFormat.Custom;
            dtpFechaPago.Location = new Point(139, 11);
            dtpFechaPago.Name = "dtpFechaPago";
            dtpFechaPago.Size = new Size(220, 23);
            dtpFechaPago.TabIndex = 5;
            // 
            // cboMedioPago
            // 
            cboMedioPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMedioPago.FormattingEnabled = true;
            cboMedioPago.Location = new Point(139, 46);
            cboMedioPago.Name = "cboMedioPago";
            cboMedioPago.Size = new Size(220, 23);
            cboMedioPago.TabIndex = 6;
            // 
            // nudImporte
            // 
            nudImporte.DecimalPlaces = 2;
            nudImporte.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            nudImporte.Location = new Point(139, 81);
            nudImporte.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            nudImporte.Name = "nudImporte";
            nudImporte.Size = new Size(220, 23);
            nudImporte.TabIndex = 7;
            nudImporte.ThousandsSeparator = true;
            // 
            // txtReferencia
            // 
            txtReferencia.Location = new Point(139, 116);
            txtReferencia.MaxLength = 100;
            txtReferencia.Name = "txtReferencia";
            txtReferencia.Size = new Size(220, 23);
            txtReferencia.TabIndex = 8;
            // 
            // txtObservaciones
            // 
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Location = new Point(139, 151);
            txtObservaciones.MaxLength = 500;
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(510, 97);
            txtObservaciones.TabIndex = 9;
            // 
            // FrmRegistrarPago
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            CancelButton = btnCancelar;
            ClientSize = new Size(684, 461);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRegistrarPago";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar pago";
            Load += FrmRegistrarPago_Load;
            tlpPrincipal.ResumeLayout(false);
            grpDatosPagos.ResumeLayout(false);
            tlpResumen.ResumeLayout(false);
            tlpResumen.PerformLayout();
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            tlpEncabezado.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            pnlAcciones.PerformLayout();
            flpAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudImporte).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private TableLayoutPanel tlpEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblSaldoDisponible;
        private GroupBox grpDatosPagos;
        private TableLayoutPanel tlpResumen;
        private Panel pnlAcciones;
        private FlowLayoutPanel flpAcciones;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblFechaHora;
        private Label lblMedioPago;
        private Label lblImporte;
        private Label lblReferencia;
        private Label lblObservaciones;
        private DateTimePicker dtpFechaPago;
        private ComboBox cboMedioPago;
        private NumericUpDown nudImporte;
        private TextBox txtReferencia;
        private TextBox txtObservaciones;
    }
}
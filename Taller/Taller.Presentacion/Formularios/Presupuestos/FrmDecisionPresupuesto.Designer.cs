namespace Taller.Presentacion.Formularios.Presupuestos
{
    partial class FrmDecisionPresupuesto
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
            pnlEncabezado = new Panel();
            tplEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            lblNumeroPresupuesto = new Label();
            grpResumen = new GroupBox();
            tlpResumen = new TableLayoutPanel();
            lblAtencion = new Label();
            lblCliente = new Label();
            lblVehiculo = new Label();
            lblTotal = new Label();
            lblAtencionValor = new Label();
            lblClienteValor = new Label();
            lblVehiculoValor = new Label();
            lblTotalValor = new Label();
            grpDecision = new GroupBox();
            tlpDecision = new TableLayoutPanel();
            txtObservaciones = new TextBox();
            lblObservaciones = new Label();
            lblDecision = new Label();
            cboDecision = new ComboBox();
            lblFecha = new Label();
            dtpFechaDecision = new DateTimePicker();
            pnlAcciones = new Panel();
            flpAcciones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnRegistrar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tplEncabezado.SuspendLayout();
            grpResumen.SuspendLayout();
            tlpResumen.SuspendLayout();
            grpDecision.SuspendLayout();
            tlpDecision.SuspendLayout();
            pnlAcciones.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(grpResumen, 0, 1);
            tlpPrincipal.Controls.Add(grpDecision, 0, 2);
            tlpPrincipal.Controls.Add(pnlAcciones, 0, 3);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(18);
            tlpPrincipal.RowCount = 4;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tlpPrincipal.Size = new Size(704, 561);
            tlpPrincipal.TabIndex = 0;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tplEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(18, 18);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 10);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(668, 75);
            pnlEncabezado.TabIndex = 0;
            // 
            // tplEncabezado
            // 
            tplEncabezado.ColumnCount = 2;
            tplEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tplEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tplEncabezado.Controls.Add(lblTitulo, 0, 0);
            tplEncabezado.Controls.Add(lblDescripcion, 0, 1);
            tplEncabezado.Controls.Add(lblNumeroPresupuesto, 1, 0);
            tplEncabezado.Dock = DockStyle.Fill;
            tplEncabezado.Location = new Point(0, 0);
            tplEncabezado.Name = "tplEncabezado";
            tplEncabezado.RowCount = 2;
            tplEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tplEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tplEncabezado.Size = new Size(668, 75);
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
            lblTitulo.Size = new Size(461, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registrar decisión del presupuesto";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(3, 37);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(461, 38);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Registre la respuesta comunicada por el cliente.";
            // 
            // lblNumeroPresupuesto
            // 
            lblNumeroPresupuesto.AutoSize = true;
            lblNumeroPresupuesto.Dock = DockStyle.Fill;
            lblNumeroPresupuesto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumeroPresupuesto.ForeColor = Color.FromArgb(30, 64, 175);
            lblNumeroPresupuesto.Location = new Point(470, 0);
            lblNumeroPresupuesto.Name = "lblNumeroPresupuesto";
            lblNumeroPresupuesto.Size = new Size(195, 37);
            lblNumeroPresupuesto.TabIndex = 2;
            lblNumeroPresupuesto.Text = "Presupuesto N.º 000001";
            lblNumeroPresupuesto.TextAlign = ContentAlignment.MiddleRight;
            // 
            // grpResumen
            // 
            grpResumen.BackColor = Color.White;
            grpResumen.Controls.Add(tlpResumen);
            grpResumen.Dock = DockStyle.Fill;
            grpResumen.Location = new Point(18, 103);
            grpResumen.Margin = new Padding(0, 0, 0, 10);
            grpResumen.Name = "grpResumen";
            grpResumen.Padding = new Padding(12);
            grpResumen.Size = new Size(668, 130);
            grpResumen.TabIndex = 1;
            grpResumen.TabStop = false;
            grpResumen.Text = "Presupuesto seleccionado";
            // 
            // tlpResumen
            // 
            tlpResumen.ColumnCount = 4;
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumen.Controls.Add(lblAtencion, 0, 0);
            tlpResumen.Controls.Add(lblCliente, 1, 0);
            tlpResumen.Controls.Add(lblVehiculo, 2, 0);
            tlpResumen.Controls.Add(lblTotal, 3, 0);
            tlpResumen.Controls.Add(lblAtencionValor, 0, 1);
            tlpResumen.Controls.Add(lblClienteValor, 1, 1);
            tlpResumen.Controls.Add(lblVehiculoValor, 2, 1);
            tlpResumen.Controls.Add(lblTotalValor, 3, 1);
            tlpResumen.Dock = DockStyle.Fill;
            tlpResumen.Location = new Point(12, 28);
            tlpResumen.Name = "tlpResumen";
            tlpResumen.Padding = new Padding(8);
            tlpResumen.RowCount = 2;
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumen.Size = new Size(644, 90);
            tlpResumen.TabIndex = 0;
            // 
            // lblAtencion
            // 
            lblAtencion.AutoSize = true;
            lblAtencion.Dock = DockStyle.Fill;
            lblAtencion.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblAtencion.ForeColor = Color.FromArgb(30, 64, 175);
            lblAtencion.Location = new Point(11, 8);
            lblAtencion.Name = "lblAtencion";
            lblAtencion.Size = new Size(151, 37);
            lblAtencion.TabIndex = 0;
            lblAtencion.Text = "ATENCIÓN";
            lblAtencion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCliente
            // 
            lblCliente.AutoSize = true;
            lblCliente.Dock = DockStyle.Fill;
            lblCliente.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblCliente.ForeColor = Color.FromArgb(30, 64, 175);
            lblCliente.Location = new Point(168, 8);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(151, 37);
            lblCliente.TabIndex = 1;
            lblCliente.Text = "CLIENTE";
            lblCliente.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculo
            // 
            lblVehiculo.AutoSize = true;
            lblVehiculo.Dock = DockStyle.Fill;
            lblVehiculo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblVehiculo.ForeColor = Color.FromArgb(30, 64, 175);
            lblVehiculo.Location = new Point(325, 8);
            lblVehiculo.Name = "lblVehiculo";
            lblVehiculo.Size = new Size(151, 37);
            lblVehiculo.TabIndex = 2;
            lblVehiculo.Text = "VEHÍCULO";
            lblVehiculo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(30, 64, 175);
            lblTotal.Location = new Point(482, 8);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(151, 37);
            lblTotal.TabIndex = 3;
            lblTotal.Text = "TOTAL";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAtencionValor
            // 
            lblAtencionValor.AutoSize = true;
            lblAtencionValor.Dock = DockStyle.Fill;
            lblAtencionValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAtencionValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblAtencionValor.Location = new Point(11, 45);
            lblAtencionValor.Name = "lblAtencionValor";
            lblAtencionValor.Size = new Size(151, 37);
            lblAtencionValor.TabIndex = 4;
            lblAtencionValor.Text = "000001";
            // 
            // lblClienteValor
            // 
            lblClienteValor.AutoSize = true;
            lblClienteValor.Dock = DockStyle.Fill;
            lblClienteValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblClienteValor.Location = new Point(168, 45);
            lblClienteValor.Name = "lblClienteValor";
            lblClienteValor.Size = new Size(151, 37);
            lblClienteValor.TabIndex = 5;
            lblClienteValor.Text = "Juan Pérez";
            // 
            // lblVehiculoValor
            // 
            lblVehiculoValor.AutoSize = true;
            lblVehiculoValor.Dock = DockStyle.Fill;
            lblVehiculoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVehiculoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblVehiculoValor.Location = new Point(325, 45);
            lblVehiculoValor.Name = "lblVehiculoValor";
            lblVehiculoValor.Size = new Size(151, 37);
            lblVehiculoValor.TabIndex = 6;
            lblVehiculoValor.Text = "Honda Fit · AB345CD";
            // 
            // lblTotalValor
            // 
            lblTotalValor.AutoSize = true;
            lblTotalValor.Dock = DockStyle.Fill;
            lblTotalValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblTotalValor.Location = new Point(482, 45);
            lblTotalValor.Name = "lblTotalValor";
            lblTotalValor.Size = new Size(151, 37);
            lblTotalValor.TabIndex = 7;
            lblTotalValor.Text = "$ 43.500,00";
            // 
            // grpDecision
            // 
            grpDecision.BackColor = Color.White;
            grpDecision.Controls.Add(tlpDecision);
            grpDecision.Dock = DockStyle.Fill;
            grpDecision.Location = new Point(18, 243);
            grpDecision.Margin = new Padding(0, 0, 0, 10);
            grpDecision.Name = "grpDecision";
            grpDecision.Padding = new Padding(12);
            grpDecision.Size = new Size(668, 225);
            grpDecision.TabIndex = 2;
            grpDecision.TabStop = false;
            grpDecision.Text = "Decisión del cliente";
            // 
            // tlpDecision
            // 
            tlpDecision.ColumnCount = 2;
            tlpDecision.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpDecision.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDecision.Controls.Add(txtObservaciones, 1, 1);
            tlpDecision.Controls.Add(lblObservaciones, 0, 1);
            tlpDecision.Controls.Add(lblDecision, 0, 0);
            tlpDecision.Controls.Add(cboDecision, 1, 0);
            tlpDecision.Controls.Add(lblFecha, 0, 2);
            tlpDecision.Controls.Add(dtpFechaDecision, 1, 2);
            tlpDecision.Dock = DockStyle.Fill;
            tlpDecision.Location = new Point(12, 28);
            tlpDecision.Name = "tlpDecision";
            tlpDecision.Padding = new Padding(8);
            tlpDecision.RowCount = 3;
            tlpDecision.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpDecision.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDecision.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tlpDecision.Size = new Size(644, 185);
            tlpDecision.TabIndex = 0;
            // 
            // txtObservaciones
            // 
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Location = new Point(163, 59);
            txtObservaciones.Margin = new Padding(5);
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(468, 68);
            txtObservaciones.TabIndex = 1;
            txtObservaciones.TextChanged += CampoEditable_Cambio;
            // 
            // lblObservaciones
            // 
            lblObservaciones.AutoSize = true;
            lblObservaciones.Dock = DockStyle.Fill;
            lblObservaciones.Location = new Point(13, 62);
            lblObservaciones.Margin = new Padding(5, 8, 12, 5);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(133, 65);
            lblObservaciones.TabIndex = 4;
            lblObservaciones.Text = "Observaciones:";
            lblObservaciones.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDecision
            // 
            lblDecision.AutoSize = true;
            lblDecision.Dock = DockStyle.Fill;
            lblDecision.Location = new Point(13, 13);
            lblDecision.Margin = new Padding(5, 5, 12, 5);
            lblDecision.Name = "lblDecision";
            lblDecision.Size = new Size(133, 36);
            lblDecision.TabIndex = 0;
            lblDecision.Text = "Decisión:";
            lblDecision.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cboDecision
            // 
            cboDecision.Anchor = AnchorStyles.Left;
            cboDecision.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDecision.FormattingEnabled = true;
            cboDecision.Location = new Point(163, 19);
            cboDecision.Margin = new Padding(5);
            cboDecision.Name = "cboDecision";
            cboDecision.Size = new Size(220, 23);
            cboDecision.TabIndex = 0;
            cboDecision.SelectedIndexChanged += CampoEditable_Cambio;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Dock = DockStyle.Fill;
            lblFecha.Location = new Point(13, 137);
            lblFecha.Margin = new Padding(5, 5, 12, 5);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(133, 35);
            lblFecha.TabIndex = 6;
            lblFecha.Text = "Fecha de decisión:";
            lblFecha.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dtpFechaDecision
            // 
            dtpFechaDecision.Anchor = AnchorStyles.Left;
            dtpFechaDecision.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpFechaDecision.Format = DateTimePickerFormat.Custom;
            dtpFechaDecision.Location = new Point(161, 143);
            dtpFechaDecision.Name = "dtpFechaDecision";
            dtpFechaDecision.Size = new Size(220, 23);
            dtpFechaDecision.TabIndex = 2;
            dtpFechaDecision.ValueChanged += CampoEditable_Cambio;
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.White;
            pnlAcciones.Controls.Add(flpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(18, 478);
            pnlAcciones.Margin = new Padding(0);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Padding = new Padding(12);
            pnlAcciones.Size = new Size(668, 65);
            pnlAcciones.TabIndex = 3;
            // 
            // flpAcciones
            // 
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Controls.Add(btnRegistrar);
            flpAcciones.Dock = DockStyle.Right;
            flpAcciones.Location = new Point(359, 12);
            flpAcciones.Margin = new Padding(0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Size = new Size(297, 41);
            flpAcciones.TabIndex = 0;
            flpAcciones.WrapContents = false;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(3, 3);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 36);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // btnRegistrar
            // 
            btnRegistrar.Location = new Point(129, 3);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(165, 36);
            btnRegistrar.TabIndex = 4;
            btnRegistrar.Text = "Registrar decisión";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += BtnRegistrar_Click;
            // 
            // FrmDecisionPresupuesto
            // 
            AcceptButton = btnRegistrar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnCancelar;
            ClientSize = new Size(704, 561);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDecisionPresupuesto";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar decisión del presupuesto";
            FormClosing += FrmDecisionPresupuesto_FormClosing;
            Load += FrmDecisionPresupuesto_Load;
            Shown += FrmDecisionPresupuesto_Shown;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tplEncabezado.ResumeLayout(false);
            tplEncabezado.PerformLayout();
            grpResumen.ResumeLayout(false);
            tlpResumen.ResumeLayout(false);
            tlpResumen.PerformLayout();
            grpDecision.ResumeLayout(false);
            tlpDecision.ResumeLayout(false);
            tlpDecision.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            pnlAcciones.PerformLayout();
            flpAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private TableLayoutPanel tplEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblNumeroPresupuesto;
        private GroupBox grpResumen;
        private TableLayoutPanel tlpResumen;
        private Label lblAtencion;
        private Label lblCliente;
        private Label lblVehiculo;
        private Label lblTotal;
        private Label lblAtencionValor;
        private Label lblClienteValor;
        private Label lblVehiculoValor;
        private Label lblTotalValor;
        private GroupBox grpDecision;
        private TableLayoutPanel tlpDecision;
        private Label lblDecision;
        private ComboBox cboDecision;
        private TextBox txtObservaciones;
        private Label lblObservaciones;
        private Label lblFecha;
        private DateTimePicker dtpFechaDecision;
        private Panel pnlAcciones;
        private FlowLayoutPanel flpAcciones;
        private Button btnCancelar;
        private Button btnRegistrar;
    }
}
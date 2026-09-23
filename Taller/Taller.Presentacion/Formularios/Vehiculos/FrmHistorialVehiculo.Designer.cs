namespace Taller.Presentacion.Formularios.Vehiculos
{
    partial class FrmHistorialVehiculo
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
            tlpPrincipal = new TableLayoutPanel();
            pnlEncabezado = new Panel();
            tlpEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            lblDominioEncabezado = new Label();
            grpVehiculo = new GroupBox();
            tlpVehiculo = new TableLayoutPanel();
            lblDominioTitulo = new Label();
            lblModeloTitulo = new Label();
            lblAnioTitulo = new Label();
            lblClienteTitulo = new Label();
            lblDominioValor = new Label();
            lblModeloValor = new Label();
            lblAnioValor = new Label();
            lblClienteValor = new Label();
            grpHistorial = new GroupBox();
            dgvHistorial = new DataGridView();
            pnlAcciones = new Panel();
            tlpAcciones = new TableLayoutPanel();
            lblCantidad = new Label();
            btnCerrar = new Button();
            colNumero = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colMotivo = new DataGridViewTextBoxColumn();
            colDiagnostico = new DataGridViewTextBoxColumn();
            colTrabajo = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            grpVehiculo.SuspendLayout();
            tlpVehiculo.SuspendLayout();
            grpHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            pnlAcciones.SuspendLayout();
            tlpAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(grpVehiculo, 0, 1);
            tlpPrincipal.Controls.Add(grpHistorial, 0, 2);
            tlpPrincipal.Controls.Add(pnlAcciones, 0, 3);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(18);
            tlpPrincipal.RowCount = 4;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tlpPrincipal.Size = new Size(904, 551);
            tlpPrincipal.TabIndex = 0;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tlpEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(18, 18);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 10);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(868, 75);
            pnlEncabezado.TabIndex = 0;
            // 
            // tlpEncabezado
            // 
            tlpEncabezado.ColumnCount = 2;
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpEncabezado.Controls.Add(lblTitulo, 0, 0);
            tlpEncabezado.Controls.Add(lblDescripcion, 0, 1);
            tlpEncabezado.Controls.Add(lblDominioEncabezado, 1, 0);
            tlpEncabezado.Dock = DockStyle.Fill;
            tlpEncabezado.Location = new Point(0, 0);
            tlpEncabezado.Margin = new Padding(0, 0, 0, 10);
            tlpEncabezado.Name = "tlpEncabezado";
            tlpEncabezado.RowCount = 2;
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpEncabezado.Size = new Size(868, 75);
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
            lblTitulo.Size = new Size(601, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Historial del vehículo";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(3, 37);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(601, 38);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Consulte las atenciones anteriores y los trabajos realizados.";
            // 
            // lblDominioEncabezado
            // 
            lblDominioEncabezado.AutoSize = true;
            lblDominioEncabezado.Dock = DockStyle.Fill;
            lblDominioEncabezado.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDominioEncabezado.ForeColor = Color.FromArgb(30, 64, 175);
            lblDominioEncabezado.Location = new Point(610, 0);
            lblDominioEncabezado.Name = "lblDominioEncabezado";
            lblDominioEncabezado.Size = new Size(255, 37);
            lblDominioEncabezado.TabIndex = 2;
            lblDominioEncabezado.Text = "AB345CD";
            lblDominioEncabezado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpVehiculo
            // 
            grpVehiculo.BackColor = Color.White;
            grpVehiculo.Controls.Add(tlpVehiculo);
            grpVehiculo.Dock = DockStyle.Fill;
            grpVehiculo.Location = new Point(18, 103);
            grpVehiculo.Margin = new Padding(0, 0, 0, 10);
            grpVehiculo.Name = "grpVehiculo";
            grpVehiculo.Padding = new Padding(12);
            grpVehiculo.Size = new Size(868, 110);
            grpVehiculo.TabIndex = 1;
            grpVehiculo.TabStop = false;
            grpVehiculo.Text = "Vehículo consultado";
            // 
            // tlpVehiculo
            // 
            tlpVehiculo.ColumnCount = 4;
            tlpVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpVehiculo.Controls.Add(lblDominioTitulo, 0, 0);
            tlpVehiculo.Controls.Add(lblModeloTitulo, 1, 0);
            tlpVehiculo.Controls.Add(lblAnioTitulo, 2, 0);
            tlpVehiculo.Controls.Add(lblClienteTitulo, 3, 0);
            tlpVehiculo.Controls.Add(lblDominioValor, 0, 1);
            tlpVehiculo.Controls.Add(lblModeloValor, 1, 1);
            tlpVehiculo.Controls.Add(lblAnioValor, 2, 1);
            tlpVehiculo.Controls.Add(lblClienteValor, 3, 1);
            tlpVehiculo.Dock = DockStyle.Fill;
            tlpVehiculo.Location = new Point(12, 28);
            tlpVehiculo.Name = "tlpVehiculo";
            tlpVehiculo.Padding = new Padding(8);
            tlpVehiculo.RowCount = 2;
            tlpVehiculo.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpVehiculo.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpVehiculo.Size = new Size(844, 70);
            tlpVehiculo.TabIndex = 0;
            // 
            // lblDominioTitulo
            // 
            lblDominioTitulo.AutoSize = true;
            lblDominioTitulo.Dock = DockStyle.Fill;
            lblDominioTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblDominioTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblDominioTitulo.Location = new Point(11, 8);
            lblDominioTitulo.Name = "lblDominioTitulo";
            lblDominioTitulo.Size = new Size(201, 27);
            lblDominioTitulo.TabIndex = 0;
            lblDominioTitulo.Text = "DOMINIO";
            lblDominioTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblModeloTitulo
            // 
            lblModeloTitulo.AutoSize = true;
            lblModeloTitulo.Dock = DockStyle.Fill;
            lblModeloTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblModeloTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblModeloTitulo.Location = new Point(218, 8);
            lblModeloTitulo.Name = "lblModeloTitulo";
            lblModeloTitulo.Size = new Size(201, 27);
            lblModeloTitulo.TabIndex = 1;
            lblModeloTitulo.Text = "VEHÍCULO";
            lblModeloTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAnioTitulo
            // 
            lblAnioTitulo.AutoSize = true;
            lblAnioTitulo.Dock = DockStyle.Fill;
            lblAnioTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblAnioTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblAnioTitulo.Location = new Point(425, 8);
            lblAnioTitulo.Name = "lblAnioTitulo";
            lblAnioTitulo.Size = new Size(201, 27);
            lblAnioTitulo.TabIndex = 2;
            lblAnioTitulo.Text = "AÑO";
            lblAnioTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.AutoSize = true;
            lblClienteTitulo.Dock = DockStyle.Fill;
            lblClienteTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblClienteTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblClienteTitulo.Location = new Point(632, 8);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.Size = new Size(201, 27);
            lblClienteTitulo.TabIndex = 3;
            lblClienteTitulo.Text = "CLIENTE ACTUAL";
            lblClienteTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioValor
            // 
            lblDominioValor.AutoSize = true;
            lblDominioValor.Dock = DockStyle.Fill;
            lblDominioValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDominioValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblDominioValor.Location = new Point(11, 35);
            lblDominioValor.Name = "lblDominioValor";
            lblDominioValor.Size = new Size(201, 27);
            lblDominioValor.TabIndex = 4;
            lblDominioValor.Text = "AB345CD";
            lblDominioValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblModeloValor
            // 
            lblModeloValor.AutoSize = true;
            lblModeloValor.Dock = DockStyle.Fill;
            lblModeloValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblModeloValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblModeloValor.Location = new Point(218, 35);
            lblModeloValor.Name = "lblModeloValor";
            lblModeloValor.Size = new Size(201, 27);
            lblModeloValor.TabIndex = 5;
            lblModeloValor.Text = "Honda Fit";
            lblModeloValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAnioValor
            // 
            lblAnioValor.AutoSize = true;
            lblAnioValor.Dock = DockStyle.Fill;
            lblAnioValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAnioValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblAnioValor.Location = new Point(425, 35);
            lblAnioValor.Name = "lblAnioValor";
            lblAnioValor.Size = new Size(201, 27);
            lblAnioValor.TabIndex = 6;
            lblAnioValor.Text = "2020";
            lblAnioValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClienteValor
            // 
            lblClienteValor.AutoSize = true;
            lblClienteValor.Dock = DockStyle.Fill;
            lblClienteValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblClienteValor.Location = new Point(632, 35);
            lblClienteValor.Name = "lblClienteValor";
            lblClienteValor.Size = new Size(201, 27);
            lblClienteValor.TabIndex = 7;
            lblClienteValor.Text = "Juan Pérez";
            lblClienteValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpHistorial
            // 
            grpHistorial.BackColor = Color.White;
            grpHistorial.Controls.Add(dgvHistorial);
            grpHistorial.Dock = DockStyle.Fill;
            grpHistorial.Location = new Point(18, 223);
            grpHistorial.Margin = new Padding(0, 0, 0, 10);
            grpHistorial.Name = "grpHistorial";
            grpHistorial.Padding = new Padding(12);
            grpHistorial.Size = new Size(868, 235);
            grpHistorial.TabIndex = 2;
            grpHistorial.TabStop = false;
            grpHistorial.Text = "Atenciones anteriores";
            // 
            // dgvHistorial
            // 
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.AllowUserToResizeRows = false;
            dgvHistorial.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvHistorial.BackgroundColor = Color.White;
            dgvHistorial.BorderStyle = BorderStyle.None;
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorial.Columns.AddRange(new DataGridViewColumn[] { colNumero, colFecha, colMotivo, colDiagnostico, colTrabajo, colEstado });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvHistorial.DefaultCellStyle = dataGridViewCellStyle1;
            dgvHistorial.Dock = DockStyle.Fill;
            dgvHistorial.Location = new Point(12, 28);
            dgvHistorial.MultiSelect = false;
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.ReadOnly = true;
            dgvHistorial.RowHeadersVisible = false;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.Size = new Size(844, 195);
            dgvHistorial.TabIndex = 0;
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.White;
            pnlAcciones.Controls.Add(tlpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(21, 471);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Padding = new Padding(12);
            pnlAcciones.Size = new Size(862, 59);
            pnlAcciones.TabIndex = 3;
            // 
            // tlpAcciones
            // 
            tlpAcciones.ColumnCount = 2;
            tlpAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAcciones.Controls.Add(lblCantidad, 0, 0);
            tlpAcciones.Controls.Add(btnCerrar, 1, 0);
            tlpAcciones.Dock = DockStyle.Fill;
            tlpAcciones.Location = new Point(12, 12);
            tlpAcciones.Margin = new Padding(0);
            tlpAcciones.Name = "tlpAcciones";
            tlpAcciones.RowCount = 1;
            tlpAcciones.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpAcciones.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpAcciones.Size = new Size(838, 35);
            tlpAcciones.TabIndex = 0;
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.ForeColor = Color.FromArgb(100, 116, 139);
            lblCantidad.Location = new Point(3, 0);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(413, 35);
            lblCantidad.TabIndex = 0;
            lblCantidad.Text = "0 atenciones registradas";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnCerrar
            // 
            btnCerrar.Dock = DockStyle.Right;
            btnCerrar.Location = new Point(715, 3);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 29);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += BtnCerrar_Click;
            // 
            // colNumero
            // 
            colNumero.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colNumero.FillWeight = 0.0135567915F;
            colNumero.HeaderText = "N.º";
            colNumero.Name = "colNumero";
            colNumero.ReadOnly = true;
            colNumero.Width = 75;
            // 
            // colFecha
            // 
            colFecha.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colFecha.FillWeight = 258.88324F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            colFecha.Width = 110;
            // 
            // colMotivo
            // 
            colMotivo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colMotivo.FillWeight = 297.8554F;
            colMotivo.HeaderText = "Motivo de consulta";
            colMotivo.Name = "colMotivo";
            colMotivo.ReadOnly = true;
            colMotivo.Width = 260;
            // 
            // colDiagnostico
            // 
            colDiagnostico.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDiagnostico.FillWeight = 37.7518158F;
            colDiagnostico.HeaderText = "Diagnóstico";
            colDiagnostico.Name = "colDiagnostico";
            colDiagnostico.ReadOnly = true;
            colDiagnostico.Width = 260;
            // 
            // colTrabajo
            // 
            colTrabajo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colTrabajo.FillWeight = 5.18014765F;
            colTrabajo.HeaderText = "Trabajo realizado";
            colTrabajo.Name = "colTrabajo";
            colTrabajo.ReadOnly = true;
            colTrabajo.Width = 260;
            // 
            // colEstado
            // 
            colEstado.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colEstado.FillWeight = 0.315886974F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            colEstado.Width = 110;
            // 
            // FrmHistorialVehiculo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnCerrar;
            ClientSize = new Size(904, 551);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmHistorialVehiculo";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Historial del vehículo";
            Load += FrmHistorialVehiculo_Load;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            tlpEncabezado.PerformLayout();
            grpVehiculo.ResumeLayout(false);
            tlpVehiculo.ResumeLayout(false);
            tlpVehiculo.PerformLayout();
            grpHistorial.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
            pnlAcciones.ResumeLayout(false);
            tlpAcciones.ResumeLayout(false);
            tlpAcciones.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private TableLayoutPanel tlpEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblDominioEncabezado;
        private GroupBox grpVehiculo;
        private TableLayoutPanel tlpVehiculo;
        private Label lblDominioTitulo;
        private Label lblModeloTitulo;
        private Label lblAnioTitulo;
        private Label lblClienteTitulo;
        private Label lblDominioValor;
        private Label lblModeloValor;
        private Label lblAnioValor;
        private Label lblClienteValor;
        private GroupBox grpHistorial;
        private DataGridView dgvHistorial;
        private Panel pnlAcciones;
        private TableLayoutPanel tlpAcciones;
        private Label lblCantidad;
        private Button btnCerrar;
        private DataGridViewTextBoxColumn colNumero;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colMotivo;
        private DataGridViewTextBoxColumn colDiagnostico;
        private DataGridViewTextBoxColumn colTrabajo;
        private DataGridViewTextBoxColumn colEstado;
    }
}
namespace Taller.Presentacion.Formularios.Reportes
{
    partial class FrmReportes
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
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tlpPrincipal = new TableLayoutPanel();
            pnlGrilla = new Panel();
            dgvUsuarios = new DataGridView();
            pnlCabecera = new Panel();
            btnNuevoUsuario = new Button();
            lblDescripcion = new Label();
            lblTitulo = new Label();
            pnlFiltros = new Panel();
            btn_generar = new Button();
            button2 = new Button();
            button1 = new Button();
            tipo_reporte = new ComboBox();
            label1 = new Label();
            hasta = new DateTimePicker();
            fecha_hasta = new Label();
            desde = new DateTimePicker();
            fecha_desde = new Label();
            btn_actualizar = new Button();
            cboEstado = new ComboBox();
            lblEstado = new Label();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            lblCantidad = new Label();
            colId = new DataGridViewTextBoxColumn();
            colNombreUsuario = new DataGridViewTextBoxColumn();
            colNombreCompleto = new DataGridViewTextBoxColumn();
            colRol = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colFechaAlta = new DataGridViewTextBoxColumn();
            fechaDesde = new DataGridViewTextBoxColumn();
            fechaHasta = new DataGridViewTextBoxColumn();
            tipoReporte = new DataGridViewTextBoxColumn();
            tlpPrincipal.SuspendLayout();
            pnlGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlGrilla, 0, 2);
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
            tlpPrincipal.Controls.Add(lblCantidad, 0, 3);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(27, 27);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 3;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 242F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPrincipal.Size = new Size(746, 546);
            tlpPrincipal.TabIndex = 2;
            // 
            // pnlGrilla
            // 
            pnlGrilla.BackColor = Color.White;
            pnlGrilla.Controls.Add(dgvUsuarios);
            pnlGrilla.Dock = DockStyle.Fill;
            pnlGrilla.Location = new Point(0, 324);
            pnlGrilla.Margin = new Padding(0);
            pnlGrilla.Name = "pnlGrilla";
            pnlGrilla.Padding = new Padding(1);
            pnlGrilla.Size = new Size(746, 188);
            pnlGrilla.TabIndex = 5;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dgvUsuarios.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(241, 245, 249);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle2.Padding = new Padding(6, 0, 6, 0);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvUsuarios.ColumnHeadersHeight = 42;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colId, colNombreUsuario, colNombreCompleto, colRol, colEstado, colFechaAlta, fechaDesde, fechaHasta, tipoReporte });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle5.Padding = new Padding(6, 0, 6, 0);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvUsuarios.DefaultCellStyle = dataGridViewCellStyle5;
            dgvUsuarios.Dock = DockStyle.Fill;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.GridColor = Color.FromArgb(226, 232, 240);
            dgvUsuarios.Location = new Point(1, 1);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.RowTemplate.Height = 38;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(744, 186);
            dgvUsuarios.TabIndex = 5;
            // 
            // pnlCabecera
            // 
            pnlCabecera.BackColor = Color.White;
            pnlCabecera.Controls.Add(btnNuevoUsuario);
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Margin = new Padding(0, 0, 0, 10);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Padding = new Padding(20, 12, 20, 10);
            pnlCabecera.Size = new Size(746, 72);
            pnlCabecera.TabIndex = 0;
            // 
            // btnNuevoUsuario
            // 
            btnNuevoUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoUsuario.BackColor = Color.FromArgb(30, 64, 175);
            btnNuevoUsuario.Cursor = Cursors.Hand;
            btnNuevoUsuario.FlatAppearance.BorderSize = 0;
            btnNuevoUsuario.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 58, 138);
            btnNuevoUsuario.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnNuevoUsuario.FlatStyle = FlatStyle.Flat;
            btnNuevoUsuario.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnNuevoUsuario.ForeColor = Color.White;
            btnNuevoUsuario.Location = new Point(1105, 27);
            btnNuevoUsuario.Name = "btnNuevoUsuario";
            btnNuevoUsuario.Size = new Size(150, 40);
            btnNuevoUsuario.TabIndex = 0;
            btnNuevoUsuario.Text = "＋ Nuevo usuario";
            btnNuevoUsuario.UseVisualStyleBackColor = false;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(23, 41);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(329, 21);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Consulta y generación de reportes del sistema";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(18, 4);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(133, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reportes";
            // 
            // pnlFiltros
            // 
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.Controls.Add(btn_generar);
            pnlFiltros.Controls.Add(button2);
            pnlFiltros.Controls.Add(button1);
            pnlFiltros.Controls.Add(tipo_reporte);
            pnlFiltros.Controls.Add(label1);
            pnlFiltros.Controls.Add(hasta);
            pnlFiltros.Controls.Add(fecha_hasta);
            pnlFiltros.Controls.Add(desde);
            pnlFiltros.Controls.Add(fecha_desde);
            pnlFiltros.Controls.Add(btn_actualizar);
            pnlFiltros.Controls.Add(cboEstado);
            pnlFiltros.Controls.Add(lblEstado);
            pnlFiltros.Controls.Add(txtBuscar);
            pnlFiltros.Controls.Add(lblBuscar);
            pnlFiltros.Location = new Point(0, 82);
            pnlFiltros.Margin = new Padding(0, 0, 0, 10);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(18, 10, 18, 10);
            pnlFiltros.Size = new Size(746, 232);
            pnlFiltros.TabIndex = 1;
            // 
            // btn_generar
            // 
            btn_generar.Cursor = Cursors.Hand;
            btn_generar.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            btn_generar.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btn_generar.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btn_generar.FlatStyle = FlatStyle.Flat;
            btn_generar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_generar.ForeColor = Color.FromArgb(30, 64, 175);
            btn_generar.Location = new Point(198, 175);
            btn_generar.Name = "btn_generar";
            btn_generar.Size = new Size(154, 32);
            btn_generar.TabIndex = 12;
            btn_generar.Text = "Generar Reporte";
            btn_generar.UseVisualStyleBackColor = true;
            btn_generar.Click += btn_generar_Click;
            // 
            // button2
            // 
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.FromArgb(30, 64, 175);
            button2.Location = new Point(379, 175);
            button2.Name = "button2";
            button2.Size = new Size(110, 32);
            button2.TabIndex = 11;
            button2.Text = "Borrar";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.FromArgb(30, 64, 175);
            button1.Location = new Point(526, 175);
            button1.Name = "button1";
            button1.Size = new Size(110, 32);
            button1.TabIndex = 10;
            button1.Text = "Imprimir";
            button1.UseVisualStyleBackColor = true;
            // 
            // tipo_reporte
            // 
            tipo_reporte.DropDownStyle = ComboBoxStyle.DropDownList;
            tipo_reporte.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tipo_reporte.FormattingEnabled = true;
            tipo_reporte.Location = new Point(526, 42);
            tipo_reporte.Name = "tipo_reporte";
            tipo_reporte.Size = new Size(145, 29);
            tipo_reporte.TabIndex = 9;
            tipo_reporte.SelectedIndexChanged += tipo_reporte_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(51, 65, 85);
            label1.Location = new Point(526, 19);
            label1.Name = "label1";
            label1.Size = new Size(124, 20);
            label1.TabIndex = 8;
            label1.Text = "Tipo de reportes";
            // 
            // hasta
            // 
            hasta.Location = new Point(356, 113);
            hasta.Name = "hasta";
            hasta.Size = new Size(280, 27);
            hasta.TabIndex = 7;
            hasta.ValueChanged += hasta_ValueChanged;
            // 
            // fecha_hasta
            // 
            fecha_hasta.AutoSize = true;
            fecha_hasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fecha_hasta.ForeColor = Color.FromArgb(51, 65, 85);
            fecha_hasta.Location = new Point(366, 90);
            fecha_hasta.Name = "fecha_hasta";
            fecha_hasta.Size = new Size(49, 20);
            fecha_hasta.TabIndex = 6;
            fecha_hasta.Text = "Hasta";
            // 
            // desde
            // 
            desde.Location = new Point(44, 113);
            desde.Name = "desde";
            desde.Size = new Size(280, 27);
            desde.TabIndex = 5;
            desde.ValueChanged += desde_ValueChanged;
            // 
            // fecha_desde
            // 
            fecha_desde.AutoSize = true;
            fecha_desde.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fecha_desde.ForeColor = Color.FromArgb(51, 65, 85);
            fecha_desde.Location = new Point(49, 90);
            fecha_desde.Name = "fecha_desde";
            fecha_desde.Size = new Size(52, 20);
            fecha_desde.TabIndex = 4;
            fecha_desde.Text = "Desde";
            // 
            // btn_actualizar
            // 
            btn_actualizar.Cursor = Cursors.Hand;
            btn_actualizar.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            btn_actualizar.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btn_actualizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btn_actualizar.FlatStyle = FlatStyle.Flat;
            btn_actualizar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_actualizar.ForeColor = Color.FromArgb(30, 64, 175);
            btn_actualizar.Location = new Point(51, 175);
            btn_actualizar.Name = "btn_actualizar";
            btn_actualizar.Size = new Size(110, 32);
            btn_actualizar.TabIndex = 3;
            btn_actualizar.Text = "Actualizar";
            btn_actualizar.UseVisualStyleBackColor = true;
            btn_actualizar.Click += btn_actualizar_Click;
            // 
            // cboEstado
            // 
            cboEstado.Location = new Point(344, 43);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(121, 28);
            cboEstado.TabIndex = 13;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstado.ForeColor = Color.FromArgb(51, 65, 85);
            lblEstado.Location = new Point(344, 19);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(56, 20);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado";
            // 
            // txtBuscar
            // 
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBuscar.Location = new Point(44, 42);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Nombre, apellido o usuario";
            txtBuscar.Size = new Size(280, 29);
            txtBuscar.TabIndex = 1;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBuscar.ForeColor = Color.FromArgb(51, 65, 85);
            lblBuscar.Location = new Point(49, 19);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(56, 20);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar";
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.ForeColor = Color.FromArgb(100, 116, 139);
            lblCantidad.Location = new Point(4, 516);
            lblCantidad.Margin = new Padding(4, 4, 0, 0);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(742, 30);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "0 usuarios encontrados";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // colId
            // 
            colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.MinimumWidth = 6;
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 60;
            // 
            // colNombreUsuario
            // 
            colNombreUsuario.DataPropertyName = "NombreUsuario";
            colNombreUsuario.FillWeight = 110F;
            colNombreUsuario.HeaderText = "Usuario";
            colNombreUsuario.MinimumWidth = 6;
            colNombreUsuario.Name = "colNombreUsuario";
            colNombreUsuario.ReadOnly = true;
            // 
            // colNombreCompleto
            // 
            colNombreCompleto.DataPropertyName = "NombreCompleto";
            colNombreCompleto.FillWeight = 170F;
            colNombreCompleto.HeaderText = "Nombre completo";
            colNombreCompleto.MinimumWidth = 6;
            colNombreCompleto.Name = "colNombreCompleto";
            colNombreCompleto.ReadOnly = true;
            // 
            // colRol
            // 
            colRol.DataPropertyName = "Rol";
            colRol.FillWeight = 110F;
            colRol.HeaderText = "Rol";
            colRol.MinimumWidth = 6;
            colRol.Name = "colRol";
            colRol.ReadOnly = true;
            // 
            // colEstado
            // 
            colEstado.DataPropertyName = "Estado";
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colEstado.DefaultCellStyle = dataGridViewCellStyle3;
            colEstado.FillWeight = 75F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            // 
            // colFechaAlta
            // 
            colFechaAlta.DataPropertyName = "FechaAlta";
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            colFechaAlta.DefaultCellStyle = dataGridViewCellStyle4;
            colFechaAlta.FillWeight = 95F;
            colFechaAlta.HeaderText = "Fecha de alta";
            colFechaAlta.MinimumWidth = 6;
            colFechaAlta.Name = "colFechaAlta";
            colFechaAlta.ReadOnly = true;
            // 
            // fechaDesde
            // 
            fechaDesde.HeaderText = "Fecha Desde";
            fechaDesde.MinimumWidth = 6;
            fechaDesde.Name = "fechaDesde";
            fechaDesde.ReadOnly = true;
            // 
            // fechaHasta
            // 
            fechaHasta.HeaderText = "Fecha Hasta";
            fechaHasta.MinimumWidth = 6;
            fechaHasta.Name = "fechaHasta";
            fechaHasta.ReadOnly = true;
            // 
            // tipoReporte
            // 
            tipoReporte.HeaderText = "Tipo Reporte";
            tipoReporte.MinimumWidth = 6;
            tipoReporte.Name = "tipoReporte";
            tipoReporte.ReadOnly = true;
            // 
            // FrmReportes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(800, 600);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(800, 600);
            Name = "FrmReportes";
            Padding = new Padding(27);
            Text = "FrmReportes";
            tlpPrincipal.ResumeLayout(false);
            tlpPrincipal.PerformLayout();
            pnlGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlGrilla;
        private DataGridView dgvUsuarios;
        private Panel pnlCabecera;
        private Button btnNuevoUsuario;
        private Label lblDescripcion;
        private Label lblTitulo;
        private Panel pnlFiltros;
        private Button btn_actualizar;
        private ComboBox cboEstado;
        private Label lblEstado;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private Label lblCantidad;
        private Label fecha_desde;
        private DateTimePicker desde;
        private DateTimePicker hasta;
        private Label fecha_hasta;
        private Button btn_generar;
        private Button button2;
        private Button button1;
        private ComboBox tipo_reporte;
        private Label label1;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombreUsuario;
        private DataGridViewTextBoxColumn colNombreCompleto;
        private DataGridViewTextBoxColumn colRol;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colFechaAlta;
        private DataGridViewTextBoxColumn fechaDesde;
        private DataGridViewTextBoxColumn fechaHasta;
        private DataGridViewTextBoxColumn tipoReporte;
    }
}
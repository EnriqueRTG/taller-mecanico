namespace Taller.Presentacion.Formularios.Vehiculos
{
    partial class FrmVehiculos
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
            lblTitulo = new Label();
            lblDescripcion = new Label();
            tlpPrincipal = new TableLayoutPanel();
            pnlGrilla = new Panel();
            dgvUsuarios = new DataGridView();
            pnlCabecera = new Panel();
            btnNuevoUsuario = new Button();
            label2 = new Label();
            pnlFiltros = new Panel();
            btnActualizar = new Button();
            cboEstado = new ComboBox();
            lblEstado = new Label();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            lblCantidad = new Label();
            pnlAccionesUsuario = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnEditarUsuario = new Button();
            btnGestionarCredenciales = new Button();
            btnCambiarEstado = new Button();
            colId = new DataGridViewTextBoxColumn();
            dominio = new DataGridViewTextBoxColumn();
            año = new DataGridViewTextBoxColumn();
            color = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colFechaAlta = new DataGridViewTextBoxColumn();
            modelo = new DataGridViewTextBoxColumn();
            label1 = new Label();
            tlpPrincipal.SuspendLayout();
            pnlGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            pnlAccionesUsuario.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(27, 29);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(307, 41);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de vehículos";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(30, 83);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(379, 23);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Consulta de vehículos y antecedentes asociados.";
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlGrilla, 0, 3);
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
            tlpPrincipal.Controls.Add(lblCantidad, 0, 4);
            tlpPrincipal.Controls.Add(pnlAccionesUsuario, 0, 2);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(27, 27);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 5;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpPrincipal.Size = new Size(860, 546);
            tlpPrincipal.TabIndex = 2;
            // 
            // pnlGrilla
            // 
            pnlGrilla.BackColor = Color.White;
            pnlGrilla.Controls.Add(dgvUsuarios);
            pnlGrilla.Dock = DockStyle.Fill;
            pnlGrilla.Location = new Point(0, 235);
            pnlGrilla.Margin = new Padding(0);
            pnlGrilla.Name = "pnlGrilla";
            pnlGrilla.Padding = new Padding(1);
            pnlGrilla.Size = new Size(860, 277);
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
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colId, dominio, año, color, colEstado, colFechaAlta, modelo });
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
            dgvUsuarios.Size = new Size(858, 275);
            dgvUsuarios.TabIndex = 5;
            // 
            // pnlCabecera
            // 
            pnlCabecera.BackColor = Color.White;
            pnlCabecera.Controls.Add(label1);
            pnlCabecera.Controls.Add(btnNuevoUsuario);
            pnlCabecera.Controls.Add(label2);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Margin = new Padding(0, 0, 0, 10);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Padding = new Padding(20, 12, 20, 10);
            pnlCabecera.Size = new Size(860, 74);
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
            btnNuevoUsuario.Location = new Point(1219, 27);
            btnNuevoUsuario.Name = "btnNuevoUsuario";
            btnNuevoUsuario.Size = new Size(150, 40);
            btnNuevoUsuario.TabIndex = 0;
            btnNuevoUsuario.Text = "＋ Nuevo usuario";
            btnNuevoUsuario.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(15, 23, 42);
            label2.Location = new Point(17, 5);
            label2.Name = "label2";
            label2.Size = new Size(281, 37);
            label2.TabIndex = 0;
            label2.Text = "Gestión de vehículos";
            // 
            // pnlFiltros
            // 
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.Controls.Add(btnActualizar);
            pnlFiltros.Controls.Add(cboEstado);
            pnlFiltros.Controls.Add(lblEstado);
            pnlFiltros.Controls.Add(txtBuscar);
            pnlFiltros.Controls.Add(lblBuscar);
            pnlFiltros.Dock = DockStyle.Fill;
            pnlFiltros.Location = new Point(0, 84);
            pnlFiltros.Margin = new Padding(0, 0, 0, 10);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(18, 10, 18, 10);
            pnlFiltros.Size = new Size(860, 82);
            pnlFiltros.TabIndex = 1;
            // 
            // btnActualizar
            // 
            btnActualizar.Cursor = Cursors.Hand;
            btnActualizar.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            btnActualizar.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnActualizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnActualizar.ForeColor = Color.FromArgb(30, 64, 175);
            btnActualizar.Location = new Point(485, 38);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(110, 32);
            btnActualizar.TabIndex = 3;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // cboEstado
            // 
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(320, 41);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(145, 29);
            cboEstado.TabIndex = 2;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstado.ForeColor = Color.FromArgb(51, 65, 85);
            lblEstado.Location = new Point(338, 18);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(56, 20);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado";
            // 
            // txtBuscar
            // 
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBuscar.Location = new Point(18, 42);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Dominio";
            txtBuscar.Size = new Size(280, 29);
            txtBuscar.TabIndex = 1;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBuscar.ForeColor = Color.FromArgb(51, 65, 85);
            lblBuscar.Location = new Point(21, 18);
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
            lblCantidad.Size = new Size(856, 30);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "0 usuarios encontrados";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlAccionesUsuario
            // 
            pnlAccionesUsuario.Controls.Add(flowLayoutPanel1);
            pnlAccionesUsuario.Dock = DockStyle.Fill;
            pnlAccionesUsuario.Location = new Point(0, 176);
            pnlAccionesUsuario.Margin = new Padding(0, 0, 0, 10);
            pnlAccionesUsuario.Name = "pnlAccionesUsuario";
            pnlAccionesUsuario.Padding = new Padding(18, 7, 18, 7);
            pnlAccionesUsuario.Size = new Size(860, 49);
            pnlAccionesUsuario.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.Controls.Add(btnEditarUsuario);
            flowLayoutPanel1.Controls.Add(btnGestionarCredenciales);
            flowLayoutPanel1.Controls.Add(btnCambiarEstado);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(18, 7);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(824, 35);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // btnEditarUsuario
            // 
            btnEditarUsuario.Cursor = Cursors.Hand;
            btnEditarUsuario.Enabled = false;
            btnEditarUsuario.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            btnEditarUsuario.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnEditarUsuario.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btnEditarUsuario.FlatStyle = FlatStyle.Flat;
            btnEditarUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEditarUsuario.ForeColor = Color.FromArgb(30, 64, 175);
            btnEditarUsuario.Location = new Point(0, 0);
            btnEditarUsuario.Margin = new Padding(0, 0, 8, 0);
            btnEditarUsuario.Name = "btnEditarUsuario";
            btnEditarUsuario.Size = new Size(120, 34);
            btnEditarUsuario.TabIndex = 0;
            btnEditarUsuario.Text = "Editar datos";
            btnEditarUsuario.UseVisualStyleBackColor = false;
            // 
            // btnGestionarCredenciales
            // 
            btnGestionarCredenciales.Cursor = Cursors.Hand;
            btnGestionarCredenciales.Enabled = false;
            btnGestionarCredenciales.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            btnGestionarCredenciales.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnGestionarCredenciales.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btnGestionarCredenciales.FlatStyle = FlatStyle.Flat;
            btnGestionarCredenciales.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGestionarCredenciales.ForeColor = Color.FromArgb(30, 64, 175);
            btnGestionarCredenciales.Location = new Point(128, 0);
            btnGestionarCredenciales.Margin = new Padding(0, 0, 8, 0);
            btnGestionarCredenciales.Name = "btnGestionarCredenciales";
            btnGestionarCredenciales.Size = new Size(180, 34);
            btnGestionarCredenciales.TabIndex = 1;
            btnGestionarCredenciales.Text = "Gestionar credenciales";
            btnGestionarCredenciales.UseVisualStyleBackColor = false;
            // 
            // btnCambiarEstado
            // 
            btnCambiarEstado.Cursor = Cursors.Hand;
            btnCambiarEstado.Enabled = false;
            btnCambiarEstado.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            btnCambiarEstado.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnCambiarEstado.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btnCambiarEstado.FlatStyle = FlatStyle.Flat;
            btnCambiarEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCambiarEstado.ForeColor = Color.FromArgb(185, 28, 28);
            btnCambiarEstado.Location = new Point(316, 0);
            btnCambiarEstado.Margin = new Padding(0, 0, 8, 0);
            btnCambiarEstado.Name = "btnCambiarEstado";
            btnCambiarEstado.Size = new Size(181, 34);
            btnCambiarEstado.TabIndex = 2;
            btnCambiarEstado.Text = "Deshabilitar vehiculo";
            btnCambiarEstado.UseVisualStyleBackColor = false;
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
            // dominio
            // 
            dominio.DataPropertyName = "dominio";
            dominio.FillWeight = 110F;
            dominio.HeaderText = "Domino";
            dominio.MinimumWidth = 6;
            dominio.Name = "dominio";
            dominio.ReadOnly = true;
            // 
            // año
            // 
            año.DataPropertyName = "año";
            año.FillWeight = 170F;
            año.HeaderText = "Año";
            año.MinimumWidth = 6;
            año.Name = "año";
            año.ReadOnly = true;
            // 
            // color
            // 
            color.DataPropertyName = "color";
            color.FillWeight = 110F;
            color.HeaderText = "Color";
            color.MinimumWidth = 6;
            color.Name = "color";
            color.ReadOnly = true;
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
            // modelo
            // 
            modelo.HeaderText = "Modelo";
            modelo.MinimumWidth = 6;
            modelo.Name = "modelo";
            modelo.ReadOnly = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.5F);
            label1.ForeColor = Color.FromArgb(100, 116, 139);
            label1.Location = new Point(23, 42);
            label1.Name = "label1";
            label1.Size = new Size(348, 21);
            label1.TabIndex = 2;
            label1.Text = "Administre los vehículos y los accesos al sistema.";
            // 
            // FrmVehiculos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(914, 600);
            Controls.Add(tlpPrincipal);
            Controls.Add(lblDescripcion);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(800, 600);
            Name = "FrmVehiculos";
            Padding = new Padding(27, 27, 27, 27);
            Text = "FrmVehiculos";
            tlpPrincipal.ResumeLayout(false);
            tlpPrincipal.PerformLayout();
            pnlGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            pnlAccionesUsuario.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDescripcion;
        private TableLayoutPanel tlpPrincipal;
        private Panel pnlGrilla;
        private DataGridView dgvUsuarios;
        private Panel pnlCabecera;
        private Button btnNuevoUsuario;
        private Label label2;
        private Panel pnlFiltros;
        private Button btnActualizar;
        private ComboBox cboEstado;
        private Label lblEstado;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private Label lblCantidad;
        private Panel pnlAccionesUsuario;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnEditarUsuario;
        private Button btnGestionarCredenciales;
        private Button btnCambiarEstado;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn dominio;
        private DataGridViewTextBoxColumn año;
        private DataGridViewTextBoxColumn color;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colFechaAlta;
        private DataGridViewTextBoxColumn modelo;
        private Label label1;
    }
}
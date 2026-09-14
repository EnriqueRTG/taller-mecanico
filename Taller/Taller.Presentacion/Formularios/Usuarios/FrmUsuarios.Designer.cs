namespace Taller.Presentacion.Formularios.Usuarios
{
    partial class FrmUsuarios
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dgvUsuarios = new DataGridView();
            tlpPrincipal = new TableLayoutPanel();
            pnlCabecera = new Panel();
            btnNuevoUsuario = new Button();
            lblDescripcion = new Label();
            lblTitulo = new Label();
            pnlFiltros = new Panel();
            btnActualizar = new Button();
            cboEstado = new ComboBox();
            lblEstado = new Label();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            pnlGrilla = new Panel();
            lblCantidad = new Label();
            colId = new DataGridViewTextBoxColumn();
            colNombreUsuario = new DataGridViewTextBoxColumn();
            colNombreCompleto = new DataGridViewTextBoxColumn();
            colRol = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colFechaAlta = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            tlpPrincipal.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            pnlGrilla.SuspendLayout();
            SuspendLayout();
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
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle2.Padding = new Padding(6, 0, 6, 0);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvUsuarios.ColumnHeadersHeight = 42;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colId, colNombreUsuario, colNombreCompleto, colRol, colEstado, colFechaAlta });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle4.Padding = new Padding(6, 0, 6, 0);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvUsuarios.DefaultCellStyle = dataGridViewCellStyle4;
            dgvUsuarios.Dock = DockStyle.Fill;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.GridColor = Color.FromArgb(226, 232, 240);
            dgvUsuarios.Location = new Point(1, 1);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowTemplate.Height = 38;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(750, 216);
            dgvUsuarios.TabIndex = 4;
            dgvUsuarios.CellFormatting += dgvUsuarios_CellFormatting;
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
            tlpPrincipal.Controls.Add(pnlGrilla, 0, 2);
            tlpPrincipal.Controls.Add(lblCantidad, 0, 3);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(24, 20);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 4;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpPrincipal.Size = new Size(752, 410);
            tlpPrincipal.TabIndex = 1;
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
            pnlCabecera.Size = new Size(752, 72);
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
            btnNuevoUsuario.Location = new Point(579, 15);
            btnNuevoUsuario.Name = "btnNuevoUsuario";
            btnNuevoUsuario.Size = new Size(150, 40);
            btnNuevoUsuario.TabIndex = 0;
            btnNuevoUsuario.Text = "＋ Nuevo usuario";
            btnNuevoUsuario.UseVisualStyleBackColor = false;
            btnNuevoUsuario.Click += btnNuevoUsuario_Click;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(22, 44);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(284, 17);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Administre las cuentas y los accesos al sistema.";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(20, 10);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(206, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de usuarios";
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
            pnlFiltros.Location = new Point(0, 82);
            pnlFiltros.Margin = new Padding(0, 0, 0, 10);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(18, 10, 18, 10);
            pnlFiltros.Size = new Size(752, 66);
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
            btnActualizar.Location = new Point(480, 27);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(110, 32);
            btnActualizar.TabIndex = 3;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // cboEstado
            // 
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(320, 28);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(145, 25);
            cboEstado.TabIndex = 2;
            cboEstado.SelectedIndexChanged += cboEstado_SelectedIndexChanged;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstado.ForeColor = Color.FromArgb(51, 65, 85);
            lblEstado.Location = new Point(320, 8);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(43, 15);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado";
            // 
            // txtBuscar
            // 
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBuscar.Location = new Point(18, 29);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Nombre, apellido o usuario";
            txtBuscar.Size = new Size(280, 25);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBuscar.ForeColor = Color.FromArgb(51, 65, 85);
            lblBuscar.Location = new Point(18, 8);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(44, 15);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar";
            // 
            // pnlGrilla
            // 
            pnlGrilla.BackColor = Color.White;
            pnlGrilla.Controls.Add(dgvUsuarios);
            pnlGrilla.Dock = DockStyle.Fill;
            pnlGrilla.Location = new Point(0, 158);
            pnlGrilla.Margin = new Padding(0);
            pnlGrilla.Name = "pnlGrilla";
            pnlGrilla.Padding = new Padding(1);
            pnlGrilla.Size = new Size(752, 218);
            pnlGrilla.TabIndex = 2;
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.ForeColor = Color.FromArgb(100, 116, 139);
            lblCantidad.Location = new Point(4, 380);
            lblCantidad.Margin = new Padding(4, 4, 0, 0);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(748, 30);
            lblCantidad.TabIndex = 3;
            lblCantidad.Text = "0 usuarios encontrados";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // colId
            // 
            colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 60;
            // 
            // colNombreUsuario
            // 
            colNombreUsuario.DataPropertyName = "NombreUsuario";
            colNombreUsuario.FillWeight = 110F;
            colNombreUsuario.HeaderText = "Usuario";
            colNombreUsuario.Name = "colNombreUsuario";
            colNombreUsuario.ReadOnly = true;
            // 
            // colNombreCompleto
            // 
            colNombreCompleto.DataPropertyName = "NombreCompleto";
            colNombreCompleto.FillWeight = 170F;
            colNombreCompleto.HeaderText = "Nombre completo";
            colNombreCompleto.Name = "colNombreCompleto";
            colNombreCompleto.ReadOnly = true;
            // 
            // colRol
            // 
            colRol.DataPropertyName = "Rol";
            colRol.FillWeight = 110F;
            colRol.HeaderText = "Rol";
            colRol.Name = "colRol";
            colRol.ReadOnly = true;
            // 
            // colEstado
            // 
            colEstado.DataPropertyName = "Estado";
            colEstado.FillWeight = 75F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            // 
            // colFechaAlta
            // 
            colFechaAlta.DataPropertyName = "FechaAlta";
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            colFechaAlta.DefaultCellStyle = dataGridViewCellStyle3;
            colFechaAlta.FillWeight = 95F;
            colFechaAlta.HeaderText = "Fecha de alta";
            colFechaAlta.Name = "colFechaAlta";
            colFechaAlta.ReadOnly = true;
            // 
            // FrmUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(800, 450);
            Controls.Add(tlpPrincipal);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(700, 450);
            Name = "FrmUsuarios";
            Padding = new Padding(24, 20, 24, 20);
            Text = "Usuarios";
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            tlpPrincipal.ResumeLayout(false);
            tlpPrincipal.PerformLayout();
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            pnlGrilla.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tlpPrincipal;
        private Panel pnlCabecera;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Button btnNuevoUsuario;
        private Panel pnlFiltros;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Label lblEstado;
        private ComboBox cboEstado;
        private Button btnActualizar;
        private Panel pnlGrilla;
        private DataGridView dgvUsuarios;
        private Label lblCantidad;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombreUsuario;
        private DataGridViewTextBoxColumn colNombreCompleto;
        private DataGridViewTextBoxColumn colRol;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colFechaAlta;
    }
}
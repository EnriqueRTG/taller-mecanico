namespace Taller.Presentacion.Formularios.Clientes
{
    partial class FrmClientes
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
            colId = new DataGridViewTextBoxColumn();
            dni = new DataGridViewTextBoxColumn();
            tipo_documento = new DataGridViewTextBoxColumn();
            colNombreCompleto = new DataGridViewTextBoxColumn();
            telefono = new DataGridViewTextBoxColumn();
            mail = new DataGridViewTextBoxColumn();
            direccion = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colFechaAlta = new DataGridViewTextBoxColumn();
            pnlCabecera = new Panel();
            nuevo = new Button();
            lblDescripcion = new Label();
            lblTitulo = new Label();
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
            btnCambiarEstado = new Button();
            tlpPrincipal.SuspendLayout();
            pnlGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            pnlAccionesUsuario.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
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
            tlpPrincipal.Location = new Point(24, 20);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 5;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 57F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tlpPrincipal.Size = new Size(652, 298);
            tlpPrincipal.TabIndex = 1;
            // 
            // pnlGrilla
            // 
            pnlGrilla.BackColor = Color.White;
            pnlGrilla.Controls.Add(dgvUsuarios);
            pnlGrilla.Dock = DockStyle.Fill;
            pnlGrilla.Location = new Point(0, 163);
            pnlGrilla.Margin = new Padding(0);
            pnlGrilla.Name = "pnlGrilla";
            pnlGrilla.Padding = new Padding(1);
            pnlGrilla.Size = new Size(652, 109);
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
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colId, dni, tipo_documento, colNombreCompleto, telefono, mail, direccion, colEstado, colFechaAlta });
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
            dgvUsuarios.Margin = new Padding(3, 2, 3, 2);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.RowTemplate.Height = 38;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(650, 107);
            dgvUsuarios.TabIndex = 5;
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
            // dni
            // 
            dni.DataPropertyName = "dni";
            dni.FillWeight = 110F;
            dni.HeaderText = "Documento";
            dni.MinimumWidth = 6;
            dni.Name = "dni";
            dni.ReadOnly = true;
            // 
            // tipo_documento
            // 
            tipo_documento.DataPropertyName = "tipo_documento";
            tipo_documento.FillWeight = 110F;
            tipo_documento.HeaderText = "Tipo documento";
            tipo_documento.MinimumWidth = 6;
            tipo_documento.Name = "tipo_documento";
            tipo_documento.ReadOnly = true;
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
            // telefono
            // 
            telefono.HeaderText = "Teléfono";
            telefono.MinimumWidth = 6;
            telefono.Name = "telefono";
            telefono.ReadOnly = true;
            // 
            // mail
            // 
            mail.HeaderText = "Email";
            mail.MinimumWidth = 6;
            mail.Name = "mail";
            mail.ReadOnly = true;
            // 
            // direccion
            // 
            direccion.HeaderText = "Dirección";
            direccion.MinimumWidth = 6;
            direccion.Name = "direccion";
            direccion.ReadOnly = true;
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
            // pnlCabecera
            // 
            pnlCabecera.BackColor = Color.White;
            pnlCabecera.Controls.Add(nuevo);
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Margin = new Padding(0, 0, 0, 8);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Padding = new Padding(18, 9, 18, 8);
            pnlCabecera.Size = new Size(652, 54);
            pnlCabecera.TabIndex = 0;
            // 
            // nuevo
            // 
            nuevo.Anchor = AnchorStyles.Right;
            nuevo.BackColor = Color.FromArgb(30, 64, 175);
            nuevo.Cursor = Cursors.Hand;
            nuevo.FlatAppearance.BorderSize = 0;
            nuevo.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 58, 138);
            nuevo.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            nuevo.FlatStyle = FlatStyle.Flat;
            nuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            nuevo.ForeColor = Color.White;
            nuevo.Location = new Point(479, 11);
            nuevo.Margin = new Padding(3, 2, 3, 2);
            nuevo.Name = "nuevo";
            nuevo.Size = new Size(131, 30);
            nuevo.TabIndex = 2;
            nuevo.Text = "＋ Nuevo cliente";
            nuevo.UseVisualStyleBackColor = false;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(5, 33);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(302, 17);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Consulta y administración de los clientes del taller.";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(5, 5);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(198, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de clientes";
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
            pnlFiltros.Location = new Point(0, 62);
            pnlFiltros.Margin = new Padding(0, 0, 0, 8);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(16, 8, 16, 8);
            pnlFiltros.Size = new Size(652, 49);
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
            btnActualizar.Location = new Point(420, 20);
            btnActualizar.Margin = new Padding(3, 2, 3, 2);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(96, 24);
            btnActualizar.TabIndex = 3;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // cboEstado
            // 
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(279, 22);
            cboEstado.Margin = new Padding(3, 2, 3, 2);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(127, 25);
            cboEstado.TabIndex = 2;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstado.ForeColor = Color.FromArgb(51, 65, 85);
            lblEstado.Location = new Point(279, 8);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(43, 15);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado";
            // 
            // txtBuscar
            // 
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBuscar.Location = new Point(16, 22);
            txtBuscar.Margin = new Padding(3, 2, 3, 2);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Nombre, apellido o cliente";
            txtBuscar.Size = new Size(245, 25);
            txtBuscar.TabIndex = 1;
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
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.ForeColor = Color.FromArgb(100, 116, 139);
            lblCantidad.Location = new Point(4, 275);
            lblCantidad.Margin = new Padding(4, 3, 0, 0);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(648, 23);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "0 usuarios encontrados";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlAccionesUsuario
            // 
            pnlAccionesUsuario.Controls.Add(flowLayoutPanel1);
            pnlAccionesUsuario.Dock = DockStyle.Fill;
            pnlAccionesUsuario.Location = new Point(0, 119);
            pnlAccionesUsuario.Margin = new Padding(0, 0, 0, 8);
            pnlAccionesUsuario.Name = "pnlAccionesUsuario";
            pnlAccionesUsuario.Padding = new Padding(16, 5, 16, 5);
            pnlAccionesUsuario.Size = new Size(652, 36);
            pnlAccionesUsuario.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.Controls.Add(btnEditarUsuario);
            flowLayoutPanel1.Controls.Add(btnCambiarEstado);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(16, 5);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(620, 26);
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
            btnEditarUsuario.Margin = new Padding(0, 0, 7, 0);
            btnEditarUsuario.Name = "btnEditarUsuario";
            btnEditarUsuario.Size = new Size(105, 26);
            btnEditarUsuario.TabIndex = 0;
            btnEditarUsuario.Text = "Editar datos";
            btnEditarUsuario.UseVisualStyleBackColor = false;
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
            btnCambiarEstado.Location = new Point(112, 0);
            btnCambiarEstado.Margin = new Padding(0, 0, 7, 0);
            btnCambiarEstado.Name = "btnCambiarEstado";
            btnCambiarEstado.Size = new Size(140, 26);
            btnCambiarEstado.TabIndex = 2;
            btnCambiarEstado.Text = "Deshabilitar usuario";
            btnCambiarEstado.UseVisualStyleBackColor = false;
            // 
            // FrmClientes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(700, 338);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(700, 338);
            Name = "FrmClientes";
            Padding = new Padding(24, 20, 24, 20);
            Text = "FrmClientes";
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
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlGrilla;
        private DataGridView dgvUsuarios;
        private Panel pnlCabecera;
        private Label lblDescripcion;
        private Label lblTitulo;
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
        private Button btnCambiarEstado;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn dni;
        private DataGridViewTextBoxColumn tipo_documento;
        private DataGridViewTextBoxColumn colNombreCompleto;
        private DataGridViewTextBoxColumn telefono;
        private DataGridViewTextBoxColumn mail;
        private DataGridViewTextBoxColumn direccion;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colFechaAlta;
        private Button nuevo;
    }
}
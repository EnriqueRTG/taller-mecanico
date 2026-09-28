namespace Taller.Presentacion.Formularios.Usuarios
{
    partial class FrmAltaUsuario
    {
        /// <summary>
        /// Variable necesaria para el diseñador.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Libera los recursos utilizados.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            lblDescripcion = new Label();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblNombreUsuario = new Label();
            txtNombreUsuario = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblConfirmarPassword = new Label();
            txtConfirmarPassword = new TextBox();
            chkMostrarPassword = new CheckBox();
            lblRol = new Label();
            cmbRol = new ComboBox();
            tlpPrincipal = new TableLayoutPanel();
            pnlCabecera = new Panel();
            pnlDatos = new Panel();
            tlpDatos = new TableLayoutPanel();
            flpAcciones = new FlowLayoutPanel();
            tlpPrincipal.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlDatos.SuspendLayout();
            tlpDatos.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 17.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(219, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nuevo usuario";
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(30, 64, 175);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 58, 138);
            btnGuardar.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(409, 19);
            btnGuardar.Margin = new Padding(9, 0, 0, 0);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(149, 51);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar usuario";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.White;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnCancelar.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 232, 240);
            btnCancelar.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9.5F);
            btnCancelar.ForeColor = Color.FromArgb(51, 65, 85);
            btnCancelar.Location = new Point(263, 19);
            btnCancelar.Margin = new Padding(9, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(137, 51);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(2, 52);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(371, 21);
            lblDescripcion.TabIndex = 4;
            lblDescripcion.Text = "Complete los datos para crear una cuenta de acceso.";
            // 
            // lblNombre
            // 
            lblNombre.Anchor = AnchorStyles.Left;
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(51, 65, 85);
            lblNombre.Location = new Point(5, 22);
            lblNombre.Margin = new Padding(5, 4, 9, 4);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(71, 20);
            lblNombre.TabIndex = 13;
            lblNombre.Text = "Nombre:";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.Left;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 9.75F);
            txtNombre.Location = new Point(171, 17);
            txtNombre.Margin = new Padding(0, 11, 0, 11);
            txtNombre.MaxLength = 100;
            txtNombre.Name = "txtNombre";
            txtNombre.PlaceholderText = "Nombre";
            txtNombre.Size = new Size(381, 29);
            txtNombre.TabIndex = 0;
            txtNombre.KeyPress += SoloLetras_KeyPress;
            // 
            // lblApellido
            // 
            lblApellido.Anchor = AnchorStyles.Left;
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblApellido.ForeColor = Color.FromArgb(51, 65, 85);
            lblApellido.Location = new Point(5, 86);
            lblApellido.Margin = new Padding(5, 4, 9, 4);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(71, 20);
            lblApellido.TabIndex = 15;
            lblApellido.Text = "Apellido:";
            lblApellido.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtApellido
            // 
            txtApellido.Anchor = AnchorStyles.Left;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Font = new Font("Segoe UI", 9.75F);
            txtApellido.Location = new Point(171, 81);
            txtApellido.Margin = new Padding(0, 11, 0, 11);
            txtApellido.MaxLength = 100;
            txtApellido.Name = "txtApellido";
            txtApellido.PlaceholderText = "Apellido";
            txtApellido.Size = new Size(381, 29);
            txtApellido.TabIndex = 1;
            txtApellido.KeyPress += SoloLetras_KeyPress;
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.Anchor = AnchorStyles.Left;
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombreUsuario.ForeColor = Color.FromArgb(51, 65, 85);
            lblNombreUsuario.Location = new Point(5, 150);
            lblNombreUsuario.Margin = new Padding(5, 4, 9, 4);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(127, 20);
            lblNombreUsuario.TabIndex = 17;
            lblNombreUsuario.Text = "Nombre usuario:";
            lblNombreUsuario.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Anchor = AnchorStyles.Left;
            txtNombreUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtNombreUsuario.Font = new Font("Segoe UI", 9.75F);
            txtNombreUsuario.Location = new Point(171, 145);
            txtNombreUsuario.Margin = new Padding(0, 11, 0, 11);
            txtNombreUsuario.MaxLength = 50;
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.PlaceholderText = "Usuario utilizado para iniciar sesión";
            txtNombreUsuario.Size = new Size(381, 29);
            txtNombreUsuario.TabIndex = 2;
            // 
            // lblPassword
            // 
            lblPassword.Anchor = AnchorStyles.Left;
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblPassword.Location = new Point(5, 214);
            lblPassword.Margin = new Padding(5, 4, 9, 4);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(92, 20);
            lblPassword.TabIndex = 19;
            lblPassword.Text = "Contraseña:";
            lblPassword.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Left;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 9.75F);
            txtPassword.Location = new Point(171, 209);
            txtPassword.Margin = new Padding(0, 11, 0, 11);
            txtPassword.MaxLength = 100;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Ingrese una contraseña";
            txtPassword.Size = new Size(381, 29);
            txtPassword.TabIndex = 3;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblConfirmarPassword
            // 
            lblConfirmarPassword.Anchor = AnchorStyles.Left;
            lblConfirmarPassword.AutoSize = true;
            lblConfirmarPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblConfirmarPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblConfirmarPassword.Location = new Point(5, 268);
            lblConfirmarPassword.Margin = new Padding(5, 4, 9, 4);
            lblConfirmarPassword.Name = "lblConfirmarPassword";
            lblConfirmarPassword.Size = new Size(90, 40);
            lblConfirmarPassword.TabIndex = 21;
            lblConfirmarPassword.Text = "Confirmar contraseña:";
            lblConfirmarPassword.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtConfirmarPassword
            // 
            txtConfirmarPassword.Anchor = AnchorStyles.Left;
            txtConfirmarPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmarPassword.Font = new Font("Segoe UI", 9.75F);
            txtConfirmarPassword.Location = new Point(171, 273);
            txtConfirmarPassword.Margin = new Padding(0, 11, 0, 11);
            txtConfirmarPassword.MaxLength = 100;
            txtConfirmarPassword.Name = "txtConfirmarPassword";
            txtConfirmarPassword.PlaceholderText = "Repita la contraseña";
            txtConfirmarPassword.Size = new Size(381, 29);
            txtConfirmarPassword.TabIndex = 4;
            txtConfirmarPassword.UseSystemPasswordChar = true;
            // 
            // chkMostrarPassword
            // 
            chkMostrarPassword.Anchor = AnchorStyles.Left;
            chkMostrarPassword.AutoSize = true;
            chkMostrarPassword.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkMostrarPassword.ForeColor = Color.FromArgb(71, 85, 105);
            chkMostrarPassword.Location = new Point(171, 333);
            chkMostrarPassword.Margin = new Padding(0, 5, 0, 5);
            chkMostrarPassword.Name = "chkMostrarPassword";
            chkMostrarPassword.Size = new Size(158, 24);
            chkMostrarPassword.TabIndex = 5;
            chkMostrarPassword.Text = "Mostrar contraseña";
            chkMostrarPassword.UseVisualStyleBackColor = true;
            chkMostrarPassword.CheckedChanged += chkMostrarPassword_CheckedChanged;
            // 
            // lblRol
            // 
            lblRol.Anchor = AnchorStyles.Left;
            lblRol.AutoSize = true;
            lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRol.ForeColor = Color.FromArgb(51, 65, 85);
            lblRol.Location = new Point(5, 394);
            lblRol.Margin = new Padding(5, 4, 9, 4);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(36, 20);
            lblRol.TabIndex = 24;
            lblRol.Text = "Rol:";
            lblRol.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbRol
            // 
            cmbRol.Anchor = AnchorStyles.Left;
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(171, 388);
            cmbRol.Margin = new Padding(0, 11, 0, 11);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(381, 29);
            cmbRol.TabIndex = 6;
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlDatos, 0, 1);
            tlpPrincipal.Controls.Add(flpAcciones, 0, 2);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(32, 29, 32, 29);
            tlpPrincipal.RowCount = 3;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 87F));
            tlpPrincipal.Size = new Size(622, 688);
            tlpPrincipal.TabIndex = 26;
            // 
            // pnlCabecera
            // 
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(32, 29);
            pnlCabecera.Margin = new Padding(0);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(558, 100);
            pnlCabecera.TabIndex = 0;
            // 
            // pnlDatos
            // 
            pnlDatos.Controls.Add(tlpDatos);
            pnlDatos.Dock = DockStyle.Fill;
            pnlDatos.Location = new Point(32, 129);
            pnlDatos.Margin = new Padding(0);
            pnlDatos.Name = "pnlDatos";
            pnlDatos.Padding = new Padding(2, 3, 2, 3);
            pnlDatos.Size = new Size(558, 443);
            pnlDatos.TabIndex = 1;
            // 
            // tlpDatos
            // 
            tlpDatos.BackColor = Color.White;
            tlpDatos.ColumnCount = 2;
            tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 171F));
            tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDatos.Controls.Add(cmbRol, 1, 6);
            tlpDatos.Controls.Add(txtConfirmarPassword, 1, 4);
            tlpDatos.Controls.Add(txtPassword, 1, 3);
            tlpDatos.Controls.Add(txtNombreUsuario, 1, 2);
            tlpDatos.Controls.Add(txtApellido, 1, 1);
            tlpDatos.Controls.Add(txtNombre, 1, 0);
            tlpDatos.Controls.Add(lblNombre, 0, 0);
            tlpDatos.Controls.Add(lblNombreUsuario, 0, 2);
            tlpDatos.Controls.Add(lblApellido, 0, 1);
            tlpDatos.Controls.Add(lblPassword, 0, 3);
            tlpDatos.Controls.Add(lblConfirmarPassword, 0, 4);
            tlpDatos.Controls.Add(lblRol, 0, 6);
            tlpDatos.Controls.Add(chkMostrarPassword, 1, 5);
            tlpDatos.Dock = DockStyle.Fill;
            tlpDatos.Location = new Point(2, 3);
            tlpDatos.Margin = new Padding(3, 4, 3, 4);
            tlpDatos.Name = "tlpDatos";
            tlpDatos.RowCount = 7;
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpDatos.Size = new Size(554, 437);
            tlpDatos.TabIndex = 0;
            // 
            // flpAcciones
            // 
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.FlowDirection = FlowDirection.RightToLeft;
            flpAcciones.Location = new Point(32, 572);
            flpAcciones.Margin = new Padding(0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Padding = new Padding(0, 19, 0, 0);
            flpAcciones.Size = new Size(558, 87);
            flpAcciones.TabIndex = 2;
            flpAcciones.WrapContents = false;
            // 
            // FrmAltaUsuario
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnCancelar;
            ClientSize = new Size(622, 688);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAltaUsuario";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nuevo usuario";
            Load += FrmAltaUsuario_Load;
            tlpPrincipal.ResumeLayout(false);
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlDatos.ResumeLayout(false);
            tlpDatos.ResumeLayout(false);
            tlpDatos.PerformLayout();
            flpAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;

        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblDescripcion;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblNombreUsuario;
        private TextBox txtNombreUsuario;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblConfirmarPassword;
        private TextBox txtConfirmarPassword;
        private CheckBox chkMostrarPassword;
        private Label lblRol;
        private ComboBox cmbRol;
        private TableLayoutPanel tlpPrincipal;
        private Panel pnlCabecera;
        private Panel pnlDatos;
        private TableLayoutPanel tlpDatos;
        private FlowLayoutPanel flpAcciones;
    }
}
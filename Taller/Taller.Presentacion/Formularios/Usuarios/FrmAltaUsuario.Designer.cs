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
            grpDatosUsuario = new GroupBox();

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

            lblRol = new Label();
            cmbRol = new ComboBox();

            chkMostrarPassword = new CheckBox();

            btnGuardar = new Button();
            btnCancelar = new Button();

            grpDatosUsuario.SuspendLayout();
            SuspendLayout();

            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(179, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Alta de usuario";

            // 
            // grpDatosUsuario
            // 
            grpDatosUsuario.Controls.Add(lblNombre);
            grpDatosUsuario.Controls.Add(txtNombre);
            grpDatosUsuario.Controls.Add(lblApellido);
            grpDatosUsuario.Controls.Add(txtApellido);
            grpDatosUsuario.Controls.Add(lblNombreUsuario);
            grpDatosUsuario.Controls.Add(txtNombreUsuario);
            grpDatosUsuario.Controls.Add(lblPassword);
            grpDatosUsuario.Controls.Add(txtPassword);
            grpDatosUsuario.Controls.Add(lblConfirmarPassword);
            grpDatosUsuario.Controls.Add(txtConfirmarPassword);
            grpDatosUsuario.Controls.Add(chkMostrarPassword);
            grpDatosUsuario.Controls.Add(lblRol);
            grpDatosUsuario.Controls.Add(cmbRol);
            grpDatosUsuario.Location = new Point(24, 68);
            grpDatosUsuario.Name = "grpDatosUsuario";
            grpDatosUsuario.Size = new Size(500, 345);
            grpDatosUsuario.TabIndex = 1;
            grpDatosUsuario.TabStop = false;
            grpDatosUsuario.Text = "Datos del usuario";

            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(24, 35);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(54, 15);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre:";

            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(170, 31);
            txtNombre.MaxLength = 100;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(280, 23);
            txtNombre.TabIndex = 1;

            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(24, 77);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(54, 15);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido:";

            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(170, 73);
            txtApellido.MaxLength = 100;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(280, 23);
            txtApellido.TabIndex = 3;

            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Location = new Point(24, 119);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(104, 15);
            lblNombreUsuario.TabIndex = 4;
            lblNombreUsuario.Text = "Nombre usuario:";

            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Location = new Point(170, 115);
            txtNombreUsuario.MaxLength = 50;
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(280, 23);
            txtNombreUsuario.TabIndex = 5;

            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(24, 161);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(70, 15);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Contraseña:";

            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(170, 157);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(280, 23);
            txtPassword.TabIndex = 7;
            txtPassword.UseSystemPasswordChar = true;

            // 
            // lblConfirmarPassword
            // 
            lblConfirmarPassword.AutoSize = true;
            lblConfirmarPassword.Location = new Point(24, 203);
            lblConfirmarPassword.Name = "lblConfirmarPassword";
            lblConfirmarPassword.Size = new Size(126, 15);
            lblConfirmarPassword.TabIndex = 8;
            lblConfirmarPassword.Text = "Confirmar contraseña:";

            // 
            // txtConfirmarPassword
            // 
            txtConfirmarPassword.Location = new Point(170, 199);
            txtConfirmarPassword.Name = "txtConfirmarPassword";
            txtConfirmarPassword.Size = new Size(280, 23);
            txtConfirmarPassword.TabIndex = 9;
            txtConfirmarPassword.UseSystemPasswordChar = true;

            // 
            // chkMostrarPassword
            // 
            chkMostrarPassword.AutoSize = true;
            chkMostrarPassword.Location = new Point(170, 231);
            chkMostrarPassword.Name = "chkMostrarPassword";
            chkMostrarPassword.Size = new Size(130, 19);
            chkMostrarPassword.TabIndex = 10;
            chkMostrarPassword.Text = "Mostrar contraseña";
            chkMostrarPassword.UseVisualStyleBackColor = true;
            chkMostrarPassword.CheckedChanged += chkMostrarPassword_CheckedChanged;

            // 
            // lblRol
            // 
            lblRol.AutoSize = true;
            lblRol.Location = new Point(24, 278);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(27, 15);
            lblRol.TabIndex = 11;
            lblRol.Text = "Rol:";

            // 
            // cmbRol
            // 
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(170, 274);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(280, 23);
            cmbRol.TabIndex = 12;

            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(328, 431);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(95, 34);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;

            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(429, 431);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(95, 34);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;

            // 
            // FrmAltaUsuario
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(550, 490);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(grpDatosUsuario);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAltaUsuario";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Alta de usuario";
            Load += FrmAltaUsuario_Load;

            grpDatosUsuario.ResumeLayout(false);
            grpDatosUsuario.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;

        private GroupBox grpDatosUsuario;

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

        private Button btnGuardar;
        private Button btnCancelar;
    }
}
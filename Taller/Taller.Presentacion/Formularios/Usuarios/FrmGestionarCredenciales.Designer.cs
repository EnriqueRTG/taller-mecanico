namespace Taller.Presentacion.Formularios.Usuarios
{
    partial class FrmGestionarCredenciales
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
            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            pnlDatos = new Panel();
            tlpDatos = new TableLayoutPanel();
            txtConfirmarPassword = new TextBox();
            txtNuevaPassword = new TextBox();
            txtNombreUsuario = new TextBox();
            txtNombreCompleto = new TextBox();
            txtId = new TextBox();
            lblId = new Label();
            lblNombreUsuario = new Label();
            lblNombreCompleto = new Label();
            lblNuevaPassword = new Label();
            lblConfirmarPassword = new Label();
            lblAyudaPassword = new Label();
            pnlOpcionesPassword = new Panel();
            chkMostrarPassword = new CheckBox();
            btnGenerarPassword = new Button();
            flpAcciones = new FlowLayoutPanel();
            btnGuardar = new Button();
            btnCancelar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlDatos.SuspendLayout();
            tlpDatos.SuspendLayout();
            pnlOpcionesPassword.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
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
            tlpPrincipal.Padding = new Padding(28, 22, 28, 22);
            tlpPrincipal.RowCount = 3;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tlpPrincipal.Size = new Size(584, 541);
            tlpPrincipal.TabIndex = 27;
            // 
            // pnlCabecera
            // 
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(28, 22);
            pnlCabecera.Margin = new Padding(0);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(528, 75);
            pnlCabecera.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 17.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(254, 31);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestionar credenciales";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(2, 39);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(400, 17);
            lblDescripcion.TabIndex = 4;
            lblDescripcion.Text = "Actualice el nombre de usuario o establezca una nueva contraseña.";
            // 
            // pnlDatos
            // 
            pnlDatos.Controls.Add(tlpDatos);
            pnlDatos.Dock = DockStyle.Fill;
            pnlDatos.Location = new Point(28, 97);
            pnlDatos.Margin = new Padding(0);
            pnlDatos.Name = "pnlDatos";
            pnlDatos.Padding = new Padding(2);
            pnlDatos.Size = new Size(528, 357);
            pnlDatos.TabIndex = 1;
            // 
            // tlpDatos
            // 
            tlpDatos.BackColor = Color.White;
            tlpDatos.ColumnCount = 2;
            tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
            tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDatos.Controls.Add(txtConfirmarPassword, 1, 4);
            tlpDatos.Controls.Add(txtNuevaPassword, 1, 3);
            tlpDatos.Controls.Add(txtNombreUsuario, 1, 2);
            tlpDatos.Controls.Add(txtNombreCompleto, 1, 1);
            tlpDatos.Controls.Add(txtId, 1, 0);
            tlpDatos.Controls.Add(lblId, 0, 0);
            tlpDatos.Controls.Add(lblNombreUsuario, 0, 2);
            tlpDatos.Controls.Add(lblNombreCompleto, 0, 1);
            tlpDatos.Controls.Add(lblNuevaPassword, 0, 3);
            tlpDatos.Controls.Add(lblConfirmarPassword, 0, 4);
            tlpDatos.Controls.Add(lblAyudaPassword, 1, 5);
            tlpDatos.Controls.Add(pnlOpcionesPassword, 1, 6);
            tlpDatos.Dock = DockStyle.Fill;
            tlpDatos.Location = new Point(2, 2);
            tlpDatos.Name = "tlpDatos";
            tlpDatos.RowCount = 7;
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpDatos.Size = new Size(524, 353);
            tlpDatos.TabIndex = 0;
            // 
            // txtConfirmarPassword
            // 
            txtConfirmarPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmarPassword.Dock = DockStyle.Fill;
            txtConfirmarPassword.Font = new Font("Segoe UI", 9.75F);
            txtConfirmarPassword.Location = new Point(155, 214);
            txtConfirmarPassword.Margin = new Padding(0, 10, 0, 10);
            txtConfirmarPassword.MaxLength = 100;
            txtConfirmarPassword.Name = "txtConfirmarPassword";
            txtConfirmarPassword.PlaceholderText = "Repita la contraseña";
            txtConfirmarPassword.Size = new Size(369, 25);
            txtConfirmarPassword.TabIndex = 2;
            txtConfirmarPassword.UseSystemPasswordChar = true;
            // 
            // txtNuevaPassword
            // 
            txtNuevaPassword.BorderStyle = BorderStyle.FixedSingle;
            txtNuevaPassword.Dock = DockStyle.Fill;
            txtNuevaPassword.Font = new Font("Segoe UI", 9.75F);
            txtNuevaPassword.Location = new Point(155, 160);
            txtNuevaPassword.Margin = new Padding(0, 10, 0, 10);
            txtNuevaPassword.MaxLength = 100;
            txtNuevaPassword.Name = "txtNuevaPassword";
            txtNuevaPassword.PlaceholderText = "Ingrese una contraseña";
            txtNuevaPassword.Size = new Size(369, 25);
            txtNuevaPassword.TabIndex = 1;
            txtNuevaPassword.UseSystemPasswordChar = true;
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtNombreUsuario.Dock = DockStyle.Fill;
            txtNombreUsuario.Font = new Font("Segoe UI", 9.75F);
            txtNombreUsuario.Location = new Point(155, 106);
            txtNombreUsuario.Margin = new Padding(0, 10, 0, 10);
            txtNombreUsuario.MaxLength = 50;
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.PlaceholderText = "Usuario utilizado para iniciar sesión";
            txtNombreUsuario.Size = new Size(369, 25);
            txtNombreUsuario.TabIndex = 0;
            // 
            // txtNombreCompleto
            // 
            txtNombreCompleto.BackColor = Color.FromArgb(241, 245, 249);
            txtNombreCompleto.BorderStyle = BorderStyle.FixedSingle;
            txtNombreCompleto.Dock = DockStyle.Fill;
            txtNombreCompleto.Font = new Font("Segoe UI", 9.75F);
            txtNombreCompleto.Location = new Point(155, 56);
            txtNombreCompleto.Margin = new Padding(0, 8, 0, 8);
            txtNombreCompleto.MaxLength = 100;
            txtNombreCompleto.Name = "txtNombreCompleto";
            txtNombreCompleto.PlaceholderText = "Nombre y Apellido";
            txtNombreCompleto.ReadOnly = true;
            txtNombreCompleto.Size = new Size(369, 25);
            txtNombreCompleto.TabIndex = 101;
            txtNombreCompleto.TabStop = false;
            // 
            // txtId
            // 
            txtId.BackColor = Color.FromArgb(241, 245, 249);
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.Dock = DockStyle.Fill;
            txtId.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtId.Location = new Point(155, 8);
            txtId.Margin = new Padding(0, 8, 0, 8);
            txtId.MaxLength = 100;
            txtId.Name = "txtId";
            txtId.PlaceholderText = "ID";
            txtId.ReadOnly = true;
            txtId.Size = new Size(369, 25);
            txtId.TabIndex = 102;
            txtId.TabStop = false;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Dock = DockStyle.Fill;
            lblId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblId.ForeColor = Color.FromArgb(30, 41, 59);
            lblId.Location = new Point(4, 3);
            lblId.Margin = new Padding(4, 3, 8, 3);
            lblId.Name = "lblId";
            lblId.Size = new Size(143, 42);
            lblId.TabIndex = 99;
            lblId.Text = "ID:";
            lblId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Dock = DockStyle.Fill;
            lblNombreUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombreUsuario.ForeColor = Color.FromArgb(30, 41, 59);
            lblNombreUsuario.Location = new Point(4, 99);
            lblNombreUsuario.Margin = new Padding(4, 3, 8, 3);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(143, 48);
            lblNombreUsuario.TabIndex = 17;
            lblNombreUsuario.Text = "Nombre usuario:";
            lblNombreUsuario.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNombreCompleto
            // 
            lblNombreCompleto.AutoSize = true;
            lblNombreCompleto.Dock = DockStyle.Fill;
            lblNombreCompleto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombreCompleto.ForeColor = Color.FromArgb(30, 41, 59);
            lblNombreCompleto.Location = new Point(4, 51);
            lblNombreCompleto.Margin = new Padding(4, 3, 8, 3);
            lblNombreCompleto.Name = "lblNombreCompleto";
            lblNombreCompleto.Size = new Size(143, 42);
            lblNombreCompleto.TabIndex = 15;
            lblNombreCompleto.Text = " Nombre completo:";
            lblNombreCompleto.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNuevaPassword
            // 
            lblNuevaPassword.AutoSize = true;
            lblNuevaPassword.Dock = DockStyle.Fill;
            lblNuevaPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNuevaPassword.ForeColor = Color.FromArgb(30, 41, 59);
            lblNuevaPassword.Location = new Point(4, 153);
            lblNuevaPassword.Margin = new Padding(4, 3, 8, 3);
            lblNuevaPassword.Name = "lblNuevaPassword";
            lblNuevaPassword.Size = new Size(143, 48);
            lblNuevaPassword.TabIndex = 19;
            lblNuevaPassword.Text = "Nueva contraseña:";
            lblNuevaPassword.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblConfirmarPassword
            // 
            lblConfirmarPassword.AutoSize = true;
            lblConfirmarPassword.Dock = DockStyle.Fill;
            lblConfirmarPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblConfirmarPassword.ForeColor = Color.FromArgb(30, 41, 59);
            lblConfirmarPassword.Location = new Point(4, 207);
            lblConfirmarPassword.Margin = new Padding(4, 3, 8, 3);
            lblConfirmarPassword.Name = "lblConfirmarPassword";
            lblConfirmarPassword.Size = new Size(143, 48);
            lblConfirmarPassword.TabIndex = 21;
            lblConfirmarPassword.Text = "Confirmar contraseña:";
            lblConfirmarPassword.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAyudaPassword
            // 
            lblAyudaPassword.Dock = DockStyle.Fill;
            lblAyudaPassword.Font = new Font("Segoe UI", 8.5F);
            lblAyudaPassword.ForeColor = Color.FromArgb(100, 116, 139);
            lblAyudaPassword.Location = new Point(158, 258);
            lblAyudaPassword.Name = "lblAyudaPassword";
            lblAyudaPassword.Size = new Size(363, 42);
            lblAyudaPassword.TabIndex = 23;
            lblAyudaPassword.Text = "Deje ambos campos vacíos para conservar la contraseña actual.";
            lblAyudaPassword.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlOpcionesPassword
            // 
            pnlOpcionesPassword.Controls.Add(chkMostrarPassword);
            pnlOpcionesPassword.Controls.Add(btnGenerarPassword);
            pnlOpcionesPassword.Dock = DockStyle.Fill;
            pnlOpcionesPassword.Location = new Point(158, 303);
            pnlOpcionesPassword.Name = "pnlOpcionesPassword";
            pnlOpcionesPassword.Size = new Size(363, 47);
            pnlOpcionesPassword.TabIndex = 24;
            // 
            // chkMostrarPassword
            // 
            chkMostrarPassword.Anchor = AnchorStyles.Left;
            chkMostrarPassword.AutoSize = true;
            chkMostrarPassword.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkMostrarPassword.ForeColor = Color.FromArgb(51, 65, 85);
            chkMostrarPassword.Location = new Point(0, 14);
            chkMostrarPassword.Margin = new Padding(0, 4, 0, 4);
            chkMostrarPassword.Name = "chkMostrarPassword";
            chkMostrarPassword.Size = new Size(133, 19);
            chkMostrarPassword.TabIndex = 3;
            chkMostrarPassword.Text = "Mostrar contraseñas";
            chkMostrarPassword.UseVisualStyleBackColor = true;
            chkMostrarPassword.CheckedChanged += chkMostrarPassword_CheckedChanged;
            // 
            // btnGenerarPassword
            // 
            btnGenerarPassword.Cursor = Cursors.Hand;
            btnGenerarPassword.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            btnGenerarPassword.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnGenerarPassword.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btnGenerarPassword.FlatStyle = FlatStyle.Flat;
            btnGenerarPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerarPassword.ForeColor = Color.FromArgb(30, 64, 175);
            btnGenerarPassword.Location = new Point(194, 6);
            btnGenerarPassword.Name = "btnGenerarPassword";
            btnGenerarPassword.Size = new Size(150, 32);
            btnGenerarPassword.TabIndex = 4;
            btnGenerarPassword.Text = "Generar contraseña";
            btnGenerarPassword.UseVisualStyleBackColor = false;
            btnGenerarPassword.Click += btnGenerarPassword_Click;
            // 
            // flpAcciones
            // 
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.FlowDirection = FlowDirection.RightToLeft;
            flpAcciones.Location = new Point(28, 454);
            flpAcciones.Margin = new Padding(0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Padding = new Padding(0, 14, 0, 0);
            flpAcciones.Size = new Size(528, 65);
            flpAcciones.TabIndex = 2;
            flpAcciones.WrapContents = false;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.None;
            btnGuardar.BackColor = Color.FromArgb(30, 64, 175);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 58, 138);
            btnGuardar.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(363, 14);
            btnGuardar.Margin = new Padding(8, 0, 0, 0);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(165, 38);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar credenciales";
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
            btnCancelar.Location = new Point(235, 14);
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FrmGestionarCredenciales
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(584, 541);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmGestionarCredenciales";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gestionar credenciales";
            tlpPrincipal.ResumeLayout(false);
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlDatos.ResumeLayout(false);
            tlpDatos.ResumeLayout(false);
            tlpDatos.PerformLayout();
            pnlOpcionesPassword.ResumeLayout(false);
            pnlOpcionesPassword.PerformLayout();
            flpAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlCabecera;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Panel pnlDatos;
        private TableLayoutPanel tlpDatos;
        private TextBox txtConfirmarPassword;
        private TextBox txtNuevaPassword;
        private TextBox txtNombreUsuario;
        private TextBox txtNombreCompleto;
        private TextBox txtId;
        private Label lblId;
        private Label lblNombreUsuario;
        private Label lblNombreCompleto;
        private Label lblNuevaPassword;
        private Label lblConfirmarPassword;
        private FlowLayoutPanel flpAcciones;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblAyudaPassword;
        private Panel pnlOpcionesPassword;
        private CheckBox chkMostrarPassword;
        private Button btnGenerarPassword;
    }
}
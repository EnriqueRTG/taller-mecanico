namespace Taller.Presentacion.Formularios.Autenticacion
{
    partial class FrmLogin
    {
        /// <summary>
        /// Contenedor de componentes utilizado por Windows Forms.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Libera los recursos utilizados por el formulario.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components is not null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            pnlContenedor = new Panel();
            pnlAcceso = new Panel();
            chkMostrarContrasena = new CheckBox();
            lblSubtitulo = new Label();
            lblTitulo = new Label();
            btnIngresar = new Button();
            txtUsuario = new TextBox();
            lblUsuario = new Label();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            pnlIdentidad = new Panel();
            lblDescripcion = new Label();
            lblNombreSistema = new Label();
            picLogo = new PictureBox();
            pnlContenedor.SuspendLayout();
            pnlAcceso.SuspendLayout();
            pnlIdentidad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // pnlContenedor
            // 
            pnlContenedor.AutoSize = true;
            pnlContenedor.BackColor = Color.FromArgb(244, 247, 251);
            pnlContenedor.Controls.Add(pnlAcceso);
            pnlContenedor.Controls.Add(pnlIdentidad);
            pnlContenedor.Dock = DockStyle.Fill;
            pnlContenedor.ForeColor = Color.FromArgb(24, 42, 70);
            pnlContenedor.Location = new Point(0, 0);
            pnlContenedor.Margin = new Padding(3, 2, 3, 2);
            pnlContenedor.Name = "pnlContenedor";
            pnlContenedor.Size = new Size(744, 401);
            pnlContenedor.TabIndex = 0;
            // 
            // pnlAcceso
            // 
            pnlAcceso.BackColor = Color.White;
            pnlAcceso.Controls.Add(chkMostrarContrasena);
            pnlAcceso.Controls.Add(lblSubtitulo);
            pnlAcceso.Controls.Add(lblTitulo);
            pnlAcceso.Controls.Add(btnIngresar);
            pnlAcceso.Controls.Add(txtUsuario);
            pnlAcceso.Controls.Add(lblUsuario);
            pnlAcceso.Controls.Add(lblContrasena);
            pnlAcceso.Controls.Add(txtContrasena);
            pnlAcceso.Dock = DockStyle.Fill;
            pnlAcceso.Location = new Point(300, 0);
            pnlAcceso.Name = "pnlAcceso";
            pnlAcceso.Size = new Size(444, 401);
            pnlAcceso.TabIndex = 8;
            // 
            // chkMostrarContrasena
            // 
            chkMostrarContrasena.AutoSize = true;
            chkMostrarContrasena.Cursor = Cursors.Hand;
            chkMostrarContrasena.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkMostrarContrasena.ForeColor = Color.FromArgb(75, 85, 100);
            chkMostrarContrasena.Location = new Point(65, 284);
            chkMostrarContrasena.Name = "chkMostrarContrasena";
            chkMostrarContrasena.Size = new Size(128, 19);
            chkMostrarContrasena.TabIndex = 2;
            chkMostrarContrasena.Text = "Mostrar contraseña";
            chkMostrarContrasena.UseVisualStyleBackColor = true;
            chkMostrarContrasena.CheckedChanged += chkMostrarContrasena_CheckedChanged;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.BackColor = Color.Transparent;
            lblSubtitulo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitulo.Location = new Point(65, 101);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(216, 15);
            lblSubtitulo.TabIndex = 6;
            lblSubtitulo.Text = "Ingrese sus credenciales para continuar.";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Segoe UI", 21F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(23, 32, 51);
            lblTitulo.Location = new Point(62, 54);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(187, 38);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Iniciar sesión";
            // 
            // btnIngresar
            // 
            btnIngresar.BackColor = Color.FromArgb(46, 125, 222);
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 83, 151);
            btnIngresar.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 105, 190);
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(65, 329);
            btnIngresar.Margin = new Padding(3, 2, 3, 2);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(330, 44);
            btnIngresar.TabIndex = 3;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = false;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.White;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Location = new Point(65, 169);
            txtUsuario.Margin = new Padding(3, 2, 3, 2);
            txtUsuario.MaxLength = 50;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(330, 25);
            txtUsuario.TabIndex = 0;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.BackColor = Color.Transparent;
            lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUsuario.ForeColor = Color.FromArgb(45, 55, 72);
            lblUsuario.Location = new Point(65, 145);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(49, 15);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario";
            // 
            // lblContrasena
            // 
            lblContrasena.AutoSize = true;
            lblContrasena.BackColor = Color.Transparent;
            lblContrasena.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblContrasena.ForeColor = Color.FromArgb(45, 55, 72);
            lblContrasena.Location = new Point(65, 218);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(69, 15);
            lblContrasena.TabIndex = 2;
            lblContrasena.Text = "Contraseña";
            // 
            // txtContrasena
            // 
            txtContrasena.BackColor = Color.White;
            txtContrasena.BorderStyle = BorderStyle.FixedSingle;
            txtContrasena.Location = new Point(65, 242);
            txtContrasena.Margin = new Padding(3, 2, 3, 2);
            txtContrasena.MaxLength = 100;
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(330, 25);
            txtContrasena.TabIndex = 1;
            txtContrasena.UseSystemPasswordChar = true;
            // 
            // pnlIdentidad
            // 
            pnlIdentidad.BackColor = Color.FromArgb(24, 42, 70);
            pnlIdentidad.Controls.Add(picLogo);
            pnlIdentidad.Controls.Add(lblDescripcion);
            pnlIdentidad.Controls.Add(lblNombreSistema);
            pnlIdentidad.Dock = DockStyle.Left;
            pnlIdentidad.Location = new Point(0, 0);
            pnlIdentidad.Name = "pnlIdentidad";
            pnlIdentidad.Size = new Size(300, 401);
            pnlIdentidad.TabIndex = 6;
            // 
            // lblDescripcion
            // 
            lblDescripcion.BackColor = Color.Transparent;
            lblDescripcion.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescripcion.ForeColor = Color.FromArgb(180, 198, 220);
            lblDescripcion.Location = new Point(38, 295);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(224, 58);
            lblDescripcion.TabIndex = 2;
            lblDescripcion.Text = "Sistema de gestión del circuito operativo\r\ndesde la recepción hasta la entrega.";
            // 
            // lblNombreSistema
            // 
            lblNombreSistema.BackColor = Color.Transparent;
            lblNombreSistema.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombreSistema.ForeColor = Color.White;
            lblNombreSistema.Location = new Point(44, 170);
            lblNombreSistema.Name = "lblNombreSistema";
            lblNombreSistema.Size = new Size(215, 65);
            lblNombreSistema.TabIndex = 1;
            lblNombreSistema.Text = "Sistema de Gestión\r\nde Taller Mecánico\r\n";
            lblNombreSistema.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picLogo
            // 
            picLogo.Anchor = AnchorStyles.None;
            picLogo.BackColor = Color.Transparent;
            picLogo.Image = Properties.Resources.LogoTallerMecanico;
            picLogo.Location = new Point(35, 40);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(230, 230);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 3;
            picLogo.TabStop = false;
            // 
            // FrmLogin
            // 
            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(744, 401);
            Controls.Add(pnlContenedor);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema de Gestión de Taller Mecánico";
            pnlContenedor.ResumeLayout(false);
            pnlAcceso.ResumeLayout(false);
            pnlAcceso.PerformLayout();
            pnlIdentidad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel pnlContenedor;
        private Panel pnlIdentidad;
        private Panel pnlAcceso;
        private Label lblTitulo;
        private Button btnIngresar;
        private TextBox txtUsuario;
        private Label lblUsuario;
        private Label lblContrasena;
        private TextBox txtContrasena;
        private Label lblNombreSistema;
        private Label lblDescripcion;
        private Label lblSubtitulo;
        private CheckBox chkMostrarContrasena;
        private PictureBox picLogo;
    }
}
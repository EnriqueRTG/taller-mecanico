namespace Taller.Presentacion.Formularios.Autenticacion
{
    partial class Login
    {
        /// <summary>
        /// Contenedor de componentes utilizado por Windows Forms.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

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
            panel1 = new Panel();
            iniciar = new Button();
            textBox_contraseña = new TextBox();
            textBox_usuario = new TextBox();
            contraseña = new Label();
            usuario = new Label();
            inicio_sesion = new Label();

            panel1.SuspendLayout();
            SuspendLayout();

            // 
            // panel1
            // 
            panel1.BackColor = Color.Beige;
            panel1.Controls.Add(iniciar);
            panel1.Controls.Add(textBox_contraseña);
            panel1.Controls.Add(textBox_usuario);
            panel1.Controls.Add(contraseña);
            panel1.Controls.Add(usuario);
            panel1.Controls.Add(inicio_sesion);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 450);
            panel1.TabIndex = 0;

            // 
            // iniciar
            // 
            iniciar.BackColor = Color.PaleGoldenrod;
            iniciar.Location = new Point(338, 301);
            iniciar.Name = "iniciar";
            iniciar.Size = new Size(137, 44);
            iniciar.TabIndex = 5;
            iniciar.Text = "Iniciar";
            iniciar.UseVisualStyleBackColor = false;
            iniciar.Click += iniciar_Click;

            // 
            // textBox_contraseña
            // 
            textBox_contraseña.Location = new Point(285, 244);
            textBox_contraseña.Name = "textBox_contraseña";
            textBox_contraseña.PasswordChar = '●';
            textBox_contraseña.Size = new Size(258, 27);
            textBox_contraseña.TabIndex = 4;

            // 
            // textBox_usuario
            // 
            textBox_usuario.Location = new Point(285, 168);
            textBox_usuario.Name = "textBox_usuario";
            textBox_usuario.Size = new Size(258, 27);
            textBox_usuario.TabIndex = 3;

            // 
            // contraseña
            // 
            contraseña.AutoSize = true;
            contraseña.Font = new Font(
                "Microsoft Sans Serif",
                10.2F,
                FontStyle.Regular);
            contraseña.Location = new Point(281, 209);
            contraseña.Name = "contraseña";
            contraseña.Size = new Size(95, 20);
            contraseña.TabIndex = 2;
            contraseña.Text = "Contraseña";

            // 
            // usuario
            // 
            usuario.AutoSize = true;
            usuario.Font = new Font(
                "Microsoft Sans Serif",
                10.2F,
                FontStyle.Regular);
            usuario.Location = new Point(281, 145);
            usuario.Name = "usuario";
            usuario.Size = new Size(67, 20);
            usuario.TabIndex = 1;
            usuario.Text = "Usuario";

            // 
            // inicio_sesion
            // 
            inicio_sesion.AutoSize = true;
            inicio_sesion.BackColor = Color.Beige;
            inicio_sesion.Font = new Font(
                "MS Reference Sans Serif",
                19.8F,
                FontStyle.Bold);
            inicio_sesion.ForeColor = Color.MidnightBlue;
            inicio_sesion.Location = new Point(278, 68);
            inicio_sesion.Name = "inicio_sesion";
            inicio_sesion.Size = new Size(256, 40);
            inicio_sesion.TabIndex = 0;
            inicio_sesion.Text = "Iniciar Sesión";

            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inicio de Sesión";

            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label contraseña;
        private Label usuario;
        private Label inicio_sesion;
        private Button iniciar;
        private TextBox textBox_contraseña;
        private TextBox textBox_usuario;
    }
}
namespace Taller.Presentacion.Formularios.Administrador
{
    partial class usuarios_grid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(usuarios_grid));
            crudUusario = new Label();
            usuario_grid = new DataGridView();
            nombre_usuario = new DataGridViewTextBoxColumn();
            password = new DataGridViewTextBoxColumn();
            nombre = new DataGridViewTextBoxColumn();
            apellido = new DataGridViewTextBoxColumn();
            activo = new DataGridViewTextBoxColumn();
            rol = new DataGridViewTextBoxColumn();
            fecha_alta = new DataGridViewTextBoxColumn();
            inhabilitar = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)usuario_grid).BeginInit();
            SuspendLayout();
            // 
            // crudUusario
            // 
            crudUusario.AutoSize = true;
            crudUusario.Font = new Font("Georgia", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crudUusario.ForeColor = Color.MidnightBlue;
            crudUusario.Location = new Point(555, 39);
            crudUusario.Name = "crudUusario";
            crudUusario.Size = new Size(191, 43);
            crudUusario.TabIndex = 1;
            crudUusario.Text = "Usuarios";
            crudUusario.Click += crudUusario_Click;
            // 
            // usuario_grid
            // 
            usuario_grid.BackgroundColor = Color.MidnightBlue;
            usuario_grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            usuario_grid.Columns.AddRange(new DataGridViewColumn[] { nombre_usuario, password, nombre, apellido, activo, rol, fecha_alta, inhabilitar });
            usuario_grid.GridColor = Color.Black;
            usuario_grid.Location = new Point(138, 100);
            usuario_grid.Name = "usuario_grid";
            usuario_grid.RowHeadersWidth = 51;
            usuario_grid.Size = new Size(1055, 553);
            usuario_grid.TabIndex = 2;
            usuario_grid.CellContentClick += usuario_grid_CellContentClick;
            // 
            // nombre_usuario
            // 
            nombre_usuario.HeaderText = "Usuario";
            nombre_usuario.MinimumWidth = 6;
            nombre_usuario.Name = "nombre_usuario";
            nombre_usuario.Width = 125;
            // 
            // password
            // 
            password.HeaderText = "Contraseña";
            password.MinimumWidth = 6;
            password.Name = "password";
            password.Width = 125;
            // 
            // nombre
            // 
            nombre.HeaderText = "Nombre";
            nombre.MinimumWidth = 6;
            nombre.Name = "nombre";
            nombre.Width = 125;
            // 
            // apellido
            // 
            apellido.HeaderText = "Apellido";
            apellido.MinimumWidth = 6;
            apellido.Name = "apellido";
            apellido.Width = 125;
            // 
            // activo
            // 
            activo.HeaderText = "Activo";
            activo.MinimumWidth = 6;
            activo.Name = "activo";
            activo.Width = 125;
            // 
            // rol
            // 
            rol.HeaderText = "Rol";
            rol.MinimumWidth = 6;
            rol.Name = "rol";
            rol.Width = 125;
            // 
            // fecha_alta
            // 
            fecha_alta.HeaderText = "Fecha de Alta";
            fecha_alta.MinimumWidth = 6;
            fecha_alta.Name = "fecha_alta";
            fecha_alta.Width = 125;
            // 
            // inhabilitar
            // 
            inhabilitar.HeaderText = "Inhabilitar";
            inhabilitar.MinimumWidth = 6;
            inhabilitar.Name = "inhabilitar";
            inhabilitar.Width = 125;
            // 
            // usuarios_grid
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Beige;
            ClientSize = new Size(1330, 707);
            Controls.Add(usuario_grid);
            Controls.Add(crudUusario);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "usuarios_grid";
            Text = "usuarios_grid";
            ((System.ComponentModel.ISupportInitialize)usuario_grid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label crudUusario;
        private DataGridView usuario_grid;
        private DataGridViewTextBoxColumn nombre_usuario;
        private DataGridViewTextBoxColumn password;
        private DataGridViewTextBoxColumn nombre;
        private DataGridViewTextBoxColumn apellido;
        private DataGridViewTextBoxColumn activo;
        private DataGridViewTextBoxColumn rol;
        private DataGridViewTextBoxColumn fecha_alta;
        private DataGridViewButtonColumn inhabilitar;
    }
}
namespace Taller.Presentacion.Formularios.Administrador
{
    partial class usuario_crud
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(usuario_crud));
            crudUusario = new Label();
            usuario = new Label();
            nombre = new Label();
            contraseña = new Label();
            apellido = new Label();
            text_nombre = new TextBox();
            text_apellido = new TextBox();
            text_usuario = new TextBox();
            text_contraseña = new TextBox();
            rol = new Label();
            combo_rol = new ComboBox();
            guardar = new Button();
            borrar = new Button();
            button1 = new Button();
            tabla_usuario = new DataGridView();
            eliminar = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)tabla_usuario).BeginInit();
            SuspendLayout();
            // 
            // crudUusario
            // 
            crudUusario.AutoSize = true;
            crudUusario.Font = new Font("Georgia", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crudUusario.ForeColor = Color.MidnightBlue;
            crudUusario.Location = new Point(528, 66);
            crudUusario.Name = "crudUusario";
            crudUusario.Size = new Size(373, 43);
            crudUusario.TabIndex = 0;
            crudUusario.Text = "CRUD de Usuarios";
            crudUusario.Click += crudUusario_Click;
            // 
            // usuario
            // 
            usuario.AutoSize = true;
            usuario.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            usuario.ForeColor = Color.MidnightBlue;
            usuario.Location = new Point(696, 188);
            usuario.Name = "usuario";
            usuario.Size = new Size(72, 23);
            usuario.TabIndex = 1;
            usuario.Text = "Usuario:";
            // 
            // nombre
            // 
            nombre.AutoSize = true;
            nombre.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            nombre.ForeColor = Color.MidnightBlue;
            nombre.Location = new Point(185, 188);
            nombre.Name = "nombre";
            nombre.Size = new Size(77, 23);
            nombre.TabIndex = 2;
            nombre.Text = "Nombre:";
            // 
            // contraseña
            // 
            contraseña.AutoSize = true;
            contraseña.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            contraseña.ForeColor = Color.MidnightBlue;
            contraseña.Location = new Point(696, 300);
            contraseña.Name = "contraseña";
            contraseña.Size = new Size(101, 23);
            contraseña.TabIndex = 3;
            contraseña.Text = "Contraseña:";
            // 
            // apellido
            // 
            apellido.AutoSize = true;
            apellido.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            apellido.ForeColor = Color.MidnightBlue;
            apellido.Location = new Point(185, 300);
            apellido.Name = "apellido";
            apellido.Size = new Size(76, 23);
            apellido.TabIndex = 4;
            apellido.Text = "Apellido:";
            // 
            // text_nombre
            // 
            text_nombre.Location = new Point(267, 188);
            text_nombre.Name = "text_nombre";
            text_nombre.Size = new Size(382, 27);
            text_nombre.TabIndex = 5;
            // 
            // text_apellido
            // 
            text_apellido.Location = new Point(267, 296);
            text_apellido.Name = "text_apellido";
            text_apellido.Size = new Size(382, 27);
            text_apellido.TabIndex = 6;
            // 
            // text_usuario
            // 
            text_usuario.Location = new Point(803, 187);
            text_usuario.Name = "text_usuario";
            text_usuario.Size = new Size(382, 27);
            text_usuario.TabIndex = 7;
            // 
            // text_contraseña
            // 
            text_contraseña.Location = new Point(803, 296);
            text_contraseña.Name = "text_contraseña";
            text_contraseña.Size = new Size(382, 27);
            text_contraseña.TabIndex = 8;
            text_contraseña.TextChanged += text_contraseña_TextChanged;
            // 
            // rol
            // 
            rol.AutoSize = true;
            rol.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rol.ForeColor = Color.MidnightBlue;
            rol.Location = new Point(185, 397);
            rol.Name = "rol";
            rol.Size = new Size(39, 23);
            rol.TabIndex = 9;
            rol.Text = "Rol:";
            // 
            // combo_rol
            // 
            combo_rol.FormattingEnabled = true;
            combo_rol.Location = new Point(267, 396);
            combo_rol.Name = "combo_rol";
            combo_rol.Size = new Size(382, 28);
            combo_rol.TabIndex = 10;
            combo_rol.SelectedIndexChanged += combo_rol_SelectedIndexChanged;
            // 
            // guardar
            // 
            guardar.BackColor = Color.LightGreen;
            guardar.Location = new Point(767, 389);
            guardar.Name = "guardar";
            guardar.Size = new Size(99, 41);
            guardar.TabIndex = 11;
            guardar.Text = "Guardar";
            guardar.UseVisualStyleBackColor = false;
            guardar.Click += guardar_Click;
            // 
            // borrar
            // 
            borrar.BackColor = Color.IndianRed;
            borrar.Location = new Point(898, 389);
            borrar.Name = "borrar";
            borrar.Size = new Size(99, 41);
            borrar.TabIndex = 12;
            borrar.Text = "Borrar";
            borrar.UseVisualStyleBackColor = false;
            borrar.Click += borrar_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.SteelBlue;
            button1.Location = new Point(1036, 389);
            button1.Name = "button1";
            button1.Size = new Size(99, 41);
            button1.TabIndex = 13;
            button1.Text = "Modificar";
            button1.UseVisualStyleBackColor = false;
            // 
            // tabla_usuario
            // 
            tabla_usuario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tabla_usuario.Columns.AddRange(new DataGridViewColumn[] { eliminar });
            tabla_usuario.Location = new Point(185, 443);
            tabla_usuario.Name = "tabla_usuario";
            tabla_usuario.RowHeadersWidth = 51;
            tabla_usuario.Size = new Size(1000, 279);
            tabla_usuario.TabIndex = 14;
            // 
            // eliminar
            // 
            eliminar.HeaderText = "Eliminar";
            eliminar.MinimumWidth = 6;
            eliminar.Name = "eliminar";
            eliminar.Text = "Eliminar";
            eliminar.Width = 125;
            // 
            // usuario_crud
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Beige;
            ClientSize = new Size(1336, 744);
            Controls.Add(tabla_usuario);
            Controls.Add(button1);
            Controls.Add(borrar);
            Controls.Add(guardar);
            Controls.Add(combo_rol);
            Controls.Add(rol);
            Controls.Add(text_contraseña);
            Controls.Add(text_usuario);
            Controls.Add(text_apellido);
            Controls.Add(text_nombre);
            Controls.Add(apellido);
            Controls.Add(contraseña);
            Controls.Add(nombre);
            Controls.Add(usuario);
            Controls.Add(crudUusario);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "usuario_crud";
            Text = "CRUD Usuario";
            Click += usuario_crud_Click;
            ((System.ComponentModel.ISupportInitialize)tabla_usuario).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label crudUusario;
        private Label usuario;
        private Label nombre;
        private Label contraseña;
        private Label apellido;
        private TextBox text_nombre;
        private TextBox text_apellido;
        private TextBox text_usuario;
        private TextBox text_contraseña;
        private Label rol;
        private ComboBox combo_rol;
        private Button guardar;
        private Button borrar;
        private Button button1;
        private DataGridView tabla_usuario;
        private DataGridViewButtonColumn eliminar;
    }
}
namespace Taller.Presentacion.Formularios.Administrador
{
    partial class usuarios_eliminados
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(usuarios_eliminados));
            crudUusario = new Label();
            tabla_eliminados = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)tabla_eliminados).BeginInit();
            SuspendLayout();
            // 
            // crudUusario
            // 
            crudUusario.AutoSize = true;
            crudUusario.Font = new Font("Georgia", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crudUusario.ForeColor = Color.MidnightBlue;
            crudUusario.Location = new Point(388, 56);
            crudUusario.Name = "crudUusario";
            crudUusario.Size = new Size(420, 43);
            crudUusario.TabIndex = 1;
            crudUusario.Text = "Usuarios Eliminados";
            // 
            // tabla_eliminados
            // 
            tabla_eliminados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tabla_eliminados.Location = new Point(112, 134);
            tabla_eliminados.Name = "tabla_eliminados";
            tabla_eliminados.RowHeadersWidth = 51;
            tabla_eliminados.Size = new Size(1000, 551);
            tabla_eliminados.TabIndex = 15;
            // 
            // usuarios_eliminados
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Beige;
            ClientSize = new Size(1256, 730);
            Controls.Add(tabla_eliminados);
            Controls.Add(crudUusario);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "usuarios_eliminados";
            Text = "Usuarios eliminados";
            ((System.ComponentModel.ISupportInitialize)tabla_eliminados).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label crudUusario;
        private DataGridView tabla_eliminados;
    }
}
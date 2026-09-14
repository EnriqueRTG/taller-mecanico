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
            lblTitulo = new Label();
            lblDescripcion = new Label();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(24, 22);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(230, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de clientes";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(26, 62);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(302, 17);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Consulta y administración de los clientes del taller.";
            // 
            // FrmClientes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(800, 450);
            Controls.Add(lblDescripcion);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(700, 450);
            Name = "FrmClientes";
            Padding = new Padding(24, 20, 24, 20);
            Text = "FrmClientes";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDescripcion;
    }
}
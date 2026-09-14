namespace Taller.Presentacion.Formularios.Principal
{
    partial class FrmInicio
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
            lblBienvenida = new Label();
            lblDescripcionInicio = new Label();
            lblRolActual = new Label();
            SuspendLayout();
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.BackColor = Color.Transparent;
            lblBienvenida.ImageAlign = ContentAlignment.MiddleRight;
            lblBienvenida.Location = new Point(32, 30);
            lblBienvenida.Margin = new Padding(7, 0, 7, 0);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(172, 40);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido";
            // 
            // lblDescripcionInicio
            // 
            lblDescripcionInicio.AutoSize = true;
            lblDescripcionInicio.BackColor = Color.Transparent;
            lblDescripcionInicio.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescripcionInicio.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcionInicio.Location = new Point(35, 78);
            lblDescripcionInicio.Name = "lblDescripcionInicio";
            lblDescripcionInicio.Size = new Size(291, 17);
            lblDescripcionInicio.TabIndex = 1;
            lblDescripcionInicio.Text = "Seleccione una opción del menú para comenzar.";
            // 
            // lblRolActual
            // 
            lblRolActual.AutoSize = true;
            lblRolActual.BackColor = Color.Transparent;
            lblRolActual.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRolActual.ForeColor = Color.FromArgb(46, 125, 222);
            lblRolActual.Location = new Point(35, 108);
            lblRolActual.Name = "lblRolActual";
            lblRolActual.Size = new Size(61, 15);
            lblRolActual.TabIndex = 2;
            lblRolActual.Text = "Rol actual";
            // 
            // FrmInicio
            // 
            AutoScaleDimensions = new SizeF(17F, 40F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(1924, 1061);
            Controls.Add(lblRolActual);
            Controls.Add(lblDescripcionInicio);
            Controls.Add(lblBienvenida);
            Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(23, 32, 51);
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(32, 30);
            Margin = new Padding(7);
            Name = "FrmInicio";
            Text = "Inicio";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida;
        private Label lblDescripcionInicio;
        private Label lblRolActual;
    }
}
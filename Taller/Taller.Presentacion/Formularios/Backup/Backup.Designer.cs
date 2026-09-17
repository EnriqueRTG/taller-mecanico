namespace Taller.Presentacion.Formularios.Backup
{
    partial class Backup
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
            btnNuevoUsuario = new Button();
            lblDescripcion = new Label();
            lblTitulo = new Label();
            pnlFiltros = new Panel();
            btn_generar = new Button();
            button2 = new Button();
            fecha_desde = new Label();
            NombreBaseDatos = new TextBox();
            lblBuscar = new Label();
            lblCantidad = new Label();
            button1 = new Button();
            RutaArchivo = new TextBox();
            tlpPrincipal.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
            tlpPrincipal.Controls.Add(lblCantidad, 0, 2);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 2;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 97F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 308F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPrincipal.Size = new Size(800, 450);
            tlpPrincipal.TabIndex = 3;
            // 
            // pnlCabecera
            // 
            pnlCabecera.BackColor = Color.White;
            pnlCabecera.Controls.Add(btnNuevoUsuario);
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Margin = new Padding(0, 0, 0, 10);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Padding = new Padding(20, 12, 20, 10);
            pnlCabecera.Size = new Size(800, 87);
            pnlCabecera.TabIndex = 0;
            // 
            // btnNuevoUsuario
            // 
            btnNuevoUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoUsuario.BackColor = Color.FromArgb(30, 64, 175);
            btnNuevoUsuario.Cursor = Cursors.Hand;
            btnNuevoUsuario.FlatAppearance.BorderSize = 0;
            btnNuevoUsuario.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 58, 138);
            btnNuevoUsuario.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnNuevoUsuario.FlatStyle = FlatStyle.Flat;
            btnNuevoUsuario.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnNuevoUsuario.ForeColor = Color.White;
            btnNuevoUsuario.Location = new Point(1685, 39);
            btnNuevoUsuario.Name = "btnNuevoUsuario";
            btnNuevoUsuario.Size = new Size(150, 40);
            btnNuevoUsuario.TabIndex = 0;
            btnNuevoUsuario.Text = "＋ Nuevo usuario";
            btnNuevoUsuario.UseVisualStyleBackColor = false;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(43, 53);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(307, 21);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Generá una copia de seguridad del sistema";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(38, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(361, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Respaldo de Base de Datos";
            // 
            // pnlFiltros
            // 
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.Controls.Add(RutaArchivo);
            pnlFiltros.Controls.Add(btn_generar);
            pnlFiltros.Controls.Add(button2);
            pnlFiltros.Controls.Add(button1);
            pnlFiltros.Controls.Add(fecha_desde);
            pnlFiltros.Controls.Add(NombreBaseDatos);
            pnlFiltros.Controls.Add(lblBuscar);
            pnlFiltros.Dock = DockStyle.Fill;
            pnlFiltros.Location = new Point(0, 97);
            pnlFiltros.Margin = new Padding(0, 0, 0, 10);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(18, 10, 18, 10);
            pnlFiltros.Size = new Size(800, 298);
            pnlFiltros.TabIndex = 1;
            // 
            // btn_generar
            // 
            btn_generar.Cursor = Cursors.Hand;
            btn_generar.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            btn_generar.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btn_generar.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btn_generar.FlatStyle = FlatStyle.Flat;
            btn_generar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_generar.ForeColor = Color.FromArgb(30, 64, 175);
            btn_generar.Location = new Point(213, 218);
            btn_generar.Name = "btn_generar";
            btn_generar.Size = new Size(154, 32);
            btn_generar.TabIndex = 12;
            btn_generar.Text = "Generar Respaldo";
            btn_generar.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.FromArgb(30, 64, 175);
            button2.Location = new Point(391, 218);
            button2.Name = "button2";
            button2.Size = new Size(154, 32);
            button2.TabIndex = 11;
            button2.Text = "Restaurar respaldo";
            button2.UseVisualStyleBackColor = true;
            // 
            // fecha_desde
            // 
            fecha_desde.AutoSize = true;
            fecha_desde.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fecha_desde.ForeColor = Color.FromArgb(51, 65, 85);
            fecha_desde.Location = new Point(169, 127);
            fecha_desde.Name = "fecha_desde";
            fecha_desde.Size = new Size(166, 20);
            fecha_desde.TabIndex = 4;
            fecha_desde.Text = "Ubicación del respaldo";
            // 
            // NombreBaseDatos
            // 
            NombreBaseDatos.BorderStyle = BorderStyle.FixedSingle;
            NombreBaseDatos.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NombreBaseDatos.Location = new Point(169, 73);
            NombreBaseDatos.Name = "NombreBaseDatos";
            NombreBaseDatos.Size = new Size(280, 29);
            NombreBaseDatos.TabIndex = 1;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBuscar.ForeColor = Color.FromArgb(51, 65, 85);
            lblBuscar.Location = new Point(169, 50);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(108, 20);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Base de Datos";
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.ForeColor = Color.FromArgb(100, 116, 139);
            lblCantidad.Location = new Point(4, 409);
            lblCantidad.Margin = new Padding(4, 4, 0, 0);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(796, 41);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "0 usuarios encontrados";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.FromArgb(30, 64, 175);
            button1.Location = new Point(468, 150);
            button1.Name = "button1";
            button1.Size = new Size(110, 32);
            button1.TabIndex = 10;
            button1.Text = "Buscar";
            button1.UseVisualStyleBackColor = true;
            // 
            // RutaArchivo
            // 
            RutaArchivo.BorderStyle = BorderStyle.FixedSingle;
            RutaArchivo.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            RutaArchivo.Location = new Point(169, 150);
            RutaArchivo.Name = "RutaArchivo";
            RutaArchivo.Size = new Size(280, 29);
            RutaArchivo.TabIndex = 13;
            RutaArchivo.TextChanged += RutaArchivo_TextChanged;
            // 
            // Backup
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Backup";
            Text = "Backup";
            tlpPrincipal.ResumeLayout(false);
            tlpPrincipal.PerformLayout();
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlCabecera;
        private Button btnNuevoUsuario;
        private Label lblDescripcion;
        private Label lblTitulo;
        private Panel pnlFiltros;
        private Button btn_generar;
        private Button button2;
        private Label fecha_desde;
        private TextBox NombreBaseDatos;
        private Label lblBuscar;
        private Label lblCantidad;
        private Button button1;
        private TextBox RutaArchivo;
    }
}
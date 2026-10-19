namespace Taller.Presentacion.Formularios.Backup
{
    partial class FrmBackup
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
            RutaArchivo = new TextBox();
            btn_generar = new Button();
            button2 = new Button();
            button1 = new Button();
            fecha_desde = new Label();
            NombreBaseDatos = new TextBox();
            lblBuscar = new Label();
            lblCantidad = new Label();
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
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 73F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 231F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 14F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tlpPrincipal.Size = new Size(700, 338);
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
            pnlCabecera.Margin = new Padding(0, 0, 0, 8);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Padding = new Padding(18, 9, 18, 8);
            pnlCabecera.Size = new Size(700, 65);
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
            btnNuevoUsuario.Location = new Point(1474, 29);
            btnNuevoUsuario.Margin = new Padding(3, 2, 3, 2);
            btnNuevoUsuario.Name = "btnNuevoUsuario";
            btnNuevoUsuario.Size = new Size(131, 30);
            btnNuevoUsuario.TabIndex = 0;
            btnNuevoUsuario.Text = "＋ Nuevo usuario";
            btnNuevoUsuario.UseVisualStyleBackColor = false;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(38, 40);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(263, 17);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Generá una copia de seguridad del sistema";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(33, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(274, 30);
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
            pnlFiltros.Location = new Point(0, 73);
            pnlFiltros.Margin = new Padding(0, 0, 0, 8);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(16, 8, 16, 8);
            pnlFiltros.Size = new Size(700, 223);
            pnlFiltros.TabIndex = 1;
            // 
            // RutaArchivo
            // 
            RutaArchivo.BorderStyle = BorderStyle.FixedSingle;
            RutaArchivo.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            RutaArchivo.Location = new Point(148, 112);
            RutaArchivo.Margin = new Padding(3, 2, 3, 2);
            RutaArchivo.Name = "RutaArchivo";
            RutaArchivo.Size = new Size(245, 25);
            RutaArchivo.TabIndex = 13;
            RutaArchivo.TextChanged += RutaArchivo_TextChanged;
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
            btn_generar.Location = new Point(186, 164);
            btn_generar.Margin = new Padding(3, 2, 3, 2);
            btn_generar.Name = "btn_generar";
            btn_generar.Size = new Size(135, 24);
            btn_generar.TabIndex = 12;
            btn_generar.Text = "Generar Respaldo";
            btn_generar.UseVisualStyleBackColor = true;
            btn_generar.Click += Btn_generar_Click;
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
            button2.Location = new Point(342, 164);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(135, 24);
            button2.TabIndex = 11;
            button2.Text = "Restaurar respaldo";
            button2.UseVisualStyleBackColor = true;
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
            button1.Location = new Point(410, 112);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(96, 24);
            button1.TabIndex = 10;
            button1.Text = "Buscar";
            button1.UseVisualStyleBackColor = true;
            // 
            // fecha_desde
            // 
            fecha_desde.AutoSize = true;
            fecha_desde.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fecha_desde.ForeColor = Color.FromArgb(51, 65, 85);
            fecha_desde.Location = new Point(148, 95);
            fecha_desde.Name = "fecha_desde";
            fecha_desde.Size = new Size(131, 15);
            fecha_desde.TabIndex = 4;
            fecha_desde.Text = "Ubicación del respaldo";
            // 
            // NombreBaseDatos
            // 
            NombreBaseDatos.BorderStyle = BorderStyle.FixedSingle;
            NombreBaseDatos.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NombreBaseDatos.Location = new Point(148, 55);
            NombreBaseDatos.Margin = new Padding(3, 2, 3, 2);
            NombreBaseDatos.Name = "NombreBaseDatos";
            NombreBaseDatos.Size = new Size(245, 25);
            NombreBaseDatos.TabIndex = 1;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBuscar.ForeColor = Color.FromArgb(51, 65, 85);
            lblBuscar.Location = new Point(148, 38);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(85, 15);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Base de Datos";
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.ForeColor = Color.FromArgb(100, 116, 139);
            lblCantidad.Location = new Point(4, 307);
            lblCantidad.Margin = new Padding(4, 3, 0, 0);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(696, 31);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "0 usuarios encontrados";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FrmBackup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmBackup";
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
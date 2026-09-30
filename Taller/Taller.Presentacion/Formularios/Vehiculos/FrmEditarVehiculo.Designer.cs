namespace Taller.Presentacion.Formularios.Vehiculos
{
    partial class FrmEditarVehiculo
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
            flpAcciones = new FlowLayoutPanel();
            btnGuardar = new Button();
            btnCancelar = new Button();
            pnlCabecera = new Panel();
            lblDescripcion = new Label();
            lblTitulo = new Label();
            pnlDatos = new Panel();
            tlpDatos = new TableLayoutPanel();
            textBox4 = new TextBox();
            label1 = new Label();
            textBox1 = new TextBox();
            lblId = new Label();
            lblNombreUsuario = new Label();
            txtId = new TextBox();
            txtNombreUsuario = new TextBox();
            txtFechaAlta = new TextBox();
            txtEstado = new TextBox();
            lblNombre = new Label();
            lblFechaAlta = new Label();
            txtApellido = new TextBox();
            textBox3 = new TextBox();
            lblApellido = new Label();
            lblEstado = new Label();
            lblRol = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            tlpPrincipal.SuspendLayout();
            flpAcciones.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlDatos.SuspendLayout();
            tlpDatos.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(flpAcciones, 0, 2);
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlDatos, 0, 1);
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(28, 22, 28, 22);
            tlpPrincipal.RowCount = 3;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 61F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpPrincipal.Size = new Size(544, 526);
            tlpPrincipal.TabIndex = 1;
            // 
            // flpAcciones
            // 
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.FlowDirection = FlowDirection.RightToLeft;
            flpAcciones.Location = new Point(28, 440);
            flpAcciones.Margin = new Padding(0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Padding = new Padding(0, 14, 0, 0);
            flpAcciones.Size = new Size(488, 64);
            flpAcciones.TabIndex = 0;
            flpAcciones.WrapContents = false;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(30, 64, 175);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 58, 138);
            btnGuardar.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(358, 14);
            btnGuardar.Margin = new Padding(8, 0, 0, 0);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(130, 38);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar cambios";
            btnGuardar.UseVisualStyleBackColor = false;
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
            btnCancelar.Location = new Point(230, 14);
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // pnlCabecera
            // 
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(28, 22);
            pnlCabecera.Margin = new Padding(0);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(488, 61);
            pnlCabecera.TabIndex = 0;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(2, 39);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(260, 17);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Modifique los datos del vehículo asignado.";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 17.25F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(174, 31);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Editar vehículo";
            // 
            // pnlDatos
            // 
            pnlDatos.BackColor = Color.White;
            pnlDatos.Controls.Add(tlpDatos);
            pnlDatos.Dock = DockStyle.Fill;
            pnlDatos.Location = new Point(28, 83);
            pnlDatos.Margin = new Padding(0);
            pnlDatos.Name = "pnlDatos";
            pnlDatos.Padding = new Padding(2);
            pnlDatos.Size = new Size(488, 357);
            pnlDatos.TabIndex = 1;
            // 
            // tlpDatos
            // 
            tlpDatos.ColumnCount = 2;
            tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDatos.Controls.Add(textBox4, 1, 5);
            tlpDatos.Controls.Add(label1, 0, 3);
            tlpDatos.Controls.Add(textBox1, 1, 4);
            tlpDatos.Controls.Add(lblId, 0, 0);
            tlpDatos.Controls.Add(lblNombreUsuario, 0, 1);
            tlpDatos.Controls.Add(txtId, 1, 0);
            tlpDatos.Controls.Add(txtNombreUsuario, 1, 1);
            tlpDatos.Controls.Add(txtFechaAlta, 1, 2);
            tlpDatos.Controls.Add(txtEstado, 1, 3);
            tlpDatos.Controls.Add(lblNombre, 0, 2);
            tlpDatos.Controls.Add(lblFechaAlta, 0, 5);
            tlpDatos.Controls.Add(txtApellido, 1, 7);
            tlpDatos.Controls.Add(textBox3, 1, 6);
            tlpDatos.Controls.Add(lblApellido, 0, 7);
            tlpDatos.Controls.Add(lblEstado, 0, 6);
            tlpDatos.Controls.Add(lblRol, 0, 4);
            tlpDatos.Controls.Add(label2, 0, 8);
            tlpDatos.Controls.Add(textBox2, 1, 8);
            tlpDatos.Dock = DockStyle.Fill;
            tlpDatos.Location = new Point(2, 2);
            tlpDatos.Margin = new Padding(0);
            tlpDatos.Name = "tlpDatos";
            tlpDatos.RowCount = 9;
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpDatos.Size = new Size(484, 353);
            tlpDatos.TabIndex = 0;
            // 
            // textBox4
            // 
            textBox4.Anchor = AnchorStyles.Left;
            textBox4.BackColor = Color.FromArgb(241, 245, 249);
            textBox4.BorderStyle = BorderStyle.FixedSingle;
            textBox4.Font = new Font("Segoe UI", 9.75F);
            textBox4.ForeColor = Color.FromArgb(71, 85, 105);
            textBox4.Location = new Point(150, 193);
            textBox4.Margin = new Padding(0, 8, 0, 8);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(320, 25);
            textBox4.TabIndex = 16;
            textBox4.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(51, 65, 85);
            label1.Location = new Point(4, 122);
            label1.Margin = new Padding(4, 3, 8, 3);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 14;
            label1.Text = "Año:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Left;
            textBox1.BackColor = Color.FromArgb(241, 245, 249);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 9.75F);
            textBox1.ForeColor = Color.FromArgb(71, 85, 105);
            textBox1.Location = new Point(150, 156);
            textBox1.Margin = new Padding(0, 8, 0, 8);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(320, 25);
            textBox1.TabIndex = 11;
            textBox1.TabStop = false;
            // 
            // lblId
            // 
            lblId.Anchor = AnchorStyles.Left;
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblId.ForeColor = Color.FromArgb(51, 65, 85);
            lblId.Location = new Point(4, 11);
            lblId.Margin = new Padding(4, 3, 8, 3);
            lblId.Name = "lblId";
            lblId.Size = new Size(23, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID:";
            lblId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.Anchor = AnchorStyles.Left;
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombreUsuario.ForeColor = Color.FromArgb(51, 65, 85);
            lblNombreUsuario.Location = new Point(4, 48);
            lblNombreUsuario.Margin = new Padding(4, 3, 8, 3);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(44, 15);
            lblNombreUsuario.TabIndex = 1;
            lblNombreUsuario.Text = "Marca:";
            lblNombreUsuario.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtId
            // 
            txtId.Anchor = AnchorStyles.Left;
            txtId.BackColor = Color.FromArgb(241, 245, 249);
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.Font = new Font("Segoe UI", 9.75F);
            txtId.ForeColor = Color.FromArgb(71, 85, 105);
            txtId.Location = new Point(150, 8);
            txtId.Margin = new Padding(0, 8, 0, 8);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(320, 25);
            txtId.TabIndex = 7;
            txtId.TabStop = false;
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Anchor = AnchorStyles.Left;
            txtNombreUsuario.BackColor = Color.FromArgb(241, 245, 249);
            txtNombreUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtNombreUsuario.Font = new Font("Segoe UI", 9.75F);
            txtNombreUsuario.ForeColor = Color.FromArgb(71, 85, 105);
            txtNombreUsuario.Location = new Point(150, 45);
            txtNombreUsuario.Margin = new Padding(0, 8, 0, 8);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.ReadOnly = true;
            txtNombreUsuario.Size = new Size(320, 25);
            txtNombreUsuario.TabIndex = 8;
            txtNombreUsuario.TabStop = false;
            // 
            // txtFechaAlta
            // 
            txtFechaAlta.Anchor = AnchorStyles.Left;
            txtFechaAlta.BackColor = Color.FromArgb(241, 245, 249);
            txtFechaAlta.BorderStyle = BorderStyle.FixedSingle;
            txtFechaAlta.Font = new Font("Segoe UI", 9.75F);
            txtFechaAlta.ForeColor = Color.FromArgb(71, 85, 105);
            txtFechaAlta.Location = new Point(150, 82);
            txtFechaAlta.Margin = new Padding(0, 8, 0, 8);
            txtFechaAlta.Name = "txtFechaAlta";
            txtFechaAlta.ReadOnly = true;
            txtFechaAlta.Size = new Size(320, 25);
            txtFechaAlta.TabIndex = 9;
            txtFechaAlta.TabStop = false;
            // 
            // txtEstado
            // 
            txtEstado.Anchor = AnchorStyles.Left;
            txtEstado.BackColor = Color.FromArgb(241, 245, 249);
            txtEstado.BorderStyle = BorderStyle.FixedSingle;
            txtEstado.Font = new Font("Segoe UI", 9.75F);
            txtEstado.ForeColor = Color.FromArgb(71, 85, 105);
            txtEstado.Location = new Point(150, 119);
            txtEstado.Margin = new Padding(0, 8, 0, 8);
            txtEstado.Name = "txtEstado";
            txtEstado.ReadOnly = true;
            txtEstado.Size = new Size(320, 25);
            txtEstado.TabIndex = 10;
            txtEstado.TabStop = false;
            // 
            // lblNombre
            // 
            lblNombre.Anchor = AnchorStyles.Left;
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(51, 65, 85);
            lblNombre.Location = new Point(4, 85);
            lblNombre.Margin = new Padding(4, 3, 8, 3);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(52, 15);
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Modelo:";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFechaAlta
            // 
            lblFechaAlta.Anchor = AnchorStyles.Left;
            lblFechaAlta.AutoSize = true;
            lblFechaAlta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFechaAlta.ForeColor = Color.FromArgb(51, 65, 85);
            lblFechaAlta.Location = new Point(4, 196);
            lblFechaAlta.Margin = new Padding(4, 3, 8, 3);
            lblFechaAlta.Name = "lblFechaAlta";
            lblFechaAlta.Size = new Size(82, 15);
            lblFechaAlta.TabIndex = 2;
            lblFechaAlta.Text = "Fecha de alta:";
            lblFechaAlta.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtApellido
            // 
            txtApellido.Anchor = AnchorStyles.Left;
            txtApellido.BackColor = Color.White;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Font = new Font("Segoe UI", 9.75F);
            txtApellido.ForeColor = Color.FromArgb(51, 65, 85);
            txtApellido.Location = new Point(150, 267);
            txtApellido.Margin = new Padding(0, 8, 0, 8);
            txtApellido.MaxLength = 100;
            txtApellido.Name = "txtApellido";
            txtApellido.PlaceholderText = "Dominio";
            txtApellido.Size = new Size(320, 25);
            txtApellido.TabIndex = 1;
            // 
            // textBox3
            // 
            textBox3.Anchor = AnchorStyles.Left;
            textBox3.BackColor = Color.FromArgb(241, 245, 249);
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.Font = new Font("Segoe UI", 9.75F);
            textBox3.ForeColor = Color.FromArgb(71, 85, 105);
            textBox3.Location = new Point(150, 230);
            textBox3.Margin = new Padding(0, 8, 0, 8);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(320, 25);
            textBox3.TabIndex = 13;
            textBox3.TabStop = false;
            // 
            // lblApellido
            // 
            lblApellido.Anchor = AnchorStyles.Left;
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblApellido.ForeColor = Color.FromArgb(51, 65, 85);
            lblApellido.Location = new Point(4, 270);
            lblApellido.Margin = new Padding(4, 3, 8, 3);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(57, 15);
            lblApellido.TabIndex = 5;
            lblApellido.Text = "Dominio:";
            lblApellido.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEstado
            // 
            lblEstado.Anchor = AnchorStyles.Left;
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEstado.ForeColor = Color.FromArgb(51, 65, 85);
            lblEstado.Location = new Point(4, 233);
            lblEstado.Margin = new Padding(4, 3, 8, 3);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(46, 15);
            lblEstado.TabIndex = 3;
            lblEstado.Text = "Estado:";
            lblEstado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRol
            // 
            lblRol.Anchor = AnchorStyles.Left;
            lblRol.AutoSize = true;
            lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRol.ForeColor = Color.FromArgb(51, 65, 85);
            lblRol.Location = new Point(4, 159);
            lblRol.Margin = new Padding(4, 3, 8, 3);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(39, 15);
            lblRol.TabIndex = 6;
            lblRol.Text = "Color:";
            lblRol.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(51, 65, 85);
            label2.Location = new Point(4, 317);
            label2.Margin = new Padding(4, 3, 8, 3);
            label2.Name = "label2";
            label2.Size = new Size(117, 15);
            label2.TabIndex = 15;
            label2.Text = "Nombre del cliente:";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Left;
            textBox2.BackColor = Color.White;
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Font = new Font("Segoe UI", 9.75F);
            textBox2.ForeColor = Color.FromArgb(51, 65, 85);
            textBox2.Location = new Point(150, 312);
            textBox2.Margin = new Padding(0, 8, 0, 8);
            textBox2.MaxLength = 100;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Nombre del cliente";
            textBox2.Size = new Size(320, 25);
            textBox2.TabIndex = 12;
            // 
            // FrmEditarVehiculo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 526);
            Controls.Add(tlpPrincipal);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmEditarVehiculo";
            Text = "Editar vehículo";
            tlpPrincipal.ResumeLayout(false);
            flpAcciones.ResumeLayout(false);
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlDatos.ResumeLayout(false);
            tlpDatos.ResumeLayout(false);
            tlpDatos.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private FlowLayoutPanel flpAcciones;
        private Button btnGuardar;
        private Button btnCancelar;
        private Panel pnlCabecera;
        private Label lblDescripcion;
        private Label lblTitulo;
        private Panel pnlDatos;
        private TableLayoutPanel tlpDatos;
        private Label lblId;
        private Label lblNombreUsuario;
        private Label lblFechaAlta;
        private Label lblEstado;
        private Label lblNombre;
        private Label lblApellido;
        private Label lblRol;
        private TextBox txtId;
        private TextBox txtNombreUsuario;
        private TextBox txtFechaAlta;
        private TextBox txtEstado;
        private TextBox txtApellido;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox4;
        private Label label2;
        private Label label1;
        private TextBox textBox3;
    }
}
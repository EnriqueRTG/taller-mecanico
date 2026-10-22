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
            txtFechaAltaVehiculo = new TextBox();
            label1 = new Label();
            txtColor = new TextBox();
            lblId = new Label();
            lblNombreUsuario = new Label();
            txtId = new TextBox();
            txtMarca = new TextBox();
            txtModelo = new TextBox();
            txtAnio = new TextBox();
            lblNombre = new Label();
            lblFechaAlta = new Label();
            txtDominio = new TextBox();
            txtEstadoVehiculo = new TextBox();
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
            tlpDatos.Controls.Add(txtFechaAltaVehiculo, 1, 5);
            tlpDatos.Controls.Add(label1, 0, 3);
            tlpDatos.Controls.Add(txtColor, 1, 4);
            tlpDatos.Controls.Add(lblId, 0, 0);
            tlpDatos.Controls.Add(lblNombreUsuario, 0, 1);
            tlpDatos.Controls.Add(txtId, 1, 0);
            tlpDatos.Controls.Add(txtMarca, 1, 1);
            tlpDatos.Controls.Add(txtModelo, 1, 2);
            tlpDatos.Controls.Add(txtAnio, 1, 3);
            tlpDatos.Controls.Add(lblNombre, 0, 2);
            tlpDatos.Controls.Add(lblFechaAlta, 0, 5);
            tlpDatos.Controls.Add(txtDominio, 1, 7);
            tlpDatos.Controls.Add(txtEstadoVehiculo, 1, 6);
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
            // txtFechaAltaVehiculo
            //
            txtFechaAltaVehiculo.Anchor = AnchorStyles.Left;
            txtFechaAltaVehiculo.BackColor = Color.FromArgb(241, 245, 249);
            txtFechaAltaVehiculo.BorderStyle = BorderStyle.FixedSingle;
            txtFechaAltaVehiculo.Font = new Font("Segoe UI", 9.75F);
            txtFechaAltaVehiculo.ForeColor = Color.FromArgb(71, 85, 105);
            txtFechaAltaVehiculo.Location = new Point(150, 193);
            txtFechaAltaVehiculo.Margin = new Padding(0, 8, 0, 8);
            txtFechaAltaVehiculo.Name = "txtFechaAltaVehiculo";
            txtFechaAltaVehiculo.ReadOnly = true;
            txtFechaAltaVehiculo.Size = new Size(320, 25);
            txtFechaAltaVehiculo.TabIndex = 16;
            txtFechaAltaVehiculo.TabStop = false;
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
            // txtColor
            //
            txtColor.Anchor = AnchorStyles.Left;
            txtColor.BackColor = Color.FromArgb(241, 245, 249);
            txtColor.BorderStyle = BorderStyle.FixedSingle;
            txtColor.Font = new Font("Segoe UI", 9.75F);
            txtColor.ForeColor = Color.FromArgb(71, 85, 105);
            txtColor.Location = new Point(150, 156);
            txtColor.Margin = new Padding(0, 8, 0, 8);
            txtColor.Name = "txtColor";
            txtColor.ReadOnly = true;
            txtColor.Size = new Size(320, 25);
            txtColor.TabIndex = 11;
            txtColor.TabStop = false;
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
            // txtMarca
            //
            txtMarca.Anchor = AnchorStyles.Left;
            txtMarca.BackColor = Color.FromArgb(241, 245, 249);
            txtMarca.BorderStyle = BorderStyle.FixedSingle;
            txtMarca.Font = new Font("Segoe UI", 9.75F);
            txtMarca.ForeColor = Color.FromArgb(71, 85, 105);
            txtMarca.Location = new Point(150, 45);
            txtMarca.Margin = new Padding(0, 8, 0, 8);
            txtMarca.Name = "txtMarca";
            txtMarca.ReadOnly = true;
            txtMarca.Size = new Size(320, 25);
            txtMarca.TabIndex = 8;
            txtMarca.TabStop = false;
            //
            // txtModelo
            //
            txtModelo.Anchor = AnchorStyles.Left;
            txtModelo.BackColor = Color.FromArgb(241, 245, 249);
            txtModelo.BorderStyle = BorderStyle.FixedSingle;
            txtModelo.Font = new Font("Segoe UI", 9.75F);
            txtModelo.ForeColor = Color.FromArgb(71, 85, 105);
            txtModelo.Location = new Point(150, 82);
            txtModelo.Margin = new Padding(0, 8, 0, 8);
            txtModelo.Name = "txtModelo";
            txtModelo.ReadOnly = true;
            txtModelo.Size = new Size(320, 25);
            txtModelo.TabIndex = 9;
            txtModelo.TabStop = false;
            //
            // txtAnio
            //
            txtAnio.Anchor = AnchorStyles.Left;
            txtAnio.BackColor = Color.FromArgb(241, 245, 249);
            txtAnio.BorderStyle = BorderStyle.FixedSingle;
            txtAnio.Font = new Font("Segoe UI", 9.75F);
            txtAnio.ForeColor = Color.FromArgb(71, 85, 105);
            txtAnio.Location = new Point(150, 119);
            txtAnio.Margin = new Padding(0, 8, 0, 8);
            txtAnio.Name = "txtAnio";
            txtAnio.ReadOnly = true;
            txtAnio.Size = new Size(320, 25);
            txtAnio.TabIndex = 10;
            txtAnio.TabStop = false;
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
            // txtDominio
            //
            txtDominio.Anchor = AnchorStyles.Left;
            txtDominio.BackColor = Color.White;
            txtDominio.BorderStyle = BorderStyle.FixedSingle;
            txtDominio.Font = new Font("Segoe UI", 9.75F);
            txtDominio.ForeColor = Color.FromArgb(51, 65, 85);
            txtDominio.Location = new Point(150, 267);
            txtDominio.Margin = new Padding(0, 8, 0, 8);
            txtDominio.MaxLength = 100;
            txtDominio.Name = "txtDominio";
            txtDominio.PlaceholderText = "Dominio";
            txtDominio.Size = new Size(320, 25);
            txtDominio.TabIndex = 1;
            //
            // txtEstadoVehiculo
            //
            txtEstadoVehiculo.Anchor = AnchorStyles.Left;
            txtEstadoVehiculo.BackColor = Color.FromArgb(241, 245, 249);
            txtEstadoVehiculo.BorderStyle = BorderStyle.FixedSingle;
            txtEstadoVehiculo.Font = new Font("Segoe UI", 9.75F);
            txtEstadoVehiculo.ForeColor = Color.FromArgb(71, 85, 105);
            txtEstadoVehiculo.Location = new Point(150, 230);
            txtEstadoVehiculo.Margin = new Padding(0, 8, 0, 8);
            txtEstadoVehiculo.Name = "txtEstadoVehiculo";
            txtEstadoVehiculo.ReadOnly = true;
            txtEstadoVehiculo.Size = new Size(320, 25);
            txtEstadoVehiculo.TabIndex = 13;
            txtEstadoVehiculo.TabStop = false;
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
            btnGuardar.DialogResult=DialogResult.None;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;
            FormClosing += Formulario_FormClosing;
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
        private TextBox txtMarca;
        private TextBox txtModelo;
        private TextBox txtAnio;
        private TextBox txtDominio;
        private TextBox txtColor;
        private TextBox textBox2;
        private TextBox txtFechaAltaVehiculo;
        private Label label2;
        private Label label1;
        private TextBox txtEstadoVehiculo;
    }
}

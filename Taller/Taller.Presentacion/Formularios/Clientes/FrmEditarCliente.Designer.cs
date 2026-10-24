namespace Taller.Presentacion.Formularios.Clientes
{
    partial class FrmEditarCliente
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
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtApellidoCliente = new TextBox();
            lblId = new Label();
            lblNombreUsuario = new Label();
            txtId = new TextBox();
            txtDocumento = new TextBox();
            txtTipoDocumento = new TextBox();
            txtNombreCliente = new TextBox();
            lblNombre = new Label();
            lblApellido = new Label();
            lblRol = new Label();
            txtTelefono = new TextBox();
            txtEmail = new TextBox();
            lblFechaAlta = new Label();
            lblEstado = new Label();
            txtFechaAltaCliente = new TextBox();
            txtEstadoCliente = new TextBox();
            txtDireccion = new TextBox();
            tlpPrincipal.SuspendLayout();
            flpAcciones.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlDatos.SuspendLayout();
            tlpDatos.SuspendLayout();
            SuspendLayout();
            //
            // tlpPrincipal
            //
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.BackColor = Color.FromArgb(245, 247, 250);
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(flpAcciones, 0, 2);
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlDatos, 0, 1);
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(32, 29, 32, 29);
            tlpPrincipal.RowCount = 3;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpPrincipal.Size = new Size(622, 701);
            tlpPrincipal.TabIndex = 1;
            //
            // flpAcciones
            //
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.FlowDirection = FlowDirection.RightToLeft;
            flpAcciones.Location = new Point(32, 585);
            flpAcciones.Margin = new Padding(0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Padding = new Padding(0, 8, 0, 0);
            flpAcciones.Size = new Size(558, 87);
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
            btnGuardar.Location = new Point(409, 19);
            btnGuardar.Margin = new Padding(9, 0, 0, 0);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(149, 38);
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
            btnCancelar.Location = new Point(263, 19);
            btnCancelar.Margin = new Padding(9, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(137, 38);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            //
            // pnlCabecera
            //
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(32, 29);
            pnlCabecera.Margin = new Padding(0);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(558, 100);
            pnlCabecera.TabIndex = 0;
            //
            // lblDescripcion
            //
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(2, 52);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(347, 21);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Modifique los datos personales y el rol asignado.";
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 17.25F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(196, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Editar cliente";
            //
            // pnlDatos
            //
            pnlDatos.BackColor = Color.White;
            pnlDatos.Controls.Add(tlpDatos);
            pnlDatos.Dock = DockStyle.Fill;
            pnlDatos.Location = new Point(32, 129);
            pnlDatos.Margin = new Padding(0);
            pnlDatos.Name = "pnlDatos";
            pnlDatos.Padding = new Padding(2, 3, 2, 3);
            pnlDatos.Size = new Size(558, 456);
            pnlDatos.TabIndex = 1;
            //
            // tlpDatos
            //
            tlpDatos.AutoScroll = false;
            tlpDatos.ColumnCount = 2;
            tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 171F));
            tlpDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDatos.Controls.Add(label3, 0, 7);
            tlpDatos.Controls.Add(label2, 0, 6);
            tlpDatos.Controls.Add(label1, 0, 5);
            tlpDatos.Controls.Add(txtApellidoCliente, 1, 4);
            tlpDatos.Controls.Add(lblId, 0, 0);
            tlpDatos.Controls.Add(lblNombreUsuario, 0, 1);
            tlpDatos.Controls.Add(txtId, 1, 0);
            tlpDatos.Controls.Add(txtDocumento, 1, 1);
            tlpDatos.Controls.Add(txtTipoDocumento, 1, 2);
            tlpDatos.Controls.Add(txtNombreCliente, 1, 3);
            tlpDatos.Controls.Add(lblNombre, 0, 2);
            tlpDatos.Controls.Add(lblApellido, 0, 3);
            tlpDatos.Controls.Add(lblRol, 0, 4);
            tlpDatos.Controls.Add(txtTelefono, 1, 5);
            tlpDatos.Controls.Add(txtEmail, 1, 6);
            tlpDatos.Controls.Add(lblFechaAlta, 0, 9);
            tlpDatos.Controls.Add(lblEstado, 0, 8);
            tlpDatos.Controls.Add(txtFechaAltaCliente, 1, 9);
            tlpDatos.Controls.Add(txtEstadoCliente, 1, 8);
            tlpDatos.Controls.Add(txtDireccion, 1, 7);
            tlpDatos.Dock = DockStyle.Top;
            tlpDatos.Location = new Point(2, 3);
            tlpDatos.Margin = new Padding(0);
            tlpDatos.Name = "tlpDatos";
            tlpDatos.RowCount = 10;
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatos.Size = new Size(554, 432);
            tlpDatos.TabIndex = 0;
            //
            // label3
            //
            label3.Anchor = AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(51, 65, 85);
            label3.Location = new Point(5, 470);
            label3.Margin = new Padding(5, 4, 9, 4);
            label3.Name = "label3";
            label3.Size = new Size(78, 20);
            label3.TabIndex = 17;
            label3.Text = "Dirección:";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            //
            // label2
            //
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(51, 65, 85);
            label2.Location = new Point(5, 406);
            label2.Margin = new Padding(5, 4, 9, 4);
            label2.Name = "label2";
            label2.Size = new Size(51, 20);
            label2.TabIndex = 15;
            label2.Text = "Email:";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            //
            // label1
            //
            label1.Anchor = AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(51, 65, 85);
            label1.Location = new Point(5, 342);
            label1.Margin = new Padding(5, 4, 9, 4);
            label1.Name = "label1";
            label1.Size = new Size(74, 20);
            label1.TabIndex = 14;
            label1.Text = "Teléfono:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtApellidoCliente
            //
            txtApellidoCliente.Anchor = AnchorStyles.Left;
            txtApellidoCliente.BackColor = Color.FromArgb(241, 245, 249);
            txtApellidoCliente.BorderStyle = BorderStyle.FixedSingle;
            txtApellidoCliente.Font = new Font("Segoe UI", 9.75F);
            txtApellidoCliente.ForeColor = Color.FromArgb(71, 85, 105);
            txtApellidoCliente.Location = new Point(171, 273);
            txtApellidoCliente.Margin = new Padding(0, 11, 0, 11);
            txtApellidoCliente.Name = "txtApellidoCliente";
            txtApellidoCliente.ReadOnly = true;
            txtApellidoCliente.Size = new Size(383, 29);
            txtApellidoCliente.TabIndex = 12;
            txtApellidoCliente.TabStop = false;
            //
            // lblId
            //
            lblId.Anchor = AnchorStyles.Left;
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblId.ForeColor = Color.FromArgb(51, 65, 85);
            lblId.Location = new Point(5, 22);
            lblId.Margin = new Padding(5, 4, 9, 4);
            lblId.Name = "lblId";
            lblId.Size = new Size(29, 20);
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
            lblNombreUsuario.Location = new Point(5, 86);
            lblNombreUsuario.Margin = new Padding(5, 4, 9, 4);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(95, 20);
            lblNombreUsuario.TabIndex = 1;
            lblNombreUsuario.Text = "Documento:";
            lblNombreUsuario.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtId
            //
            txtId.Anchor = AnchorStyles.Left;
            txtId.BackColor = Color.FromArgb(241, 245, 249);
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.Font = new Font("Segoe UI", 9.75F);
            txtId.ForeColor = Color.FromArgb(71, 85, 105);
            txtId.Location = new Point(171, 17);
            txtId.Margin = new Padding(0, 11, 0, 11);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(383, 29);
            txtId.TabIndex = 7;
            txtId.TabStop = false;
            //
            // txtDocumento
            //
            txtDocumento.Anchor = AnchorStyles.Left;
            txtDocumento.BackColor = Color.FromArgb(241, 245, 249);
            txtDocumento.BorderStyle = BorderStyle.FixedSingle;
            txtDocumento.Font = new Font("Segoe UI", 9.75F);
            txtDocumento.ForeColor = Color.FromArgb(71, 85, 105);
            txtDocumento.Location = new Point(171, 81);
            txtDocumento.Margin = new Padding(0, 11, 0, 11);
            txtDocumento.Name = "txtDocumento";
            txtDocumento.ReadOnly = true;
            txtDocumento.Size = new Size(383, 29);
            txtDocumento.TabIndex = 8;
            txtDocumento.TabStop = false;
            //
            // txtTipoDocumento
            //
            txtTipoDocumento.Anchor = AnchorStyles.Left;
            txtTipoDocumento.BackColor = Color.FromArgb(241, 245, 249);
            txtTipoDocumento.BorderStyle = BorderStyle.FixedSingle;
            txtTipoDocumento.Font = new Font("Segoe UI", 9.75F);
            txtTipoDocumento.ForeColor = Color.FromArgb(71, 85, 105);
            txtTipoDocumento.Location = new Point(171, 145);
            txtTipoDocumento.Margin = new Padding(0, 11, 0, 11);
            txtTipoDocumento.Name = "txtTipoDocumento";
            txtTipoDocumento.ReadOnly = true;
            txtTipoDocumento.Size = new Size(383, 29);
            txtTipoDocumento.TabIndex = 9;
            txtTipoDocumento.TabStop = false;
            //
            // txtNombreCliente
            //
            txtNombreCliente.Anchor = AnchorStyles.Left;
            txtNombreCliente.BackColor = Color.FromArgb(241, 245, 249);
            txtNombreCliente.BorderStyle = BorderStyle.FixedSingle;
            txtNombreCliente.Font = new Font("Segoe UI", 9.75F);
            txtNombreCliente.ForeColor = Color.FromArgb(71, 85, 105);
            txtNombreCliente.Location = new Point(171, 209);
            txtNombreCliente.Margin = new Padding(0, 11, 0, 11);
            txtNombreCliente.Name = "txtNombreCliente";
            txtNombreCliente.ReadOnly = true;
            txtNombreCliente.Size = new Size(383, 29);
            txtNombreCliente.TabIndex = 10;
            txtNombreCliente.TabStop = false;
            //
            // lblNombre
            //
            lblNombre.Anchor = AnchorStyles.Left;
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(51, 65, 85);
            lblNombre.Location = new Point(5, 150);
            lblNombre.Margin = new Padding(5, 4, 9, 4);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(128, 20);
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Tipo documento:";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblApellido
            //
            lblApellido.Anchor = AnchorStyles.Left;
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblApellido.ForeColor = Color.FromArgb(51, 65, 85);
            lblApellido.Location = new Point(5, 214);
            lblApellido.Margin = new Padding(5, 4, 9, 4);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(71, 20);
            lblApellido.TabIndex = 5;
            lblApellido.Text = "Nombre:";
            lblApellido.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblRol
            //
            lblRol.Anchor = AnchorStyles.Left;
            lblRol.AutoSize = true;
            lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRol.ForeColor = Color.FromArgb(51, 65, 85);
            lblRol.Location = new Point(5, 278);
            lblRol.Margin = new Padding(5, 4, 9, 4);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(71, 20);
            lblRol.TabIndex = 6;
            lblRol.Text = "Apellido:";
            lblRol.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtTelefono
            //
            txtTelefono.Anchor = AnchorStyles.Left;
            txtTelefono.BackColor = Color.White;
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Font = new Font("Segoe UI", 9.75F);
            txtTelefono.ForeColor = Color.FromArgb(51, 65, 85);
            txtTelefono.Location = new Point(171, 337);
            txtTelefono.Margin = new Padding(0, 11, 0, 11);
            txtTelefono.MaxLength = 100;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.PlaceholderText = "teléfono";
            txtTelefono.Size = new Size(383, 29);
            txtTelefono.TabIndex = 1;
            //
            // txtEmail
            //
            txtEmail.Anchor = AnchorStyles.Left;
            txtEmail.BackColor = Color.White;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 9.75F);
            txtEmail.ForeColor = Color.FromArgb(51, 65, 85);
            txtEmail.Location = new Point(171, 401);
            txtEmail.Margin = new Padding(0, 11, 0, 11);
            txtEmail.MaxLength = 100;
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "email";
            txtEmail.Size = new Size(383, 29);
            txtEmail.TabIndex = 0;
            //
            // lblFechaAlta
            //
            lblFechaAlta.Anchor = AnchorStyles.Left;
            lblFechaAlta.AutoSize = true;
            lblFechaAlta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFechaAlta.ForeColor = Color.FromArgb(51, 65, 85);
            lblFechaAlta.Location = new Point(5, 598);
            lblFechaAlta.Margin = new Padding(5, 4, 9, 4);
            lblFechaAlta.Name = "lblFechaAlta";
            lblFechaAlta.Size = new Size(104, 20);
            lblFechaAlta.TabIndex = 2;
            lblFechaAlta.Text = "Fecha de alta:";
            lblFechaAlta.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblEstado
            //
            lblEstado.Anchor = AnchorStyles.Left;
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEstado.ForeColor = Color.FromArgb(51, 65, 85);
            lblEstado.Location = new Point(5, 534);
            lblEstado.Margin = new Padding(5, 4, 9, 4);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(60, 20);
            lblEstado.TabIndex = 3;
            lblEstado.Text = "Estado:";
            lblEstado.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtFechaAltaCliente
            //
            txtFechaAltaCliente.Anchor = AnchorStyles.Left;
            txtFechaAltaCliente.BackColor = Color.FromArgb(241, 245, 249);
            txtFechaAltaCliente.BorderStyle = BorderStyle.FixedSingle;
            txtFechaAltaCliente.Font = new Font("Segoe UI", 9.75F);
            txtFechaAltaCliente.ForeColor = Color.FromArgb(71, 85, 105);
            txtFechaAltaCliente.Location = new Point(171, 593);
            txtFechaAltaCliente.Margin = new Padding(0, 11, 0, 11);
            txtFechaAltaCliente.Name = "txtFechaAltaCliente";
            txtFechaAltaCliente.ReadOnly = true;
            txtFechaAltaCliente.Size = new Size(383, 29);
            txtFechaAltaCliente.TabIndex = 11;
            txtFechaAltaCliente.TabStop = false;
            //
            // txtEstadoCliente
            //
            txtEstadoCliente.Anchor = AnchorStyles.Left;
            txtEstadoCliente.BackColor = Color.FromArgb(241, 245, 249);
            txtEstadoCliente.BorderStyle = BorderStyle.FixedSingle;
            txtEstadoCliente.Font = new Font("Segoe UI", 9.75F);
            txtEstadoCliente.ForeColor = Color.FromArgb(71, 85, 105);
            txtEstadoCliente.Location = new Point(171, 529);
            txtEstadoCliente.Margin = new Padding(0, 11, 0, 11);
            txtEstadoCliente.Name = "txtEstadoCliente";
            txtEstadoCliente.ReadOnly = true;
            txtEstadoCliente.Size = new Size(383, 29);
            txtEstadoCliente.TabIndex = 13;
            txtEstadoCliente.TabStop = false;
            //
            // txtDireccion
            //
            txtDireccion.Anchor = AnchorStyles.Left;
            txtDireccion.BackColor = Color.White;
            txtDireccion.BorderStyle = BorderStyle.FixedSingle;
            txtDireccion.Font = new Font("Segoe UI", 9.75F);
            txtDireccion.ForeColor = Color.FromArgb(51, 65, 85);
            txtDireccion.Location = new Point(171, 465);
            txtDireccion.Margin = new Padding(0, 11, 0, 11);
            txtDireccion.MaxLength = 100;
            txtDireccion.Name = "txtDireccion";
            txtDireccion.PlaceholderText = "dirección";
            txtDireccion.Size = new Size(383, 29);
            txtDireccion.TabIndex = 16;
            //
            // FrmEditarCliente
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 680);
            MinimumSize = new Size(600, 460);
            lblId.Visible = txtId.Visible = false;
            pnlDatos.AutoScroll = true;
            tlpDatos.MinimumSize = new Size(0, 432);
            Controls.Add(tlpPrincipal);
            Name = "FrmEditarCliente";
            ShowIcon = false;
            Text = "Editar Cliente";
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
            Shown += Formulario_Shown;
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
        private TextBox txtDocumento;
        private TextBox txtTipoDocumento;
        private TextBox txtNombreCliente;
        private TextBox txtEmail;
        private TextBox txtTelefono;
        private TextBox txtFechaAltaCliente;
        private TextBox txtEstadoCliente;
        private TextBox txtApellidoCliente;
        private Label label2;
        private Label label1;
        private Label label3;
        private TextBox txtDireccion;
    }
}

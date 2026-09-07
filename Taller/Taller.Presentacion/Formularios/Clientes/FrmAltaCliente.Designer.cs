namespace Taller.Presentacion.Formularios.Clientes
{
    partial class FrmAltaCliente
    {
        /// <summary>
        /// Variable necesaria para el diseñador.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Libera los recursos utilizados.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            grpDatosCliente = new GroupBox();
            lblTipoDocumento = new Label();
            cmbTipoDocumento = new ComboBox();
            lblDocumento = new Label();
            txtDocumento = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();

            grpDatosCliente.SuspendLayout();
            SuspendLayout();

            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(172, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Alta de cliente";

            // 
            // grpDatosCliente
            // 
            grpDatosCliente.Controls.Add(lblTipoDocumento);
            grpDatosCliente.Controls.Add(cmbTipoDocumento);
            grpDatosCliente.Controls.Add(lblDocumento);
            grpDatosCliente.Controls.Add(txtDocumento);
            grpDatosCliente.Controls.Add(lblNombre);
            grpDatosCliente.Controls.Add(txtNombre);
            grpDatosCliente.Controls.Add(lblApellido);
            grpDatosCliente.Controls.Add(txtApellido);
            grpDatosCliente.Controls.Add(lblTelefono);
            grpDatosCliente.Controls.Add(txtTelefono);
            grpDatosCliente.Controls.Add(lblEmail);
            grpDatosCliente.Controls.Add(txtEmail);
            grpDatosCliente.Controls.Add(lblDireccion);
            grpDatosCliente.Controls.Add(txtDireccion);
            grpDatosCliente.Location = new Point(24, 68);
            grpDatosCliente.Name = "grpDatosCliente";
            grpDatosCliente.Size = new Size(500, 345);
            grpDatosCliente.TabIndex = 1;
            grpDatosCliente.TabStop = false;
            grpDatosCliente.Text = "Datos del cliente";

            // 
            // lblTipoDocumento
            // 
            lblTipoDocumento.AutoSize = true;
            lblTipoDocumento.Location = new Point(24, 35);
            lblTipoDocumento.Name = "lblTipoDocumento";
            lblTipoDocumento.Size = new Size(112, 15);
            lblTipoDocumento.TabIndex = 0;
            lblTipoDocumento.Text = "Tipo de documento:";

            // 
            // cmbTipoDocumento
            // 
            cmbTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoDocumento.FormattingEnabled = true;
            cmbTipoDocumento.Location = new Point(170, 31);
            cmbTipoDocumento.Name = "cmbTipoDocumento";
            cmbTipoDocumento.Size = new Size(280, 23);
            cmbTipoDocumento.TabIndex = 1;

            // 
            // lblDocumento
            // 
            lblDocumento.AutoSize = true;
            lblDocumento.Location = new Point(24, 77);
            lblDocumento.Name = "lblDocumento";
            lblDocumento.Size = new Size(73, 15);
            lblDocumento.TabIndex = 2;
            lblDocumento.Text = "Documento:";

            // 
            // txtDocumento
            // 
            txtDocumento.Location = new Point(170, 73);
            txtDocumento.MaxLength = 20;
            txtDocumento.Name = "txtDocumento";
            txtDocumento.Size = new Size(280, 23);
            txtDocumento.TabIndex = 3;

            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(24, 119);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(54, 15);
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Nombre:";

            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(170, 115);
            txtNombre.MaxLength = 80;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(280, 23);
            txtNombre.TabIndex = 5;

            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(24, 161);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(54, 15);
            lblApellido.TabIndex = 6;
            lblApellido.Text = "Apellido:";

            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(170, 157);
            txtApellido.MaxLength = 80;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(280, 23);
            txtApellido.TabIndex = 7;

            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(24, 203);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(55, 15);
            lblTelefono.TabIndex = 8;
            lblTelefono.Text = "Teléfono:";

            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(170, 199);
            txtTelefono.MaxLength = 30;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(280, 23);
            txtTelefono.TabIndex = 9;

            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(24, 245);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 10;
            lblEmail.Text = "Email:";

            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(170, 241);
            txtEmail.MaxLength = 120;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(280, 23);
            txtEmail.TabIndex = 11;

            // 
            // lblDireccion
            // 
            lblDireccion.AutoSize = true;
            lblDireccion.Location = new Point(24, 287);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(60, 15);
            lblDireccion.TabIndex = 12;
            lblDireccion.Text = "Dirección:";

            // 
            // txtDireccion
            // 
            txtDireccion.Location = new Point(170, 283);
            txtDireccion.MaxLength = 200;
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(280, 23);
            txtDireccion.TabIndex = 13;

            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(328, 431);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(95, 34);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;

            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(429, 431);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(95, 34);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;

            // 
            // FrmAltaCliente
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(550, 490);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(grpDatosCliente);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAltaCliente";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Alta de cliente";
            Load += FrmAltaCliente_Load;

            grpDatosCliente.ResumeLayout(false);
            grpDatosCliente.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private GroupBox grpDatosCliente;
        private Label lblTipoDocumento;
        private ComboBox cmbTipoDocumento;
        private Label lblDocumento;
        private TextBox txtDocumento;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
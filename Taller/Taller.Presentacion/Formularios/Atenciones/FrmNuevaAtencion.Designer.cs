namespace Taller.Presentacion.Formularios.Atenciones
{
    partial class FrmNuevaAtencion
    {
        private System.ComponentModel.IContainer components = null;

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

            grpRecepcion = new GroupBox();
            lblUsuarioTitulo = new Label();
            lblUsuarioRecepcion = new Label();

            grpCliente = new GroupBox();
            lblTipoDocumento = new Label();
            cmbTipoDocumento = new ComboBox();
            lblDocumentoCliente = new Label();
            txtDocumentoCliente = new TextBox();
            btnBuscarCliente = new Button();
            btnNuevoCliente = new Button();
            lblClienteTitulo = new Label();
            lblClienteSeleccionado = new Label();

            grpVehiculo = new GroupBox();
            lblDominio = new Label();
            txtDominio = new TextBox();
            btnBuscarVehiculo = new Button();
            btnNuevoVehiculo = new Button();
            lblVehiculoTitulo = new Label();
            lblVehiculoSeleccionado = new Label();

            grpMotivo = new GroupBox();
            txtMotivoConsulta = new TextBox();

            btnRegistrar = new Button();
            btnCancelar = new Button();

            grpRecepcion.SuspendLayout();
            grpCliente.SuspendLayout();
            grpVehiculo.SuspendLayout();
            grpMotivo.SuspendLayout();

            SuspendLayout();

            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(191, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nueva atención";

            // 
            // grpRecepcion
            // 
            grpRecepcion.Controls.Add(lblUsuarioTitulo);
            grpRecepcion.Controls.Add(lblUsuarioRecepcion);
            grpRecepcion.Location = new Point(24, 65);
            grpRecepcion.Name = "grpRecepcion";
            grpRecepcion.Size = new Size(650, 70);
            grpRecepcion.TabIndex = 1;
            grpRecepcion.TabStop = false;
            grpRecepcion.Text = "Recepción";

            // 
            // lblUsuarioTitulo
            // 
            lblUsuarioTitulo.AutoSize = true;
            lblUsuarioTitulo.Location = new Point(20, 31);
            lblUsuarioTitulo.Name = "lblUsuarioTitulo";
            lblUsuarioTitulo.Size = new Size(110, 15);
            lblUsuarioTitulo.TabIndex = 0;
            lblUsuarioTitulo.Text = "Usuario recepción:";

            // 
            // lblUsuarioRecepcion
            // 
            lblUsuarioRecepcion.AutoSize = true;
            lblUsuarioRecepcion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUsuarioRecepcion.Location = new Point(145, 31);
            lblUsuarioRecepcion.Name = "lblUsuarioRecepcion";
            lblUsuarioRecepcion.Size = new Size(128, 15);
            lblUsuarioRecepcion.TabIndex = 1;
            lblUsuarioRecepcion.Text = "Usuario no autenticado";

            // 
            // grpCliente
            // 
            grpCliente.Controls.Add(lblTipoDocumento);
            grpCliente.Controls.Add(cmbTipoDocumento);
            grpCliente.Controls.Add(lblDocumentoCliente);
            grpCliente.Controls.Add(txtDocumentoCliente);
            grpCliente.Controls.Add(btnBuscarCliente);
            grpCliente.Controls.Add(btnNuevoCliente);
            grpCliente.Controls.Add(lblClienteTitulo);
            grpCliente.Controls.Add(lblClienteSeleccionado);
            grpCliente.Location = new Point(24, 150);
            grpCliente.Name = "grpCliente";
            grpCliente.Size = new Size(650, 155);
            grpCliente.TabIndex = 2;
            grpCliente.TabStop = false;
            grpCliente.Text = "Cliente";

            // 
            // lblTipoDocumento
            // 
            lblTipoDocumento.AutoSize = true;
            lblTipoDocumento.Location = new Point(20, 32);
            lblTipoDocumento.Name = "lblTipoDocumento";
            lblTipoDocumento.Size = new Size(112, 15);
            lblTipoDocumento.TabIndex = 0;
            lblTipoDocumento.Text = "Tipo de documento:";

            // 
            // cmbTipoDocumento
            // 
            cmbTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoDocumento.FormattingEnabled = true;
            cmbTipoDocumento.Location = new Point(145, 28);
            cmbTipoDocumento.Name = "cmbTipoDocumento";
            cmbTipoDocumento.Size = new Size(170, 23);
            cmbTipoDocumento.TabIndex = 1;

            // 
            // lblDocumentoCliente
            // 
            lblDocumentoCliente.AutoSize = true;
            lblDocumentoCliente.Location = new Point(20, 71);
            lblDocumentoCliente.Name = "lblDocumentoCliente";
            lblDocumentoCliente.Size = new Size(73, 15);
            lblDocumentoCliente.TabIndex = 2;
            lblDocumentoCliente.Text = "Documento:";

            // 
            // txtDocumentoCliente
            // 
            txtDocumentoCliente.Location = new Point(145, 67);
            txtDocumentoCliente.Name = "txtDocumentoCliente";
            txtDocumentoCliente.Size = new Size(170, 23);
            txtDocumentoCliente.TabIndex = 3;

            // 
            // btnBuscarCliente
            // 
            btnBuscarCliente.Location = new Point(330, 66);
            btnBuscarCliente.Name = "btnBuscarCliente";
            btnBuscarCliente.Size = new Size(90, 25);
            btnBuscarCliente.TabIndex = 4;
            btnBuscarCliente.Text = "Buscar";
            btnBuscarCliente.UseVisualStyleBackColor = true;
            btnBuscarCliente.Click += btnBuscarCliente_Click;

            // 
            // btnNuevoCliente
            // 
            btnNuevoCliente.Enabled = false;
            btnNuevoCliente.Location = new Point(430, 66);
            btnNuevoCliente.Name = "btnNuevoCliente";
            btnNuevoCliente.Size = new Size(130, 25);
            btnNuevoCliente.TabIndex = 5;
            btnNuevoCliente.Text = "Nuevo cliente";
            btnNuevoCliente.UseVisualStyleBackColor = true;
            btnNuevoCliente.Click += btnNuevoCliente_Click;

            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.AutoSize = true;
            lblClienteTitulo.Location = new Point(20, 117);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.Size = new Size(118, 15);
            lblClienteTitulo.TabIndex = 6;
            lblClienteTitulo.Text = "Cliente seleccionado:";

            // 
            // lblClienteSeleccionado
            // 
            lblClienteSeleccionado.AutoSize = true;
            lblClienteSeleccionado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteSeleccionado.Location = new Point(145, 117);
            lblClienteSeleccionado.Name = "lblClienteSeleccionado";
            lblClienteSeleccionado.Size = new Size(178, 15);
            lblClienteSeleccionado.TabIndex = 7;
            lblClienteSeleccionado.Text = "Ningún cliente seleccionado.";

            // 
            // grpVehiculo
            // 
            grpVehiculo.Controls.Add(lblDominio);
            grpVehiculo.Controls.Add(txtDominio);
            grpVehiculo.Controls.Add(btnBuscarVehiculo);
            grpVehiculo.Controls.Add(btnNuevoVehiculo);
            grpVehiculo.Controls.Add(lblVehiculoTitulo);
            grpVehiculo.Controls.Add(lblVehiculoSeleccionado);
            grpVehiculo.Location = new Point(24, 320);
            grpVehiculo.Name = "grpVehiculo";
            grpVehiculo.Size = new Size(650, 125);
            grpVehiculo.TabIndex = 3;
            grpVehiculo.TabStop = false;
            grpVehiculo.Text = "Vehículo";

            // 
            // lblDominio
            // 
            lblDominio.AutoSize = true;
            lblDominio.Location = new Point(20, 34);
            lblDominio.Name = "lblDominio";
            lblDominio.Size = new Size(58, 15);
            lblDominio.TabIndex = 0;
            lblDominio.Text = "Dominio:";

            // 
            // txtDominio
            // 
            txtDominio.Location = new Point(145, 30);
            txtDominio.Name = "txtDominio";
            txtDominio.Size = new Size(170, 23);
            txtDominio.TabIndex = 1;

            // 
            // btnBuscarVehiculo
            // 
            btnBuscarVehiculo.Location = new Point(330, 29);
            btnBuscarVehiculo.Name = "btnBuscarVehiculo";
            btnBuscarVehiculo.Size = new Size(90, 25);
            btnBuscarVehiculo.TabIndex = 2;
            btnBuscarVehiculo.Text = "Buscar";
            btnBuscarVehiculo.UseVisualStyleBackColor = true;
            btnBuscarVehiculo.Click += btnBuscarVehiculo_Click;

            // 
            // btnNuevoVehiculo
            // 
            btnNuevoVehiculo.Enabled = false;
            btnNuevoVehiculo.Location = new Point(430, 29);
            btnNuevoVehiculo.Name = "btnNuevoVehiculo";
            btnNuevoVehiculo.Size = new Size(130, 25);
            btnNuevoVehiculo.TabIndex = 3;
            btnNuevoVehiculo.Text = "Nuevo vehículo";
            btnNuevoVehiculo.UseVisualStyleBackColor = true;
            btnNuevoVehiculo.Click += btnNuevoVehiculo_Click;

            // 
            // lblVehiculoTitulo
            // 
            lblVehiculoTitulo.AutoSize = true;
            lblVehiculoTitulo.Location = new Point(20, 82);
            lblVehiculoTitulo.Name = "lblVehiculoTitulo";
            lblVehiculoTitulo.Size = new Size(126, 15);
            lblVehiculoTitulo.TabIndex = 4;
            lblVehiculoTitulo.Text = "Vehículo seleccionado:";

            // 
            // lblVehiculoSeleccionado
            // 
            lblVehiculoSeleccionado.AutoSize = true;
            lblVehiculoSeleccionado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVehiculoSeleccionado.Location = new Point(155, 82);
            lblVehiculoSeleccionado.Name = "lblVehiculoSeleccionado";
            lblVehiculoSeleccionado.Size = new Size(190, 15);
            lblVehiculoSeleccionado.TabIndex = 5;
            lblVehiculoSeleccionado.Text = "Ningún vehículo seleccionado.";

            // 
            // grpMotivo
            // 
            grpMotivo.Controls.Add(txtMotivoConsulta);
            grpMotivo.Location = new Point(24, 460);
            grpMotivo.Name = "grpMotivo";
            grpMotivo.Size = new Size(650, 150);
            grpMotivo.TabIndex = 4;
            grpMotivo.TabStop = false;
            grpMotivo.Text = "Motivo de consulta";

            // 
            // txtMotivoConsulta
            // 
            txtMotivoConsulta.Location = new Point(20, 27);
            txtMotivoConsulta.Multiline = true;
            txtMotivoConsulta.Name = "txtMotivoConsulta";
            txtMotivoConsulta.ScrollBars = ScrollBars.Vertical;
            txtMotivoConsulta.Size = new Size(610, 100);
            txtMotivoConsulta.TabIndex = 0;

            // 
            // btnRegistrar
            // 
            btnRegistrar.Location = new Point(474, 628);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(95, 34);
            btnRegistrar.TabIndex = 5;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(579, 628);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(95, 34);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;

            // 
            // FrmNuevaAtencion
            // 
            AcceptButton = btnRegistrar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(700, 685);

            Controls.Add(btnCancelar);
            Controls.Add(btnRegistrar);
            Controls.Add(grpMotivo);
            Controls.Add(grpVehiculo);
            Controls.Add(grpCliente);
            Controls.Add(grpRecepcion);
            Controls.Add(lblTitulo);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            Name = "FrmNuevaAtencion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Nueva atención";

            Load += FrmNuevaAtencion_Load;

            grpRecepcion.ResumeLayout(false);
            grpRecepcion.PerformLayout();

            grpCliente.ResumeLayout(false);
            grpCliente.PerformLayout();

            grpVehiculo.ResumeLayout(false);
            grpVehiculo.PerformLayout();

            grpMotivo.ResumeLayout(false);
            grpMotivo.PerformLayout();

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;

        private GroupBox grpRecepcion;
        private Label lblUsuarioTitulo;
        private Label lblUsuarioRecepcion;

        private GroupBox grpCliente;
        private Label lblTipoDocumento;
        private ComboBox cmbTipoDocumento;
        private Label lblDocumentoCliente;
        private TextBox txtDocumentoCliente;
        private Button btnBuscarCliente;
        private Button btnNuevoCliente;
        private Label lblClienteTitulo;
        private Label lblClienteSeleccionado;

        private GroupBox grpVehiculo;
        private Label lblDominio;
        private TextBox txtDominio;
        private Button btnBuscarVehiculo;
        private Button btnNuevoVehiculo;
        private Label lblVehiculoTitulo;
        private Label lblVehiculoSeleccionado;

        private GroupBox grpMotivo;
        private TextBox txtMotivoConsulta;

        private Button btnRegistrar;
        private Button btnCancelar;
    }
}
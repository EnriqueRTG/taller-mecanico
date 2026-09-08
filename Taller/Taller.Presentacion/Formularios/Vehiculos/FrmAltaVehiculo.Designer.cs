namespace Taller.Presentacion.Formularios.Vehiculos
{
    partial class FrmAltaVehiculo
    {
        /// <summary>
        /// Variable necesaria para el diseñador.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpia los recursos utilizados.
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

        /// <summary>
        /// Método necesario para admitir el Diseñador.
        /// No modificar el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblDominio = new Label();
            txtDominio = new TextBox();
            lblMarca = new Label();
            cmbMarca = new ComboBox();
            lblModelo = new Label();
            cmbModelo = new ComboBox();
            lblAnio = new Label();
            nudAnio = new NumericUpDown();
            lblColor = new Label();
            txtColor = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            grpDatosVehiculo = new GroupBox();

            ((System.ComponentModel.ISupportInitialize)nudAnio).BeginInit();
            grpDatosVehiculo.SuspendLayout();
            SuspendLayout();

            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(196, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Alta de vehículo";

            // 
            // grpDatosVehiculo
            // 
            grpDatosVehiculo.Controls.Add(lblDominio);
            grpDatosVehiculo.Controls.Add(txtDominio);
            grpDatosVehiculo.Controls.Add(lblMarca);
            grpDatosVehiculo.Controls.Add(cmbMarca);
            grpDatosVehiculo.Controls.Add(lblModelo);
            grpDatosVehiculo.Controls.Add(cmbModelo);
            grpDatosVehiculo.Controls.Add(lblAnio);
            grpDatosVehiculo.Controls.Add(nudAnio);
            grpDatosVehiculo.Controls.Add(lblColor);
            grpDatosVehiculo.Controls.Add(txtColor);
            grpDatosVehiculo.Location = new Point(24, 68);
            grpDatosVehiculo.Name = "grpDatosVehiculo";
            grpDatosVehiculo.Size = new Size(450, 245);
            grpDatosVehiculo.TabIndex = 1;
            grpDatosVehiculo.TabStop = false;
            grpDatosVehiculo.Text = "Datos del vehículo";

            // 
            // lblDominio
            // 
            lblDominio.AutoSize = true;
            lblDominio.Location = new Point(24, 34);
            lblDominio.Name = "lblDominio";
            lblDominio.Size = new Size(58, 15);
            lblDominio.TabIndex = 0;
            lblDominio.Text = "Dominio:";

            // 
            // txtDominio
            // 
            txtDominio.Location = new Point(140, 30);
            txtDominio.MaxLength = 10;
            txtDominio.Name = "txtDominio";
            txtDominio.Size = new Size(250, 23);
            txtDominio.TabIndex = 1;

            // 
            // lblMarca
            // 
            lblMarca.AutoSize = true;
            lblMarca.Location = new Point(24, 76);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(43, 15);
            lblMarca.TabIndex = 2;
            lblMarca.Text = "Marca:";

            // 
            // cmbMarca
            // 
            cmbMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMarca.FormattingEnabled = true;
            cmbMarca.Location = new Point(140, 72);
            cmbMarca.Name = "cmbMarca";
            cmbMarca.Size = new Size(250, 23);
            cmbMarca.TabIndex = 3;
            cmbMarca.SelectedIndexChanged += cmbMarca_SelectedIndexChanged;

            // 
            // lblModelo
            // 
            lblModelo.AutoSize = true;
            lblModelo.Location = new Point(24, 118);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(50, 15);
            lblModelo.TabIndex = 4;
            lblModelo.Text = "Modelo:";

            // 
            // cmbModelo
            // 
            cmbModelo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelo.Enabled = false;
            cmbModelo.FormattingEnabled = true;
            cmbModelo.Location = new Point(140, 114);
            cmbModelo.Name = "cmbModelo";
            cmbModelo.Size = new Size(250, 23);
            cmbModelo.TabIndex = 5;

            // 
            // lblAnio
            // 
            lblAnio.AutoSize = true;
            lblAnio.Location = new Point(24, 160);
            lblAnio.Name = "lblAnio";
            lblAnio.Size = new Size(32, 15);
            lblAnio.TabIndex = 6;
            lblAnio.Text = "Año:";

            // 
            // nudAnio
            // 
            nudAnio.Location = new Point(140, 156);
            nudAnio.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            nudAnio.Minimum = new decimal(new int[] { 1900, 0, 0, 0 });
            nudAnio.Name = "nudAnio";
            nudAnio.Size = new Size(120, 23);
            nudAnio.TabIndex = 7;
            nudAnio.Value = new decimal(new int[] { 2026, 0, 0, 0 });

            // 
            // lblColor
            // 
            lblColor.AutoSize = true;
            lblColor.Location = new Point(24, 202);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(39, 15);
            lblColor.TabIndex = 8;
            lblColor.Text = "Color:";

            // 
            // txtColor
            // 
            txtColor.Location = new Point(140, 198);
            txtColor.MaxLength = 50;
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(250, 23);
            txtColor.TabIndex = 9;

            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(278, 330);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(95, 32);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;

            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(379, 330);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(95, 32);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;

            // 
            // FrmAltaVehiculo
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(500, 385);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(grpDatosVehiculo);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAltaVehiculo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Alta de vehículo";
            Load += FrmAltaVehiculo_Load;

            ((System.ComponentModel.ISupportInitialize)nudAnio).EndInit();
            grpDatosVehiculo.ResumeLayout(false);
            grpDatosVehiculo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private GroupBox grpDatosVehiculo;
        private Label lblDominio;
        private TextBox txtDominio;
        private Label lblMarca;
        private ComboBox cmbMarca;
        private Label lblModelo;
        private ComboBox cmbModelo;
        private Label lblAnio;
        private NumericUpDown nudAnio;
        private Label lblColor;
        private TextBox txtColor;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
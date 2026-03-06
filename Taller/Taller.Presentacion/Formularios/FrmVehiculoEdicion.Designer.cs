namespace Taller.Presentacion.Formularios
{
    partial class FrmVehiculoEdicion
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.lblCliente = new System.Windows.Forms.Label();
            this.cboCliente = new System.Windows.Forms.ComboBox();

            this.lblPatente = new System.Windows.Forms.Label();
            this.txtPatente = new System.Windows.Forms.TextBox();

            this.lblMarca = new System.Windows.Forms.Label();
            this.txtMarca = new System.Windows.Forms.TextBox();

            this.lblModelo = new System.Windows.Forms.Label();
            this.txtModelo = new System.Windows.Forms.TextBox();

            this.lblAnio = new System.Windows.Forms.Label();
            this.numAnio = new System.Windows.Forms.NumericUpDown();

            this.lblColor = new System.Windows.Forms.Label();
            this.txtColor = new System.Windows.Forms.TextBox();

            this.lblVin = new System.Windows.Forms.Label();
            this.txtVin = new System.Windows.Forms.TextBox();

            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();

            //this.chkActivo = new System.Windows.Forms.CheckBox();

            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.numAnio)).BeginInit();
            this.SuspendLayout();

            // lblCliente
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(12, 15);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(55, 15);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Cliente*:";

            // cboCliente
            this.cboCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCliente.Location = new System.Drawing.Point(140, 12);
            this.cboCliente.Name = "cboCliente";
            this.cboCliente.Size = new System.Drawing.Size(260, 23);
            this.cboCliente.TabIndex = 0;

            // lblPatente
            this.lblPatente.AutoSize = true;
            this.lblPatente.Location = new System.Drawing.Point(12, 50);
            this.lblPatente.Name = "lblPatente";
            this.lblPatente.Size = new System.Drawing.Size(57, 15);
            this.lblPatente.TabIndex = 2;
            this.lblPatente.Text = "Patente*:";

            // txtPatente
            this.txtPatente.Location = new System.Drawing.Point(140, 47);
            this.txtPatente.Name = "txtPatente";
            this.txtPatente.Size = new System.Drawing.Size(150, 23);
            this.txtPatente.TabIndex = 1;

            // lblMarca
            this.lblMarca.AutoSize = true;
            this.lblMarca.Location = new System.Drawing.Point(12, 85);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Size = new System.Drawing.Size(47, 15);
            this.lblMarca.TabIndex = 4;
            this.lblMarca.Text = "Marca*:";

            // txtMarca
            this.txtMarca.Location = new System.Drawing.Point(140, 82);
            this.txtMarca.Name = "txtMarca";
            this.txtMarca.Size = new System.Drawing.Size(260, 23);
            this.txtMarca.TabIndex = 2;

            // lblModelo
            this.lblModelo.AutoSize = true;
            this.lblModelo.Location = new System.Drawing.Point(12, 120);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(55, 15);
            this.lblModelo.TabIndex = 6;
            this.lblModelo.Text = "Modelo*:";

            // txtModelo
            this.txtModelo.Location = new System.Drawing.Point(140, 117);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new System.Drawing.Size(260, 23);
            this.txtModelo.TabIndex = 3;

            // lblAnio
            this.lblAnio.AutoSize = true;
            this.lblAnio.Location = new System.Drawing.Point(12, 155);
            this.lblAnio.Name = "lblAnio";
            this.lblAnio.Size = new System.Drawing.Size(33, 15);
            this.lblAnio.TabIndex = 8;
            this.lblAnio.Text = "Año*:";

            // numAnio
            this.numAnio.Location = new System.Drawing.Point(140, 152);
            this.numAnio.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            this.numAnio.Minimum = new decimal(new int[] { 1950, 0, 0, 0 });
            this.numAnio.Name = "numAnio";
            this.numAnio.Size = new System.Drawing.Size(100, 23);
            this.numAnio.TabIndex = 4;
            this.numAnio.Value = new decimal(new int[] { 2014, 0, 0, 0 });

            // lblColor
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(12, 190);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(40, 15);
            this.lblColor.TabIndex = 10;
            this.lblColor.Text = "Color:";

            // txtColor
            this.txtColor.Location = new System.Drawing.Point(140, 187);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(150, 23);
            this.txtColor.TabIndex = 5;

            // lblVin
            this.lblVin.AutoSize = true;
            this.lblVin.Location = new System.Drawing.Point(12, 225);
            this.lblVin.Name = "lblVin";
            this.lblVin.Size = new System.Drawing.Size(30, 15);
            this.lblVin.TabIndex = 12;
            this.lblVin.Text = "VIN:";

            // txtVin
            this.txtVin.Location = new System.Drawing.Point(140, 222);
            this.txtVin.Name = "txtVin";
            this.txtVin.Size = new System.Drawing.Size(260, 23);
            this.txtVin.TabIndex = 6;

            // lblObservaciones
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(12, 260);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(92, 15);
            this.lblObservaciones.TabIndex = 14;
            this.lblObservaciones.Text = "Observaciones:";

            // txtObservaciones
            this.txtObservaciones.Location = new System.Drawing.Point(140, 257);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(260, 70);
            this.txtObservaciones.TabIndex = 7;

            // chkActivo
            //this.chkActivo.AutoSize = true;
            //this.chkActivo.Location = new System.Drawing.Point(140, 335);
            //this.chkActivo.Name = "chkActivo";
            //this.chkActivo.Size = new System.Drawing.Size(59, 19);
            //this.chkActivo.TabIndex = 8;
            //this.chkActivo.Text = "Activo";
            //this.chkActivo.UseVisualStyleBackColor = true;

            // btnGuardar
            this.btnGuardar.Location = new System.Drawing.Point(244, 370);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 27);
            this.btnGuardar.TabIndex = 9;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            // btnCancelar
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(325, 370);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 27);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // FrmVehiculoEdicion
            this.AcceptButton = this.btnGuardar;
            this.CancelButton = this.btnCancelar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 410);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            //this.Controls.Add(this.chkActivo);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.lblObservaciones);
            this.Controls.Add(this.txtVin);
            this.Controls.Add(this.lblVin);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.numAnio);
            this.Controls.Add(this.lblAnio);
            this.Controls.Add(this.txtModelo);
            this.Controls.Add(this.lblModelo);
            this.Controls.Add(this.txtMarca);
            this.Controls.Add(this.lblMarca);
            this.Controls.Add(this.txtPatente);
            this.Controls.Add(this.lblPatente);
            this.Controls.Add(this.cboCliente);
            this.Controls.Add(this.lblCliente);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVehiculoEdicion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nuevo vehículo";
            ((System.ComponentModel.ISupportInitialize)(this.numAnio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.ComboBox cboCliente;

        private System.Windows.Forms.Label lblPatente;
        private System.Windows.Forms.TextBox txtPatente;

        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.TextBox txtMarca;

        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.TextBox txtModelo;

        private System.Windows.Forms.Label lblAnio;
        private System.Windows.Forms.NumericUpDown numAnio;

        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.TextBox txtColor;

        private System.Windows.Forms.Label lblVin;
        private System.Windows.Forms.TextBox txtVin;

        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;

        //private System.Windows.Forms.CheckBox chkActivo;

        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
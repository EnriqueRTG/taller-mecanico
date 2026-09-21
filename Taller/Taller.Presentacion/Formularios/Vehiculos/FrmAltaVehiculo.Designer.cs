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
            tlpPrincipal = new TableLayoutPanel();
            pnlEncabezado = new Panel();
            tlpEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            pnlContenido = new Panel();
            tlpContenido = new TableLayoutPanel();
            pnlIdentificacion = new Panel();
            tlpIdentificacion = new TableLayoutPanel();
            lblSeccionIdentificacion = new Label();
            lblDominio = new Label();
            lblAnio = new Label();
            txtDominio = new TextBox();
            nudAnio = new NumericUpDown();
            lblColor = new Label();
            txtColor = new TextBox();
            pnlClasificacion = new Panel();
            tlpClasificacion = new TableLayoutPanel();
            lblSeccionClasificacion = new Label();
            lblMarca = new Label();
            lblModelo = new Label();
            cboMarca = new ComboBox();
            cboModelo = new ComboBox();
            lblAyudaModelo = new Label();
            pnlAcciones = new Panel();
            flpAcciones = new FlowLayoutPanel();
            btnGuardar = new Button();
            btnCancelar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            pnlContenido.SuspendLayout();
            tlpContenido.SuspendLayout();
            pnlIdentificacion.SuspendLayout();
            tlpIdentificacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAnio).BeginInit();
            pnlClasificacion.SuspendLayout();
            tlpClasificacion.SuspendLayout();
            pnlAcciones.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(pnlContenido, 0, 1);
            tlpPrincipal.Controls.Add(pnlAcciones, 0, 2);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 3;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tlpPrincipal.Size = new Size(744, 531);
            tlpPrincipal.TabIndex = 0;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tlpEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Margin = new Padding(0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(24, 14, 24, 10);
            pnlEncabezado.Size = new Size(744, 82);
            pnlEncabezado.TabIndex = 0;
            // 
            // tlpEncabezado
            // 
            tlpEncabezado.ColumnCount = 1;
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpEncabezado.Controls.Add(lblTitulo, 0, 0);
            tlpEncabezado.Controls.Add(lblDescripcion, 0, 1);
            tlpEncabezado.Dock = DockStyle.Fill;
            tlpEncabezado.Location = new Point(24, 14);
            tlpEncabezado.Margin = new Padding(0);
            tlpEncabezado.Name = "tlpEncabezado";
            tlpEncabezado.RowCount = 2;
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 47.36842F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 52.63158F));
            tlpEncabezado.Size = new Size(696, 58);
            tlpEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Margin = new Padding(0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(696, 27);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registrar vehículo";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(0, 27);
            lblDescripcion.Margin = new Padding(0);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(696, 31);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Complete los datos de identificación, marca y modelo del vehículo.";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlContenido
            // 
            pnlContenido.AutoScroll = true;
            pnlContenido.Controls.Add(tlpContenido);
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Location = new Point(0, 82);
            pnlContenido.Margin = new Padding(0);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Padding = new Padding(24, 16, 24, 16);
            pnlContenido.Size = new Size(744, 379);
            pnlContenido.TabIndex = 1;
            // 
            // tlpContenido
            // 
            tlpContenido.ColumnCount = 1;
            tlpContenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContenido.Controls.Add(pnlIdentificacion, 0, 0);
            tlpContenido.Controls.Add(pnlClasificacion, 0, 1);
            tlpContenido.Dock = DockStyle.Top;
            tlpContenido.Location = new Point(24, 16);
            tlpContenido.Margin = new Padding(0);
            tlpContenido.Name = "tlpContenido";
            tlpContenido.RowCount = 2;
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 196F));
            tlpContenido.Size = new Size(679, 386);
            tlpContenido.TabIndex = 0;
            // 
            // pnlIdentificacion
            // 
            pnlIdentificacion.BackColor = Color.White;
            pnlIdentificacion.Controls.Add(tlpIdentificacion);
            pnlIdentificacion.Dock = DockStyle.Fill;
            pnlIdentificacion.Location = new Point(0, 0);
            pnlIdentificacion.Margin = new Padding(0, 0, 0, 12);
            pnlIdentificacion.Name = "pnlIdentificacion";
            pnlIdentificacion.Padding = new Padding(18, 12, 18, 12);
            pnlIdentificacion.Size = new Size(679, 178);
            pnlIdentificacion.TabIndex = 0;
            // 
            // tlpIdentificacion
            // 
            tlpIdentificacion.ColumnCount = 3;
            tlpIdentificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tlpIdentificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpIdentificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpIdentificacion.Controls.Add(lblSeccionIdentificacion, 0, 0);
            tlpIdentificacion.Controls.Add(lblDominio, 0, 1);
            tlpIdentificacion.Controls.Add(lblAnio, 2, 1);
            tlpIdentificacion.Controls.Add(txtDominio, 0, 2);
            tlpIdentificacion.Controls.Add(nudAnio, 2, 2);
            tlpIdentificacion.Controls.Add(lblColor, 0, 3);
            tlpIdentificacion.Controls.Add(txtColor, 0, 4);
            tlpIdentificacion.Dock = DockStyle.Fill;
            tlpIdentificacion.Location = new Point(18, 12);
            tlpIdentificacion.Margin = new Padding(0);
            tlpIdentificacion.Name = "tlpIdentificacion";
            tlpIdentificacion.RowCount = 5;
            tlpIdentificacion.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpIdentificacion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpIdentificacion.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpIdentificacion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpIdentificacion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpIdentificacion.Size = new Size(643, 154);
            tlpIdentificacion.TabIndex = 0;
            // 
            // lblSeccionIdentificacion
            // 
            tlpIdentificacion.SetColumnSpan(lblSeccionIdentificacion, 3);
            lblSeccionIdentificacion.Dock = DockStyle.Fill;
            lblSeccionIdentificacion.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSeccionIdentificacion.ForeColor = Color.FromArgb(30, 64, 175);
            lblSeccionIdentificacion.Location = new Point(0, 0);
            lblSeccionIdentificacion.Margin = new Padding(0);
            lblSeccionIdentificacion.Name = "lblSeccionIdentificacion";
            lblSeccionIdentificacion.Size = new Size(643, 24);
            lblSeccionIdentificacion.TabIndex = 0;
            lblSeccionIdentificacion.Text = "IDENTIFICACIÓN DEL VEHÍCULO";
            lblSeccionIdentificacion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominio
            // 
            lblDominio.AutoSize = true;
            lblDominio.Dock = DockStyle.Fill;
            lblDominio.ForeColor = Color.FromArgb(51, 65, 85);
            lblDominio.Location = new Point(0, 24);
            lblDominio.Margin = new Padding(0);
            lblDominio.Name = "lblDominio";
            lblDominio.Size = new Size(402, 20);
            lblDominio.TabIndex = 1;
            lblDominio.Text = "Dominio *";
            lblDominio.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblAnio
            // 
            lblAnio.AutoSize = true;
            lblAnio.Dock = DockStyle.Fill;
            lblAnio.ForeColor = Color.FromArgb(51, 65, 85);
            lblAnio.Location = new Point(426, 24);
            lblAnio.Margin = new Padding(0);
            lblAnio.Name = "lblAnio";
            lblAnio.Size = new Size(217, 20);
            lblAnio.TabIndex = 2;
            lblAnio.Text = "Año *";
            lblAnio.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtDominio
            // 
            txtDominio.CharacterCasing = CharacterCasing.Upper;
            txtDominio.Dock = DockStyle.Fill;
            txtDominio.Location = new Point(0, 47);
            txtDominio.Margin = new Padding(0, 3, 0, 3);
            txtDominio.MaxLength = 10;
            txtDominio.Name = "txtDominio";
            txtDominio.Size = new Size(402, 23);
            txtDominio.TabIndex = 0;
            txtDominio.TextChanged += CampoObligatorio_Cambio;
            // 
            // nudAnio
            // 
            nudAnio.Dock = DockStyle.Fill;
            nudAnio.Location = new Point(426, 47);
            nudAnio.Margin = new Padding(0, 3, 0, 3);
            nudAnio.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            nudAnio.Minimum = new decimal(new int[] { 1900, 0, 0, 0 });
            nudAnio.Name = "nudAnio";
            nudAnio.Size = new Size(217, 23);
            nudAnio.TabIndex = 1;
            nudAnio.Value = new decimal(new int[] { 1900, 0, 0, 0 });
            nudAnio.ValueChanged += CampoObligatorio_Cambio;
            // 
            // lblColor
            // 
            lblColor.AutoSize = true;
            tlpIdentificacion.SetColumnSpan(lblColor, 3);
            lblColor.Dock = DockStyle.Fill;
            lblColor.ForeColor = Color.FromArgb(51, 65, 85);
            lblColor.Location = new Point(0, 78);
            lblColor.Margin = new Padding(0);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(643, 20);
            lblColor.TabIndex = 3;
            lblColor.Text = "Color *";
            lblColor.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtColor
            // 
            txtColor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tlpIdentificacion.SetColumnSpan(txtColor, 3);
            txtColor.Location = new Point(0, 101);
            txtColor.Margin = new Padding(0, 3, 0, 0);
            txtColor.MaxLength = 50;
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(643, 23);
            txtColor.TabIndex = 2;
            txtColor.TextChanged += CampoObligatorio_Cambio;
            // 
            // pnlClasificacion
            // 
            pnlClasificacion.BackColor = Color.White;
            pnlClasificacion.Controls.Add(tlpClasificacion);
            pnlClasificacion.Dock = DockStyle.Fill;
            pnlClasificacion.Location = new Point(0, 190);
            pnlClasificacion.Margin = new Padding(0);
            pnlClasificacion.Name = "pnlClasificacion";
            pnlClasificacion.Padding = new Padding(18, 12, 18, 12);
            pnlClasificacion.Size = new Size(679, 196);
            pnlClasificacion.TabIndex = 1;
            // 
            // tlpClasificacion
            // 
            tlpClasificacion.ColumnCount = 3;
            tlpClasificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpClasificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpClasificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpClasificacion.Controls.Add(lblSeccionClasificacion, 0, 0);
            tlpClasificacion.Controls.Add(lblMarca, 0, 1);
            tlpClasificacion.Controls.Add(lblModelo, 2, 1);
            tlpClasificacion.Controls.Add(cboMarca, 0, 2);
            tlpClasificacion.Controls.Add(cboModelo, 2, 2);
            tlpClasificacion.Controls.Add(lblAyudaModelo, 0, 3);
            tlpClasificacion.Dock = DockStyle.Fill;
            tlpClasificacion.Location = new Point(18, 12);
            tlpClasificacion.Margin = new Padding(0);
            tlpClasificacion.Name = "tlpClasificacion";
            tlpClasificacion.RowCount = 4;
            tlpClasificacion.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpClasificacion.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
            tlpClasificacion.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpClasificacion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpClasificacion.Size = new Size(643, 172);
            tlpClasificacion.TabIndex = 0;
            // 
            // lblSeccionClasificacion
            // 
            tlpClasificacion.SetColumnSpan(lblSeccionClasificacion, 3);
            lblSeccionClasificacion.Dock = DockStyle.Fill;
            lblSeccionClasificacion.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSeccionClasificacion.ForeColor = Color.FromArgb(30, 64, 175);
            lblSeccionClasificacion.Location = new Point(0, 0);
            lblSeccionClasificacion.Margin = new Padding(0);
            lblSeccionClasificacion.Name = "lblSeccionClasificacion";
            lblSeccionClasificacion.Size = new Size(643, 24);
            lblSeccionClasificacion.TabIndex = 0;
            lblSeccionClasificacion.Text = "MARCA Y MODELO";
            lblSeccionClasificacion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMarca
            // 
            lblMarca.AutoSize = true;
            lblMarca.Dock = DockStyle.Fill;
            lblMarca.ForeColor = Color.FromArgb(51, 65, 85);
            lblMarca.Location = new Point(0, 24);
            lblMarca.Margin = new Padding(0);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(309, 19);
            lblMarca.TabIndex = 1;
            lblMarca.Text = "Marca *";
            lblMarca.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblModelo
            // 
            lblModelo.AutoSize = true;
            lblModelo.Dock = DockStyle.Fill;
            lblModelo.ForeColor = Color.FromArgb(51, 65, 85);
            lblModelo.Location = new Point(333, 24);
            lblModelo.Margin = new Padding(0);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(310, 19);
            lblModelo.TabIndex = 2;
            lblModelo.Text = "Modelo *";
            lblModelo.TextAlign = ContentAlignment.BottomLeft;
            // 
            // cboMarca
            // 
            cboMarca.Dock = DockStyle.Fill;
            cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMarca.FormattingEnabled = true;
            cboMarca.Location = new Point(0, 46);
            cboMarca.Margin = new Padding(0, 3, 0, 3);
            cboMarca.Name = "cboMarca";
            cboMarca.Size = new Size(309, 23);
            cboMarca.TabIndex = 3;
            cboMarca.SelectedIndexChanged += cboMarca_SelectedIndexChanged;
            // 
            // cboModelo
            // 
            cboModelo.Dock = DockStyle.Fill;
            cboModelo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboModelo.Enabled = false;
            cboModelo.FormattingEnabled = true;
            cboModelo.Location = new Point(333, 46);
            cboModelo.Margin = new Padding(0, 3, 0, 3);
            cboModelo.Name = "cboModelo";
            cboModelo.Size = new Size(310, 23);
            cboModelo.TabIndex = 4;
            cboModelo.SelectedIndexChanged += CampoObligatorio_Cambio;
            // 
            // lblAyudaModelo
            // 
            lblAyudaModelo.AutoEllipsis = true;
            tlpClasificacion.SetColumnSpan(lblAyudaModelo, 3);
            lblAyudaModelo.Dock = DockStyle.Fill;
            lblAyudaModelo.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblAyudaModelo.ForeColor = Color.FromArgb(100, 116, 139);
            lblAyudaModelo.Location = new Point(0, 78);
            lblAyudaModelo.Margin = new Padding(0);
            lblAyudaModelo.Name = "lblAyudaModelo";
            lblAyudaModelo.Size = new Size(643, 94);
            lblAyudaModelo.TabIndex = 5;
            lblAyudaModelo.Text = "Seleccione primero una marca para consultar sus modelos disponibles.";
            lblAyudaModelo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.White;
            pnlAcciones.Controls.Add(flpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(0, 461);
            pnlAcciones.Margin = new Padding(0);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Padding = new Padding(24, 15, 24, 15);
            pnlAcciones.Size = new Size(744, 70);
            pnlAcciones.TabIndex = 2;
            // 
            // flpAcciones
            // 
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.FlowDirection = FlowDirection.RightToLeft;
            flpAcciones.Location = new Point(24, 15);
            flpAcciones.Margin = new Padding(0, 15, 16, 0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Size = new Size(696, 40);
            flpAcciones.TabIndex = 0;
            flpAcciones.WrapContents = false;
            // 
            // btnGuardar
            // 
            btnGuardar.Enabled = false;
            btnGuardar.Location = new Point(526, 0);
            btnGuardar.Margin = new Padding(8, 0, 0, 0);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(170, 38);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Registrar vehículo";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(398, 0);
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FrmAltaVehiculo
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnCancelar;
            ClientSize = new Size(744, 531);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(720, 540);
            Name = "FrmAltaVehiculo";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar vehículo ";
            Load += FrmAltaVehiculo_Load;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            pnlContenido.ResumeLayout(false);
            tlpContenido.ResumeLayout(false);
            pnlIdentificacion.ResumeLayout(false);
            tlpIdentificacion.ResumeLayout(false);
            tlpIdentificacion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudAnio).EndInit();
            pnlClasificacion.ResumeLayout(false);
            tlpClasificacion.ResumeLayout(false);
            tlpClasificacion.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            flpAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private Panel pnlContenido;
        private Panel pnlAcciones;
        private TableLayoutPanel tlpEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private TableLayoutPanel tlpContenido;
        private Panel pnlIdentificacion;
        private Panel pnlClasificacion;
        private FlowLayoutPanel flpAcciones;
        private TableLayoutPanel tlpIdentificacion;
        private Label lblSeccionIdentificacion;
        private Label lblDominio;
        private Label lblAnio;
        private TextBox txtDominio;
        private NumericUpDown nudAnio;
        private Label lblColor;
        private TextBox txtColor;
        private TableLayoutPanel tlpClasificacion;
        private Label lblSeccionClasificacion;
        private Label lblMarca;
        private Label lblModelo;
        private ComboBox cboMarca;
        private ComboBox cboModelo;
        private Label lblAyudaModelo;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
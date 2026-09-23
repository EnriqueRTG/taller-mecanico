namespace Taller.Presentacion.Formularios.Atenciones
{
    partial class FrmEditarAtencion
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
            pnlEncabezado = new Panel();
            tlpEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            lblNumeroAtencion = new Label();
            grpAtencionSeleccionada = new GroupBox();
            tlpResumen = new TableLayoutPanel();
            lblClienteTitulo = new Label();
            lblVehiculoTitulo = new Label();
            lblDominioTitulo = new Label();
            lblFechaIngresoTitulo = new Label();
            lblClienteValor = new Label();
            lblVehiculoValor = new Label();
            lblDominioValor = new Label();
            lblFechaIngresoValor = new Label();
            tlpDatosEditables = new TableLayoutPanel();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            lblPrioridad = new Label();
            cboPrioridad = new ComboBox();
            pnlAcciones = new Panel();
            flpAcciones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            grpAtencionSeleccionada.SuspendLayout();
            tlpResumen.SuspendLayout();
            tlpDatosEditables.SuspendLayout();
            pnlAcciones.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(grpAtencionSeleccionada, 0, 1);
            tlpPrincipal.Controls.Add(tlpDatosEditables, 0, 2);
            tlpPrincipal.Controls.Add(pnlAcciones, 0, 3);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(18);
            tlpPrincipal.RowCount = 4;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 145F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tlpPrincipal.Size = new Size(804, 561);
            tlpPrincipal.TabIndex = 0;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tlpEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(18, 18);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 10);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(18, 12, 18, 12);
            pnlEncabezado.Size = new Size(768, 75);
            pnlEncabezado.TabIndex = 0;
            // 
            // tlpEncabezado
            // 
            tlpEncabezado.ColumnCount = 2;
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpEncabezado.Controls.Add(lblTitulo, 0, 0);
            tlpEncabezado.Controls.Add(lblDescripcion, 0, 1);
            tlpEncabezado.Controls.Add(lblNumeroAtencion, 1, 0);
            tlpEncabezado.Dock = DockStyle.Fill;
            tlpEncabezado.Location = new Point(18, 12);
            tlpEncabezado.Margin = new Padding(0);
            tlpEncabezado.Name = "tlpEncabezado";
            tlpEncabezado.RowCount = 2;
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpEncabezado.Size = new Size(732, 51);
            tlpEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(3, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(506, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Editar datos de la atención";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(3, 25);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(506, 26);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Modifique la información administrativa permitida.";
            // 
            // lblNumeroAtencion
            // 
            lblNumeroAtencion.AutoSize = true;
            lblNumeroAtencion.Dock = DockStyle.Fill;
            lblNumeroAtencion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumeroAtencion.ForeColor = Color.FromArgb(30, 64, 175);
            lblNumeroAtencion.Location = new Point(515, 0);
            lblNumeroAtencion.Name = "lblNumeroAtencion";
            lblNumeroAtencion.Size = new Size(214, 25);
            lblNumeroAtencion.TabIndex = 2;
            lblNumeroAtencion.Text = "Atención N.º 000001";
            lblNumeroAtencion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // grpAtencionSeleccionada
            // 
            grpAtencionSeleccionada.BackColor = Color.White;
            grpAtencionSeleccionada.Controls.Add(tlpResumen);
            grpAtencionSeleccionada.Dock = DockStyle.Fill;
            grpAtencionSeleccionada.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpAtencionSeleccionada.ForeColor = Color.FromArgb(15, 23, 42);
            grpAtencionSeleccionada.Location = new Point(18, 103);
            grpAtencionSeleccionada.Margin = new Padding(0, 0, 0, 10);
            grpAtencionSeleccionada.Name = "grpAtencionSeleccionada";
            grpAtencionSeleccionada.Padding = new Padding(12);
            grpAtencionSeleccionada.Size = new Size(768, 135);
            grpAtencionSeleccionada.TabIndex = 1;
            grpAtencionSeleccionada.TabStop = false;
            grpAtencionSeleccionada.Text = "Atención seleccionada";
            // 
            // tlpResumen
            // 
            tlpResumen.ColumnCount = 4;
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpResumen.Controls.Add(lblClienteTitulo, 0, 0);
            tlpResumen.Controls.Add(lblVehiculoTitulo, 1, 0);
            tlpResumen.Controls.Add(lblDominioTitulo, 2, 0);
            tlpResumen.Controls.Add(lblFechaIngresoTitulo, 3, 0);
            tlpResumen.Controls.Add(lblClienteValor, 0, 1);
            tlpResumen.Controls.Add(lblVehiculoValor, 1, 1);
            tlpResumen.Controls.Add(lblDominioValor, 2, 1);
            tlpResumen.Controls.Add(lblFechaIngresoValor, 3, 1);
            tlpResumen.Dock = DockStyle.Fill;
            tlpResumen.Location = new Point(12, 28);
            tlpResumen.Margin = new Padding(0);
            tlpResumen.Name = "tlpResumen";
            tlpResumen.Padding = new Padding(8);
            tlpResumen.RowCount = 2;
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpResumen.Size = new Size(744, 95);
            tlpResumen.TabIndex = 0;
            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.AutoSize = true;
            lblClienteTitulo.Dock = DockStyle.Fill;
            lblClienteTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblClienteTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblClienteTitulo.Location = new Point(16, 13);
            lblClienteTitulo.Margin = new Padding(8, 5, 8, 0);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.Size = new Size(166, 25);
            lblClienteTitulo.TabIndex = 0;
            lblClienteTitulo.Text = "CLIENTE";
            lblClienteTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculoTitulo
            // 
            lblVehiculoTitulo.AutoSize = true;
            lblVehiculoTitulo.Dock = DockStyle.Fill;
            lblVehiculoTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblVehiculoTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblVehiculoTitulo.Location = new Point(198, 13);
            lblVehiculoTitulo.Margin = new Padding(8, 5, 8, 0);
            lblVehiculoTitulo.Name = "lblVehiculoTitulo";
            lblVehiculoTitulo.Size = new Size(166, 25);
            lblVehiculoTitulo.TabIndex = 1;
            lblVehiculoTitulo.Text = "VEHÍCULO";
            lblVehiculoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioTitulo
            // 
            lblDominioTitulo.AutoSize = true;
            lblDominioTitulo.Dock = DockStyle.Fill;
            lblDominioTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblDominioTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblDominioTitulo.Location = new Point(380, 13);
            lblDominioTitulo.Margin = new Padding(8, 5, 8, 0);
            lblDominioTitulo.Name = "lblDominioTitulo";
            lblDominioTitulo.Size = new Size(166, 25);
            lblDominioTitulo.TabIndex = 2;
            lblDominioTitulo.Text = "DOMINIO";
            lblDominioTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFechaIngresoTitulo
            // 
            lblFechaIngresoTitulo.AutoSize = true;
            lblFechaIngresoTitulo.Dock = DockStyle.Fill;
            lblFechaIngresoTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblFechaIngresoTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblFechaIngresoTitulo.Location = new Point(562, 13);
            lblFechaIngresoTitulo.Margin = new Padding(8, 5, 8, 0);
            lblFechaIngresoTitulo.Name = "lblFechaIngresoTitulo";
            lblFechaIngresoTitulo.Size = new Size(166, 25);
            lblFechaIngresoTitulo.TabIndex = 3;
            lblFechaIngresoTitulo.Text = "INGRESO";
            lblFechaIngresoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClienteValor
            // 
            lblClienteValor.AutoSize = true;
            lblClienteValor.Dock = DockStyle.Fill;
            lblClienteValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblClienteValor.Location = new Point(16, 38);
            lblClienteValor.Margin = new Padding(8, 0, 8, 5);
            lblClienteValor.Name = "lblClienteValor";
            lblClienteValor.Size = new Size(166, 44);
            lblClienteValor.TabIndex = 4;
            lblClienteValor.Text = "Juan Pérez";
            lblClienteValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculoValor
            // 
            lblVehiculoValor.AutoSize = true;
            lblVehiculoValor.Dock = DockStyle.Fill;
            lblVehiculoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblVehiculoValor.Location = new Point(198, 38);
            lblVehiculoValor.Margin = new Padding(8, 0, 8, 5);
            lblVehiculoValor.Name = "lblVehiculoValor";
            lblVehiculoValor.Size = new Size(166, 44);
            lblVehiculoValor.TabIndex = 5;
            lblVehiculoValor.Text = "Honda Fit";
            lblVehiculoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioValor
            // 
            lblDominioValor.AutoSize = true;
            lblDominioValor.Dock = DockStyle.Fill;
            lblDominioValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblDominioValor.Location = new Point(380, 38);
            lblDominioValor.Margin = new Padding(8, 0, 8, 5);
            lblDominioValor.Name = "lblDominioValor";
            lblDominioValor.Size = new Size(166, 44);
            lblDominioValor.TabIndex = 6;
            lblDominioValor.Text = "AB345CD";
            lblDominioValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFechaIngresoValor
            // 
            lblFechaIngresoValor.AutoSize = true;
            lblFechaIngresoValor.Dock = DockStyle.Fill;
            lblFechaIngresoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblFechaIngresoValor.Location = new Point(562, 38);
            lblFechaIngresoValor.Margin = new Padding(8, 0, 8, 5);
            lblFechaIngresoValor.Name = "lblFechaIngresoValor";
            lblFechaIngresoValor.Size = new Size(166, 44);
            lblFechaIngresoValor.TabIndex = 7;
            lblFechaIngresoValor.Text = "07/09/2026 16:07";
            lblFechaIngresoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpDatosEditables
            // 
            tlpDatosEditables.ColumnCount = 2;
            tlpDatosEditables.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpDatosEditables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDatosEditables.Controls.Add(lblMotivo, 0, 0);
            tlpDatosEditables.Controls.Add(txtMotivo, 1, 0);
            tlpDatosEditables.Controls.Add(lblObservaciones, 0, 1);
            tlpDatosEditables.Controls.Add(txtObservaciones, 1, 1);
            tlpDatosEditables.Controls.Add(lblPrioridad, 0, 2);
            tlpDatosEditables.Controls.Add(cboPrioridad, 1, 2);
            tlpDatosEditables.Dock = DockStyle.Fill;
            tlpDatosEditables.Location = new Point(21, 251);
            tlpDatosEditables.Name = "tlpDatosEditables";
            tlpDatosEditables.Padding = new Padding(8);
            tlpDatosEditables.RowCount = 3;
            tlpDatosEditables.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpDatosEditables.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpDatosEditables.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDatosEditables.Size = new Size(762, 224);
            tlpDatosEditables.TabIndex = 2;
            // 
            // lblMotivo
            // 
            lblMotivo.AutoSize = true;
            lblMotivo.Dock = DockStyle.Fill;
            lblMotivo.Location = new Point(13, 16);
            lblMotivo.Margin = new Padding(5, 8, 12, 5);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(133, 67);
            lblMotivo.TabIndex = 0;
            lblMotivo.Text = "Motivo de consulta:";
            lblMotivo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtMotivo
            // 
            txtMotivo.BackColor = Color.White;
            txtMotivo.BorderStyle = BorderStyle.FixedSingle;
            txtMotivo.Dock = DockStyle.Fill;
            txtMotivo.ForeColor = Color.FromArgb(51, 65, 85);
            txtMotivo.Location = new Point(163, 13);
            txtMotivo.Margin = new Padding(5);
            txtMotivo.Multiline = true;
            txtMotivo.Name = "txtMotivo";
            txtMotivo.ScrollBars = ScrollBars.Vertical;
            txtMotivo.Size = new Size(586, 70);
            txtMotivo.TabIndex = 0;
            txtMotivo.TextChanged += CampoEditable_Cambio;
            // 
            // lblObservaciones
            // 
            lblObservaciones.AutoSize = true;
            lblObservaciones.Dock = DockStyle.Fill;
            lblObservaciones.Location = new Point(13, 96);
            lblObservaciones.Margin = new Padding(5, 8, 12, 5);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(133, 67);
            lblObservaciones.TabIndex = 2;
            lblObservaciones.Text = "Observaciones:";
            lblObservaciones.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtObservaciones
            // 
            txtObservaciones.BackColor = Color.White;
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.ForeColor = Color.FromArgb(51, 65, 85);
            txtObservaciones.Location = new Point(163, 93);
            txtObservaciones.Margin = new Padding(5);
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(586, 70);
            txtObservaciones.TabIndex = 1;
            txtObservaciones.TextChanged += CampoEditable_Cambio;
            // 
            // lblPrioridad
            // 
            lblPrioridad.AutoSize = true;
            lblPrioridad.Dock = DockStyle.Fill;
            lblPrioridad.Location = new Point(13, 173);
            lblPrioridad.Margin = new Padding(5, 5, 12, 5);
            lblPrioridad.Name = "lblPrioridad";
            lblPrioridad.Size = new Size(133, 38);
            lblPrioridad.TabIndex = 4;
            lblPrioridad.Text = "Prioridad:";
            lblPrioridad.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cboPrioridad
            // 
            cboPrioridad.Anchor = AnchorStyles.Left;
            cboPrioridad.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPrioridad.FormattingEnabled = true;
            cboPrioridad.Location = new Point(163, 180);
            cboPrioridad.Margin = new Padding(5);
            cboPrioridad.Name = "cboPrioridad";
            cboPrioridad.Size = new Size(220, 23);
            cboPrioridad.TabIndex = 2;
            cboPrioridad.SelectedIndexChanged += CampoEditable_Cambio;
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.White;
            pnlAcciones.Controls.Add(flpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(18, 478);
            pnlAcciones.Margin = new Padding(0);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Padding = new Padding(12);
            pnlAcciones.Size = new Size(768, 65);
            pnlAcciones.TabIndex = 3;
            // 
            // flpAcciones
            // 
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Dock = DockStyle.Right;
            flpAcciones.Location = new Point(458, 12);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Size = new Size(298, 41);
            flpAcciones.TabIndex = 0;
            flpAcciones.WrapContents = false;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(5, 5);
            btnCancelar.Margin = new Padding(5);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 36);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(135, 5);
            btnGuardar.Margin = new Padding(5);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(155, 36);
            btnGuardar.TabIndex = 4;
            btnGuardar.Text = "Guardar cambios";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // FrmEditarAtencion
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnCancelar;
            ClientSize = new Size(804, 561);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditarAtencion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editar atención";
            FormClosing += FrmEditarAtencion_FormClosing;
            Load += FrmEditarAtencion_Load;
            Shown += FrmEditarAtencion_Shown;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            tlpEncabezado.PerformLayout();
            grpAtencionSeleccionada.ResumeLayout(false);
            tlpResumen.ResumeLayout(false);
            tlpResumen.PerformLayout();
            tlpDatosEditables.ResumeLayout(false);
            tlpDatosEditables.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            flpAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private TableLayoutPanel tlpEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblNumeroAtencion;
        private GroupBox grpAtencionSeleccionada;
        private TableLayoutPanel tlpResumen;
        private Label lblClienteTitulo;
        private Label lblVehiculoTitulo;
        private Label lblDominioTitulo;
        private Label lblFechaIngresoTitulo;
        private Label lblClienteValor;
        private Label lblVehiculoValor;
        private Label lblDominioValor;
        private Label lblFechaIngresoValor;
        private TableLayoutPanel tlpDatosEditables;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Label lblPrioridad;
        private ComboBox cboPrioridad;
        private Panel pnlAcciones;
        private FlowLayoutPanel flpAcciones;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}
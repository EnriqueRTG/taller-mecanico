namespace Taller.Presentacion.Formularios.Diagnosticos
{
    partial class FrmRegistrarDiagnostico
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
            lblNumeroAtencion = new Label();
            grpResumenAtencion = new GroupBox();
            tlpResumenAtencion = new TableLayoutPanel();
            lblClienteTitulo = new Label();
            lblClienteValor = new Label();
            lblVehiculoTitulo = new Label();
            lblVehiculoValor = new Label();
            lblDominioTitulo = new Label();
            lblDominioValor = new Label();
            lblMotivoTitulo = new Label();
            lblMotivoValor = new Label();
            grpDatosDiagnostico = new GroupBox();
            tlpDatosDiagnostico = new TableLayoutPanel();
            lblTecnico = new Label();
            txtTecnico = new TextBox();
            lblFechaDiagnostico = new Label();
            dtpFechaDiagnostico = new DateTimePicker();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            lblResultado = new Label();
            cboResultado = new ComboBox();
            flpAcciones = new FlowLayoutPanel();
            btnGuardar = new Button();
            btnCancelar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            grpResumenAtencion.SuspendLayout();
            tlpResumenAtencion.SuspendLayout();
            grpDatosDiagnostico.SuspendLayout();
            tlpDatosDiagnostico.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(grpResumenAtencion, 0, 1);
            tlpPrincipal.Controls.Add(grpDatosDiagnostico, 0, 2);
            tlpPrincipal.Controls.Add(flpAcciones, 0, 3);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(18);
            tlpPrincipal.RowCount = 4;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 106F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            tlpPrincipal.Size = new Size(804, 651);
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
            pnlEncabezado.Padding = new Padding(14, 8, 14, 8);
            pnlEncabezado.Size = new Size(768, 58);
            pnlEncabezado.TabIndex = 0;
            // 
            // tlpEncabezado
            // 
            tlpEncabezado.ColumnCount = 2;
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            tlpEncabezado.Controls.Add(lblTitulo, 0, 0);
            tlpEncabezado.Controls.Add(lblNumeroAtencion, 1, 0);
            tlpEncabezado.Dock = DockStyle.Fill;
            tlpEncabezado.Location = new Point(14, 8);
            tlpEncabezado.Margin = new Padding(0);
            tlpEncabezado.Name = "tlpEncabezado";
            tlpEncabezado.RowCount = 1;
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpEncabezado.Size = new Size(740, 42);
            tlpEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Margin = new Padding(0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(532, 42);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registrar diagnóstico técnico";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNumeroAtencion
            // 
            lblNumeroAtencion.Dock = DockStyle.Fill;
            lblNumeroAtencion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumeroAtencion.ForeColor = Color.FromArgb(30, 64, 175);
            lblNumeroAtencion.Location = new Point(532, 0);
            lblNumeroAtencion.Margin = new Padding(0);
            lblNumeroAtencion.Name = "lblNumeroAtencion";
            lblNumeroAtencion.Size = new Size(208, 42);
            lblNumeroAtencion.TabIndex = 1;
            lblNumeroAtencion.Text = "Atención N.º 000001";
            lblNumeroAtencion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpResumenAtencion
            // 
            grpResumenAtencion.Controls.Add(tlpResumenAtencion);
            grpResumenAtencion.Dock = DockStyle.Fill;
            grpResumenAtencion.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpResumenAtencion.ForeColor = Color.FromArgb(15, 23, 42);
            grpResumenAtencion.Location = new Point(18, 86);
            grpResumenAtencion.Margin = new Padding(0, 0, 0, 10);
            grpResumenAtencion.Name = "grpResumenAtencion";
            grpResumenAtencion.Padding = new Padding(12);
            grpResumenAtencion.Size = new Size(768, 96);
            grpResumenAtencion.TabIndex = 1;
            grpResumenAtencion.TabStop = false;
            grpResumenAtencion.Text = "Atención seleccionada";
            // 
            // tlpResumenAtencion
            // 
            tlpResumenAtencion.ColumnCount = 4;
            tlpResumenAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            tlpResumenAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tlpResumenAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            tlpResumenAtencion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tlpResumenAtencion.Controls.Add(lblClienteTitulo, 0, 0);
            tlpResumenAtencion.Controls.Add(lblClienteValor, 1, 0);
            tlpResumenAtencion.Controls.Add(lblVehiculoTitulo, 2, 0);
            tlpResumenAtencion.Controls.Add(lblVehiculoValor, 3, 0);
            tlpResumenAtencion.Controls.Add(lblDominioTitulo, 0, 1);
            tlpResumenAtencion.Controls.Add(lblDominioValor, 1, 1);
            tlpResumenAtencion.Controls.Add(lblMotivoTitulo, 2, 1);
            tlpResumenAtencion.Controls.Add(lblMotivoValor, 3, 1);
            tlpResumenAtencion.Dock = DockStyle.Fill;
            tlpResumenAtencion.Location = new Point(12, 28);
            tlpResumenAtencion.Margin = new Padding(0);
            tlpResumenAtencion.Name = "tlpResumenAtencion";
            tlpResumenAtencion.RowCount = 2;
            tlpResumenAtencion.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumenAtencion.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpResumenAtencion.Size = new Size(744, 56);
            tlpResumenAtencion.TabIndex = 0;
            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.Dock = DockStyle.Fill;
            lblClienteTitulo.Font = new Font("Segoe UI", 9F);
            lblClienteTitulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblClienteTitulo.Location = new Point(0, 0);
            lblClienteTitulo.Margin = new Padding(0);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.Size = new Size(126, 28);
            lblClienteTitulo.TabIndex = 0;
            lblClienteTitulo.Text = "Cliente:";
            lblClienteTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClienteValor
            // 
            lblClienteValor.AutoEllipsis = true;
            lblClienteValor.Dock = DockStyle.Fill;
            lblClienteValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblClienteValor.Location = new Point(126, 0);
            lblClienteValor.Margin = new Padding(0);
            lblClienteValor.Name = "lblClienteValor";
            lblClienteValor.Size = new Size(245, 28);
            lblClienteValor.TabIndex = 1;
            lblClienteValor.Text = "Juan Pérez";
            lblClienteValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculoTitulo
            // 
            lblVehiculoTitulo.Dock = DockStyle.Fill;
            lblVehiculoTitulo.Font = new Font("Segoe UI", 9F);
            lblVehiculoTitulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblVehiculoTitulo.Location = new Point(371, 0);
            lblVehiculoTitulo.Margin = new Padding(0);
            lblVehiculoTitulo.Name = "lblVehiculoTitulo";
            lblVehiculoTitulo.Size = new Size(126, 28);
            lblVehiculoTitulo.TabIndex = 2;
            lblVehiculoTitulo.Text = "Vehículo:";
            lblVehiculoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculoValor
            // 
            lblVehiculoValor.AutoEllipsis = true;
            lblVehiculoValor.Dock = DockStyle.Fill;
            lblVehiculoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblVehiculoValor.Location = new Point(497, 0);
            lblVehiculoValor.Margin = new Padding(0);
            lblVehiculoValor.Name = "lblVehiculoValor";
            lblVehiculoValor.Size = new Size(247, 28);
            lblVehiculoValor.TabIndex = 3;
            lblVehiculoValor.Text = "Honda Fit";
            lblVehiculoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioTitulo
            // 
            lblDominioTitulo.Dock = DockStyle.Fill;
            lblDominioTitulo.Font = new Font("Segoe UI", 9F);
            lblDominioTitulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblDominioTitulo.Location = new Point(0, 28);
            lblDominioTitulo.Margin = new Padding(0);
            lblDominioTitulo.Name = "lblDominioTitulo";
            lblDominioTitulo.Size = new Size(126, 28);
            lblDominioTitulo.TabIndex = 4;
            lblDominioTitulo.Text = "Dominio:";
            lblDominioTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioValor
            // 
            lblDominioValor.AutoEllipsis = true;
            lblDominioValor.Dock = DockStyle.Fill;
            lblDominioValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblDominioValor.Location = new Point(126, 28);
            lblDominioValor.Margin = new Padding(0);
            lblDominioValor.Name = "lblDominioValor";
            lblDominioValor.Size = new Size(245, 28);
            lblDominioValor.TabIndex = 5;
            lblDominioValor.Text = "AB345CD";
            lblDominioValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMotivoTitulo
            // 
            lblMotivoTitulo.Dock = DockStyle.Fill;
            lblMotivoTitulo.Font = new Font("Segoe UI", 9F);
            lblMotivoTitulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblMotivoTitulo.Location = new Point(371, 28);
            lblMotivoTitulo.Margin = new Padding(0);
            lblMotivoTitulo.Name = "lblMotivoTitulo";
            lblMotivoTitulo.Size = new Size(126, 28);
            lblMotivoTitulo.TabIndex = 6;
            lblMotivoTitulo.Text = "Motivo:";
            lblMotivoTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMotivoValor
            // 
            lblMotivoValor.AutoEllipsis = true;
            lblMotivoValor.Dock = DockStyle.Fill;
            lblMotivoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblMotivoValor.Location = new Point(497, 28);
            lblMotivoValor.Margin = new Padding(0);
            lblMotivoValor.Name = "lblMotivoValor";
            lblMotivoValor.Size = new Size(247, 28);
            lblMotivoValor.TabIndex = 7;
            lblMotivoValor.Text = "Dificultades para arrancar";
            lblMotivoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpDatosDiagnostico
            // 
            grpDatosDiagnostico.Controls.Add(tlpDatosDiagnostico);
            grpDatosDiagnostico.Dock = DockStyle.Fill;
            grpDatosDiagnostico.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpDatosDiagnostico.ForeColor = Color.FromArgb(15, 23, 42);
            grpDatosDiagnostico.Location = new Point(18, 192);
            grpDatosDiagnostico.Margin = new Padding(0);
            grpDatosDiagnostico.Name = "grpDatosDiagnostico";
            grpDatosDiagnostico.Padding = new Padding(12);
            grpDatosDiagnostico.Size = new Size(768, 373);
            grpDatosDiagnostico.TabIndex = 2;
            grpDatosDiagnostico.TabStop = false;
            grpDatosDiagnostico.Text = "Datos del diagnóstico";
            // 
            // tlpDatosDiagnostico
            // 
            tlpDatosDiagnostico.ColumnCount = 2;
            tlpDatosDiagnostico.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
            tlpDatosDiagnostico.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDatosDiagnostico.Controls.Add(lblTecnico, 0, 0);
            tlpDatosDiagnostico.Controls.Add(txtTecnico, 1, 0);
            tlpDatosDiagnostico.Controls.Add(lblFechaDiagnostico, 0, 1);
            tlpDatosDiagnostico.Controls.Add(dtpFechaDiagnostico, 1, 1);
            tlpDatosDiagnostico.Controls.Add(lblDescripcion, 0, 2);
            tlpDatosDiagnostico.Controls.Add(txtDescripcion, 1, 2);
            tlpDatosDiagnostico.Controls.Add(lblObservaciones, 0, 3);
            tlpDatosDiagnostico.Controls.Add(txtObservaciones, 1, 3);
            tlpDatosDiagnostico.Controls.Add(lblResultado, 0, 4);
            tlpDatosDiagnostico.Controls.Add(cboResultado, 1, 4);
            tlpDatosDiagnostico.Dock = DockStyle.Fill;
            tlpDatosDiagnostico.Location = new Point(12, 28);
            tlpDatosDiagnostico.Margin = new Padding(0);
            tlpDatosDiagnostico.Name = "tlpDatosDiagnostico";
            tlpDatosDiagnostico.RowCount = 5;
            tlpDatosDiagnostico.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpDatosDiagnostico.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpDatosDiagnostico.RowStyles.Add(new RowStyle(SizeType.Absolute, 106F));
            tlpDatosDiagnostico.RowStyles.Add(new RowStyle(SizeType.Absolute, 106F));
            tlpDatosDiagnostico.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpDatosDiagnostico.Size = new Size(744, 333);
            tlpDatosDiagnostico.TabIndex = 0;
            // 
            // lblTecnico
            // 
            lblTecnico.Dock = DockStyle.Fill;
            lblTecnico.Font = new Font("Segoe UI", 9F);
            lblTecnico.ForeColor = Color.FromArgb(51, 65, 85);
            lblTecnico.Location = new Point(0, 0);
            lblTecnico.Margin = new Padding(0);
            lblTecnico.Name = "lblTecnico";
            lblTecnico.Size = new Size(155, 42);
            lblTecnico.TabIndex = 0;
            lblTecnico.Text = "Técnico responsable:";
            lblTecnico.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTecnico
            // 
            txtTecnico.BackColor = Color.FromArgb(248, 250, 252);
            txtTecnico.BorderStyle = BorderStyle.FixedSingle;
            txtTecnico.Dock = DockStyle.Fill;
            txtTecnico.ForeColor = Color.FromArgb(51, 65, 85);
            txtTecnico.Location = new Point(159, 4);
            txtTecnico.Margin = new Padding(4);
            txtTecnico.Name = "txtTecnico";
            txtTecnico.ReadOnly = true;
            txtTecnico.Size = new Size(581, 23);
            txtTecnico.TabIndex = 1;
            txtTecnico.Text = "Técnico actual";
            // 
            // lblFechaDiagnostico
            // 
            lblFechaDiagnostico.Dock = DockStyle.Fill;
            lblFechaDiagnostico.Font = new Font("Segoe UI", 9F);
            lblFechaDiagnostico.ForeColor = Color.FromArgb(51, 65, 85);
            lblFechaDiagnostico.Location = new Point(0, 42);
            lblFechaDiagnostico.Margin = new Padding(0);
            lblFechaDiagnostico.Name = "lblFechaDiagnostico";
            lblFechaDiagnostico.Size = new Size(155, 42);
            lblFechaDiagnostico.TabIndex = 2;
            lblFechaDiagnostico.Text = "Fecha del diagnóstico:";
            lblFechaDiagnostico.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpFechaDiagnostico
            // 
            dtpFechaDiagnostico.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpFechaDiagnostico.Dock = DockStyle.Fill;
            dtpFechaDiagnostico.Format = DateTimePickerFormat.Custom;
            dtpFechaDiagnostico.Location = new Point(159, 46);
            dtpFechaDiagnostico.Margin = new Padding(4);
            dtpFechaDiagnostico.Name = "dtpFechaDiagnostico";
            dtpFechaDiagnostico.ShowUpDown = true;
            dtpFechaDiagnostico.Size = new Size(581, 23);
            dtpFechaDiagnostico.TabIndex = 3;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.Font = new Font("Segoe UI", 9F);
            lblDescripcion.ForeColor = Color.FromArgb(51, 65, 85);
            lblDescripcion.Location = new Point(0, 84);
            lblDescripcion.Margin = new Padding(0);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Padding = new Padding(0, 8, 0, 0);
            lblDescripcion.Size = new Size(155, 106);
            lblDescripcion.TabIndex = 4;
            lblDescripcion.Text = "Diagnóstico:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.BackColor = Color.White;
            txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
            txtDescripcion.Dock = DockStyle.Fill;
            txtDescripcion.Font = new Font("Segoe UI", 9F);
            txtDescripcion.ForeColor = Color.FromArgb(51, 65, 85);
            txtDescripcion.Location = new Point(159, 88);
            txtDescripcion.Margin = new Padding(4);
            txtDescripcion.MaxLength = 1000;
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Size = new Size(581, 98);
            txtDescripcion.TabIndex = 5;
            txtDescripcion.TextChanged += CampoValidado_Cambio;
            // 
            // lblObservaciones
            // 
            lblObservaciones.Dock = DockStyle.Fill;
            lblObservaciones.Font = new Font("Segoe UI", 9F);
            lblObservaciones.ForeColor = Color.FromArgb(51, 65, 85);
            lblObservaciones.Location = new Point(0, 190);
            lblObservaciones.Margin = new Padding(0);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Padding = new Padding(0, 8, 0, 0);
            lblObservaciones.Size = new Size(155, 106);
            lblObservaciones.TabIndex = 6;
            lblObservaciones.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            txtObservaciones.BackColor = Color.White;
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Font = new Font("Segoe UI", 9F);
            txtObservaciones.ForeColor = Color.FromArgb(51, 65, 85);
            txtObservaciones.Location = new Point(159, 194);
            txtObservaciones.Margin = new Padding(4);
            txtObservaciones.MaxLength = 1000;
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(581, 98);
            txtObservaciones.TabIndex = 7;
            // 
            // lblResultado
            // 
            lblResultado.Dock = DockStyle.Fill;
            lblResultado.ForeColor = Color.FromArgb(51, 65, 85);
            lblResultado.Location = new Point(0, 296);
            lblResultado.Margin = new Padding(0);
            lblResultado.Name = "lblResultado";
            lblResultado.Size = new Size(155, 42);
            lblResultado.TabIndex = 8;
            lblResultado.Text = "Resultado:";
            lblResultado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboResultado
            // 
            cboResultado.Dock = DockStyle.Top;
            cboResultado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboResultado.FormattingEnabled = true;
            cboResultado.Location = new Point(159, 300);
            cboResultado.Margin = new Padding(4);
            cboResultado.Name = "cboResultado";
            cboResultado.Size = new Size(581, 23);
            cboResultado.TabIndex = 9;
            cboResultado.SelectedIndexChanged += CampoValidado_Cambio;
            // 
            // flpAcciones
            // 
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.FlowDirection = FlowDirection.RightToLeft;
            flpAcciones.Location = new Point(18, 565);
            flpAcciones.Margin = new Padding(0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Padding = new Padding(0, 10, 0, 0);
            flpAcciones.Size = new Size(768, 68);
            flpAcciones.TabIndex = 3;
            flpAcciones.WrapContents = false;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(593, 10);
            btnGuardar.Margin = new Padding(8, 0, 0, 0);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(175, 38);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar diagnóstico";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(475, 10);
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 38);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // FrmRegistrarDiagnostico
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnCancelar;
            ClientSize = new Size(804, 651);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRegistrarDiagnostico";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar diagnóstico";
            FormClosing += FrmRegistrarDiagnostico_FormClosing;
            Load += FrmRegistrarDiagnostico_Load;
            Shown += FrmRegistrarDiagnostico_Shown;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            grpResumenAtencion.ResumeLayout(false);
            tlpResumenAtencion.ResumeLayout(false);
            grpDatosDiagnostico.ResumeLayout(false);
            tlpDatosDiagnostico.ResumeLayout(false);
            tlpDatosDiagnostico.PerformLayout();
            flpAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private TableLayoutPanel tlpEncabezado;
        private Label lblTitulo;
        private Label lblNumeroAtencion;
        private GroupBox grpResumenAtencion;
        private TableLayoutPanel tlpResumenAtencion;
        private Label lblClienteTitulo;
        private Label lblClienteValor;
        private Label lblVehiculoTitulo;
        private Label lblVehiculoValor;
        private Label lblDominioTitulo;
        private Label lblDominioValor;
        private Label lblMotivoTitulo;
        private Label lblMotivoValor;
        private GroupBox grpDatosDiagnostico;
        private TableLayoutPanel tlpDatosDiagnostico;
        private Label lblTecnico;
        private TextBox txtTecnico;
        private Label lblFechaDiagnostico;
        private DateTimePicker dtpFechaDiagnostico;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Label lblResultado;
        private ComboBox cboResultado;
        private FlowLayoutPanel flpAcciones;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
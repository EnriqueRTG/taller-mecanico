namespace Taller.Presentacion.Formularios.Atenciones
{
    partial class FrmRegistrarEntrega
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
            grpDatosEntrega = new GroupBox();
            tlpDatosEntrega = new TableLayoutPanel();
            lblFechaHora = new Label();
            lblRecibe = new Label();
            lblDocumento = new Label();
            lblResponsableEntrega = new Label();
            lblObservaciones = new Label();
            lblConfirmacion = new Label();
            dtpFechaEntrega = new DateTimePicker();
            txtRecibe = new TextBox();
            txtDocumento = new TextBox();
            txtResponsable = new TextBox();
            txtObservaciones = new TextBox();
            chkConfirmarEntrega = new CheckBox();
            grpAtencionSeleccionada = new GroupBox();
            tlpResumen = new TableLayoutPanel();
            lblSaldoValor = new Label();
            lblSaldoPendienteTitulo = new Label();
            lblClienteTitulo = new Label();
            lblVehiculoTitulo = new Label();
            lblDominioTitulo = new Label();
            lblFechaIngresoTitulo = new Label();
            lblClienteValor = new Label();
            lblVehiculoValor = new Label();
            lblDominioValor = new Label();
            lblEstadoPagoValor = new Label();
            pnlEncabezado = new Panel();
            tlpEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            lblNumeroAtencion = new Label();
            pnlAcciones = new Panel();
            tlpAcciones = new TableLayoutPanel();
            flpAcciones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnConfirmarEntrega = new Button();
            tlpPrincipal.SuspendLayout();
            grpDatosEntrega.SuspendLayout();
            tlpDatosEntrega.SuspendLayout();
            grpAtencionSeleccionada.SuspendLayout();
            tlpResumen.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            pnlAcciones.SuspendLayout();
            tlpAcciones.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(grpDatosEntrega, 0, 2);
            tlpPrincipal.Controls.Add(grpAtencionSeleccionada, 0, 1);
            tlpPrincipal.Controls.Add(pnlEncabezado, 0, 0);
            tlpPrincipal.Controls.Add(pnlAcciones, 0, 3);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(20);
            tlpPrincipal.RowCount = 4;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 125F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tlpPrincipal.Size = new Size(744, 584);
            tlpPrincipal.TabIndex = 1;
            // 
            // grpDatosEntrega
            // 
            grpDatosEntrega.BackColor = Color.White;
            grpDatosEntrega.Controls.Add(tlpDatosEntrega);
            grpDatosEntrega.Dock = DockStyle.Fill;
            grpDatosEntrega.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpDatosEntrega.ForeColor = Color.FromArgb(15, 23, 42);
            grpDatosEntrega.Location = new Point(20, 235);
            grpDatosEntrega.Margin = new Padding(0, 0, 0, 10);
            grpDatosEntrega.Name = "grpDatosEntrega";
            grpDatosEntrega.Padding = new Padding(12);
            grpDatosEntrega.Size = new Size(704, 244);
            grpDatosEntrega.TabIndex = 4;
            grpDatosEntrega.TabStop = false;
            grpDatosEntrega.Text = "Datos de la entrega";
            // 
            // tlpDatosEntrega
            // 
            tlpDatosEntrega.ColumnCount = 2;
            tlpDatosEntrega.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpDatosEntrega.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tlpDatosEntrega.Controls.Add(lblFechaHora, 0, 0);
            tlpDatosEntrega.Controls.Add(lblRecibe, 0, 1);
            tlpDatosEntrega.Controls.Add(lblDocumento, 0, 2);
            tlpDatosEntrega.Controls.Add(lblResponsableEntrega, 0, 3);
            tlpDatosEntrega.Controls.Add(lblObservaciones, 0, 4);
            tlpDatosEntrega.Controls.Add(lblConfirmacion, 0, 5);
            tlpDatosEntrega.Controls.Add(dtpFechaEntrega, 1, 0);
            tlpDatosEntrega.Controls.Add(txtRecibe, 1, 1);
            tlpDatosEntrega.Controls.Add(txtDocumento, 1, 2);
            tlpDatosEntrega.Controls.Add(txtResponsable, 1, 3);
            tlpDatosEntrega.Controls.Add(txtObservaciones, 1, 4);
            tlpDatosEntrega.Controls.Add(chkConfirmarEntrega, 1, 5);
            tlpDatosEntrega.Dock = DockStyle.Fill;
            tlpDatosEntrega.Location = new Point(12, 28);
            tlpDatosEntrega.Name = "tlpDatosEntrega";
            tlpDatosEntrega.RowCount = 6;
            tlpDatosEntrega.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpDatosEntrega.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpDatosEntrega.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpDatosEntrega.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpDatosEntrega.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDatosEntrega.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpDatosEntrega.Size = new Size(680, 204);
            tlpDatosEntrega.TabIndex = 0;
            // 
            // lblFechaHora
            // 
            lblFechaHora.AutoSize = true;
            lblFechaHora.Location = new Point(3, 0);
            lblFechaHora.Name = "lblFechaHora";
            lblFechaHora.Size = new Size(79, 15);
            lblFechaHora.TabIndex = 0;
            lblFechaHora.Text = "Fecha y hora:";
            // 
            // lblRecibe
            // 
            lblRecibe.AutoSize = true;
            lblRecibe.Location = new Point(3, 30);
            lblRecibe.Name = "lblRecibe";
            lblRecibe.Size = new Size(80, 15);
            lblRecibe.TabIndex = 1;
            lblRecibe.Text = "Recibido por:";
            // 
            // lblDocumento
            // 
            lblDocumento.AutoSize = true;
            lblDocumento.Location = new Point(3, 60);
            lblDocumento.Name = "lblDocumento";
            lblDocumento.Size = new Size(76, 15);
            lblDocumento.TabIndex = 2;
            lblDocumento.Text = "Documento:";
            // 
            // lblResponsableEntrega
            // 
            lblResponsableEntrega.AutoSize = true;
            lblResponsableEntrega.Location = new Point(3, 90);
            lblResponsableEntrega.Name = "lblResponsableEntrega";
            lblResponsableEntrega.Size = new Size(143, 15);
            lblResponsableEntrega.TabIndex = 3;
            lblResponsableEntrega.Text = "Responsable de entrega:";
            // 
            // lblObservaciones
            // 
            lblObservaciones.AutoSize = true;
            lblObservaciones.Location = new Point(3, 120);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(91, 15);
            lblObservaciones.TabIndex = 4;
            lblObservaciones.Text = "Observaciones:";
            // 
            // lblConfirmacion
            // 
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Location = new Point(3, 174);
            lblConfirmacion.Name = "lblConfirmacion";
            lblConfirmacion.Size = new Size(84, 15);
            lblConfirmacion.TabIndex = 5;
            lblConfirmacion.Text = "Confirmación:";
            // 
            // dtpFechaEntrega
            // 
            dtpFechaEntrega.Dock = DockStyle.Left;
            dtpFechaEntrega.Format = DateTimePickerFormat.Custom;
            dtpFechaEntrega.Location = new Point(173, 3);
            dtpFechaEntrega.Name = "dtpFechaEntrega";
            dtpFechaEntrega.Size = new Size(200, 23);
            dtpFechaEntrega.TabIndex = 6;
            // 
            // txtRecibe
            // 
            txtRecibe.BorderStyle = BorderStyle.FixedSingle;
            txtRecibe.Dock = DockStyle.Fill;
            txtRecibe.Location = new Point(173, 33);
            txtRecibe.Name = "txtRecibe";
            txtRecibe.Size = new Size(504, 23);
            txtRecibe.TabIndex = 7;
            txtRecibe.WordWrap = false;
            // 
            // txtDocumento
            // 
            txtDocumento.BorderStyle = BorderStyle.FixedSingle;
            txtDocumento.Dock = DockStyle.Fill;
            txtDocumento.Location = new Point(173, 63);
            txtDocumento.Name = "txtDocumento";
            txtDocumento.Size = new Size(504, 23);
            txtDocumento.TabIndex = 8;
            txtDocumento.WordWrap = false;
            // 
            // txtResponsable
            // 
            txtResponsable.BorderStyle = BorderStyle.FixedSingle;
            txtResponsable.Dock = DockStyle.Fill;
            txtResponsable.Enabled = false;
            txtResponsable.Location = new Point(173, 93);
            txtResponsable.Name = "txtResponsable";
            txtResponsable.Size = new Size(504, 23);
            txtResponsable.TabIndex = 9;
            txtResponsable.WordWrap = false;
            // 
            // txtObservaciones
            // 
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Location = new Point(173, 123);
            txtObservaciones.MaxLength = 500;
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(504, 48);
            txtObservaciones.TabIndex = 10;
            // 
            // chkConfirmarEntrega
            // 
            chkConfirmarEntrega.AutoSize = true;
            chkConfirmarEntrega.Dock = DockStyle.Fill;
            chkConfirmarEntrega.Location = new Point(173, 177);
            chkConfirmarEntrega.Name = "chkConfirmarEntrega";
            chkConfirmarEntrega.Size = new Size(504, 24);
            chkConfirmarEntrega.TabIndex = 11;
            chkConfirmarEntrega.Text = "Confirmo que el vehículo y la documentación fueron entregados.";
            chkConfirmarEntrega.UseVisualStyleBackColor = true;
            // 
            // grpAtencionSeleccionada
            // 
            grpAtencionSeleccionada.BackColor = Color.White;
            grpAtencionSeleccionada.Controls.Add(tlpResumen);
            grpAtencionSeleccionada.Dock = DockStyle.Fill;
            grpAtencionSeleccionada.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpAtencionSeleccionada.ForeColor = Color.FromArgb(15, 23, 42);
            grpAtencionSeleccionada.Location = new Point(20, 110);
            grpAtencionSeleccionada.Margin = new Padding(0, 0, 0, 10);
            grpAtencionSeleccionada.Name = "grpAtencionSeleccionada";
            grpAtencionSeleccionada.Padding = new Padding(12);
            grpAtencionSeleccionada.Size = new Size(704, 115);
            grpAtencionSeleccionada.TabIndex = 3;
            grpAtencionSeleccionada.TabStop = false;
            grpAtencionSeleccionada.Text = "Atención seleccionada";
            // 
            // tlpResumen
            // 
            tlpResumen.ColumnCount = 5;
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpResumen.Controls.Add(lblSaldoValor, 4, 1);
            tlpResumen.Controls.Add(lblSaldoPendienteTitulo, 4, 0);
            tlpResumen.Controls.Add(lblClienteTitulo, 0, 0);
            tlpResumen.Controls.Add(lblVehiculoTitulo, 1, 0);
            tlpResumen.Controls.Add(lblDominioTitulo, 2, 0);
            tlpResumen.Controls.Add(lblFechaIngresoTitulo, 3, 0);
            tlpResumen.Controls.Add(lblClienteValor, 0, 1);
            tlpResumen.Controls.Add(lblVehiculoValor, 1, 1);
            tlpResumen.Controls.Add(lblDominioValor, 2, 1);
            tlpResumen.Controls.Add(lblEstadoPagoValor, 3, 1);
            tlpResumen.Dock = DockStyle.Fill;
            tlpResumen.Location = new Point(12, 28);
            tlpResumen.Margin = new Padding(0);
            tlpResumen.Name = "tlpResumen";
            tlpResumen.Padding = new Padding(8);
            tlpResumen.RowCount = 2;
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpResumen.Size = new Size(680, 75);
            tlpResumen.TabIndex = 0;
            // 
            // lblSaldoValor
            // 
            lblSaldoValor.AutoSize = true;
            lblSaldoValor.Dock = DockStyle.Fill;
            lblSaldoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblSaldoValor.Location = new Point(544, 38);
            lblSaldoValor.Margin = new Padding(8, 0, 8, 5);
            lblSaldoValor.Name = "lblSaldoValor";
            lblSaldoValor.Size = new Size(120, 24);
            lblSaldoValor.TabIndex = 9;
            lblSaldoValor.Text = "$";
            lblSaldoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSaldoPendienteTitulo
            // 
            lblSaldoPendienteTitulo.AutoSize = true;
            lblSaldoPendienteTitulo.Dock = DockStyle.Fill;
            lblSaldoPendienteTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblSaldoPendienteTitulo.ForeColor = Color.FromArgb(30, 64, 175);
            lblSaldoPendienteTitulo.Location = new Point(544, 13);
            lblSaldoPendienteTitulo.Margin = new Padding(8, 5, 8, 0);
            lblSaldoPendienteTitulo.Name = "lblSaldoPendienteTitulo";
            lblSaldoPendienteTitulo.Size = new Size(120, 25);
            lblSaldoPendienteTitulo.TabIndex = 8;
            lblSaldoPendienteTitulo.Text = "SALDO PENDIENTE";
            lblSaldoPendienteTitulo.TextAlign = ContentAlignment.MiddleLeft;
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
            lblClienteTitulo.Size = new Size(116, 25);
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
            lblVehiculoTitulo.Location = new Point(148, 13);
            lblVehiculoTitulo.Margin = new Padding(8, 5, 8, 0);
            lblVehiculoTitulo.Name = "lblVehiculoTitulo";
            lblVehiculoTitulo.Size = new Size(116, 25);
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
            lblDominioTitulo.Location = new Point(280, 13);
            lblDominioTitulo.Margin = new Padding(8, 5, 8, 0);
            lblDominioTitulo.Name = "lblDominioTitulo";
            lblDominioTitulo.Size = new Size(116, 25);
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
            lblFechaIngresoTitulo.Location = new Point(412, 13);
            lblFechaIngresoTitulo.Margin = new Padding(8, 5, 8, 0);
            lblFechaIngresoTitulo.Name = "lblFechaIngresoTitulo";
            lblFechaIngresoTitulo.Size = new Size(116, 25);
            lblFechaIngresoTitulo.TabIndex = 3;
            lblFechaIngresoTitulo.Text = "ESTADO";
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
            lblClienteValor.Size = new Size(116, 24);
            lblClienteValor.TabIndex = 4;
            lblClienteValor.Text = "Juan Pérez";
            lblClienteValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVehiculoValor
            // 
            lblVehiculoValor.AutoSize = true;
            lblVehiculoValor.Dock = DockStyle.Fill;
            lblVehiculoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblVehiculoValor.Location = new Point(148, 38);
            lblVehiculoValor.Margin = new Padding(8, 0, 8, 5);
            lblVehiculoValor.Name = "lblVehiculoValor";
            lblVehiculoValor.Size = new Size(116, 24);
            lblVehiculoValor.TabIndex = 5;
            lblVehiculoValor.Text = "Honda Fit";
            lblVehiculoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDominioValor
            // 
            lblDominioValor.AutoSize = true;
            lblDominioValor.Dock = DockStyle.Fill;
            lblDominioValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblDominioValor.Location = new Point(280, 38);
            lblDominioValor.Margin = new Padding(8, 0, 8, 5);
            lblDominioValor.Name = "lblDominioValor";
            lblDominioValor.Size = new Size(116, 24);
            lblDominioValor.TabIndex = 6;
            lblDominioValor.Text = "AB345CD";
            lblDominioValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEstadoPagoValor
            // 
            lblEstadoPagoValor.AutoSize = true;
            lblEstadoPagoValor.Dock = DockStyle.Fill;
            lblEstadoPagoValor.ForeColor = Color.FromArgb(51, 65, 85);
            lblEstadoPagoValor.Location = new Point(412, 38);
            lblEstadoPagoValor.Margin = new Padding(8, 0, 8, 5);
            lblEstadoPagoValor.Name = "lblEstadoPagoValor";
            lblEstadoPagoValor.Size = new Size(116, 24);
            lblEstadoPagoValor.TabIndex = 7;
            lblEstadoPagoValor.Text = "Pendiente";
            lblEstadoPagoValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.White;
            pnlEncabezado.Controls.Add(tlpEncabezado);
            pnlEncabezado.Dock = DockStyle.Fill;
            pnlEncabezado.Location = new Point(20, 20);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 10);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(20, 10, 20, 10);
            pnlEncabezado.Size = new Size(704, 80);
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
            tlpEncabezado.Location = new Point(20, 10);
            tlpEncabezado.Margin = new Padding(0);
            tlpEncabezado.Name = "tlpEncabezado";
            tlpEncabezado.RowCount = 2;
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
            tlpEncabezado.Size = new Size(664, 60);
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
            lblTitulo.Size = new Size(464, 34);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registrar entrega del vehículo";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(0, 34);
            lblDescripcion.Margin = new Padding(0);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(464, 26);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Confirme los datos de la persona que recibe el vehículo.";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNumeroAtencion
            // 
            lblNumeroAtencion.Dock = DockStyle.Fill;
            lblNumeroAtencion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumeroAtencion.ForeColor = Color.FromArgb(30, 64, 175);
            lblNumeroAtencion.Location = new Point(464, 0);
            lblNumeroAtencion.Margin = new Padding(0);
            lblNumeroAtencion.Name = "lblNumeroAtencion";
            lblNumeroAtencion.Size = new Size(200, 34);
            lblNumeroAtencion.TabIndex = 2;
            lblNumeroAtencion.Text = "Atención N.º 000001";
            lblNumeroAtencion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.White;
            pnlAcciones.Controls.Add(tlpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(20, 499);
            pnlAcciones.Margin = new Padding(0, 10, 0, 0);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Padding = new Padding(12, 8, 12, 8);
            pnlAcciones.Size = new Size(704, 65);
            pnlAcciones.TabIndex = 2;
            // 
            // tlpAcciones
            // 
            tlpAcciones.ColumnCount = 2;
            tlpAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tlpAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tlpAcciones.Controls.Add(flpAcciones, 1, 0);
            tlpAcciones.Dock = DockStyle.Fill;
            tlpAcciones.Location = new Point(12, 8);
            tlpAcciones.Margin = new Padding(0);
            tlpAcciones.Name = "tlpAcciones";
            tlpAcciones.RowCount = 1;
            tlpAcciones.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAcciones.Size = new Size(680, 49);
            tlpAcciones.TabIndex = 0;
            // 
            // flpAcciones
            // 
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Controls.Add(btnConfirmarEntrega);
            flpAcciones.Dock = DockStyle.Right;
            flpAcciones.Location = new Point(375, 3);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Size = new Size(302, 43);
            flpAcciones.TabIndex = 0;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(3, 3);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.TabIndex = 0;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // btnConfirmarEntrega
            // 
            btnConfirmarEntrega.Location = new Point(129, 3);
            btnConfirmarEntrega.Name = "btnConfirmarEntrega";
            btnConfirmarEntrega.Size = new Size(170, 38);
            btnConfirmarEntrega.TabIndex = 1;
            btnConfirmarEntrega.Text = "Confirmar entrega";
            btnConfirmarEntrega.UseVisualStyleBackColor = true;
            btnConfirmarEntrega.Click += BtnConfirmarEntrega_Click;
            // 
            // FrmRegistrarEntrega
            // 
            AcceptButton = btnConfirmarEntrega;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            CancelButton = btnCancelar;
            ClientSize = new Size(744, 584);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRegistrarEntrega";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar entrega";
            FormClosing += FrmRegistrarEntrega_FormClosing;
            Load += FrmRegistrarEntrega_Load;
            Shown += FrmRegistrarEntrega_Shown;
            tlpPrincipal.ResumeLayout(false);
            grpDatosEntrega.ResumeLayout(false);
            tlpDatosEntrega.ResumeLayout(false);
            tlpDatosEntrega.PerformLayout();
            grpAtencionSeleccionada.ResumeLayout(false);
            tlpResumen.ResumeLayout(false);
            tlpResumen.PerformLayout();
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            pnlAcciones.ResumeLayout(false);
            tlpAcciones.ResumeLayout(false);
            tlpAcciones.PerformLayout();
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
        private Panel pnlAcciones;
        private TableLayoutPanel tlpAcciones;
        private GroupBox grpAtencionSeleccionada;
        private TableLayoutPanel tlpResumen;
        private Label lblClienteTitulo;
        private Label lblVehiculoTitulo;
        private Label lblDominioTitulo;
        private Label lblFechaIngresoTitulo;
        private Label lblClienteValor;
        private Label lblVehiculoValor;
        private Label lblDominioValor;
        private Label lblEstadoPagoValor;
        private Label lblSaldoValor;
        private Label lblSaldoPendienteTitulo;
        private GroupBox grpDatosEntrega;
        private TableLayoutPanel tlpDatosEntrega;
        private Label lblFechaHora;
        private Label lblRecibe;
        private Label lblDocumento;
        private Label lblResponsableEntrega;
        private Label lblObservaciones;
        private Label lblConfirmacion;
        private DateTimePicker dtpFechaEntrega;
        private TextBox txtRecibe;
        private TextBox txtDocumento;
        private TextBox txtResponsable;
        private TextBox txtObservaciones;
        private CheckBox chkConfirmarEntrega;
        private FlowLayoutPanel flpAcciones;
        private Button btnCancelar;
        private Button btnConfirmarEntrega;
    }
}
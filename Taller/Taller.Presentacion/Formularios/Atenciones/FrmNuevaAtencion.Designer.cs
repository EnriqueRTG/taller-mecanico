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
            tlpPrincipal = new TableLayoutPanel();
            pnlEncabezado = new Panel();
            tlpEncabezado = new TableLayoutPanel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            pnlContenido = new Panel();
            tlpContenido = new TableLayoutPanel();
            pnlRecepcion = new Panel();
            tlpRecepcion = new TableLayoutPanel();
            lblSeccionRecepcion = new Label();
            lblUsuarioRecepcion = new Label();
            lblFechaIngreso = new Label();
            pnlCliente = new Panel();
            tlpCliente = new TableLayoutPanel();
            lblSeccionCliente = new Label();
            tlpBusquedaCliente = new TableLayoutPanel();
            lblTipoDocumento = new Label();
            cboTipoDocumento = new ComboBox();
            lblDocumento = new Label();
            txtDocumento = new TextBox();
            btnBuscarCliente = new Button();
            lblTituloClienteSeleccionado = new Label();
            tlpClienteSeleccionado = new TableLayoutPanel();
            lblClienteSeleccionado = new Label();
            btnNuevoCliente = new Button();
            pnlVehiculo = new Panel();
            tlpVehiculo = new TableLayoutPanel();
            lblSeccionVehiculo = new Label();
            tlpBusquedaVehiculo = new TableLayoutPanel();
            lblDominio = new Label();
            txtDominio = new TextBox();
            btnBuscarVehiculo = new Button();
            lblTituloVehiculoSeleccionado = new Label();
            tlpVehiculoSeleccionado = new TableLayoutPanel();
            lblVehiculoSeleccionado = new Label();
            btnNuevoVehiculo = new Button();
            pnlMotivo = new Panel();
            tlpMotivo = new TableLayoutPanel();
            lblSeccionMotivo = new Label();
            lblAyudaMotivo = new Label();
            txtMotivo = new TextBox();
            lblContadorMotivo = new Label();
            pnlAcciones = new Panel();
            flpAcciones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnRegistrar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            pnlContenido.SuspendLayout();
            tlpContenido.SuspendLayout();
            pnlRecepcion.SuspendLayout();
            tlpRecepcion.SuspendLayout();
            pnlCliente.SuspendLayout();
            tlpCliente.SuspendLayout();
            tlpBusquedaCliente.SuspendLayout();
            tlpClienteSeleccionado.SuspendLayout();
            pnlVehiculo.SuspendLayout();
            tlpVehiculo.SuspendLayout();
            tlpBusquedaVehiculo.SuspendLayout();
            tlpVehiculoSeleccionado.SuspendLayout();
            pnlMotivo.SuspendLayout();
            tlpMotivo.SuspendLayout();
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
            tlpPrincipal.Location = new Point(2, 2);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 3;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tlpPrincipal.Size = new Size(860, 647);
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
            pnlEncabezado.Padding = new Padding(24, 16, 24, 12);
            pnlEncabezado.Size = new Size(860, 82);
            pnlEncabezado.TabIndex = 0;
            // 
            // tlpEncabezado
            // 
            tlpEncabezado.ColumnCount = 1;
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpEncabezado.Controls.Add(lblTitulo, 0, 0);
            tlpEncabezado.Controls.Add(lblDescripcion, 0, 1);
            tlpEncabezado.Dock = DockStyle.Fill;
            tlpEncabezado.Location = new Point(24, 16);
            tlpEncabezado.Margin = new Padding(0);
            tlpEncabezado.Name = "tlpEncabezado";
            tlpEncabezado.RowCount = 2;
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tlpEncabezado.Size = new Size(812, 54);
            tlpEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Margin = new Padding(0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(812, 29);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nueva atención";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(0, 29);
            lblDescripcion.Margin = new Padding(0);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(812, 25);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Registre el ingreso del vehículo y el motivo informado por el cliente.";
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
            pnlContenido.Size = new Size(860, 495);
            pnlContenido.TabIndex = 1;
            // 
            // tlpContenido
            // 
            tlpContenido.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tlpContenido.AutoSize = true;
            tlpContenido.ColumnCount = 1;
            tlpContenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContenido.Controls.Add(pnlRecepcion, 0, 0);
            tlpContenido.Controls.Add(pnlCliente, 0, 1);
            tlpContenido.Controls.Add(pnlVehiculo, 0, 2);
            tlpContenido.Controls.Add(pnlMotivo, 0, 3);
            tlpContenido.Location = new Point(24, 16);
            tlpContenido.Margin = new Padding(0);
            tlpContenido.Name = "tlpContenido";
            tlpContenido.RowCount = 4;
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 155F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 155F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            tlpContenido.Size = new Size(812, 600);
            tlpContenido.TabIndex = 0;
            // 
            // pnlRecepcion
            // 
            pnlRecepcion.AutoScroll = true;
            pnlRecepcion.AutoSize = true;
            pnlRecepcion.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlRecepcion.BackColor = Color.White;
            pnlRecepcion.Controls.Add(tlpRecepcion);
            pnlRecepcion.Dock = DockStyle.Fill;
            pnlRecepcion.Location = new Point(0, 0);
            pnlRecepcion.Margin = new Padding(0, 0, 0, 12);
            pnlRecepcion.Name = "pnlRecepcion";
            pnlRecepcion.Padding = new Padding(18, 14, 18, 14);
            pnlRecepcion.Size = new Size(812, 88);
            pnlRecepcion.TabIndex = 0;
            // 
            // tlpRecepcion
            // 
            tlpRecepcion.ColumnCount = 2;
            tlpRecepcion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tlpRecepcion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpRecepcion.Controls.Add(lblSeccionRecepcion, 0, 0);
            tlpRecepcion.Controls.Add(lblUsuarioRecepcion, 0, 1);
            tlpRecepcion.Controls.Add(lblFechaIngreso, 1, 1);
            tlpRecepcion.Dock = DockStyle.Fill;
            tlpRecepcion.Location = new Point(18, 14);
            tlpRecepcion.Margin = new Padding(0);
            tlpRecepcion.Name = "tlpRecepcion";
            tlpRecepcion.RowCount = 2;
            tlpRecepcion.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRecepcion.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpRecepcion.Size = new Size(776, 60);
            tlpRecepcion.TabIndex = 0;
            // 
            // lblSeccionRecepcion
            // 
            tlpRecepcion.SetColumnSpan(lblSeccionRecepcion, 2);
            lblSeccionRecepcion.Dock = DockStyle.Fill;
            lblSeccionRecepcion.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSeccionRecepcion.ForeColor = Color.FromArgb(30, 64, 175);
            lblSeccionRecepcion.Location = new Point(0, 0);
            lblSeccionRecepcion.Margin = new Padding(0);
            lblSeccionRecepcion.Name = "lblSeccionRecepcion";
            lblSeccionRecepcion.Size = new Size(776, 24);
            lblSeccionRecepcion.TabIndex = 0;
            lblSeccionRecepcion.Text = "RECEPCIÓN";
            lblSeccionRecepcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUsuarioRecepcion
            // 
            lblUsuarioRecepcion.AutoEllipsis = true;
            lblUsuarioRecepcion.Dock = DockStyle.Fill;
            lblUsuarioRecepcion.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUsuarioRecepcion.ForeColor = Color.FromArgb(15, 23, 42);
            lblUsuarioRecepcion.Location = new Point(0, 24);
            lblUsuarioRecepcion.Margin = new Padding(0);
            lblUsuarioRecepcion.Name = "lblUsuarioRecepcion";
            lblUsuarioRecepcion.Size = new Size(504, 36);
            lblUsuarioRecepcion.TabIndex = 1;
            lblUsuarioRecepcion.Text = "Usuario de recepción: —";
            lblUsuarioRecepcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFechaIngreso
            // 
            lblFechaIngreso.Dock = DockStyle.Fill;
            lblFechaIngreso.ForeColor = Color.FromArgb(71, 85, 105);
            lblFechaIngreso.Location = new Point(504, 24);
            lblFechaIngreso.Margin = new Padding(0);
            lblFechaIngreso.Name = "lblFechaIngreso";
            lblFechaIngreso.Size = new Size(272, 36);
            lblFechaIngreso.TabIndex = 2;
            lblFechaIngreso.Text = "Fecha de ingreso: —";
            lblFechaIngreso.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlCliente
            // 
            pnlCliente.BackColor = Color.White;
            pnlCliente.Controls.Add(tlpCliente);
            pnlCliente.Dock = DockStyle.Fill;
            pnlCliente.Location = new Point(0, 100);
            pnlCliente.Margin = new Padding(0, 0, 0, 12);
            pnlCliente.Name = "pnlCliente";
            pnlCliente.Padding = new Padding(18, 10, 18, 10);
            pnlCliente.Size = new Size(812, 143);
            pnlCliente.TabIndex = 1;
            // 
            // tlpCliente
            // 
            tlpCliente.ColumnCount = 1;
            tlpCliente.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCliente.Controls.Add(lblSeccionCliente, 0, 0);
            tlpCliente.Controls.Add(tlpBusquedaCliente, 0, 1);
            tlpCliente.Controls.Add(lblTituloClienteSeleccionado, 0, 2);
            tlpCliente.Controls.Add(tlpClienteSeleccionado, 0, 3);
            tlpCliente.Dock = DockStyle.Fill;
            tlpCliente.Location = new Point(18, 10);
            tlpCliente.Margin = new Padding(0);
            tlpCliente.Name = "tlpCliente";
            tlpCliente.RowCount = 4;
            tlpCliente.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpCliente.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpCliente.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpCliente.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCliente.Size = new Size(776, 123);
            tlpCliente.TabIndex = 0;
            // 
            // lblSeccionCliente
            // 
            lblSeccionCliente.Dock = DockStyle.Fill;
            lblSeccionCliente.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSeccionCliente.ForeColor = Color.FromArgb(30, 64, 175);
            lblSeccionCliente.Location = new Point(0, 0);
            lblSeccionCliente.Margin = new Padding(0);
            lblSeccionCliente.Name = "lblSeccionCliente";
            lblSeccionCliente.Size = new Size(776, 22);
            lblSeccionCliente.TabIndex = 0;
            lblSeccionCliente.Text = "1. CLIENTE";
            lblSeccionCliente.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpBusquedaCliente
            // 
            tlpBusquedaCliente.ColumnCount = 6;
            tlpBusquedaCliente.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            tlpBusquedaCliente.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            tlpBusquedaCliente.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            tlpBusquedaCliente.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpBusquedaCliente.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            tlpBusquedaCliente.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            tlpBusquedaCliente.Controls.Add(lblTipoDocumento, 0, 0);
            tlpBusquedaCliente.Controls.Add(cboTipoDocumento, 1, 0);
            tlpBusquedaCliente.Controls.Add(lblDocumento, 2, 0);
            tlpBusquedaCliente.Controls.Add(txtDocumento, 3, 0);
            tlpBusquedaCliente.Controls.Add(btnBuscarCliente, 5, 0);
            tlpBusquedaCliente.Dock = DockStyle.Fill;
            tlpBusquedaCliente.Location = new Point(0, 22);
            tlpBusquedaCliente.Margin = new Padding(0);
            tlpBusquedaCliente.Name = "tlpBusquedaCliente";
            tlpBusquedaCliente.RowCount = 1;
            tlpBusquedaCliente.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBusquedaCliente.Size = new Size(776, 34);
            tlpBusquedaCliente.TabIndex = 1;
            // 
            // lblTipoDocumento
            // 
            lblTipoDocumento.AutoSize = true;
            lblTipoDocumento.Dock = DockStyle.Fill;
            lblTipoDocumento.ForeColor = Color.FromArgb(51, 65, 85);
            lblTipoDocumento.Location = new Point(0, 0);
            lblTipoDocumento.Margin = new Padding(0, 0, 8, 0);
            lblTipoDocumento.Name = "lblTipoDocumento";
            lblTipoDocumento.Size = new Size(117, 34);
            lblTipoDocumento.TabIndex = 0;
            lblTipoDocumento.Text = "Tipo de documento";
            lblTipoDocumento.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboTipoDocumento
            // 
            cboTipoDocumento.Dock = DockStyle.Fill;
            cboTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoDocumento.FormattingEnabled = true;
            cboTipoDocumento.Location = new Point(125, 4);
            cboTipoDocumento.Margin = new Padding(0, 4, 12, 3);
            cboTipoDocumento.Name = "cboTipoDocumento";
            cboTipoDocumento.Size = new Size(133, 23);
            cboTipoDocumento.TabIndex = 0;
            cboTipoDocumento.SelectedIndexChanged += cboTipoDocumento_SelectedIndexChanged;
            // 
            // lblDocumento
            // 
            lblDocumento.AutoSize = true;
            lblDocumento.Dock = DockStyle.Fill;
            lblDocumento.ForeColor = Color.FromArgb(51, 65, 85);
            lblDocumento.Location = new Point(270, 0);
            lblDocumento.Margin = new Padding(0, 0, 8, 0);
            lblDocumento.Name = "lblDocumento";
            lblDocumento.Size = new Size(87, 34);
            lblDocumento.TabIndex = 1;
            lblDocumento.Text = "Documento";
            lblDocumento.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDocumento
            // 
            txtDocumento.Dock = DockStyle.Fill;
            txtDocumento.Location = new Point(365, 4);
            txtDocumento.Margin = new Padding(0, 4, 0, 3);
            txtDocumento.MaxLength = 20;
            txtDocumento.Name = "txtDocumento";
            txtDocumento.Size = new Size(303, 23);
            txtDocumento.TabIndex = 1;
            txtDocumento.TextChanged += txtDocumento_TextChanged;
            // 
            // btnBuscarCliente
            // 
            btnBuscarCliente.Cursor = Cursors.Hand;
            btnBuscarCliente.Dock = DockStyle.Fill;
            btnBuscarCliente.Location = new Point(680, 2);
            btnBuscarCliente.Margin = new Padding(0, 2, 0, 2);
            btnBuscarCliente.Name = "btnBuscarCliente";
            btnBuscarCliente.Size = new Size(96, 30);
            btnBuscarCliente.TabIndex = 2;
            btnBuscarCliente.Text = "Buscar";
            btnBuscarCliente.UseVisualStyleBackColor = false;
            btnBuscarCliente.Click += btnBuscarCliente_Click;
            // 
            // lblTituloClienteSeleccionado
            // 
            lblTituloClienteSeleccionado.Dock = DockStyle.Fill;
            lblTituloClienteSeleccionado.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblTituloClienteSeleccionado.ForeColor = Color.FromArgb(100, 116, 139);
            lblTituloClienteSeleccionado.Location = new Point(0, 56);
            lblTituloClienteSeleccionado.Margin = new Padding(0);
            lblTituloClienteSeleccionado.Name = "lblTituloClienteSeleccionado";
            lblTituloClienteSeleccionado.Size = new Size(776, 22);
            lblTituloClienteSeleccionado.TabIndex = 2;
            lblTituloClienteSeleccionado.Text = "Cliente seleccionado";
            lblTituloClienteSeleccionado.TextAlign = ContentAlignment.BottomLeft;
            // 
            // tlpClienteSeleccionado
            // 
            tlpClienteSeleccionado.ColumnCount = 3;
            tlpClienteSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpClienteSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            tlpClienteSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            tlpClienteSeleccionado.Controls.Add(lblClienteSeleccionado, 0, 0);
            tlpClienteSeleccionado.Controls.Add(btnNuevoCliente, 2, 0);
            tlpClienteSeleccionado.Dock = DockStyle.Fill;
            tlpClienteSeleccionado.Location = new Point(0, 78);
            tlpClienteSeleccionado.Margin = new Padding(0);
            tlpClienteSeleccionado.Name = "tlpClienteSeleccionado";
            tlpClienteSeleccionado.RowCount = 1;
            tlpClienteSeleccionado.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpClienteSeleccionado.Size = new Size(776, 45);
            tlpClienteSeleccionado.TabIndex = 3;
            // 
            // lblClienteSeleccionado
            // 
            lblClienteSeleccionado.AutoEllipsis = true;
            lblClienteSeleccionado.Dock = DockStyle.Fill;
            lblClienteSeleccionado.ForeColor = Color.FromArgb(100, 116, 139);
            lblClienteSeleccionado.Location = new Point(0, 0);
            lblClienteSeleccionado.Margin = new Padding(0);
            lblClienteSeleccionado.Name = "lblClienteSeleccionado";
            lblClienteSeleccionado.Size = new Size(619, 45);
            lblClienteSeleccionado.TabIndex = 0;
            lblClienteSeleccionado.Text = "Todavía no seleccionó un cliente.";
            lblClienteSeleccionado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnNuevoCliente
            // 
            btnNuevoCliente.Cursor = Cursors.Hand;
            btnNuevoCliente.Dock = DockStyle.Fill;
            btnNuevoCliente.Location = new Point(631, 3);
            btnNuevoCliente.Margin = new Padding(0, 3, 0, 3);
            btnNuevoCliente.Name = "btnNuevoCliente";
            btnNuevoCliente.Size = new Size(145, 39);
            btnNuevoCliente.TabIndex = 3;
            btnNuevoCliente.Text = "+ Registrar cliente";
            btnNuevoCliente.UseVisualStyleBackColor = false;
            btnNuevoCliente.Click += btnNuevoCliente_Click;
            // 
            // pnlVehiculo
            // 
            pnlVehiculo.BackColor = Color.White;
            pnlVehiculo.Controls.Add(tlpVehiculo);
            pnlVehiculo.Dock = DockStyle.Fill;
            pnlVehiculo.Location = new Point(0, 255);
            pnlVehiculo.Margin = new Padding(0, 0, 0, 12);
            pnlVehiculo.Name = "pnlVehiculo";
            pnlVehiculo.Padding = new Padding(18, 10, 18, 10);
            pnlVehiculo.Size = new Size(812, 143);
            pnlVehiculo.TabIndex = 2;
            // 
            // tlpVehiculo
            // 
            tlpVehiculo.ColumnCount = 1;
            tlpVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpVehiculo.Controls.Add(lblSeccionVehiculo, 0, 0);
            tlpVehiculo.Controls.Add(tlpBusquedaVehiculo, 0, 1);
            tlpVehiculo.Controls.Add(lblTituloVehiculoSeleccionado, 0, 2);
            tlpVehiculo.Controls.Add(tlpVehiculoSeleccionado, 0, 3);
            tlpVehiculo.Dock = DockStyle.Fill;
            tlpVehiculo.Location = new Point(18, 10);
            tlpVehiculo.Margin = new Padding(0);
            tlpVehiculo.Name = "tlpVehiculo";
            tlpVehiculo.RowCount = 4;
            tlpVehiculo.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpVehiculo.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpVehiculo.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpVehiculo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpVehiculo.Size = new Size(776, 123);
            tlpVehiculo.TabIndex = 0;
            // 
            // lblSeccionVehiculo
            // 
            lblSeccionVehiculo.Dock = DockStyle.Fill;
            lblSeccionVehiculo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSeccionVehiculo.ForeColor = Color.FromArgb(30, 64, 175);
            lblSeccionVehiculo.Location = new Point(0, 0);
            lblSeccionVehiculo.Margin = new Padding(0);
            lblSeccionVehiculo.Name = "lblSeccionVehiculo";
            lblSeccionVehiculo.Size = new Size(776, 22);
            lblSeccionVehiculo.TabIndex = 0;
            lblSeccionVehiculo.Text = "2. VEHÍCULO";
            lblSeccionVehiculo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpBusquedaVehiculo
            // 
            tlpBusquedaVehiculo.ColumnCount = 4;
            tlpBusquedaVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            tlpBusquedaVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpBusquedaVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            tlpBusquedaVehiculo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            tlpBusquedaVehiculo.Controls.Add(lblDominio, 0, 0);
            tlpBusquedaVehiculo.Controls.Add(txtDominio, 1, 0);
            tlpBusquedaVehiculo.Controls.Add(btnBuscarVehiculo, 3, 0);
            tlpBusquedaVehiculo.Dock = DockStyle.Fill;
            tlpBusquedaVehiculo.Location = new Point(0, 22);
            tlpBusquedaVehiculo.Margin = new Padding(0);
            tlpBusquedaVehiculo.Name = "tlpBusquedaVehiculo";
            tlpBusquedaVehiculo.RowCount = 1;
            tlpBusquedaVehiculo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBusquedaVehiculo.Size = new Size(776, 34);
            tlpBusquedaVehiculo.TabIndex = 1;
            // 
            // lblDominio
            // 
            lblDominio.AutoSize = true;
            lblDominio.Dock = DockStyle.Fill;
            lblDominio.ForeColor = Color.FromArgb(51, 65, 85);
            lblDominio.Location = new Point(0, 0);
            lblDominio.Margin = new Padding(0, 0, 8, 0);
            lblDominio.Name = "lblDominio";
            lblDominio.Size = new Size(117, 34);
            lblDominio.TabIndex = 0;
            lblDominio.Text = "Dominio";
            lblDominio.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDominio
            // 
            txtDominio.CharacterCasing = CharacterCasing.Upper;
            txtDominio.Dock = DockStyle.Fill;
            txtDominio.Enabled = false;
            txtDominio.Location = new Point(125, 4);
            txtDominio.Margin = new Padding(0, 4, 0, 3);
            txtDominio.MaxLength = 10;
            txtDominio.Name = "txtDominio";
            txtDominio.Size = new Size(543, 23);
            txtDominio.TabIndex = 4;
            txtDominio.TextChanged += txtDominio_TextChanged;
            // 
            // btnBuscarVehiculo
            // 
            btnBuscarVehiculo.Cursor = Cursors.Hand;
            btnBuscarVehiculo.Dock = DockStyle.Fill;
            btnBuscarVehiculo.Enabled = false;
            btnBuscarVehiculo.Location = new Point(680, 2);
            btnBuscarVehiculo.Margin = new Padding(0, 2, 0, 2);
            btnBuscarVehiculo.Name = "btnBuscarVehiculo";
            btnBuscarVehiculo.Size = new Size(96, 30);
            btnBuscarVehiculo.TabIndex = 5;
            btnBuscarVehiculo.Text = "Buscar";
            btnBuscarVehiculo.UseVisualStyleBackColor = false;
            btnBuscarVehiculo.Click += btnBuscarVehiculo_Click;
            // 
            // lblTituloVehiculoSeleccionado
            // 
            lblTituloVehiculoSeleccionado.Dock = DockStyle.Fill;
            lblTituloVehiculoSeleccionado.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblTituloVehiculoSeleccionado.ForeColor = Color.FromArgb(100, 116, 139);
            lblTituloVehiculoSeleccionado.Location = new Point(0, 56);
            lblTituloVehiculoSeleccionado.Margin = new Padding(0);
            lblTituloVehiculoSeleccionado.Name = "lblTituloVehiculoSeleccionado";
            lblTituloVehiculoSeleccionado.Size = new Size(776, 22);
            lblTituloVehiculoSeleccionado.TabIndex = 2;
            lblTituloVehiculoSeleccionado.Text = "Vehículo seleccionado";
            lblTituloVehiculoSeleccionado.TextAlign = ContentAlignment.BottomLeft;
            // 
            // tlpVehiculoSeleccionado
            // 
            tlpVehiculoSeleccionado.ColumnCount = 3;
            tlpVehiculoSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpVehiculoSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            tlpVehiculoSeleccionado.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            tlpVehiculoSeleccionado.Controls.Add(lblVehiculoSeleccionado, 0, 0);
            tlpVehiculoSeleccionado.Controls.Add(btnNuevoVehiculo, 2, 0);
            tlpVehiculoSeleccionado.Dock = DockStyle.Fill;
            tlpVehiculoSeleccionado.Location = new Point(0, 78);
            tlpVehiculoSeleccionado.Margin = new Padding(0);
            tlpVehiculoSeleccionado.Name = "tlpVehiculoSeleccionado";
            tlpVehiculoSeleccionado.RowCount = 1;
            tlpVehiculoSeleccionado.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpVehiculoSeleccionado.Size = new Size(776, 45);
            tlpVehiculoSeleccionado.TabIndex = 3;
            // 
            // lblVehiculoSeleccionado
            // 
            lblVehiculoSeleccionado.AutoEllipsis = true;
            lblVehiculoSeleccionado.Dock = DockStyle.Fill;
            lblVehiculoSeleccionado.ForeColor = Color.FromArgb(100, 116, 139);
            lblVehiculoSeleccionado.Location = new Point(0, 0);
            lblVehiculoSeleccionado.Margin = new Padding(0);
            lblVehiculoSeleccionado.Name = "lblVehiculoSeleccionado";
            lblVehiculoSeleccionado.Size = new Size(619, 45);
            lblVehiculoSeleccionado.TabIndex = 0;
            lblVehiculoSeleccionado.Text = "Primero seleccione un cliente.";
            lblVehiculoSeleccionado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnNuevoVehiculo
            // 
            btnNuevoVehiculo.Cursor = Cursors.Hand;
            btnNuevoVehiculo.Dock = DockStyle.Fill;
            btnNuevoVehiculo.Enabled = false;
            btnNuevoVehiculo.Location = new Point(631, 3);
            btnNuevoVehiculo.Margin = new Padding(0, 3, 0, 3);
            btnNuevoVehiculo.Name = "btnNuevoVehiculo";
            btnNuevoVehiculo.Size = new Size(145, 39);
            btnNuevoVehiculo.TabIndex = 6;
            btnNuevoVehiculo.Text = "+ Registrar vehículo";
            btnNuevoVehiculo.UseVisualStyleBackColor = false;
            btnNuevoVehiculo.Click += btnNuevoVehiculo_Click;
            // 
            // pnlMotivo
            // 
            pnlMotivo.BackColor = Color.White;
            pnlMotivo.Controls.Add(tlpMotivo);
            pnlMotivo.Dock = DockStyle.Fill;
            pnlMotivo.Location = new Point(0, 410);
            pnlMotivo.Margin = new Padding(0);
            pnlMotivo.Name = "pnlMotivo";
            pnlMotivo.Padding = new Padding(18, 10, 18, 10);
            pnlMotivo.Size = new Size(812, 190);
            pnlMotivo.TabIndex = 3;
            // 
            // tlpMotivo
            // 
            tlpMotivo.ColumnCount = 1;
            tlpMotivo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMotivo.Controls.Add(lblSeccionMotivo, 0, 0);
            tlpMotivo.Controls.Add(lblAyudaMotivo, 0, 1);
            tlpMotivo.Controls.Add(txtMotivo, 0, 2);
            tlpMotivo.Controls.Add(lblContadorMotivo, 0, 3);
            tlpMotivo.Dock = DockStyle.Fill;
            tlpMotivo.Location = new Point(18, 10);
            tlpMotivo.Margin = new Padding(0);
            tlpMotivo.Name = "tlpMotivo";
            tlpMotivo.RowCount = 4;
            tlpMotivo.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpMotivo.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpMotivo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMotivo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpMotivo.Size = new Size(776, 170);
            tlpMotivo.TabIndex = 0;
            // 
            // lblSeccionMotivo
            // 
            lblSeccionMotivo.Dock = DockStyle.Fill;
            lblSeccionMotivo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSeccionMotivo.ForeColor = Color.FromArgb(30, 64, 175);
            lblSeccionMotivo.Location = new Point(0, 0);
            lblSeccionMotivo.Margin = new Padding(0);
            lblSeccionMotivo.Name = "lblSeccionMotivo";
            lblSeccionMotivo.Size = new Size(776, 22);
            lblSeccionMotivo.TabIndex = 0;
            lblSeccionMotivo.Text = "3. MOTIVO DE CONSULTA";
            lblSeccionMotivo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAyudaMotivo
            // 
            lblAyudaMotivo.AutoEllipsis = true;
            lblAyudaMotivo.Dock = DockStyle.Fill;
            lblAyudaMotivo.Font = new Font("Segoe UI", 8.5F);
            lblAyudaMotivo.ForeColor = Color.FromArgb(100, 116, 139);
            lblAyudaMotivo.Location = new Point(0, 22);
            lblAyudaMotivo.Margin = new Padding(0);
            lblAyudaMotivo.Name = "lblAyudaMotivo";
            lblAyudaMotivo.Size = new Size(776, 30);
            lblAyudaMotivo.TabIndex = 1;
            lblAyudaMotivo.Text = "Describa el inconveniente informado por el cliente. El diagnóstico técnico se registrará posteriormente.";
            lblAyudaMotivo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMotivo
            // 
            txtMotivo.AcceptsReturn = true;
            txtMotivo.Dock = DockStyle.Fill;
            txtMotivo.Font = new Font("Segoe UI", 9.5F);
            txtMotivo.Location = new Point(0, 54);
            txtMotivo.Margin = new Padding(0, 2, 0, 2);
            txtMotivo.MaxLength = 500;
            txtMotivo.Multiline = true;
            txtMotivo.Name = "txtMotivo";
            txtMotivo.ScrollBars = ScrollBars.Vertical;
            txtMotivo.Size = new Size(776, 94);
            txtMotivo.TabIndex = 7;
            txtMotivo.TextChanged += txtMotivo_TextChanged;
            // 
            // lblContadorMotivo
            // 
            lblContadorMotivo.Dock = DockStyle.Fill;
            lblContadorMotivo.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblContadorMotivo.ForeColor = Color.FromArgb(100, 116, 139);
            lblContadorMotivo.Location = new Point(0, 150);
            lblContadorMotivo.Margin = new Padding(0);
            lblContadorMotivo.Name = "lblContadorMotivo";
            lblContadorMotivo.Size = new Size(776, 20);
            lblContadorMotivo.TabIndex = 8;
            lblContadorMotivo.Text = "0 / 500";
            lblContadorMotivo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.White;
            pnlAcciones.Controls.Add(flpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(0, 577);
            pnlAcciones.Margin = new Padding(0);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Padding = new Padding(24, 15, 24, 15);
            pnlAcciones.Size = new Size(860, 70);
            pnlAcciones.TabIndex = 2;
            // 
            // flpAcciones
            // 
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Controls.Add(btnRegistrar);
            flpAcciones.Dock = DockStyle.Right;
            flpAcciones.Location = new Point(506, 15);
            flpAcciones.Margin = new Padding(0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Size = new Size(330, 40);
            flpAcciones.TabIndex = 0;
            flpAcciones.WrapContents = false;
            // 
            // btnCancelar
            // 
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(0, 1);
            btnCancelar.Margin = new Padding(0, 1, 10, 1);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnRegistrar
            // 
            btnRegistrar.Cursor = Cursors.Hand;
            btnRegistrar.Enabled = false;
            btnRegistrar.Location = new Point(130, 1);
            btnRegistrar.Margin = new Padding(0, 1, 0, 1);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(170, 38);
            btnRegistrar.TabIndex = 9;
            btnRegistrar.Text = "Registrar atención";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // FrmNuevaAtencion
            // 
            AcceptButton = btnRegistrar;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnCancelar;
            ClientSize = new Size(864, 651);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(820, 650);
            Name = "FrmNuevaAtencion";
            Padding = new Padding(2);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nueva atención";
            Load += FrmNuevaAtencion_Load;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            pnlContenido.ResumeLayout(false);
            pnlContenido.PerformLayout();
            tlpContenido.ResumeLayout(false);
            tlpContenido.PerformLayout();
            pnlRecepcion.ResumeLayout(false);
            tlpRecepcion.ResumeLayout(false);
            pnlCliente.ResumeLayout(false);
            tlpCliente.ResumeLayout(false);
            tlpBusquedaCliente.ResumeLayout(false);
            tlpBusquedaCliente.PerformLayout();
            tlpClienteSeleccionado.ResumeLayout(false);
            pnlVehiculo.ResumeLayout(false);
            tlpVehiculo.ResumeLayout(false);
            tlpBusquedaVehiculo.ResumeLayout(false);
            tlpBusquedaVehiculo.PerformLayout();
            tlpVehiculoSeleccionado.ResumeLayout(false);
            pnlMotivo.ResumeLayout(false);
            tlpMotivo.ResumeLayout(false);
            tlpMotivo.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            flpAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlEncabezado;
        private Panel pnlContenido;
        private TableLayoutPanel tlpContenido;
        private Panel pnlAcciones;
        private FlowLayoutPanel flpAcciones;
        private TableLayoutPanel tlpEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Panel pnlRecepcion;
        private TableLayoutPanel tlpRecepcion;
        private Label lblSeccionRecepcion;
        private Label lblUsuarioRecepcion;
        private Label lblFechaIngreso;
        private Panel pnlCliente;
        private Panel pnlVehiculo;
        private Panel pnlMotivo;
        private TableLayoutPanel tlpCliente;
        private Label lblSeccionCliente;
        private TableLayoutPanel tlpBusquedaCliente;
        private Label lblTipoDocumento;
        private ComboBox cboTipoDocumento;
        private Label lblDocumento;
        private TextBox txtDocumento;
        private Button btnBuscarCliente;
        private Label lblTituloClienteSeleccionado;
        private TableLayoutPanel tlpClienteSeleccionado;
        private Label lblClienteSeleccionado;
        private Button btnNuevoCliente;
        private TableLayoutPanel tlpVehiculo;
        private Label lblSeccionVehiculo;
        private TableLayoutPanel tlpBusquedaVehiculo;
        private Label lblDominio;
        private TextBox txtDominio;
        private Button btnBuscarVehiculo;
        private Label lblTituloVehiculoSeleccionado;
        private TableLayoutPanel tlpVehiculoSeleccionado;
        private Label lblVehiculoSeleccionado;
        private Button btnNuevoVehiculo;
        private TableLayoutPanel tlpMotivo;
        private Label lblSeccionMotivo;
        private Label lblAyudaMotivo;
        private TextBox txtMotivo;
        private Label lblContadorMotivo;
        private Button btnCancelar;
        private Button btnRegistrar;
    }
}
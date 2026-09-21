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
            lblTipoDocumento = new Label();
            lblDocumento = new Label();
            cboTipoDocumento = new ComboBox();
            txtDocumento = new TextBox();
            lblNombre = new Label();
            lblApellido = new Label();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            pnlContacto = new Panel();
            tlpContacto = new TableLayoutPanel();
            lblSeccionContacto = new Label();
            tlpDatosContacto = new TableLayoutPanel();
            lblTelefono = new Label();
            lblEmail = new Label();
            txtTelefono = new TextBox();
            txtEmail = new TextBox();
            tlpDireccion = new TableLayoutPanel();
            lblCalle = new Label();
            lblAltura = new Label();
            txtCalle = new TextBox();
            txtAltura = new TextBox();
            lblAyudaContacto = new Label();
            pnlAcciones = new Panel();
            flpAcciones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            tlpPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            tlpEncabezado.SuspendLayout();
            pnlContenido.SuspendLayout();
            tlpContenido.SuspendLayout();
            pnlIdentificacion.SuspendLayout();
            tlpIdentificacion.SuspendLayout();
            pnlContacto.SuspendLayout();
            tlpContacto.SuspendLayout();
            tlpDatosContacto.SuspendLayout();
            tlpDireccion.SuspendLayout();
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
            tlpPrincipal.Size = new Size(744, 561);
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
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
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
            lblTitulo.Size = new Size(696, 31);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registrar cliente";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(0, 31);
            lblDescripcion.Margin = new Padding(0);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(696, 27);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Complete los datos personales y de contacto del cliente.";
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
            pnlContenido.Size = new Size(744, 409);
            pnlContenido.TabIndex = 1;
            // 
            // tlpContenido
            // 
            tlpContenido.ColumnCount = 1;
            tlpContenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContenido.Controls.Add(pnlIdentificacion, 0, 0);
            tlpContenido.Controls.Add(pnlContacto, 0, 1);
            tlpContenido.Dock = DockStyle.Top;
            tlpContenido.Location = new Point(24, 16);
            tlpContenido.Margin = new Padding(0);
            tlpContenido.Name = "tlpContenido";
            tlpContenido.RowCount = 2;
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 236F));
            tlpContenido.Size = new Size(679, 416);
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
            pnlIdentificacion.Size = new Size(679, 168);
            pnlIdentificacion.TabIndex = 0;
            // 
            // tlpIdentificacion
            // 
            tlpIdentificacion.ColumnCount = 3;
            tlpIdentificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpIdentificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpIdentificacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpIdentificacion.Controls.Add(lblSeccionIdentificacion, 0, 0);
            tlpIdentificacion.Controls.Add(lblTipoDocumento, 0, 1);
            tlpIdentificacion.Controls.Add(lblDocumento, 2, 1);
            tlpIdentificacion.Controls.Add(cboTipoDocumento, 0, 2);
            tlpIdentificacion.Controls.Add(txtDocumento, 2, 2);
            tlpIdentificacion.Controls.Add(lblNombre, 0, 3);
            tlpIdentificacion.Controls.Add(lblApellido, 2, 3);
            tlpIdentificacion.Controls.Add(txtNombre, 0, 4);
            tlpIdentificacion.Controls.Add(txtApellido, 2, 4);
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
            tlpIdentificacion.Size = new Size(643, 144);
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
            lblSeccionIdentificacion.Text = "DATOS DE IDENTIFICACIÓN";
            lblSeccionIdentificacion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTipoDocumento
            // 
            lblTipoDocumento.AutoSize = true;
            lblTipoDocumento.Dock = DockStyle.Fill;
            lblTipoDocumento.ForeColor = Color.FromArgb(51, 65, 85);
            lblTipoDocumento.Location = new Point(0, 24);
            lblTipoDocumento.Margin = new Padding(0);
            lblTipoDocumento.Name = "lblTipoDocumento";
            lblTipoDocumento.Size = new Size(309, 20);
            lblTipoDocumento.TabIndex = 1;
            lblTipoDocumento.Text = "Tipo de documento *";
            lblTipoDocumento.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblDocumento
            // 
            lblDocumento.AutoSize = true;
            lblDocumento.Dock = DockStyle.Fill;
            lblDocumento.ForeColor = Color.FromArgb(51, 65, 85);
            lblDocumento.Location = new Point(333, 24);
            lblDocumento.Margin = new Padding(0);
            lblDocumento.Name = "lblDocumento";
            lblDocumento.Size = new Size(310, 20);
            lblDocumento.TabIndex = 2;
            lblDocumento.Text = "Número de documento *";
            lblDocumento.TextAlign = ContentAlignment.BottomLeft;
            // 
            // cboTipoDocumento
            // 
            cboTipoDocumento.Dock = DockStyle.Fill;
            cboTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoDocumento.FormattingEnabled = true;
            cboTipoDocumento.Location = new Point(0, 47);
            cboTipoDocumento.Margin = new Padding(0, 3, 0, 3);
            cboTipoDocumento.Name = "cboTipoDocumento";
            cboTipoDocumento.Size = new Size(309, 23);
            cboTipoDocumento.TabIndex = 0;
            cboTipoDocumento.SelectedIndexChanged += CamposObligatorios_TextChanged;
            // 
            // txtDocumento
            // 
            txtDocumento.Dock = DockStyle.Fill;
            txtDocumento.Location = new Point(333, 47);
            txtDocumento.Margin = new Padding(0, 3, 0, 3);
            txtDocumento.MaxLength = 20;
            txtDocumento.Name = "txtDocumento";
            txtDocumento.Size = new Size(310, 23);
            txtDocumento.TabIndex = 1;
            txtDocumento.TextChanged += CamposObligatorios_TextChanged;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Dock = DockStyle.Fill;
            lblNombre.ForeColor = Color.FromArgb(51, 65, 85);
            lblNombre.Location = new Point(0, 78);
            lblNombre.Margin = new Padding(0);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(309, 20);
            lblNombre.TabIndex = 3;
            lblNombre.Text = "Nombre *";
            lblNombre.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Dock = DockStyle.Fill;
            lblApellido.ForeColor = Color.FromArgb(51, 65, 85);
            lblApellido.Location = new Point(333, 78);
            lblApellido.Margin = new Padding(0);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(310, 20);
            lblApellido.TabIndex = 4;
            lblApellido.Text = "Apellido *";
            lblApellido.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtNombre
            // 
            txtNombre.Dock = DockStyle.Top;
            txtNombre.Location = new Point(0, 101);
            txtNombre.Margin = new Padding(0, 3, 0, 0);
            txtNombre.MaxLength = 80;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(309, 23);
            txtNombre.TabIndex = 2;
            txtNombre.TextChanged += CamposObligatorios_TextChanged;
            // 
            // txtApellido
            // 
            txtApellido.Dock = DockStyle.Top;
            txtApellido.Location = new Point(333, 101);
            txtApellido.Margin = new Padding(0, 3, 0, 0);
            txtApellido.MaxLength = 80;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(310, 23);
            txtApellido.TabIndex = 3;
            txtApellido.TextChanged += CamposObligatorios_TextChanged;
            // 
            // pnlContacto
            // 
            pnlContacto.BackColor = Color.White;
            pnlContacto.Controls.Add(tlpContacto);
            pnlContacto.Dock = DockStyle.Fill;
            pnlContacto.Location = new Point(0, 180);
            pnlContacto.Margin = new Padding(0);
            pnlContacto.Name = "pnlContacto";
            pnlContacto.Padding = new Padding(18, 12, 18, 12);
            pnlContacto.Size = new Size(679, 236);
            pnlContacto.TabIndex = 1;
            // 
            // tlpContacto
            // 
            tlpContacto.ColumnCount = 1;
            tlpContacto.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContacto.Controls.Add(lblSeccionContacto, 0, 0);
            tlpContacto.Controls.Add(tlpDatosContacto, 0, 1);
            tlpContacto.Controls.Add(tlpDireccion, 0, 2);
            tlpContacto.Controls.Add(lblAyudaContacto, 0, 3);
            tlpContacto.Dock = DockStyle.Fill;
            tlpContacto.Location = new Point(18, 12);
            tlpContacto.Margin = new Padding(0);
            tlpContacto.Name = "tlpContacto";
            tlpContacto.RowCount = 4;
            tlpContacto.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpContacto.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            tlpContacto.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            tlpContacto.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpContacto.Size = new Size(643, 212);
            tlpContacto.TabIndex = 0;
            // 
            // lblSeccionContacto
            // 
            lblSeccionContacto.Dock = DockStyle.Fill;
            lblSeccionContacto.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSeccionContacto.ForeColor = Color.FromArgb(30, 64, 175);
            lblSeccionContacto.Location = new Point(0, 0);
            lblSeccionContacto.Margin = new Padding(0);
            lblSeccionContacto.Name = "lblSeccionContacto";
            lblSeccionContacto.Size = new Size(643, 24);
            lblSeccionContacto.TabIndex = 0;
            lblSeccionContacto.Text = "DATOS DE CONTACTO";
            lblSeccionContacto.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpDatosContacto
            // 
            tlpDatosContacto.ColumnCount = 3;
            tlpDatosContacto.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpDatosContacto.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpDatosContacto.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpDatosContacto.Controls.Add(lblTelefono, 0, 0);
            tlpDatosContacto.Controls.Add(lblEmail, 2, 0);
            tlpDatosContacto.Controls.Add(txtTelefono, 0, 1);
            tlpDatosContacto.Controls.Add(txtEmail, 2, 1);
            tlpDatosContacto.Dock = DockStyle.Fill;
            tlpDatosContacto.Location = new Point(0, 24);
            tlpDatosContacto.Margin = new Padding(0);
            tlpDatosContacto.Name = "tlpDatosContacto";
            tlpDatosContacto.RowCount = 2;
            tlpDatosContacto.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpDatosContacto.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDatosContacto.Size = new Size(643, 62);
            tlpDatosContacto.TabIndex = 1;
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Dock = DockStyle.Fill;
            lblTelefono.ForeColor = Color.FromArgb(51, 65, 85);
            lblTelefono.Location = new Point(0, 0);
            lblTelefono.Margin = new Padding(0);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(309, 22);
            lblTelefono.TabIndex = 0;
            lblTelefono.Text = "Teléfono";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Dock = DockStyle.Fill;
            lblEmail.ForeColor = Color.FromArgb(51, 65, 85);
            lblEmail.Location = new Point(333, 0);
            lblEmail.Margin = new Padding(0);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(310, 22);
            lblEmail.TabIndex = 1;
            lblEmail.Text = "Correo electrónico";
            lblEmail.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtTelefono
            // 
            txtTelefono.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTelefono.Location = new Point(0, 25);
            txtTelefono.Margin = new Padding(0, 3, 0, 0);
            txtTelefono.MaxLength = 30;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(309, 23);
            txtTelefono.TabIndex = 4;
            // 
            // txtEmail
            // 
            txtEmail.Dock = DockStyle.Top;
            txtEmail.Location = new Point(333, 25);
            txtEmail.Margin = new Padding(0, 3, 0, 0);
            txtEmail.MaxLength = 100;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(310, 23);
            txtEmail.TabIndex = 5;
            // 
            // tlpDireccion
            // 
            tlpDireccion.ColumnCount = 3;
            tlpDireccion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tlpDireccion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpDireccion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpDireccion.Controls.Add(lblCalle, 0, 0);
            tlpDireccion.Controls.Add(lblAltura, 2, 0);
            tlpDireccion.Controls.Add(txtCalle, 0, 1);
            tlpDireccion.Controls.Add(txtAltura, 2, 1);
            tlpDireccion.Dock = DockStyle.Fill;
            tlpDireccion.Location = new Point(0, 86);
            tlpDireccion.Margin = new Padding(0);
            tlpDireccion.Name = "tlpDireccion";
            tlpDireccion.RowCount = 2;
            tlpDireccion.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpDireccion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDireccion.Size = new Size(643, 62);
            tlpDireccion.TabIndex = 2;
            // 
            // lblCalle
            // 
            lblCalle.AutoSize = true;
            lblCalle.Dock = DockStyle.Fill;
            lblCalle.ForeColor = Color.FromArgb(51, 65, 85);
            lblCalle.Location = new Point(0, 0);
            lblCalle.Margin = new Padding(0);
            lblCalle.Name = "lblCalle";
            lblCalle.Size = new Size(464, 22);
            lblCalle.TabIndex = 0;
            lblCalle.Text = "Calle";
            lblCalle.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblAltura
            // 
            lblAltura.AutoSize = true;
            lblAltura.Dock = DockStyle.Fill;
            lblAltura.ForeColor = Color.FromArgb(51, 65, 85);
            lblAltura.Location = new Point(491, 0);
            lblAltura.Name = "lblAltura";
            lblAltura.Size = new Size(149, 22);
            lblAltura.TabIndex = 1;
            lblAltura.Text = "Altura";
            lblAltura.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtCalle
            // 
            txtCalle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCalle.Location = new Point(0, 25);
            txtCalle.Margin = new Padding(0, 3, 0, 0);
            txtCalle.MaxLength = 100;
            txtCalle.Name = "txtCalle";
            txtCalle.Size = new Size(464, 23);
            txtCalle.TabIndex = 6;
            // 
            // txtAltura
            // 
            txtAltura.Dock = DockStyle.Top;
            txtAltura.Location = new Point(488, 25);
            txtAltura.Margin = new Padding(0, 3, 0, 0);
            txtAltura.MaxLength = 10;
            txtAltura.Name = "txtAltura";
            txtAltura.Size = new Size(155, 23);
            txtAltura.TabIndex = 7;
            // 
            // lblAyudaContacto
            // 
            lblAyudaContacto.AutoEllipsis = true;
            lblAyudaContacto.Dock = DockStyle.Fill;
            lblAyudaContacto.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblAyudaContacto.ForeColor = Color.FromArgb(100, 116, 139);
            lblAyudaContacto.Location = new Point(0, 148);
            lblAyudaContacto.Margin = new Padding(0);
            lblAyudaContacto.Name = "lblAyudaContacto";
            lblAyudaContacto.Size = new Size(643, 64);
            lblAyudaContacto.TabIndex = 3;
            lblAyudaContacto.Text = "Los datos de contacto y domicilio son opcionales. Si informa el domicilio, complete la calle y la altura.";
            lblAyudaContacto.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.White;
            pnlAcciones.Controls.Add(flpAcciones);
            pnlAcciones.Dock = DockStyle.Fill;
            pnlAcciones.Location = new Point(0, 491);
            pnlAcciones.Margin = new Padding(0);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Padding = new Padding(24, 15, 24, 15);
            pnlAcciones.Size = new Size(744, 70);
            pnlAcciones.TabIndex = 2;
            // 
            // flpAcciones
            // 
            flpAcciones.Controls.Add(btnCancelar);
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Dock = DockStyle.Right;
            flpAcciones.Location = new Point(390, 15);
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
            // btnGuardar
            // 
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.Location = new Point(130, 1);
            btnGuardar.Margin = new Padding(0, 1, 0, 1);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(170, 38);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Registrar cliente";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // FrmAltaCliente
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnCancelar;
            ClientSize = new Size(744, 561);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MaximumSize = new Size(760, 600);
            MinimizeBox = false;
            Name = "FrmAltaCliente";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar cliente";
            Load += FrmAltaCliente_Load;
            tlpPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            tlpEncabezado.ResumeLayout(false);
            pnlContenido.ResumeLayout(false);
            tlpContenido.ResumeLayout(false);
            pnlIdentificacion.ResumeLayout(false);
            tlpIdentificacion.ResumeLayout(false);
            tlpIdentificacion.PerformLayout();
            pnlContacto.ResumeLayout(false);
            tlpContacto.ResumeLayout(false);
            tlpDatosContacto.ResumeLayout(false);
            tlpDatosContacto.PerformLayout();
            tlpDireccion.ResumeLayout(false);
            tlpDireccion.PerformLayout();
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
        private Panel pnlContacto;
        private FlowLayoutPanel flpAcciones;
        private TableLayoutPanel tlpIdentificacion;
        private Label lblSeccionIdentificacion;
        private Label lblTipoDocumento;
        private Label lblDocumento;
        private ComboBox cboTipoDocumento;
        private TextBox txtDocumento;
        private Label lblNombre;
        private Label lblApellido;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TableLayoutPanel tlpContacto;
        private Label lblSeccionContacto;
        private TableLayoutPanel tlpDatosContacto;
        private Label lblTelefono;
        private Label lblEmail;
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private TableLayoutPanel tlpDireccion;
        private Label lblCalle;
        private Label lblAltura;
        private TextBox txtCalle;
        private TextBox txtAltura;
        private Label lblAyudaContacto;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}
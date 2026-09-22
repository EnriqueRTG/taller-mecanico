namespace Taller.Presentacion.Formularios.Atenciones
{
    partial class FrmAtenciones
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            tlpPrincipal = new TableLayoutPanel();
            pnlCabecera = new Panel();
            btnNuevaAtencion = new Button();
            lblDescripcion = new Label();
            lblTitulo = new Label();
            pnlFiltros = new Panel();
            tlpFiltros = new TableLayoutPanel();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            lblEstado = new Label();
            cboEstado = new ComboBox();
            lblDesde = new Label();
            lblHasta = new Label();
            dtpDesde = new DateTimePicker();
            dtpHasta = new DateTimePicker();
            btnActualizar = new Button();
            lblEspacioActualizar = new Label();
            pnlGrilla = new Panel();
            dgvAtenciones = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colFechaApertura = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colMotivo = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colEtapa = new DataGridViewTextBoxColumn();
            colRecepcion = new DataGridViewTextBoxColumn();
            pnlPie = new Panel();
            tlpPie = new TableLayoutPanel();
            flpAcciones = new FlowLayoutPanel();
            btnVerDetalle = new Button();
            btnEditar = new Button();
            btnIniciarProceso = new Button();
            btnCancelarAtencion = new Button();
            btnRegistrarEntrega = new Button();
            btnVerHistorial = new Button();
            btnRegistrarDiagnostico = new Button();
            btnFinalizarTrabajo = new Button();
            lblCantidad = new Label();
            pnlInformacionRol = new Panel();
            lblInformacionRol = new Label();
            pnlIndicador = new Panel();
            tlpPrincipal.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            tlpFiltros.SuspendLayout();
            pnlGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAtenciones).BeginInit();
            pnlPie.SuspendLayout();
            tlpPie.SuspendLayout();
            flpAcciones.SuspendLayout();
            pnlInformacionRol.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.BackColor = Color.Transparent;
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
            tlpPrincipal.Controls.Add(pnlGrilla, 0, 2);
            tlpPrincipal.Controls.Add(pnlPie, 0, 3);
            tlpPrincipal.Controls.Add(pnlInformacionRol, 0, 4);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(24, 20);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 5;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpPrincipal.Size = new Size(752, 460);
            tlpPrincipal.TabIndex = 0;
            // 
            // pnlCabecera
            // 
            pnlCabecera.BackColor = Color.White;
            pnlCabecera.Controls.Add(btnNuevaAtencion);
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Margin = new Padding(0, 0, 0, 10);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Padding = new Padding(18, 8, 18, 8);
            pnlCabecera.Size = new Size(752, 65);
            pnlCabecera.TabIndex = 0;
            // 
            // btnNuevaAtencion
            // 
            btnNuevaAtencion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevaAtencion.FlatStyle = FlatStyle.Flat;
            btnNuevaAtencion.Location = new Point(533, 11);
            btnNuevaAtencion.Name = "btnNuevaAtencion";
            btnNuevaAtencion.Size = new Size(170, 40);
            btnNuevaAtencion.TabIndex = 0;
            btnNuevaAtencion.Text = "＋ Nueva atención";
            btnNuevaAtencion.UseVisualStyleBackColor = true;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(20, 42);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(358, 17);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Registro y seguimiento de los vehículos ingresados al taller.";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(18, 8);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(229, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de atenciones";
            // 
            // pnlFiltros
            // 
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.Controls.Add(tlpFiltros);
            pnlFiltros.Dock = DockStyle.Fill;
            pnlFiltros.Location = new Point(0, 75);
            pnlFiltros.Margin = new Padding(0, 0, 0, 10);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(18, 8, 18, 8);
            pnlFiltros.Size = new Size(752, 75);
            pnlFiltros.TabIndex = 1;
            // 
            // tlpFiltros
            // 
            tlpFiltros.ColumnCount = 5;
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            tlpFiltros.Controls.Add(lblBuscar, 0, 0);
            tlpFiltros.Controls.Add(txtBuscar, 0, 1);
            tlpFiltros.Controls.Add(lblEstado, 1, 0);
            tlpFiltros.Controls.Add(cboEstado, 1, 1);
            tlpFiltros.Controls.Add(lblDesde, 2, 0);
            tlpFiltros.Controls.Add(lblHasta, 3, 0);
            tlpFiltros.Controls.Add(dtpDesde, 2, 1);
            tlpFiltros.Controls.Add(dtpHasta, 3, 1);
            tlpFiltros.Controls.Add(btnActualizar, 4, 1);
            tlpFiltros.Controls.Add(lblEspacioActualizar, 4, 0);
            tlpFiltros.Dock = DockStyle.Fill;
            tlpFiltros.Location = new Point(18, 8);
            tlpFiltros.Margin = new Padding(0);
            tlpFiltros.Name = "tlpFiltros";
            tlpFiltros.RowCount = 2;
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFiltros.Size = new Size(716, 59);
            tlpFiltros.TabIndex = 0;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Dock = DockStyle.Fill;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBuscar.Location = new Point(5, 0);
            lblBuscar.Margin = new Padding(5, 0, 5, 0);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(233, 22);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar";
            lblBuscar.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtBuscar
            // 
            txtBuscar.Dock = DockStyle.Fill;
            txtBuscar.Font = new Font("Segoe UI", 9.75F);
            txtBuscar.Location = new Point(5, 22);
            txtBuscar.Margin = new Padding(5, 0, 5, 5);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "N.º, dominio, cliente o documento";
            txtBuscar.Size = new Size(233, 25);
            txtBuscar.TabIndex = 1;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Dock = DockStyle.Fill;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEstado.Location = new Point(248, 0);
            lblEstado.Margin = new Padding(5, 0, 5, 0);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(118, 22);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado";
            lblEstado.TextAlign = ContentAlignment.BottomLeft;
            // 
            // cboEstado
            // 
            cboEstado.Dock = DockStyle.Fill;
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Font = new Font("Segoe UI", 9.75F);
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(248, 22);
            cboEstado.Margin = new Padding(5, 0, 5, 5);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(118, 25);
            cboEstado.TabIndex = 3;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Dock = DockStyle.Fill;
            lblDesde.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDesde.Location = new Point(376, 0);
            lblDesde.Margin = new Padding(5, 0, 5, 0);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(118, 22);
            lblDesde.TabIndex = 4;
            lblDesde.Text = "Desde";
            lblDesde.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Dock = DockStyle.Fill;
            lblHasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblHasta.Location = new Point(504, 0);
            lblHasta.Margin = new Padding(5, 0, 5, 0);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(118, 22);
            lblHasta.TabIndex = 5;
            lblHasta.Text = "Hasta";
            lblHasta.TextAlign = ContentAlignment.BottomLeft;
            // 
            // dtpDesde
            // 
            dtpDesde.Dock = DockStyle.Fill;
            dtpDesde.Font = new Font("Segoe UI", 9.75F);
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(376, 22);
            dtpDesde.Margin = new Padding(5, 0, 5, 5);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(118, 25);
            dtpDesde.TabIndex = 7;
            // 
            // dtpHasta
            // 
            dtpHasta.Dock = DockStyle.Fill;
            dtpHasta.Font = new Font("Segoe UI", 9.75F);
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(504, 22);
            dtpHasta.Margin = new Padding(5, 0, 5, 5);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(118, 25);
            dtpHasta.TabIndex = 8;
            // 
            // btnActualizar
            // 
            btnActualizar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnActualizar.Location = new Point(635, 24);
            btnActualizar.Margin = new Padding(8, 0, 0, 5);
            btnActualizar.MaximumSize = new Size(0, 32);
            btnActualizar.MinimumSize = new Size(120, 30);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(120, 30);
            btnActualizar.TabIndex = 9;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // lblEspacioActualizar
            // 
            lblEspacioActualizar.AutoSize = true;
            lblEspacioActualizar.Dock = DockStyle.Fill;
            lblEspacioActualizar.Location = new Point(627, 0);
            lblEspacioActualizar.Margin = new Padding(0);
            lblEspacioActualizar.Name = "lblEspacioActualizar";
            lblEspacioActualizar.Size = new Size(89, 22);
            lblEspacioActualizar.TabIndex = 10;
            // 
            // pnlGrilla
            // 
            pnlGrilla.BackColor = Color.White;
            pnlGrilla.Controls.Add(dgvAtenciones);
            pnlGrilla.Dock = DockStyle.Fill;
            pnlGrilla.Location = new Point(0, 160);
            pnlGrilla.Margin = new Padding(0);
            pnlGrilla.Name = "pnlGrilla";
            pnlGrilla.Padding = new Padding(1);
            pnlGrilla.Size = new Size(752, 177);
            pnlGrilla.TabIndex = 2;
            // 
            // dgvAtenciones
            // 
            dgvAtenciones.AllowUserToAddRows = false;
            dgvAtenciones.AllowUserToDeleteRows = false;
            dgvAtenciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAtenciones.Columns.AddRange(new DataGridViewColumn[] { colId, colFechaApertura, colDominio, colVehiculo, colCliente, colMotivo, colEstado, colEtapa, colRecepcion });
            dgvAtenciones.Dock = DockStyle.Fill;
            dgvAtenciones.Location = new Point(1, 1);
            dgvAtenciones.MultiSelect = false;
            dgvAtenciones.Name = "dgvAtenciones";
            dgvAtenciones.ReadOnly = true;
            dgvAtenciones.RowHeadersVisible = false;
            dgvAtenciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAtenciones.Size = new Size(750, 175);
            dgvAtenciones.TabIndex = 0;
            // 
            // colId
            // 
            colId.DataPropertyName = "Numero";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            colId.DefaultCellStyle = dataGridViewCellStyle1;
            colId.HeaderText = "N.º";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 65;
            // 
            // colFechaApertura
            // 
            colFechaApertura.DataPropertyName = "FechaApertura";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colFechaApertura.DefaultCellStyle = dataGridViewCellStyle2;
            colFechaApertura.HeaderText = "Ingreso";
            colFechaApertura.Name = "colFechaApertura";
            colFechaApertura.ReadOnly = true;
            colFechaApertura.Width = 125;
            // 
            // colDominio
            // 
            colDominio.DataPropertyName = "Dominio";
            colDominio.HeaderText = "Dominio";
            colDominio.Name = "colDominio";
            colDominio.ReadOnly = true;
            // 
            // colVehiculo
            // 
            colVehiculo.DataPropertyName = "Vehiculo";
            colVehiculo.HeaderText = "Vehículo";
            colVehiculo.Name = "colVehiculo";
            colVehiculo.ReadOnly = true;
            colVehiculo.Width = 135;
            // 
            // colCliente
            // 
            colCliente.DataPropertyName = "Cliente";
            colCliente.HeaderText = "Cliente";
            colCliente.Name = "colCliente";
            colCliente.ReadOnly = true;
            colCliente.Width = 135;
            // 
            // colMotivo
            // 
            colMotivo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colMotivo.DataPropertyName = "Motivo";
            colMotivo.HeaderText = "Motivo";
            colMotivo.MinimumWidth = 220;
            colMotivo.Name = "colMotivo";
            colMotivo.ReadOnly = true;
            // 
            // colEstado
            // 
            colEstado.DataPropertyName = "Estado";
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            // 
            // colEtapa
            // 
            colEtapa.DataPropertyName = "Etapa";
            colEtapa.HeaderText = "Etapa actua";
            colEtapa.Name = "colEtapa";
            colEtapa.ReadOnly = true;
            colEtapa.Width = 150;
            // 
            // colRecepcion
            // 
            colRecepcion.DataPropertyName = "Recepcion";
            colRecepcion.HeaderText = "Recepción";
            colRecepcion.Name = "colRecepcion";
            colRecepcion.ReadOnly = true;
            colRecepcion.Width = 130;
            // 
            // pnlPie
            // 
            pnlPie.BackColor = Color.White;
            pnlPie.Controls.Add(tlpPie);
            pnlPie.Dock = DockStyle.Fill;
            pnlPie.Location = new Point(0, 347);
            pnlPie.Margin = new Padding(0, 10, 0, 0);
            pnlPie.Name = "pnlPie";
            pnlPie.Padding = new Padding(12, 10, 12, 10);
            pnlPie.Size = new Size(752, 65);
            pnlPie.TabIndex = 3;
            // 
            // tlpPie
            // 
            tlpPie.ColumnCount = 2;
            tlpPie.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpPie.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tlpPie.Controls.Add(flpAcciones, 1, 0);
            tlpPie.Controls.Add(lblCantidad, 0, 0);
            tlpPie.Dock = DockStyle.Fill;
            tlpPie.Location = new Point(12, 10);
            tlpPie.Margin = new Padding(0);
            tlpPie.Name = "tlpPie";
            tlpPie.RowCount = 1;
            tlpPie.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPie.Size = new Size(728, 45);
            tlpPie.TabIndex = 0;
            // 
            // flpAcciones
            // 
            flpAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnVerDetalle);
            flpAcciones.Controls.Add(btnEditar);
            flpAcciones.Controls.Add(btnIniciarProceso);
            flpAcciones.Controls.Add(btnCancelarAtencion);
            flpAcciones.Controls.Add(btnRegistrarEntrega);
            flpAcciones.Controls.Add(btnVerHistorial);
            flpAcciones.Controls.Add(btnRegistrarDiagnostico);
            flpAcciones.Controls.Add(btnFinalizarTrabajo);
            flpAcciones.Location = new Point(182, 0);
            flpAcciones.Margin = new Padding(0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Padding = new Padding(0, 2, 0, 2);
            flpAcciones.Size = new Size(546, 38);
            flpAcciones.TabIndex = 4;
            flpAcciones.WrapContents = false;
            // 
            // btnVerDetalle
            // 
            btnVerDetalle.AutoSize = true;
            btnVerDetalle.Location = new Point(4, 2);
            btnVerDetalle.Margin = new Padding(4, 0, 4, 0);
            btnVerDetalle.MinimumSize = new Size(105, 34);
            btnVerDetalle.Name = "btnVerDetalle";
            btnVerDetalle.Size = new Size(105, 34);
            btnVerDetalle.TabIndex = 0;
            btnVerDetalle.Text = "Ver detalle";
            btnVerDetalle.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            btnEditar.AutoSize = true;
            btnEditar.Location = new Point(117, 2);
            btnEditar.Margin = new Padding(4, 0, 4, 0);
            btnEditar.MinimumSize = new Size(105, 34);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(105, 34);
            btnEditar.TabIndex = 1;
            btnEditar.Text = "Editar datos";
            btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnIniciarProceso
            // 
            btnIniciarProceso.AutoSize = true;
            btnIniciarProceso.Location = new Point(230, 2);
            btnIniciarProceso.Margin = new Padding(4, 0, 4, 0);
            btnIniciarProceso.MinimumSize = new Size(105, 34);
            btnIniciarProceso.Name = "btnIniciarProceso";
            btnIniciarProceso.Size = new Size(105, 34);
            btnIniciarProceso.TabIndex = 2;
            btnIniciarProceso.Text = "Iniciar proceso";
            btnIniciarProceso.UseVisualStyleBackColor = true;
            // 
            // btnCancelarAtencion
            // 
            btnCancelarAtencion.AutoSize = true;
            btnCancelarAtencion.Location = new Point(343, 2);
            btnCancelarAtencion.Margin = new Padding(4, 0, 4, 0);
            btnCancelarAtencion.MinimumSize = new Size(105, 34);
            btnCancelarAtencion.Name = "btnCancelarAtencion";
            btnCancelarAtencion.Size = new Size(105, 34);
            btnCancelarAtencion.TabIndex = 3;
            btnCancelarAtencion.Text = "Cancelar";
            btnCancelarAtencion.UseVisualStyleBackColor = true;
            // 
            // btnRegistrarEntrega
            // 
            btnRegistrarEntrega.AutoSize = true;
            btnRegistrarEntrega.Location = new Point(456, 2);
            btnRegistrarEntrega.Margin = new Padding(4, 0, 4, 0);
            btnRegistrarEntrega.MinimumSize = new Size(105, 34);
            btnRegistrarEntrega.Name = "btnRegistrarEntrega";
            btnRegistrarEntrega.Size = new Size(106, 34);
            btnRegistrarEntrega.TabIndex = 4;
            btnRegistrarEntrega.Text = "Registrar entrega";
            btnRegistrarEntrega.UseVisualStyleBackColor = true;
            // 
            // btnVerHistorial
            // 
            btnVerHistorial.AutoSize = true;
            btnVerHistorial.Location = new Point(570, 2);
            btnVerHistorial.Margin = new Padding(4, 0, 4, 0);
            btnVerHistorial.MinimumSize = new Size(105, 34);
            btnVerHistorial.Name = "btnVerHistorial";
            btnVerHistorial.Size = new Size(105, 34);
            btnVerHistorial.TabIndex = 5;
            btnVerHistorial.Text = "Ver historial";
            btnVerHistorial.UseVisualStyleBackColor = true;
            // 
            // btnRegistrarDiagnostico
            // 
            btnRegistrarDiagnostico.AutoSize = true;
            btnRegistrarDiagnostico.Location = new Point(683, 2);
            btnRegistrarDiagnostico.Margin = new Padding(4, 0, 4, 0);
            btnRegistrarDiagnostico.MinimumSize = new Size(145, 34);
            btnRegistrarDiagnostico.Name = "btnRegistrarDiagnostico";
            btnRegistrarDiagnostico.Size = new Size(145, 34);
            btnRegistrarDiagnostico.TabIndex = 6;
            btnRegistrarDiagnostico.Text = "Registrar diagnóstico";
            btnRegistrarDiagnostico.UseVisualStyleBackColor = true;
            // 
            // btnFinalizarTrabajo
            // 
            btnFinalizarTrabajo.AutoSize = true;
            btnFinalizarTrabajo.Location = new Point(836, 2);
            btnFinalizarTrabajo.Margin = new Padding(4, 0, 4, 0);
            btnFinalizarTrabajo.MinimumSize = new Size(105, 34);
            btnFinalizarTrabajo.Name = "btnFinalizarTrabajo";
            btnFinalizarTrabajo.Size = new Size(105, 34);
            btnFinalizarTrabajo.TabIndex = 7;
            btnFinalizarTrabajo.Text = "Finalizar trabajo";
            btnFinalizarTrabajo.UseVisualStyleBackColor = true;
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.ForeColor = Color.FromArgb(100, 116, 139);
            lblCantidad.Location = new Point(2, 0);
            lblCantidad.Margin = new Padding(2, 0, 8, 0);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(172, 45);
            lblCantidad.TabIndex = 5;
            lblCantidad.Text = "0 atenciones encontradas";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlInformacionRol
            // 
            pnlInformacionRol.BackColor = Color.FromArgb(239, 246, 255);
            pnlInformacionRol.Controls.Add(lblInformacionRol);
            pnlInformacionRol.Controls.Add(pnlIndicador);
            pnlInformacionRol.Dock = DockStyle.Fill;
            pnlInformacionRol.Location = new Point(0, 420);
            pnlInformacionRol.Margin = new Padding(0, 8, 0, 0);
            pnlInformacionRol.Name = "pnlInformacionRol";
            pnlInformacionRol.Padding = new Padding(12, 10, 12, 8);
            pnlInformacionRol.Size = new Size(752, 40);
            pnlInformacionRol.TabIndex = 4;
            // 
            // lblInformacionRol
            // 
            lblInformacionRol.AutoSize = true;
            lblInformacionRol.Font = new Font("Segoe UI", 8.75F);
            lblInformacionRol.ForeColor = Color.FromArgb(51, 65, 85);
            lblInformacionRol.Location = new Point(15, 13);
            lblInformacionRol.Name = "lblInformacionRol";
            lblInformacionRol.Size = new Size(38, 15);
            lblInformacionRol.TabIndex = 1;
            lblInformacionRol.Text = "label1";
            // 
            // pnlIndicador
            // 
            pnlIndicador.BackColor = Color.FromArgb(30, 64, 175);
            pnlIndicador.Dock = DockStyle.Left;
            pnlIndicador.Location = new Point(12, 10);
            pnlIndicador.Name = "pnlIndicador";
            pnlIndicador.Size = new Size(4, 22);
            pnlIndicador.TabIndex = 0;
            // 
            // FrmAtenciones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(800, 500);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(800, 500);
            Name = "FrmAtenciones";
            Padding = new Padding(24, 20, 24, 20);
            Text = "FrmAtenciones";
            tlpPrincipal.ResumeLayout(false);
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            tlpFiltros.ResumeLayout(false);
            tlpFiltros.PerformLayout();
            pnlGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAtenciones).EndInit();
            pnlPie.ResumeLayout(false);
            tlpPie.ResumeLayout(false);
            tlpPie.PerformLayout();
            flpAcciones.ResumeLayout(false);
            flpAcciones.PerformLayout();
            pnlInformacionRol.ResumeLayout(false);
            pnlInformacionRol.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlCabecera;
        private Label lblDescripcion;
        private Label lblTitulo;
        private Button btnNuevaAtencion;
        private Panel pnlFiltros;
        private TableLayoutPanel tlpFiltros;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Label lblEstado;
        private ComboBox cboEstado;
        private Label lblDesde;
        private Label lblHasta;
        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private Button btnActualizar;
        private Panel pnlGrilla;
        private DataGridView dgvAtenciones;
        private Panel pnlPie;
        private Panel pnlInformacionRol;
        private Panel pnlIndicador;
        private Label lblInformacionRol;
        private Label lblEspacioActualizar;
        private TableLayoutPanel tlpPie;
        private FlowLayoutPanel flpAcciones;
        private Button btnVerDetalle;
        private Button btnEditar;
        private Button btnIniciarProceso;
        private Label lblCantidad;
        private Button btnCancelarAtencion;
        private Button btnRegistrarEntrega;
        private Button btnVerHistorial;
        private Button btnRegistrarDiagnostico;
        private Button btnFinalizarTrabajo;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colFechaApertura;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colCliente;
        private DataGridViewTextBoxColumn colMotivo;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colEtapa;
        private DataGridViewTextBoxColumn colRecepcion;
    }
}
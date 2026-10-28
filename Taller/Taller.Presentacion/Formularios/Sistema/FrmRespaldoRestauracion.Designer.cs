#nullable enable
namespace Taller.Presentacion.Formularios.Sistema;
partial class FrmRespaldoRestauracion
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpPrincipal = new TableLayoutPanel();
        tlpPrincipal.Name = "tlpPrincipal";
        lblTitulo = new Label();
        lblTitulo.Name = "lblTitulo";
        lblEstado = new Label();
        lblEstado.Name = "lblEstado";
        tabs = new TabControl();
        tabs.Name = "tabs";
        tabCopias = new TabPage();
        tabCopias.Name = "tabCopias";
        tabPrograma = new TabPage();
        tabPrograma.Name = "tabPrograma";
        tabRestaurar = new TabPage();
        tabRestaurar.Name = "tabRestaurar";
        tabHistorial = new TabPage();
        tabHistorial.Name = "tabHistorial";
        tlpCopias = new TableLayoutPanel();
        tlpCopias.Name = "tlpCopias";
        flpManual = new FlowLayoutPanel();
        flpManual.Name = "flpManual";
        txtCarpetaManual = new TextBox();
        txtCarpetaManual.Name = "txtCarpetaManual";
        cboTipo = new ComboBox();
        cboTipo.Name = "cboTipo";
        btnCrear = new Button();
        btnCrear.Name = "btnCrear";
        lblManual = new Label();
        lblManual.Name = "lblManual";
        dgvCopias = new DataGridView();
        dgvCopias.Name = "dgvCopias";
        tlpPrograma = new TableLayoutPanel();
        tlpPrograma.Name = "tlpPrograma";
        lblAyudaPrograma = new Label();
        lblAyudaPrograma.Name = "lblAyudaPrograma";
        txtCarpetaAuto = new TextBox();
        txtCarpetaAuto.Name = "txtCarpetaAuto";
        chkProgramado = new CheckBox();
        chkProgramado.Name = "chkProgramado";
        chkDiferenciales = new CheckBox();
        chkDiferenciales.Name = "chkDiferenciales";
        flpHorario = new FlowLayoutPanel();
        flpHorario.Name = "flpHorario";
        lblHora = new Label();
        lblHora.Name = "lblHora";
        dtpHora = new DateTimePicker();
        dtpHora.Name = "dtpHora";
        lblDia = new Label();
        lblDia.Name = "lblDia";
        cboDia = new ComboBox();
        cboDia.Name = "cboDia";
        lblZona = new Label();
        lblZona.Name = "lblZona";
        cboZona = new ComboBox();
        cboZona.Name = "cboZona";
        btnGuardarPrograma = new Button();
        btnGuardarPrograma.Name = "btnGuardarPrograma";
        txtEstadoPrograma = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical
        };
        txtEstadoPrograma.Name = "txtEstadoPrograma";
        tlpRestaurar = new TableLayoutPanel();
        tlpRestaurar.Name = "tlpRestaurar";
        lblRiesgos = new Label();
        lblRiesgos.Name = "lblRiesgos";
        txtPlan = new TextBox();
        txtPlan.Name = "txtPlan";
        flpRestaurar = new FlowLayoutPanel();
        flpRestaurar.Name = "flpRestaurar";
        btnEnsayar = new Button();
        btnEnsayar.Name = "btnEnsayar";
        btnRestaurar = new Button();
        btnRestaurar.Name = "btnRestaurar";
        dgvHistorial = new DataGridView();
        dgvHistorial.Name = "dgvHistorial";
        btnActualizar = new Button();
        btnActualizar.Name = "btnActualizar";
        chkAzureManual = new CheckBox
        {
            Name = "chkAzureManual",
            Text = "Replicar también en Azure privado",
            AutoSize = true
        };
        chkAzureAuto = new CheckBox
        {
            Name = "chkAzureAuto",
            Text = "Replicar también en Azure privado",
            AutoSize = true
        };
        btnReintentarAzure = new Button
        {
            Name = "btnReintentarAzure",
            Text = "Reintentar Azure de la copia seleccionada",
            AutoSize = true
        };
        btnReintentarAzure.Click += BtnReintentarAzure_Click;
        SuspendLayout();
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 9F);
        ClientSize = new Size(1060, 630);
        MinimumSize = new Size(800, 520);
        BackColor = Color.FromArgb(244, 247, 251);
        Text = "Respaldos y recuperación";
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Padding = new Padding(12);
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.RowCount = 4;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        Controls.Add(tlpPrincipal);
        tlpPrincipal.Controls.Add(lblTitulo, 0, 0);
        tlpPrincipal.Controls.Add(lblEstado, 0, 1);
        tlpPrincipal.Controls.Add(tabs, 0, 2);
        tlpPrincipal.Controls.Add(btnActualizar, 0, 3);
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Font = new Font("Segoe UI", 17, FontStyle.Bold);
        lblTitulo.Text = "Respaldos y recuperación de información";
        lblEstado.Dock = DockStyle.Fill;
        lblEstado.Text = "Consultando configuración…";
        lblEstado.ForeColor = Color.FromArgb(71, 85, 105);
        tabs.Dock = DockStyle.Fill;
        tabs.Controls.AddRange([tabCopias, tabPrograma, tabRestaurar, tabHistorial]);
        tabCopias.Text = "1 · Copias manuales";
        tabPrograma.Text = "2 · Programación automática";
        tabRestaurar.Text = "3 · Restaurar";
        tabHistorial.Text = "Historial y aprobaciones";
        tlpCopias.Dock = DockStyle.Fill;
        tlpCopias.Padding = new Padding(8);
        tlpCopias.ColumnCount = 1;
        tlpCopias.RowCount = 3;
        tlpCopias.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpCopias.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
        tlpCopias.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
        tlpCopias.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tabCopias.Controls.Add(tlpCopias);
        lblManual.Dock = DockStyle.Fill;
        lblManual.Text = "Carpeta absoluta del SERVIDOR SQL (no de esta PC). SQL Server debe poder escribir en ella. Completa copia toda la base de datos; diferencial copia los cambios desde su completa base. Fechas de las copias: hora informada por SQL Server. No se eliminan copias automáticamente.";
        tlpCopias.Controls.Add(lblManual, 0, 0);
        tlpCopias.Controls.Add(flpManual, 0, 1);
        tlpCopias.Controls.Add(dgvCopias, 0, 2);
        flpManual.Dock = DockStyle.Fill;
        flpManual.AutoScroll = true;
        flpManual.WrapContents = true;
        txtCarpetaManual.Width = 460;
        txtCarpetaManual.MaxLength = 145;
        txtCarpetaManual.PlaceholderText = "Ej.: D:\\RespaldosSGTM o /var/opt/mssql/backup";
        cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipo.Width = 135;
        btnCrear.Text = "Crear y verificar copia";
        btnCrear.AutoSize = true;
        flpManual.Controls.AddRange([txtCarpetaManual, cboTipo, chkAzureManual, btnCrear, btnReintentarAzure]);
        dgvCopias.Dock = DockStyle.Fill;
        dgvCopias.ReadOnly = true;
        dgvCopias.AutoGenerateColumns = false;
        dgvCopias.AllowUserToAddRows = false;
        dgvCopias.AllowUserToDeleteRows = false;
        dgvCopias.AllowUserToResizeColumns = true;
        dgvCopias.RowHeadersVisible = false;
        dgvCopias.MultiSelect = false;
        dgvCopias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCopias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCopias.BackgroundColor = Color.White;
        colFechaServidor = new DataGridViewTextBoxColumn
        {
            Name = "colFechaServidor",
            DataPropertyName = "FechaServidor",
            HeaderText = "Fecha (servidor SQL)",
            MinimumWidth = 90,
            FillWeight = 150
        };
        dgvCopias.Columns.Add(colFechaServidor);
        colFechaServidor.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
        colTipo = new DataGridViewTextBoxColumn
        {
            Name = "colTipo",
            DataPropertyName = "Tipo",
            HeaderText = "Tipo",
            MinimumWidth = 90,
            FillWeight = 100
        };
        dgvCopias.Columns.Add(colTipo);
        colSoloCopia = new DataGridViewTextBoxColumn
        {
            Name = "colSoloCopia",
            DataPropertyName = "SoloCopia",
            HeaderText = "Protectora / sólo copia",
            MinimumWidth = 90,
            FillWeight = 100
        };
        dgvCopias.Columns.Add(colSoloCopia);
        colRutaServidor = new DataGridViewTextBoxColumn
        {
            Name = "colRutaServidor",
            DataPropertyName = "RutaServidor",
            HeaderText = "Archivo en servidor",
            MinimumWidth = 90,
            FillWeight = 350
        };
        dgvCopias.Columns.Add(colRutaServidor);
        colAutor = new DataGridViewTextBoxColumn
        {
            Name = "colAutor",
            DataPropertyName = "Autor",
            HeaderText = "Solicitada por",
            MinimumWidth = 90,
            FillWeight = 130
        };
        dgvCopias.Columns.Add(colAutor);
        colRutaControl = new DataGridViewTextBoxColumn
        {
            Name = "colRutaControl",
            DataPropertyName = "RutaControl",
            HeaderText = "Copia auxiliar de control",
            MinimumWidth = 120,
            FillWeight = 220
        };
        dgvCopias.Columns.Add(colRutaControl);
        colAdvertencia = new DataGridViewTextBoxColumn
        {
            Name = "colAdvertencia",
            DataPropertyName = "Advertencia",
            HeaderText = "Advertencia",
            MinimumWidth = 100,
            FillWeight = 180
        };
        dgvCopias.Columns.Add(colAdvertencia);
        dgvCopias.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAzure", DataPropertyName = "EstadoAzure", HeaderText = "Azure", MinimumWidth = 120 });
        tlpPrograma.Dock = DockStyle.Fill;
        tlpPrograma.Padding = new Padding(10);
        tlpPrograma.ColumnCount = 1;
        tlpPrograma.RowCount = 8;
        tlpPrograma.AutoScroll = true;
        tlpPrograma.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrograma.RowStyles.Add(new RowStyle(SizeType.Absolute, 66));
        tlpPrograma.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        tlpPrograma.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        tlpPrograma.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlpPrograma.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        tlpPrograma.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        tlpPrograma.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        tlpPrograma.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tabPrograma.Controls.Add(tlpPrograma);
        lblAyudaPrograma.Text = "El agente SGTMRespaldos debe estar instalado en un equipo encendido con acceso al servidor. El plan corre una vez por día al horario indicado; no depende de una sesión abierta. Si falta una completa base, el agente crea una. Los fallos reintentan a los 15 minutos y quedan registrados.";
        lblAyudaPrograma.Dock = DockStyle.Fill;
        chkProgramado.Text = "Activar plan diario";
        chkProgramado.AutoSize = true;
        chkDiferenciales.Text = "Completa semanal y diferenciales los demás días (desmarcado: completa todos los días)";
        chkDiferenciales.AutoSize = true;
        txtCarpetaAuto.Dock = DockStyle.Fill;
        txtCarpetaAuto.MaxLength = 145;
        txtCarpetaAuto.PlaceholderText = "Carpeta de destino en el servidor SQL";
        flpHorario.Dock = DockStyle.Fill;
        flpHorario.WrapContents = true;
        flpHorario.AutoScroll = true;
        lblHora.Text = "Hora";
        lblDia.Text = "Completa semanal";
        lblZona.Text = "Zona";
        lblHora.AutoSize = lblDia.AutoSize = lblZona.AutoSize = true;
        dtpHora.Format = DateTimePickerFormat.Custom;
        dtpHora.CustomFormat = "HH:mm";
        dtpHora.ShowUpDown = true;
        dtpHora.Width = 85;
        cboDia.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDia.Width = 130;
        cboZona.DropDownStyle = ComboBoxStyle.DropDownList;
        cboZona.Width = 230;
        flpHorario.Controls.AddRange([lblHora, dtpHora, lblDia, cboDia, lblZona, cboZona]);
        btnGuardarPrograma.Text = "Guardar programación";
        btnGuardarPrograma.AutoSize = true;
        btnGuardarPrograma.Anchor = AnchorStyles.Left;
        txtEstadoPrograma.Dock = DockStyle.Fill;
        tlpPrograma.Controls.Add(lblAyudaPrograma, 0, 0);
        tlpPrograma.Controls.Add(chkProgramado, 0, 1);
        tlpPrograma.Controls.Add(chkDiferenciales, 0, 2);
        tlpPrograma.Controls.Add(txtCarpetaAuto, 0, 3);
        tlpPrograma.Controls.Add(flpHorario, 0, 4);
        tlpPrograma.Controls.Add(btnGuardarPrograma, 0, 5);
        tlpPrograma.Controls.Add(chkAzureAuto, 0, 6);
        tlpPrograma.Controls.Add(txtEstadoPrograma, 0, 7);
        tlpRestaurar.Dock = DockStyle.Fill;
        tlpRestaurar.Padding = new Padding(10);
        tlpRestaurar.ColumnCount = 1;
        tlpRestaurar.RowCount = 3;
        tlpRestaurar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpRestaurar.RowStyles.Add(new RowStyle(SizeType.Absolute, 125));
        tlpRestaurar.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpRestaurar.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        tabRestaurar.Controls.Add(tlpRestaurar);
        lblRiesgos.Dock = DockStyle.Fill;
        lblRiesgos.ForeColor = Color.Firebrick;
        lblRiesgos.Text = "RESTAURAR REEMPLAZA DATOS: se pierden del estado activo los cambios posteriores al punto de copia, incluidos cobros, usuarios y contraseñas. Coordine una ventana de mantenimiento y solicite acompañamiento del personal técnico.\r\nPrimero seleccione una copia en la pestaña Copias manuales y ensaye su recuperación en una base temporal. Después se requieren el administrador de esta sesión y otro administrador o administrativo, con credenciales distintas. Las demás conexiones deben terminar; no se fuerza su desconexión.";
        txtPlan.Dock = DockStyle.Fill;
        txtPlan.Multiline = true;
        txtPlan.ReadOnly = true;
        txtPlan.ScrollBars = ScrollBars.Vertical;
        txtPlan.BackColor = Color.White;
        flpRestaurar.Dock = DockStyle.Fill;
        flpRestaurar.WrapContents = false;
        flpRestaurar.AutoScroll = true;
        btnEnsayar.Text = "Ensayar restauración de la copia seleccionada";
        btnRestaurar.Text = "Autorizar y restaurar…";
        btnEnsayar.AutoSize = btnRestaurar.AutoSize = true;
        btnRestaurar.ForeColor = Color.Firebrick;
        btnEnsayar.Enabled = btnRestaurar.Enabled = false;
        flpRestaurar.Controls.AddRange([btnEnsayar, btnRestaurar]);
        tlpRestaurar.Controls.Add(lblRiesgos, 0, 0);
        tlpRestaurar.Controls.Add(txtPlan, 0, 1);
        tlpRestaurar.Controls.Add(flpRestaurar, 0, 2);
        tabHistorial.Controls.Add(dgvHistorial);
        dgvHistorial.Dock = DockStyle.Fill;
        dgvHistorial.ReadOnly = true;
        dgvHistorial.AutoGenerateColumns = false;
        dgvHistorial.AllowUserToAddRows = false;
        dgvHistorial.AllowUserToDeleteRows = false;
        dgvHistorial.AllowUserToResizeColumns = true;
        dgvHistorial.RowHeadersVisible = false;
        dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvHistorial.BackgroundColor = Color.White;
        colEventoFechaUtc = new DataGridViewTextBoxColumn
        {
            Name = "colEventoFechaUtc",
            DataPropertyName = "FechaLocal",
            HeaderText = "Fecha y hora — Argentina",
            MinimumWidth = 90,
            FillWeight = 130
        };
        dgvHistorial.Columns.Add(colEventoFechaUtc);
        colEventoFechaUtc.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
        colEventoAccion = new DataGridViewTextBoxColumn
        {
            Name = "colEventoAccion",
            DataPropertyName = "Accion",
            HeaderText = "Operación",
            MinimumWidth = 90,
            FillWeight = 130
        };
        dgvHistorial.Columns.Add(colEventoAccion);
        colEventoResultado = new DataGridViewTextBoxColumn
        {
            Name = "colEventoResultado",
            DataPropertyName = "Resultado",
            HeaderText = "Resultado",
            MinimumWidth = 90,
            FillWeight = 100
        };
        dgvHistorial.Columns.Add(colEventoResultado);
        colEventoAutor = new DataGridViewTextBoxColumn
        {
            Name = "colEventoAutor",
            DataPropertyName = "Autor",
            HeaderText = "Solicitante",
            MinimumWidth = 90,
            FillWeight = 110
        };
        dgvHistorial.Columns.Add(colEventoAutor);
        colEventoSegundoAprobador = new DataGridViewTextBoxColumn
        {
            Name = "colEventoSegundoAprobador",
            DataPropertyName = "SegundoAprobador",
            HeaderText = "Segundo aprobador",
            MinimumWidth = 90,
            FillWeight = 110
        };
        dgvHistorial.Columns.Add(colEventoSegundoAprobador);
        colEventoDetalle = new DataGridViewTextBoxColumn
        {
            Name = "colEventoDetalle",
            DataPropertyName = "Detalle",
            HeaderText = "Detalle / archivos",
            MinimumWidth = 90,
            FillWeight = 350
        };
        dgvHistorial.Columns.Add(colEventoDetalle);
        btnActualizar.Text = "Actualizar estado e historial";
        btnActualizar.AutoSize = true;
        btnActualizar.Anchor = AnchorStyles.Left;
        Load += Formulario_Load;
        FormClosing += Formulario_FormClosing;
        btnActualizar.Click += BtnActualizar_Click;
        btnCrear.Click += BtnCrear_Click;
        btnGuardarPrograma.Click += BtnGuardarPrograma_Click;
        btnEnsayar.Click += BtnEnsayar_Click;
        btnRestaurar.Click += BtnRestaurar_Click;
        dgvCopias.SelectionChanged += DgvCopias_SelectionChanged;
        ResumeLayout(false);
    }

    private CheckBox chkAzureManual = null!, chkAzureAuto = null!;
    private Button btnReintentarAzure = null!;
    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblTitulo = null!;
    private Label lblEstado = null!;
    private TabControl tabs = null!;
    private TabPage tabCopias = null!;
    private TabPage tabPrograma = null!;
    private TabPage tabRestaurar = null!;
    private TabPage tabHistorial = null!;
    private TableLayoutPanel tlpCopias = null!;
    private FlowLayoutPanel flpManual = null!;
    private TextBox txtCarpetaManual = null!;
    private ComboBox cboTipo = null!;
    private Button btnCrear = null!;
    private Label lblManual = null!;
    private DataGridView dgvCopias = null!;
    private TableLayoutPanel tlpPrograma = null!;
    private Label lblAyudaPrograma = null!;
    private TextBox txtCarpetaAuto = null!;
    private CheckBox chkProgramado = null!;
    private CheckBox chkDiferenciales = null!;
    private FlowLayoutPanel flpHorario = null!;
    private Label lblHora = null!;
    private DateTimePicker dtpHora = null!;
    private Label lblDia = null!;
    private ComboBox cboDia = null!;
    private Label lblZona = null!;
    private ComboBox cboZona = null!;
    private Button btnGuardarPrograma = null!;
    private TextBox txtEstadoPrograma = null!;
    private TableLayoutPanel tlpRestaurar = null!;
    private Label lblRiesgos = null!;
    private TextBox txtPlan = null!;
    private FlowLayoutPanel flpRestaurar = null!;
    private Button btnEnsayar = null!;
    private Button btnRestaurar = null!;
    private DataGridView dgvHistorial = null!;
    private Button btnActualizar = null!;
    private DataGridViewTextBoxColumn colRutaControl = null!, colAdvertencia = null!;
    private DataGridViewTextBoxColumn colFechaServidor = null!;
    private DataGridViewTextBoxColumn colTipo = null!;
    private DataGridViewTextBoxColumn colSoloCopia = null!;
    private DataGridViewTextBoxColumn colRutaServidor = null!;
    private DataGridViewTextBoxColumn colAutor = null!;
    private DataGridViewTextBoxColumn colEventoFechaUtc = null!;
    private DataGridViewTextBoxColumn colEventoAccion = null!;
    private DataGridViewTextBoxColumn colEventoResultado = null!;
    private DataGridViewTextBoxColumn colEventoAutor = null!;
    private DataGridViewTextBoxColumn colEventoSegundoAprobador = null!;
    private DataGridViewTextBoxColumn colEventoDetalle = null!;
}

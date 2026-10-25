#nullable enable
namespace Taller.Presentacion.Formularios.Atenciones;
partial class FrmSeguimientoTrabajo
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private TabControl tabs = null!;
    private Button btnImprimirInforme = null!, btnExportarAvance = null!;
    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblResumen = null!;
    private ComboBox cboTarea = null!;
    private TextBox txtDescripcion = null!, txtRecomendacion = null!, txtMaterial = null!;
    private CheckBox chkImpide = null!, chkConforme = null!;
    private NumericUpDown nudRecibido = null!;
    private DataGridView dgvProgreso = null!;
    private DataGridView dgvHallazgos = null!, dgvPausas = null!, dgvMateriales = null!;
    private Button btnHallazgo = null!, btnAdicional = null!, btnPausar = null!, btnResolver = null!, btnRecibir = null!;
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpPrincipal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(12)
        };
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        lblResumen = new Label
        {
            Dock = DockStyle.Fill
        };
        cboTarea = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        tabs = new TabControl
        {
            Dock = DockStyle.Fill
        };
        var hallazgos = new TabPage("Hallazgos e informes") { Name = "tabHallazgos" };
        var pausas = new TabPage("Pausas y continuidad") { Name = "tabPausas" };
        var materiales = new TabPage("Materiales del cliente") { Name = "tabMateriales" };
        var progreso = new TabPage("Avance de trabajos") { Name = "tabAvance" };
        dgvProgreso = new DataGridView { Name = "dgvProgreso", Dock = DockStyle.Fill, AutoGenerateColumns = false, ReadOnly = true,
            AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false, BackgroundColor = Color.White,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells };
        dgvProgreso.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvProgreso.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTrabajo", DataPropertyName = "Descripcion", HeaderText = "Trabajo autorizado", FillWeight = 200, MinimumWidth = 180 });
        dgvProgreso.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAutorizado", DataPropertyName = "CantidadAutorizada", HeaderText = "Autorizado", MinimumWidth = 85, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvProgreso.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRealizado", DataPropertyName = "CantidadEjecutada", HeaderText = "Realizado", MinimumWidth = 85, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvProgreso.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", DataPropertyName = "EstadoDescripcion", HeaderText = "Estado", MinimumWidth = 120 });
        dgvProgreso.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNota", DataPropertyName = "Observaciones", HeaderText = "Registro del técnico", FillWeight = 150, MinimumWidth = 140 });
        progreso.Controls.Add(dgvProgreso);
        tabs.TabPages.AddRange(new[] { progreso, hallazgos, pausas, materiales });
        dgvHallazgos = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        dgvHallazgos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", DataPropertyName = "Fecha", HeaderText = "Fecha", MinimumWidth = 125, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
        dgvHallazgos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescripcion", DataPropertyName = "Descripcion", HeaderText = "Hallazgo", MinimumWidth = 220, FillWeight = 180 });
        dgvHallazgos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRecomendacion", DataPropertyName = "Recomendacion", HeaderText = "Recomendación", MinimumWidth = 220, FillWeight = 180 });
        dgvHallazgos.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colImpide", DataPropertyName = "ImpideContinuar", HeaderText = "Impide continuar", MinimumWidth = 90 });
        var ht = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4
        };
        ht.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        ht.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        ht.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        ht.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        txtDescripcion = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            MaxLength = 1000,
            PlaceholderText = "Hallazgo o conclusión puntual (obligatorio)"
        };
        txtRecomendacion = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            MaxLength = 1000,
            PlaceholderText = "Recomendación / condiciones para continuar (obligatorio)"
        };
        var ha = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };
        chkImpide = new CheckBox
        {
            Text = "Impide continuar el alcance seleccionado",
            AutoSize = true
        };
        btnHallazgo = new Button
        {
            Text = "Registrar informe",
            AutoSize = true
        };
        btnHallazgo.Click += Hallazgo;
        btnAdicional = new Button
        {
            Text = "Preparar adicional del hallazgo seleccionado",
            AutoSize = true
        };
        btnAdicional.Click += Adicional;
        ha.Controls.AddRange(new Control[] { chkImpide, btnHallazgo, btnAdicional });
        ht.Controls.Add(dgvHallazgos, 0, 0);
        ht.Controls.Add(txtDescripcion, 0, 1);
        ht.Controls.Add(txtRecomendacion, 0, 2);
        ht.Controls.Add(ha, 0, 3);
        hallazgos.Controls.Add(ht);
        dgvPausas = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        dgvPausas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTarea", DataPropertyName = "Alcance", HeaderText = "Ítem (vacío: toda)", MinimumWidth = 95 });
        dgvPausas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMotivo", DataPropertyName = "Motivo", HeaderText = "Motivo", MinimumWidth = 220, FillWeight = 200 });
        dgvPausas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colResolucion", DataPropertyName = "Resolucion", HeaderText = "Resolución técnica", MinimumWidth = 220, FillWeight = 200 });
        dgvPausas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFechaResolucion", DataPropertyName = "FechaResolucion", HeaderText = "Resuelta el", MinimumWidth = 125 });
        var pt = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        pt.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        pt.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        var pa = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };
        btnPausar = new Button
        {
            Text = "Pausar alcance seleccionado",
            AutoSize = true
        };
        btnPausar.Click += Pausar;
        btnResolver = new Button
        {
            Text = "Resolver pausa seleccionada",
            AutoSize = true
        };
        btnResolver.Click += Resolver;
        pa.Controls.AddRange(new Control[] { btnPausar, btnResolver });
        pt.Controls.Add(dgvPausas, 0, 0);
        pt.Controls.Add(pa, 0, 1);
        pausas.Controls.Add(pt);
        dgvMateriales = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        dgvMateriales.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaterial", DataPropertyName = "Descripcion", HeaderText = "Material requerido", MinimumWidth = 220, FillWeight = 200 });
        dgvMateriales.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCantidad", DataPropertyName = "Cantidad", HeaderText = "Requerido", MinimumWidth = 90 });
        dgvMateriales.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRecibido", DataPropertyName = "Recibida", HeaderText = "Recibido", MinimumWidth = 90 });
        dgvMateriales.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colConforme", DataPropertyName = "Conforme", HeaderText = "Conforme", MinimumWidth = 85 });
        dgvMateriales.SelectionChanged += MaterialSeleccionado;
        var mt = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };
        mt.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mt.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        mt.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        txtMaterial = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            MaxLength = 1000,
            PlaceholderText = "Describa lo recibido y su conformidad o motivo de rechazo. El taller no cobra la venta de este material."
        };
        var ma = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };
        nudRecibido = new NumericUpDown
        {
            DecimalPlaces = 3,
            Maximum = 1000000,
            Width = 130
        };
        chkConforme = new CheckBox
        {
            Text = "Apto para utilizar",
            AutoSize = true
        };
        btnRecibir = new Button
        {
            Text = "Registrar recepción acumulada y revisión",
            AutoSize = true
        };
        btnRecibir.Click += Recibir;
        ma.Controls.AddRange(new Control[] { new Label { Text = "Cantidad recibida:", AutoSize = true }, nudRecibido, chkConforme, btnRecibir });
        mt.Controls.Add(dgvMateriales, 0, 0);
        mt.Controls.Add(txtMaterial, 0, 1);
        mt.Controls.Add(ma, 0, 2);
        materiales.Controls.Add(mt);
        var acciones = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill
        };
        var actualizar = new Button
        {
            Text = "Actualizar",
            AutoSize = true
        };
        actualizar.Click += Actualizar;
        var imprimir = new Button
        {
            Text = "Informe de intervención",
            AutoSize = true
        };
        btnImprimirInforme = imprimir;
        imprimir.Click += Imprimir;
        acciones.Controls.Add(imprimir);
        var exportar = new Button { Text = "Exportar avance HTML", AutoSize = true };
        btnExportarAvance = exportar;
        exportar.Click += Exportar;
        acciones.Controls.Add(exportar);
        var cerrar = new Button
        {
            Text = "Cerrar",
            AutoSize = true
        };
        cerrar.Click += Cerrar;
        acciones.Controls.AddRange(new Control[] { actualizar, cerrar });
        acciones.AutoScroll = true;
        foreach (var boton in new[] { actualizar, cerrar, imprimir, exportar, btnHallazgo, btnAdicional, btnPausar, btnResolver, btnRecibir }) Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(boton);
        tlpPrincipal.Controls.Add(lblResumen, 0, 0);
        tlpPrincipal.Controls.Add(cboTarea, 0, 1);
        tlpPrincipal.Controls.Add(tabs, 0, 2);
        tlpPrincipal.Controls.Add(acciones, 0, 3);
        tlpPrincipal.AutoScroll = true;
        dgvHallazgos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvHallazgos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvHallazgos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPausas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvPausas.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvPausas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMateriales.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvMateriales.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvMateriales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        Controls.Add(tlpPrincipal);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 10);
        foreach (var grilla in new[] { dgvProgreso, dgvHallazgos, dgvPausas, dgvMateriales })
        {
            grilla.BackgroundColor = Color.White;
            grilla.AllowUserToResizeColumns = true;
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }
        Text = "Progreso y seguimiento";
        ClientSize = new Size(1180, 700);
        MinimumSize = new Size(820, 570);
        StartPosition = FormStartPosition.CenterParent;
        Load += Cargar;
        FormClosing += AlCerrar;
    }
}

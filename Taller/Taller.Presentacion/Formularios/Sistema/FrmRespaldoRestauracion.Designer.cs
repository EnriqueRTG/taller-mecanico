#nullable enable
using Taller.Presentacion.Estilos;
namespace Taller.Presentacion.Formularios.Sistema;
partial class FrmRespaldoRestauracion
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblTitulo = null!, lblEstado = null!, lblManual = null!, lblAyudaPrograma = null!, lblRiesgos = null!, lblSeleccion = null!;
    private TabControl tabs = null!;
    private TabPage tabCopias = null!, tabPrograma = null!, tabRestaurar = null!, tabHistorial = null!;
    private TextBox txtCarpetaManual = null!, txtCarpetaAuto = null!, txtCarpetaDrive = null!, txtEstadoPrograma = null!, txtPlan = null!;
    private ComboBox cboTipo = null!, cboDia = null!, cboZona = null!;
    private CheckBox chkProgramado = null!, chkDiferenciales = null!;
    private DateTimePicker dtpHora = null!;
    private Button btnCrear = null!, btnCarpetaManual = null!, btnCarpetaAuto = null!, btnGuardarPrograma = null!, btnEnsayar = null!, btnRestaurar = null!, btnActualizar = null!;
    private DataGridView dgvCopias = null!, dgvRestaurar = null!, dgvHistorial = null!;

    private static Label Ayuda(string texto)
    {
        var label = new Label { Text = texto, AutoSize = true, Dock = DockStyle.Top, Margin = new Padding(3, 5, 3, 10), ForeColor = EstilosInterfaz.TextoNormal };
        label.ParentChanged += (_, _) => {
            if (label.Parent is not { } parent) return;
            void Ajustar() { if (!label.IsDisposed) label.MaximumSize = new Size(Math.Max(100, parent.ClientSize.Width - parent.Padding.Horizontal - 12), 0); }
            parent.SizeChanged += (_, _) => Ajustar(); Ajustar();
        };
        return label;
    }
    private static TableLayoutPanel Columna() => new() { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(14), AutoScroll = true };
    private static Button Boton(string texto, EventHandler click, bool primario = false, bool peligro = false)
    {
        var b = new Button { Text = texto, AutoSize = true, MinimumSize = new Size(110, 36), Margin = new Padding(3, 6, 8, 6) };
        if (peligro) EstilosInterfaz.AplicarBotonPeligro(b);
        else if (primario) EstilosInterfaz.AplicarBotonPrimario(b);
        else EstilosInterfaz.AplicarBotonSecundario(b);
        b.Click += click; return b;
    }
    private static Control Campo(string titulo, Control control)
    {
        var p = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Top, ColumnCount = 1, Margin = new Padding(3, 4, 14, 6) };
        p.Controls.Add(new Label { Text = titulo, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Margin = new Padding(3, 3, 3, 7) });
        control.Dock = DockStyle.Top; p.Controls.Add(control); return p;
    }
    private static Control Selector(TextBox texto, Button boton)
    {
        var p = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); p.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        texto.Dock = DockStyle.Fill; p.Controls.Add(texto, 0, 0); p.Controls.Add(boton, 1, 0); return p;
    }
    private static DataGridView GrillaCopias(string nombre)
    {
        var g = new DataGridView { Name = nombre, Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        EstilosInterfaz.AplicarGrilla(g);
        foreach (var (prop, titulo, ancho) in new[] { ("FechaServidor", "Fecha de la copia (SQL)", 150), ("Tipo", "Tipo", 90), ("SoloCopia", "Protectora", 85), ("Autor", "Solicitada por", 150), ("RutaServidor", "Archivo local", 250), ("EstadoDrive", "Google Drive", 210), ("Advertencia", "Observaciones", 190) })
        {
            var c = new DataGridViewTextBoxColumn { DataPropertyName = prop, HeaderText = titulo, MinimumWidth = ancho, FillWeight = ancho };
            if (prop == "FechaServidor") c.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            g.Columns.Add(c);
        }
        g.CellFormatting += (_, e) => { if (e.ColumnIndex >= 0 && g.Columns[e.ColumnIndex].DataPropertyName == "SoloCopia" && e.Value is bool valor) { e.Value = valor ? "Sí" : "No"; e.FormattingApplied = true; } };
        return g;
    }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); SuspendLayout();
        AutoScaleMode = AutoScaleMode.Font; Font = new Font("Segoe UI", 10F); ClientSize = new Size(1180, 760); MinimumSize = new Size(850, 620);
        BackColor = EstilosInterfaz.FondoAplicacion; Text = "Respaldos y recuperación";
        tlpPrincipal = Columna(); tlpPrincipal.AutoScroll = false;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); Controls.Add(tlpPrincipal);
        lblTitulo = Ayuda("Respaldos y recuperación de información"); lblTitulo.Font = new Font("Segoe UI", 17, FontStyle.Bold);
        lblEstado = Ayuda("Consultando configuración…");
        tabs = new TabControl { Name = "tabs", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        tabCopias = new TabPage("Copias manuales"); tabPrograma = new TabPage("Programación automática"); tabRestaurar = new TabPage("Restaurar"); tabHistorial = new TabPage("Historial y aprobaciones");
        tabs.TabPages.AddRange([tabCopias, tabPrograma, tabRestaurar, tabHistorial]);
        foreach (TabPage t in tabs.TabPages) { t.Font = Font; t.BackColor = Color.White; }
        btnActualizar = Boton("Actualizar estado e historial", BtnActualizar_Click); btnActualizar.Anchor = AnchorStyles.Left;
        tlpPrincipal.Controls.Add(lblTitulo, 0, 0); tlpPrincipal.Controls.Add(lblEstado, 0, 1); tlpPrincipal.Controls.Add(tabs, 0, 2); tlpPrincipal.Controls.Add(btnActualizar, 0, 3);
        var manual = Columna(); tabCopias.Controls.Add(manual);
        manual.RowStyles.Add(new RowStyle(SizeType.AutoSize)); manual.RowStyles.Add(new RowStyle(SizeType.AutoSize)); manual.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); manual.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        lblManual = Ayuda(""); manual.Controls.Add(lblManual, 0, 0);
        var opciones = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, WrapContents = true };
        txtCarpetaManual = new TextBox { Name = "txtCarpetaManual", MaxLength = 145, Width = 380 };
        btnCarpetaManual = Boton("Elegir carpeta…", ElegirCarpetaManual_Click); btnCarpetaManual.Name = "btnCarpetaManual"; btnCarpetaManual.Visible = false;
        var carpeta = Campo("Carpeta local de respaldo", Selector(txtCarpetaManual, btnCarpetaManual)); carpeta.Width = 500; opciones.Controls.Add(carpeta);
        cboTipo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 145 }; var tipo = Campo("Tipo de copia", cboTipo); tipo.Width = 160; opciones.Controls.Add(tipo);
        btnCrear = Boton("Crear y verificar copia", BtnCrear_Click, true); opciones.Controls.Add(Campo("Crear respaldo local", btnCrear));
        manual.Controls.Add(opciones, 0, 1); dgvCopias = GrillaCopias("dgvCopias"); manual.Controls.Add(dgvCopias, 0, 2);
        var programa = Columna(); tabPrograma.Controls.Add(programa); programa.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        lblAyudaPrograma = Ayuda("El agente se ejecuta al iniciar sesión en Windows y sigue funcionando con SGTM cerrado. El equipo debe estar encendido, la sesión iniciada y Drive disponible. Si falta la completa base, se crea una. Los fallos se reintentan cada 15 minutos."); programa.Controls.Add(lblAyudaPrograma);
        chkProgramado = new CheckBox { Text = "Activar respaldo diario", AutoSize = true }; programa.Controls.Add(chkProgramado);
        txtCarpetaAuto = new TextBox { MaxLength = 145 }; btnCarpetaAuto = Boton("Elegir carpeta…", ElegirCarpetaAuto_Click); btnCarpetaAuto.Name = "btnCarpetaAuto"; btnCarpetaAuto.Visible = false;
        programa.Controls.Add(Campo("Carpeta local (SQL Server debe poder escribir aquí)", Selector(txtCarpetaAuto, btnCarpetaAuto)));
        txtCarpetaDrive = new TextBox { MaxLength = 500 }; var elegirDrive = Boton("Elegir carpeta…", (_, _) => ElegirCarpeta(txtCarpetaDrive));
        programa.Controls.Add(Campo("Carpeta de Google Drive para los paquetes automáticos (opcional)", Selector(txtCarpetaDrive, elegirDrive)));
        chkDiferenciales = new CheckBox { Text = "Completa semanal y diferenciales los demás días; desmarcado: completa diaria", AutoSize = true }; programa.Controls.Add(chkDiferenciales);
        var horario = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, WrapContents = true };
        dtpHora = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Width = 100 };
        cboDia = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 }; cboZona = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 240 };
        horario.Controls.Add(Campo("Hora diaria (24 horas)", dtpHora)); horario.Controls.Add(Campo("Día de la copia completa", cboDia)); horario.Controls.Add(Campo("Zona horaria del plan", cboZona)); programa.Controls.Add(horario);
        btnGuardarPrograma = Boton("Guardar programación", BtnGuardarPrograma_Click, true); programa.Controls.Add(btnGuardarPrograma);
        txtEstadoPrograma = new TextBox { Multiline = true, ReadOnly = true, Height = 120, Dock = DockStyle.Top, ScrollBars = ScrollBars.Vertical }; programa.Controls.Add(Campo("Estado del agente y próxima ejecución", txtEstadoPrograma));
        var restaurar = Columna(); tabRestaurar.Controls.Add(restaurar); restaurar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        restaurar.RowStyles.Add(new RowStyle(SizeType.AutoSize)); restaurar.RowStyles.Add(new RowStyle(SizeType.Percent, 65)); restaurar.RowStyles.Add(new RowStyle(SizeType.AutoSize)); restaurar.RowStyles.Add(new RowStyle(SizeType.Percent, 35)); restaurar.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        lblRiesgos = Ayuda("RESTAURAR REEMPLAZA DATOS posteriores a la copia, incluidos cobros, usuarios y contraseñas. Seleccione aquí la copia, ensaye y luego autorice con dos cuentas distintas. Cierre las demás conexiones; se creará una copia protectora."); lblRiesgos.ForeColor = EstilosInterfaz.Peligro;
        dgvRestaurar = GrillaCopias("dgvRestaurar"); dgvRestaurar.SelectionChanged += DgvCopias_SelectionChanged;
        lblSeleccion = Ayuda("Ninguna copia seleccionada."); lblSeleccion.Font = new Font(Font, FontStyle.Bold);
        txtPlan = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, BackColor = Color.White };
        var acciones = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, WrapContents = false };
        btnEnsayar = Boton("Ensayar restauración", BtnEnsayar_Click, true); btnRestaurar = Boton("Autorizar y restaurar…", BtnRestaurar_Click, peligro: true); btnEnsayar.Enabled = btnRestaurar.Enabled = false; acciones.Controls.AddRange([btnEnsayar, btnRestaurar]);
        restaurar.Controls.Add(lblRiesgos, 0, 0); restaurar.Controls.Add(dgvRestaurar, 0, 1); restaurar.Controls.Add(lblSeleccion, 0, 2); restaurar.Controls.Add(txtPlan, 0, 3); restaurar.Controls.Add(acciones, 0, 4);
        var historial = Columna(); historial.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); historial.RowStyles.Add(new RowStyle(SizeType.AutoSize)); historial.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); tabHistorial.Controls.Add(historial);
        historial.Controls.Add(Ayuda("Registro de las copias, exportaciones, ensayos y restauraciones: permite consultar qué ocurrió, cuándo y quién autorizó. Se conserva separado de los datos restaurados. Seleccione una fila para consultar su detalle completo."), 0, 0);
        dgvHistorial = new DataGridView { Name = "dgvHistorial", Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill }; EstilosInterfaz.AplicarGrilla(dgvHistorial);
        foreach (var (prop, titulo) in new[] { ("FechaLocal", "Fecha y hora — Argentina"), ("Accion", "Operación"), ("Resultado", "Resultado"), ("Autor", "Solicitante"), ("SegundoAprobador", "Segundo aprobador"), ("Detalle", "Detalle / archivos") }) { var c = new DataGridViewTextBoxColumn { DataPropertyName = prop, HeaderText = titulo, MinimumWidth = 110 }; if (prop == "FechaLocal") c.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss"; if (prop == "Detalle") c.FillWeight = 300; dgvHistorial.Columns.Add(c); }
        dgvHistorial.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) MessageBox.Show(this, dgvHistorial.Rows[e.RowIndex].Cells[5].Value?.ToString(), "Detalle de la operación"); }; historial.Controls.Add(dgvHistorial, 0, 1);
        Load += Formulario_Load; FormClosing += Formulario_FormClosing; ResumeLayout(false);
    }
}

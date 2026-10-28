#nullable enable
namespace Taller.Presentacion.Formularios.Atenciones;
partial class FrmEjecutarAtencion
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private System.Windows.Forms.Timer temporizadorPausas = null!;
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        temporizadorPausas = new System.Windows.Forms.Timer(components) { Interval = 10000 };
        temporizadorPausas.Tick += ComprobarPausas;
        tlpPrincipal = new TableLayoutPanel();
        tlpPrincipal.Name = "tlpPrincipal";
        txtResumen = new TextBox();
        txtResumen.Name = "txtResumen";
        lblAviso = new Label();
        lblAviso.Name = "lblAviso";
        dgvItems = new DataGridView();
        dgvItems.Name = "dgvItems";
        txtObservaciones = new TextBox();
        txtObservaciones.Name = "txtObservaciones";
        flpAcciones = new FlowLayoutPanel();
        flpAcciones.Name = "flpAcciones";
        btnTomar = new Button();
        btnTomar.Name = "btnTomar";
        btnGuardar = new Button();
        btnGuardar.Name = "btnGuardar";
        btnLiberar = new Button();
        btnLiberar.Name = "btnLiberar";
        btnFinalizar = new Button();
        btnFinalizar.Name = "btnFinalizar";
        btnCerrar = new Button();
        btnCerrar.Name = "btnCerrar";
        SuspendLayout();
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 10F);
        ClientSize = new Size(1180, 720);
        MinimumSize = new Size(820, 600);
        StartPosition = FormStartPosition.CenterParent;
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Padding = new Padding(12);
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowCount = 6;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 68));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(tlpPrincipal);
        Text = "Ejecución de trabajos autorizados";
        tlpPrincipal.Controls.Add(txtResumen, 0, 0);
        txtResumen.Dock = DockStyle.Fill;
        var inicio = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        inicio.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        inicio.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        inicio.Controls.Add(lblAviso, 0, 0);
        inicio.Controls.Add(btnTomar, 1, 0);
        tlpPrincipal.Controls.Add(inicio, 0, 1);
        lblAviso.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(dgvItems, 0, 2);
        dgvItems.Dock = DockStyle.Fill;
        var notas = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
        notas.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
        notas.Controls.Add(new Label { Text = "Observaciones y resultado de la intervención", Dock = DockStyle.Fill }, 0, 0);
        notas.Controls.Add(txtObservaciones, 0, 1);
        tlpPrincipal.Controls.Add(notas, 0, 3);
        txtObservaciones.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(flpAcciones, 0, 5);
        flpAcciones.Dock = DockStyle.Fill;
        dgvItems.AutoGenerateColumns = false;
        dgvItems.AllowUserToAddRows = false;
        dgvItems.AllowUserToDeleteRows = false;
        dgvItems.AllowUserToResizeColumns = true;
        dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvItems.MultiSelect = false;
        dgvItems.ReadOnly = true;
        dgvItems.RowHeadersVisible = false;
        dgvItems.BackgroundColor = Color.White;
        dgvItems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvItems.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescripcion", DataPropertyName = "Descripcion", HeaderText = "Trabajo autorizado", FillWeight = 200, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCantidadAutorizada", DataPropertyName = "CantidadAutorizada", HeaderText = "Autorizado", FillWeight = 80, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCantidadEjecutada", DataPropertyName = "CantidadEjecutada", HeaderText = "Realizado", FillWeight = 80, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colObservaciones", DataPropertyName = "Observaciones", HeaderText = "Motivo / registro técnico", FillWeight = 190, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable });
        dgvItems.ReadOnly = false;
        dgvItems.Columns[0].ReadOnly = true;
        dgvItems.Columns[1].ReadOnly = true;
        dgvItems.Columns.Insert(3, new DataGridViewComboBoxColumn { Name = "colEstado", DataPropertyName = "Estado", HeaderText = "Estado", DataSource = Enum.GetValues<Taller.Dominio.Enumeraciones.EstadoEjecucionDetalle>().Select(estado => new KeyValuePair<Taller.Dominio.Enumeraciones.EstadoEjecucionDetalle, string>(estado, new Taller.Aplicacion.DTOs.Ejecuciones.EjecucionItemDto { Estado = estado }.EstadoDescripcion)).ToList(), ValueMember = "Key", DisplayMember = "Value", FillWeight = 95, MinimumWidth = 125 });
        dgvItems.CellValueChanged += CeldaCambiada;
        dgvItems.CurrentCellDirtyStateChanged += ConfirmarEstado;
        dgvItems.DataError += ErrorCelda;
        txtObservaciones.Multiline = true;
        txtObservaciones.MaxLength = 1000;
        txtObservaciones.ScrollBars = ScrollBars.Vertical;
        txtObservaciones.PlaceholderText = "Revisión del alcance, resultado del trabajo o observaciones del avance.";
        txtObservaciones.TextChanged += TextoCambiado;
        btnTomar.Text = "Iniciar ejecución";
        btnTomar.AutoSize = true;
        btnTomar.Height = 34;
        btnTomar.Click += Tomar;

        btnGuardar.Text = "Guardar avance";
        btnGuardar.AutoSize = true;
        btnGuardar.Height = 34;
        btnGuardar.Click += Guardar;
        flpAcciones.Controls.Add(btnGuardar);
        btnCompletar = new Button { Name = "btnCompletarSeleccionada", Text = "Completar fila seleccionada", AutoSize = true, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(30, 64, 175) };
        btnCompletar.Click += CompletarSeleccionada;
        flpAcciones.Controls.Add(btnCompletar);
        btnLiberar.Text = "Liberar actividad";
        btnLiberar.AutoSize = true;
        btnLiberar.Height = 34;
        btnLiberar.Click += Liberar;

        btnFinalizar.Text = "Finalizar trabajo";
        btnFinalizar.AutoSize = true;
        btnFinalizar.Height = 34;
        btnFinalizar.Click += Finalizar;
        flpAcciones.Controls.Add(btnFinalizar);

        btnSeguimiento = new Button
        {
            Name = "btnSeguimiento",
            Text = "Hallazgos, pausas y materiales",
            AutoSize = true,
            Height = 34
        };
        btnSeguimiento.Click += Seguimiento;
        chkConformidad = new CheckBox { Name = "chkConformidadTecnica", Text = "Comprobé el resultado y doy conformidad técnica", AutoSize = true };
        chkConformidad.CheckedChanged += ActualizarConformidad;
        tlpPrincipal.Controls.Add(chkConformidad, 0, 4);
        flpAcciones.AutoSize = true;
        flpAcciones.WrapContents = true;


        btnCerrar.Text = "Cerrar";
        btnCerrar.AutoSize = true;
        btnCerrar.Height = 34;
        btnCerrar.Click += Cerrar;
        flpAcciones.Controls.Add(btnCerrar);
        Load += Cargar;
        FormClosing += AlCerrar;
        BackColor = Color.FromArgb(248, 250, 252);
        btnFinalizar.BackColor = Color.FromArgb(30, 64, 175);
        btnFinalizar.ForeColor = Color.White;
        btnFinalizar.FlatStyle = FlatStyle.Flat;
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(btnCerrar);
        CancelButton = btnCerrar;
        btnCerrar.DialogResult = DialogResult.None;
        txtResumen.ReadOnly = true;
        txtResumen.Multiline = true;
        txtResumen.ScrollBars = ScrollBars.Vertical;
        txtResumen.TabStop = false;
        var btnActualizarSituacion = new Button { Text = "Actualizar situación", AutoSize = true };
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(btnActualizarSituacion);
        btnActualizarSituacion.Click += ActualizarSituacion;
        flpAcciones.Controls.Add(btnActualizarSituacion);
        menuOpciones = new ContextMenuStrip(components);
        menuOpciones.Items.Add("Registrar hallazgo / solicitar adicional", null, Hallazgos);
        menuOpciones.Items.Add("Gestionar pausas", null, Pausas);
        menuOpciones.Items.Add("Registrar material del cliente", null, Materiales);
        menuOpciones.Items.Add(new ToolStripSeparator());
        menuOpciones.Items.Add("Dejar disponible para otro técnico", null, Liberar);
        btnOtrasAcciones = new Button { Name = "btnOtrasAcciones", Text = "Otras acciones…", AutoSize = true, Height = 34 };
        btnOtrasAcciones.Click += MostrarOpciones;
        flpAcciones.Controls.Add(btnOtrasAcciones);
        flpAcciones.Controls.SetChildIndex(btnCerrar, flpAcciones.Controls.Count - 1);
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(btnOtrasAcciones);
        foreach (var boton in new[] { btnTomar, btnGuardar, btnLiberar, btnSeguimiento, btnCerrar }) Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(boton);
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonPrimario(btnTomar);
        flpAcciones.AutoScroll = true;
        var ayuda = new ToolTip(components) { AutoPopDelay = 20000 };
        ayuda.SetToolTip(dgvItems, "Completado aplica toda la cantidad autorizada. Alcanzar esa cantidad también marca Completado. En curso admite cero mientras se trabaja. Guardar hace visible el avance a recepción.");
        ayuda.SetToolTip(chkConformidad, "Confirma, bajo responsabilidad del técnico, que comprobó el resultado final. Solo se requiere al finalizar normalmente; no para guardar avances.");
        ayuda.SetToolTip(txtObservaciones, "Resumen general opcional de los avances y de la comprobación final. Si falta material, guarde el avance y registre una pausa.");
        ayuda.SetToolTip(btnLiberar, "Conserva los avances guardados y deja la ejecución disponible para otro técnico. Cerrar la ventana conserva la reserva.");
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonPrimario(btnFinalizar);
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpPrincipal = null!;
    private TextBox txtResumen = null!;
    private Label lblAviso = null!;
    private DataGridView dgvItems = null!;
    private TextBox txtObservaciones = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private CheckBox chkConformidad = null!;
    private ContextMenuStrip menuOpciones = null!;
    private Button btnOtrasAcciones = null!;
    private Button btnCompletar = null!;
    private Button btnTomar = null!;
    private Button btnGuardar = null!;
    private Button btnLiberar = null!;
    private Button btnFinalizar = null!;
    private Button btnSeguimiento = null!;
    private Button btnCerrar = null!;
}

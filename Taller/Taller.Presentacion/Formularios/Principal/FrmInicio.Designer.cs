#nullable enable
namespace Taller.Presentacion.Formularios.Principal;
partial class FrmInicio
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
        lblBienvenida = new Label();
        lblDescripcion = new Label();
        txtIndicadores = new TextBox();
        dgvActividad = new DataGridView();
        lblEstado = new Label();
        btnActualizar = new Button();
        tlpPrincipal.Name = "tlpPrincipal";
        lblBienvenida.Name = "lblBienvenida";
        lblDescripcion.Name = "lblDescripcion";
        txtIndicadores.Name = "txtIndicadores";
        dgvActividad.Name = "dgvActividad";
        lblEstado.Name = "lblEstado";
        btnActualizar.Name = "btnActualizar";
        reloj = new System.Windows.Forms.Timer(components)
        {
            Interval = 1000
        };
        reloj.Tick += Reloj_Tick;
        lblReloj = new Label
        {
            Name = "lblReloj",
            Dock = DockStyle.Top,
            Height = 26,
            TextAlign = ContentAlignment.MiddleLeft
        };
        calendario = new MonthCalendar
        {
            Name = "calendario",
            MaxSelectionCount = 1,
            Dock = DockStyle.Top
        };
        pnlResumen = new TableLayoutPanel
        {
            Name = "pnlResumen",
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        pnlResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        pnlResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));
        pnlCalendario = new Panel
        {
            Name = "pnlCalendario",
            Dock = DockStyle.Fill
        };
        pnlCalendario.Controls.Add(calendario);
        pnlCalendario.Controls.Add(lblReloj);
        pnlResumen.Controls.Add(txtIndicadores, 0, 0);
        pnlResumen.Controls.Add(pnlCalendario, 1, 0);
        SuspendLayout();
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 9F);
        ClientSize = new Size(1020, 620);
        MinimumSize = new Size(760, 480);
        BackColor = Color.FromArgb(244, 247, 251);
        Text = "Inicio";
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Padding = new Padding(16);
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.RowCount = 6;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 210));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        Controls.Add(tlpPrincipal);
        tlpPrincipal.Controls.Add(lblBienvenida, 0, 0);
        tlpPrincipal.Controls.Add(lblDescripcion, 0, 1);
        tlpPrincipal.Controls.Add(pnlResumen, 0, 2);
        tlpPrincipal.Controls.Add(dgvActividad, 0, 3);
        tlpPrincipal.Controls.Add(lblEstado, 0, 4);
        tlpPrincipal.Controls.Add(btnActualizar, 0, 5);
        lblBienvenida.Dock = DockStyle.Fill;
        lblBienvenida.Font = new Font("Segoe UI", 18, FontStyle.Bold);
        lblBienvenida.Text = "Bienvenido";
        lblDescripcion.Dock = DockStyle.Fill;
        lblDescripcion.Text = "Resumen de datos reales según su rol. Últimos 20 registros de las consultas resumidas, según su fecha de referencia.";
        txtIndicadores.Dock = DockStyle.Fill;
        txtIndicadores.Multiline = true;
        txtIndicadores.ReadOnly = true;
        txtIndicadores.ScrollBars = ScrollBars.Vertical;
        txtIndicadores.BackColor = Color.White;
        dgvActividad.Dock = DockStyle.Fill;
        dgvActividad.AutoGenerateColumns = false;
        dgvActividad.ReadOnly = true;
        dgvActividad.AllowUserToAddRows = false;
        dgvActividad.AllowUserToDeleteRows = false;
        dgvActividad.AllowUserToResizeColumns = true;
        dgvActividad.RowHeadersVisible = false;
        dgvActividad.BackgroundColor = Color.White;
        dgvActividad.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        colFecha = new DataGridViewTextBoxColumn
        {
            Name = "colFecha",
            DataPropertyName = "Fecha",
            HeaderText = "Fecha",
            MinimumWidth = 110,
            FillWeight = 100
        };
        colFecha.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        colCodigo = new DataGridViewTextBoxColumn
        {
            Name = "colCodigo",
            DataPropertyName = "Codigo",
            HeaderText = "Código",
            MinimumWidth = 65,
            FillWeight = 70
        };
        colReporte = new DataGridViewTextBoxColumn
        {
            Name = "colReporte",
            DataPropertyName = "Reporte",
            HeaderText = "Consulta",
            MinimumWidth = 100,
            FillWeight = 150
        };
        colDescripcion = new DataGridViewTextBoxColumn
        {
            Name = "colDescripcion",
            DataPropertyName = "Descripcion",
            HeaderText = "Descripción",
            MinimumWidth = 180,
            FillWeight = 280
        };
        colEstado = new DataGridViewTextBoxColumn
        {
            Name = "colEstado",
            DataPropertyName = "Estado",
            HeaderText = "Estado / acción",
            MinimumWidth = 100,
            FillWeight = 120
        };
        dgvActividad.Columns.AddRange([colFecha, colCodigo, colDescripcion, colEstado]);
        lblEstado.Dock = DockStyle.Fill;
        btnActualizar.Text = "Actualizar resumen";
        btnActualizar.AutoSize = true;
        btnActualizar.Anchor = AnchorStyles.Left;
        btnActualizar.Click += BtnActualizar_Click;
        Load += FrmInicio_Load;
        FormClosing += FrmInicio_FormClosing;
        ResumeLayout(false);
    }

    private System.Windows.Forms.Timer reloj = null!;
    private Label lblReloj = null!;
    private MonthCalendar calendario = null!;
    private TableLayoutPanel pnlResumen = null!;
    private Panel pnlCalendario = null!;
    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblBienvenida = null!, lblDescripcion = null!, lblEstado = null!;
    private TextBox txtIndicadores = null!;
    private DataGridView dgvActividad = null!;
    private DataGridViewTextBoxColumn colFecha = null!, colCodigo = null!, colReporte = null!, colDescripcion = null!, colEstado = null!;
    private Button btnActualizar = null!;
}

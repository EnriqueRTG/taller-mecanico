using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Formularios.Reportes;

/// <summary>
/// Presenta reportes demostrativos adaptados al rol autenticado.
/// </summary>
public partial class FrmReportes : Form
{
    private readonly SesionUsuario _sesionUsuario;
    private readonly List<ReporteFila> _resultados = [];
    private bool _configurando;

    public FrmReportes(
        UsuarioServicio usuarioServicio,
        SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        _ = usuarioServicio
            ?? throw new ArgumentNullException(nameof(usuarioServicio));

        _sesionUsuario = sesionUsuario
            ?? throw new ArgumentNullException(nameof(sesionUsuario));

        ConfigurarFormulario();
        ConectarEventosAdicionales();
        CargarTiposSegunRol();
        ActualizarEstadoAcciones();
    }

    private void ConfigurarFormulario()
    {
        _configurando = true;

        desde.MaxDate = DateTime.Today;
        hasta.MaxDate = DateTime.Today;
        desde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        hasta.Value = DateTime.Today;

        tipo_reporte.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;

        txtBuscar.PlaceholderText = "Código, descripción o responsable";

        btnNuevoUsuario.Text = "Ver detalle";
        btnNuevoUsuario.Size = new Size(125, 34);
        btnNuevoUsuario.Visible = true;
        ReubicarBotonDetalle();

        btn_generar.Text = "Generar reporte";
        btn_actualizar.Text = "Exportar";
        button2.Text = "Limpiar";
        button1.Text = "Imprimir";

        ConfigurarGrilla();
        _configurando = false;
    }

    private void ConfigurarGrilla()
    {
        dgvUsuarios.AutoGenerateColumns = false;
        dgvUsuarios.Columns.Clear();
        dgvUsuarios.MultiSelect = false;
        dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgvUsuarios.Columns.Add(CrearColumna("colCodigo", "Código", 70));
        dgvUsuarios.Columns.Add(CrearColumna("colFecha", "Fecha", 85));
        dgvUsuarios.Columns.Add(CrearColumna("colDescripcion", "Descripción", 190));
        dgvUsuarios.Columns.Add(CrearColumna("colResponsable", "Responsable", 125));
        dgvUsuarios.Columns.Add(CrearColumna("colEstadoReporte", "Estado", 90));
        dgvUsuarios.Columns.Add(CrearColumna("colTotal", "Importe / cantidad", 100));
    }

    private static DataGridViewTextBoxColumn CrearColumna(
        string nombre,
        string encabezado,
        float peso)
    {
        return new DataGridViewTextBoxColumn
        {
            Name = nombre,
            HeaderText = encabezado,
            DataPropertyName = nombre switch
            {
                "colCodigo" => nameof(ReporteFila.Codigo),
                "colFecha" => nameof(ReporteFila.FechaTexto),
                "colDescripcion" => nameof(ReporteFila.Descripcion),
                "colResponsable" => nameof(ReporteFila.Responsable),
                "colEstadoReporte" => nameof(ReporteFila.Estado),
                _ => nameof(ReporteFila.Valor)
            },
            FillWeight = peso,
            ReadOnly = true
        };
    }

    private void ConectarEventosAdicionales()
    {
        btnNuevoUsuario.Click += BtnVerDetalle_Click;
        button2.Click += BtnLimpiar_Click;
        button1.Click += BtnImprimir_Click;
        txtBuscar.TextChanged += Filtro_Cambio;
        cboEstado.SelectedIndexChanged += Filtro_Cambio;
        dgvUsuarios.SelectionChanged += DgvReportes_SelectionChanged;
        dgvUsuarios.CellDoubleClick += DgvReportes_CellDoubleClick;
        pnlCabecera.Resize += (_, _) => ReubicarBotonDetalle();
    }

    private void ReubicarBotonDetalle()
    {
        btnNuevoUsuario.Location = new Point(
            Math.Max(20, pnlCabecera.ClientSize.Width - btnNuevoUsuario.Width - 20),
            18);
    }

    private string ObtenerRolActual()
    {
        return _sesionUsuario.UsuarioActual?.Rol?.Nombre?.Trim()
            ?? "Administrador";
    }

    private void CargarTiposSegunRol()
    {
        _configurando = true;

        string rol = ObtenerRolActual();
        string rolNormalizado = rol.ToLowerInvariant();

        tipo_reporte.Items.Clear();

        if (rolNormalizado.Contains("técnico")
            || rolNormalizado.Contains("tecnico"))
        {
            tipo_reporte.Items.AddRange(
            [
                "Atenciones asignadas",
                "Diagnósticos realizados",
                "Presupuestos confeccionados",
                "Trabajos pendientes y finalizados"
            ]);
        }
        else if (rolNormalizado.Contains("administrativo"))
        {
            tipo_reporte.Items.AddRange(
            [
                "Comprobantes y pagos",
                "Atenciones registradas",
                "Entregas de vehículos",
                "Clientes y vehículos atendidos"
            ]);
        }
        else
        {
            tipo_reporte.Items.AddRange(
            [
                "Ingresos y pagos por período",
                "Atenciones por estado",
                "Rendimiento de técnicos",
                "Servicios más realizados",
                "Presupuestos aprobados y rechazados"
            ]);
        }

        lblDescripcion.Text =
            $"Consulta y generación simulada de reportes · Rol: {rol}";

        tipo_reporte.SelectedIndex = tipo_reporte.Items.Count > 0 ? 0 : -1;
        _configurando = false;

        ConfigurarEstadosSegunTipo();
    }

    private void ConfigurarEstadosSegunTipo()
    {
        string tipo = tipo_reporte.SelectedItem?.ToString() ?? string.Empty;
        string seleccionado = cboEstado.SelectedItem?.ToString() ?? "Todos";

        _configurando = true;
        cboEstado.Items.Clear();
        cboEstado.Items.Add("Todos");

        if (tipo.Contains("pago", StringComparison.OrdinalIgnoreCase)
            || tipo.Contains("presupuesto", StringComparison.OrdinalIgnoreCase)
            || tipo.Contains("ingreso", StringComparison.OrdinalIgnoreCase))
        {
            cboEstado.Items.AddRange(["Pagado", "Pendiente", "Aprobado", "Rechazado"]);
        }
        else
        {
            cboEstado.Items.AddRange(["Pendiente", "En proceso", "Finalizada"]);
        }

        int indice = cboEstado.Items.IndexOf(seleccionado);
        cboEstado.SelectedIndex = indice >= 0 ? indice : 0;
        _configurando = false;
    }

    private bool ValidarFechas()
    {
        if (desde.Value.Date <= hasta.Value.Date)
            return true;

        MessageBox.Show(
            "La fecha desde no puede ser mayor que la fecha hasta.",
            "Período inválido",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        desde.Focus();
        return false;
    }

    private void GenerarReporte()
    {
        if (!ValidarFechas())
            return;

        if (tipo_reporte.SelectedItem is null)
        {
            MessageBox.Show(
                "Seleccione el tipo de reporte que desea generar.",
                "Tipo de reporte requerido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        string tipo = tipo_reporte.SelectedItem.ToString()!;
        _resultados.Clear();

        _resultados.AddRange(CrearResultadosSimulados(tipo));
        AplicarFiltros();

        MessageBox.Show(
            $"Se generó el reporte \"{tipo}\" con datos demostrativos.",
            "Reporte generado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private IEnumerable<ReporteFila> CrearResultadosSimulados(string tipo)
    {
        DateTime fechaBase = hasta.Value.Date;

        if (tipo.Contains("pago", StringComparison.OrdinalIgnoreCase)
            || tipo.Contains("ingreso", StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                new("R-001", fechaBase.AddDays(-8), "Pago de reparación integral",
                    "María López", "Pagado", "$ 185.000"),
                new("R-002", fechaBase.AddDays(-5), "Seña de presupuesto",
                    "Carlos Ruiz", "Pendiente", "$ 48.500"),
                new("R-003", fechaBase.AddDays(-2), "Pago de mantenimiento",
                    "Laura Gómez", "Pagado", "$ 72.300")
            ];
        }

        if (tipo.Contains("presupuesto", StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                new("P-031", fechaBase.AddDays(-10), "Reparación de tren delantero",
                    "Téc. Martín Silva", "Aprobado", "$ 210.000"),
                new("P-034", fechaBase.AddDays(-6), "Cambio de distribución",
                    "Téc. Ana Torres", "Pendiente", "$ 165.000"),
                new("P-038", fechaBase.AddDays(-1), "Reparación eléctrica",
                    "Téc. Martín Silva", "Rechazado", "$ 94.500")
            ];
        }

        if (tipo.Contains("diagnóstico", StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                new("D-018", fechaBase.AddDays(-7), "Falla en sistema de encendido",
                    "Téc. Ana Torres", "Finalizada", "1 diagnóstico"),
                new("D-021", fechaBase.AddDays(-4), "Ruido en tren delantero",
                    "Téc. Martín Silva", "Finalizada", "1 diagnóstico"),
                new("D-024", fechaBase.AddDays(-1), "Pérdida de refrigerante",
                    "Téc. Ana Torres", "En proceso", "1 diagnóstico")
            ];
        }

        return
        [
            new("A-104", fechaBase.AddDays(-9), $"{tipo}: mantenimiento general",
                "Téc. Martín Silva", "Finalizada", "3 servicios"),
            new("A-108", fechaBase.AddDays(-5), $"{tipo}: revisión de frenos",
                "Téc. Ana Torres", "En proceso", "2 servicios"),
            new("A-112", fechaBase.AddDays(-2), $"{tipo}: falla de arranque",
                "Sin asignar", "Pendiente", "1 servicio")
        ];
    }

    private void AplicarFiltros()
    {
        if (_configurando)
            return;

        string busqueda = txtBuscar.Text.Trim();
        string estado = cboEstado.SelectedItem?.ToString() ?? "Todos";

        IEnumerable<ReporteFila> consulta = _resultados.Where(fila =>
            fila.Fecha.Date >= desde.Value.Date
            && fila.Fecha.Date <= hasta.Value.Date);

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(fila =>
                fila.Codigo.Contains(busqueda, StringComparison.OrdinalIgnoreCase)
                || fila.Descripcion.Contains(busqueda, StringComparison.OrdinalIgnoreCase)
                || fila.Responsable.Contains(busqueda, StringComparison.OrdinalIgnoreCase));
        }

        if (estado != "Todos")
        {
            consulta = consulta.Where(fila =>
                fila.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase));
        }

        List<ReporteFila> visibles = consulta.ToList();

        dgvUsuarios.DataSource = null;
        dgvUsuarios.DataSource = visibles;
        dgvUsuarios.ClearSelection();
        dgvUsuarios.CurrentCell = null;

        lblCantidad.Text = visibles.Count == 1
            ? "1 resultado encontrado"
            : $"{visibles.Count} resultados encontrados";

        ActualizarEstadoAcciones();
    }

    private ReporteFila? ObtenerFilaSeleccionada()
    {
        return dgvUsuarios.SelectedRows.Count == 1
            ? dgvUsuarios.SelectedRows[0].DataBoundItem as ReporteFila
            : null;
    }

    private void ActualizarEstadoAcciones()
    {
        bool hayResultados = dgvUsuarios.Rows.Count > 0;

        btnNuevoUsuario.Enabled = ObtenerFilaSeleccionada() is not null;
        btn_actualizar.Enabled = hayResultados;
        button1.Enabled = hayResultados;
    }

    private void BtnVerDetalle_Click(object? sender, EventArgs e)
    {
        ReporteFila? fila = ObtenerFilaSeleccionada();
        if (fila is null)
            return;

        using var formulario = new FrmDetalleReporte(
            tipo_reporte.SelectedItem?.ToString() ?? "Reporte",
            ObtenerRolActual(),
            desde.Value.Date,
            hasta.Value.Date,
            fila.Codigo,
            fila.Descripcion,
            fila.Responsable,
            fila.Estado,
            fila.Valor);

        formulario.ShowDialog(this);
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        _configurando = true;
        txtBuscar.Clear();
        desde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        hasta.Value = DateTime.Today;
        cboEstado.SelectedIndex = cboEstado.Items.Count > 0 ? 0 : -1;
        _resultados.Clear();
        dgvUsuarios.DataSource = null;
        lblCantidad.Text = "0 resultados encontrados";
        _configurando = false;
        ActualizarEstadoAcciones();
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(
            "Se abrirá la vista de impresión cuando se implemente la generación real del reporte.",
            "Impresión simulada",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void btn_actualizar_Click(object sender, EventArgs e)
    {
        if (!ValidarFechas() || dgvUsuarios.Rows.Count == 0)
            return;

        MessageBox.Show(
            "El reporte se exportará a PDF o Excel cuando se implemente la funcionalidad definitiva.",
            "Exportación simulada",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void btn_generar_Click(object sender, EventArgs e) => GenerarReporte();

    private void tipo_reporte_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_configurando)
            return;

        ConfigurarEstadosSegunTipo();
        _resultados.Clear();
        dgvUsuarios.DataSource = null;
        lblCantidad.Text = "0 resultados encontrados";
        ActualizarEstadoAcciones();
    }

    private void desde_ValueChanged(object sender, EventArgs e)
    {
        if (!_configurando && _resultados.Count > 0)
            AplicarFiltros();
    }

    private void hasta_ValueChanged(object sender, EventArgs e)
    {
        if (!_configurando && _resultados.Count > 0)
            AplicarFiltros();
    }

    private void Filtro_Cambio(object? sender, EventArgs e)
    {
        if (_resultados.Count > 0)
            AplicarFiltros();
    }

    private void DgvReportes_SelectionChanged(object? sender, EventArgs e) =>
        ActualizarEstadoAcciones();

    private void DgvReportes_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            BtnVerDetalle_Click(sender, EventArgs.Empty);
    }

    private void pnlFiltros_Paint(object sender, PaintEventArgs e)
    {
    }

    private sealed class ReporteFila
    {
        public ReporteFila(
            string codigo,
            DateTime fecha,
            string descripcion,
            string responsable,
            string estado,
            string valor)
        {
            Codigo = codigo;
            Fecha = fecha;
            Descripcion = descripcion;
            Responsable = responsable;
            Estado = estado;
            Valor = valor;
        }

        public string Codigo { get; }
        public DateTime Fecha { get; }
        public string FechaTexto => Fecha.ToString("dd/MM/yyyy");
        public string Descripcion { get; }
        public string Responsable { get; }
        public string Estado { get; }
        public string Valor { get; }
    }
}

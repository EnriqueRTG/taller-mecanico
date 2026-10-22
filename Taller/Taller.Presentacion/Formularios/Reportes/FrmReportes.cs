using System.Text;
using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Reportes;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Impresion;
using Taller.Presentacion.Navegacion;

namespace Taller.Presentacion.Formularios.Reportes;
/// <summary>Genera documentos por rol. Los filtros modificados invalidan el documento anterior antes de exportarlo.</summary>
public partial class FrmReportes : Form, IFormularioConOperacion
{
    private readonly ReporteServicio _servicio;
    private ReporteDocumentoDto? _documento;
    private bool _configurando;
    public bool OperacionEnCurso { get; private set; }

    public FrmReportes(ReporteServicio servicio)
    {
        InitializeComponent();
        _servicio = servicio;
    }

    private async void FrmReportes_Load(object? sender, EventArgs e)
    {
        try
        {
            OperacionEnCurso = true;
            pnlFiltros.Enabled = flpAcciones.Enabled = false;
            _configurando = true;
            dtpHasta.MaxDate = dtpDesde.MaxDate = DateTime.Today;
            dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpHasta.Value = DateTime.Today;
            cboTipo.DisplayMember = "Nombre";
            cboTipo.ValueMember = "Tipo";
            cboEstado.Items.Add("Todos");
            cboEstado.SelectedIndex = 0;
            cboTipo.DataSource = (await _servicio.ListarTiposAsync()).Select(t => new Opcion(t, ReporteServicio.Nombre(t))).ToList();
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Reportes");
        }
        finally
        {
            _configurando = false;
            OperacionEnCurso = false;
            pnlFiltros.Enabled = flpAcciones.Enabled = true;
            ConfigurarPeriodo();
            Invalidar();
        }
    }

    private sealed record Opcion(TipoReporte Tipo, string Nombre);
    private void ConfigurarPeriodo()
    {
        bool periodo = (cboTipo.SelectedItem as Opcion)?.Tipo is not (TipoReporte.SaldosPendientes or TipoReporte.CargaTecnica);
        dtpDesde.Enabled = dtpHasta.Enabled = periodo;
        lblMetodo.Text = periodo ? "El período utiliza la fecha del registro indicada en cada reporte. Presione Generar para consultar." : "Situación actual completa, incluyendo registros antiguos. Este reporte no utiliza filtro de fechas.";
    }

    private void Invalidar()
    {
        if (_configurando)
            return;
        _documento = null;
        dgvDetalle.DataSource = null;
        txtResumen.Clear();
        pnlGrafico.Invalidate();
        btnExportar.Enabled = btnCsv.Enabled = btnImprimir.Enabled = false;
        lblCantidad.Text = "Configure los filtros y genere el documento.";
    }

    private void Filtro_Cambio(object? sender, EventArgs e) => Invalidar();
    private void CboTipo_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_configurando)
            return;
        _configurando = true;
        cboEstado.Items.Clear();
        cboEstado.Items.Add("Todos");
        cboEstado.SelectedIndex = 0;
        _configurando = false;
        ConfigurarPeriodo();
        Invalidar();
    }

    private async void BtnGenerar_Click(object? sender, EventArgs e)
    {
        if (OperacionEnCurso || cboTipo.SelectedItem is not Opcion opcion)
            return;
        try
        {
            OperacionEnCurso = true;
            pnlFiltros.Enabled = false;
            flpAcciones.Enabled = false;
            Cursor = Cursors.WaitCursor;
            _documento = await _servicio.GenerarAsync(new SolicitudReporteDto { Tipo = opcion.Tipo, Desde = dtpDesde.Value.Date, Hasta = dtpHasta.Value.Date, Busqueda = txtBuscar.Text, Estado = cboEstado.SelectedItem?.ToString() ?? "Todos" });
            // Actualiza opciones sin cambiar el filtro elegido ni regenerar con parámetros distintos.
            _configurando = true;
            string estado = cboEstado.SelectedItem?.ToString() ?? "Todos";
            if (estado == "Todos")
            {
                cboEstado.Items.Clear();
                cboEstado.Items.Add("Todos");
                foreach (var valor in _documento.Registros.Select(x => ReporteServicio.Legible(x.Estado)).Distinct().Order())
                    cboEstado.Items.Add(valor);
                cboEstado.SelectedIndex = 0;
            }

            _configurando = false;
            dgvDetalle.DataSource = _documento.Registros.Select(x => new { x.Codigo, Atencion = x.IdAtencion, x.Fecha, x.Descripcion, x.Responsable, Estado = ReporteServicio.Legible(x.Estado), x.Cantidad, x.Importe, x.ImporteAceptado }).ToList();
            dgvDetalle.ClearSelection();
            txtResumen.Text = string.Join("   |   ", _documento.Indicadores.Select(x => x.Nombre + ": " + x.Valor)) + Environment.NewLine + _documento.Recomendacion + Environment.NewLine + _documento.Metodologia;
            lblMetodo.Text = _documento.Alcance + " · " + _documento.Filtros;
            lblCantidad.Text = $"{_documento.Registros.Count} registros · generado {_documento.Generado:dd/MM/yyyy HH:mm:ss} · gráfico en {_documento.UnidadGrafico}";
            btnExportar.Enabled = btnCsv.Enabled = btnImprimir.Enabled = true;
            pnlGrafico.Invalidate();
        }
        catch (Exception ex)
        {
            _configurando = false;
            Invalidar();
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Generar reporte");
        }
        finally
        {
            _configurando = false;
            OperacionEnCurso = false;
            pnlFiltros.Enabled = flpAcciones.Enabled = true;
            Cursor = Cursors.Default;
            ConfigurarControlesPeriodo();
        }
    }

    private void ConfigurarControlesPeriodo()
    {
        bool p = (cboTipo.SelectedItem as Opcion)?.Tipo is not (TipoReporte.SaldosPendientes or TipoReporte.CargaTecnica);
        dtpDesde.Enabled = dtpHasta.Enabled = p;
    }

    private void PnlGrafico_Paint(object? sender, PaintEventArgs e)
    {
        if (_documento is { } d)
            GraficoReporte.Dibujar(e.Graphics, Rectangle.Inflate(pnlGrafico.ClientRectangle, -12, -8), d);
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        _configurando = true;
        txtBuscar.Clear();
        cboEstado.SelectedIndex = cboEstado.Items.Count > 0 ? 0 : -1;
        dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpHasta.Value = DateTime.Today;
        _configurando = false;
        ConfigurarPeriodo();
        Invalidar();
    }

    private void Exportar(bool csv)
    {
        if (_documento is not { } d)
            return;
        using var guardar = new SaveFileDialog
        {
            Filter = csv ? "Detalle CSV (*.csv)|*.csv" : "Documento HTML con gráfico (*.html)|*.html",
            FileName = $"SGTM-{d.Generado:yyyyMMdd-HHmmss}.{(csv ? "csv" : "html")}",
            AddExtension = true
        };
        if (guardar.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            File.WriteAllText(guardar.FileName, csv ? DocumentoReporteExportador.Csv(d) : DocumentoReporteExportador.Html(d), new UTF8Encoding(csv));
            MessageBox.Show(this, "Archivo guardado correctamente.", "Exportar reporte");
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Exportar reporte");
        }
    }

    private void BtnExportar_Click(object? sender, EventArgs e) => Exportar(false);
    private void BtnCsv_Click(object? sender, EventArgs e) => Exportar(true);
    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        if (_documento is not { } d)
            return;
        try
        {
            ImpresionReporte.Mostrar(this, d);
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Imprimir reporte");
        }
    }

    private void FrmReportes_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (OperacionEnCurso)
            e.Cancel = true;
    }
}

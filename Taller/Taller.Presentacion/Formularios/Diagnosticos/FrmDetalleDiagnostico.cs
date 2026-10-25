using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Diagnosticos;
/// <summary>
/// Presenta un informe técnico registrado, sin permitir modificaciones.
/// </summary>
public partial class FrmDetalleDiagnostico : Form
{
    private readonly int _idDiagnostico;
    private readonly DiagnosticoServicio? _diagnosticoServicio;
    private Taller.Aplicacion.DTOs.Diagnosticos.DiagnosticoDetalleDto? _datos;
    private Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto? _empresa;
    public FrmDetalleDiagnostico()
    {
        InitializeComponent();
    }

    public FrmDetalleDiagnostico(int idDiagnostico, DiagnosticoServicio diagnosticoServicio, Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto empresa) : this()
    {
        if (idDiagnostico <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idDiagnostico));
        }

        _idDiagnostico = idDiagnostico;
        _empresa = empresa;
        _diagnosticoServicio = diagnosticoServicio ?? throw new ArgumentNullException(nameof(diagnosticoServicio));
    }

    private string NombreArchivo => Taller.Aplicacion.Reportes.DocumentoReporteExportador.NombreArchivo("Informe-tecnico-" + _idDiagnostico.ToString("D6"), Taller.Dominio.Tiempo.FechaHoraTaller.Ahora);
    private void Exportar(object? s, EventArgs e)
    {
        if (_datos is null || _empresa is null) return;
        try { Taller.Presentacion.Impresion.ExportacionDocumento.GuardarHtml(this, Taller.Aplicacion.Reportes.DocumentoOperativoExportador.Diagnostico(_datos, _empresa), NombreArchivo); }
        catch (Exception ex) { Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Exportar informe"); }
    }
    private void Imprimir(object? s, EventArgs e)
    {
        if (_datos is null || _empresa is null) return;
        try { Taller.Presentacion.Impresion.ImpresionDocumento.Mostrar(this, Taller.Aplicacion.Reportes.DocumentoOperativoExportador.TextoDiagnostico(_datos), _empresa, NombreArchivo); }
        catch (Exception ex) { Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Imprimir informe"); }
    }
    private async void FrmDetalleDiagnostico_Load(object? sender, EventArgs e)
    {
        EstilosInterfaz.AplicarBotonSecundario(btnCerrar);
        DisposicionVentanas.AjustarAlAreaDisponible(this);
        try
        {
            if (_diagnosticoServicio is null)
            {
                throw new InvalidOperationException("Debe seleccionar un diagnóstico para consultarlo.");
            }

            var datos = await _diagnosticoServicio.ObtenerDetalleAsync(_idDiagnostico);
            if (IsDisposed)
            {
                return;
            }

            _datos = datos;
            btnExportar.Enabled = btnImprimir.Enabled = true;
            Text = $"Diagnóstico N.º {datos.IdDiagnostico:D6}";
            txtAtencion.Text = datos.IdAtencion.ToString("D6");
            txtFecha.Text = datos.FechaDiagnostico.ToString("dd/MM/yyyy HH:mm");
            txtTecnicoAutor.Text = datos.TecnicoAutor;
            txtTipo.Text = datos.Tipo.ToString();
            txtKilometraje.Text = datos.Kilometraje?.ToString() ?? "No registrado";
            txtFallaDetectada.Text = datos.FallaDetectada;
            txtTrabajoRecomendado.Text = datos.TrabajoRecomendado;
            txtObservaciones.Text = datos.Observaciones ?? "Sin observaciones.";
        }
        catch (Exception ex)
        {
            if (!IsDisposed)
            {
                Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Consultar diagnóstico");
                Close();
            }
        }
    }
}

using Taller.Aplicacion.DTOs.Atenciones;
namespace Taller.Presentacion.Formularios.Presupuestos;
/// <summary>Consulta inmutable de un presupuesto persistido; los importes no equivalen a un cobro.</summary>
public partial class FrmConsultarPropuesta : Form
{
    private readonly PresupuestoConsultaDto _datos;
    private readonly Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto _empresa;
    public FrmConsultarPropuesta(PresupuestoConsultaDto datos, Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto empresa)
    {
        InitializeComponent();
        Taller.Presentacion.Estilos.CabeceraOperacion.Aplicar(lblResumen);
        foreach (var boton in new[] { btnExportar, btnImprimir, btnCerrar }) Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(boton);
        _datos = datos;
        _empresa = empresa;
        Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);
        lblResumen.Text = $"Presupuesto {datos.IdPresupuesto:D6} · Atención {datos.IdAtencion:D6} · {datos.Cliente} · {datos.Vehiculo}\nTipo: {datos.TipoDescripcion} · Estado: {datos.EstadoDescripcion} · {(datos.Vigente ? "Vigente" : "Histórico")} · Revisa: {datos.IdPresupuestoAnterior?.ToString() ?? "—"}";
        dgvItems.DataSource = datos.Items;
        txtObservaciones.Text = datos.Observaciones + (string.IsNullOrWhiteSpace(datos.PersonaDecision) ? "" : $"\r\nCliente / representante: {datos.PersonaDecision} · Medio: {datos.MedioDecision}");
        lblTotal.Text = $"Total del documento: {datos.Total:N2}. No es un comprobante de pago. Solo se liquida lo autorizado y efectivamente realizado.";
    }
    private string NombreArchivo => Taller.Aplicacion.Reportes.DocumentoReporteExportador.NombreArchivo("Presupuesto-" + _datos.IdPresupuesto.ToString("D6"), Taller.Dominio.Tiempo.FechaHoraTaller.Ahora);
    private void Exportar(object? s, EventArgs e)
    {
        try { Taller.Presentacion.Impresion.ExportacionDocumento.GuardarHtml(this, Taller.Aplicacion.Reportes.DocumentoOperativoExportador.Presupuesto(_datos, _empresa), NombreArchivo); }
        catch (Exception ex) { Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Exportar presupuesto"); }
    }
    private void Imprimir(object? s, EventArgs e)
    {
        try { Taller.Presentacion.Impresion.ImpresionDocumento.Mostrar(this, Taller.Aplicacion.Reportes.DocumentoOperativoExportador.TextoPresupuesto(_datos), _empresa, NombreArchivo); }
        catch (Exception ex) { Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Imprimir presupuesto"); }
    }
    private void Cerrar(object? s, EventArgs e) => Close();
}

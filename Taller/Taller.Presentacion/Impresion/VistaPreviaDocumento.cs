using System.Drawing.Printing;
namespace Taller.Presentacion.Impresion;
/// <summary>Vista paginada con navegación explícita. Imprimir vuelve a generar el documento completo.</summary>
internal partial class VistaPreviaDocumento : Form
{
    private readonly PrintDocument _documento;
    private readonly Func<int> _contarPaginas;
    private bool _actualizando;
    public VistaPreviaDocumento(PrintDocument documento, Func<int> contarPaginas)
    {
        InitializeComponent();
        _documento = documento;
        _contarPaginas = contarPaginas;
        vista.Document = documento;
        cboZoom.SelectedIndex = 0;
        documento.EndPrint += Documento_EndPrint;
    }
    private void Preparar(object? sender, EventArgs e) => Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);
    private void Documento_EndPrint(object? sender, PrintEventArgs e)
    {
        if (IsHandleCreated && !IsDisposed && !Disposing)
            BeginInvoke((Action)ActualizarPaginas);
    }
    private void ActualizarPaginas()
    {
        if (IsDisposed || Disposing) return;
        _actualizando = true;
        int total = Math.Max(1, _contarPaginas());
        nudPagina.Maximum = total;
        nudPagina.Value = Math.Clamp(vista.StartPage + 1, 1, total);
        lblTotal.Text = "de " + total;
        btnAnterior.Enabled = nudPagina.Value > 1;
        btnSiguiente.Enabled = nudPagina.Value < total;
        _actualizando = false;
    }
    private void PaginaCambio(object? sender, EventArgs e)
    {
        if (_actualizando) return;
        vista.StartPage = (int)nudPagina.Value - 1;
        ActualizarPaginas();
    }
    private void InicioPaginaCambio(object? sender, EventArgs e) => ActualizarPaginas();
    private void Anterior(object? sender, EventArgs e) { if (nudPagina.Value > 1) nudPagina.Value--; }
    private void Siguiente(object? sender, EventArgs e) { if (nudPagina.Value < nudPagina.Maximum) nudPagina.Value++; }
    private void Rueda(object? sender, MouseEventArgs e)
    {
        if (e.Delta < 0) Siguiente(sender, e); else if (e.Delta > 0) Anterior(sender, e);
    }
    private void ZoomCambio(object? sender, EventArgs e)
    {
        vista.AutoZoom = cboZoom.SelectedIndex <= 0;
        if (!vista.AutoZoom) vista.Zoom = new[] { 1d, .5, .75, 1, 1.5 }[cboZoom.SelectedIndex];
    }
    private void Imprimir(object? sender, EventArgs e)
    {
        try
        {
            using var elegir = new PrintDialog { Document = _documento, UseEXDialog = true, AllowSomePages = false };
            if (elegir.ShowDialog(this) == DialogResult.OK) _documento.Print();
        }
        catch (Exception ex) { Errores.ErroresInterfaz.Mostrar(this, ex, "Imprimir documento"); }
    }
    private void Cerrar(object? sender, EventArgs e) => Close();
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _documento.EndPrint -= Documento_EndPrint;
        base.OnFormClosed(e);
    }
}

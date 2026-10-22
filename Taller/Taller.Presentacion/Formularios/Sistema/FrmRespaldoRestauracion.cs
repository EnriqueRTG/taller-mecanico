namespace Taller.Presentacion.Formularios.Sistema;
/// <summary>Informa que los respaldos deben realizarse mediante SQL Server hasta implementar su integración.</summary>
public partial class FrmRespaldoRestauracion : Form
{
    public FrmRespaldoRestauracion() { InitializeComponent(); }
    private void FrmRespaldoRestauracion_Load(object? sender, EventArgs e)
    {
        lblEstado.Text = "Respaldo y restauración no implementados. Realice y verifique la copia desde SQL Server Management Studio antes de migrar.";
        lblUltimoRespaldo.Text = "No existe un historial de respaldos registrado por esta aplicación.";
        dgvHistorial.Rows.Clear();
        btnCrearRespaldo.Enabled = btnRestaurar.Enabled = btnValidarArchivo.Enabled = btnExaminarCarpeta.Enabled = btnExaminarArchivo.Enabled = false;
    }
    private void BtnExaminarCarpeta_Click(object? sender, EventArgs e) { }
    private void BtnCrearRespaldo_Click(object? sender, EventArgs e) { }
    private void BtnExaminarArchivo_Click(object? sender, EventArgs e) { }
    private void BtnValidarArchivo_Click(object? sender, EventArgs e) { }
    private void BtnRestaurar_Click(object? sender, EventArgs e) { }
}

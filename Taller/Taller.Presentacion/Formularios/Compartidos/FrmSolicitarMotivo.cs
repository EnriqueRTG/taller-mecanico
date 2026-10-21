namespace Taller.Presentacion.Formularios.Compartidos;
/// <summary>Recoge una explicación operativa sin modificar datos por sí misma.</summary>
public partial class FrmSolicitarMotivo : Form
{
    public string Motivo => txtMotivo.Text.Trim();
    public FrmSolicitarMotivo(string titulo, string ayuda)
    { InitializeComponent(); Text = titulo; lblAyuda.Text = ayuda; }
    private void Confirmar(object? s, EventArgs e)
    {
        if (Motivo.Length == 0) { MessageBox.Show(this, "Ingrese el motivo o condición de la operación."); return; }
        DialogResult = DialogResult.OK; Close();
    }
    private void Cancelar(object? s, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
}

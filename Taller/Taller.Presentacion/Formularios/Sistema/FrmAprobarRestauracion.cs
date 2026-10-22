namespace Taller.Presentacion.Formularios.Sistema;
/// <summary>Recoge dos credenciales de una sola operación; no modifica la sesión ni conserva contraseñas en auditoría.</summary>
public partial class FrmAprobarRestauracion : Form
{
    public string UsuarioPrimero => txtPrimero.Text.Trim();
    public string ClavePrimero => txtClavePrimero.Text;
    public string UsuarioSegundo => txtSegundo.Text.Trim();
    public string ClaveSegundo => txtClaveSegundo.Text;
    public string Motivo => txtMotivo.Text.Trim();
    public bool ReconoceConsecuencias => chkConsecuencias.Checked && chkVentana.Checked;

    public FrmAprobarRestauracion(string primero, string resumen)
    {
        InitializeComponent();
        txtPrimero.Text = primero;
        txtResumen.Text = resumen;
    }

    public void LimpiarClaves()
    {
        txtClavePrimero.Clear();
        txtClaveSegundo.Clear();
    }

    private void BtnConfirmar_Click(object? sender, EventArgs e)
    {
        if (!ReconoceConsecuencias || string.IsNullOrWhiteSpace(ClavePrimero) || string.IsNullOrWhiteSpace(UsuarioSegundo) || string.IsNullOrWhiteSpace(ClaveSegundo) || Motivo.Length < 10)
        {
            MessageBox.Show(this, "Complete las dos credenciales, el motivo y las dos confirmaciones. El segundo usuario debe ser otra persona autorizada.", "Doble aprobación");
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        LimpiarClaves();
        DialogResult = DialogResult.Cancel;
        Close();
    }
}

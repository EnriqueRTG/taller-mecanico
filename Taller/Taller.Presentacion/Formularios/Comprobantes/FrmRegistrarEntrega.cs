using Taller.Aplicacion.DTOs.Cobros;
namespace Taller.Presentacion.Formularios.Comprobantes;
/// <summary>Confirma el retiro con los mínimos datos nuevos; el servicio valida nuevamente la cuenta.</summary>
public sealed partial class FrmRegistrarEntrega : Form
{
    private readonly TextBox persona = new() { Dock = DockStyle.Fill, MaxLength = 150 };
    private readonly TextBox notas = new() { Dock = DockStyle.Fill, Multiline = true, MaxLength = 500, ScrollBars = ScrollBars.Vertical };
    private readonly CheckBox conforme = new() { Text = "Se entrega el vehículo y se registra la conformidad de quien retira", Dock = DockStyle.Fill };
    public string PersonaRetira => persona.Text.Trim();
    public bool Conformidad => conforme.Checked;
    public string Observaciones => notas.Text.Trim();
    public FrmRegistrarEntrega(string cliente, string resultado)
    {
        InitializeComponent();
        persona.Text = cliente;
        Text = "Registrar entrega · " + resultado;
    }

    private void Confirmar(object? s, EventArgs e)
    {
        if (PersonaRetira.Length == 0) { MessageBox.Show(this, "Indique quién retira el vehículo."); persona.Focus(); return; }
        if (!conforme.Checked) { MessageBox.Show(this, "Registre la conformidad de recepción."); conforme.Focus(); return; }
        DialogResult = DialogResult.OK;
    }
}

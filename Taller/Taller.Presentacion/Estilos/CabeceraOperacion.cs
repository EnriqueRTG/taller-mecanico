namespace Taller.Presentacion.Estilos;
/// <summary>Identidad visual de la operación, sin consultas ni reglas comerciales.</summary>
internal static class CabeceraOperacion
{
    public static void Aplicar(Control control)
    {
        control.BackColor = Color.FromArgb(239, 246, 255);
        control.ForeColor = Color.FromArgb(30, 58, 138);
        control.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        control.Padding = new Padding(8);
        if (control is TextBox texto) { texto.BorderStyle = BorderStyle.FixedSingle; texto.ReadOnly = true; }
    }
}

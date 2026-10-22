namespace Taller.Presentacion.Estilos;
/// <summary>Limita las ventanas al área útil después del escalado de Windows, conservando sus disposiciones adaptables.</summary>
internal static class DisposicionVentanas
{
    public static void AjustarAlAreaDisponible(Form formulario)
    {
        var area = Screen.FromControl(formulario).WorkingArea;
        var ancho = Math.Max(1, area.Width - 16);
        var alto = Math.Max(1, area.Height - 16);
        formulario.MinimumSize = new Size(Math.Min(formulario.MinimumSize.Width, ancho), Math.Min(formulario.MinimumSize.Height, alto));
        formulario.Size = new Size(Math.Min(formulario.Width, ancho), Math.Min(formulario.Height, alto));
        formulario.Location = new Point(area.Left + (area.Width - formulario.Width) / 2, area.Top + (area.Height - formulario.Height) / 2);
    }
}

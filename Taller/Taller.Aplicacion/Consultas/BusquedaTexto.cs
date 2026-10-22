using System.Globalization;

namespace Taller.Aplicacion.Consultas;
/// <summary>Comparación cultural de consulta; no cambia los datos guardados ni las claves únicas.</summary>
public static class BusquedaTexto
{
    public static bool Coincide(string? valor, string? busqueda) => CultureInfo.GetCultureInfo("es-AR").CompareInfo.IndexOf(valor ?? "", (busqueda ?? "").Trim(), CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
}

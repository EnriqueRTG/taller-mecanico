namespace Taller.Aplicacion.DTOs.Vehiculos;
/// <summary>Modelo seleccionable; se conserva el actual aunque haya sido deshabilitado para nuevas altas.</summary>
public sealed record ModeloOpcionDto(int Id, string Marca, string Modelo, bool Activo)
{
    public string Nombre => $"{Marca} · {Modelo}" + (Activo ? "" : " (actual, inactivo)");
}

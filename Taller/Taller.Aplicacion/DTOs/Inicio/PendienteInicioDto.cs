namespace Taller.Aplicacion.DTOs.Inicio;
/// <summary>Acción prioritaria real; la fecha corresponde al último evento de la atención.</summary>
public sealed record PendienteInicioDto(string Codigo, DateTime Fecha, string Descripcion, string Estado, int Prioridad);

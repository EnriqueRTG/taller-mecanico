namespace Taller.Aplicacion.DTOs.Respaldos;
/// <summary>Identidad validada para una sola operación. La huella permite detectar revocación antes del reemplazo; no se audita.</summary>
public sealed record AprobadorRestauracionDto(int Id, int RolId, string Nombre, string HuellaCredencial);

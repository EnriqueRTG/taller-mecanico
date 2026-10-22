namespace Taller.Aplicacion.DTOs.Respaldos;
/// <summary>Resultado que exige reiniciar las sesiones, también cuando se recuperó la copia protectora tras un fallo.</summary>
public sealed record ResultadoRestauracionDto(bool Restaurada, bool RecuperadaAnterior, string RutaProtectora, string Mensaje);

namespace Taller.Aplicacion.DTOs.Respaldos;
/// <summary>Auditoría de operación sin contraseñas; permanece fuera de la base restaurada.</summary>
public sealed record EventoRespaldoDto(DateTimeOffset FechaUtc, string Accion, string Resultado, string Autor, string SegundoAprobador, string Detalle);

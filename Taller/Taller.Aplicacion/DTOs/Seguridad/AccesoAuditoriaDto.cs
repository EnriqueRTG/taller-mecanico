namespace Taller.Aplicacion.DTOs.Seguridad;
public sealed record AccesoAuditoriaDto(long Id, DateTimeOffset FechaUtc, string Usuario, bool Correcto);

namespace Taller.Aplicacion.DTOs.Vehiculos;
/// <summary>Vehículo leído al abrir el editor, con su token de concurrencia y modelo original.</summary>
public sealed record VehiculoEdicionDto(int Id, string Dominio, int Anio, string Color, int IdModelo, string Marca, string Modelo, bool Activo, DateTime FechaAlta, byte[] Version);

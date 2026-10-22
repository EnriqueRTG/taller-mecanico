namespace Taller.Aplicacion.DTOs.Clientes;
/// <summary>Datos originales de una edición; Version pertenece a la lectura inicial y no se renueva al guardar.</summary>
public sealed record ClienteEdicionDto(int Id, string TipoDocumento, string Documento, string Nombre, string Apellido, string? Telefono, string? Email, string? Direccion, bool Activo, DateTime FechaAlta, byte[] Version);

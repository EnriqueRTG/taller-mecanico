namespace Taller.Aplicacion.Abstracciones.Seguridad;
/// <summary>Limita intentos por identidad normalizada y conserva evidencia sin contraseñas.</summary>
public interface IProteccionAcceso
{
    Task<bool> EstaBloqueadoAsync(string nombre);
    Task RegistrarAsync(string nombre, int? idUsuario, bool correcto);
}

namespace Taller.Aplicacion.Excepciones;

/// <summary>
/// Representa un incumplimiento de las reglas de validación
/// de una operación de la aplicación.
/// </summary>
public sealed class ValidacionException : Exception
{
    /// <summary>
    /// Nombre técnico del dato que produjo la validación,
    /// cuando resulte posible identificarlo.
    /// </summary>
    public string? Campo { get; }

    /// <summary>
    /// Inicializa una excepción de validación con un mensaje
    /// comprensible para el usuario.
    /// </summary>
    public ValidacionException(
        string mensaje,
        string? campo = null)
        : base(mensaje)
    {
        Campo = campo;
    }
}
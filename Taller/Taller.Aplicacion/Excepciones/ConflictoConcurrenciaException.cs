namespace Taller.Aplicacion.Excepciones;

/// <summary>
/// Indica que una operación no pudo completarse porque
/// el registro fue modificado o eliminado por otra operación.
/// </summary>
public sealed class ConflictoConcurrenciaException : Exception
{
    public ConflictoConcurrenciaException(
        string mensaje,
        Exception? excepcionInterna = null)
        : base(mensaje, excepcionInterna)
    {
    }
}

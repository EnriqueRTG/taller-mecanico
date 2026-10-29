namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Confirma los cambios preparados para una operación de aplicación.
/// </summary>
public interface IUnidadDeTrabajo
{
    /// <summary>
    /// Guarda conjuntamente los cambios pendientes.
    /// </summary>
    Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Descarta el seguimiento y los cambios pendientes
    /// que todavía no fueron guardados.
    /// </summary>
    void DescartarCambios();

    /// <summary>Ejecuta una consulta compuesta breve con aislamiento de lectura,
    /// sin mantener una transacción mientras el usuario opera el formulario.</summary>
    Task<T> LeerConsistenteAsync<T>(Func<Task<T>> consulta);
}
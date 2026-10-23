using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;
/// <summary>
/// Mantiene el estado de la sesión del usuario durante
/// la ejecución de la aplicación.
/// </summary>
/// <remarks>
/// Esta clase no autentica credenciales ni consulta la base de datos.
/// La autenticación corresponde a <see cref = "AutenticacionServicio"/>.
///
/// Para conservar la misma sesión entre los distintos formularios,
/// debe registrarse como una dependencia única mediante
/// <c>AddSingleton&lt;SesionUsuario&gt;()</c>.
/// </remarks>
public sealed class SesionUsuario
{
    private readonly TimeProvider _reloj;
    private Usuario? _usuario;
    private DateTimeOffset _inicio, _actividad;
    public SesionUsuario(TimeProvider? reloj = null) => _reloj = reloj ?? TimeProvider.System;
    /// <summary>La actividad no renueva sesiones vencidas. Límite: 30 minutos inactiva u ocho horas totales.</summary>
    public void RegistrarActividad()
    {
        if (UsuarioActual is not null)
            _actividad = _reloj.GetUtcNow();
    }

    /// <summary>
    /// Obtiene el usuario actualmente autenticado.
    /// </summary>
    /// <value>
    /// El usuario autenticado o <see langword="null"/> cuando
    /// no existe una sesión activa.
    /// </value>
    public Usuario? UsuarioActual
    {
        get
        {
            var ahora = _reloj.GetUtcNow();
            if (_usuario is not null && (ahora - _actividad >= TimeSpan.FromMinutes(30) || ahora - _inicio >= TimeSpan.FromHours(8)))
                _usuario = null;
            return _usuario;
        }
    }

    /// <summary>
    /// Indica si actualmente existe una sesión autenticada.
    /// </summary>
    public bool EstaAutenticado => UsuarioActual is not null;

    /// <summary>
    /// Inicia una sesión utilizando el usuario previamente
    /// validado por el servicio de autenticación.
    /// </summary>
    /// <param name = "usuario">
    /// Usuario autenticado que debe conservarse en la sesión.
    /// </param>
    /// <exception cref = "ArgumentNullException">
    /// Se produce cuando el usuario recibido es nulo.
    /// </exception>
    /// <remarks>
    /// Este método no vuelve a validar las credenciales. Debe
    /// invocarse solamente después de una autenticación exitosa.
    /// </remarks>
    public void Iniciar(Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        _inicio = _actividad = _reloj.GetUtcNow();
        _usuario = usuario;
    }

    /// <summary>
    /// Finaliza la sesión actual eliminando la referencia
    /// al usuario autenticado.
    /// </summary>
    /// <remarks>
    /// La operación es idempotente: puede invocarse aunque
    /// no exista una sesión activa.
    /// </remarks>
    public void Cerrar()
    {
        _usuario = null;
    }
}

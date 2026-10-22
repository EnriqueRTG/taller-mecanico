using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Seguridad;
/// <summary>Revalida contra persistencia el usuario y rol operativo de la sesión.</summary>
public sealed class AutorizacionOperativa(IUsuarioRepositorio usuarios, SesionUsuario sesion)
{
    public async Task<Usuario> ExigirAsync(int? rol = null)
    {
        var u = sesion.UsuarioActual is { } s ? await usuarios.ObtenerPorIdAsync(s.Id) : null;
        if (!UsuarioAutorizado.EstaHabilitado(u) || (rol.HasValue ? u!.RolId != rol.Value : u!.RolId != RolesSistema.AdministrativoId && u.RolId != RolesSistema.TecnicoId))
            throw new ValidacionException("La operación requiere una sesión activa con el rol correspondiente.");
        return u!;
    }

    /// <summary>Revalida una sesión para una operación concreta. No acepta identidades elegidas por el formulario.</summary>
    public async Task<Usuario> ExigirRolesAsync(params int[] roles)
    {
        var u = sesion.UsuarioActual is { } actual ? await usuarios.ObtenerPorIdAsync(actual.Id) : null;
        if (!UsuarioAutorizado.EstaHabilitado(u) || !roles.Contains(u!.RolId))
            throw new ValidacionException("La operación requiere una sesión y un rol activos autorizados.");
        return u!;
    }

    public static void Version(byte[]? recibida, byte[] actual)
    {
        if (recibida is null || recibida.Length != 8 || !recibida.SequenceEqual(actual))
            throw new ConflictoConcurrenciaException("La información cambió. Actualice y vuelva a abrir la operación.");
    }

    public static string Texto(string? texto, bool requerido = true)
    {
        var valor = (texto ?? "").Trim();
        if (valor.Length > 1000 || requerido && valor.Length == 0)
            throw new ValidacionException("Ingrese un motivo/observación de hasta 1000 caracteres.");
        return valor;
    }
}

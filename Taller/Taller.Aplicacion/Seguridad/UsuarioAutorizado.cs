using Taller.Dominio.Entidades;
using Taller.Dominio.Constantes;

namespace Taller.Aplicacion.Seguridad;
/// <summary>Valida la cuenta persistida y la identidad del rol, no solo el identificador recibido de la pantalla.</summary>
public static class UsuarioAutorizado
{
    public static bool EstaHabilitado([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] Usuario? u) => u is not null && u.Activo && u.Rol is not null && u.Rol.Activo && RolesSistema.EsRolValido(u.RolId) && u.Rol.Id == u.RolId && u.Rol.Nombre == RolesSistema.ObtenerNombreEsperado(u.RolId);
}

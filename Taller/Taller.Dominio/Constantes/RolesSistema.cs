namespace Taller.Dominio.Constantes;

/// <summary>
/// Define los roles fijos reconocidos por el sistema.
/// </summary>
public static class RolesSistema
{
    public const int AdministradorId = 1;
    public const int AdministrativoId = 2;
    public const int TecnicoId = 3;

    public const string Administrador = "Administrador";
    public const string Administrativo = "Administrativo";
    public const string Tecnico = "Técnico";

    /// <summary>
    /// Indica si el identificador corresponde a uno
    /// de los roles definidos por el sistema.
    /// </summary>
    public static bool EsRolValido(int rolId)
    {
        return rolId == AdministradorId
            || rolId == AdministrativoId
            || rolId == TecnicoId;
    }

    /// <summary>
    /// Obtiene el nombre esperado para un rol del sistema.
    /// </summary>
    public static string? ObtenerNombreEsperado(int rolId)
    {
        return rolId switch
        {
            AdministradorId => Administrador,
            AdministrativoId => Administrativo,
            TecnicoId => Tecnico,
            _ => null
        };
    }
}
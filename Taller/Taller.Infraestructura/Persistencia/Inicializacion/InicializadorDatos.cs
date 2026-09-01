using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Inicializacion;

/// <summary>
/// Inicializa los datos mínimos requeridos por el sistema.
/// </summary>
public sealed class InicializadorDatos
{
    private readonly TallerDbContext _contexto;
    private readonly IPasswordHasher _passwordHasher;

    public InicializadorDatos(TallerDbContext contexto, IPasswordHasher passwordHasher)
    {
        _contexto = contexto;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Crea el usuario administrador inicial si todavía
    /// no existe ningún usuario con ese nombre.
    /// </summary>
    public async Task InicializarAsync ()
    {
        var existeAdministrador = await _contexto.Usuarios.AnyAsync(u => u.NombreUsuario == "admin");

        if (existeAdministrador)
            return;

        var administrador = new Usuario
        {
            NombreUsuario = "admin",
            PasswordHash = _passwordHasher.Hash("Admin123!"),
            Nombre = "Administrador",
            Apellido = "Sistema",
            RolId = 1,
            Activo = true
        };

        await _contexto.Usuarios.AddAsync(administrador);
        await _contexto.SaveChangesAsync();
    }

}
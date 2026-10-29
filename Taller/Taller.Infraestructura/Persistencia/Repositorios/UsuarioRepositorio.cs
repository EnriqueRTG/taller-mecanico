using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Entidades;
using Taller.Dominio.Constantes;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>Consulta usuarios con EF y confirma altas/ediciones mediante SP parametrizados.</summary>
/// <remarks>Las actualizaciones exigen la versión leída; SQL protege también al último administrador activo.</remarks>
public sealed class UsuarioRepositorio(TallerDbContext contexto) : IUsuarioRepositorio
{
    public Task<Usuario?> ObtenerPorIdAsync(int id) => contexto.Usuarios.AsNoTracking().Include(u => u.Rol).SingleOrDefaultAsync(u => u.Id == id);
    public Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario) => contexto.Usuarios.AsNoTracking().Include(u => u.Rol).SingleOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
    public Task<List<Usuario>> ListarAsync() => contexto.Usuarios.AsNoTracking().Include(u => u.Rol).OrderBy(u => u.Apellido).ThenBy(u => u.Nombre).ToListAsync();
    public Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario) => contexto.Usuarios.AsNoTracking().AnyAsync(u => u.NombreUsuario == nombreUsuario);

    private static SqlParameter Texto(string nombre, int longitud, string? valor) => new(nombre, SqlDbType.NVarChar, longitud) { Value = (object?)valor ?? DBNull.Value };
    private static SqlParameter Numero(string nombre, int valor) => new(nombre, SqlDbType.Int) { Value = valor };
    private static SqlParameter Version(byte[] version)
    {
        if (version is null || version.Length != 8) throw new ConflictoConcurrenciaException("La versión del usuario no es válida. Actualice los datos.");
        return new SqlParameter("@Version", SqlDbType.Binary, 8) { Value = version };
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        var id = new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output };
        await EjecutarAsync("EXEC dbo.Usuario_Crear @NombreUsuario, @PasswordHash, @Nombre, @Apellido, @RolId, @FechaAlta, @Id OUTPUT",
            Texto("@NombreUsuario", UsuarioRestricciones.NombreUsuarioMaximo, usuario.NombreUsuario), Texto("@PasswordHash", UsuarioRestricciones.PasswordHashMaximo, usuario.PasswordHash),
            Texto("@Nombre", UsuarioRestricciones.NombreMaximo, usuario.Nombre), Texto("@Apellido", UsuarioRestricciones.ApellidoMaximo, usuario.Apellido), Numero("@RolId", usuario.RolId),
            new SqlParameter("@FechaAlta", SqlDbType.DateTime2) { Value = usuario.FechaAlta }, id);
        usuario.Id = (int)id.Value;
        usuario.Version = (await ObtenerPorIdAsync(usuario.Id))!.Version.ToArray();
    }

    public Task ActualizarDatosAsync(int id, string nombre, string apellido, int rolId, byte[] version) =>
        EjecutarAsync("EXEC dbo.Usuario_ActualizarDatos @Id, @Nombre, @Apellido, @RolId, @Version",
            Numero("@Id", id), Texto("@Nombre", UsuarioRestricciones.NombreMaximo, nombre), Texto("@Apellido", UsuarioRestricciones.ApellidoMaximo, apellido), Numero("@RolId", rolId), Version(version));

    public Task ActualizarCredencialesAsync(int id, string nombreUsuario, string? passwordHash, byte[] version) =>
        EjecutarAsync("EXEC dbo.Usuario_ActualizarCredenciales @Id, @NombreUsuario, @PasswordHash, @Version",
            Numero("@Id", id), Texto("@NombreUsuario", UsuarioRestricciones.NombreUsuarioMaximo, nombreUsuario), Texto("@PasswordHash", UsuarioRestricciones.PasswordHashMaximo, passwordHash), Version(version));

    public Task CambiarEstadoAsync(int id, bool activo, byte[] version) =>
        EjecutarAsync("EXEC dbo.Usuario_CambiarEstado @Id, @Activo, @Version",
            Numero("@Id", id), new SqlParameter("@Activo", SqlDbType.Bit) { Value = activo }, Version(version));

    private async Task EjecutarAsync(string sql, params object[] parametros)
    {
        try { await contexto.Database.ExecuteSqlRawAsync(sql, parametros); }
        catch (SqlException ex) when (ex.Number == 51001)
        { throw new ValidacionException("Ese nombre de usuario ya está registrado. Ingrese otro nombre de usuario.", "nombreUsuario"); }
        catch (SqlException ex) when (ex.Number == 51002)
        { throw new ValidacionException("El usuario seleccionado ya no existe. Actualice el listado."); }
        catch (SqlException ex) when (ex.Number is 51003 or 51004)
        { throw new ValidacionException("La operación dejaría al sistema sin un administrador activo."); }
        catch (SqlException ex) when (ex.Number == 51005)
        { throw new ConflictoConcurrenciaException("El usuario cambió desde que se consultó. Actualice antes de continuar.", ex); }
    }
}

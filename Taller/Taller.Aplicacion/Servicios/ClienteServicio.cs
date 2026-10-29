using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Validaciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;
/// <summary>Administra clientes persistidos. Revalida recepción, datos y versión antes de cada escritura.</summary>
public sealed class ClienteServicio(IClienteRepositorio clientes, AutorizacionOperativa acceso)
{
    private Task<Usuario> AutorizarAsync() => acceso.ExigirRolesAsync(RolesSistema.AdministrativoId);
    /// <summary>Consulta un conjunto coherente de registros activos e inactivos para la bandeja.</summary>
    public async Task<List<Cliente>> ListarAsync()
    {
        await AutorizarAsync();
        return await clientes.ListarTodosAsync();
    }

    public async Task<List<Cliente>> ListarActivosAsync()
    {
        await AutorizarAsync();
        return await clientes.ListarActivosAsync();
    }


    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        await AutorizarAsync();
        DatosMaestros.Id(id);
        return await clientes.ObtenerPorIdAsync(id);
    }

    public async Task<Cliente?> ObtenerPorDocumentoAsync(string tipoDocumento, string documento)
    {
        await AutorizarAsync();
        var t = DatosMaestros.TipoDocumento(tipoDocumento);
        return await clientes.ObtenerPorDocumentoAsync(t, DatosMaestros.Documento(t, documento));
    }

    public async Task<Taller.Aplicacion.DTOs.Clientes.ClienteEdicionDto> ObtenerEdicionAsync(int id)
    {
        var c = await ObtenerPorIdAsync(id) ?? throw new ValidacionException("El cliente ya no existe.");
        return new(c.IdCliente, c.TipoDocumento, c.Documento, c.Nombre, c.Apellido, c.Telefono, c.Email, c.Direccion, c.Activo, c.FechaAlta, c.Version.ToArray());
    }

    private static void Asignar(Cliente c, string tipoDocumento, string documento, string nombre, string apellido, string? telefono, string? email, string? direccion)
    {
        c.TipoDocumento = DatosMaestros.TipoDocumento(tipoDocumento);
        c.Documento = DatosMaestros.Documento(c.TipoDocumento, documento);
        c.Nombre = DatosMaestros.Texto(nombre, 80, "Nombre");
        c.Apellido = DatosMaestros.Texto(apellido, 80, "Apellido");
        c.Telefono = DatosMaestros.Telefono(telefono);
        c.Email = DatosMaestros.Email(email);
        c.Direccion = DatosMaestros.Opcional(direccion, 200, "Dirección");
    }

    public async Task<Cliente> RegistrarAsync(string tipoDocumento, string documento, string nombre, string apellido, string? telefono, string? email, string? direccion)
    {
        await AutorizarAsync();
        var c = new Cliente();
        Asignar(c, tipoDocumento, documento, nombre, apellido, telefono, email, direccion);
        if (await clientes.ExisteDocumentoAsync(c.TipoDocumento, c.Documento))
            throw new ValidacionException("Ya existe un cliente con ese documento, incluso si está deshabilitado.");
        await clientes.AgregarAsync(c);
        return c;
    }

    public async Task ModificarAsync(int idCliente, string tipoDocumento, string documento, string nombre, string apellido, string? telefono, string? email, string? direccion, byte[] version)
    {
        await AutorizarAsync();
        DatosMaestros.Id(idCliente);
        var c = await clientes.ObtenerPorIdAsync(idCliente) ?? throw new ValidacionException("El cliente ya no existe.");
        AutorizacionOperativa.Version(version, c.Version);
        Asignar(c, tipoDocumento, documento, nombre, apellido, telefono, email, direccion);
        if (await clientes.ExisteDocumentoAsync(c.TipoDocumento, c.Documento, idCliente))
            throw new ValidacionException("Otro cliente tiene ese documento, incluso si está deshabilitado.");
        await clientes.ActualizarAsync(c);
    }

    public Task DarDeBajaAsync(int id, byte[] version) => CambiarEstadoAsync(id, version, false);
    public Task ReactivarAsync(int id, byte[] version) => CambiarEstadoAsync(id, version, true);
    private async Task CambiarEstadoAsync(int id, byte[] version, bool activo)
    {
        await AutorizarAsync();
        DatosMaestros.Id(id);
        var c = await clientes.ObtenerPorIdAsync(id) ?? throw new ValidacionException("El cliente ya no existe.");
        AutorizacionOperativa.Version(version, c.Version);
        if (c.Activo == activo)
            throw new ValidacionException("El estado ya fue modificado. Actualice el listado.");
        c.Activo = activo;
        await clientes.ActualizarAsync(c);
    }
}

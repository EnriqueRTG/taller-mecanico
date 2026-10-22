using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Validaciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;
/// <summary>Gestiona vehículos reales; conserva fecha de alta e historial y exige versión para editar o cambiar disponibilidad.</summary>
public sealed class VehiculoServicio(IVehiculoRepositorio vehiculos, IModeloRepositorio modelos, AutorizacionOperativa acceso)
{
    private Task<Usuario> RecepcionAsync() => acceso.ExigirRolesAsync(RolesSistema.AdministrativoId);
    /// <summary>Consulta un conjunto coherente de registros activos e inactivos para la bandeja.</summary>
    public async Task<List<Vehiculo>> ListarAsync()
    {
        await RecepcionAsync();
        return await vehiculos.ListarTodosAsync();
    }

    public async Task<List<Vehiculo>> ListarActivosAsync()
    {
        await RecepcionAsync();
        return await vehiculos.ListarActivosAsync();
    }

    public async Task<List<Vehiculo>> ListarInactivosAsync()
    {
        await RecepcionAsync();
        return await vehiculos.ListarInactivosAsync();
    }

    public async Task<Vehiculo?> ObtenerPorIdAsync(int id)
    {
        await acceso.ExigirAsync();
        DatosMaestros.Id(id);
        return await vehiculos.ObtenerPorIdAsync(id);
    }

    public async Task<Vehiculo?> ObtenerPorDominioAsync(string dominio)
    {
        await acceso.ExigirAsync();
        return await vehiculos.ObtenerPorDominioAsync(DatosMaestros.Dominio(dominio));
    }

    public async Task<Taller.Aplicacion.DTOs.Vehiculos.VehiculoEdicionDto> ObtenerEdicionAsync(int id)
    {
        await RecepcionAsync();
        var v = await ObtenerPorIdAsync(id) ?? throw new ValidacionException("El vehículo ya no existe.");
        return new(v.IdVehiculo, v.Dominio, v.Anio, v.Color, v.IdModelo, v.Modelo.Marca.Nombre, v.Modelo.Nombre, v.Activo, v.FechaAlta, v.Version.ToArray());
    }

    public async Task<List<Taller.Aplicacion.DTOs.Vehiculos.ModeloOpcionDto>> ListarModelosAsync(int actual)
    {
        await RecepcionAsync();
        return (await modelos.ListarParaEdicionAsync(actual)).Select(m => new Taller.Aplicacion.DTOs.Vehiculos.ModeloOpcionDto(m.IdModelo, m.Marca.Nombre, m.Nombre, m.Activo && m.Marca.Activo)).ToList();
    }

    public async Task<List<Marca>> ListarMarcasAsync()
    {
        await RecepcionAsync();
        return (await modelos.ListarParaEdicionAsync(0)).Select(m => m.Marca).DistinctBy(m => m.IdMarca).OrderBy(m => m.Nombre).ToList();
    }

    public async Task<List<Modelo>> ListarModelosPorMarcaAsync(int idMarca)
    {
        await RecepcionAsync();
        DatosMaestros.Id(idMarca);
        return (await modelos.ListarParaEdicionAsync(0)).Where(m => m.IdMarca == idMarca).ToList();
    }

    private async Task AsignarAsync(Vehiculo v, string dominio, int anio, string color, int idModelo)
    {
        v.Dominio = DatosMaestros.Dominio(dominio);
        v.Color = DatosMaestros.Texto(color, 50, "Color");
        if (anio < 1900 || anio > DateTime.Today.Year + 1)
            throw new ValidacionException($"El año debe estar entre 1900 y {DateTime.Today.Year + 1}.");
        v.Anio = anio;
        DatosMaestros.Id(idModelo);
        var m = await modelos.ObtenerPorIdAsync(idModelo) ?? throw new ValidacionException("El modelo no existe.");
        if ((v.IdVehiculo == 0 || v.IdModelo != idModelo) && (!m.Activo || !m.Marca.Activo))
            throw new ValidacionException("Seleccione un modelo y marca activos.");
        v.IdModelo = idModelo;
    }

    public async Task<Vehiculo> RegistrarAsync(string dominio, int anio, string color, int idModelo)
    {
        await RecepcionAsync();
        var v = new Vehiculo();
        await AsignarAsync(v, dominio, anio, color, idModelo);
        if (await vehiculos.ExisteDominioAsync(v.Dominio))
            throw new ValidacionException("Ya existe un vehículo con ese dominio, incluso si está deshabilitado.");
        await vehiculos.AgregarAsync(v);
        return v;
    }

    public async Task<Vehiculo> ModificarAsync(int idVehiculo, string dominio, int anio, string color, int idModelo, byte[] version)
    {
        await RecepcionAsync();
        DatosMaestros.Id(idVehiculo);
        var v = await vehiculos.ObtenerPorIdAsync(idVehiculo) ?? throw new ValidacionException("El vehículo ya no existe.");
        AutorizacionOperativa.Version(version, v.Version);
        await AsignarAsync(v, dominio, anio, color, idModelo);
        if (await vehiculos.ExisteDominioAsync(v.Dominio, idVehiculo))
            throw new ValidacionException("Otro vehículo tiene ese dominio, incluso si está deshabilitado.");
        await vehiculos.ActualizarAsync(v);
        return v;
    }

    public Task DarDeBajaAsync(int id, byte[] version) => CambiarEstadoAsync(id, version, false);
    public Task ReactivarAsync(int id, byte[] version) => CambiarEstadoAsync(id, version, true);
    private async Task CambiarEstadoAsync(int id, byte[] version, bool activo)
    {
        await RecepcionAsync();
        DatosMaestros.Id(id);
        var v = await vehiculos.ObtenerPorIdAsync(id) ?? throw new ValidacionException("El vehículo ya no existe.");
        AutorizacionOperativa.Version(version, v.Version);
        if (v.Activo == activo)
            throw new ValidacionException("El estado ya fue modificado. Actualice el listado.");
        v.Activo = activo;
        await vehiculos.ActualizarAsync(v);
    }
}

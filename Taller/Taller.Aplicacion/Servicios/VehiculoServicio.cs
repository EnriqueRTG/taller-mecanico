using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Proporciona operaciones de negocio relacionadas con la gestión de vehículos.
/// </summary>
public sealed class VehiculoServicio
{
    // Repositorio de vehículos utilizado para acceder a la persistencia de datos.
    private readonly IVehiculoRepositorio _vehiculoRepositorio;
    // Repositorio de modelos utilizado para validar la existencia y estado de los modelos.
    private readonly IModeloRepositorio _modeloRepositorio;

    /// <summary>
    /// Inicializa el servicio de vehículos con los repositorios necesarios.
    /// </summary>
    /// <param name="vehiculoRepositorio"></param>
    /// <param name="modeloRepositorio"></param>
    public VehiculoServicio(IVehiculoRepositorio vehiculoRepositorio, IModeloRepositorio modeloRepositorio)
    {
        _vehiculoRepositorio = vehiculoRepositorio;
        _modeloRepositorio = modeloRepositorio;
    }

    /// <summary>
    /// Lista todos los vehículos activos.
    /// </summary>
    /// <returns>Una lista de vehículos activos.</returns>
    public async Task<List<Vehiculo>> ListarActivosAsync()
    {
        return await _vehiculoRepositorio.ListarActivosAsync();
    }

    /// <summary>
    /// Lista todos los vehículos inactivos.
    /// </summary>
    /// <returns>Una lista de vehículos inactivos.</returns>
    public async Task<List<Vehiculo>> ListarInactivosAsync()
    {
        return await _vehiculoRepositorio.ListarInactivosAsync();
    }

    /// <summary>
    /// Obtiene un vehículo por su identificador.
    /// </summary>
    /// <param name="idVehiculo"></param>
    /// <returns>El vehículo encontrado o null si no se encuentra.</returns>
    public async Task<Vehiculo?> ObtenerPorIdAsync(int idVehiculo)
    {
        ValidarId(idVehiculo);

        return await _vehiculoRepositorio.ObtenerPorIdAsync(idVehiculo);
    }

    /// <summary>
    /// Obtiene un vehículo por su dominio.
    /// </summary>
    /// <param name="dominio"></param>
    /// <returns>El vehículo encontrado o null si no se encuentra.</returns>
    public async Task<Vehiculo?> ObtenerPorDominioAsync(string dominio)
    {
        dominio = NormalizarDominio(dominio);

        return await _vehiculoRepositorio.ObtenerPorDominioAsync(dominio);
    }

    /// <summary>
    /// Registra un nuevo vehículo en el sistema.
    /// </summary>
    /// <param name="dominio"></param>
    /// <param name="anio"></param>
    /// <param name="color"></param>
    /// <param name="idModelo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<Vehiculo> RegistrarAsync(string dominio, int anio, string color, int idModelo)
    {
        dominio = NormalizarDominio(dominio);

        ValidarAnio(anio);

        color = NormalizarColor(color);

        ValidarIdModelo(idModelo);

        await ValidarModeloAsync(idModelo);

        if (await _vehiculoRepositorio.ExisteDominioAsync(dominio))
        {
            throw new InvalidOperationException($"Ya existe un vehículo con el dominio '{dominio}'.");
        }

        var vehiculo = new Vehiculo
        {
            Dominio = dominio,
            Anio = anio,
            Color = color,
            IdModelo = idModelo,
            Activo = true,
            FechaAlta = DateTime.Now
        };

        await _vehiculoRepositorio.AgregarAsync(vehiculo);

        return vehiculo;
    }

    /// <summary>
    /// Modifica los datos de un vehículo existente.
    /// </summary>
    /// <param name="idVehiculo"></param>
    /// <param name="dominio"></param>
    /// <param name="anio"></param>
    /// <param name="color"></param>
    /// <param name="idModelo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<Vehiculo> ModificarAsync(int idVehiculo, string dominio, int anio, string color, int idModelo)
    {
        ValidarId(idVehiculo);

        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(idVehiculo);

        if (vehiculo is null)
        {
            throw new InvalidOperationException($"No se encontró un vehículo con ID '{idVehiculo}'.");
        }

        dominio = NormalizarDominio(dominio);

        ValidarAnio(anio);

        color = NormalizarColor(color);

        ValidarIdModelo(idModelo);

        await ValidarModeloAsync(idModelo);

        if (await _vehiculoRepositorio.ExisteDominioAsync(dominio, idVehiculo))
        {
            throw new InvalidOperationException($"Ya existe otro vehículo con el dominio '{dominio}'.");
        }

        vehiculo.Dominio = dominio;
        vehiculo.Anio = anio;
        vehiculo.Color = color;
        vehiculo.IdModelo = idModelo;

        await _vehiculoRepositorio.ActualizarAsync(vehiculo);

        return vehiculo;
    }

    /// <summary>
    /// Da de baja un vehículo, marcándolo como inactivo.
    /// </summary>
    /// <param name="idVehiculo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DarDeBajaAsync(int idVehiculo)
    {
        ValidarId(idVehiculo);

        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(idVehiculo);

        if (vehiculo is null)
        {
            throw new InvalidOperationException($"No se encontró un vehículo con ID '{idVehiculo}'.");
        }

        if (!vehiculo.Activo)
        {
            throw new InvalidOperationException($"El vehículo con ID '{idVehiculo}' ya se encuentra dado de baja.");
        }

        vehiculo.Activo = false;

        await _vehiculoRepositorio.ActualizarAsync(vehiculo);
    }

    /// <summary>
    /// Reactiva un vehículo, marcándolo como activo.
    /// </summary>
    /// <param name="idVehiculo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ReactivarAsync(int idVehiculo)
    {
        ValidarId(idVehiculo);

        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(idVehiculo);

        if (vehiculo is null)
        {
            throw new InvalidOperationException($"No se encontró un vehículo con ID '{idVehiculo}'.");
        }

        if (vehiculo.Activo)
        {
            throw new InvalidOperationException($"El vehículo con ID '{idVehiculo}' ya se encuentra activo.");
        }

        vehiculo.Activo = true;

        await _vehiculoRepositorio.ActualizarAsync(vehiculo);
    }

    /// <summary>
    /// Valida que el identificador del vehículo sea válido.
    /// </summary>
    /// <param name="idVehiculo"></param>
    /// <exception cref="ArgumentException"></exception>
    private static void ValidarId(int idVehiculo)
    {
        if (idVehiculo <= 0)
        {
            throw new ArgumentException(
                "El identificador del vehículo no es válido.",
                nameof(idVehiculo));
        }
    }

    /// <summary>
    /// Valida que el identificador del modelo sea válido.
    /// </summary>
    /// <param name="idModelo"></param>
    /// <exception cref="ArgumentException"></exception>
    private static void ValidarIdModelo(int idModelo)
    {
        if (idModelo <= 0)
        {
            throw new ArgumentException(
                "El modelo seleccionado no es válido.",
                nameof(idModelo));
        }
    }

    /// <summary>
    /// Valida que el modelo exista y esté activo, así como su marca asociada.
    /// </summary>
    /// <param name="idModelo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task ValidarModeloAsync(int idModelo)
    {
        var modelo = await _modeloRepositorio.ObtenerPorIdAsync(idModelo);

        if (modelo is null)
        {
            throw new InvalidOperationException("El modelo seleccionado no existe.");
        }

        if (!modelo.Activo)
        {
            throw new InvalidOperationException("El modelo seleccionado se encuentra inactivo.");
        }

        if (!modelo.Marca.Activo)
        {
            throw new InvalidOperationException("La marca asociada al modelo se encuentra inactiva.");
        }
    }

    /// <summary>
    /// Valida que el año del vehículo esté dentro de un rango válido (1900 hasta el año actual + 1).
    /// </summary>
    /// <param name="anio"></param>
    /// <exception cref="ArgumentException"></exception>
    private static void ValidarAnio(int anio)
    {
        int anioMaximo = DateTime.Now.Year + 1;

        if (anio < 1900 || anio > anioMaximo)
        {
            throw new ArgumentException(
                $"El año del vehículo debe estar comprendido entre 1900 y {anioMaximo}.",
                nameof(anio));
        }
    }

    /// <summary>
    /// Normaliza el dominio del vehículo, asegurando que no esté vacío, que no supere los 10 caracteres y que esté en mayúsculas.
    /// </summary>
    /// <param name="dominio"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static string NormalizarDominio(string? dominio)
    {
        if (string.IsNullOrWhiteSpace(dominio))
        {
            throw new ArgumentException(
                "El dominio del vehículo es obligatorio.",
                nameof(dominio));
        }

        dominio = dominio
            .Trim()
            .ToUpperInvariant();

        if (dominio.Length > 10)
        {
            throw new ArgumentException(
                "El dominio no puede superar los 10 caracteres.",
                nameof(dominio));
        }

        return dominio;
    }

    /// <summary>
    /// Normaliza el color del vehículo, asegurando que no esté vacío y que no supere los 50 caracteres.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static string NormalizarColor(string? color)
    {
        color = NormalizarObligatorio(
            color, 
            nameof(color), 
            "El color es obligatorio.");

        if (color.Length > 50)
        {
            throw new ArgumentException(
                "El color no puede superar los 50 caracteres.", 
                nameof(color));
        }

        return color;
    }

    /// <summary>
    /// Normaliza un valor obligatorio, asegurando que no esté vacío ni contenga solo espacios en blanco.
    /// </summary>
    /// <param name="valor"></param>
    /// <param name="nombreParametro"></param>
    /// <param name="mensaje"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static string NormalizarObligatorio(string? valor, string nombreParametro, string mensaje)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException(
                mensaje, 
                nombreParametro);
        }

        return valor.Trim();
    }
}

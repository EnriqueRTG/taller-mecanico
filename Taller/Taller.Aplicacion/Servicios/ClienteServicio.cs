using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Servicio para gestionar operaciones relacionadas con clientes.
/// </summary>
public sealed class ClienteServicio
{
    /// <summary>
    /// Repositorio de clientes para acceder a los datos de los clientes.
    /// </summary>
    private readonly IClienteRepositorio _clienteRepositorio;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ClienteServicio"/> con el repositorio de clientes proporcionado.
    /// </summary>
    /// <param name="clienteRepositorio"></param>
    public ClienteServicio(IClienteRepositorio clienteRepositorio)
    {
        _clienteRepositorio = clienteRepositorio;
    }

    /// <summary>
    /// Lista todos los clientes activos.
    /// </summary>
    /// <returns>Una lista de clientes activos.</returns>
    public async Task<List<Cliente>> ListarActivosAsync()
    {
        return await _clienteRepositorio.ListarActivosAsync();
    }

    /// <summary>
    /// Lista todos los clientes inactivos.
    /// </summary>
    /// <returns>Una lista de clientes inactivos.</returns>
    public async Task<List<Cliente>> ListarInactivosAsync()
    {
        return await _clienteRepositorio.ListarInactivosAsync();
    }

    /// <summary>
    /// Obtiene un cliente por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>El cliente encontrado o null si no se encuentra.</returns>
    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        ValidarId(id);

        return await _clienteRepositorio.ObtenerPorIdAsync(id);
    }

    /// <summary>
    /// Registra un nuevo cliente con los datos proporcionados. Lanza una excepción si ya existe un cliente con el mismo tipo y número de documento.    
    /// </summary>
    /// <param name="tipoDocumento"></param>
    /// <param name="documento"></param>
    /// <param name="nombre"></param>
    /// <param name="apellido"></param>
    /// <param name="telefono"></param>
    /// <param name="email"></param>
    /// <param name="direccion"></param>
    /// <returns>El cliente registrado.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<Cliente> RegistrarAsync(string tipoDocumento, string documento, string nombre, string apellido, string? telefono, string? email, string? direccion)
    {
        tipoDocumento = NormalizarObligatorio(
            tipoDocumento,
            nameof(tipoDocumento),
            "El tipo de documento es obligatorio.");

        documento = NormalizarObligatorio(
            documento,
            nameof(documento),
            "El número de documento es obligatorio.");

        nombre = NormalizarObligatorio(
            nombre,
            nameof(nombre),
            "El nombre es obligatorio.");

        apellido = NormalizarObligatorio(
            apellido,
            nameof(apellido),
            "El apellido es obligatorio.");

        if (await _clienteRepositorio.ExisteDocumentoAsync(
            tipoDocumento,
            documento))
        {
            throw new InvalidOperationException(
                "Ya existe un cliente registrado con ese tipo y número de documento.");
        }

        var cliente = new Cliente()
        {
            TipoDocumento = tipoDocumento,
            Documento = documento,
            Nombre = nombre,
            Apellido = apellido,
            Telefono = NormalizarOpcional(telefono),
            Email = NormalizarOpcional(email),
            Direccion = NormalizarOpcional(direccion),
            Activo = true,
            FechaAlta = DateTime.Now
        };

        await _clienteRepositorio.AgregarAsync(cliente);

        return cliente;
    }

    /// <summary>
    /// Obtiene un cliente por su tipo y número de documento. Devuelve null si no se encuentra ningún cliente con esos datos.
    /// </summary>
    /// <param name="tipoDocumento"></param>
    /// <param name="documento"></param>
    /// <returns>El cliente encontrado o null si no se encuentra.</returns>
    public async Task<Cliente?> ObtenerPorDocumentoAsync(
    string tipoDocumento,
    string documento)
    {
        tipoDocumento = NormalizarObligatorio(
            tipoDocumento,
            nameof(tipoDocumento),
            "El tipo de documento es obligatorio.");

        documento = NormalizarObligatorio(
            documento,
            nameof(documento),
            "El número de documento es obligatorio.");

        return await _clienteRepositorio.ObtenerPorDocumentoAsync(
            tipoDocumento,
            documento);
    }

    /// <summary>
    /// Modifica los datos de un cliente existente. Lanza una excepción si el cliente no existe o si ya existe otro cliente con el mismo tipo y número de documento.
    /// </summary>
    /// <param name="idCliente"></param>
    /// <param name="tipoDocumento"></param>
    /// <param name="documento"></param>
    /// <param name="nombre"></param>
    /// <param name="apellido"></param>
    /// <param name="telefono"></param>
    /// <param name="email"></param>
    /// <param name="direccion"></param>
    /// <returns>El cliente modificado.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ModificarAsync(int idCliente, string tipoDocumento, string documento, string nombre, string apellido, string? telefono, string? email, string? direccion)
    {
        ValidarId(idCliente);

        var cliente = await _clienteRepositorio.ObtenerPorIdAsync(idCliente);

        if (cliente is null)
        {
            throw new InvalidOperationException("El cliente seleccionado no existe.");
        }

        tipoDocumento = NormalizarObligatorio(tipoDocumento, nameof(tipoDocumento), "El tipo de documento es obligatorio.");

        documento = NormalizarObligatorio(
            documento,
            nameof(documento),
            "El número de documento es obligatorio.");

        nombre = NormalizarObligatorio(
            nombre,
            nameof(nombre),
            "El nombre es obligatorio.");

        apellido = NormalizarObligatorio(
            apellido,
            nameof(apellido),
            "El apellido es obligatorio.");

        if (await _clienteRepositorio.ExisteDocumentoAsync(
            tipoDocumento,
            documento,
            idCliente))
        {
            throw new InvalidOperationException(
                "Ya existe otro cliente registrado con ese tipo y número de documento.");
        }

        cliente.TipoDocumento = tipoDocumento;
        cliente.Documento = documento;
        cliente.Nombre = nombre;
        cliente.Apellido = apellido;
        cliente.Telefono = NormalizarOpcional(telefono);
        cliente.Email = NormalizarOpcional(email);
        cliente.Direccion = NormalizarOpcional(direccion);

        await _clienteRepositorio.ActualizarAsync(cliente);
    }

    /// <summary>
    /// Da de baja a un cliente, marcándolo como inactivo. Lanza una excepción si el cliente no existe o si ya está inactivo.
    /// </summary>
    /// <param name="idCliente"></param>
    /// <returns>El cliente dado de baja.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DarDeBajaAsync(int idCliente)
    {
        ValidarId(idCliente);

        var cliente = await _clienteRepositorio.ObtenerPorIdAsync(idCliente);

        if (cliente is null)
        {
            throw new InvalidOperationException("El cliente seleccionado no existe.");
        }

        if (!cliente.Activo)
        {
            throw new InvalidOperationException("El cliente seleccionado ya se encuentra inactivo.");
        }

        cliente.Activo = false;

        await _clienteRepositorio.ActualizarAsync(cliente);
    }

    /// <summary>
    /// Reactiva a un cliente, marcándolo como activo. Lanza una excepción si el cliente no existe o si ya está activo.
    /// </summary>
    /// <param name="idCliente"></param>
    /// <returns>El cliente reactivado.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ReactivarAsync(int idCliente)
    {
        ValidarId(idCliente);

        var cliente = await _clienteRepositorio.ObtenerPorIdAsync(idCliente);

        if (cliente is null)
        {
            throw new InvalidOperationException("El cliente seleccionado no existe.");
        }

        if (cliente.Activo)
        {
            throw new InvalidOperationException("El cliente seleccionado ya se encuentra activo.");
        }

        cliente.Activo = true;

        await _clienteRepositorio.ActualizarAsync(cliente);
    }

    /// <summary>
    /// Valida que el identificador del cliente sea mayor a cero. Lanza una excepción si no lo es.
    /// </summary>
    /// <param name="idCliente"></param>
    /// <exception cref="ArgumentException"></exception>
    private static void ValidarId(int idCliente)
    {
        if (idCliente <= 0)
        {
            throw new ArgumentException(
                "El identificador del cliente no es válido.",
                nameof(idCliente));
        }
    }

    /// <summary>
    ///     Normaliza un valor opcional, devolviendo null si es nulo o vacío, o el valor recortado si no lo es. 
    /// </summary>
    /// <param name="valor"></param>
    /// <returns>El valor normalizado o null si es nulo o vacío.</returns>
    private static string? NormalizarOpcional(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor)
            ? null
            : valor.Trim();
    }

    /// <summary>
    /// Normaliza un valor obligatorio, lanzando una excepción si es nulo o vacío, o devolviendo el valor recortado si no lo es.
    /// </summary>
    /// <param name="valor"></param>
    /// <param name="nombreParametro"></param>
    /// <param name="mensaje"></param>
    /// <returns>El valor normalizado.</returns>
    /// <exception cref="ArgumentException"></exception>
    private static string NormalizarObligatorio(
    string? valor,
    string nombreParametro,
    string mensaje)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException(mensaje, nombreParametro);
        }

        return valor.Trim();
    }
}

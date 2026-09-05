using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Inicializacion;

/// <summary>
/// Representa un inicializador de datos que se encarga de crear los datos mínimos requeridos por el sistema, incluyendo la creación del usuario administrador y la inicialización del catálogo de vehículos.
/// </summary>
public sealed class InicializadorDatos
{
    // Variable para el contexto de la base de datos
    private readonly TallerDbContext _contexto;

    // Variable para el servicio de hash de contraseñas
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="InicializadorDatos"/> con el contexto de la base de datos y el servicio de hash de contraseñas proporcionados.
    /// </summary>
    /// <param name="contexto"></param>
    /// <param name="passwordHasher"></param>
    public InicializadorDatos(TallerDbContext contexto, IPasswordHasher passwordHasher)
    {
        _contexto = contexto;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Inicializa los datos mínimos requeridos por el sistema, incluyendo la creación del usuario administrador y la inicialización del catálogo de vehículos.
    /// </summary>
    /// <returns></returns>
    public async Task InicializarAsync()
    {
        await InicializarAdministradorAsync();
        await InicializarCatalogoVehiculosAsync();
    }

    /// <summary>
    /// Inicializa el usuario administrador si no existe en la base de datos. Crea un usuario con nombre de usuario "admin", contraseña "Admin123!", y asigna el rol de administrador.
    /// </summary>
    /// <returns></returns>
    private async Task InicializarAdministradorAsync()
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

    /// <summary>
    /// Inicializa las marcas y modelos básicos utilizados
    /// como catálogo para el registro de vehículos.
    /// </summary>
    /// <returns></returns>
    private async Task InicializarCatalogoVehiculosAsync()
    {
        var chevrolet = await ObtenerOCrearMarcaAsync("Chevrolet");
        var ford = await ObtenerOCrearMarcaAsync("Ford");
        var toyota = await ObtenerOCrearMarcaAsync("Toyota");
        var honda = await ObtenerOCrearMarcaAsync("Honda");
        var fiat = await ObtenerOCrearMarcaAsync("Fiat");
        var peugeot = await ObtenerOCrearMarcaAsync("Peugeot");
        var citroen = await ObtenerOCrearMarcaAsync("Citroën");
        var renault = await ObtenerOCrearMarcaAsync("Renault");
        var nissan = await ObtenerOCrearMarcaAsync("Nissan");
        var volkswagen = await ObtenerOCrearMarcaAsync("Volkswagen");

        await ObtenerOCrearModeloAsync(chevrolet.IdMarca, "Onix");
        await ObtenerOCrearModeloAsync(chevrolet.IdMarca, "S10");

        await ObtenerOCrearModeloAsync(ford.IdMarca, "Ranger");
        await ObtenerOCrearModeloAsync(ford.IdMarca, "Maverick");

        await ObtenerOCrearModeloAsync(toyota.IdMarca, "Corolla");
        await ObtenerOCrearModeloAsync(toyota.IdMarca, "Hilux");

        await ObtenerOCrearModeloAsync(honda.IdMarca, "Civic");
        await ObtenerOCrearModeloAsync(honda.IdMarca, "HR-V");

        await ObtenerOCrearModeloAsync(fiat.IdMarca, "Cronos");
        await ObtenerOCrearModeloAsync(fiat.IdMarca, "Pulse");

        await ObtenerOCrearModeloAsync(peugeot.IdMarca, "208");
        await ObtenerOCrearModeloAsync(peugeot.IdMarca, "Partner");

        await ObtenerOCrearModeloAsync(citroen.IdMarca, "C4");
        await ObtenerOCrearModeloAsync(citroen.IdMarca, "Jumper");

        await ObtenerOCrearModeloAsync(renault.IdMarca, "Duster");
        await ObtenerOCrearModeloAsync(renault.IdMarca, "Kwid");

        await ObtenerOCrearModeloAsync(nissan.IdMarca, "Kicks");
        await ObtenerOCrearModeloAsync(nissan.IdMarca, "Sentra");

        await ObtenerOCrearModeloAsync(volkswagen.IdMarca, "Polo");
        await ObtenerOCrearModeloAsync(volkswagen.IdMarca, "Amarok");
    }

    /// <summary>
    /// Obtiene una marca existente por nombre o la crea si no existe.
    /// </summary>
    /// <param name="nombre">Nombre de la marca.</param>
    /// <returns>La marca existente o recién creada.</returns>
    private async Task<Marca> ObtenerOCrearMarcaAsync(string nombre)
    {
        var marca = await _contexto.Marcas
            .FirstOrDefaultAsync(m =>
            m.Nombre == nombre);

        if (marca is not null)
        {
            return marca;
        }

        marca = new Marca
        {
            Nombre = nombre,
            Activo = true
        };

        await _contexto.Marcas.AddAsync(marca);
        await _contexto.SaveChangesAsync();

        return marca;
    }

    /// <summary>
    /// Obtiene un modelo existente dentro de una marca
    /// o lo crea si no existe.
    /// </summary>
    /// <param name="idMarca">Identificador de la marca asociada.</param>
    /// <param name="nombreModelo">Nombre del modelo.</param>
    /// <returns>El modelo existente o recién creado.</returns>
    private async Task<Modelo> ObtenerOCrearModeloAsync(int idMarca, string nombreModelo)
    {
        var modelo = await _contexto.Modelos
            .FirstOrDefaultAsync(m =>
            m.IdMarca == idMarca &&
            m.Nombre == nombreModelo);

        if (modelo is not null)
        {
            return modelo;
        }

        modelo = new Modelo
        {
            IdMarca = idMarca,
            Nombre = nombreModelo,
            Activo = true
        };

        await _contexto.Modelos.AddAsync(modelo);
        await _contexto.SaveChangesAsync();

        return modelo;
    }
}
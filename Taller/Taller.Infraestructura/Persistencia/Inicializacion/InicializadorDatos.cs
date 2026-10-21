using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

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
        await InicializarCatalogoAsync();
        
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

    /// <summary>
    /// Inicializa las 16 categorías y los 3 (Consumible, Repuestos y Servicio) conceptos predeterminados del taller mecánico clasificados    
    /// </summary>
    private async Task InicializarCatalogoAsync()
    {  
        // Registra los conceptos con sus respectivos tipos y categorías asociadas
       
        await ObtenerOCrearConceptoAsync(
            idCategoria: 1,
            tipo: TipoConcepto.Consumible,
            nombre: "Cambio de aceite y filtros",
            descripcion: "Insumos y materiales consumibles utilizados en el taller.",
            precioReferencia: 15000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 2,
            tipo: TipoConcepto.Servicio,
            nombre: "Alineación y balanceo",
            descripcion: "Servicio de alineación y balanceo.",
            precioReferencia: 12000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 3,
            tipo: TipoConcepto.Servicio,
            nombre: "Control y rotación de neumáticos",
            descripcion: "Servicio de control y rotación de neumáticos.",
            precioReferencia: 5000.00m 
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 4,
            tipo: TipoConcepto.Repuesto,
            nombre: "Pastillas de freno",
            descripcion: "Repuestos utilizados en el sistema de frenos.",
            precioReferencia: 18000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 5,
            tipo: TipoConcepto.Servicio,
            nombre: "Reparación y revisión de frenos",
            descripcion: "Servicio de reparación del sistema de dirección.",
            precioReferencia: 30000.00m
        );
        
        await ObtenerOCrearConceptoAsync(
            idCategoria: 6,
            tipo: TipoConcepto.Repuesto,
            nombre: "Amortiguadores",
            descripcion: "Repuestos utilizados en el sistema de suspensión.",
            precioReferencia: 22000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 7,
            tipo: TipoConcepto.Servicio,
            nombre: "Reparación de dirección",
            descripcion: "Servicio de reparación del sistema d3e dirección.",
            precioReferencia: 30000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 8,
            tipo: TipoConcepto.Repuesto,
            nombre: "Kit de embrague",
            descripcion: "Repuestos y componentes del sistema de embrague.",
            precioReferencia: 40000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 9,
            tipo: TipoConcepto.Servicio,
            nombre: "Limpieza del radiador",
            descripcion: "Servicio de limpieza y mantenimiento del sistema de refrigeración.",
            precioReferencia: 14000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 10,
            tipo: TipoConcepto.Servicio,
            nombre: "Diagnóstico computarizado",
            descripcion: "Servicio de diagnóstico electrónico mediante escáner.",
            precioReferencia: 10000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 11,
            tipo: TipoConcepto.Servicio,
            nombre: "Reparación del sistema de arranque",
            descripcion: "Servicio de reparación del alternador y motor de arranque.",
            precioReferencia: 28000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 12,
            tipo: TipoConcepto.Repuesto,
            nombre: "Batería automotriz",
            descripcion: "Baterías utilizadas en vehículos.",
            precioReferencia: 86000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 13,
            tipo: TipoConcepto.Repuesto,
            nombre: "Lámparas y fusibles",
            descripcion: "Componentes eléctricos utilizados en vehículos.",
            precioReferencia: 9000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 14,
            tipo: TipoConcepto.Servicio,
            nombre: "Carga de aire acondicionado",
            descripcion: "Servicio de revisión y reparación del aire acondicionado.",
            precioReferencia: 30000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 15,
            tipo: TipoConcepto.Servicio,
            nombre: "Revisión de calefacción",
            descripcion: "Servicio de revisión del sistema de calefacción.",
            precioReferencia: 16000.00m
        );

        await ObtenerOCrearConceptoAsync(
            idCategoria: 16,
            tipo: TipoConcepto.Servicio,
            nombre: "Diagnóstico técnico inicial",
            descripcion: "Evaluación general del estado del vehículo.",
            precioReferencia: 10000.00m
        );
    }    
    
    private async Task<Categoria> ObtenerOCrearCategoriaAsync(string nombre, string descripcion)
    {
        var categoria = await _contexto.Categorias.FirstOrDefaultAsync(c => c.Nombre == nombre);

        if (categoria is not null)
            return categoria;

        categoria = new Categoria
        {
            Nombre = nombre,
            Descripcion = descripcion,
            Activo = true
        };

        await _contexto.Categorias.AddAsync(categoria);
        await _contexto.SaveChangesAsync();

        return categoria;
    }

    private async Task ObtenerOCrearConceptoAsync(int idCategoria, TipoConcepto tipo, string nombre, string descripcion, decimal precioReferencia)
    {
        var existeConcepto = await _contexto.Conceptos.AnyAsync(c => c.IdCategoria == idCategoria && c.Nombre == nombre);

        if (existeConcepto)
            return;

        var concepto = new Concepto
        {
            IdCategoria = idCategoria,
            Tipo = tipo,
            Nombre = nombre,
            Descripcion = descripcion,
            PrecioReferencia = precioReferencia,
            Activo = true
        };

        await _contexto.Conceptos.AddAsync(concepto);
        //await _contexto.SaveChangesAsync();
    }
}
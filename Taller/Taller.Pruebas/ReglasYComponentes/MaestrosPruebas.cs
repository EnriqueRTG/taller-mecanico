using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Servicios;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Validaciones;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Entidades;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Xunit;

namespace Taller.Pruebas;
public sealed class MaestrosPruebas
{
    private sealed class Caso : IDisposable
    {
        public TallerDbContext Db = new(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        public SesionUsuario Sesion = new();
        public AutorizacionOperativa Acceso;
        public ClienteServicio Clientes;
        public VehiculoServicio Vehiculos;
        public ConceptoServicio Catalogo;
        public Caso()
        {
            Db.Database.EnsureCreated();
            Db.Usuarios.AddRange(new Usuario { Id = 91, NombreUsuario = "recepcion-prueba", Nombre = "Recepción", Apellido = "Prueba", PasswordHash = "hash", RolId = 2 }, new Usuario { Id = 92, NombreUsuario = "tecnico-prueba", Nombre = "Técnico", Apellido = "Prueba", PasswordHash = "hash", RolId = 3 });
            Db.Marcas.Add(new Marca { IdMarca = 91, Nombre = "Marca Prueba" });
            Db.Modelos.Add(new Modelo { IdModelo = 91, IdMarca = 91, Nombre = "Modelo Prueba" });
            Db.SaveChanges();
            Db.ChangeTracker.Clear();
            Acceso = new(new UsuarioRepositorio(Db), Sesion);
            Clientes = new(new ClienteRepositorio(Db), Acceso);
            Vehiculos = new(new VehiculoRepositorio(Db), new ModeloRepositorio(Db), Acceso);
            Catalogo = new(new ConceptoRepositorio(Db), Acceso);
            Entrar(91);
        }

        public void Entrar(int id)
        {
            Sesion.Iniciar(Db.Usuarios.AsNoTracking().Single(x => x.Id == id));
        }

        public void Dispose() => Db.Dispose();
    }

    [Theory]
    [InlineData("dni", "12345678", "DNI", "12345678")]
    [InlineData("CUIT", "20-12345678-6", "CUIT", "20123456786")]
    [InlineData("Pasaporte", "ab12345", "PASAPORTE", "AB12345")]
    public void DocumentoSeNormalizaSinInventarIdentidad(string tipo, string numero, string esperado, string valor)
    {
        var t = DatosMaestros.TipoDocumento(tipo);
        Assert.Equal(esperado, t);
        Assert.Equal(valor, DatosMaestros.Documento(t, numero));
    }

    [Theory]
    [InlineData("DNI", "12")]
    [InlineData("DNI", "ABCDEFGH")]
    [InlineData("CUIT", "20123456780")]
    [InlineData("PASAPORTE", "ab/123")]
    public void DocumentoInvalidoSeRechaza(string tipo, string numero) => Assert.Throws<ValidacionException>(() => DatosMaestros.Documento(tipo, numero));
    [Theory]
    [InlineData("")]
    [InlineData("a@")]
    [InlineData("a @b.com")]
    public void CorreoOpcionalNoAceptaFormatoInvalido(string correo)
    {
        if (correo == "")
            Assert.Null(DatosMaestros.Email(correo));
        else
            Assert.Throws<ValidacionException>(() => DatosMaestros.Email(correo));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("123")]
    [InlineData("1234567890123456")]
    [InlineData("379-ABC-1234")]
    [InlineData("379+4123456")]
    [InlineData("++543794123456")]
    public async Task TelefonoObligatorioSeValidaEnAltaYEdicion(string? telefono)
    {
        using var c = new Caso();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Clientes.RegistrarAsync("DNI", "12345678", "Ana", "Pérez", telefono, null, null));
        Assert.Empty(await c.Db.Clientes.ToListAsync());
        var cliente = await c.Clientes.RegistrarAsync("DNI", "12345678", "Ana", "Pérez", "3794123456", null, null);
        // InMemory no genera rowversion: se asigna una versión para ejercitar la validación de edición.
        (await c.Db.Clientes.SingleAsync()).Version = new byte[8];
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Clientes.ModificarAsync(cliente.IdCliente, "DNI", "12345678", "Ana", "Pérez", telefono, null, null, new byte[8]));
        c.Db.ChangeTracker.Clear();
        Assert.Equal("3794123456", (await c.Clientes.ObtenerPorIdAsync(cliente.IdCliente))!.Telefono);
    }

    [Theory]
    [InlineData("  +54 (379) 412-3456  ", "+54 (379) 412-3456")]
    [InlineData("3794123456", "3794123456")]
    [InlineData("1234567", "1234567")]
    public void TelefonoConservaFormatoLegibleSinAlterarDigitos(string entrada, string esperado)
        => Assert.Equal(esperado, DatosMaestros.Telefono(entrada));

    [Fact]
    public async Task AltaClienteConservaDatosRealesYRechazaDuplicadoInactivo()
    {
        using var c = new Caso();
        var a = await c.Clientes.RegistrarAsync("DNI", "12345678", "Ana María", "Pérez", "+54 379 1234567", "ana@example.com", "Calle 123");
        Assert.Equal("Ana María", (await c.Clientes.ObtenerPorIdAsync(a.IdCliente))!.Nombre);
        var d = await c.Db.Clientes.SingleAsync();
        d.Activo = false;
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Clientes.RegistrarAsync("DNI", "12345678", "Otra", "Persona", "3794123456", null, null));
    }

    [Fact]
    public async Task ServicioValidaLongitudesAunqueNoSeUseElFormulario()
    {
        using var c = new Caso();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Clientes.RegistrarAsync("DNI", "12345678", new string ('a', 81), "Apellido", "3794123456", null, null));
        await Assert.ThrowsAsync<ValidacionException>(() => c.Clientes.RegistrarAsync("DNI", "12345678", "Nombre", "Apellido", "abc", null, null));
        Assert.Empty(await c.Db.Clientes.ToListAsync());
    }

    [Fact]
    public async Task TecnicoNoPuedeCrearClientesNiVehiculosNiUsarIdentidadDeOtroRol()
    {
        using var c = new Caso();
        c.Entrar(92);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Clientes.RegistrarAsync("DNI", "12345678", "Nombre", "Apellido", "3794123456", null, null));
        await Assert.ThrowsAsync<ValidacionException>(() => c.Vehiculos.RegistrarAsync("AA123BB", 2020, "Azul", 91));
        c.Sesion.UsuarioActual!.RolId = 2;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Clientes.ListarActivosAsync());
    }

    [Fact]
    public async Task RevocarRolInvalidaUnaSesionExistente()
    {
        using var c = new Caso();
        var rol = await c.Db.Roles.SingleAsync(x => x.Id == 2);
        rol.Activo = false;
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Clientes.ListarActivosAsync());
    }

    [Fact]
    public async Task VehiculoNormalizaDominioYExigeMarcaYModeloActivos()
    {
        using var c = new Caso();
        var v = await c.Vehiculos.RegistrarAsync("aa123bb", 2020, " Azul ", 91);
        Assert.Equal("AA123BB", v.Dominio);
        var m = await c.Db.Modelos.SingleAsync(x => x.IdModelo == 91);
        m.Activo = false;
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Vehiculos.RegistrarAsync("AC123DD", 2020, "Azul", 91));
    }

    [Fact]
    public async Task VersionObsoletaNoModificaClienteNiVehiculo()
    {
        using var c = new Caso();
        var a = await c.Clientes.RegistrarAsync("DNI", "12345678", "Ana", "Pérez", "3794123456", null, null);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => c.Clientes.ModificarAsync(a.IdCliente, "DNI", "12345678", "Cambio", "Pérez", null, null, null, new byte[8]));
        Assert.Equal("Ana", (await c.Clientes.ObtenerPorIdAsync(a.IdCliente))!.Nombre);
        var v = await c.Vehiculos.RegistrarAsync("AA123BB", 2020, "Azul", 91);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => c.Vehiculos.DarDeBajaAsync(v.IdVehiculo, new byte[8]));
        Assert.True((await c.Vehiculos.ObtenerPorIdAsync(v.IdVehiculo))!.Activo);
    }

    [Fact]
    public async Task CatalogoProvieneDePersistenciaYNoConfundeCategoriaInactivaConOferta()
    {
        using var c = new Caso();
        c.Db.Categorias.Add(new Categoria { IdCategoria = 91, Nombre = "Categoría Prueba", Activo = false });
        c.Db.Conceptos.Add(new Concepto { IdConcepto = 91, IdCategoria = 91, Nombre = "Concepto Real", Tipo = Taller.Dominio.Enumeraciones.TipoConcepto.Servicio, PrecioReferencia = null });
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        c.Entrar(92);
        var d = Assert.Single(await c.Catalogo.ListarAsync(), x => x.IdConcepto == 91);
        Assert.False(d.Disponible);
        Assert.Equal("A cotizar", d.Precio);
        c.Sesion.Cerrar();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Catalogo.ListarAsync());
    }

    [Fact]
    public void FiltrosCombinanTextoCategoriaTipoYDisponibilidadSinCrearFilas()
    {
        Taller.Aplicacion.DTOs.Catalogo.ConceptoCatalogoDto[] datos = [new(1, 1, "Mecánica", Taller.Dominio.Enumeraciones.TipoConcepto.Servicio, "Inspección inicial", "Revisión", 100, true), new(2, 1, "Mecánica", Taller.Dominio.Enumeraciones.TipoConcepto.Repuesto, "Repuesto", "", null, true), new(3, 2, "Electricidad", Taller.Dominio.Enumeraciones.TipoConcepto.Servicio, "Inspección eléctrica", "", 200, false)];
        var f = new Taller.Aplicacion.DTOs.Catalogo.FiltroCatalogoDto("inspeccion", 1, Taller.Dominio.Enumeraciones.TipoConcepto.Servicio, 1);
        Assert.Equal(1, Assert.Single(Taller.Aplicacion.Consultas.ConsultaCatalogo.Aplicar(datos, f)).IdConcepto);
        Assert.Empty(Taller.Aplicacion.Consultas.ConsultaCatalogo.Aplicar(datos, f with { IdCategoria = 2 }));
        Assert.Equal(3, Assert.Single(Taller.Aplicacion.Consultas.ConsultaCatalogo.Aplicar(datos, new(Disponibilidad: 2))).IdConcepto);
        Assert.True(Taller.Aplicacion.Consultas.BusquedaTexto.Coincide("Gómez", "gomez"));
    }
}

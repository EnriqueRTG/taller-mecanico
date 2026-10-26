using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taller.Aplicacion;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Infraestructura;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Inicializacion;
using Taller.Demostracion;

// Herramienta explícita, nunca referenciada desde Presentación ni desde el agente de respaldos.
if (args.Length != 1 || args[0] != "--cargar-demo")
{
    Console.WriteLine("Uso: dotnet run --project Taller/Taller.Demostracion -- --cargar-demo. Requiere SGTM_CONN hacia una base terminada en _Demo y SGTM_DEMO_PASSWORD.");
    return 1;
}

var conexion = Environment.GetEnvironmentVariable("SGTM_CONN") ?? "";
var cadena = new SqlConnectionStringBuilder(conexion);
if (!cadena.InitialCatalog.EndsWith("_Demo", StringComparison.OrdinalIgnoreCase))
    throw new InvalidOperationException("La carga solo admite una base cuyo nombre termine en _Demo. No use SGTM_DB ni la copia de sus datos reales.");
var clave = Environment.GetEnvironmentVariable("SGTM_DEMO_PASSWORD") ?? "";
if (clave.Length < 12 || !clave.Any(char.IsUpper) || !clave.Any(char.IsLower) || !clave.Any(char.IsDigit) || !clave.Any(c => !char.IsLetterOrDigit(c)))
    throw new InvalidOperationException("Defina SGTM_DEMO_PASSWORD: mínimo 12 caracteres, mayúscula, minúscula, número y símbolo. No se imprimirá ni guardará en un archivo.");
var builder = Host.CreateApplicationBuilder();
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:Default"] = conexion, ["Respaldos:Habilitado"] = "false", ["Instalacion:ClaveAdministrador"] = clave });
builder.Services.AddAplicacion().AddInfraestructura(builder.Configuration);
using var host = builder.Build();
using var scope = host.Services.CreateScope();
var sp = scope.ServiceProvider;
var db = sp.GetRequiredService<TallerDbContext>();
await db.Database.MigrateAsync();
// Una única transacción revierte TODO el lote si se interrumpe. Segunda ejecución no agrega duplicados.
await using var transaccion = await db.Database.BeginTransactionAsync();
await db.Database.ExecuteSqlRawAsync("DECLARE @r int; EXEC @r=sys.sp_getapplock @Resource=N'SGTM:CargaDemostracion', @LockMode=N'Exclusive', @LockOwner=N'Transaction', @LockTimeout=0; IF @r<0 THROW 51130,'Existe otra carga de demostración en curso.',1;");
if (await db.Clientes.AnyAsync() || await db.Vehiculos.AnyAsync() || await db.Atenciones.AnyAsync() || await db.Usuarios.AnyAsync())
    throw new InvalidOperationException("La base ya contiene registros. La herramienta no mezcla lotes ni elimina datos; utilice una base _Demo nueva o restaure su punto de partida.");
await sp.GetRequiredService<InicializadorDatos>().InicializarAsync();
await CargadorDemostracion.CargarAsync(sp, clave);
await transaccion.CommitAsync();
Console.WriteLine("Lote DEMO v3 completo: 19 clientes, 48 vehículos y 48 atenciones, con catálogo ampliado y precios ARS orientativos. Consulte docs/coloquio-demostracion.md. Contraseña: la suministrada en SGTM_DEMO_PASSWORD. Retire esa variable al terminar.");
return 0;

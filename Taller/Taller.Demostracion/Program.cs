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
if (clave.Length < 12 || clave.Length > 100 || !clave.Any(char.IsUpper) || !clave.Any(char.IsLower) || !clave.Any(char.IsDigit) || !clave.Any(c => !char.IsLetterOrDigit(c)))
    throw new InvalidOperationException("Defina SGTM_DEMO_PASSWORD: mínimo 12 caracteres, mayúscula, minúscula, número y símbolo. No se imprimirá ni guardará en un archivo.");
var builder = Host.CreateApplicationBuilder();
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:Default"] = conexion, ["Respaldos:Habilitado"] = "false", ["Instalacion:ClaveAdministrador"] = clave });
builder.Services.AddAplicacion().AddInfraestructura(builder.Configuration);
using var host = builder.Build();
using var scope = host.Services.CreateScope();
var sp = scope.ServiceProvider;
var db = sp.GetRequiredService<TallerDbContext>();
var resumen = await PreparadorDemostracion.CargarAsync(sp, clave);
Console.WriteLine($"Lote DEMO {PreparadorDemostracion.Version} completo: {resumen.Clientes} clientes, {resumen.Vehiculos} vehículos, {resumen.Atenciones} atenciones, {resumen.Marcas} marcas, {resumen.Modelos} modelos, {resumen.Categorias} categorías y {resumen.Conceptos} conceptos.");
Console.WriteLine($"Operaciones: {resumen.Presupuestos} presupuestos, {resumen.Liquidaciones} liquidaciones y {resumen.Cobros} cobros. Consulte docs/pruebas/catalogo-demo.md. Retire SGTM_DEMO_PASSWORD al terminar.");
return 0;

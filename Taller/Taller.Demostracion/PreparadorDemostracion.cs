using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Taller.Dominio.Constantes;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Inicializacion;

namespace Taller.Demostracion;

/// <summary>Prepara únicamente una base Demo vacía; confirma el lote completo o revierte sus datos.</summary>
public static class PreparadorDemostracion
{
    public const int ClientesActivos = 80;
    public const int ClientesInactivos = 8;
    public const int Vehiculos = 256;
    public const int Atenciones = 240;
    public const string Version = "v5";

    public sealed record Resumen(int Clientes, int Vehiculos, int Atenciones, int Marcas, int Modelos,
        int Categorias, int Conceptos, int Presupuestos, int Liquidaciones, int Cobros);

    public static async Task<Resumen> CargarAsync(IServiceProvider servicios, string clave)
    {
        var db = servicios.GetRequiredService<TallerDbContext>();
        if (!db.Database.IsSqlServer() || !db.Database.GetDbConnection().Database.EndsWith("_Demo", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("La carga solo admite una base SQL terminada en _Demo.");
        if (clave.Length < 12 || clave.Length > UsuarioRestricciones.PasswordMaximo || !clave.Any(char.IsUpper)
            || !clave.Any(char.IsLower) || !clave.Any(char.IsDigit) || !clave.Any(c => !char.IsLetterOrDigit(c)))
            throw new InvalidOperationException("La clave Demo debe tener de 12 a 100 caracteres, mayúscula, minúscula, número y símbolo.");
        await db.Database.MigrateAsync();
        await using var transaccion = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        await db.Database.ExecuteSqlRawAsync("""
            DECLARE @r int;
            EXEC @r=sys.sp_getapplock @Resource=N'SGTM:CargaDemostracion',
                @LockMode=N'Exclusive', @LockOwner=N'Transaction', @LockTimeout=0;
            IF @r<0 THROW 51130,'Existe otra carga de demostración en curso.',1;
            """);
        try
        {
            if (await db.Clientes.AnyAsync() || await db.Vehiculos.AnyAsync() || await db.Atenciones.AnyAsync() || await db.Usuarios.AnyAsync())
                throw new InvalidOperationException("La base ya contiene registros. No se mezclan ni eliminan lotes: conserve su copia y prepare explícitamente la Demo antes de cargar v5.");
            await servicios.GetRequiredService<InicializadorDatos>().InicializarAsync();
            await CargadorDemostracion.CargarAsync(servicios, clave);
            var resumen = new Resumen(await db.Clientes.CountAsync(), await db.Vehiculos.CountAsync(), await db.Atenciones.CountAsync(),
                await db.Marcas.CountAsync(), await db.Modelos.CountAsync(), await db.Categorias.CountAsync(), await db.Conceptos.CountAsync(),
                await db.Presupuestos.CountAsync(), await db.Comprobantes.CountAsync(), await db.Pagos.CountAsync());
            await transaccion.CommitAsync();
            return resumen;
        }
        finally
        {
            db.ChangeTracker.Clear();
        }
    }
}

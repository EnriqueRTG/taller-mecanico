using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Infraestructura.Persistencia;

namespace Taller.Infraestructura.Seguridad;
/// <summary>SQL serializa el contador entre clientes. Cinco fallos en 15 minutos bloquean otros 15 minutos.</summary>
public sealed class ProteccionAccesoSql(TallerDbContext db) : IProteccionAcceso
{
    private static string Clave(string nombre) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(nombre.Trim().ToUpperInvariant())));
    public Task<bool> EstaBloqueadoAsync(string nombre)
    {
        var clave = Clave(nombre);
        var ahora = DateTimeOffset.UtcNow;
        return db.Set<Taller.Dominio.Entidades.ControlAcceso>().AsNoTracking().AnyAsync(x => x.Clave == clave && x.BloqueadoHastaUtc > ahora);
    }

    public async Task RegistrarAsync(string nombre, int? idUsuario, bool correcto)
    {
        var clave = Clave(nombre);
        // No se modifica el ChangeTracker del contexto que también utiliza el servicio de usuarios.
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            SET XACT_ABORT ON;
            BEGIN TRANSACTION;
            IF NOT EXISTS(SELECT 1 FROM dbo.ControlesAcceso WITH(UPDLOCK,HOLDLOCK) WHERE Clave={clave})
                INSERT dbo.ControlesAcceso(Clave,Intentos,VentanaUtc,BloqueadoHastaUtc) VALUES({clave},0,SYSUTCDATETIME(),NULL);
            IF {correcto}=1
                UPDATE dbo.ControlesAcceso SET Intentos=0,VentanaUtc=SYSUTCDATETIME(),BloqueadoHastaUtc=NULL WHERE Clave={clave};
            ELSE
            BEGIN
                UPDATE dbo.ControlesAcceso SET Intentos=0,VentanaUtc=SYSUTCDATETIME(),BloqueadoHastaUtc=NULL
                WHERE Clave={clave} AND VentanaUtc<=DATEADD(minute,-15,SYSUTCDATETIME()) AND (BloqueadoHastaUtc IS NULL OR BloqueadoHastaUtc<=SYSUTCDATETIME());
                UPDATE dbo.ControlesAcceso SET Intentos=Intentos+1,
                    BloqueadoHastaUtc=CASE WHEN Intentos+1>=5 THEN DATEADD(minute,15,SYSUTCDATETIME()) ELSE BloqueadoHastaUtc END
                WHERE Clave={clave} AND (BloqueadoHastaUtc IS NULL OR BloqueadoHastaUtc<=SYSUTCDATETIME());
            END;
            INSERT dbo.EventosAcceso(Clave,IdUsuario,Correcto,FechaUtc) VALUES({clave},{idUsuario},{correcto},SYSUTCDATETIME());
            COMMIT;
            """);
    }
}

using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Excepciones;

namespace Taller.Infraestructura.Persistencia.Inicializacion;

/// <summary>Comprueba la versión una vez al iniciar, antes de leer tablas de negocio. No migra bases compartidas.</summary>
public static class CompatibilidadEsquema
{
    public static async Task VerificarAsync(TallerDbContext contexto)
    {
        if (!contexto.Database.IsRelational()) return;
        var esperadas = contexto.Database.GetMigrations().ToArray();
        var aplicadas = (await contexto.Database.GetAppliedMigrationsAsync()).ToArray();
        if (!esperadas.SequenceEqual(aplicadas, StringComparer.Ordinal))
            throw new ValidacionException("La versión de la base de datos no coincide con esta aplicación. Cierre todos los puestos y solicite al responsable técnico verificar la base configurada, realizar un respaldo y aplicar las migraciones de esta versión. No se modificaron los datos.");
    }
}

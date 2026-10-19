using Microsoft.EntityFrameworkCore;

namespace Taller.Infraestructura.Persistencia.Inicializacion;

/// <summary>
/// Configura la carga inicial del catálogo durante
/// las operaciones de preparación de la base de datos.
/// </summary>
public static class ConfiguracionSemilla
{
    public static DbContextOptionsBuilder UsarSemillaCatalogo(
        this DbContextOptionsBuilder opciones)
    {
        opciones.UseSeeding((contexto, _) =>
        {
            // Las herramientas de EF utilizan esta variante síncrona.
            // Ejecutamos la rutina asincrónica sin depender
            // del contexto de sincronización del hilo llamador.
            Task.Run(() =>
                InicializadorCatalogoConceptos.InicializarAsync(
                    (TallerDbContext)contexto))
                .GetAwaiter()
                .GetResult();
        });

        opciones.UseAsyncSeeding((contexto, _, cancellationToken) =>
            InicializadorCatalogoConceptos.InicializarAsync(
                (TallerDbContext)contexto,
                cancellationToken));

        return opciones;
    }
}
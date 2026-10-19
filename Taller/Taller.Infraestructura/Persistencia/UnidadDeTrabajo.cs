using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;

namespace Taller.Infraestructura.Persistencia;

/// <summary>
/// Confirma los cambios preparados en el contexto compartido.
/// </summary>
public sealed class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly TallerDbContext _contexto;

    public UnidadDeTrabajo(
        TallerDbContext contexto)
    {
        _contexto = contexto
            ?? throw new ArgumentNullException(nameof(contexto));
    }

    public async Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _contexto.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConflictoConcurrenciaException(
                "La información fue modificada o eliminada por otra operación. " +
                "Actualice los datos antes de continuar.",
                ex);
        }
        catch (DbUpdateException ex)
            when (EsDiagnosticoInicialDuplicado(ex))
        {
            throw new ValidacionException(
                "La atención ya tiene registrado un diagnóstico inicial.");
        }
        finally
        {
            // Las operaciones siguientes deben consultar datos actuales.
            // Ningún cambio fallido debe quedar pendiente de otro guardado.
            _contexto.ChangeTracker.Clear();
        }
    }

    public void DescartarCambios()
    {
        _contexto.ChangeTracker.Clear();
    }

    private static bool EsDiagnosticoInicialDuplicado(
        DbUpdateException excepcion)
    {
        return excepcion.InnerException is SqlException sqlEx
            && sqlEx.Errors.Cast<SqlError>().Any(error =>
                (error.Number == 2601 || error.Number == 2627)
                && error.Message.Contains(
                    "UX_Diagnosticos_InicialPorAtencion",
                    StringComparison.OrdinalIgnoreCase));
    }
}
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
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && sql.Errors.Cast<SqlError>().Any(e =>
            (e.Number == 2601 || e.Number == 2627) && e.Message.Contains("IX_Pagos_IdOperacion", StringComparison.OrdinalIgnoreCase)))
        {
            throw new ConflictoConcurrenciaException("Este cobro pudo haberse registrado. Se comprobará el identificador del intento.", ex);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && sql.Errors.Cast<SqlError>().Any(e =>
            (e.Number == 2601 || e.Number == 2627) && e.Message.Contains("IX_Comprobantes_IdAtencion", StringComparison.OrdinalIgnoreCase)))
        {
            throw new ValidacionException("La atención ya fue liquidada por otra operación. Actualice el listado.");
        }
        catch (DbUpdateException ex)
            when (EsDiagnosticoDuplicado(ex))
        {
            throw new ValidacionException(
                "La atención ya tiene registrado su informe técnico definitivo. Actualice la información.");
        }
        finally
        {
            // Las operaciones siguientes deben consultar datos actuales.
            // Ningún cambio fallido debe quedar pendiente de otro guardado.
            _contexto.ChangeTracker.Clear();
        }
    }

    public async Task<T> LeerConsistenteAsync<T>(Func<Task<T>> consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);
        if (!_contexto.Database.IsRelational() || _contexto.Database.CurrentTransaction is not null)
            return await consulta();
        await using var transaccion = await _contexto.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var resultado = await consulta();
        await transaccion.CommitAsync();
        return resultado;
    }

    public void DescartarCambios()
    {
        _contexto.ChangeTracker.Clear();
    }

    private static bool EsDiagnosticoDuplicado(
        DbUpdateException excepcion)
    {
        return excepcion.InnerException is SqlException sqlEx
            && sqlEx.Errors.Cast<SqlError>().Any(error =>
                (error.Number == 2601 || error.Number == 2627)
                && error.Message.Contains(
                    "UX_Diagnosticos_Atencion",
                    StringComparison.OrdinalIgnoreCase));
    }
}
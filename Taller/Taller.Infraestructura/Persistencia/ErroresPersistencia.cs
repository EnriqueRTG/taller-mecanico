using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Excepciones;

namespace Taller.Infraestructura.Persistencia;
/// <summary>Traduce conflictos esperables sin confundir fallos de conexión con validaciones de negocio.</summary>
internal static class ErroresPersistencia
{
    public static Exception Traducir(DbUpdateException e, string indice, string duplicado)
    {
        if (e is DbUpdateConcurrencyException)
            return new ConflictoConcurrenciaException("El registro cambió o fue eliminado. Actualice y vuelva a abrir la operación.", e);
        if (e.InnerException is SqlException s && s.Errors.Cast<SqlError>().Any(x => x.Number is 2601 or 2627 && x.Message.Contains(indice, StringComparison.OrdinalIgnoreCase)))
            return new ValidacionException(duplicado);
        return e;
    }
}

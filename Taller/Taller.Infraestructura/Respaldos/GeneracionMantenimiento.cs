namespace Taller.Infraestructura.Respaldos;
/// <summary>Invalida todas las sesiones del proceso cuando la base ha sido reemplazada.</summary>
public sealed class GeneracionMantenimiento
{
    private long _generacion;
    public void Validar(long actual)
    {
        if (actual <= 0)
            throw new InvalidOperationException("Generación de recuperación inválida.");
        long anterior = Interlocked.CompareExchange(ref _generacion, actual, 0);
        if (anterior != 0 && anterior != actual)
            throw new InvalidOperationException("La base fue restaurada. Cierre SGTM y vuelva a iniciarlo antes de continuar; no se guardaron datos de esta sesión anterior.");
    }
}

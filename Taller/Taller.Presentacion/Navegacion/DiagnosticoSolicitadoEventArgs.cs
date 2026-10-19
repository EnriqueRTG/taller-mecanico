namespace Taller.Presentacion.Navegacion;

/// <summary>
/// Identifica la atención cuyo diagnóstico se desea iniciar o retomar.
/// </summary>
public sealed class DiagnosticoSolicitadoEventArgs : EventArgs
{
    public int IdAtencion { get; }

    public DiagnosticoSolicitadoEventArgs(int idAtencion)
    {
        if (idAtencion <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idAtencion));
        }

        IdAtencion = idAtencion;
    }
}
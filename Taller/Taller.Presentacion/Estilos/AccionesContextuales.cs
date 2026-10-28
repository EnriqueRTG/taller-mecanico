namespace Taller.Presentacion.Estilos;

/// <summary>Oculta acciones sin permiso, selección o etapa. No interpreta el bloqueo temporal de un panel ocupado como una regla de negocio.</summary>
internal static class AccionesContextuales
{
    public static void Actualizar(params Control[] acciones)
    {
        foreach (var accion in acciones)
        {
            if (accion.Parent is { Enabled: false } contenedor)
            {
                // Enabled incluye el estado heredado. Esperar al panel evita ocultar una acción válida al recargar.
                void AlHabilitar(object? sender, EventArgs e)
                {
                    if (!contenedor.Enabled) return;
                    contenedor.EnabledChanged -= AlHabilitar;
                    if (!accion.IsDisposed) accion.Visible = accion.Enabled;
                }
                contenedor.EnabledChanged += AlHabilitar;
            }
            else accion.Visible = accion.Enabled;
        }
    }
}

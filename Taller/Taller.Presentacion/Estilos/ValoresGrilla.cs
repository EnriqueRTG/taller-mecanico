using Taller.Dominio.Enumeraciones;
using Taller.Aplicacion.Servicios;
namespace Taller.Presentacion.Estilos;
/// <summary>Traduce valores visibles sin modificar los datos enlazados ni las reglas del negocio.</summary>
internal static class ValoresGrilla
{
    public static void Aplicar(DataGridView grilla) => grilla.CellFormatting += Formatear;
    private static void Formatear(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (sender is not DataGridView grilla || e.ColumnIndex < 0 || grilla.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn) return;
        string? texto = e.Value switch
        {
            bool valor => valor ? "Sí" : "No",
            TipoSolicitudAtencion.ServicioDefinido => "Servicio solicitado",
            TipoSolicitudAtencion.Problema => "Problema a diagnosticar",
            TipoSolicitudAtencion.Mixta => "Servicio y problema",
            EstadoAtencion estado => ReporteServicio.Legible(estado.ToString()),
            _ => null
        };
        if (texto is null) return;
        e.Value = texto;
        e.FormattingApplied = true;
    }
}

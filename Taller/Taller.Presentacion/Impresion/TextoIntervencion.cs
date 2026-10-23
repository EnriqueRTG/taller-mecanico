using Taller.Dominio.Tiempo;
using System.Text;
using Taller.Aplicacion.DTOs.Ejecuciones;

namespace Taller.Presentacion.Impresion;
/// <summary>Informe de consulta del expediente: incluye hallazgos sin convertirlos en cargos.</summary>
internal static class TextoIntervencion
{
    public static string Generar(SeguimientoTrabajoDto d)
    {
        var t = new StringBuilder($"SGTM · INFORME DE INTERVENCIÓN\n{d.Resumen}\nEstado al consultar: {d.Estado} · Emitido {FechaHoraTaller.Ahora:dd/MM/yyyy HH:mm}\nEste informe no sustituye la autorización del cliente ni acredita un pago.\n\n");
        t.AppendLine("EVALUACIÓN TÉCNICA\n" + (string.IsNullOrWhiteSpace(d.InformeInicial) ? "Sin diagnóstico registrado: consultar la solicitud definida y las propuestas." : d.InformeInicial));
        t.AppendLine("\nTRABAJOS AUTORIZADOS Y REGISTRO TÉCNICO\n" + d.TrabajosRegistrados);
        t.AppendLine("\nHALLAZGOS Y RECOMENDACIONES");
        foreach (var h in d.Hallazgos.OrderBy(h => h.Fecha))
            t.AppendLine($"{h.Fecha:dd/MM/yyyy HH:mm} · Informe puntual {h.Id} · Técnico {h.IdTecnico} · {(h.IdDetalle.HasValue ? $"Ítem {h.IdDetalle}" : "Intervención general")}\n{h.Descripcion}\nRecomendación: {h.Recomendacion}\nImpide continuar el alcance: {(h.ImpideContinuar ? "Sí" : "No")}\n");
        t.AppendLine("\nPAUSAS Y RESOLUCIONES");
        foreach (var p in d.Pausas.OrderBy(p => p.Fecha))
            t.AppendLine($"{p.Fecha:dd/MM/yyyy HH:mm} · {p.Alcance} · Usuario {p.IdUsuario}\n{p.Motivo}\nResolución: {p.Resolucion ?? "Pendiente"} · Técnico {p.IdTecnicoResolucion} · {p.FechaResolucion:dd/MM/yyyy HH:mm}\n");
        t.AppendLine("\nMATERIALES APORTADOS POR EL CLIENTE · SIN VENTA DEL TALLER");
        foreach (var m in d.Items.Where(m => m.Origen == Taller.Dominio.Enumeraciones.OrigenMaterial.Cliente))
            t.AppendLine($"{m.Etiqueta}\nRequerido {m.Cantidad:N3}, recibido {m.Recibida:N3}, conforme {(m.Conforme ? "Sí" : "No")}. {m.Observaciones}\n");
        t.AppendLine("\nLa finalización técnica y la entrega son actos distintos. La entrega se confirma por recepción con conformidad del cliente y saldo cero. Consulte los comprobantes y recibos para conocer los cobros.");
        return t.ToString();
    }
}

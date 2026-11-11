using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Consultas;
using Taller.Aplicacion.DTOs.Reportes;
using Taller.Dominio.Enumeraciones;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Consultas acotadas y proyectadas en SQL; 
/// los importes provienen de documentos, nunca del precio actual del catálogo.
/// </summary>
public sealed class ReporteRepositorio(TallerDbContext contexto) : IReporteRepositorio
{
    private static readonly string[] AccionesTecnicas = Taller.Aplicacion.Consultas.ActividadTecnicaReporte.Acciones.ToArray();
    private static readonly string[] AccionesAdministrativas = IntervencionAdministrativaReporte.Acciones.ToArray();

    /// <summary>
    /// Obtiene los registros que usa el servicio de reportes para calcular
    /// los indicadores de las tarjetas de Inicio.
    /// </summary>
    /// <param name="tipo">Tipo de reporte que determina la consulta a ejecutar.</param>
    /// <param name="desde">
    /// Inicio inclusivo del período para las consultas que filtran por fecha.
    /// </param>
    /// <param name="hastaExclusivo">
    /// Fin exclusivo del período. Para incluir el último día seleccionado,
    /// el servicio pasa el comienzo del día siguiente.
    /// </param>
    /// <param name="tecnico">
    /// Identificador opcional del técnico. En Carga técnica incluye sus
    /// reservas y las atenciones sin técnico asignado. En Actividad técnica
    /// limita los eventos al usuario indicado. En Presupuestos se pasa
    /// a la consulta que lista las propuestas.
    /// </param>
    /// <returns>
    /// Tarea con las filas necesarias para calcular los indicadores.
    /// Los campos proyectados dependen del tipo de reporte.
    /// </returns>
    /// <remarks>
    /// Carga técnica y Saldos pendientes consultan la situación actual.
    /// Los conteos, las sumas y los porcentajes se calculan en ReporteServicio.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// El tipo solicitado no tiene una consulta para las tarjetas de Inicio.
    /// </exception>
    public Task<List<ReporteRegistroDto>> ListarIndicadoresAsync(
        TipoReporte tipo, 
        DateTime desde, 
        DateTime hastaExclusivo,
        int? tecnico)
    {
        return tipo switch
        {
            TipoReporte.Atenciones => contexto.Atenciones.AsNoTracking()
                .Where(a => a.FechaApertura >= desde && a.FechaApertura < hastaExclusivo)
                .Select(a => new ReporteRegistroDto { IdAtencion = a.IdAtencion, Estado = a.Estado.ToString() }).ToListAsync(),

            TipoReporte.CargaTecnica => contexto.Atenciones.AsNoTracking()
                .Where(a => (a.Estado == EstadoAtencion.EnEvaluacionTecnica || a.Estado == EstadoAtencion.EnEjecucion)
                    && (!tecnico.HasValue || a.IdTecnicoEnActividad == null || a.IdTecnicoEnActividad == tecnico))
                .Select(a => new ReporteRegistroDto { IdAtencion = a.IdAtencion,
                    Estado = a.Pausas.Any(p => p.FechaResolucion == null) ? "Con pausas pendientes"
                        : a.IdTecnicoEnActividad == null ? "Disponible para tomar" : "Reservada" }).ToListAsync(),

            TipoReporte.ActividadTecnica => contexto.HistorialAtenciones.AsNoTracking()
                .Where(h => h.Fecha >= desde && h.Fecha < hastaExclusivo && AccionesTecnicas.Contains(h.Accion)
                    && (!tecnico.HasValue || h.IdUsuario == tecnico))
                .Select(h => new ReporteRegistroDto { IdAtencion = h.IdAtencion }).ToListAsync(),

            TipoReporte.SaldosPendientes => contexto.Comprobantes.AsNoTracking().Where(c => c.Total > c.TotalPagado)
                .Select(c => new ReporteRegistroDto { IdAtencion = c.IdAtencion, Fecha = c.FechaEmision, Importe = c.Total - c.TotalPagado }).ToListAsync(),

            TipoReporte.MovimientosCobro => contexto.PagoMedios.AsNoTracking()
                .Where(m => m.Pago.Fecha >= desde && m.Pago.Fecha < hastaExclusivo)
                .Select(m => new ReporteRegistroDto { IdAtencion = m.Pago.Comprobante.IdAtencion, Estado = "Cobro", Importe = m.Importe })
                .Concat(contexto.PagoMedios.AsNoTracking()
                    .Where(m => m.Pago.Anulado && m.Pago.FechaAnulacion >= desde && m.Pago.FechaAnulacion < hastaExclusivo)
                    .Select(m => new ReporteRegistroDto { IdAtencion = m.Pago.Comprobante.IdAtencion, Estado = "Anulación", Importe = -m.Importe })).ToListAsync(),

            // Esta consulta ya proyecta cantidades/precios por ítem. Se conserva el
            // redondeo por ítem en .NET para no alterar importes autorizados.
            TipoReporte.Presupuestos => ListarPresupuestosAsync(desde, hastaExclusivo, tecnico),

            _ => throw new InvalidOperationException("El tipo no corresponde a una tarjeta de inicio.")
        };
    }

    public Task<List<ReporteRegistroDto>> ListarCargaAsync(int? tecnico) =>
        contexto.Atenciones
            .AsNoTracking()
            .Where(a => (a.Estado == EstadoAtencion.EnEvaluacionTecnica || a.Estado == EstadoAtencion.EnEjecucion) && (!tecnico.HasValue || a.IdTecnicoEnActividad == null || a.IdTecnicoEnActividad == tecnico))
            .Select(a => new ReporteRegistroDto
            {
                Codigo = "A-" + a.IdAtencion,
                IdAtencion = a.IdAtencion,
                Fecha = a.FechaApertura,
                Descripcion = a.Vehiculo.Dominio + " · " + a.MotivoConsulta,
                Responsable = a.TecnicoEnActividad == null ? "Disponible para tomar" : a.TecnicoEnActividad.Apellido + ", " + a.TecnicoEnActividad.Nombre,
                Estado = a.Pausas.Any(p => p.FechaResolucion == null) ? "Con pausas pendientes" : a.IdTecnicoEnActividad == null ? "Disponible para tomar" : "Reservada",
                Grupo = a.Estado.ToString()
            })
            .ToListAsync();

    public Task<List<ReporteRegistroDto>> ListarIntervencionesAsync(DateTime desde, DateTime hastaExclusivo, int administrativo) =>
        contexto.HistorialAtenciones.AsNoTracking()
            .Where(h => h.IdUsuario == administrativo && h.Fecha >= desde && h.Fecha < hastaExclusivo
                && AccionesAdministrativas.Contains(h.Accion))
            .Select(h => new ReporteRegistroDto
            {
                Codigo = "I-" + h.IdHistorialAtencion,
                IdAtencion = h.IdAtencion,
                Fecha = h.Fecha,
                Descripcion = h.Atencion.Vehiculo.Dominio + " · Estado actual: " + h.Atencion.Estado.ToString()
                    + " · " + (h.Observaciones ?? ""),
                Responsable = h.Usuario.Apellido + ", " + h.Usuario.Nombre,
                Estado = h.Accion,
                Grupo = h.Accion
            }).ToListAsync();

    public Task<List<ReporteRegistroDto>> ListarAtencionesAsync(DateTime desde, DateTime hastaExclusivo) =>
        contexto.Atenciones
            .AsNoTracking()
            .Where(a => a.FechaApertura >= desde && a.FechaApertura < hastaExclusivo)
            .Select(a => new ReporteRegistroDto
            {
                Codigo = "A-" + a.IdAtencion,
                IdAtencion = a.IdAtencion,
                Fecha = a.FechaApertura,
                Descripcion = a.Vehiculo.Dominio + " · " + a.Cliente.Apellido + ", " + a.Cliente.Nombre + " · " + a.MotivoConsulta,
                Responsable = a.UsuarioRecepcion.Apellido + ", " + a.UsuarioRecepcion.Nombre,
                Estado = a.Estado.ToString(),
                Grupo = a.Estado.ToString()
            })
            .ToListAsync();

    public async Task<List<ReporteRegistroDto>> ListarPresupuestosAsync(DateTime desde, DateTime hastaExclusivo, int? tecnico, int? administrativo = null)
    {
        var propuestas = await contexto.Presupuestos
            .AsNoTracking()
            .Where(p => p.Vigente && (!tecnico.HasValue || p.IdUsuarioCreacion == tecnico)
                && (administrativo.HasValue
                    ? p.IdUsuarioDecision == administrativo && p.FechaDecision >= desde && p.FechaDecision < hastaExclusivo
                    : p.FechaCreacion >= desde && p.FechaCreacion < hastaExclusivo))
            .Select(p => new
            {
                p.IdPresupuesto,
                p.IdAtencion,
                Fecha = administrativo.HasValue ? p.FechaDecision!.Value : p.FechaCreacion,
                p.Estado,
                Dominio = p.Atencion.Vehiculo.Dominio,
                Responsable = administrativo.HasValue
                    ? p.UsuarioDecision!.Apellido + ", " + p.UsuarioDecision.Nombre
                    : p.UsuarioCreacion.Apellido + ", " + p.UsuarioCreacion.Nombre,
                Detalles = p.Detalles
                    .Select(d => new { d.Cantidad, d.PrecioUnitario, d.AceptadoPorCliente })
                    .ToList()
            })
            .ToListAsync();
        // Se redondea cada ítem igual que en el documento original, antes de sumar. No se usa el precio del catálogo.
        return propuestas
            .Select(p => new ReporteRegistroDto
            {
                Codigo = "P-" + p.IdPresupuesto,
                IdAtencion = p.IdAtencion,
                Fecha = p.Fecha,
                Descripcion = p.Dominio + " · Atención " + p.IdAtencion,
                Responsable = p.Responsable,
                Estado = p.Estado == EstadoPresupuesto.Aceptado && p.Detalles.Any(d => d.AceptadoPorCliente == false) ? "Aceptado parcialmente" : p.Estado.ToString(),
                Grupo = p.Estado.ToString(),
                Importe = p.Detalles.Sum(d => decimal.Round(d.Cantidad * d.PrecioUnitario, 2, MidpointRounding.AwayFromZero)),
                ImporteAceptado = p.Estado != EstadoPresupuesto.Aceptado ? 0 : p.Detalles
                    .Where(d => d.AceptadoPorCliente == true).Sum(d => decimal.Round(d.Cantidad * d.PrecioUnitario, 2, MidpointRounding.AwayFromZero))
            })
            .ToList();
    }

    public Task<List<ReporteRegistroDto>> ListarMovimientosAsync(DateTime desde, DateTime hastaExclusivo, int? administrativo = null)
    {
        // Un solo comando SQL evita mezclar dos lecturas entre un cobro concurrente y su anulación.
        var medios = contexto.PagoMedios
            .AsNoTracking();
        var cobros = medios
            .Where(m => m.Pago.Fecha >= desde && m.Pago.Fecha < hastaExclusivo
                && (!administrativo.HasValue || m.Pago.IdUsuarioRegistro == administrativo))
            .Select(m => new
            {
                m.IdPago,
                m.Orden,
                m.Pago.Comprobante.IdAtencion,
                Fecha = m.Pago.Fecha,
                Descripcion = m.Pago.Comprobante.Dominio + " · " + m.Pago.Comprobante.Cliente,
                Responsable = m.Pago.UsuarioRegistroNombre,
                Anulacion = false,
                Grupo = m.NombreMedio,
                m.Importe
            });
        var anulaciones = medios
            .Where(m => m.Pago.Anulado && m.Pago.FechaAnulacion >= desde && m.Pago.FechaAnulacion < hastaExclusivo
                && (!administrativo.HasValue || m.Pago.IdUsuarioAnulacion == administrativo))
            .Select(m => new
            {
                m.IdPago,
                m.Orden,
                m.Pago.Comprobante.IdAtencion,
                Fecha = m.Pago.FechaAnulacion!.Value,
                Descripcion = m.Pago.Comprobante.Dominio + " · " + m.Pago.MotivoAnulacion,
                Responsable = m.Pago.UsuarioAnulacion!.Apellido + ", " + m.Pago.UsuarioAnulacion.Nombre,
                Anulacion = true,
                Grupo = m.NombreMedio,
                Importe = -m.Importe
            });
        return cobros.Concat(anulaciones)
            .Select(m => new ReporteRegistroDto
            {
                Codigo = (m.Anulacion ? "AN-R-" : "R-") + m.IdPago + "/" + m.Orden,
                IdAtencion = m.IdAtencion,
                Fecha = m.Fecha,
                Descripcion = m.Descripcion,
                Responsable = m.Responsable,
                Estado = m.Anulacion ? "Anulación" : "Cobro",
                Grupo = m.Grupo,
                Importe = m.Importe
            })
            .ToListAsync();
    }

    public Task<List<ReporteRegistroDto>> ListarSaldosAsync(int? administrativo = null) =>
        contexto.Comprobantes
            .AsNoTracking()
            .Where(c => c.Total > c.TotalPagado && (!administrativo.HasValue || c.Atencion.Historial.Any(h =>
                h.IdUsuario == administrativo && AccionesAdministrativas.Contains(h.Accion))))
            .Select(c => new ReporteRegistroDto
            {
                Codigo = "L-" + c.IdComprobante,
                IdAtencion = c.IdAtencion,
                Fecha = c.FechaEmision,
                Descripcion = c.Dominio + " · " + c.Cliente,
                Responsable = c.UsuarioEmision.Apellido + ", " + c.UsuarioEmision.Nombre,
                Estado = c.TotalPagado == 0 ? "Sin cobros" : "Cobrado parcialmente",
                Grupo = c.TotalPagado == 0 ? "Sin cobros" : "Cobrado parcialmente",
                Importe = c.Total - c.TotalPagado
            })
            .ToListAsync();

    public Task<List<ReporteRegistroDto>> ListarActividadAsync(DateTime desde, DateTime hastaExclusivo, int? tecnico) =>
        contexto.HistorialAtenciones
            .AsNoTracking()
            .Where(h => h.Fecha >= desde && h.Fecha < hastaExclusivo && AccionesTecnicas.Contains(h.Accion) && (!tecnico.HasValue || h.IdUsuario == tecnico))
            .Select(h => new ReporteRegistroDto
            {
                Codigo = "EV-" + h.IdHistorialAtencion,
                IdAtencion = h.IdAtencion,
                Fecha = h.Fecha,
                Descripcion = h.Atencion.Vehiculo.Dominio + " · " + h.Accion + " · " + (h.Observaciones ?? ""),
                Responsable = h.Usuario.Apellido + ", " + h.Usuario.Nombre,
                Estado = h.Accion,
                Grupo = h.Accion
            })
            .ToListAsync();

    public Task<List<ReporteRegistroDto>> ListarConceptosAsync(DateTime desde, DateTime hastaExclusivo, int? tecnico) =>
        contexto.PresupuestoDetalles
            .AsNoTracking()
            .Where(d => d.CantidadEjecutada > 0 && d.FechaRegistroEjecucion >= desde && d.FechaRegistroEjecucion < hastaExclusivo && (!tecnico.HasValue || d.IdTecnicoRegistro == tecnico))
            .Select(d => new ReporteRegistroDto
            {
                Codigo = "ITEM-" + d.IdPresupuestoDetalle,
                IdAtencion = d.Presupuesto.IdAtencion,
                Fecha = d.FechaRegistroEjecucion!.Value,
                Descripcion = d.Descripcion + " · " + d.Presupuesto.Atencion.Vehiculo.Dominio,
                Responsable = d.TecnicoRegistro!.Apellido + ", " + d.TecnicoRegistro.Nombre,
                Estado = d.EstadoEjecucion.ToString(),
                Grupo = d.Concepto.Tipo.ToString(),
                Cantidad = d.CantidadEjecutada,
                Importe = decimal.Round(d.CantidadEjecutada * d.PrecioUnitario, 2, MidpointRounding.AwayFromZero)
            })
            .ToListAsync();
}

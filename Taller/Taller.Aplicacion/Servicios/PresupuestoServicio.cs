using Taller.Dominio.Tiempo;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Presupuestos;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.Servicios;
/// <summary>Registra decisiones por ítem y revisiones conservando propuestas y decisiones anteriores.</summary>
public sealed class PresupuestoServicio(IEvaluacionRepositorio evaluacion, ICircuitoRepositorio repositorio, IUnidadDeTrabajo unidad, AutorizacionOperativa acceso)
{
    private async Task<Presupuesto> RequerirAsync(int id) => await repositorio.ObtenerPresupuestoAsync(id) ?? throw new ValidacionException("El presupuesto no existe.");
    public async Task<PresupuestoGestionDto> ObtenerAsync(int id)
    {
        try
        {
            await acceso.ExigirAsync();
            var p = await RequerirAsync(id);
            return new PresupuestoGestionDto
            {
                EsAdicional = p.EsAdicional,
                EsInspeccion = p.EsInspeccion,
                IdPresupuesto = id,
                IdAtencion = p.IdAtencion,
                Estado = p.Estado,
                Vigente = p.Vigente,
                Version = p.Version.ToArray(),
                Resumen = $"Atención {p.IdAtencion:D6} · {p.Atencion.Cliente.Apellido}, {p.Atencion.Cliente.Nombre} · {p.Atencion.Vehiculo.Dominio}",
                Observaciones = p.ObservacionesDecision ?? p.MotivoRevision ?? p.Observaciones,
                Items = p.Detalles.OrderBy(d => d.IdPresupuestoDetalle).Select(d => new DecisionItemDto { IdSolicitudAtencion = d.IdSolicitudAtencion, IdConcepto = d.IdConcepto, OrigenMaterial = d.OrigenMaterial, IdDetalle = d.IdPresupuestoDetalle, Aceptar = d.AceptadoPorCliente ?? true, Descripcion = d.Descripcion, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario }).ToList()
            };
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task DecidirAsync(DecisionPresupuestoDto datos)
    {
        ArgumentNullException.ThrowIfNull(datos);
        try
        {
            var usuario = await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            var p = await RequerirAsync(datos.IdPresupuesto);
            AutorizacionOperativa.Version(datos.Version, p.Version);
            if (!p.Vigente || p.Estado != EstadoPresupuesto.PendienteDecision || (!p.EsAdicional && (p.Atencion.Estado != EstadoAtencion.PendienteDecision || p.Atencion.IdTecnicoEnActividad != null)) || (p.EsAdicional && p.Atencion.Estado != (p.EsInspeccion ? EstadoAtencion.EnEvaluacionTecnica : EstadoAtencion.EnEjecucion)))
                throw new ValidacionException("La propuesta no está disponible para registrar una decisión.");
            if (await repositorio.ObtenerPorAtencionAsync(p.IdAtencion) != null)
                throw new ValidacionException("La atención ya está liquidada.");
            if (datos.ItemsAceptados is null || datos.ItemsAceptados.Distinct().Count() != datos.ItemsAceptados.Count || datos.ItemsAceptados.Except(p.Detalles.Select(d => d.IdPresupuestoDetalle)).Any())
                throw new ValidacionException("La selección contiene ítems repetidos o ajenos a esta propuesta.");
            if (p.Detalles.Count == 0)
                throw new ValidacionException("No puede decidir sobre una propuesta vacía.");
            var persona = datos.PersonaDecision?.Trim() ?? "";
            var medio = datos.MedioDecision?.Trim() ?? "";
            if (persona.Length > 150 || medio.Length > 30 || p.Atencion.Solicitudes.Count > 0 && (persona.Length == 0 || !new[] { "Presencial", "Teléfono", "Correo", "Mensajería" }.Contains(medio)))
                throw new ValidacionException("Identifique quién comunica la decisión y seleccione el medio de comunicación.");
            p.PersonaDecision = persona.Length == 0 ? null : persona;
            p.MedioDecision = medio.Length == 0 ? null : medio;
            var motivo = AutorizacionOperativa.Texto(datos.Observaciones, datos.ItemsAceptados.Count != p.Detalles.Count);
            foreach (var d in p.Detalles)
                d.AceptadoPorCliente = datos.ItemsAceptados.Contains(d.IdPresupuestoDetalle);
            var parcial = datos.ItemsAceptados.Count > 0 && datos.ItemsAceptados.Count < p.Detalles.Count;
            p.Estado = parcial ? EstadoPresupuesto.PendienteRevisionTecnica : datos.ItemsAceptados.Count > 0 ? EstadoPresupuesto.Aceptado : EstadoPresupuesto.Rechazado;
            p.AlcanceValidado = !parcial;
            p.FechaDecision = FechaHoraTaller.Ahora;
            p.IdUsuarioDecision = usuario.Id;
            p.ObservacionesDecision = motivo;
            var a = p.Atencion;
            var anterior = a.Estado;
            if (!p.EsAdicional)
                a.Estado = parcial ? EstadoAtencion.PendienteDecision : p.Estado == EstadoPresupuesto.Aceptado ? EstadoAtencion.EnEjecucion : EstadoAtencion.PendienteLiquidacion;
            evaluacion.PrepararVersion(a, a.Version);
            evaluacion.AgregarHistorial(new HistorialAtencion { IdAtencion = a.IdAtencion, IdUsuario = usuario.Id, Fecha = FechaHoraTaller.Ahora, Accion = "DecisionPresupuestoRegistrada", EstadoAnterior = anterior, EstadoNuevo = a.Estado, Observaciones = $"Presupuesto {p.IdPresupuesto}: {datos.ItemsAceptados.Count}/{p.Detalles.Count} ítems aceptados. Persona: {p.PersonaDecision ?? "No informada (registro anterior)"}. Medio: {p.MedioDecision ?? "No informado"}. {motivo}" });
            await unidad.GuardarCambiosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    /// <summary>Solo revisa antes de ejecutar trabajos de esta propuesta. No borra ni modifica su contenido histórico.</summary>
    public async Task<int> SolicitarRevisionAsync(int id, byte[] version, string motivo)
    {
        try
        {
            var usuario = await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            motivo = AutorizacionOperativa.Texto(motivo);
            var p = await RequerirAsync(id);
            AutorizacionOperativa.Version(version, p.Version);
            if (p.EsAdicional)
            {
                if (!p.Vigente || p.Estado is EstadoPresupuesto.Borrador or EstadoPresupuesto.PendienteRevisionTecnica || p.Detalles.Any(d => d.CantidadEjecutada > 0 || d.EstadoEjecucion != EstadoEjecucionDetalle.NoIniciado) || p.Atencion.FechaCierre != null || await repositorio.ObtenerPorAtencionAsync(p.IdAtencion) != null)
                    throw new ValidacionException("Solo puede revisar un adicional sin ejecución, sin liquidación y aún vigente.");
                p.Vigente = false;
                var revision = new Presupuesto
                {
                    IdAtencion = p.IdAtencion,
                    IdDiagnostico = p.IdDiagnostico,
                    EsAdicional = true,
                    EsInspeccion = p.EsInspeccion,
                    IdHallazgoAtencion = p.IdHallazgoAtencion,
                    IdPresupuestoAnterior = p.IdPresupuesto,

                    Estado = EstadoPresupuesto.PendienteRevisionTecnica,
                    IdUsuarioCreacion = usuario.Id,
                    FechaCreacion = FechaHoraTaller.Ahora,
                    FechaPresentacion = FechaHoraTaller.Ahora,
                    FechaDecision = FechaHoraTaller.Ahora,
                    IdUsuarioDecision = usuario.Id,
                    MotivoRevision = motivo,
                    ObservacionesDecision = motivo,
                    Detalles = p.Detalles.Select(d => new PresupuestoDetalle { IdSolicitudAtencion = d.IdSolicitudAtencion, IdConcepto = d.IdConcepto, Descripcion = d.Descripcion, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, OrigenMaterial = d.OrigenMaterial, AceptadoPorCliente = true }).ToList()
                };
                evaluacion.AgregarPresupuesto(revision);
                evaluacion.PrepararVersion(p.Atencion, p.Atencion.Version);
                evaluacion.AgregarHistorial(new HistorialAtencion { IdAtencion = p.IdAtencion, IdUsuario = usuario.Id, Fecha = FechaHoraTaller.Ahora, Accion = "RevisionAdicionalSolicitada", EstadoAnterior = p.Atencion.Estado, EstadoNuevo = p.Atencion.Estado, Observaciones = motivo });
                await unidad.GuardarCambiosAsync();
                return revision.IdPresupuesto;
            }

            if (p.Estado == EstadoPresupuesto.PendienteRevisionTecnica || !p.Vigente || p.Estado == EstadoPresupuesto.Borrador || p.Atencion.IdTecnicoEnActividad != null || p.Atencion.Estado is EstadoAtencion.Cerrada or EstadoAtencion.Cancelada || p.Detalles.Any(d => d.CantidadEjecutada > 0 || d.EstadoEjecucion != EstadoEjecucionDetalle.NoIniciado) || await repositorio.ObtenerPorAtencionAsync(p.IdAtencion) != null)
                throw new ValidacionException("Solo puede revisar una propuesta vigente, sin reserva, sin ejecución ni liquidación. Con trabajos iniciados conserve el avance y resuelva las pausas o adicionales antes de continuar.");
            p.Vigente = false;
            var nueva = new Presupuesto
            {
                IdAtencion = p.IdAtencion,
                IdDiagnostico = p.IdDiagnostico,
                IdPresupuestoAnterior = id,

                Estado = EstadoPresupuesto.Borrador,
                IdUsuarioCreacion = usuario.Id,
                FechaCreacion = FechaHoraTaller.Ahora,
                MotivoRevision = motivo,
                Observaciones = motivo,
                Detalles = p.Detalles.Select(d => new PresupuestoDetalle { IdSolicitudAtencion = d.IdSolicitudAtencion, IdConcepto = d.IdConcepto, Descripcion = d.Descripcion, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, OrigenMaterial = d.OrigenMaterial }).ToList()
            };
            evaluacion.AgregarPresupuesto(nueva);
            var a = p.Atencion;
            var anterior = a.Estado;
            a.Estado = EstadoAtencion.EnEvaluacionTecnica;
            var avance = await evaluacion.ObtenerAvanceAsync(a.IdAtencion);
            if (avance != null)
            {
                avance.Finalizada = false;
                avance.SoloEvaluacion = false;
                avance.FechaActualizacion = FechaHoraTaller.Ahora;
            }

            evaluacion.PrepararVersion(a, a.Version);
            evaluacion.AgregarHistorial(new HistorialAtencion { IdAtencion = a.IdAtencion, IdUsuario = usuario.Id, Fecha = FechaHoraTaller.Ahora, Accion = "RevisionPresupuestoSolicitada", EstadoAnterior = anterior, EstadoNuevo = a.Estado, Observaciones = $"Se conserva presupuesto {id}. {motivo}" });
            await unidad.GuardarCambiosAsync();
            return nueva.IdPresupuesto;
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }
}

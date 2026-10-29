using Taller.Dominio.Tiempo;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.Servicios;
/// <summary>Coordina recepción, reserva, avances e informe/propuesta sin confundir autorización, ejecución y pago.</summary>
public sealed class EvaluacionServicio(IEvaluacionRepositorio repositorio, IUsuarioRepositorio usuarios, IUnidadDeTrabajo unidad, SesionUsuario sesion)
{
    private async Task<Usuario> AutorizarAsync(bool tecnico = false, bool recepcion = false)
    {
        var id = sesion.UsuarioActual?.Id ?? throw new ValidacionException("Debe iniciar sesión.");
        var usuario = await usuarios.ObtenerPorIdAsync(id);
        if (!UsuarioAutorizado.EstaHabilitado(usuario) || (tecnico && usuario.RolId != RolesSistema.TecnicoId) || (recepcion && usuario.RolId != RolesSistema.AdministrativoId) || (!tecnico && !recepcion && usuario.RolId != RolesSistema.TecnicoId && usuario.RolId != RolesSistema.AdministrativoId))
            throw new ValidacionException("No tiene permisos para esta operación.");
        return usuario;
    }

    private async Task<Atencion> AtencionAsync(int id)
    {
        if (id <= 0)
            throw new ValidacionException("Seleccione una atención válida.");
        return await repositorio.ObtenerAtencionAsync(id) ?? throw new ValidacionException("La atención no existe.");
    }

    private static void VerificarActividad(Atencion a, Usuario u)
    {
        if (a.Estado != EstadoAtencion.EnEvaluacionTecnica || a.FechaCierre is not null || a.IdTecnicoEnActividad != u.Id || a.ActividadTecnica is not (ActividadTecnica.Diagnostico or ActividadTecnica.Presupuesto))
            throw new ValidacionException("La evaluación debe estar reservada por usted. Actualice el listado.");
    }

    private void Evento(Atencion a, Usuario u, string accion, EstadoAtencion anterior, string texto) => repositorio.AgregarHistorial(new HistorialAtencion { IdAtencion = a.IdAtencion, IdUsuario = u.Id, Fecha = FechaHoraTaller.Ahora, Accion = accion, EstadoAnterior = anterior, EstadoNuevo = a.Estado, Observaciones = texto });


    /// <summary>Cancela una apertura antes del ingreso técnico; conserva el motivo y el historial.</summary>
    public async Task CancelarIngresoAsync(int id, byte[] version, string motivo)
    {
        try
        {
            var usuario = await AutorizarAsync(recepcion: true);
            var a = await AtencionAsync(id);
            Seguridad.AutorizacionOperativa.Version(version, a.Version);
            motivo = Seguridad.AutorizacionOperativa.Texto(motivo);
            if (a.Estado is not (EstadoAtencion.EnEvaluacionTecnica) || a.IdTecnicoEnActividad != null || a.FechaCierre != null || a.Hallazgos.Count > 0 || a.Pausas.Count > 0 || await repositorio.ObtenerAvanceAsync(id) != null || (await repositorio.ObtenerPresupuestosAsync(id)).Count > 0 || (await repositorio.ObtenerDiagnosticosAsync(id)).Count > 0)
                throw new ValidacionException("Solo puede cancelar un ingreso sin actividad técnica, evaluación ni servicios autorizados. Los trabajos iniciados requieren resolver la actividad técnica y liquidar lo realizado.");
            var anterior = a.Estado;
            a.Estado = EstadoAtencion.Cancelada;
            a.FechaCierre = FechaHoraTaller.Ahora;
            repositorio.PrepararVersion(a, version);
            Evento(a, usuario, "AperturaCancelada", anterior, motivo);
            await unidad.GuardarCambiosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task TomarAsync(int id)
    {
        try
        {
            var usuario = await AutorizarAsync(tecnico: true);
            var a = await AtencionAsync(id);
            if (a.Estado != EstadoAtencion.EnEvaluacionTecnica)
                throw new ValidacionException("La atención no está pendiente de evaluación.");
            if ((await repositorio.ObtenerAvanceAsync(id))?.Finalizada == true)
                throw new ValidacionException("La evaluación ya está finalizada.");
            if ((await repositorio.ObtenerPresupuestosAsync(id)).Any(p => !p.EsAdicional && p.Vigente && p.Estado != EstadoPresupuesto.Borrador))
                throw new ValidacionException("La propuesta ya está preparada. Consúltela en Presupuestos.");
            if (a.IdTecnicoEnActividad is not null)
                throw new ValidacionException("La actividad ya fue tomada. Actualice el listado.");
            var anterior = a.Estado;
            a.Estado = EstadoAtencion.EnEvaluacionTecnica;
            a.IdTecnicoEnActividad = usuario.Id;
            a.ActividadTecnica = a.TipoSolicitud == TipoSolicitudAtencion.ServicioDefinido ? ActividadTecnica.Presupuesto : ActividadTecnica.Diagnostico;
            Evento(a, usuario, "EvaluacionTomada", anterior, "El técnico tomó la evaluación y preparación de propuesta.");
            await unidad.GuardarCambiosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task LiberarAsync(int id)
    {
        try
        {
            var usuario = await AutorizarAsync(tecnico: true);
            var a = await AtencionAsync(id);
            VerificarActividad(a, usuario);
            a.IdTecnicoEnActividad = null;
            a.ActividadTecnica = null;
            Evento(a, usuario, "EvaluacionLiberada", a.Estado, "Se liberó la actividad conservando todos los avances guardados.");
            await unidad.GuardarCambiosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task<List<EvaluacionBandejaDto>> ListarAsync()
    {
        try
        {
            await AutorizarAsync();
            return await repositorio.ListarAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task<List<ConceptoPropuestaDto>> ListarCatalogoAsync()
    {
        try
        {
            await AutorizarAsync(tecnico: true);
            return (await repositorio.ListarCatalogoAsync()).Select(c => new ConceptoPropuestaDto { IdConcepto = c.IdConcepto, Nombre = c.Nombre, Categoria = c.Categoria.Nombre, Descripcion = c.Descripcion, Tipo = c.Tipo, PrecioReferencia = c.PrecioReferencia }).ToList();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task<EvaluacionAtencionDto> ObtenerAsync(int id)
    {
        try
        {
            var usuario = await AutorizarAsync(tecnico: true);
            var a = await AtencionAsync(id);
            VerificarActividad(a, usuario);
            var avance = await repositorio.ObtenerAvanceAsync(id);
            if (avance?.Finalizada == true)
                throw new ValidacionException("La evaluación ya se finalizó.");
            var presupuestos = await repositorio.ObtenerPresupuestosAsync(id);
            var informe = (await repositorio.ObtenerDiagnosticosAsync(id)).FirstOrDefault();
            var borrador = presupuestos.SingleOrDefault(p => !p.EsAdicional && p.Vigente && p.Estado == EstadoPresupuesto.Borrador);
            return new EvaluacionAtencionDto
            {
                IdAtencion = id,
                VersionAtencion = a.Version.ToArray(),
                TipoSolicitud = a.TipoSolicitud,
                Cliente = a.Cliente.Apellido + ", " + a.Cliente.Nombre,
                Vehiculo = a.Vehiculo.Dominio + " · " + a.Vehiculo.Modelo.Marca.Nombre + " " + a.Vehiculo.Modelo.Nombre,
                Motivo = a.MotivoConsulta,
                Solicitudes = a.Solicitudes.OrderBy(s => s.Orden).Select(s => new SolicitudIngresoDto { IdSolicitud = s.IdSolicitudAtencion, Tipo = s.Tipo, Descripcion = s.Descripcion }).ToList(),
                ObservacionesRecepcion = a.ObservacionesRecepcion,
                MotivoRevision = borrador?.MotivoRevision,
                InformeHistorico = informe != null,
                IncluyeDiagnostico = avance?.IncluyeDiagnostico ?? (informe != null || a.TipoSolicitud != TipoSolicitudAtencion.ServicioDefinido),
                FallaDetectada = avance?.FallaDetectada ?? informe?.FallaDetectada ?? "",
                TrabajoRecomendado = avance?.TrabajoRecomendado ?? informe?.TrabajoRecomendado ?? "",
                Kilometraje = avance?.Kilometraje ?? informe?.Kilometraje ?? a.KilometrajeRecepcion,
                Observaciones = avance?.Observaciones ?? informe?.Observaciones,
                SoloEvaluacion = avance?.SoloEvaluacion ?? false,
                Items = borrador?.Detalles.Select(d => new ItemPropuestaDto { IdSolicitudAtencion = d.IdSolicitudAtencion, OrigenMaterial = d.OrigenMaterial, IdConcepto = d.IdConcepto, Descripcion = d.Descripcion, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario }).ToList() ?? [],
                InspeccionesAutorizadas = presupuestos.Where(p => (p.EsInspeccion && p.Vigente) && p.Estado == EstadoPresupuesto.Aceptado).SelectMany(p => p.Detalles).Select(d => new ServicioEvaluadoDto { IdDetalle = d.IdPresupuestoDetalle, Descripcion = d.Descripcion, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, Estado = d.EstadoEjecucion, Observaciones = d.ObservacionesEjecucion }).ToList()
            };
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    /// <summary>Guarda un avance o finaliza el informe y la propuesta en una única transacción.</summary>
    public async Task GuardarAsync(EvaluacionAtencionDto datos, bool finalizar)
    {
        ArgumentNullException.ThrowIfNull(datos);
        if (datos.Items is null || datos.InspeccionesAutorizadas is null || datos.Items.Any(i => i is null) || datos.InspeccionesAutorizadas.Any(i => i is null))
            throw new ValidacionException("La selección de ítems o servicios es inválida.");
        try
        {
            var usuario = await AutorizarAsync(tecnico: true);
            var a = await AtencionAsync(datos.IdAtencion);
            VerificarActividad(a, usuario);
            if (datos.VersionAtencion is null || datos.VersionAtencion.Length != 8 || !datos.VersionAtencion.SequenceEqual(a.Version))
                throw new ConflictoConcurrenciaException("La atención cambió. Cierre y vuelva a abrir la evaluación.");
            var avance = await repositorio.ObtenerAvanceAsync(a.IdAtencion);
            if (avance?.Finalizada == true)
                throw new ValidacionException("La evaluación ya está finalizada.");
            ValidarTexto(datos.FallaDetectada);
            ValidarTexto(datos.TrabajoRecomendado);
            ValidarTexto(datos.Observaciones);
            if (datos.Kilometraje is < 0)
                throw new ValidacionException("El kilometraje no puede ser negativo.");
            if (finalizar && a.TipoSolicitud != TipoSolicitudAtencion.ServicioDefinido && !datos.IncluyeDiagnostico)
                throw new ValidacionException("Una solicitud por problema requiere registrar el resultado de la evaluación.");
            if (finalizar && datos.IncluyeDiagnostico && (string.IsNullOrWhiteSpace(datos.FallaDetectada) || string.IsNullOrWhiteSpace(datos.TrabajoRecomendado)))
                throw new ValidacionException("Complete el hallazgo/conclusión y el trabajo recomendado.");
            if (datos.SoloEvaluacion && (string.IsNullOrWhiteSpace(datos.Observaciones) || datos.Items.Count > 0))
                throw new ValidacionException("Para finalizar sin propuesta, quite los ítems y explique el motivo en observaciones.");
            if (a.Solicitudes.Count == 0) throw new ValidacionException("La atención no tiene solicitudes registradas. Corrija su recepción antes de preparar una propuesta.");
            if (finalizar && !datos.SoloEvaluacion && datos.Items.Count == 0)
                throw new ValidacionException("Agregue la propuesta o indique que finaliza sin propuesta, con su motivo.");
            var presupuestos = await repositorio.ObtenerPresupuestosAsync(a.IdAtencion);
            if (presupuestos.Any(p => !p.EsAdicional && p.Vigente && p.Estado != EstadoPresupuesto.Borrador))
                throw new ValidacionException("Ya existe una propuesta confirmada. Su revisión requiere un nuevo circuito.");
            if (finalizar && (a.Pausas.Any(p => p.FechaResolucion == null) || presupuestos.Any(p => p.EsAdicional && p.Vigente && p.Estado is EstadoPresupuesto.Borrador or EstadoPresupuesto.PendientePresentacion or EstadoPresupuesto.PendienteDecision or EstadoPresupuesto.PendienteRevisionTecnica)))
                throw new ValidacionException("Resuelva pausas y propuestas adicionales de inspección antes de finalizar la evaluación.");
            if (datos.Items.Any(i => i.IdSolicitudAtencion.HasValue && !a.Solicitudes.Any(s => s.IdSolicitudAtencion == i.IdSolicitudAtencion)))
                throw new ValidacionException("Un ítem refiere a una solicitud ajena a esta atención.");
            if (finalizar && !datos.SoloEvaluacion && datos.Items.Any(i => !i.IdSolicitudAtencion.HasValue))
                throw new ValidacionException("Indique a qué solicitud responde cada ítem del presupuesto.");
            if (finalizar && !datos.SoloEvaluacion && !presupuestos.Any(p => p.Vigente && p.IdPresupuestoAnterior.HasValue) && a.Solicitudes.Any(s => !datos.Items.Any(i => i.IdSolicitudAtencion == s.IdSolicitudAtencion)))
                throw new ValidacionException("El presupuesto debe responder a cada solicitud del cliente. Revise los pedidos antes de enviarlo.");
            var catalogo = (await repositorio.ListarCatalogoAsync()).ToDictionary(c => c.IdConcepto);
            if (datos.Items.GroupBy(i => new { i.IdConcepto, i.IdSolicitudAtencion }).Any(g => g.Count() > 1))
                throw new ValidacionException("No repita conceptos en la propuesta.");
            if (datos.Items.Count > 0)
                _ = SeguimientoTrabajoServicio.PrepararItems(datos.Items, catalogo.Values.ToList());
            decimal total = 0;
            const decimal maximo = 9999999999999999.99m;
            foreach (var item in datos.Items)
            {
                if (!catalogo.ContainsKey(item.IdConcepto))
                    throw new ValidacionException("Un concepto ya no está activo. Actualice el catálogo.");
                if (string.IsNullOrWhiteSpace(item.Descripcion) || item.Descripcion.Trim().Length > 500)
                    throw new ValidacionException("Cada ítem requiere una descripción de hasta 500 caracteres.");
                if (item.Cantidad <= 0 || item.Cantidad > 999999999999999.999m || decimal.Round(item.Cantidad, 3) != item.Cantidad || item.PrecioUnitario < 0 || item.PrecioUnitario > maximo || decimal.Round(item.PrecioUnitario, 2) != item.PrecioUnitario || item.PrecioUnitario > maximo / item.Cantidad)
                    throw new ValidacionException("Revise cantidades y precios: hasta tres y dos decimales respectivamente.");
                if (item.Subtotal > maximo - total)
                    throw new ValidacionException("El total supera el importe permitido.");
                total += item.Subtotal;
            }

            var iniciales = presupuestos.Where(p => p.EsInspeccion && p.Vigente && p.Estado == EstadoPresupuesto.Aceptado).SelectMany(p => p.Detalles).ToList();
            if (datos.InspeccionesAutorizadas.Select(d => d.IdDetalle).Distinct().Count() != iniciales.Count || datos.InspeccionesAutorizadas.Count != iniciales.Count)
                throw new ValidacionException("Los inspecciones autorizadas cambiaron. Vuelva a abrir la evaluación.");
            foreach (var registro in datos.InspeccionesAutorizadas)
            {
                var detalle = iniciales.SingleOrDefault(d => d.IdPresupuestoDetalle == registro.IdDetalle) ?? throw new ValidacionException("Inspección autorizada inválida.");
                if (!Enum.IsDefined(registro.Estado))
                    throw new ValidacionException("Estado de inspección inválido.");
                if (detalle.EstadoEjecucion == EstadoEjecucionDetalle.Completado && registro.Estado != EstadoEjecucionDetalle.Completado)
                    throw new ValidacionException("No puede revertir una inspección completada.");
                ValidarTexto(registro.Observaciones);
                if (finalizar && registro.Estado != EstadoEjecucionDetalle.Completado)
                    throw new ValidacionException("Complete todas las inspecciones autorizadas antes de finalizar la evaluación. Puede guardar el avance y continuar después.");
                if (registro.Estado != detalle.EstadoEjecucion && a.Pausas.Any(p => p.FechaResolucion == null && (p.IdDetalle == null || p.IdDetalle == detalle.IdPresupuestoDetalle)))
                    throw new ValidacionException("La inspección está pausada. Resuelva su causa antes de continuar.");
                if (detalle.EstadoEjecucion != registro.Estado || detalle.ObservacionesEjecucion != registro.Observaciones?.Trim())
                {
                    detalle.EstadoEjecucion = registro.Estado;
                    detalle.CantidadEjecutada = registro.Estado == EstadoEjecucionDetalle.Completado ? detalle.Cantidad : 0;
                    detalle.IdTecnicoRegistro = registro.Estado == EstadoEjecucionDetalle.NoIniciado ? null : usuario.Id;
                    detalle.FechaRegistroEjecucion = registro.Estado == EstadoEjecucionDetalle.NoIniciado ? null : FechaHoraTaller.Ahora;
                    detalle.ObservacionesEjecucion = registro.Observaciones?.Trim();
                }
            }

            if (avance is null)
            {
                avance = new EvaluacionAtencion
                {
                    IdAtencion = a.IdAtencion
                };
                repositorio.AgregarAvance(avance);
            }

            avance.IncluyeDiagnostico = datos.IncluyeDiagnostico;
            avance.FallaDetectada = (datos.FallaDetectada ?? "").Trim();
            avance.TrabajoRecomendado = (datos.TrabajoRecomendado ?? "").Trim();
            avance.Observaciones = datos.Observaciones?.Trim();
            avance.Kilometraje = datos.Kilometraje;
            avance.SoloEvaluacion = datos.SoloEvaluacion;
            avance.Finalizada = finalizar;
            avance.IdTecnicoUltimaEdicion = usuario.Id;
            avance.FechaActualizacion = FechaHoraTaller.Ahora;
            var propuesta = presupuestos.SingleOrDefault(p => !p.EsAdicional && p.Vigente && p.Estado == EstadoPresupuesto.Borrador);
            if (datos.Items.Count > 0 || propuesta is not null)
            {
                if (propuesta is null)
                {
                    propuesta = new Presupuesto
                    {
                        IdAtencion = a.IdAtencion,

                        Estado = EstadoPresupuesto.Borrador,
                        IdUsuarioCreacion = usuario.Id,
                        FechaCreacion = FechaHoraTaller.Ahora
                    };
                    repositorio.AgregarPresupuesto(propuesta);
                }

                repositorio.ReemplazarDetalles(propuesta, datos.Items.Select(i => new PresupuestoDetalle { IdSolicitudAtencion = i.IdSolicitudAtencion, OrigenMaterial = i.OrigenMaterial, IdConcepto = i.IdConcepto, Descripcion = i.Descripcion.Trim(), Cantidad = i.Cantidad, PrecioUnitario = i.PrecioUnitario }));
                propuesta.Observaciones = avance.Observaciones;
            }

            var anterior = a.Estado;
            if (finalizar)
            {
                var informe = (await repositorio.ObtenerDiagnosticosAsync(a.IdAtencion)).FirstOrDefault();
                if (informe is not null && (!datos.IncluyeDiagnostico || informe.FallaDetectada != avance.FallaDetectada || informe.TrabajoRecomendado != avance.TrabajoRecomendado || informe.Kilometraje != avance.Kilometraje))
                    throw new ValidacionException("El informe inicial existente es histórico. Consérvelo sin modificar; use las observaciones de la propuesta para aclaraciones.");
                if (datos.IncluyeDiagnostico && informe is null)
                {
                    informe = new Diagnostico
                    {
                        IdAtencion = a.IdAtencion,
                        IdTecnicoAutor = usuario.Id,
                        FechaDiagnostico = FechaHoraTaller.Ahora,

                        FallaDetectada = avance.FallaDetectada,
                        TrabajoRecomendado = avance.TrabajoRecomendado,
                        Kilometraje = avance.Kilometraje,
                        Observaciones = avance.Observaciones
                    };
                    repositorio.AgregarDiagnostico(informe);
                }

                if (!datos.SoloEvaluacion && propuesta is not null)
                {
                    propuesta.Diagnostico = informe;
                    propuesta.Estado = EstadoPresupuesto.PendientePresentacion;
                }

                if (datos.SoloEvaluacion && propuesta is not null)
                    repositorio.EliminarBorrador(propuesta);
                a.Estado = datos.SoloEvaluacion ? EstadoAtencion.PendienteLiquidacion : EstadoAtencion.EnEvaluacionTecnica;
                a.IdTecnicoEnActividad = null;
                a.ActividadTecnica = null;
            }

            Evento(a, usuario, finalizar ? "EvaluacionFinalizada" : "EvaluacionAvanceGuardado", anterior, finalizar ? "Evaluación finalizada. La propuesta, si existe, espera presentación; no se autorizó reparar." : "Avance guardado; se conserva la reserva técnica.");
            // El historial y todos los cambios comparten el token de concurrencia de Atención.
            // Forzamos su actualización aun cuando permanece en la misma etapa.
            repositorio.PrepararVersion(a, datos.VersionAtencion);
            await unidad.GuardarCambiosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    private static void ValidarTexto(string? texto)
    {
        if (texto?.Length > 1000)
            throw new ValidacionException("Los textos no pueden superar 1000 caracteres.");
    }

    public async Task<List<PresupuestoConsultaDto>> ListarPresupuestosAsync()
    {
        try
        {
            await AutorizarAsync();
            return await repositorio.ListarPresupuestosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task PresentarAsync(int idPresupuesto, byte[] version)
    {
        try
        {
            var usuario = await AutorizarAsync(recepcion: true);
            var datos = (await repositorio.ListarPresupuestosAsync()).SingleOrDefault(p => p.IdPresupuesto == idPresupuesto) ?? throw new ValidacionException("El presupuesto no existe.");
            var a = await AtencionAsync(datos.IdAtencion);
            var p = (await repositorio.ObtenerPresupuestosAsync(a.IdAtencion)).Single(p => p.IdPresupuesto == idPresupuesto);
            if ((!p.Vigente || p.Estado != EstadoPresupuesto.PendientePresentacion) || version is null || version.Length != 8 || !p.Version.SequenceEqual(version))
                throw new ValidacionException("La propuesta cambió o ya fue presentada. Actualice el listado.");
            if ((!p.EsAdicional && (a.Estado != EstadoAtencion.EnEvaluacionTecnica || a.IdTecnicoEnActividad is not null)) || (p.EsAdicional && a.Estado != (p.EsInspeccion ? EstadoAtencion.EnEvaluacionTecnica : EstadoAtencion.EnEjecucion)))
                throw new ValidacionException("La atención no está disponible para presentar esta propuesta.");
            p.Estado = EstadoPresupuesto.PendienteDecision;
            p.FechaPresentacion = FechaHoraTaller.Ahora;
            var anterior = a.Estado;
            if (!p.EsAdicional)
                a.Estado = EstadoAtencion.PendienteDecision;
            repositorio.PrepararVersion(a, a.Version);
            Evento(a, usuario, "PresupuestoPresentado", anterior, "Recepción registró que comunicó la propuesta al cliente. Decisión pendiente.");
            await unidad.GuardarCambiosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }
}

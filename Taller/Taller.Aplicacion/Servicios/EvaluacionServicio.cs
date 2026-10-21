using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.Servicios;

/// <summary>Coordina recepción, reserva, avances e informe/propuesta sin confundir autorización, ejecución y pago.</summary>
public sealed class EvaluacionServicio(
    IEvaluacionRepositorio repositorio,
    IUsuarioRepositorio usuarios,
    IUnidadDeTrabajo unidad,
    SesionUsuario sesion)
{
    private async Task<Usuario> AutorizarAsync(bool tecnico = false, bool recepcion = false)
    {
        var id = sesion.UsuarioActual?.Id ?? throw new ValidacionException("Debe iniciar sesión.");
        var usuario = await usuarios.ObtenerPorIdAsync(id);
        if (usuario is null || !usuario.Activo
            || (tecnico && usuario.RolId != RolesSistema.TecnicoId)
            || (recepcion && usuario.RolId != RolesSistema.AdministrativoId)
            || (!tecnico && !recepcion && usuario.RolId != RolesSistema.TecnicoId && usuario.RolId != RolesSistema.AdministrativoId))
            throw new ValidacionException("No tiene permisos para esta operación.");
        return usuario;
    }

    private async Task<Atencion> AtencionAsync(int id)
    {
        if (id <= 0) throw new ValidacionException("Seleccione una atención válida.");
        return await repositorio.ObtenerAtencionAsync(id) ?? throw new ValidacionException("La atención no existe.");
    }

    private static void VerificarActividad(Atencion a, Usuario u)
    {
        if (a.Estado != EstadoAtencion.EnEvaluacionTecnica || a.FechaCierre is not null
            || a.IdTecnicoEnActividad != u.Id || a.ActividadTecnica is not (ActividadTecnica.Diagnostico or ActividadTecnica.Presupuesto))
            throw new ValidacionException("La evaluación debe estar reservada por usted. Actualice el listado.");
    }

    private void Evento(Atencion a, Usuario u, string accion, EstadoAtencion anterior, string texto) =>
        repositorio.AgregarHistorial(new HistorialAtencion
        {
            IdAtencion = a.IdAtencion,
            IdUsuario = u.Id,
            Fecha = DateTime.Now,
            Accion = accion,
            EstadoAnterior = anterior,
            EstadoNuevo = a.Estado,
            Observaciones = texto
        });

    public async Task EnviarAEvaluacionAsync(int id, TipoSolicitudAtencion tipo, byte[] version)
    {
        try
        {
            var usuario = await AutorizarAsync(recepcion: true);
            var a = await AtencionAsync(id);
            if (version is null || version.Length != 8 || !version.SequenceEqual(a.Version))
                throw new ConflictoConcurrenciaException("La atención cambió. Vuelva a abrir el detalle antes de enviarla.");
            if (!Enum.IsDefined(tipo) || a.Estado != EstadoAtencion.Abierta || a.IdTecnicoEnActividad is not null)
                throw new ValidacionException("Solo puede enviar una atención abierta con una solicitud válida.");
            a.TipoSolicitud = tipo;
            a.Estado = EstadoAtencion.EnEvaluacionTecnica;
            Evento(a, usuario, "EvaluacionSolicitada", EstadoAtencion.Abierta,
                "Se solicita revisión técnica y preparación de propuesta. No autoriza tareas cobrables ni reparaciones.");
            await unidad.GuardarCambiosAsync();
        }
        finally { unidad.DescartarCambios(); }
    }

    public async Task TomarAsync(int id)
    {
        try
        {
            var usuario = await AutorizarAsync(tecnico: true);
            var a = await AtencionAsync(id);
            if (a.Estado != EstadoAtencion.PendienteDiagnostico && a.Estado != EstadoAtencion.EnEvaluacionTecnica)
                throw new ValidacionException("La atención no está pendiente de evaluación.");
            if ((await repositorio.ObtenerAvanceAsync(id))?.Finalizada == true)
                throw new ValidacionException("La evaluación ya está finalizada.");
            if ((await repositorio.ObtenerPresupuestosAsync(id)).Any(p => p.Tipo == TipoPresupuesto.Reparacion && p.Vigente && p.Estado != EstadoPresupuesto.Borrador))
                throw new ValidacionException("La propuesta ya está preparada. Consúltela en Presupuestos.");
            if (a.IdTecnicoEnActividad is not null) throw new ValidacionException("La actividad ya fue tomada. Actualice el listado.");
            var anterior = a.Estado;
            a.Estado = EstadoAtencion.EnEvaluacionTecnica;
            a.IdTecnicoEnActividad = usuario.Id;
            a.ActividadTecnica = a.TipoSolicitud == TipoSolicitudAtencion.ServicioDefinido ? ActividadTecnica.Presupuesto : ActividadTecnica.Diagnostico;
            Evento(a, usuario, "EvaluacionTomada", anterior, "El técnico tomó la evaluación y preparación de propuesta.");
            await unidad.GuardarCambiosAsync();
        }
        finally { unidad.DescartarCambios(); }
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
        finally { unidad.DescartarCambios(); }
    }

    public async Task<List<EvaluacionBandejaDto>> ListarAsync()
    {
        try { await AutorizarAsync(); return await repositorio.ListarAsync(); }
        finally { unidad.DescartarCambios(); }
    }

    public async Task<List<ConceptoPropuestaDto>> ListarCatalogoAsync()
    {
        try
        {
            await AutorizarAsync(tecnico: true);
            return (await repositorio.ListarCatalogoAsync()).Select(c => new ConceptoPropuestaDto
            {
                IdConcepto = c.IdConcepto,
                Nombre = c.Nombre,
                Categoria = c.Categoria.Nombre,
                Descripcion = c.Descripcion,
                Tipo = c.Tipo,
                PrecioReferencia = c.PrecioReferencia
            }).ToList();
        }
        finally { unidad.DescartarCambios(); }
    }

    public async Task<EvaluacionAtencionDto> ObtenerAsync(int id)
    {
        try
        {
            var usuario = await AutorizarAsync(tecnico: true);
            var a = await AtencionAsync(id);
            VerificarActividad(a, usuario);
            var avance = await repositorio.ObtenerAvanceAsync(id);
            if (avance?.Finalizada == true) throw new ValidacionException("La evaluación ya se finalizó.");
            var presupuestos = await repositorio.ObtenerPresupuestosAsync(id);
            var informe = (await repositorio.ObtenerDiagnosticosAsync(id)).FirstOrDefault(d => d.Tipo == TipoDiagnostico.Inicial);
            var borrador = presupuestos.SingleOrDefault(p => p.Tipo == TipoPresupuesto.Reparacion && p.Vigente && p.Estado == EstadoPresupuesto.Borrador);
            return new EvaluacionAtencionDto
            {
                IdAtencion = id,
                VersionAtencion = a.Version.ToArray(),
                TipoSolicitud = a.TipoSolicitud,
                Cliente = a.Cliente.Apellido + ", " + a.Cliente.Nombre,
                Vehiculo = a.Vehiculo.Dominio + " · " + a.Vehiculo.Modelo.Marca.Nombre + " " + a.Vehiculo.Modelo.Nombre,
                Motivo = a.MotivoConsulta,
                MotivoRevision = borrador?.MotivoRevision,
                InformeHistorico = informe != null,
                IncluyeDiagnostico = avance?.IncluyeDiagnostico ?? (informe != null || a.TipoSolicitud != TipoSolicitudAtencion.ServicioDefinido),
                FallaDetectada = avance?.FallaDetectada ?? informe?.FallaDetectada ?? "",
                TrabajoRecomendado = avance?.TrabajoRecomendado ?? informe?.TrabajoRecomendado ?? "",
                Kilometraje = avance?.Kilometraje ?? informe?.Kilometraje,
                Observaciones = avance?.Observaciones ?? informe?.Observaciones,
                SoloEvaluacion = avance?.SoloEvaluacion ?? false,
                Items = borrador?.Detalles.Select(d => new ItemPropuestaDto { IdConcepto = d.IdConcepto, Descripcion = d.Descripcion, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario }).ToList() ?? [],
                ServiciosIniciales = presupuestos.Where(p => p.Tipo == TipoPresupuesto.Diagnostico && p.Estado == EstadoPresupuesto.Aceptado)
                    .SelectMany(p => p.Detalles).Select(d => new ServicioEvaluadoDto
                    {
                        IdDetalle = d.IdPresupuestoDetalle,
                        Descripcion = d.Descripcion,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Estado = d.EstadoEjecucion,
                        Observaciones = d.ObservacionesEjecucion
                    }).ToList()
            };
        }
        finally { unidad.DescartarCambios(); }
    }

    /// <summary>Guarda un avance o finaliza el informe y la propuesta en una única transacción.</summary>
    public async Task GuardarAsync(EvaluacionAtencionDto datos, bool finalizar)
    {
        ArgumentNullException.ThrowIfNull(datos);
        if (datos.Items is null || datos.ServiciosIniciales is null || datos.Items.Any(i => i is null) || datos.ServiciosIniciales.Any(i => i is null))
            throw new ValidacionException("La selección de ítems o servicios es inválida.");
        try
        {
            var usuario = await AutorizarAsync(tecnico: true);
            var a = await AtencionAsync(datos.IdAtencion);
            VerificarActividad(a, usuario);
            if (datos.VersionAtencion is null || datos.VersionAtencion.Length != 8 || !datos.VersionAtencion.SequenceEqual(a.Version))
                throw new ConflictoConcurrenciaException("La atención cambió. Cierre y vuelva a abrir la evaluación.");
            var avance = await repositorio.ObtenerAvanceAsync(a.IdAtencion);
            if (avance?.Finalizada == true) throw new ValidacionException("La evaluación ya está finalizada.");
            ValidarTexto(datos.FallaDetectada); ValidarTexto(datos.TrabajoRecomendado); ValidarTexto(datos.Observaciones);
            if (datos.Kilometraje is < 0) throw new ValidacionException("El kilometraje no puede ser negativo.");
            if (finalizar && a.TipoSolicitud != TipoSolicitudAtencion.ServicioDefinido && !datos.IncluyeDiagnostico)
                throw new ValidacionException("Una solicitud por problema requiere registrar el resultado de la evaluación.");
            if (finalizar && datos.IncluyeDiagnostico && (string.IsNullOrWhiteSpace(datos.FallaDetectada) || string.IsNullOrWhiteSpace(datos.TrabajoRecomendado)))
                throw new ValidacionException("Complete el hallazgo/conclusión y el trabajo recomendado.");
            if (datos.SoloEvaluacion && (string.IsNullOrWhiteSpace(datos.Observaciones) || datos.Items.Count > 0))
                throw new ValidacionException("Para finalizar sin propuesta, quite los ítems y explique el motivo en observaciones.");
            if (finalizar && !datos.SoloEvaluacion && datos.Items.Count == 0)
                throw new ValidacionException("Agregue la propuesta o indique que finaliza sin propuesta, con su motivo.");
            var presupuestos = await repositorio.ObtenerPresupuestosAsync(a.IdAtencion);
            if (presupuestos.Any(p => p.Tipo == TipoPresupuesto.Reparacion && p.Vigente && p.Estado != EstadoPresupuesto.Borrador))
                throw new ValidacionException("Ya existe una propuesta confirmada. Su revisión requiere un nuevo circuito.");
            var catalogo = (await repositorio.ListarCatalogoAsync()).ToDictionary(c => c.IdConcepto);
            if (datos.Items.GroupBy(i => i.IdConcepto).Any(g => g.Count() > 1)) throw new ValidacionException("No repita conceptos en la propuesta.");
            decimal total = 0;
            const decimal maximo = 9999999999999999.99m;
            foreach (var item in datos.Items)
            {
                if (!catalogo.ContainsKey(item.IdConcepto)) throw new ValidacionException("Un concepto ya no está activo. Actualice el catálogo.");
                if (string.IsNullOrWhiteSpace(item.Descripcion) || item.Descripcion.Trim().Length > 500)
                    throw new ValidacionException("Cada ítem requiere una descripción de hasta 500 caracteres.");
                if (item.Cantidad <= 0 || item.Cantidad > 999999999999999.999m || decimal.Round(item.Cantidad, 3) != item.Cantidad
                    || item.PrecioUnitario < 0 || item.PrecioUnitario > maximo || decimal.Round(item.PrecioUnitario, 2) != item.PrecioUnitario
                    || item.PrecioUnitario > maximo / item.Cantidad)
                    throw new ValidacionException("Revise cantidades y precios: hasta tres y dos decimales respectivamente.");
                if (item.Subtotal > maximo - total) throw new ValidacionException("El total supera el importe permitido.");
                total += item.Subtotal;
            }
            var iniciales = presupuestos.Where(p => p.Tipo == TipoPresupuesto.Diagnostico).SelectMany(p => p.Detalles).ToList();
            if (datos.ServiciosIniciales.Select(d => d.IdDetalle).Distinct().Count() != iniciales.Count
                || datos.ServiciosIniciales.Count != iniciales.Count)
                throw new ValidacionException("Los servicios iniciales cambiaron. Vuelva a abrir la evaluación.");
            foreach (var registro in datos.ServiciosIniciales)
            {
                var detalle = iniciales.SingleOrDefault(d => d.IdPresupuestoDetalle == registro.IdDetalle)
                    ?? throw new ValidacionException("Servicio inicial inválido.");
                if (registro.Estado != EstadoEjecucionDetalle.NoIniciado && registro.Estado != EstadoEjecucionDetalle.Completado && registro.Estado != EstadoEjecucionDetalle.Omitido)
                    throw new ValidacionException("Indique pendiente, completado u omitido para los servicios iniciales.");
                if (detalle.EstadoEjecucion != EstadoEjecucionDetalle.NoIniciado && (detalle.EstadoEjecucion != registro.Estado || detalle.ObservacionesEjecucion != registro.Observaciones?.Trim()))
                    throw new ValidacionException("No puede revertir una ejecución registrada desde esta pantalla.");
                ValidarTexto(registro.Observaciones);
                if (registro.Estado == EstadoEjecucionDetalle.Omitido && string.IsNullOrWhiteSpace(registro.Observaciones))
                    throw new ValidacionException("Justifique cada servicio omitido.");
                if (finalizar && registro.Estado == EstadoEjecucionDetalle.NoIniciado)
                    throw new ValidacionException("Resuelva los servicios iniciales: completados u omitidos con justificación.");
                if (detalle.EstadoEjecucion != registro.Estado || detalle.ObservacionesEjecucion != registro.Observaciones?.Trim())
                {
                    detalle.EstadoEjecucion = registro.Estado;
                    detalle.CantidadEjecutada = registro.Estado == EstadoEjecucionDetalle.Completado ? detalle.Cantidad : 0;
                    detalle.IdTecnicoRegistro = registro.Estado == EstadoEjecucionDetalle.NoIniciado ? null : usuario.Id;
                    detalle.FechaRegistroEjecucion = registro.Estado == EstadoEjecucionDetalle.NoIniciado ? null : DateTime.Now;
                    detalle.ObservacionesEjecucion = registro.Observaciones?.Trim();
                }
            }
            if (avance is null)
            {
                avance = new EvaluacionAtencion { IdAtencion = a.IdAtencion };
                repositorio.AgregarAvance(avance);
            }
            avance.IncluyeDiagnostico = datos.IncluyeDiagnostico;
            avance.FallaDetectada = (datos.FallaDetectada ?? "").Trim(); avance.TrabajoRecomendado = (datos.TrabajoRecomendado ?? "").Trim();
            avance.Observaciones = datos.Observaciones?.Trim(); avance.Kilometraje = datos.Kilometraje;
            avance.SoloEvaluacion = datos.SoloEvaluacion; avance.Finalizada = finalizar;
            avance.IdTecnicoUltimaEdicion = usuario.Id; avance.FechaActualizacion = DateTime.Now;
            var propuesta = presupuestos.SingleOrDefault(p => p.Tipo == TipoPresupuesto.Reparacion && p.Vigente && p.Estado == EstadoPresupuesto.Borrador);
            if (datos.Items.Count > 0 || propuesta is not null)
            {
                if (propuesta is null)
                {
                    propuesta = new Presupuesto
                    {
                        IdAtencion = a.IdAtencion,
                        Tipo = TipoPresupuesto.Reparacion,
                        Estado = EstadoPresupuesto.Borrador,
                        IdUsuarioCreacion = usuario.Id,
                        FechaCreacion = DateTime.Now
                    };
                    repositorio.AgregarPresupuesto(propuesta);
                }
                repositorio.ReemplazarDetalles(propuesta, datos.Items.Select(i => new PresupuestoDetalle
                {
                    IdConcepto = i.IdConcepto,
                    Descripcion = i.Descripcion.Trim(),
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario
                }));
                propuesta.Observaciones = avance.Observaciones;
            }
            var anterior = a.Estado;
            if (finalizar)
            {
                var informe = (await repositorio.ObtenerDiagnosticosAsync(a.IdAtencion)).FirstOrDefault(d => d.Tipo == TipoDiagnostico.Inicial);
                if (informe is not null && (!datos.IncluyeDiagnostico || informe.FallaDetectada != avance.FallaDetectada || informe.TrabajoRecomendado != avance.TrabajoRecomendado
                    || informe.Kilometraje != avance.Kilometraje))
                    throw new ValidacionException("El informe inicial existente es histórico. Consérvelo sin modificar; use las observaciones de la propuesta para aclaraciones.");
                if (datos.IncluyeDiagnostico && informe is null)
                {
                    informe = new Diagnostico
                    {
                        IdAtencion = a.IdAtencion,
                        IdTecnicoAutor = usuario.Id,
                        FechaDiagnostico = DateTime.Now,
                        Tipo = TipoDiagnostico.Inicial,
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
                a.IdTecnicoEnActividad = null; a.ActividadTecnica = null;
            }
            Evento(a, usuario, finalizar ? "EvaluacionFinalizada" : "EvaluacionAvanceGuardado", anterior,
                finalizar ? "Evaluación finalizada. La propuesta, si existe, espera presentación; no se autorizó reparar." : "Avance guardado; se conserva la reserva técnica.");
            // El historial y todos los cambios comparten el token de concurrencia de Atención.
            // Forzamos su actualización aun cuando permanece en la misma etapa.
            repositorio.PrepararVersion(a, datos.VersionAtencion);
            await unidad.GuardarCambiosAsync();
        }
        finally { unidad.DescartarCambios(); }
    }

    private static void ValidarTexto(string? texto)
    {
        if (texto?.Length > 1000) throw new ValidacionException("Los textos no pueden superar 1000 caracteres.");
    }

    public async Task<List<PresupuestoConsultaDto>> ListarPresupuestosAsync()
    {
        try { await AutorizarAsync(); return await repositorio.ListarPresupuestosAsync(); }
        finally { unidad.DescartarCambios(); }
    }

    public async Task PresentarAsync(int idPresupuesto, byte[] version)
    {
        try
        {
            var usuario = await AutorizarAsync(recepcion: true);
            var datos = (await repositorio.ListarPresupuestosAsync()).SingleOrDefault(p => p.IdPresupuesto == idPresupuesto)
                ?? throw new ValidacionException("El presupuesto no existe.");
            var a = await AtencionAsync(datos.IdAtencion);
            var p = (await repositorio.ObtenerPresupuestosAsync(a.IdAtencion)).Single(p => p.IdPresupuesto == idPresupuesto);
            if ((!p.Vigente || p.Estado != EstadoPresupuesto.PendientePresentacion) || p.Tipo != TipoPresupuesto.Reparacion || version is null || version.Length != 8 || !p.Version.SequenceEqual(version))
                throw new ValidacionException("La propuesta cambió o ya fue presentada. Actualice el listado.");
            if (a.Estado != EstadoAtencion.EnEvaluacionTecnica || a.IdTecnicoEnActividad is not null)
                throw new ValidacionException("La atención no está disponible para presentar esta propuesta.");
            p.Estado = EstadoPresupuesto.PendienteDecision; p.FechaPresentacion = DateTime.Now;
            var anterior = a.Estado; a.Estado = EstadoAtencion.PendienteDecision;
            Evento(a, usuario, "PresupuestoPresentado", anterior, "Recepción registró que comunicó la propuesta al cliente. Decisión pendiente.");
            await unidad.GuardarCambiosAsync();
        }
        finally { unidad.DescartarCambios(); }
    }
}

using Taller.Dominio.Tiempo;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.Servicios;
/// <summary>Registra evidencias y ampliaciones sin reescribir autorizaciones previas. Cada mutación usa la versión de Atención.</summary>
public sealed class SeguimientoTrabajoServicio(IEvaluacionRepositorio repositorio, ICircuitoRepositorio circuito, IUnidadDeTrabajo unidad, AutorizacionOperativa acceso)
{
    private async Task<Atencion> CargarAsync(int id)
    {
        var a = await repositorio.ObtenerAtencionAsync(id) ?? throw new ValidacionException("La atención no existe.");
        return a;
    }

    private async Task<Atencion> CargarAsync(int id, byte[] version)
    {
        var a = await CargarAsync(id);
        AutorizacionOperativa.Version(version, a.Version);
        return a;
    }

    private async Task ExigirAbiertaAsync(Atencion a)
    {
        if (a.FechaCierre != null || a.Estado is not (EstadoAtencion.EnEvaluacionTecnica or EstadoAtencion.PendienteDiagnostico or EstadoAtencion.PendienteDecision or EstadoAtencion.EnEjecucion) || await circuito.ObtenerPorAtencionAsync(a.IdAtencion) != null)
            throw new ValidacionException("La atención debe estar en proceso y sin liquidación. Actualice el expediente.");
    }

    private static void ExigirTecnicoDisponible(Atencion a, Usuario u)
    {
        if (a.IdTecnicoEnActividad != null && a.IdTecnicoEnActividad != u.Id)
            throw new ValidacionException("Otro técnico tiene reservada la actividad. Debe liberarla antes de esta intervención.");
    }

    private async Task GuardarAsync(Atencion a, Usuario u, string accion, string nota)
    {
        repositorio.PrepararVersion(a, a.Version);
        repositorio.AgregarHistorial(new HistorialAtencion { IdAtencion = a.IdAtencion, IdUsuario = u.Id, Fecha = FechaHoraTaller.Ahora, Accion = accion, EstadoAnterior = a.Estado, EstadoNuevo = a.Estado, Observaciones = nota.Length > 2000 ? nota[..2000] : nota });
        await unidad.GuardarCambiosAsync();
    }

    public async Task<SeguimientoTrabajoDto> ObtenerAsync(int id)
    {
        try
        {
            var u = await acceso.ExigirAsync();
            var a = await CargarAsync(id);
            var ps = await repositorio.ObtenerPresupuestosAsync(id);
            var informes = await repositorio.ObtenerDiagnosticosAsync(id);
            var operable = a.FechaCierre == null && (a.Estado is EstadoAtencion.EnEvaluacionTecnica or EstadoAtencion.PendienteDiagnostico or EstadoAtencion.PendienteDecision or EstadoAtencion.EnEjecucion) && await circuito.ObtenerPorAtencionAsync(id) == null;
            return new()
            {
                IdAtencion = id,
                Version = a.Version.ToArray(),
                Estado = a.Estado,
                Progreso = ps.Where(p => !p.EsInspeccion && p.Tipo == TipoPresupuesto.Reparacion && p.Estado == EstadoPresupuesto.Aceptado)
                    .SelectMany(p => p.Detalles.Where(d => d.AceptadoPorCliente == true && (p.Vigente || d.CantidadEjecutada > 0)))
                    .OrderBy(d => d.IdPresupuestoDetalle).Select(d => new EjecucionItemDto {
                        IdDetalle = d.IdPresupuestoDetalle, Descripcion = d.Descripcion, CantidadAutorizada = d.Cantidad,
                        CantidadEjecutada = d.CantidadEjecutada, Estado = d.EstadoEjecucion, Observaciones = d.ObservacionesEjecucion }).ToList(),
                PuedePausar = operable,
                InformeInicial = string.Join("\n\n", informes.Select(d => $"Informe {d.IdDiagnostico} ({d.Tipo}) · {d.FechaDiagnostico:dd/MM/yyyy HH:mm} · Técnico {d.IdTecnicoAutor}\nConclusión: {d.FallaDetectada}\nRecomendación: {d.TrabajoRecomendado}\n{d.Observaciones}")),
                TrabajosRegistrados = string.Join("\n\n", ps.Where(p => p.Estado == EstadoPresupuesto.Aceptado).SelectMany(p => p.Detalles.Where(d => d.AceptadoPorCliente == true && (p.Vigente || d.CantidadEjecutada > 0))).Select(d => $"P{d.IdPresupuesto} / Ítem {d.IdPresupuestoDetalle}: {d.Descripcion} · Aporta {d.OrigenMaterial}\nAutorizado {d.Cantidad:N3}; realizado {d.CantidadEjecutada:N3}; estado {new EjecucionItemDto { Estado = d.EstadoEjecucion }.EstadoDescripcion}; técnico {d.IdTecnicoRegistro}; registro {d.FechaRegistroEjecucion:dd/MM/yyyy HH:mm}\n{d.ObservacionesEjecucion}")),
                Resumen = $"Atención {id:D6} · {a.Vehiculo.Dominio} · {a.Cliente.Apellido}, {a.Cliente.Nombre}",
                PuedeRegistrar = u.RolId == RolesSistema.TecnicoId && a.FechaCierre == null && (a.Estado is EstadoAtencion.EnEvaluacionTecnica or EstadoAtencion.PendienteDiagnostico or EstadoAtencion.PendienteDecision or EstadoAtencion.EnEjecucion) && (a.IdTecnicoEnActividad == null || a.IdTecnicoEnActividad == u.Id) && await circuito.ObtenerPorAtencionAsync(id) == null,
                Hallazgos = a.Hallazgos.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.IdHallazgoAtencion).Select(x => new HallazgoConsultaDto { Id = x.IdHallazgoAtencion, IdTecnico = x.IdTecnico, Fecha = x.Fecha, Descripcion = x.Descripcion, Recomendacion = x.Recomendacion, ImpideContinuar = x.ImpideContinuar, IdDetalle = x.IdDetalleAfectado }).ToList(),
                Pausas = a.Pausas.OrderBy(x => x.FechaResolucion != null).ThenByDescending(x => x.Fecha).Select(x => new PausaConsultaDto { Id = x.IdPausaTrabajo, IdUsuario = x.IdUsuario, IdTecnicoResolucion = x.IdTecnicoResolucion, IdDetalle = x.IdDetalle, Motivo = x.Motivo, Fecha = x.Fecha, FechaResolucion = x.FechaResolucion, Resolucion = x.Resolucion }).ToList(),
                Items = ps.Where(p => p.Vigente && p.Estado == EstadoPresupuesto.Aceptado).SelectMany(p => p.Detalles.Where(d => d.AceptadoPorCliente == true)).Select(d => new MaterialTrabajoDto { IdDetalle = d.IdPresupuestoDetalle, IdPresupuesto = d.IdPresupuesto, Descripcion = d.Descripcion, Origen = d.OrigenMaterial, Cantidad = d.Cantidad, Recibida = d.CantidadRecibidaCliente, Conforme = d.MaterialClienteConforme, Observaciones = d.ObservacionesMaterial }).ToList()
            };
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    private async Task<PresupuestoDetalle> DetalleAsync(int id, int detalle) => (await repositorio.ObtenerPresupuestosAsync(id)).Where(p => p.Vigente && p.Estado == EstadoPresupuesto.Aceptado).SelectMany(p => p.Detalles).SingleOrDefault(d => d.IdPresupuestoDetalle == detalle && d.AceptadoPorCliente == true) ?? throw new ValidacionException("Seleccione un ítem autorizado de esta atención.");
    public async Task RegistrarHallazgoAsync(RegistrarHallazgoDto datos)
    {
        ArgumentNullException.ThrowIfNull(datos);
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.TecnicoId);
            var a = await CargarAsync(datos.IdAtencion, datos.Version);
            await ExigirAbiertaAsync(a);
            ExigirTecnicoDisponible(a, u);
            var texto = AutorizacionOperativa.Texto(datos.Descripcion);
            var recomendacion = AutorizacionOperativa.Texto(datos.Recomendacion);
            if (datos.IdDetalle is int id)
                await DetalleAsync(a.IdAtencion, id);
            a.Hallazgos.Add(new HallazgoAtencion { IdAtencion = a.IdAtencion, IdTecnico = u.Id, Fecha = FechaHoraTaller.Ahora, Descripcion = texto, Recomendacion = recomendacion, ImpideContinuar = datos.ImpideContinuar, IdDetalleAfectado = datos.IdDetalle });
            if (datos.ImpideContinuar)
                a.Pausas.Add(new PausaTrabajo { IdAtencion = a.IdAtencion, IdDetalle = datos.IdDetalle, IdUsuario = u.Id, Fecha = FechaHoraTaller.Ahora, Motivo = ("Hallazgo que impide continuar: " + texto).Substring(0, Math.Min(1000, ("Hallazgo que impide continuar: " + texto).Length)) });
            await GuardarAsync(a, u, "HallazgoRegistrado", $"{texto}. Recomendación: {recomendacion}. Bloquea: {datos.ImpideContinuar}.");
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task PausarAsync(int id, byte[] version, int? idDetalle, string motivo)
    {
        try
        {
            var u = await acceso.ExigirAsync();
            var a = await CargarAsync(id, version);
            await ExigirAbiertaAsync(a);
            if (u.RolId == RolesSistema.TecnicoId)
                ExigirTecnicoDisponible(a, u);
            if (u.RolId == RolesSistema.AdministrativoId && idDetalle.HasValue)
                throw new ValidacionException("Recepción registra una detención general del cliente; las pausas por tarea corresponden al técnico.");
            motivo = AutorizacionOperativa.Texto(motivo);
            if (u.RolId == RolesSistema.AdministrativoId) motivo = "Detención solicitada por el cliente: " + motivo;
            if (motivo.Length > 1000) throw new ValidacionException("Reduzca el motivo de la detención a 950 caracteres.");
            if (idDetalle is int d)
                await DetalleAsync(id, d);
            if (a.Pausas.Any(p => p.FechaResolucion == null && p.IdDetalle == idDetalle))
                throw new ValidacionException("Ya existe una pausa activa para ese alcance.");
            a.Pausas.Add(new PausaTrabajo { IdAtencion = id, IdDetalle = idDetalle, IdUsuario = u.Id, Fecha = FechaHoraTaller.Ahora, Motivo = motivo });
            await GuardarAsync(a, u, "TrabajoPausado", motivo);
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task ResolverPausaAsync(int id, byte[] version, int pausa, string resolucion)
    {
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.TecnicoId);
            var a = await CargarAsync(id, version);
            await ExigirAbiertaAsync(a);
            ExigirTecnicoDisponible(a, u);
            var p = a.Pausas.SingleOrDefault(x => x.IdPausaTrabajo == pausa && x.FechaResolucion == null) ?? throw new ValidacionException("La pausa ya fue resuelta o no pertenece a esta atención.");
            p.Resolucion = AutorizacionOperativa.Texto(resolucion);
            p.IdTecnicoResolucion = u.Id;
            p.FechaResolucion = FechaHoraTaller.Ahora;
            await GuardarAsync(a, u, "PausaResuelta", p.Resolucion);
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task RecibirMaterialAsync(int id, byte[] version, int detalle, decimal cantidad, bool conforme, string nota)
    {
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.TecnicoId);
            var a = await CargarAsync(id, version);
            await ExigirAbiertaAsync(a);
            ExigirTecnicoDisponible(a, u);
            var d = await DetalleAsync(id, detalle);
            nota = AutorizacionOperativa.Texto(nota);
            if (d.OrigenMaterial != OrigenMaterial.Cliente || cantidad < d.CantidadRecibidaCliente || cantidad > d.Cantidad || cantidad < 0 || decimal.Round(cantidad, 3) != cantidad || conforme && cantidad == 0 || !conforme && d.CantidadEjecutada > 0)
                throw new ValidacionException("Revise el material: recepción acumulada sin disminuir, hasta la cantidad acordada; no puede invalidar material ya utilizado.");
            d.CantidadRecibidaCliente = cantidad;
            d.MaterialClienteConforme = conforme;
            d.ObservacionesMaterial = nota;
            d.IdTecnicoMaterial = u.Id;
            d.FechaRecepcionMaterial = FechaHoraTaller.Ahora;
            await GuardarAsync(a, u, "MaterialClienteRevisado", $"Ítem {detalle}: recibidos {cantidad:N3}; conformidad técnica {conforme}. {nota}");
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    internal static List<PresupuestoDetalle> PrepararItems(List<ItemPropuestaDto> items, List<Concepto> catalogo)
    {
        if (items == null || items.Count == 0 || items.Any(i => i == null) || items.Select(i => (i.IdConcepto, i.IdSolicitudAtencion)).Distinct().Count() != items.Count)
            throw new ValidacionException("Agregue conceptos distintos a la propuesta.");
        decimal total = 0;
        const decimal max = 9999999999999999.99m;
        return items.Select(i =>
        {
            var c = catalogo.SingleOrDefault(c => c.IdConcepto == i.IdConcepto) ?? throw new ValidacionException("El concepto ya no está disponible.");
            if (!Enum.IsDefined(i.OrigenMaterial) || i.OrigenMaterial == OrigenMaterial.Cliente && (c.Tipo == TipoConcepto.Servicio || i.PrecioUnitario != 0))
                throw new ValidacionException("Solo los repuestos y consumibles pueden ser aportados por el cliente, siempre sin precio de venta del taller.");
            if (i.Cantidad <= 0 || i.Cantidad > 999999999999999.999m || decimal.Round(i.Cantidad, 3) != i.Cantidad || i.PrecioUnitario < 0 || i.PrecioUnitario > max || decimal.Round(i.PrecioUnitario, 2) != i.PrecioUnitario || i.PrecioUnitario > max / i.Cantidad || i.Subtotal > max - total)
                throw new ValidacionException("Cantidades o importes inválidos.");
            var descripcion = AutorizacionOperativa.Texto(i.Descripcion);
            if (descripcion.Length > 500)
                throw new ValidacionException("La descripción admite hasta 500 caracteres.");
            total += i.Subtotal;
            return new PresupuestoDetalle
            {
                IdSolicitudAtencion = i.IdSolicitudAtencion,
                IdConcepto = i.IdConcepto,
                Descripcion = descripcion,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario,
                OrigenMaterial = i.OrigenMaterial
            };
        }).ToList();
    }

    public async Task<int> CrearAdicionalAsync(PropuestaAdicionalDto datos)
    {
        ArgumentNullException.ThrowIfNull(datos);
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.TecnicoId);
            var a = await CargarAsync(datos.IdAtencion, datos.Version);
            await ExigirAbiertaAsync(a);
            ExigirTecnicoDisponible(a, u);
            if (a.Estado is not (EstadoAtencion.EnEjecucion or EstadoAtencion.EnEvaluacionTecnica))
                throw new ValidacionException("Los adicionales se preparan durante la ejecución. Para cambios previos solicite una revisión.");
            if (datos.Items is null || datos.Items.Any(i => i is null || i.IdSolicitudAtencion.HasValue && !a.Solicitudes.Any(s => s.IdSolicitudAtencion == i.IdSolicitudAtencion)))
                throw new ValidacionException("Revise las solicitudes asociadas a los ítems del adicional.");
            var motivo = AutorizacionOperativa.Texto(datos.Motivo);
            var h = datos.IdHallazgo is int hi ? a.Hallazgos.SingleOrDefault(h => h.IdHallazgoAtencion == hi) ?? throw new ValidacionException("El hallazgo no pertenece a la atención.") : null;
            var p = new Presupuesto
            {
                IdAtencion = a.IdAtencion,
                EsAdicional = true,
                EsInspeccion = a.Estado == EstadoAtencion.EnEvaluacionTecnica,
                IdHallazgoAtencion = h?.IdHallazgoAtencion,
                Estado = EstadoPresupuesto.PendientePresentacion,
                Tipo = TipoPresupuesto.Reparacion,
                FechaCreacion = FechaHoraTaller.Ahora,
                IdUsuarioCreacion = u.Id,
                Observaciones = motivo,
                Detalles = PrepararItems(datos.Items, await repositorio.ListarCatalogoAsync())
            };
            var catalogo = await repositorio.ListarCatalogoAsync();
            if (p.EsInspeccion && p.Detalles.Any(d => !catalogo.Any(c => c.IdConcepto == d.IdConcepto && c.Tipo == TipoConcepto.Servicio)))
                throw new ValidacionException("La ampliación de inspección admite servicios. Los materiales para reparación se incluyen en la propuesta de trabajo.");
            repositorio.AgregarPresupuesto(p);
            await GuardarAsync(a, u, "PresupuestoAdicionalPreparado", motivo);
            return p.IdPresupuesto;
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task<int> RevisarSeleccionAsync(int id, byte[] version, List<ItemPropuestaDto> items, string justificacion)
    {
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.TecnicoId);
            var p = await circuito.ObtenerPresupuestoAsync(id) ?? throw new ValidacionException("El presupuesto no existe.");
            AutorizacionOperativa.Version(version, p.Version);
            var a = await CargarAsync(p.IdAtencion);
            await ExigirAbiertaAsync(a);
            ExigirTecnicoDisponible(a, u);
            if (!p.Vigente || p.Estado != EstadoPresupuesto.PendienteRevisionTecnica)
                throw new ValidacionException("La selección ya no está pendiente de revisión.");
            if (items is null || items.Any(i => i is null || i.IdSolicitudAtencion.HasValue && !a.Solicitudes.Any(s => s.IdSolicitudAtencion == i.IdSolicitudAtencion)))
                throw new ValidacionException("Revise las solicitudes asociadas a los ítems de la revisión.");
            justificacion = AutorizacionOperativa.Texto(justificacion);
            p.Vigente = false;
            var nueva = new Presupuesto
            {
                IdAtencion = p.IdAtencion,
                IdDiagnostico = p.IdDiagnostico,
                EsAdicional = p.EsAdicional,
                EsInspeccion = p.EsInspeccion,
                IdHallazgoAtencion = p.IdHallazgoAtencion,
                IdPresupuestoAnterior = id,
                Tipo = TipoPresupuesto.Reparacion,
                Estado = EstadoPresupuesto.PendientePresentacion,
                FechaCreacion = FechaHoraTaller.Ahora,
                IdUsuarioCreacion = u.Id,
                MotivoRevision = justificacion,
                Observaciones = justificacion,
                AlcanceValidado = true,
                Detalles = PrepararItems(items, await repositorio.ListarCatalogoAsync())
            };
            var catalogo = await repositorio.ListarCatalogoAsync();
            if (nueva.EsInspeccion && nueva.Detalles.Any(d => !catalogo.Any(c => c.IdConcepto == d.IdConcepto && c.Tipo == TipoConcepto.Servicio)))
                throw new ValidacionException("La ampliación de inspección admite servicios.");
            repositorio.AgregarPresupuesto(nueva);
            if (!p.EsAdicional)
                a.Estado = EstadoAtencion.EnEvaluacionTecnica;
            await GuardarAsync(a, u, "SeleccionParcialRevisada", $"Se conserva propuesta {id}; nueva propuesta viable pendiente de comunicar y aceptar. {justificacion}");
            return nueva.IdPresupuesto;
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }
}

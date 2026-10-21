using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.Servicios;
/// <summary>Reserva y registra únicamente trabajos autorizados; no cambia precios ni confirma cobros.</summary>
public sealed class EjecucionServicio(IEvaluacionRepositorio repositorio, ICircuitoRepositorio circuito,
    IUnidadDeTrabajo unidad, AutorizacionOperativa acceso)
{
    private async Task<Atencion> AtencionAsync(int id) => await repositorio.ObtenerAtencionAsync(id) ?? throw new ValidacionException("La atención no existe.");
    private async Task<Presupuesto> PropuestaAsync(int id) => (await repositorio.ObtenerPresupuestosAsync(id))
        .SingleOrDefault(p => p.Vigente && p.Tipo == TipoPresupuesto.Reparacion && p.Estado == EstadoPresupuesto.Aceptado)
        ?? throw new ValidacionException("La atención no tiene una propuesta vigente aceptada.");
    private void Evento(Atencion a, Usuario u, string accion, EstadoAtencion anterior, string motivo) => repositorio.AgregarHistorial(new HistorialAtencion
    {
        IdAtencion = a.IdAtencion,
        IdUsuario = u.Id,
        Fecha = DateTime.Now,
        Accion = accion,
        EstadoAnterior = anterior,
        EstadoNuevo = a.Estado,
        Observaciones = motivo
    });
    public async Task<EjecucionAtencionDto> ObtenerAsync(int id)
    {
        try
        {
            await acceso.ExigirAsync(RolesSistema.TecnicoId); var a = await AtencionAsync(id); var p = await PropuestaAsync(id);
            return new EjecucionAtencionDto
            {
                IdAtencion = id,
                Version = a.Version.ToArray(),
                IdTecnico = a.IdTecnicoEnActividad,
                Estado = a.Estado,
                TieneExclusiones = p.Detalles.Any(d => d.AceptadoPorCliente != true),
                Resumen = $"Atención {id:D6} · {a.Cliente.Apellido}, {a.Cliente.Nombre} · {a.Vehiculo.Dominio}\nPresupuesto {p.IdPresupuesto:D6}. Motivo: {a.MotivoConsulta}"
                    + $"\nDecisión comunicada: {p.ObservacionesDecision}"
                    + (p.Detalles.Any(d => d.AceptadoPorCliente != true)
                        ? "\nÍtems EXCLUIDOS por el cliente: " + string.Join("; ",p.Detalles.Where(d => d.AceptadoPorCliente != true).Select(d => d.Descripcion)) : ""),
                Items = p.Detalles.Where(d => d.AceptadoPorCliente == true).OrderBy(d => d.IdPresupuestoDetalle).Select(d => new EjecucionItemDto
                {
                    IdDetalle = d.IdPresupuestoDetalle,
                    Descripcion = d.Descripcion,
                    CantidadAutorizada = d.Cantidad,
                    CantidadEjecutada = d.CantidadEjecutada,
                    Estado = d.EstadoEjecucion,
                    Observaciones = d.ObservacionesEjecucion
                }).ToList()
            };
        }
        finally { unidad.DescartarCambios(); }
    }
    public async Task TomarAsync(int id, byte[] version, string observaciones)
    {
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.TecnicoId); var a = await AtencionAsync(id); AutorizacionOperativa.Version(version, a.Version);
            var p = await PropuestaAsync(id);
            if (a.Estado != EstadoAtencion.EnEjecucion || a.IdTecnicoEnActividad != null || await circuito.ObtenerPorAtencionAsync(id) != null)
                throw new ValidacionException("La ejecución no está disponible para tomar.");
            var motivo = AutorizacionOperativa.Texto(observaciones, p.Detalles.Any(d => d.AceptadoPorCliente != true));
            a.IdTecnicoEnActividad = u.Id; a.ActividadTecnica = ActividadTecnica.Ejecucion; p.AlcanceValidado = true;
            repositorio.PrepararVersion(a, version);
            Evento(a, u, "EjecucionTomada", a.Estado, $"El técnico validó la viabilidad del alcance aceptado, incluyendo sus exclusiones. {motivo}");
            await unidad.GuardarCambiosAsync();
        }
        finally { unidad.DescartarCambios(); }
    }
    public async Task LiberarAsync(int id, byte[] version)
    {
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.TecnicoId); var a = await AtencionAsync(id); AutorizacionOperativa.Version(version, a.Version);
            ExigirReserva(a, u); a.IdTecnicoEnActividad = null; a.ActividadTecnica = null; repositorio.PrepararVersion(a, version);
            Evento(a, u, "EjecucionLiberada", a.Estado, "Se conserva lo ejecutado y queda disponible para otro técnico.");
            await unidad.GuardarCambiosAsync();
        }
        finally { unidad.DescartarCambios(); }
    }
    private static void ExigirReserva(Atencion a, Usuario u)
    {
        if (a.Estado != EstadoAtencion.EnEjecucion || a.IdTecnicoEnActividad != u.Id || a.ActividadTecnica != ActividadTecnica.Ejecucion)
            throw new ValidacionException("Debe tomar esta ejecución con su usuario antes de modificarla.");
    }
    public async Task GuardarAsync(EjecucionAtencionDto datos, bool finalizar, bool interrumpir = false)
    {
        ArgumentNullException.ThrowIfNull(datos);
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.TecnicoId); var a = await AtencionAsync(datos.IdAtencion);
            AutorizacionOperativa.Version(datos.Version, a.Version); ExigirReserva(a, u); var p = await PropuestaAsync(a.IdAtencion);
            if (!p.AlcanceValidado || await circuito.ObtenerPorAtencionAsync(a.IdAtencion) != null) throw new ValidacionException("El alcance no está validado o ya fue liquidado.");
            var items = p.Detalles.Where(d => d.AceptadoPorCliente == true).ToDictionary(d => d.IdPresupuestoDetalle);
            if (datos.Items is null || datos.Items.Any(d => d is null) || datos.Items.Count != items.Count || datos.Items.Select(d => d.IdDetalle).Distinct().Count() != items.Count)
                throw new ValidacionException("Los ítems autorizados cambiaron. Actualice la ejecución.");
            var motivo = AutorizacionOperativa.Texto(datos.Observaciones, interrumpir);
            foreach (var registro in datos.Items)
            {
                if (!items.TryGetValue(registro.IdDetalle, out var d)) throw new ValidacionException("No puede ejecutar un ítem excluido o ajeno.");
                var q = registro.CantidadEjecutada; var estado = registro.Estado;
                if (!Enum.IsDefined(estado) || estado == EstadoEjecucionDetalle.Parcial && q <= 0 || q < d.CantidadEjecutada || q < 0 || q > d.Cantidad || decimal.Round(q, 3) != q
                    || estado == EstadoEjecucionDetalle.Completado && q != d.Cantidad
                    || estado is EstadoEjecucionDetalle.NoIniciado or EstadoEjecucionDetalle.Omitido && q != 0
                    || estado is EstadoEjecucionDetalle.Parcial or EstadoEjecucionDetalle.EnCurso && q >= d.Cantidad
                    || d.EstadoEjecucion == EstadoEjecucionDetalle.Omitido && estado != EstadoEjecucionDetalle.Omitido)
                    throw new ValidacionException($"Revise estado y cantidad de «{d.Descripcion}». No puede disminuir lo registrado ni superar lo autorizado.");
                var nota = AutorizacionOperativa.Texto(registro.Observaciones, estado is EstadoEjecucionDetalle.Omitido or EstadoEjecucionDetalle.Parcial);
                if (finalizar && (estado is EstadoEjecucionDetalle.NoIniciado or EstadoEjecucionDetalle.EnCurso || !interrumpir && estado == EstadoEjecucionDetalle.Parcial))
                    throw new ValidacionException("Antes de terminar resuelva cada ítem: completado u omitido con motivo. Una interrupción admite cantidades parciales justificadas.");
                if (d.CantidadEjecutada != q || d.EstadoEjecucion != estado || (d.ObservacionesEjecucion ?? "") != nota)
                {
                    d.CantidadEjecutada = q; d.EstadoEjecucion = estado; d.ObservacionesEjecucion = nota;
                    d.IdTecnicoRegistro = u.Id; d.FechaRegistroEjecucion = DateTime.Now;
                }
            }
            var anterior = a.Estado;
            if (finalizar) { a.Estado = interrumpir ? EstadoAtencion.PendienteLiquidacion : EstadoAtencion.TrabajoFinalizado; a.IdTecnicoEnActividad = null; a.ActividadTecnica = null; }
            repositorio.PrepararVersion(a, datos.Version);
            Evento(a, u, finalizar ? (interrumpir ? "EjecucionInterrumpida" : "TrabajoFinalizado") : "EjecucionAvanceGuardado", anterior, motivo.Length > 0 ? motivo : "Se registró el avance de los ítems autorizados.");
            await unidad.GuardarCambiosAsync();
        }
        finally { unidad.DescartarCambios(); }
    }
}

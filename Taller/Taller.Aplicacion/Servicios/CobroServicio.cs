using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.Servicios;
/// <summary>Liquida lo autorizado y realizado, registra cobros mixtos/parciales y entrega con saldo cero.</summary>
public sealed class CobroServicio(ICircuitoRepositorio repositorio, IEvaluacionRepositorio evaluacion, IUnidadDeTrabajo unidad, AutorizacionOperativa acceso)
{
    private const decimal Maximo = 9999999999999999.99m;
    private async Task<Atencion> AtencionAsync(int id) => await evaluacion.ObtenerAtencionAsync(id) ?? throw new ValidacionException("La atención no existe.");
    private void Evento(Atencion a, Usuario u, string accion, string texto, EstadoAtencion? anterior = null) => evaluacion.AgregarHistorial(new HistorialAtencion { IdAtencion = a.IdAtencion, IdUsuario = u.Id, Fecha = DateTime.Now, Accion = accion, EstadoAnterior = anterior ?? a.Estado, EstadoNuevo = a.Estado, Observaciones = texto });
    private static void AbiertaParaCobros(Atencion a)
    {
        if (a.Estado is not (EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion) || a.IdTecnicoEnActividad != null)
            throw new ValidacionException("La atención debe haber terminado técnicamente, sin actividad reservada y sin entrega registrada.");
    }

    private async Task<List<PresupuestoDetalle>> FacturablesAsync(int id)
    {
        var presupuestos = await evaluacion.ObtenerPresupuestosAsync(id);
        if (presupuestos.Any(p => p.Vigente && p.Estado is EstadoPresupuesto.Borrador or EstadoPresupuesto.PendientePresentacion or EstadoPresupuesto.PendienteDecision or EstadoPresupuesto.PendienteRevisionTecnica))
            throw new ValidacionException("Debe resolver las propuestas vigentes antes de liquidar.");
        var a = await AtencionAsync(id);
        if (a.Pausas.Any(p => p.FechaResolucion == null))
            throw new ValidacionException("Resuelva las pausas antes de liquidar.");
        var autorizados = presupuestos.Where(p => p.Estado == EstadoPresupuesto.Aceptado).SelectMany(p => p.Detalles.Where(d => d.AceptadoPorCliente == true && (p.Vigente || d.CantidadEjecutada > 0))).ToList();
        if (autorizados.Any(d => d.EstadoEjecucion is EstadoEjecucionDetalle.NoIniciado or EstadoEjecucionDetalle.EnCurso))
            throw new ValidacionException("Hay servicios autorizados pendientes de resolver. El técnico debe registrar lo realizado u omitido.");
        if (autorizados.Any(d => d.CantidadEjecutada > 0 && (d.CantidadEjecutada > d.Cantidad || d.PrecioUnitario > Maximo / d.CantidadEjecutada)))
            throw new ValidacionException("Hay cantidades o importes ejecutados fuera del límite de liquidación.");
        return autorizados.Where(d => d.CantidadEjecutada > 0 && d.OrigenMaterial != OrigenMaterial.Cliente).ToList();
    }

    private static List<ComprobanteItemDto> Items(IEnumerable<PresupuestoDetalle> detalles) => detalles.Select(d => new ComprobanteItemDto { Descripcion = d.Descripcion, Cantidad = d.CantidadEjecutada, PrecioUnitario = d.PrecioUnitario, Subtotal = decimal.Round(d.CantidadEjecutada * d.PrecioUnitario, 2, MidpointRounding.AwayFromZero) }).ToList();
    private static ComprobanteDto Mapear(Comprobante d) => new()
    {
        IdComprobante = d.IdComprobante,
        IdAtencion = d.IdAtencion,
        Fecha = d.FechaEmision,
        Version = d.Version.ToArray(),
        Moneda = d.Moneda,
        Cliente = d.Cliente,
        Documento = d.DocumentoCliente,
        Dominio = d.Dominio,
        Observaciones = d.Observaciones,
        Total = d.Total,
        Pagado = d.TotalPagado,
        EstadoAtencion = d.Atencion.Estado,
        Items = d.Detalles.OrderBy(x => x.IdComprobanteDetalle).Select(x => new ComprobanteItemDto { Descripcion = x.Descripcion, Cantidad = x.Cantidad, PrecioUnitario = x.PrecioUnitario, Subtotal = x.Subtotal }).ToList(),
        Pagos = d.Pagos.OrderByDescending(p => p.Fecha).ThenByDescending(p => p.IdPago).Select(p => new PagoConsultaDto { IdPago = p.IdPago, Fecha = p.Fecha, Importe = p.Importe, SaldoPosterior = p.SaldoPosterior, Anulado = p.Anulado, MotivoAnulacion = p.MotivoAnulacion, Usuario = p.UsuarioRegistroNombre, Medios = string.Join("; ", p.Medios.OrderBy(m => m.Orden).Select(m => $"{m.NombreMedio}: {m.Importe:N2}" + (string.IsNullOrEmpty(m.Referencia) ? "" : $" ({m.Referencia})"))) }).ToList()
    };
    public async Task<List<AtencionCobroDto>> ListarAsync()
    {
        try
        {
            await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            var atenciones = await repositorio.ListarAtencionesParaCobrosAsync();
            var documentos = (await repositorio.ListarComprobantesAsync()).ToDictionary(d => d.IdAtencion);
            return atenciones.Select(a =>
            {
                documentos.TryGetValue(a.IdAtencion, out var d);
                return new AtencionCobroDto
                {
                    IdAtencion = a.IdAtencion,
                    Cliente = a.Cliente.Apellido + ", " + a.Cliente.Nombre,
                    Dominio = a.Vehiculo.Dominio,
                    Estado = a.Estado,
                    Fecha = d == null ? a.FechaApertura : d.Pagos.Select(p => p.FechaAnulacion ?? p.Fecha).Append(d.FechaEmision).Max(),
                    IdComprobante = d?.IdComprobante,
                    Total = d?.Total,
                    Pagado = d?.TotalPagado
                };
            }).ToList();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task<CuentaAtencionDto> ObtenerAsync(int idAtencion)
    {
        try
        {
            await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            var a = await AtencionAsync(idAtencion);
            var d = await repositorio.ObtenerPorAtencionAsync(idAtencion);
            return new CuentaAtencionDto
            {
                IdAtencion = idAtencion,
                Estado = a.Estado,
                VersionAtencion = a.Version.ToArray(),
                Resumen = $"Atención {idAtencion:D6} · {a.Cliente.Apellido}, {a.Cliente.Nombre} · {a.Vehiculo.Dominio} · {a.Estado}",
                Comprobante = d == null ? null : Mapear(d),
                ItemsPorLiquidar = d != null || a.Estado == EstadoAtencion.Cerrada ? [] : Items(await FacturablesAsync(idAtencion))
            };
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task<int> LiquidarAsync(int idAtencion, byte[] version, string observaciones)
    {
        try
        {
            var usuario = await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            var a = await AtencionAsync(idAtencion);
            var existente = await repositorio.ObtenerPorAtencionAsync(idAtencion);
            if (existente != null)
                return existente.IdComprobante;
            AutorizacionOperativa.Version(version, a.Version);
            AbiertaParaCobros(a);
            var origenes = await FacturablesAsync(idAtencion);
            var filas = Items(origenes);
            decimal total = 0;
            foreach (var f in filas)
            {
                if (f.Subtotal < 0 || f.Subtotal > Maximo - total)
                    throw new ValidacionException("El importe a liquidar excede el límite permitido.");
                total += f.Subtotal;
            }

            var nota = AutorizacionOperativa.Texto(observaciones, total == 0);
            var d = new Comprobante
            {
                IdAtencion = idAtencion,
                IdUsuarioEmision = usuario.Id,
                FechaEmision = DateTime.Now,
                Cliente = a.Cliente.Apellido + ", " + a.Cliente.Nombre,
                DocumentoCliente = a.Cliente.TipoDocumento + " " + a.Cliente.Documento,
                Dominio = a.Vehiculo.Dominio,
                Total = total,
                Observaciones = nota,
                Detalles = origenes.Select(x => new ComprobanteDetalle { IdPresupuestoDetalle = x.IdPresupuestoDetalle, Descripcion = x.Descripcion, Cantidad = x.CantidadEjecutada, PrecioUnitario = x.PrecioUnitario, Subtotal = decimal.Round(x.CantidadEjecutada * x.PrecioUnitario, 2, MidpointRounding.AwayFromZero) }).ToList()
            };
            repositorio.AgregarComprobante(d);
            evaluacion.PrepararVersion(a, version);
            Evento(a, usuario, "LiquidacionEmitida", $"Documento interno por {total:N2}, calculado sobre lo autorizado y realizado. {nota}");
            await unidad.GuardarCambiosAsync();
            return d.IdComprobante;
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task<List<MedioPagoDto>> ListarMediosAsync()
    {
        try
        {
            await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            return (await repositorio.ListarMediosAsync()).Select(m => new MedioPagoDto { IdMedioPago = m.IdMedioPago, Nombre = m.Nombre, RequiereReferencia = m.RequiereReferencia }).ToList();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    private static bool MismoIntento(Pago pago, RegistrarPagoDto d, int usuario) => pago.IdComprobante == d.IdComprobante && pago.IdUsuarioRegistro == usuario && pago.Medios.Count == d.Medios.Count && pago.Medios.OrderBy(m => m.Orden).Select((m, i) => m.IdMedioPago == d.Medios[i].IdMedioPago && m.Importe == d.Medios[i].Importe && (m.Referencia ?? "") == (d.Medios[i].Referencia ?? "").Trim()).All(x => x);
    public async Task<int> RegistrarPagoAsync(RegistrarPagoDto datos)
    {
        ArgumentNullException.ThrowIfNull(datos);
        int usuarioId = 0;
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            usuarioId = u.Id;
            if (datos.IdOperacion == Guid.Empty || datos.Medios is null || datos.Medios.Count == 0 || datos.Medios.Any(m => m is null))
                throw new ValidacionException("Agregue al menos un medio e importe y un identificador de operación válido.");
            var anterior = await repositorio.ObtenerPagoPorOperacionAsync(datos.IdOperacion);
            if (anterior != null)
            {
                if (!MismoIntento(anterior, datos, u.Id))
                    throw new ValidacionException("Este identificador ya pertenece a otro cobro. Actualice el formulario.");
                return anterior.IdPago;
            }

            var d = await repositorio.ObtenerComprobanteAsync(datos.IdComprobante) ?? throw new ValidacionException("El comprobante no existe.");
            AutorizacionOperativa.Version(datos.Version, d.Version);
            var a = await AtencionAsync(d.IdAtencion);
            AbiertaParaCobros(a);
            if (d.TotalPagado != d.Pagos.Where(p => !p.Anulado).Sum(p => p.Importe))
                throw new ValidacionException("El saldo no coincide con los pagos registrados. Revise los datos.");
            var medios = (await repositorio.ListarMediosAsync()).ToDictionary(m => m.IdMedioPago);
            decimal total = 0;
            foreach (var linea in datos.Medios)
            {
                if (!medios.TryGetValue(linea.IdMedioPago, out var medio))
                    throw new ValidacionException("Un medio de pago ya no está activo.");
                if (linea.Importe <= 0 || linea.Importe > Maximo - total || decimal.Round(linea.Importe, 2) != linea.Importe)
                    throw new ValidacionException("Cada importe debe ser positivo y tener hasta dos decimales.");
                if ((linea.Referencia?.Trim().Length ?? 0) > 200 || medio.RequiereReferencia && string.IsNullOrWhiteSpace(linea.Referencia))
                    throw new ValidacionException($"Ingrese una referencia válida para {medio.Nombre} (hasta 200 caracteres).");
                total += linea.Importe;
            }

            if (total > d.Total - d.TotalPagado)
                throw new ValidacionException("El importe supera el saldo pendiente; no se registra dinero de más.");
            var pago = new Pago
            {
                IdOperacion = datos.IdOperacion,
                IdComprobante = d.IdComprobante,
                IdUsuarioRegistro = u.Id,
                UsuarioRegistroNombre = u.Apellido + ", " + u.Nombre,
                Fecha = DateTime.Now,
                Importe = total,
                SaldoPosterior = d.Total - d.TotalPagado - total,
                Medios = datos.Medios.Select((m, i) => new PagoMedio { Orden = i, IdMedioPago = m.IdMedioPago, NombreMedio = medios[m.IdMedioPago].Nombre, Importe = m.Importe, Referencia = m.Referencia?.Trim() }).ToList()
            };
            repositorio.AgregarPago(pago);
            d.TotalPagado += total;
            repositorio.PrepararVersion(d, datos.Version);
            evaluacion.PrepararVersion(a, a.Version);
            Evento(a, u, "PagoRegistrado", $"Cobro por {total:N2} en {datos.Medios.Count} medio(s), sobre liquidación {d.IdComprobante}. Saldo {pago.SaldoPosterior:N2}.");
            await unidad.GuardarCambiosAsync();
            return pago.IdPago;
        }
        catch (ConflictoConcurrenciaException)
        {
            unidad.DescartarCambios();
            var existente = await repositorio.ObtenerPagoPorOperacionAsync(datos.IdOperacion);
            if (existente != null && datos.Medios != null && MismoIntento(existente, datos, usuarioId))
                return existente.IdPago;
            throw;
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task AnularPagoAsync(int idComprobante, int idPago, byte[] version, string motivo)
    {
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            motivo = AutorizacionOperativa.Texto(motivo);
            var d = await repositorio.ObtenerComprobanteAsync(idComprobante) ?? throw new ValidacionException("El comprobante no existe.");
            AutorizacionOperativa.Version(version, d.Version);
            var a = await AtencionAsync(d.IdAtencion);
            AbiertaParaCobros(a);
            var p = d.Pagos.SingleOrDefault(p => p.IdPago == idPago) ?? throw new ValidacionException("El pago no pertenece a este documento.");
            if (p.Anulado)
                throw new ValidacionException("El pago ya fue anulado.");
            if (d.TotalPagado != d.Pagos.Where(x => !x.Anulado).Sum(x => x.Importe))
                throw new ValidacionException("El saldo no coincide con los pagos registrados. Revise los datos antes de anular.");
            p.Anulado = true;
            p.MotivoAnulacion = motivo;
            p.FechaAnulacion = DateTime.Now;
            p.IdUsuarioAnulacion = u.Id;
            d.TotalPagado -= p.Importe;
            repositorio.PrepararVersion(d, version);
            evaluacion.PrepararVersion(a, a.Version);
            Evento(a, u, "PagoAnulado", $"Se anuló el registro del recibo {idPago} por {p.Importe:N2}. Motivo: {motivo}");
            await unidad.GuardarCambiosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }

    public async Task EntregarAsync(int idAtencion, byte[] version, string condicion)
    {
        try
        {
            var u = await acceso.ExigirAsync(RolesSistema.AdministrativoId);
            condicion = AutorizacionOperativa.Texto(condicion);
            var a = await AtencionAsync(idAtencion);
            AutorizacionOperativa.Version(version, a.Version);
            AbiertaParaCobros(a);
            var d = await repositorio.ObtenerPorAtencionAsync(idAtencion) ?? throw new ValidacionException("Primero debe emitir la liquidación, incluso si no hay cargo.");
            if (d.Total - d.TotalPagado != 0 || d.TotalPagado != d.Pagos.Where(p => !p.Anulado).Sum(p => p.Importe))
                throw new ValidacionException("La entrega exige saldo exactamente cero y pagos consistentes.");
            var anterior = a.Estado;
            a.Estado = EstadoAtencion.Cerrada;
            a.FechaCierre = DateTime.Now;
            evaluacion.PrepararVersion(a, version);
            Evento(a, u, "VehiculoEntregado", condicion, anterior);
            await unidad.GuardarCambiosAsync();
        }
        finally
        {
            unidad.DescartarCambios();
        }
    }
}

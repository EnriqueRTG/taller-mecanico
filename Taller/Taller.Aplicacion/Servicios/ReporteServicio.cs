using Taller.Aplicacion.Consultas;
using Taller.Dominio.Tiempo;
using Taller.Aplicacion.Seguridad;
using System.Globalization;
using System.Text.RegularExpressions;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Enumeraciones;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;
/// <summary>Autoriza reportes por rol y construye un documento coherente con los mismos registros filtrados.</summary>
public sealed class ReporteServicio(IReporteRepositorio reportes, IUsuarioRepositorio usuarios, SesionUsuario sesion)
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-AR");
    public static string Nombre(TipoReporte tipo) => tipo switch
    {
        TipoReporte.CargaTecnica => "Carga técnica actual y trabajo disponible",
        TipoReporte.Atenciones => "Atenciones recibidas y situación actual",
        TipoReporte.Presupuestos => "Decisiones de presupuestos vigentes",
        TipoReporte.MovimientosCobro => "Cobros y anulaciones por medio de pago",
        TipoReporte.SaldosPendientes => "Saldos pendientes actuales",
        TipoReporte.ActividadTecnica => "Actividad técnica registrada",
        TipoReporte.ConceptosRealizados => "Conceptos realizados registrados",
        _ => throw new ValidacionException("Tipo de reporte inválido.")};
    public static string Nombre(TipoReporte tipo, bool alcancePersonal) =>
        alcancePersonal ? tipo switch
        {
            TipoReporte.Atenciones => "Intervenciones en atenciones",
            TipoReporte.SaldosPendientes => "Saldos pendientes de atenciones gestionadas",
            _ => Nombre(tipo)
        } : Nombre(tipo);

    public static string AlcancePara(TipoReporte tipo, int rol) => (rol, tipo) switch
    {
        (RolesSistema.AdministradorId, _) => "Todo el taller",
        (RolesSistema.AdministrativoId, TipoReporte.Atenciones) => "Acciones registradas por el usuario actual",
        (RolesSistema.AdministrativoId, TipoReporte.Presupuestos) => "Decisiones registradas por el usuario actual",
        (RolesSistema.AdministrativoId, TipoReporte.MovimientosCobro) => "Cobros y anulaciones registrados por el usuario actual",
        (RolesSistema.AdministrativoId, TipoReporte.SaldosPendientes) => "Atenciones con al menos una intervención del usuario actual",
        (RolesSistema.TecnicoId, TipoReporte.CargaTecnica) => "Reservas del técnico actual y trabajos disponibles",
        (RolesSistema.TecnicoId, TipoReporte.Presupuestos) => "Presupuestos de autoría del técnico actual",
        (RolesSistema.TecnicoId, TipoReporte.ActividadTecnica) => "Eventos registrados por el técnico actual",
        (RolesSistema.TecnicoId, TipoReporte.ConceptosRealizados) => "Conceptos cuyo último registro corresponde al técnico actual",
        _ => throw new ValidacionException("Tipo de reporte no disponible para este rol.")
    };

    public static string Legible(string valor) => valor switch
    {
        "RecepcionRegistrada" => "Apertura",
        "PresupuestoPresentado" => "Comunicación de presupuesto",
        "DecisionPresupuestoRegistrada" => "Registro de decisión",
        "LiquidacionEmitida" => "Emisión de liquidación",
        "PagoRegistrado" => "Registro de cobro",
        "PagoAnulado" => "Anulación de cobro",
        "VehiculoEntregado" => "Entrega de vehículo",
        "EnEvaluacionTecnica" => "En preparación técnica",
        "PendienteRevisionTecnica" => "Revisión técnica pendiente",
        "PendientePresentacion" => "Por presentar",
        "PendienteDecision" => "Esperando decisión",
        "True" or "true" => "Sí",
        "False" or "false" => "No",
        "SeleccionClienteValidada" => "Selección del cliente validada",
        "SeleccionParcialRevisada" => "Selección reformulada por el técnico",
        "EjecucionAvanceGuardado" => "Avance de ejecución guardado",
        "EvaluacionAvanceGuardado" => "Avance de evaluación guardado",
        "PausaResuelta" => "Pausa resuelta",
        _ => Regex.Replace(valor, "([a-záéíóúñ])([A-ZÁÉÍÓÚÑ])", "$1 $2")
    };
    /// <summary>Opciones del dominio de cada reporte; no dependen de generar un documento ni de que existan filas.</summary>
    public static IReadOnlyList<string> EstadosPara(TipoReporte tipo, bool alcancePersonal = false)
    {
        IEnumerable<string> valores = tipo switch
        {
        TipoReporte.Atenciones => alcancePersonal ? IntervencionAdministrativaReporte.Acciones : Enum.GetNames<EstadoAtencion>(),
        TipoReporte.Presupuestos => Enum.GetNames<EstadoPresupuesto>().Append("Aceptado parcialmente"),
        TipoReporte.MovimientosCobro => new[] { "Cobro", "Anulación" },
        TipoReporte.SaldosPendientes => new[] { "Sin cobros", "Cobrado parcialmente" },
        TipoReporte.CargaTecnica => new[] { "Disponible para tomar", "Reservada" },
        TipoReporte.ConceptosRealizados => Enum.GetNames<EstadoEjecucionDetalle>().Where(e => e != nameof(EstadoEjecucionDetalle.NoIniciado)),
        TipoReporte.ActividadTecnica => ActividadTecnicaReporte.AccionesFiltrables,
        _ => throw new ValidacionException("Tipo de reporte inválido.")
        };
        return valores.Select(Legible).Distinct().Order().ToArray();
    }



    public static IReadOnlyList<TipoReporte> TiposParaRol(int rol) => rol switch
    {
        RolesSistema.AdministradorId => Enum.GetValues<TipoReporte>(),
        RolesSistema.AdministrativoId => [TipoReporte.Atenciones, TipoReporte.Presupuestos, TipoReporte.MovimientosCobro, TipoReporte.SaldosPendientes],
        RolesSistema.TecnicoId => [TipoReporte.CargaTecnica, TipoReporte.ActividadTecnica, TipoReporte.ConceptosRealizados, TipoReporte.Presupuestos],
        _ => []
    };
    private readonly AutorizacionOperativa _acceso = new(usuarios, sesion);
    private Task<Usuario> ExigirUsuarioAsync() => _acceso.ExigirRolesAsync(
        RolesSistema.AdministradorId, RolesSistema.AdministrativoId, RolesSistema.TecnicoId);

    public async Task<IReadOnlyList<TipoReporte>> ListarTiposAsync() => TiposParaRol((await ExigirUsuarioAsync()).RolId);
    public async Task<int> ObtenerRolAsync() => (await ExigirUsuarioAsync()).RolId;

    public async Task<ReporteDocumentoDto> GenerarAsync(SolicitudReporteDto solicitud)
    {
        ArgumentNullException.ThrowIfNull(solicitud);
        return await GenerarAutorizadoAsync(solicitud, await ExigirUsuarioAsync(), false);
    }

    /// <summary>Usado por Inicio tras su autorización única. Comparte las fórmulas y redondeos de reportes.</summary>
    internal async Task<List<ReporteDocumentoDto>> ResumirInicioAsync(Usuario usuario)
    {
        TipoReporte[] tipos = TiposParaRol(usuario.RolId).Contains(TipoReporte.MovimientosCobro)
            ? [TipoReporte.Atenciones, TipoReporte.MovimientosCobro, TipoReporte.SaldosPendientes]
            : [TipoReporte.CargaTecnica, TipoReporte.ActividadTecnica, TipoReporte.Presupuestos];
        var hoy = FechaHoraTaller.Hoy;
        var documentos = new List<ReporteDocumentoDto>();
        // El contexto se utiliza secuencialmente; no se paralelizan operaciones EF sobre la misma instancia.
        foreach (var tipo in tipos)
            documentos.Add(await GenerarAutorizadoAsync(new SolicitudReporteDto {
                Tipo = tipo, Desde = new DateTime(hoy.Year, hoy.Month, 1), Hasta = hoy }, usuario, true));
        return documentos;
    }

    private async Task<ReporteDocumentoDto> GenerarAutorizadoAsync(SolicitudReporteDto solicitud, Usuario usuario, bool soloIndicadores)
    {
        if (!TiposParaRol(usuario.RolId).Contains(solicitud.Tipo))
            throw new ValidacionException("Su rol no permite consultar este reporte.");
        bool alcancePersonal = usuario.RolId == RolesSistema.AdministrativoId;
        int? administrativo = alcancePersonal ? usuario.Id : null;
        var estadosDisponibles = EstadosPara(solicitud.Tipo, alcancePersonal);
        if (solicitud.Estado != "Todos" && !estadosDisponibles.Contains(solicitud.Estado))
            throw new ValidacionException("Seleccione un estado válido para el tipo de reporte.");
        var hoy = FechaHoraTaller.Hoy;
        if (solicitud.Tipo is not (TipoReporte.SaldosPendientes or TipoReporte.CargaTecnica) && (solicitud.Desde.Date > solicitud.Hasta.Date || solicitud.Hasta.Date > hoy || solicitud.Hasta.Year >= 9999))
            throw new ValidacionException("Seleccione un período válido, sin fechas futuras.");
        if ((solicitud.Busqueda?.Length ?? 0) > 150)
            throw new ValidacionException("La búsqueda admite hasta 150 caracteres.");
        int? tecnico = usuario.RolId == RolesSistema.TecnicoId ? usuario.Id : null;
        var desde = solicitud.Desde.Date;
        var hasta = (solicitud.Tipo is TipoReporte.SaldosPendientes or TipoReporte.CargaTecnica ? hoy : solicitud.Hasta.Date).AddDays(1);
        var registros = soloIndicadores && !alcancePersonal
            ? await reportes.ListarIndicadoresAsync(solicitud.Tipo, desde, hasta, tecnico)
            : solicitud.Tipo switch
        {
            TipoReporte.CargaTecnica => await reportes.ListarCargaAsync(tecnico),
            TipoReporte.Atenciones => alcancePersonal
                ? await reportes.ListarIntervencionesAsync(desde, hasta, usuario.Id)
                : await reportes.ListarAtencionesAsync(desde, hasta),
            TipoReporte.Presupuestos => await reportes.ListarPresupuestosAsync(desde, hasta, tecnico, administrativo),
            TipoReporte.MovimientosCobro => await reportes.ListarMovimientosAsync(desde, hasta, administrativo),
            TipoReporte.SaldosPendientes => await reportes.ListarSaldosAsync(administrativo),
            TipoReporte.ActividadTecnica => await reportes.ListarActividadAsync(desde, hasta, tecnico),
            TipoReporte.ConceptosRealizados => await reportes.ListarConceptosAsync(desde, hasta, tecnico),
            _ => throw new ValidacionException("Tipo de reporte inválido.")};
        string texto = (solicitud.Busqueda ?? "").Trim();
        registros = registros.Where(x => (texto.Length == 0 || BusquedaTexto.Coincide($"{x.Codigo} {x.IdAtencion} {x.Descripcion} {x.Responsable}", texto)) && (solicitud.Estado == "Todos" || Legible(x.Estado) == solicitud.Estado)).OrderByDescending(x => x.Fecha).ThenBy(x => x.Codigo).ToList();
        if (solicitud.Tipo == TipoReporte.SaldosPendientes)
            registros = registros.OrderBy(x => x.Fecha).ThenByDescending(x => x.Importe).ToList();
        if (solicitud.Tipo == TipoReporte.CargaTecnica)
            registros = registros.OrderByDescending(x => x.Estado == "Reservada").ThenBy(x => x.Fecha).ToList();
        var indicadores = new List<IndicadorReporteDto>
        {
            new(alcancePersonal && solicitud.Tipo == TipoReporte.Atenciones ? "Intervenciones" : "Registros", registros.Count.ToString(Cultura)),
            new("Atenciones distintas", registros.Select(x => x.IdAtencion).Distinct().Count().ToString(Cultura))
        };
        bool monetario = solicitud.Tipo is TipoReporte.MovimientosCobro or TipoReporte.SaldosPendientes;
        string metodologia;
        string recomendacion;
        if (solicitud.Tipo == TipoReporte.MovimientosCobro)
        {
            decimal cobros = registros.Where(x => x.Estado == "Cobro").Sum(x => x.Importe ?? 0), anulaciones = -registros.Where(x => x.Estado == "Anulación").Sum(x => x.Importe ?? 0);
            indicadores.AddRange([new("Cobros registrados · ARS", Dinero(cobros)), new("Anulaciones · ARS", Dinero(anulaciones)), new("Movimiento neto · ARS", Dinero(cobros - anulaciones))]);
            metodologia = "Cada fila representa un medio de un recibo. El cobro se suma en su fecha y la anulación se resta en su propia fecha, incluso si el cobro fue anterior al período. No mide utilidad ni ventas; las anulaciones no acreditan una devolución bancaria.";
            recomendacion = anulaciones > 0 ? "Revisar los motivos de las anulaciones y conciliar los medios de pago con caja/banco." : "Conciliar los importes por medio con caja y movimientos bancarios del período.";
        }
        else if (solicitud.Tipo == TipoReporte.SaldosPendientes)
        {
            indicadores.Add(new("Saldo pendiente · ARS", Dinero(registros.Sum(x => x.Importe ?? 0))));
            indicadores.Add(new("Antigüedad máxima · días", registros.Count == 0 ? "0" : registros.Max(x => Math.Max(0, (hoy - x.Fecha.Date).Days)).ToString(Cultura)));
            metodologia = "Situación actual de todas las liquidaciones con saldo positivo, sin límite de fecha de emisión. Antigüedad desde la emisión, no mora: no se registran vencimientos. Los servicios todavía sin liquidar no integran este saldo.";
            recomendacion = "Priorizar la revisión de liquidaciones antiguas y cobros parciales; la entrega exige saldo cero.";
        }
        else if (solicitud.Tipo == TipoReporte.Presupuestos)
        {
            int decididos = registros.Count(x => x.Grupo is "Aceptado" or "Rechazado"), aceptados = registros.Count(x => x.Grupo == "Aceptado");
            indicadores.Add(new("Aceptación entre decididos", decididos == 0 ? "Sin decisiones" : ((decimal)aceptados / decididos).ToString("P1", Cultura)));
            indicadores.Add(new("Importe autorizado · ARS", Dinero(registros.Sum(x => x.ImporteAceptado ?? 0))));
            metodologia = "Cohorte por fecha de creación: propuestas vigentes, incluidas inspecciones y adicionales; excluye versiones reemplazadas. Estado actual al generar. Aceptación = propuestas aceptadas total o parcialmente / aceptadas y rechazadas; no incluye pendientes. Importe autorizado conserva precios de la propuesta y no representa cobros.";
            recomendacion = "Revisar propuestas pendientes de presentación o decisión y las exclusiones antes de ejecutar.";
        }
        else if (solicitud.Tipo == TipoReporte.ConceptosRealizados)
        {
            indicadores.Add(new("Valor realizado · ARS", Dinero(registros.Sum(x => decimal.Round(x.Importe ?? 0, 2, MidpointRounding.AwayFromZero)))));
            metodologia = "Ítems con cantidad realizada positiva, según fecha de su última actualización de ejecución. Cantidad acumulada del ítem, no producción incremental del período; se atribuye al último técnico que registró, no a todos los colaboradores. No suma cantidades de unidades diferentes. El valor realizado a precio acordado no equivale a cobro ni ganancia.";
            recomendacion = "Revisar la mezcla de servicios, repuestos y consumibles para organizar recursos; este registro no permite medir horas ni comparar productividad individual.";
        }
        else if (solicitud.Tipo == TipoReporte.CargaTecnica)
        {
            indicadores.Add(new("Disponibles para tomar", registros.Count(x => x.Estado == "Disponible para tomar").ToString(Cultura)));
            indicadores.Add(new("Reservadas", registros.Count(x => x.Estado == "Reservada").ToString(Cultura)));
            metodologia = "Situación actual: evaluación y ejecución pendientes, incluyendo aperturas antiguas. Fecha del registro = apertura. El técnico ve sus reservas y los trabajos sin reserva; no las reservas de otros técnicos. No muestra cerradas ni canceladas. El estado detallado es el de la reserva y el gráfico agrupa por etapa.";
            recomendacion = "Continuar las actividades propias y coordinar las disponibles de mayor antigüedad; liberar una reserva si otro técnico debe continuarla.";
        }
        else if (solicitud.Tipo == TipoReporte.ActividadTecnica)
        {
            metodologia = "Selección de hitos operativos del historial realizados por técnicos durante el período; no es una auditoría completa de todas las acciones. Las correcciones administrativas y la preparación de su nueva versión se consultan en el historial de la atención. Un evento no equivale a un diagnóstico, una hora ni un trabajo terminado; varias acciones pueden pertenecer a la misma atención. El técnico consulta exclusivamente sus propios eventos. El filtro ofrece hitos principales y tres alternativas habituales; «Todos» conserva además avances, liberaciones, pausas y materiales.";
            recomendacion = "Revisar el inicio y finalización de evaluaciones y trabajos; consultar hallazgos y revisiones del alcance cuando corresponda.";
        }
        else
        {
            indicadores.Add(new("Actualmente activas", registros.Count(x => x.Estado is not ("Cerrada" or "Cancelada")).ToString(Cultura)));
            metodologia = "Atenciones recibidas durante el período, agrupadas por su estado actual. No reconstruye el estado que tenían al cierre del período ni incluye aperturas anteriores. El importe no se infiere del motivo ni del estado.";
            recomendacion = "Revisar las atenciones activas de mayor antigüedad y los casos pendientes de decisión; ampliar el período para incluir aperturas anteriores.";
        }

        if (alcancePersonal)
        {
            metodologia = solicitud.Tipo switch
            {
                TipoReporte.Atenciones => "Una fila por acción propia en la fecha de intervención. Una atención puede aparecer varias veces. El detalle muestra su estado al generar el reporte.",
                TipoReporte.Presupuestos => "Propuestas vigentes con decisión del cliente registrada por usted, filtradas por fecha de decisión. Las comunicaciones se consultan en Intervenciones en atenciones. El resultado mostrado es el vigente al generar.",
                TipoReporte.SaldosPendientes => "Incluye liquidaciones con saldo de atenciones con al menos una intervención propia registrada. Muestra el saldo actual completo de cada liquidación y considera los pagos de todos los operadores.",
                _ => metodologia + " Solo incluye cobros registrados y anulaciones efectuadas por usted."
            };
            if (solicitud.Tipo == TipoReporte.Atenciones)
            {
                indicadores.RemoveAt(indicadores.Count - 1);
                recomendacion = "Use Acción para distinguir las intervenciones y revise el estado actual de cada atención en el detalle.";
            }
        }

        if (soloIndicadores)
            return new ReporteDocumentoDto { Titulo = Nombre(solicitud.Tipo, alcancePersonal), Indicadores = indicadores };

        var grafico = registros.GroupBy(x => Legible(x.Grupo)).Select(g => new PuntoReporteDto(g.Key, monetario ? g.Sum(x => x.Importe ?? 0) : g.Count())).OrderByDescending(x => Math.Abs(x.Valor)).ThenBy(x => x.Etiqueta).ToList();
        if (grafico.Count > 8)
        {
            var resto = grafico.Skip(7).ToList();
            metodologia += " El gráfico muestra siete categorías principales y un resto desglosado: "
                + string.Join("; ", resto.Select(p => $"{p.Etiqueta}: {p.Valor.ToString(monetario ? "N2" : "N0", Cultura)} {(monetario ? "ARS" : "registros")}")) + ". Todos los registros se conservan en el detalle.";
            grafico = grafico.Take(7).Append(new PuntoReporteDto($"Resto ({resto.Count} categorías; ver desglose)", resto.Sum(x => x.Valor))).ToList();
        }
        return new ReporteDocumentoDto
        {
            EstadosDisponibles = estadosDisponibles,
            Titulo = Nombre(solicitud.Tipo, alcancePersonal),
            Generado = FechaHoraTaller.Ahora,
            EmitidoPor = $"{usuario.Apellido}, {usuario.Nombre}",
            Alcance = AlcancePara(solicitud.Tipo, usuario.RolId),
            Filtros = solicitud.Tipo is TipoReporte.SaldosPendientes or TipoReporte.CargaTecnica ? $"Situación actual · Estado: {solicitud.Estado} · Búsqueda: {texto}" : $"Desde {desde:dd/MM/yyyy} hasta {solicitud.Hasta:dd/MM/yyyy} inclusive · {(alcancePersonal && solicitud.Tipo == TipoReporte.Atenciones ? "Acción" : "Estado")}: {solicitud.Estado} · Búsqueda: {texto}",
            Metodologia = metodologia,
            Recomendacion = registros.Count == 0 ? "No hay registros para estos filtros. No implica ausencia de actividad fuera del alcance consultado." : recomendacion,
            UnidadGrafico = monetario ? "ARS" : "Registros",
            Indicadores = indicadores,
            Grafico = grafico,
            Registros = registros
        };
    }

    private static string Dinero(decimal valor) => valor.ToString("N2", Cultura);
}

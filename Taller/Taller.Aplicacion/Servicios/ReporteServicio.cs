using System.Globalization;
using System.Text.RegularExpressions;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;
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
        _ => throw new ValidacionException("Tipo de reporte inválido.")
    };
    public static string Legible(string valor) => Regex.Replace(valor, "([a-záéíóúñ])([A-ZÁÉÍÓÚÑ])", "$1 $2");
    public static IReadOnlyList<TipoReporte> TiposParaRol(int rol) => rol switch
    {
        RolesSistema.AdministradorId => Enum.GetValues<TipoReporte>(),
        RolesSistema.AdministrativoId => [TipoReporte.Atenciones, TipoReporte.Presupuestos, TipoReporte.MovimientosCobro, TipoReporte.SaldosPendientes],
        RolesSistema.TecnicoId => [TipoReporte.CargaTecnica, TipoReporte.ActividadTecnica, TipoReporte.ConceptosRealizados, TipoReporte.Presupuestos],
        _ => []
    };
    private async Task<Usuario> ExigirUsuarioAsync()
    {
        var u = sesion.UsuarioActual is { } actual ? await usuarios.ObtenerPorIdAsync(actual.Id) : null;
        if (u is null || !u.Activo || TiposParaRol(u.RolId).Count == 0) throw new ValidacionException("Se requiere una sesión activa autorizada para consultar reportes.");
        return u;
    }
    public async Task<IReadOnlyList<TipoReporte>> ListarTiposAsync() => TiposParaRol((await ExigirUsuarioAsync()).RolId);
    public async Task<ReporteDocumentoDto> GenerarAsync(SolicitudReporteDto solicitud)
    {
        ArgumentNullException.ThrowIfNull(solicitud);
        var usuario = await ExigirUsuarioAsync();
        if (!TiposParaRol(usuario.RolId).Contains(solicitud.Tipo)) throw new ValidacionException("Su rol no permite consultar este reporte.");
        var hoy = DateTime.Today;
        if (solicitud.Tipo is not (TipoReporte.SaldosPendientes or TipoReporte.CargaTecnica) && (solicitud.Desde.Date > solicitud.Hasta.Date || solicitud.Hasta.Date > hoy || solicitud.Hasta.Year >= 9999))
            throw new ValidacionException("Seleccione un período válido, sin fechas futuras.");
        if ((solicitud.Busqueda?.Length ?? 0) > 150) throw new ValidacionException("La búsqueda admite hasta 150 caracteres.");
        int? tecnico = usuario.RolId == RolesSistema.TecnicoId ? usuario.Id : null;
        var desde = solicitud.Desde.Date; var hasta = (solicitud.Tipo is TipoReporte.SaldosPendientes or TipoReporte.CargaTecnica ? hoy : solicitud.Hasta.Date).AddDays(1);
        var registros = solicitud.Tipo switch
        {
            TipoReporte.CargaTecnica => await reportes.ListarCargaAsync(tecnico),
            TipoReporte.Atenciones => await reportes.ListarAtencionesAsync(desde, hasta),
            TipoReporte.Presupuestos => await reportes.ListarPresupuestosAsync(desde, hasta, tecnico),
            TipoReporte.MovimientosCobro => await reportes.ListarMovimientosAsync(desde, hasta),
            TipoReporte.SaldosPendientes => await reportes.ListarSaldosAsync(),
            TipoReporte.ActividadTecnica => await reportes.ListarActividadAsync(desde, hasta, tecnico),
            TipoReporte.ConceptosRealizados => await reportes.ListarConceptosAsync(desde, hasta, tecnico),
            _ => throw new ValidacionException("Tipo de reporte inválido.")
        };
        string texto = (solicitud.Busqueda ?? "").Trim();
        registros = registros.Where(x => (texto.Length == 0 || $"{x.Codigo} {x.IdAtencion} {x.Descripcion} {x.Responsable}".Contains(texto, StringComparison.OrdinalIgnoreCase))
            && (solicitud.Estado == "Todos" || Legible(x.Estado) == solicitud.Estado))
            .OrderByDescending(x => x.Fecha).ThenBy(x => x.Codigo).ToList();
        if (solicitud.Tipo == TipoReporte.SaldosPendientes) registros = registros.OrderBy(x => x.Fecha).ThenByDescending(x => x.Importe).ToList();
        if (solicitud.Tipo == TipoReporte.CargaTecnica) registros = registros.OrderByDescending(x => x.Estado == "Reservada").ThenBy(x => x.Fecha).ToList();
        var indicadores = new List<IndicadorReporteDto> { new("Registros", registros.Count.ToString(Cultura)), new("Atenciones distintas", registros.Select(x => x.IdAtencion).Distinct().Count().ToString(Cultura)) };
        bool monetario = solicitud.Tipo is TipoReporte.MovimientosCobro or TipoReporte.SaldosPendientes;
        string metodologia; string recomendacion;
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
            metodologia = "Cohorte por fecha de creación: solo propuestas de reparación vigentes; excluye servicios iniciales y versiones reemplazadas. Estado actual al generar. Aceptación = propuestas aceptadas total o parcialmente / aceptadas y rechazadas; no incluye pendientes. Importe autorizado conserva precios de la propuesta y no representa cobros.";
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
            metodologia = "Eventos del historial realizados por usuarios técnicos durante el período. Un evento no equivale a un diagnóstico, una hora ni un trabajo terminado; varias acciones pueden pertenecer a la misma atención. El técnico consulta exclusivamente sus propios eventos.";
            recomendacion = "Identificar interrupciones y liberaciones frecuentes para revisar causas y coordinar continuidad de las atenciones.";
        }
        else
        {
            indicadores.Add(new("Actualmente activas", registros.Count(x => x.Estado is not ("Cerrada" or "Cancelada")).ToString(Cultura)));
            metodologia = "Atenciones recibidas durante el período, agrupadas por su estado actual. No reconstruye el estado que tenían al cierre del período ni incluye aperturas anteriores. El importe no se infiere del motivo ni del estado.";
            recomendacion = "Revisar las atenciones activas de mayor antigüedad y los casos pendientes de decisión; ampliar el período para incluir aperturas anteriores.";
        }
        var grafico = registros.GroupBy(x => Legible(x.Grupo)).Select(g => new PuntoReporteDto(g.Key, monetario ? g.Sum(x => x.Importe ?? 0) : g.Count()))
            .OrderByDescending(x => Math.Abs(x.Valor)).ThenBy(x => x.Etiqueta).ToList();
        if (grafico.Count > 8) grafico = grafico.Take(7).Append(new PuntoReporteDto("Otras categorías", grafico.Skip(7).Sum(x => x.Valor))).ToList();
        return new ReporteDocumentoDto
        {
            Titulo = Nombre(solicitud.Tipo),
            Generado = DateTime.Now,
            EmitidoPor = $"{usuario.Apellido}, {usuario.Nombre}",
            Alcance = tecnico.HasValue ? (solicitud.Tipo == TipoReporte.CargaTecnica ? "Reservas propias y actividades disponibles" : "Solo registros propios del técnico autenticado") : "Taller completo · " + RolesSistema.ObtenerNombreEsperado(usuario.RolId),
            Filtros = solicitud.Tipo is TipoReporte.SaldosPendientes or TipoReporte.CargaTecnica ? $"Situación actual · Estado: {solicitud.Estado} · Búsqueda: {texto}" : $"Desde {desde:dd/MM/yyyy} hasta {solicitud.Hasta:dd/MM/yyyy} inclusive · Estado: {solicitud.Estado} · Búsqueda: {texto}",
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

using System.ComponentModel;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Atenciones;
/// <summary>Consulta las solicitudes y el expediente consolidado de la atención.</summary>
public partial class FrmDetalleAtencion : Form
{
    private readonly IServiceProvider _proveedor;
    private readonly int _id;
    private readonly AtencionServicio _atenciones;
    private readonly EvaluacionServicio _evaluaciones;
    private readonly SesionUsuario _sesion;
    private Atencion? _atencion;
    private bool _ocupado;
    public FrmDetalleAtencion(int idAtencion, AtencionServicio atencionServicio, SesionUsuario sesionUsuario, EvaluacionServicio evaluaciones, IServiceProvider proveedor)
    {
        InitializeComponent();
        _proveedor = proveedor;
        _id = idAtencion;
        _atenciones = atencionServicio;
        _sesion = sesionUsuario;
        _evaluaciones = evaluaciones;
    }

    private void VerProgreso(object? sender, EventArgs e)
    {
        if (_ocupado || !btnProgreso.Enabled) return;
        try
        {
            using var f = new FrmSeguimientoTrabajo(_id,
                Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<SeguimientoTrabajoServicio>(_proveedor),
                _evaluaciones, Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<PresupuestoServicio>(_proveedor),
                Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto>(_proveedor));
            f.ShowDialog(this);
        }
        catch (Exception ex) { Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Consultar progreso"); }
    }
    private async void Cargar(object? s, EventArgs e) => await RecargarAsync();
    private async Task RecargarAsync()
    {
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            _atencion = await _atenciones.ObtenerPorIdAsync(_id) ?? throw new InvalidOperationException("La atención no existe.");
            lblResumen.Text = $"Atención {_id:D6} · {_atencion.Estado} · Solicitud: {_atencion.TipoSolicitud}\n{_atencion.Cliente.Apellido}, {_atencion.Cliente.Nombre} · {_atencion.Vehiculo.Dominio} · Apertura {_atencion.FechaApertura:dd/MM/yyyy HH:mm}";
            txtMotivo.Text = (_atencion.Solicitudes.Count == 0 ? _atencion.MotivoConsulta : string.Join(Environment.NewLine, _atencion.Solicitudes.OrderBy(s => s.Orden).Select(s => $"{s.Orden}. {(s.Tipo == Taller.Dominio.Enumeraciones.TipoSolicitudIngreso.TrabajoSolicitado ? "Solicita" : "Informa")}: {s.Descripcion}")))
                + $"\r\nKilometraje de ingreso: {_atencion.KilometrajeRecepcion?.ToString("N0") ?? "No informado"}\r\nObservaciones: {_atencion.ObservacionesRecepcion}";
            btnProgreso.Enabled = true;
            bool recepcion = _sesion.UsuarioActual?.RolId == RolesSistema.AdministrativoId;
            var documentos = (await _evaluaciones.ListarPresupuestosAsync()).Where(p => p.IdAtencion == _id).OrderBy(p => p.IdPresupuesto).ToList();
            var seguimiento = await Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<SeguimientoTrabajoServicio>(_proveedor).ObtenerAsync(_id);
            var detalle = new System.Text.StringBuilder();
            detalle.AppendLine($"ATENCIÓN {_id:D6} · {_atencion.Estado}\r\nCliente: {_atencion.Cliente.Apellido}, {_atencion.Cliente.Nombre} · Vehículo: {_atencion.Vehiculo.Dominio}");
            detalle.AppendLine("\r\nINFORMES TÉCNICOS");
            detalle.AppendLine(string.IsNullOrWhiteSpace(seguimiento.InformeInicial) ? "Sin diagnóstico registrado." : seguimiento.InformeInicial.Replace("\n", Environment.NewLine));
            detalle.AppendLine("\r\nPRESUPUESTOS Y VERSIONES");
            foreach (var p in documentos) detalle.AppendLine($"{p.IdPresupuesto:D6} · {p.TipoDescripcion} · {p.EstadoDescripcion} · {(p.Vigente ? "Vigente" : "Histórico")} · Total {p.Total:N2}");
            detalle.AppendLine("\r\nTRABAJOS AUTORIZADOS Y AVANCE");
            foreach (var i in seguimiento.Progreso) detalle.AppendLine($"{i.Descripcion}: {i.EstadoDescripcion} · {i.CantidadEjecutada:N3} / {i.CantidadAutorizada:N3}. {i.Observaciones}");
            detalle.AppendLine("\r\nHALLAZGOS");
            foreach (var h in seguimiento.Hallazgos) detalle.AppendLine($"{h.Fecha:dd/MM/yyyy HH:mm} · {h.Descripcion} · {h.Recomendacion}");
            if (recepcion && _atencion.Estado is Taller.Dominio.Enumeraciones.EstadoAtencion.TrabajoFinalizado or Taller.Dominio.Enumeraciones.EstadoAtencion.PendienteLiquidacion or Taller.Dominio.Enumeraciones.EstadoAtencion.Cerrada)
            {
                var cuenta = await Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<CobroServicio>(_proveedor).ObtenerAsync(_id);
                detalle.AppendLine("\r\nLIQUIDACIÓN Y COBROS");
                if (cuenta.Comprobante is { } c)
                {
                    detalle.AppendLine($"{c.Numero} · Liquidado {c.Total:N2} · Pagado {c.Pagado:N2} · Saldo {c.Saldo:N2}");
                    foreach (var pago in c.Pagos) detalle.AppendLine($"{pago.Numero} · {pago.Fecha:dd/MM/yyyy HH:mm} · {pago.Importe:N2} · {pago.Medios} · {(pago.Anulado ? "Anulado: " + pago.MotivoAnulacion : "Vigente")}");
                }
                else detalle.AppendLine("Sin liquidación emitida.");
            }
            detalle.AppendLine("\r\nHISTORIAL Y ENTREGA");
            foreach (var h in _atencion.Historial.OrderBy(h => h.Fecha).ThenBy(h => h.IdHistorialAtencion)) detalle.AppendLine($"{h.Fecha:dd/MM/yyyy HH:mm} · {h.Accion} · {h.Observaciones}");
            txtExpediente.Text = detalle.ToString();
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado)
        {
            e.Cancel = true;
            return;
        }

    }

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Detalle de atención");
}

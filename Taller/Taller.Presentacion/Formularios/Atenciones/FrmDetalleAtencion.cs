using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
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
    private readonly EvaluacionServicio _evaluaciones;
    private readonly SesionUsuario _sesion;
    private Atencion? _atencion;
    private bool _ocupado;
    private bool _cerrando;
    private bool _expedienteCargado;
    public FrmDetalleAtencion(int idAtencion, AtencionServicio atencionServicio, SesionUsuario sesionUsuario, EvaluacionServicio evaluaciones, IServiceProvider proveedor)
    {
        InitializeComponent();
        _proveedor = proveedor;
        _id = idAtencion;
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
        btnProgreso.Enabled = false;
        lblResumen.Text = "Cargando ingreso de la atención… Puede cerrar esta consulta.";
        try
        {
            using var alcance = _proveedor.CreateScope();
            _atencion = await MedirAsync("Ingreso", () => alcance.ServiceProvider.GetRequiredService<AtencionServicio>().ObtenerPorIdAsync(_id)) ?? throw new InvalidOperationException("La atención no existe.");
            if (_cerrando || IsDisposed) return;
            lblResumen.Text = $"Atención {_id:D6} · {TextoAtencion.Estado(_atencion.Estado)} · Solicitud: {TextoAtencion.Solicitud(_atencion.TipoSolicitud)}\n{_atencion.Cliente.Apellido}, {_atencion.Cliente.Nombre} · {_atencion.Vehiculo.Dominio} · Apertura {_atencion.FechaApertura:dd/MM/yyyy HH:mm}";
            txtMotivo.Text = (_atencion.Solicitudes.Count == 0 ? _atencion.MotivoConsulta : string.Join(Environment.NewLine, _atencion.Solicitudes.OrderBy(s => s.Orden).Select(s => $"{s.Orden}. {(s.Tipo == Taller.Dominio.Enumeraciones.TipoSolicitudIngreso.TrabajoSolicitado ? "Solicita" : "Informa")}: {s.Descripcion}")))
                + $"\r\nKilometraje de ingreso: {_atencion.KilometrajeRecepcion?.ToString("N0") ?? "No informado"}\r\nObservaciones: {_atencion.ObservacionesRecepcion}";
            btnProgreso.Enabled = true;

        }
        catch (Exception ex)
        {
            if (!_cerrando && !IsDisposed) Error(ex);
        }
        finally
        {
            _ocupado = false;
            if (!_cerrando && !IsDisposed && _atencion != null && tabs.SelectedIndex == 1)
                await CargarExpedienteAsync();
        }
    }

    private async void SeleccionarPestana(object? sender, EventArgs e)
    {
        if (tabs.SelectedIndex == 1) await CargarExpedienteAsync();
    }

    private async Task CargarExpedienteAsync()
    {
        if (_ocupado || _atencion == null || _expedienteCargado || _cerrando) return;
        _ocupado = true;
        btnProgreso.Enabled = false;
        txtExpediente.Text = "Cargando expediente… Puede consultar el ingreso o cerrar esta ventana.";
        try
        {
            // La consulta conserva su propio contexto hasta terminar, incluso si se cierra
            // la ventana. Nunca deja una operación pendiente en el contexto del padre.
            using var alcance = _proveedor.CreateScope();
            var proveedor = alcance.ServiceProvider;
            bool recepcion = _sesion.UsuarioActual?.RolId == RolesSistema.AdministrativoId;
            var documentos = (await MedirAsync("Presupuestos de la atención", () => proveedor.GetRequiredService<EvaluacionServicio>().ListarPresupuestosAsync(_id))).OrderBy(p => p.IdPresupuesto).ToList();
            if (_cerrando || IsDisposed) return;
            var seguimiento = await MedirAsync("Seguimiento", () => proveedor.GetRequiredService<SeguimientoTrabajoServicio>().ObtenerAsync(_id));
            if (_cerrando || IsDisposed) return;
            var detalle = new System.Text.StringBuilder();
            detalle.AppendLine($"ATENCIÓN {_id:D6} · {TextoAtencion.Estado(_atencion.Estado)}\r\nCliente: {_atencion.Cliente.Apellido}, {_atencion.Cliente.Nombre} · Vehículo: {_atencion.Vehiculo.Dominio}");
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
                var cuenta = await MedirAsync("Cuenta", () => proveedor.GetRequiredService<CobroServicio>().ObtenerAsync(_id));
                if (_cerrando || IsDisposed) return;
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
            if (_cerrando || IsDisposed) return;
            _expedienteCargado = true;
            txtExpediente.Text = detalle.ToString();
        }
        catch (Exception ex)
        {
            if (!_cerrando && !IsDisposed)
            {
                txtExpediente.Text = "No se pudo cargar el expediente. Vuelva a seleccionar esta pestaña para reintentar.";
                Error(ex);
            }
        }
        finally
        {
            _ocupado = false;
            if (!_cerrando && !IsDisposed) btnProgreso.Enabled = true;
        }
    }

    private async Task<T> MedirAsync<T>(string consulta, Func<Task<T>> cargar)
    {
        var reloj = Stopwatch.StartNew();
        try { return await cargar(); }
        finally
        {
            Serilog.Log.Information("Consulta de detalle {Consulta}; atención {IdAtencion}; duración {DuracionMs} ms",
                consulta, _id, reloj.ElapsedMilliseconds);
        }
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e) => _cerrando = true;

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Detalle de atención");
}

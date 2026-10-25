using System.ComponentModel;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Atenciones;
/// <summary>Consulta la recepción y prepara servicios iniciales opcionales antes de solicitar evaluación.</summary>
public partial class FrmDetalleAtencion : Form
{
    private readonly IServiceProvider _proveedor;
    private readonly int _id;
    private readonly AtencionServicio _atenciones;
    private readonly EvaluacionServicio _evaluaciones;
    private readonly SesionUsuario _sesion;
    private Atencion? _atencion;
    private BindingList<ServicioPreparado> _items = [];
    private bool _ocupado;
    private sealed class ServicioPreparado
    {
        public int IdConcepto { get; init; }
        public string Descripcion { get; init; } = "";
        public decimal Cantidad { get; init; }
        public decimal PrecioUnitario { get; init; }
        public decimal Subtotal => decimal.Round(Cantidad * PrecioUnitario, 2, MidpointRounding.AwayFromZero);
    }

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
            btnProgreso.Enabled = _atencion.Estado != Taller.Dominio.Enumeraciones.EstadoAtencion.Abierta;
            bool recepcion = _sesion.UsuarioActual?.RolId == RolesSistema.AdministrativoId;
            grpIngreso.Visible = recepcion && _atencion.Estado == Taller.Dominio.Enumeraciones.EstadoAtencion.Abierta;
            btnConfirmarIngreso.Visible = grpIngreso.Visible;
            dgvServicios.DataSource = null;
            if (grpIngreso.Visible)
            {
                cboServicioIngreso.DisplayMember = nameof(ServicioIngresoDto.Nombre);
                cboServicioIngreso.DataSource = await _atenciones.ListarServiciosDisponiblesParaIngresoAsync();
                dgvServicios.DataSource = _items;
            }
            else
            {
                var presupuestos = await _evaluaciones.ListarPresupuestosAsync();
                dgvServicios.DataSource = presupuestos.Where(p => p.IdAtencion == _id && p.Tipo == Taller.Dominio.Enumeraciones.TipoPresupuesto.Diagnostico).SelectMany(p => p.Items).ToList();
            }

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
            EstadoAutorizacion(null, EventArgs.Empty);
            AjustarConsulta(recepcion && _atencion.Estado == Taller.Dominio.Enumeraciones.EstadoAtencion.Abierta, dgvServicios.Rows.Count > 0);
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

    /// <summary>En una consulta sin cargos iniciales muestra los pedidos, no secciones vacías de ingreso.</summary>
    private void AjustarConsulta(bool prepararIngreso, bool tieneServicios)
    {
        bool mostrarServicios = prepararIngreso || tieneServicios;
        tlpPrincipal.RowStyles[1].SizeType = mostrarServicios ? SizeType.Absolute : SizeType.Percent;
        tlpPrincipal.RowStyles[1].Height = 100;
        tlpPrincipal.RowStyles[2].SizeType = SizeType.Absolute;
        tlpPrincipal.RowStyles[2].Height = prepararIngreso ? 180 : 0;
        tlpPrincipal.RowStyles[3].SizeType = mostrarServicios ? SizeType.Percent : SizeType.Absolute;
        tlpPrincipal.RowStyles[3].Height = mostrarServicios ? 100 : 0;
        tlpPrincipal.RowStyles[4].SizeType = SizeType.Absolute;
        tlpPrincipal.RowStyles[4].Height = mostrarServicios ? 46 : 0;
        dgvServicios.Visible = lblTotal.Visible = mostrarServicios;
        if (!prepararIngreso && tieneServicios)
        {
            tlpPrincipal.RowStyles[1].SizeType = SizeType.Percent; tlpPrincipal.RowStyles[1].Height = 65;
            tlpPrincipal.RowStyles[3].SizeType = SizeType.Percent; tlpPrincipal.RowStyles[3].Height = 35;
        }
        Text = prepararIngreso ? "Autorizar inspección inicial" : "Detalle de atención y solicitudes";
    }

    private void ServicioElegido(object? s, EventArgs e)
    {
        if (cboServicioIngreso.SelectedItem is ServicioIngresoDto c)
            nudImporteIngreso.Value = Math.Min(nudImporteIngreso.Maximum, c.PrecioReferencia ?? 0);
    }

    private void ServicioConfirmado(object? s, EventArgs e)
    {
        nudImporteIngreso.Focus();
        nudImporteIngreso.Select(0, nudImporteIngreso.Text.Length);
    }

    private void Agregar(object? s, EventArgs e)
    {
        ValidateChildren();
        if (cboServicioIngreso.SelectedItem is not ServicioIngresoDto c)
            return;
        if (_items.Any(i => i.IdConcepto == c.IdConcepto))
        {
            MessageBox.Show(this, "El servicio ya está agregado. Quite la fila para modificarlo.");
            return;
        }

        _items.Add(new ServicioPreparado { IdConcepto = c.IdConcepto, Descripcion = c.Nombre, Cantidad = nudCantidadIngreso.Value, PrecioUnitario = nudImporteIngreso.Value });
        chkAutorizaIngreso.Checked = false;
        EstadoAutorizacion(null, EventArgs.Empty);
    }

    private void Quitar(object? s, EventArgs e)
    {
        if (dgvServicios.CurrentRow?.DataBoundItem is ServicioPreparado i)
            _items.Remove(i);
        chkAutorizaIngreso.Checked = false;
        EstadoAutorizacion(null, EventArgs.Empty);
    }

    private void EstadoAutorizacion(object? s, EventArgs e)
    {
        chkAutorizaIngreso.Enabled = _items.Count > 0;
        btnConfirmarIngreso.Enabled = _items.Count == 0 || chkAutorizaIngreso.Checked;
        btnConfirmarIngreso.Text = _items.Count == 0 ? "Enviar a taller sin cargos iniciales" : "Autorizar inspección y enviar a taller";
        lblTotal.Text = $"Servicios iniciales: {_items.Sum(i => i.Subtotal):N2}. No equivale a ejecución ni pago. Puede continuar sin agregar servicios.";
    }

    private async void Confirmar(object? s, EventArgs e)
    {
        if (_ocupado || _atencion == null)
            return;
        if (MessageBox.Show(this, _items.Count == 0 ? "Se enviará al técnico sin servicios iniciales cobrables. No autoriza una reparación. ¿Continuar?" : "¿El cliente autorizó los servicios e importes listados? Solo se registrará esa autorización, no su ejecución ni cobro.", "Solicitar evaluación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            if (_items.Count == 0)
                await _evaluaciones.EnviarAEvaluacionAsync(_id, _atencion.TipoSolicitud, _atencion.Version);
            else
                await _atenciones.ConfirmarIngresoAsync(new ConfirmarIngresoDto { IdAtencion = _id, VersionAtencion = _atencion.Version, ClienteAutoriza = chkAutorizaIngreso.Checked, Servicios = _items.Select(i => new ServicioIngresoSeleccionadoDto { IdConcepto = i.IdConcepto, Cantidad = i.Cantidad, PrecioUnitario = i.PrecioUnitario }).ToList() });
            _items.Clear();
            DialogResult = DialogResult.OK;
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

        if (DialogResult == DialogResult.OK)
            Close();
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado)
        {
            e.Cancel = true;
            return;
        }

        if (_items.Count > 0 && MessageBox.Show(this, "¿Descartar los servicios preparados sin confirmar?", "Cerrar atención", MessageBoxButtons.YesNo) != DialogResult.Yes)
            e.Cancel = true;
    }

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Detalle de atención");
}

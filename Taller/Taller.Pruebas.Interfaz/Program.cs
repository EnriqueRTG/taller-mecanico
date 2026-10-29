using System.Reflection;
using Taller.Aplicacion.DTOs.Reportes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Infraestructura;
using Taller.Presentacion;
using Taller.Presentacion.Formularios.Principal;
using Taller.Presentacion.Formularios.Sistema;
using Taller.Presentacion.Formularios.Usuarios;
using Taller.Presentacion.Formularios.Reportes;

namespace Taller.Pruebas.Interfaz;
/// <summary>Prueba de construcción WinForms en STA. No abre conexión SQL ni sustituye aceptación visual.</summary>
internal static class Program
{
    [STAThread]
    private static int Main()
    {
        ApplicationConfiguration.Initialize();
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string?> {
            ["ConnectionStrings:Default"]="Server=localhost;Database=SGTM_PruebaConstruccion;Integrated Security=True;Encrypt=True",
            ["Respaldos:Habilitado"]="false"
        }).Build();
        var servicios=new ServiceCollection();
        servicios.AddSingleton<IConfiguration>(config);
        servicios.AddLogging();
        servicios.AddAplicacion().AddInfraestructura(config).AddPresentacion();
        using var proveedor=servicios.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        foreach(int rol in new[]{1,2,3})
        {
            using var scope=proveedor.CreateScope();
            scope.ServiceProvider.GetRequiredService<SesionUsuario>().Iniciar(new Usuario { Id=rol,Nombre="Prueba",Apellido="Interfaz",RolId=rol,Rol=new Rol { Id=rol,Nombre=RolesSistema.ObtenerNombreEsperado(rol)!,Activo=true } });
            foreach(var tipo in new[]{typeof(FrmPrincipal),typeof(FrmInicio),typeof(FrmAuditoriaAccesos),typeof(FrmRespaldoRestauracion),typeof(FrmAltaUsuario),typeof(FrmReportes),typeof(Taller.Presentacion.Formularios.Comprobantes.FrmComprobantes)})
            {
                using var formulario=(Form)scope.ServiceProvider.GetRequiredService(tipo);
                formulario.PerformLayout();
                if (formulario is FrmInicio)
                    VerificarTarjetasInicio(formulario);
                if(formulario.Controls.Count==0)throw new InvalidOperationException("Formulario sin controles: "+tipo.Name);
                Console.WriteLine($"Construcción y layout: {tipo.Name}, rol {rol}");
            }
        }
        VerificarSelectoresRespaldos(proveedor);
        VerificarFiltrosReportes(proveedor);
        VerificarAccionesContextuales();
        VerificarFiltrosListados(proveedor);
        VerificarUniformidadListados(proveedor);
        VerificarRecorridoTecnico(proveedor);
        VerificarSolicitudesIngreso(proveedor);
        VerificarContextoHistorial(proveedor);
        VerificarEditorPresupuesto(proveedor);
        VerificarEjecucionYConsulta(proveedor);
        VerificarCobroRestanteYEntrega(proveedor);
        VerificarContextoSeguimiento(proveedor);
        VerificarCantidadConcepto(proveedor);
        VerificarSeleccionRecibos(proveedor);
        VerificarNavegacionDocumento();
        return 0;
    }
    private static void VerificarSelectoresRespaldos(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        using var f = scope.ServiceProvider.GetRequiredService<FrmRespaldoRestauracion>();
        f.PerformLayout();
        foreach (var nombre in new[] { "btnCarpetaManual", "btnCarpetaAuto" })
        {
            var boton = f.Controls.Find(nombre, true).Single() as Button;
            if (boton is null || boton.Visible) throw new InvalidOperationException("El explorador requiere verificar primero el equipo del servidor.");
        }
        var ruta = f.Controls.Find("txtCarpetaManual", true).Single() as TextBox;
        ruta!.Text = @"C:\SGTM\Respaldos";
        typeof(FrmRespaldoRestauracion).GetMethod("ElegirCarpeta", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(f, [ruta]);
        if (ruta.Text != @"C:\SGTM\Respaldos") throw new InvalidOperationException("Un selector no disponible no debe modificar la ruta escrita.");
        Console.WriteLine("Respaldos: selectores manual/programado y ruta conservada sin servidor local verificado.");
    }

    private static void VerificarFiltrosReportes(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        using var f = scope.ServiceProvider.GetRequiredService<FrmReportes>();
        f.BindingContext = new BindingContext();
        var tipo = typeof(FrmReportes);
        var combo = (ComboBox)tipo.GetField("cboTipo", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(f)!;
        var estados = (ComboBox)tipo.GetField("cboEstado", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(f)!;
        var opcion = tipo.GetNestedType("Opcion", BindingFlags.NonPublic)!;
        combo.DisplayMember = "Nombre";
        foreach (var reporte in Enum.GetValues<TipoReporte>())
            combo.Items.Add(Activator.CreateInstance(opcion, reporte, ReporteServicio.Nombre(reporte))!);
        for (int i = 0; i < combo.Items.Count; i++)
        {
            combo.SelectedIndex = i;
            var esperados = ReporteServicio.EstadosPara((TipoReporte)i).Prepend("Todos").ToArray();
            if (!esperados.SequenceEqual(estados.Items.Cast<string>())) throw new InvalidOperationException("Estados incompletos antes de generar el reporte.");
            estados.SelectedIndex = estados.Items.Count - 1;
            tipo.GetMethod("BtnLimpiar_Click", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(f, [null, EventArgs.Empty]);
            if (estados.SelectedIndex != 0 || !esperados.SequenceEqual(estados.Items.Cast<string>())) throw new InvalidOperationException("Limpiar debe conservar todas las opciones del tipo actual.");
        }
        Console.WriteLine("Reportes: estados completos antes de generar, al cambiar de tipo y después de limpiar.");
    }

    private static void VerificarAccionesContextuales()
    {
        using var panel = new Panel { Enabled = false };
        using var boton = new Button { Enabled = true };
        panel.Controls.Add(boton);
        var metodo = typeof(FrmPrincipal).Assembly.GetType("Taller.Presentacion.Estilos.AccionesContextuales")!.GetMethod("Actualizar")!;
        metodo.Invoke(null, [new Control[] { boton }]);
        panel.Enabled = true;
        if (!boton.Visible) throw new InvalidOperationException("Recargar un panel no debe ocultar una acción que vuelve a estar disponible.");
        boton.Enabled = false; metodo.Invoke(null, [new Control[] { boton }]);
        if (boton.Visible) throw new InvalidOperationException("Una acción sin selección/etapa no debe quedar visible deshabilitada.");
        boton.Enabled = true; metodo.Invoke(null, [new Control[] { boton }]);
        if (!boton.Visible) throw new InvalidOperationException("La acción debe reaparecer al cumplirse la condición.");
        Console.WriteLine("Acciones contextuales: ocultas sin condición y recuperadas después de una recarga.");
    }

    private static void VerificarContextoSeguimiento(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        using var f = new Taller.Presentacion.Formularios.Atenciones.FrmSeguimientoTrabajo(1,
            scope.ServiceProvider.GetRequiredService<SeguimientoTrabajoServicio>(), scope.ServiceProvider.GetRequiredService<EvaluacionServicio>(),
            scope.ServiceProvider.GetRequiredService<PresupuestoServicio>(), scope.ServiceProvider.GetRequiredService<EmpresaDocumentoDto>());
        object Campo(string n) => f.GetType().GetField(n, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(f)!;
        void Aplicar() => f.GetType().GetMethod("ActualizarContexto", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(f, [null, EventArgs.Empty]);
        var datos = new Taller.Aplicacion.DTOs.Ejecuciones.SeguimientoTrabajoDto {
            PuedeRegistrar = true, PuedePausar = true, Estado = Taller.Dominio.Enumeraciones.EstadoAtencion.EnEjecucion };
        f.GetType().GetField("_datos", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(f, datos);
        var tabs = (TabControl)Campo("tabs");
        var panel = (TableLayoutPanel)Campo("panelContexto");
        tabs.SelectedTab = tabs.TabPages["tabAvance"];
        Aplicar();
        if (panel.RowStyles[1].Height != 0) throw new InvalidOperationException("Avance no debe mostrar selector de operaciones.");
        tabs.SelectedTab = tabs.TabPages["tabHallazgos"];
        Aplicar();
        if (panel.RowStyles[1].Height == 0 || ((Button)Campo("btnAdicional")).Enabled)
            throw new InvalidOperationException("Hallazgo requiere alcance contextual y selección para adicional.");
        tabs.SelectedTab = tabs.TabPages["tabMateriales"];
        Aplicar();
        if (panel.RowStyles[1].Height != 0 || ((Button)Campo("btnRecibir")).Enabled)
            throw new InvalidOperationException("Materiales requiere seleccionar su fila.");
        datos.PuedeRegistrar = false;
        tabs.SelectedTab = tabs.TabPages["tabPausas"];
        Aplicar();
        if (panel.RowStyles[1].Height != 0 || !((Button)Campo("btnPausar")).Text.Contains("cliente") || ((Button)Campo("btnResolver")).Enabled)
            throw new InvalidOperationException("Recepción solo debe registrar detención general del cliente.");
        Console.WriteLine("Seguimiento: alcance contextual, selección obligatoria y detención administrativa explícita.");
    }

    private static void VerificarCantidadConcepto(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        using var f = new Taller.Presentacion.Formularios.Diagnosticos.FrmEvaluarAtencion(1,
            scope.ServiceProvider.GetRequiredService<EvaluacionServicio>(), scope.ServiceProvider);
        f.BindingContext = new BindingContext();
        var tipo = f.GetType();
        var combo = (ComboBox)tipo.GetField("cboConcepto", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(f)!;
        var cantidad = (NumericUpDown)tipo.GetField("nudCantidad", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(f)!;
        combo.DataSource = new List<Taller.Aplicacion.DTOs.Atenciones.ConceptoPropuestaDto> {
            new() { IdConcepto = 1, Nombre = "Repuesto", Tipo = Taller.Dominio.Enumeraciones.TipoConcepto.Repuesto },
            new() { IdConcepto = 2, Nombre = "Servicio", Tipo = Taller.Dominio.Enumeraciones.TipoConcepto.Servicio } };
        combo.SelectedIndex = 0;
        cantidad.Value = 7;
        combo.SelectedIndex = 1;
        if (cantidad.Value != 1) throw new InvalidOperationException("Un nuevo concepto debe iniciar cantidad en uno.");
        Console.WriteLine("Presupuesto: cambiar de concepto reinicia cantidad sin alterar filas agregadas.");
    }

    private static void VerificarSeleccionRecibos(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        using var f = new Taller.Presentacion.Formularios.Comprobantes.FrmComprobantePagos(1,
            scope.ServiceProvider.GetRequiredService<CobroServicio>(), scope.ServiceProvider.GetRequiredService<EmpresaDocumentoDto>());
        f.BindingContext = new BindingContext();
        var tipo = f.GetType();
        var cuenta = new Taller.Aplicacion.DTOs.Cobros.CuentaAtencionDto { Estado = Taller.Dominio.Enumeraciones.EstadoAtencion.TrabajoFinalizado,
            Comprobante = new() { Total = 100, Pagado = 100, Pagos = [new() { IdPago = 1, Importe = 100 }] } };
        tipo.GetField("_datos", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(f, cuenta);
        var recibos = (DataGridView)f.Controls.Find("dgvPagos", true).Single();
        var tabs = (TabControl)f.Controls.Find("tabs", true).Single();
        foreach (int pagina in new[] { 0, 1, 0 })
        {
            tabs.SelectedIndex = pagina;
            tipo.GetMethod("Seleccion", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(f, [null, EventArgs.Empty]);
            if (!((Button)f.Controls.Find("btnRecibo", true).Single()).Enabled || !((Button)f.Controls.Find("btnAnular", true).Single()).Enabled)
                throw new InvalidOperationException("Las acciones de recibo no deben depender de visitar su pestaña.");
        }
        cuenta.Comprobante.Pagos[0].Anulado = true;
        tipo.GetMethod("Seleccion", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(f, [null, EventArgs.Empty]);
        if (((Button)f.Controls.Find("btnAnular", true).Single()).Enabled) throw new InvalidOperationException("No debe anular dos veces el recibo.");
        Console.WriteLine("Cuenta: recibo disponible desde ambas pestañas y anulación condicionada al pago vigente.");
    }

    private static void VerificarFiltrosListados(IServiceProvider proveedor)
    {
        foreach (Type tipo in new[] {
            typeof(Taller.Presentacion.Formularios.Clientes.FrmClientes),
            typeof(Taller.Presentacion.Formularios.Vehiculos.FrmVehiculos) })
        {
            using var scope = proveedor.CreateScope();
            using var formulario = (Form)scope.ServiceProvider.GetRequiredService(tipo);
            var grilla = (DataGridView)formulario.Controls.Find("dgvUsuarios", true).Single();
            var buscar = (TextBox)formulario.Controls.Find("txtBuscar", true).Single();
            var estado = (ComboBox)formulario.Controls.Find("cboEstado", true).Single();
            var limpiar = (Button)formulario.Controls.Find("btnLimpiarFiltros", true).Single();
            if (grilla.Columns["colId"]!.Visible)
                throw new InvalidOperationException("El identificador interno no debe mostrarse.");
            foreach (int ancho in new[] { 700, 1020, 1366, 1690 })
            {
                formulario.ClientSize = new Size(ancho, 650);
                Disponer(formulario);
                Control tabla = formulario.Controls.Find("tlpFiltros", true).Single();
                foreach (Control control in new Control[] { buscar, estado, limpiar,
                    formulario.Controls.Find("btnActualizar", true).Single() })
                {
                    Rectangle bounds = control.Bounds;
                    for (Control? padre = control.Parent; padre is not null && padre != tabla; padre = padre.Parent)
                        bounds.Offset(padre.Location);
                    if (bounds.Left < 0 || bounds.Top < 0 || bounds.Right > tabla.Width || bounds.Bottom > tabla.Height)
                        throw new InvalidOperationException($"Filtro recortado: {tipo.Name}, {control.Name}, ancho {ancho}: control {bounds}, tabla {tabla.ClientSize}, contenedor {control.Parent!.Bounds}.");
                }
            }
            buscar.Text = "ZZZ-INEXISTENTE";
            estado.SelectedIndex = 2;
            // Invoca el evento sin mostrar el formulario ni disparar Load/conexiones SQL.
            tipo.GetMethod("BtnLimpiarFiltros_Click", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(formulario, new object?[] { limpiar, EventArgs.Empty });
            if (buscar.Text.Length != 0 || estado.SelectedIndex != 0 || grilla.CurrentCell is not null)
                throw new InvalidOperationException("Limpiar debe restablecer criterios y selección.");
            foreach (string nombre in new[] { "btnEditarUsuario", "btnCambiarEstado" })
                if (formulario.Controls.Find(nombre, true).Single().Enabled)
                    throw new InvalidOperationException("Acción habilitada sin registro seleccionado.");
            Console.WriteLine($"Filtros adaptables y limpieza sin conexión SQL: {tipo.Name}");
        }
    }

    private static void VerificarCobroRestanteYEntrega(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        using var pago = new Taller.Presentacion.Formularios.Comprobantes.FrmRegistrarPago(new Taller.Aplicacion.DTOs.Cobros.ComprobanteDto { Total = 19520.15m }, scope.ServiceProvider.GetRequiredService<CobroServicio>());
        var medios = (System.ComponentModel.BindingList<Taller.Aplicacion.DTOs.Cobros.PagoMedioDto>)pago.GetType().GetField("_medios", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(pago)!;
        medios.Add(new() { Importe = 19000 });
        pago.GetType().GetMethod("UsarRestante", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(pago, [null, EventArgs.Empty]);
        if (((NumericUpDown)pago.Controls.Find("nudImporte", true).Single()).Value != 520.15m) throw new InvalidOperationException("Usar restante pierde el importe exacto o los centavos.");
        using var entrega = new Taller.Presentacion.Formularios.Comprobantes.FrmRegistrarEntrega("Ana Pérez", "Trabajos finalizados");
        if (entrega.PersonaRetira != "Ana Pérez" || entrega.Conformidad) throw new InvalidOperationException("La entrega debe proponer al cliente sin afirmar conformidad por él.");
        using var ejecucion = new Taller.Presentacion.Formularios.Atenciones.FrmEjecutarAtencion(1,
            scope.ServiceProvider.GetRequiredService<EjecucionServicio>(), scope.ServiceProvider.GetRequiredService<SesionUsuario>(), scope.ServiceProvider);
        var iniciar = ejecucion.Controls.Find("btnTomar", true).Single();
        var acciones = ejecucion.Controls.Find("flpAcciones", true).Single();
        if (iniciar.Parent == acciones) throw new InvalidOperationException("Iniciar ejecución debe preceder a la lista, fuera del grupo inferior.");
        Console.WriteLine("Cobros: restante exacto; retiro prefijado sin conformidad automática; inicio de ejecución separado.");
    }

    private static void VerificarEjecucionYConsulta(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        var sesion = scope.ServiceProvider.GetRequiredService<SesionUsuario>();
        sesion.Iniciar(new Usuario { Id = 1, RolId = RolesSistema.TecnicoId });
        using var formulario = new Taller.Presentacion.Formularios.Atenciones.FrmEjecutarAtencion(1,
            scope.ServiceProvider.GetRequiredService<EjecucionServicio>(), sesion, scope.ServiceProvider);
        var tipo = formulario.GetType();
        var item = new Taller.Aplicacion.DTOs.Ejecuciones.EjecucionItemDto { IdDetalle = 1, Descripcion = "Trabajo", Estado = Taller.Dominio.Enumeraciones.EstadoEjecucionDetalle.NoIniciado, CantidadAutorizada = 2 };
        var lista = new System.ComponentModel.BindingList<Taller.Aplicacion.DTOs.Ejecuciones.EjecucionItemDto>([item]);
        tipo.GetField("_cargando", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(formulario, true);
        tipo.GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(formulario, lista);
        tipo.GetField("_datos", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(formulario,
            new Taller.Aplicacion.DTOs.Ejecuciones.EjecucionAtencionDto { IdTecnico = 1, Estado = Taller.Dominio.Enumeraciones.EstadoAtencion.EnEjecucion });
        formulario.BindingContext = new BindingContext();
        _ = formulario.Handle;
        var grilla = (DataGridView)formulario.Controls.Find("dgvItems", true).Single();
        _ = grilla.Handle;
        var opciones = (System.Collections.IEnumerable)((DataGridViewComboBoxColumn)grilla.Columns["colEstado"]!).DataSource!;
        if (opciones.Cast<object>().Count() != 3) throw new InvalidOperationException("La ejecución debe ofrecer solo tres estados.");
        grilla.DataSource = lista;
        tipo.GetField("_cargando", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(formulario, false);
        item.Estado = Taller.Dominio.Enumeraciones.EstadoEjecucionDetalle.Completado;
        tipo.GetMethod("CeldaCambiada", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario, [grilla, new DataGridViewCellEventArgs(grilla.Columns["colEstado"]!.Index, 0)]);
        if (item.CantidadEjecutada != 2) throw new InvalidOperationException("Completado debe aplicar la cantidad autorizada.");
        item.CantidadEjecutada = 1;
        tipo.GetMethod("CeldaCambiada", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario, [grilla, new DataGridViewCellEventArgs(grilla.Columns["colCantidadEjecutada"]!.Index, 0)]);
        if (item.Estado != Taller.Dominio.Enumeraciones.EstadoEjecucionDetalle.EnCurso) throw new InvalidOperationException("Una cantidad menor debe quedar en curso, sin inventar un cierre parcial.");
        foreach (int ancho in new[] { 820, 1180, 1366 })
        {
            formulario.ClientSize = new Size(ancho, 650); Disponer(formulario);
            VerificarDentroDelFormulario(formulario, formulario.Controls.Find("btnFinalizar", true).Single());
        }
        tipo.GetField("_pausasActivas", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(formulario,
            new List<Taller.Aplicacion.DTOs.Ejecuciones.PausaConsultaDto> { new() { Id = 1, Motivo = "Detención del cliente" } });
        tipo.GetMethod("AplicarPausas", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario, null);
        if (!grilla.ReadOnly || item.CantidadEjecutada != 1 || item.Estado != Taller.Dominio.Enumeraciones.EstadoEjecucionDetalle.EnCurso)
            throw new InvalidOperationException("La pausa debe bloquear sin reemplazar la edición local.");
        tipo.GetField("_pausasActivas", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(formulario,
            new List<Taller.Aplicacion.DTOs.Ejecuciones.PausaConsultaDto>());
        tipo.GetMethod("AplicarPausas", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario, null);
        if (grilla.ReadOnly) throw new InvalidOperationException("La continuidad resuelta debe recuperar la edición propia.");
        sesion.Iniciar(new Usuario { Id = 2, RolId = RolesSistema.AdministrativoId });
        using var bandeja = scope.ServiceProvider.GetRequiredService<Taller.Presentacion.Formularios.Ejecuciones.FrmTrabajosEnCurso>();
        using var seguimiento = new Taller.Presentacion.Formularios.Atenciones.FrmSeguimientoTrabajo(1,
            scope.ServiceProvider.GetRequiredService<SeguimientoTrabajoServicio>(), scope.ServiceProvider.GetRequiredService<EvaluacionServicio>(),
            scope.ServiceProvider.GetRequiredService<PresupuestoServicio>(), scope.ServiceProvider.GetRequiredService<EmpresaDocumentoDto>());
        var progreso = (DataGridView)seguimiento.Controls.Find("dgvProgreso", true).Single();
        if (!progreso.ReadOnly) throw new InvalidOperationException("La consulta de progreso no puede editar la ejecución.");
        var notas = (TextBox)seguimiento.GetType().GetField("txtDescripcion", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(seguimiento)!;
        if (!notas.ReadOnly) throw new InvalidOperationException("La consulta no debe iniciar con campos técnicos editables.");
        Console.WriteLine("Ejecución: estado/cantidad sincronizados; progreso de recepción de solo lectura; acciones dentro de la ventana.");
    }

    private static void VerificarEditorPresupuesto(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        using var formulario = new Taller.Presentacion.Formularios.Diagnosticos.FrmEvaluarAtencion(1,
            scope.ServiceProvider.GetRequiredService<EvaluacionServicio>(), scope.ServiceProvider);
        formulario.BindingContext = new BindingContext();
        formulario.GetType().GetMethod("CargarDatos", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario,
            [new Taller.Aplicacion.DTOs.Atenciones.EvaluacionAtencionDto {
                TipoSolicitud = Taller.Dominio.Enumeraciones.TipoSolicitudAtencion.ServicioDefinido,
                Items = Enumerable.Range(1, 20).Select(n => new Taller.Aplicacion.DTOs.Atenciones.ItemPropuestaDto {
                    IdConcepto = n, Descripcion = "Ítem de presupuesto " + n, Cantidad = 1, PrecioUnitario = n * 100 }).ToList()
            }]);
        // TabControl calcula el área de sus páginas con el control nativo creado.
        // Crear handles no muestra la ventana ni dispara Load/conexiones SQL.
        _ = formulario.Handle;
        var pestanas = (TabControl)formulario.Controls.Find("tabs", true).Single();
        _ = pestanas.Handle;
        _ = pestanas.SelectedTab!.Handle;
        var grilla = (DataGridView)formulario.Controls.Find("dgvItems", true).Single();
        if (grilla.ReadOnly || grilla.Columns["colCantidad"]!.ReadOnly || grilla.Columns["colPrecioUnitario"]!.ReadOnly
            || !grilla.Columns["colSubtotal"]!.ReadOnly || grilla.ScrollBars != ScrollBars.Both)
            throw new InvalidOperationException("El presupuesto debe permitir cantidades y precios, sin editar totales calculados.");
        foreach (int ancho in new[] { 820, 1020, 1366 })
        {
            formulario.ClientSize = new Size(ancho, 620);
            Disponer(formulario);
            if (grilla.Height < 160) throw new InvalidOperationException($"Lista de presupuesto demasiado pequeña: {ancho}, {grilla.Height}.");
        }
        Console.WriteLine("Presupuesto: espacio para lista, desplazamiento y columnas editables delimitadas.");
    }

    private static void VerificarContextoHistorial(IServiceProvider proveedor)
    {
        foreach (int rol in new[] { RolesSistema.AdministrativoId, RolesSistema.TecnicoId })
        {
            using var scope = proveedor.CreateScope();
            scope.ServiceProvider.GetRequiredService<SesionUsuario>().Iniciar(new Usuario { Id = rol, RolId = rol });
            using var historial = scope.ServiceProvider.GetRequiredService<Taller.Presentacion.Formularios.Vehiculos.FrmHistorialVehiculo>();
            historial.PrepararConsulta(" aa111bb ");
            var dominio = (string)historial.GetType().GetField("_dominio", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(historial)!;
            if (dominio != "AA111BB") throw new InvalidOperationException("El historial perdió el vehículo seleccionado.");
            try { historial.PrepararConsulta(""); throw new InvalidOperationException("No debe admitirse una consulta sin vehículo."); }
            catch (ArgumentException) { }
        }
        Console.WriteLine("Historial: contexto de dominio explícito y normalizado para recepción y técnico.");
    }

    private static void VerificarSolicitudesIngreso(IServiceProvider proveedor)
    {
        using var editor = new Taller.Presentacion.Formularios.Atenciones.EditorSolicitudes();
        var tipo = (ComboBox)editor.Controls.Find("cboTipo", true).Single();
        var texto = (TextBox)editor.Controls.Find("txtSolicitud", true).Single();
        texto.Text = "Cambiar aceite y filtros";
        editor.GetType().GetMethod("Agregar", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(editor, [null, EventArgs.Empty]);
        tipo.SelectedIndex = 1; texto.Text = "Ruido al frenar";
        if (!editor.ConfirmarPendiente() || editor.Solicitudes.Count != 2)
            throw new InvalidOperationException("El ingreso pierde el pedido escrito o mezcla las solicitudes.");
        if (editor.Solicitudes[0].Tipo == editor.Solicitudes[1].Tipo)
            throw new InvalidOperationException("Los tipos de solicitud no se conservan.");
        using var scope = proveedor.CreateScope();
        using var formulario = scope.ServiceProvider.GetRequiredService<Taller.Presentacion.Formularios.Atenciones.FrmNuevaAtencion>();
        foreach (int ancho in new[] { 820, 1020, 1366 })
        {
            formulario.ClientSize = new Size(ancho, 620);
            Disponer(formulario);
            VerificarDentroDelFormulario(formulario, formulario.Controls.Find("btnRegistrar", true).Single());
            if (formulario.Controls.Find("editorSolicitudes", true).Length != 1)
                throw new InvalidOperationException("La recepción no contiene el editor de solicitudes.");
        }
        Console.WriteLine("Ingreso: pedidos individuales, síntoma independiente y confirmación del texto pendiente.");
    }

    private static void VerificarRecorridoTecnico(IServiceProvider proveedor)
    {
        using var scope = proveedor.CreateScope();
        foreach (bool diagnostico in new[] { false, true })
        {
            using var formulario = new Taller.Presentacion.Formularios.Diagnosticos.FrmEvaluarAtencion(1,
                scope.ServiceProvider.GetRequiredService<EvaluacionServicio>(), scope.ServiceProvider);
            var tipo = formulario.GetType();
            tipo.GetMethod("CargarDatos", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario,
                [new Taller.Aplicacion.DTOs.Atenciones.EvaluacionAtencionDto {
                    IdAtencion = 1,
                    TipoSolicitud = diagnostico ? Taller.Dominio.Enumeraciones.TipoSolicitudAtencion.Problema : Taller.Dominio.Enumeraciones.TipoSolicitudAtencion.ServicioDefinido,
                    IncluyeDiagnostico = diagnostico
                }]);
            var tabs = (TabControl)formulario.Controls.Find("tabs", true).Single();
            if (tabs.TabPages.Count != (diagnostico ? 2 : 1) || tabs.TabPages.ContainsKey("tabIniciales"))
                throw new InvalidOperationException("El recorrido muestra etapas que no corresponden a la solicitud.");
            if (tabs.SelectedTab?.Name != (diagnostico ? "tabInforme" : "tabPropuesta"))
                throw new InvalidOperationException("La apertura técnica no comienza en la tarea correspondiente.");
            var finalizar = (Button)formulario.Controls.Find("btnFinalizar", true).Single();
            if (finalizar.Text != "Enviar a recepción") throw new InvalidOperationException("Acción principal ambigua.");
            foreach (int ancho in new[] { 820, 1020, 1366 })
            {
                formulario.ClientSize = new Size(ancho, 620);
                Disponer(formulario);
                VerificarDentroDelFormulario(formulario, finalizar);
            }
        }
        Console.WriteLine("Recorrido técnico contextual: presupuesto directo, diagnóstico e inspección opcional.");
    }

    private static void VerificarUniformidadListados(IServiceProvider proveedor)
    {
        Type[] tipos = [
            typeof(Taller.Presentacion.Formularios.Diagnosticos.FrmDiagnosticos),
            typeof(Taller.Presentacion.Formularios.Presupuestos.FrmPresupuestos),
            typeof(Taller.Presentacion.Formularios.Comprobantes.FrmComprobantes),
            typeof(FrmUsuarios),
            typeof(Taller.Presentacion.Formularios.Atenciones.FrmAtenciones),
            typeof(Taller.Presentacion.Formularios.Catalogo.FrmCatalogo),
            typeof(FrmAuditoriaAccesos) ];
        foreach (Type tipo in tipos)
        {
            using var scope = proveedor.CreateScope();
            scope.ServiceProvider.GetRequiredService<SesionUsuario>().Iniciar(new Usuario {
                Id = 2, Nombre = "Prueba", Apellido = "Recepción", RolId = 2,
                Rol = new Rol { Id = 2, Nombre = RolesSistema.ObtenerNombreEsperado(2)!, Activo = true } });
            using var formulario = (Form)scope.ServiceProvider.GetRequiredService(tipo);
            string limpiarNombre = tipo.Name == "FrmCatalogo" ? "btnLimpiar" : "btnLimpiarFiltros";
            var limpiar = (Button)formulario.Controls.Find(limpiarNombre, true).Single();
            var actualizar = (Button)formulario.Controls.Find(tipo == typeof(FrmAuditoriaAccesos) ? "consultar" : "btnActualizar", true).Single();
            if (limpiar.FlatStyle != FlatStyle.Flat || actualizar.FlatStyle != FlatStyle.Flat ||
                limpiar.ForeColor != Color.FromArgb(30, 64, 175))
                throw new InvalidOperationException("Estilos de filtros incoherentes: " + tipo.Name);
            foreach (int ancho in new[] { 820, 1020, 1366 })
            {
                formulario.ClientSize = new Size(ancho, 650);
                Disponer(formulario);
                VerificarDentroDelFormulario(formulario, limpiar);
                VerificarDentroDelFormulario(formulario, actualizar);
                if (limpiar.Bounds.IntersectsWith(actualizar.Bounds) && limpiar.Parent == actualizar.Parent)
                    throw new InvalidOperationException("Botones de filtros superpuestos: " + tipo.Name);
            }
            if (tipo != typeof(FrmAuditoriaAccesos) && tipo.Name != "FrmCatalogo")
            {
                var buscar = (TextBox)formulario.Controls.Find("txtBuscar", true).Single();
                var combo = (ComboBox)formulario.Controls.Find("cboEstado", true).Single();
                if (combo.Items.Count == 0) combo.Items.AddRange(["Todos", "Otro"]);
                buscar.Text = "ZZZ-INEXISTENTE";
                combo.SelectedIndex = 1;
                string metodo = "LimpiarFiltros_Click";
                tipo.GetMethod(metodo, BindingFlags.Instance | BindingFlags.NonPublic)!
                    .Invoke(formulario, new object?[] { limpiar, EventArgs.Empty });
                if (buscar.Text != "" || combo.SelectedIndex != 0)
                    throw new InvalidOperationException("No se restablecen los filtros: " + tipo.Name);
                if (tipo.Name == "FrmAtenciones")
                    foreach (string nombre in new[] { "dtpDesde", "dtpHasta" })
                        if (((DateTimePicker)formulario.Controls.Find(nombre, true).Single()).Checked)
                            throw new InvalidOperationException("Limpiar no debe restringir por fecha.");
            }
            Console.WriteLine("Uniformidad, filtros y acciones visibles: " + tipo.Name);
        }
        using var alcanceEdicion = proveedor.CreateScope();
        using var editar = alcanceEdicion.ServiceProvider.GetRequiredService<Taller.Presentacion.Formularios.Clientes.FrmEditarCliente>();
        foreach (Size tamano in new[] { new Size(720, 680), new Size(600, 460), new Size(1000, 700) })
        {
            editar.ClientSize = tamano;
            Disponer(editar);
            foreach (string nombre in new[] { "btnGuardar", "btnCancelar" })
                VerificarDentroDelFormulario(editar, editar.Controls.Find(nombre, true).Single());
        }
        Console.WriteLine("Editar cliente: Guardar y Cancelar visibles en tres tamaños.");
    }

    private static void VerificarDentroDelFormulario(Form formulario, Control control)
    {
        Rectangle bounds = control.Bounds;
        for (Control? padre = control.Parent; padre != null && padre != formulario; padre = padre.Parent)
        {
            if (bounds.Left < 0 || bounds.Top < 0 || bounds.Right > padre.ClientSize.Width || bounds.Bottom > padre.ClientSize.Height)
                throw new InvalidOperationException($"Control recortado: {formulario.Name}, {control.Name}, {bounds}, padre {padre.Name}: {padre.ClientSize}.");
            bounds.Offset(padre.Location);
        }
        if (bounds.Right > formulario.ClientSize.Width || bounds.Bottom > formulario.ClientSize.Height)
            throw new InvalidOperationException("Control fuera del formulario: " + control.Name);
    }

    private static void VerificarNavegacionDocumento()
    {
        using var documento = new System.Drawing.Printing.PrintDocument();
        Type tipo = typeof(FrmReportes).Assembly.GetType("Taller.Presentacion.Impresion.VistaPreviaDocumento")!;
        using var formulario = (Form)Activator.CreateInstance(tipo, documento, (Func<int>)(() => 3))!;
        tipo.GetMethod("ActualizarPaginas", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario, null);
        var pagina = (NumericUpDown)formulario.Controls.Find("nudPagina", true).Single();
        var siguiente = (Button)formulario.Controls.Find("btnSiguiente", true).Single();
        var anterior = (Button)formulario.Controls.Find("btnAnterior", true).Single();
        if (pagina.Maximum != 3 || anterior.Enabled || !siguiente.Enabled)
            throw new InvalidOperationException("No se identifica el total de páginas.");
        pagina.Value = 2;
        if (!anterior.Enabled || !siguiente.Enabled)
            throw new InvalidOperationException("No se puede navegar a la segunda página.");
        pagina.Value = 3;
        if (siguiente.Enabled || !anterior.Enabled)
            throw new InvalidOperationException("La navegación supera la última página.");
        var vista = formulario.Controls.Find("vista", true).Single();
        var tecla = vista.GetType().GetMethod("OnKeyDown", BindingFlags.Instance | BindingFlags.NonPublic)!;
        tecla.Invoke(vista, [new KeyEventArgs(Keys.PageUp)]);
        if (pagina.Value != 2) throw new InvalidOperationException("RePág debe navegar sin usar la barra.");
        tecla.Invoke(vista, [new KeyEventArgs(Keys.Home)]);
        if (pagina.Value != 1) throw new InvalidOperationException("Inicio debe volver a la primera hoja.");
        Console.WriteLine("Vista previa: tres páginas y navegación por teclado sin impresora ni conexión SQL.");
    }
    private static void VerificarTarjetasInicio(Form formulario)
    {
        // Reproduce la regresión de la tercera tarjeta recortada, sin abrir una conexión SQL.
        var documentos = Enumerable.Range(1, 3).Select(i => new ReporteDocumentoDto
        {
            Titulo = "Indicador " + i,
            Indicadores = [new IndicadorReporteDto("Registros", "10")]
        }).ToList();
        typeof(FrmInicio).GetMethod("MostrarTarjetas", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario, [documentos]);
        var tarjetas = (TableLayoutPanel)formulario.Controls.Find("pnlTarjetas", true).Single();
        foreach (int ancho in new[] { 1020, 1366, 1690 })
        {
            formulario.ClientSize = new Size(ancho, 650);
            Disponer(formulario);
            if (tarjetas.Controls.Count != 3)
                throw new InvalidOperationException("Deben mostrarse las tres tarjetas del inicio.");
            Rectangle anterior = Rectangle.Empty;
            foreach (Control tarjeta in tarjetas.Controls)
            {
                if (tarjeta.Width <= 0 || tarjeta.Height <= 0 || tarjeta.Bottom > tarjetas.ClientSize.Height || tarjeta.Right > tarjetas.ClientSize.Width)
                    throw new InvalidOperationException("Tarjeta recortada para ancho " + ancho);
                if (!anterior.IsEmpty && tarjeta.Top != anterior.Top)
                    throw new InvalidOperationException("Una tarjeta saltó a otra fila.");
                if (!anterior.IsEmpty && tarjeta.Bounds.IntersectsWith(anterior))
                    throw new InvalidOperationException("Tarjetas superpuestas.");
                anterior = tarjeta.Bounds;
            }
            Console.WriteLine("Tres tarjetas visibles sin superposición: " + ancho);
        }
    }

    private static void Disponer(Control control)
    {
        control.PerformLayout();
        foreach (Control hijo in control.Controls)
            Disponer(hijo);
    }

}

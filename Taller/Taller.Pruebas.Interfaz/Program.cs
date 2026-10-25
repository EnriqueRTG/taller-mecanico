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
        using var proveedor=servicios.BuildServiceProvider();
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
        VerificarFiltrosListados(proveedor);
        VerificarUniformidadListados(proveedor);
        VerificarRecorridoTecnico(proveedor);
        VerificarSolicitudesIngreso(proveedor);
        VerificarContextoHistorial(proveedor);
        VerificarEditorPresupuesto(proveedor);
        VerificarEjecucionYConsulta(proveedor);
        VerificarNavegacionDocumento();
        return 0;
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
        using var editar = proveedor.GetRequiredService<Taller.Presentacion.Formularios.Clientes.FrmEditarCliente>();
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
        Console.WriteLine("Vista previa: navegación entre tres páginas sin impresora ni conexión SQL.");
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

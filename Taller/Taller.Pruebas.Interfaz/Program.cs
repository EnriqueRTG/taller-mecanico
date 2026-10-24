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

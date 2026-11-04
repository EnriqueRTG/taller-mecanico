using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Formularios.Catalogo;
using Taller.Presentacion.Formularios.Comprobantes;
using Taller.Presentacion.Formularios.Diagnosticos;
using Taller.Presentacion.Formularios.Ejecuciones;
using Taller.Presentacion.Formularios.Presupuestos;

namespace Taller.Pruebas.Interfaz;

/// <summary>Ejercita filtros reales, permisos de consulta y distribución sin disparar Load ni conectar a SQL.</summary>
internal static class PruebasFiltrosBandeja
{
    private const BindingFlags Privado = BindingFlags.Instance | BindingFlags.NonPublic;
    public static void Verificar(IServiceProvider proveedor)
    {
        DateTime dia = new(2026, 8, 15);
        foreach (int rol in new[] { 2, 3 })
        {
            using var scope = proveedor.CreateScope();
            scope.ServiceProvider.GetRequiredService<SesionUsuario>().Iniciar(new Usuario { Id = rol, RolId = rol });
            foreach (Type tipo in new[] { typeof(FrmDiagnosticos), typeof(FrmPresupuestos), typeof(FrmTrabajosEnCurso), typeof(FrmComprobantes), typeof(FrmCatalogo) })
            {
                using var f = (Form)scope.ServiceProvider.GetRequiredService(tipo);
                f.BindingContext = new BindingContext();
                foreach (int ancho in new[] { 820, 1020, 1366, 1690 })
                {
                    f.ClientSize = new Size(ancho, 750);
                    Disponer(f);
                    var tabla = f.Controls.Find("tlpFiltros", true).Single();
                    var editores = tipo == typeof(FrmCatalogo)
                        ? new[] { "txtBuscar", "cmbCategorias", "cmbTiposConcepto", "cboDisponibilidad", "btnLimpiar", "btnActualizar" }
                        : new[] { "txtBuscar", "cboEstado", "dtpDesde", "dtpHasta", "btnLimpiarFiltros", "btnActualizar" };
                    var rectangulos = editores.Select(nombre => (Nombre: nombre, Rect: Relativo(f.Controls.Find(nombre, true).Single(), tabla))).ToArray();
                    foreach (var (nombre, rect) in rectangulos)
                        if (rect.Width < 90 || rect.Height < 20)
                            throw new InvalidOperationException($"Filtro demasiado pequeño: {tipo.Name}/{nombre}, {ancho}: {rect}.");
                        else if (!tabla.ClientRectangle.Contains(rect))
                            throw new InvalidOperationException($"Filtro recortado: {tipo.Name}/{nombre}, {ancho}: {rect}, tabla {tabla.ClientSize}.");
                    for (int i = 0; i < rectangulos.Length; i++)
                        for (int j = i + 1; j < rectangulos.Length; j++)
                            if (rectangulos[i].Rect.IntersectsWith(rectangulos[j].Rect))
                                throw new InvalidOperationException($"Filtros superpuestos: {tipo.Name}, {ancho}.");
                }
                if (tipo == typeof(FrmCatalogo)) continue;
                if (tipo == typeof(FrmDiagnosticos))
                {
                    var estado = (ComboBox)f.Controls.Find("cboEstado", true).Single();
                    if (estado.Items.Contains("Mis actividades") != (rol == 3))
                        throw new InvalidOperationException("Mis actividades solo corresponde al técnico.");
                }
                var desde = (DateTimePicker)f.Controls.Find("dtpDesde", true).Single();
                var hasta = (DateTimePicker)f.Controls.Find("dtpHasta", true).Single();
                if (desde.Checked || hasta.Checked) throw new InvalidOperationException("Las fechas deben comenzar sin restringir.");
                object datos = tipo == typeof(FrmDiagnosticos) ? new List<EvaluacionBandejaDto> {
                    new() { IdAtencion = 1, Fecha = dia.AddDays(-1) }, new() { IdAtencion = 2, Fecha = dia.AddHours(23).AddMinutes(59), Finalizada = true, IdDiagnostico = 20 }, new() { IdAtencion = 3, Fecha = dia.AddDays(1), Finalizada = true } }
                    : tipo == typeof(FrmPresupuestos) ? new List<PresupuestoConsultaDto> {
                    new() { IdPresupuesto = 1, Fecha = dia.AddDays(-1) }, new() { IdPresupuesto = 2, Fecha = dia.AddHours(23).AddMinutes(59) }, new() { IdPresupuesto = 3, Fecha = dia.AddDays(1) } }
                    : tipo == typeof(FrmTrabajosEnCurso) ? new List<TrabajoEnCursoDto> {
                    new() { IdAtencion = 1, Fecha = dia.AddDays(-1) }, new() { IdAtencion = 2, Fecha = dia.AddHours(23).AddMinutes(59) }, new() { IdAtencion = 3, Fecha = dia.AddDays(1) } }
                    : new List<AtencionCobroDto> {
                    new() { IdAtencion = 1, Fecha = dia.AddDays(-1) }, new() { IdAtencion = 2, Fecha = dia.AddHours(23).AddMinutes(59) }, new() { IdAtencion = 3, Fecha = dia.AddDays(1) } };
                tipo.GetField("_datos", Privado)!.SetValue(f, datos);
                ((ComboBox)f.Controls.Find("cboEstado", true).Single()).SelectedIndex = 0;
                void Filtrar() => tipo.GetMethod("Filtrar", Privado)!.Invoke(f, [null, EventArgs.Empty]);
                var grilla = (DataGridView)f.Controls.Find("dgvDatos", true).Single();
                Filtrar();
                if (grilla.Rows.Count != 3) throw new InvalidOperationException("Sin fechas no debe ocultar registros antiguos.");
                desde.Value = hasta.Value = dia;
                desde.Checked = hasta.Checked = true;
                Filtrar();
                if (grilla.Rows.Count != 1) throw new InvalidOperationException("El rango debe incluir el día completo de ambos extremos.");
                hasta.Checked = false;
                Filtrar();
                if (grilla.Rows.Count != 2) throw new InvalidOperationException("Desde debe funcionar sin Hasta.");
                desde.Checked = false; hasta.Checked = true;
                Filtrar();
                if (grilla.Rows.Count != 2) throw new InvalidOperationException("Hasta debe funcionar sin Desde.");
                desde.Value = dia.AddDays(1); desde.Checked = true;
                Filtrar();
                if (grilla.Rows.Count != 0) throw new InvalidOperationException("Un rango invertido no debe ignorarse.");
                tipo.GetMethod(tipo == typeof(FrmTrabajosEnCurso) ? "Limpiar" : "LimpiarFiltros_Click", Privado)!.Invoke(f, [null, EventArgs.Empty]);
                if (desde.Checked || hasta.Checked || grilla.Rows.Count != 3 || grilla.CurrentCell != null)
                    throw new InvalidOperationException("Limpiar debe recuperar todos los registros y quitar la selección.");
                if (tipo == typeof(FrmDiagnosticos))
                {
                    grilla.CurrentCell = grilla.Rows.Cast<DataGridViewRow>().Single(r => r.DataBoundItem is EvaluacionBandejaDto { IdAtencion: 2 }).Cells[0];
                    tipo.GetMethod("Seleccion", Privado)!.Invoke(f, [null, EventArgs.Empty]);
                    if (!f.Controls.Find("btnInforme", true).Single().Enabled)
                        throw new InvalidOperationException("Un informe existente debe ser consultable por recepción y técnico.");
                    if (rol == 2 && f.Controls.Find("btnTrabajar", true).Single().Enabled)
                        throw new InvalidOperationException("Recepción no puede preparar trabajos técnicos.");
                    grilla.CurrentCell = grilla.Rows.Cast<DataGridViewRow>().Single(r => r.DataBoundItem is EvaluacionBandejaDto { IdAtencion: 3 }).Cells[0];
                    tipo.GetMethod("Seleccion", Privado)!.Invoke(f, [null, EventArgs.Empty]);
                    if (f.Controls.Find("btnInforme", true).Single().Enabled)
                        throw new InvalidOperationException("Una preparación sin diagnóstico no debe ofrecer un informe inexistente.");
                    var estado = (ComboBox)f.Controls.Find("cboEstado", true).Single();
                    estado.SelectedItem = "Disponibles";
                    if (grilla.Rows.Count != 1 || grilla.Rows[0].DataBoundItem is not EvaluacionBandejaDto { IdAtencion: 1 })
                        throw new InvalidOperationException("Disponibles debe mantener su significado en ambos roles.");
                    estado.SelectedItem = "Finalizadas";
                    if (grilla.Rows.Count != 2) throw new InvalidOperationException("Finalizadas no debe depender del índice de Mis actividades.");
                }
                Console.WriteLine($"Filtros, fechas inclusivas y distribución: {tipo.Name}, rol {rol}.");
            }
        }
    }
    private static Rectangle Relativo(Control control, Control tabla)
    {
        var rect = control.Bounds;
        for (var padre = control.Parent; padre != null && padre != tabla; padre = padre.Parent) rect.Offset(padre.Location);
        return rect;
    }
    private static void Disponer(Control control)
    {
        control.PerformLayout();
        foreach (Control hijo in control.Controls) Disponer(hijo);
        control.PerformLayout();
    }
}

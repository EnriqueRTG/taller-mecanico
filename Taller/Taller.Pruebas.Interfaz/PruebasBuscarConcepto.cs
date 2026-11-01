using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Formularios.Diagnosticos;

namespace Taller.Pruebas.Interfaz;
internal static class PruebasBuscarConcepto
{
    public static void Verificar()
    {
        var repuesto = new ConceptoPropuestaDto { IdConcepto = 1, Nombre = "Bieleta", Tipo = TipoConcepto.Repuesto, Categoria = "Suspensión", Descripcion = "Por unidad", PrecioReferencia = 30000 };
        var servicio = new ConceptoPropuestaDto { IdConcepto = 2, Nombre = "Reemplazo de bieleta", Tipo = TipoConcepto.Servicio, Categoria = "Suspensión" };
        using var f = new FrmBuscarConcepto([repuesto, servicio,
            new ConceptoPropuestaDto { IdConcepto = 3, Nombre = "Aceite", Tipo = TipoConcepto.Consumible, Categoria = "Motor" }], "BIELETA");
        _ = f.Handle;
        var lista = (DataGridView)f.Controls.Find("dgvConceptos", true).Single();
        _ = lista.Handle;
        var buscar = (TextBox)f.Controls.Find("txtBuscar", true).Single();
        var tipo = (ComboBox)f.Controls.Find("cboTipo", true).Single();
        var categoria = (ComboBox)f.Controls.Find("cboCategoria", true).Single();
        var seleccionar = (Button)f.Controls.Find("btnSeleccionar", true).Single();
        if (lista.RowCount != 2) throw new Exception("La búsqueda inicial debe ignorar mayúsculas.");
        tipo.SelectedItem = TipoConcepto.Repuesto;
        if (lista.RowCount != 1 || !ReferenceEquals(lista.Rows[0].DataBoundItem, repuesto)) throw new Exception("Debe combinar texto y tipo.");
        categoria.SelectedItem = "Motor";
        if (lista.RowCount != 0 || seleccionar.Enabled) throw new Exception("Sin coincidencias no se puede seleccionar.");
        buscar.Clear();
        tipo.SelectedIndex = 0;
        if (lista.RowCount != 1) throw new Exception("Debe filtrar por categoría.");
        categoria.SelectedIndex = 0;
        buscar.Text = "Por unidad";
        if (lista.RowCount != 1) throw new Exception("Debe buscar por descripción.");
        typeof(FrmBuscarConcepto).GetMethod("Seleccionar", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(f, null);
        if (f.DialogResult != DialogResult.OK || !ReferenceEquals(f.ConceptoSeleccionado, repuesto)) throw new Exception("Debe devolver el concepto original.");
        using var vacio = new FrmBuscarConcepto([]);
        if (((Button)vacio.Controls.Find("btnSeleccionar", true).Single()).Enabled) throw new Exception("Catálogo vacío no seleccionable.");
        vacio.Close();
        if (vacio.ConceptoSeleccionado is not null) throw new Exception("Cerrar no debe seleccionar.");
        Console.WriteLine("Selector catálogo: filtros combinados, descripción, vacío y devolución del concepto verificados.");
    }
}

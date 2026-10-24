using Taller.Aplicacion.DTOs.Catalogo;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Navegacion;
using Taller.Presentacion.Errores;

namespace Taller.Presentacion.Formularios.Catalogo;
/// <summary>Consulta comercial persistida; no confunde catálogo disponible con existencias de inventario.</summary>
public partial class FrmCatalogo : Form, IFormularioConOperacion
{
    private readonly ConceptoServicio _servicio;
    private List<ConceptoCatalogoDto> _datos = [];
    private bool _configurando;
    public bool OperacionEnCurso { get; private set; }

    private sealed record Opcion(int Id, string Nombre);
    public FrmCatalogo(ConceptoServicio servicio)
    {
        InitializeComponent();
        _servicio = servicio;
    }

    private async void FormCatalogo_Load(object? sender, EventArgs e) => await CargarAsync();
    private async Task CargarAsync()
    {
        if (OperacionEnCurso)
            return;
        OperacionEnCurso = true;
        tlpPrincipal.Enabled = false;
        _configurando = true;
        try
        {
            var categoria = (cmbCategorias.SelectedItem as Opcion)?.Id ?? 0;
            var tipo = cmbTiposConcepto.SelectedIndex;
            var estado = cboDisponibilidad.SelectedIndex;
            _datos = await _servicio.ListarAsync();
            cmbCategorias.DisplayMember = "Nombre";
            cmbCategorias.DataSource = new[]
            {
                new Opcion(0, "Todas las categorías")
            }.Concat(_datos.Select(d => new Opcion(d.IdCategoria, d.Categoria)).Distinct().OrderBy(c => c.Nombre)).ToList();
            cmbCategorias.SelectedIndex = 0;
            foreach (Opcion c in cmbCategorias.Items)
                if (c.Id == categoria)
                    cmbCategorias.SelectedItem = c;
            cmbTiposConcepto.Items.Clear();
            cmbTiposConcepto.Items.Add("Todos los tipos");
            foreach (var t in Enum.GetValues<TipoConcepto>())
                cmbTiposConcepto.Items.Add(t);
            cmbTiposConcepto.SelectedIndex = tipo >= 0 ? tipo : 0;
            cboDisponibilidad.SelectedIndex = estado >= 0 ? estado : 0;
        }
        catch (Exception ex)
        {
            _datos.Clear();
            ErroresInterfaz.Mostrar(this, ex, "Consultar catálogo");
        }
        finally
        {
            _configurando = false;
            OperacionEnCurso = false;
            tlpPrincipal.Enabled = true;
            AplicarFiltros();
        }
    }

    private void Filtro_Cambio(object? sender, EventArgs e) => AplicarFiltros();
    private void AplicarFiltros()
    {
        if (_configurando)
            return;
        var categoria = (cmbCategorias.SelectedItem as Opcion)?.Id ?? 0;
        dgvConceptos.DataSource = Taller.Aplicacion.Consultas.ConsultaCatalogo.Aplicar(_datos, new FiltroCatalogoDto(txtBuscar.Text, categoria, cmbTiposConcepto.SelectedItem as TipoConcepto? , cboDisponibilidad.SelectedIndex)).ToList();
        dgvConceptos.ClearSelection();
        dgvConceptos.CurrentCell = null;
        txtDetalle.Clear();
        lblCantidad.Text = $"{dgvConceptos.Rows.Count} conceptos encontrados";
    }

    private void Seleccion_Cambio(object? sender, EventArgs e)
    {
        txtDetalle.Text = dgvConceptos.CurrentRow?.DataBoundItem is ConceptoCatalogoDto d ? $"{d.Nombre} · {d.Categoria} · {d.Estado}\r\n{d.Descripcion}\r\nPrecio de referencia ARS: {d.Precio}. Se confirma al preparar la propuesta. El sistema no registra stock físico." : "";
    }

    private async void Actualizar_Click(object? sender, EventArgs e) => await CargarAsync();
    private void Limpiar_Click(object? sender, EventArgs e)
    {
        if (OperacionEnCurso) return;
        _configurando = true;
        txtBuscar.Clear();
        cmbCategorias.SelectedIndex = cmbTiposConcepto.SelectedIndex = cboDisponibilidad.SelectedIndex = 0;
        _configurando = false;
        AplicarFiltros();
        txtBuscar.Focus();
    }

    private void Formulario_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (OperacionEnCurso)
            e.Cancel = true;
    }
}

using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Catalogo;

public partial class FrmCatalogo : Form
{
    private readonly ConceptoServicio _conceptoServicio;
    private readonly CategoriaServicio _categoriaServicio;
    private List<Concepto> _listaConceptosActuales = [];

    public FrmCatalogo(ConceptoServicio conceptoServicio, CategoriaServicio categoriaServicio)
    {
        InitializeComponent();
        _conceptoServicio = conceptoServicio;
        _categoriaServicio = categoriaServicio;
    }

    private async void FormCatalogo_Load(object sender, EventArgs e)
    {
        await CargarCategoriasAsync();
        CargarTiposConcepto();
        await CargarTodosLosConceptosAsync();
    }

    private async Task CargarCategoriasAsync()
    {
        try
        {
            List<Categoria> listaCategorias = await _categoriaServicio.ListarAsync();

            cmbCategorias.DataSource = listaCategorias;
            cmbCategorias.DisplayMember = nameof(Categoria.Nombre);
            cmbCategorias.ValueMember = nameof(Categoria.IdCategoria);
            cmbCategorias.SelectedIndex = -1;
            cmbCategorias.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al cargar las categorías: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        cmbCategorias.Enabled = true;
    }
    private void CargarTiposConcepto()
    {
        var tipos = Enum.GetValues(typeof(TipoConcepto))
        .Cast<TipoConcepto>()
        .ToList();

        cmbTiposConcepto.DataSource = tipos;

        cmbTiposConcepto.SelectedIndex = -1;
    }

    private async Task CargarTodosLosConceptosAsync()
    {
        try
        {
            _listaConceptosActuales = await _conceptoServicio.ListarAsync();
            ActualizarGrilla(_listaConceptosActuales);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar los conceptos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void cmbTiposConcepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        //AplicarFiltrosCombinados();
    }
    private void txtBuscar_TextChanged(object sender, EventArgs e)
    {
        //AplicarFiltroPorTexto();
    }

    private void AplicarFiltroPorTexto()
    {/*
        string textoBusqueda = txtBuscar.Text.Trim().ToLower();

        if (string.IsNullOrEmpty(textoBusqueda))
        {
            dgvConceptos.DataSource = _listaConceptosActuales;
        }
        else
        {
            var filtrados = _listaConceptosActuales
                .Where(c => c.Nombre.ToLower().Contains(textoBusqueda) ||
                           (c.Descripcion != null && c.Descripcion.ToLower().Contains(textoBusqueda)))
                .ToList();

            dgvConceptos.DataSource = filtrados;
        }*/
    }

    /// <summary>
    /// Aplica de forma fluida los filtros de Categoría, Tipo de Concepto y Texto de búsqueda.
    /// </summary>
    private void AplicarFiltrosCombinados()
    {/*
        var query = _listaConceptosActuales.AsEnumerable();

        // Filtrar por Tipo de Concepto 
        if (cmbTiposConcepto.SelectedItem is TipoConcepto tipoSeleccionado)
        {
            query = query.Where(c => c.Tipo == tipoSeleccionado);
        }

        // Filtrar por texto de búsqueda
        string textoBusqueda = txtBuscar.Text.Trim().ToLower();
        if (!string.IsNullOrEmpty(textoBusqueda))
        {
            query = query.Where(c => c.Nombre.ToLower().Contains(textoBusqueda) ||
                                   (c.Descripcion != null && c.Descripcion.ToLower().Contains(textoBusqueda)));
        }

        dgvConceptos.DataSource = query.ToList();*/
    }

    private void ActualizarGrilla(List<Concepto> conceptos)
    {
        dgvConceptos.AutoGenerateColumns = false;
        //dgvConceptos.DataSource = null;
        dgvConceptos.DataSource = conceptos;
        
        if (dgvConceptos.Columns["Categoria"] != null)
            dgvConceptos.Columns["Categoria"].Visible = false;

        if (dgvConceptos.Columns["AtencionesRecepcionadas"] != null)
            dgvConceptos.Columns["AtencionesRecepcionadas"].Visible = false;    
    }
}
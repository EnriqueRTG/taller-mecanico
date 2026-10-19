using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Diagnosticos;

/// <summary>
/// Presenta un informe técnico registrado, sin permitir modificaciones.
/// </summary>
public partial class FrmDetalleDiagnostico : Form
{
    private readonly int _idDiagnostico;
    private readonly DiagnosticoServicio? _diagnosticoServicio;

    public FrmDetalleDiagnostico()
    {
        InitializeComponent();
    }

    public FrmDetalleDiagnostico(
        int idDiagnostico,
        DiagnosticoServicio diagnosticoServicio)
        : this()
    {
        if (idDiagnostico <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idDiagnostico));
        }

        _idDiagnostico = idDiagnostico;
        _diagnosticoServicio = diagnosticoServicio
            ?? throw new ArgumentNullException(nameof(diagnosticoServicio));
    }

    private async void FrmDetalleDiagnostico_Load(
        object? sender,
        EventArgs e)
    {
        EstilosInterfaz.AplicarBotonSecundario(btnCerrar);

        try
        {
            if (_diagnosticoServicio is null)
            {
                throw new InvalidOperationException(
                    "Debe seleccionar un diagnóstico para consultarlo.");
            }

            var datos = await _diagnosticoServicio.ObtenerDetalleAsync(
                _idDiagnostico);

            if (IsDisposed)
            {
                return;
            }

            Text = $"Diagnóstico N.º {datos.IdDiagnostico:D6}";

            txtAtencion.Text = datos.IdAtencion.ToString("D6");
            txtFecha.Text = datos.FechaDiagnostico.ToString("dd/MM/yyyy HH:mm");
            txtTecnicoAutor.Text = datos.TecnicoAutor;
            txtTipo.Text = datos.Tipo.ToString();
            txtKilometraje.Text = datos.Kilometraje?.ToString() ?? "No registrado";
            txtFallaDetectada.Text = datos.FallaDetectada;
            txtTrabajoRecomendado.Text = datos.TrabajoRecomendado;
            txtObservaciones.Text = datos.Observaciones ?? "Sin observaciones.";
        }
        catch (Exception ex)
        {
            if (!IsDisposed)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    "Consultar diagnóstico",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                Close();
            }
        }
    }
}
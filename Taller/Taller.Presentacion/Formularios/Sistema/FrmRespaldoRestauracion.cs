using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Sistema;

/// <summary>
/// Simula la creación y restauración de copias de seguridad.
/// </summary>
/// <remarks>
/// Esta versión no ejecuta comandos contra SQL Server. Las operaciones
/// se conservan únicamente en memoria mientras el formulario está abierto.
/// </remarks>
public partial class FrmRespaldoRestauracion : Form
{
    private bool _archivoValidado;

    public FrmRespaldoRestauracion()
    {
        InitializeComponent();
    }

    private void FrmRespaldoRestauracion_Load(object? sender, EventArgs e)
    {
        AplicarEstilos();
        CargarValoresIniciales();
        CargarHistorialDemostrativo();
        ActualizarAccionesRestauracion();
    }

    private void CargarValoresIniciales()
    {
        txtBaseDatos.Text = "SGTM_DB";
        txtCarpeta.Text = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "SGTM",
            "Respaldos");
        txtNombreArchivo.Text = GenerarNombreSugerido();
        lblUltimoRespaldo.Text = "Último respaldo: 20/09/2026 18:30";
    }

    private static string GenerarNombreSugerido()
    {
        return $"SGTM_DB_{DateTime.Now:yyyyMMdd_HHmm}.bak";
    }

    private void CargarHistorialDemostrativo()
    {
        dgvHistorial.Rows.Clear();
        AgregarHistorial(DateTime.Now.AddDays(-3), "Respaldo", "SGTM_DB_20260920_1830.bak", "Administrador", "Correcto");
        AgregarHistorial(DateTime.Now.AddDays(-10), "Respaldo", "SGTM_DB_20260913_1745.bak", "Administrador", "Correcto");
        AgregarHistorial(DateTime.Now.AddDays(-18), "Restauración", "SGTM_DB_20260905_0900.bak", "Administrador", "Correcto");
        dgvHistorial.ClearSelection();
        dgvHistorial.CurrentCell = null;
    }

    private void BtnExaminarCarpeta_Click(object? sender, EventArgs e)
    {
        using var dialogo = new SaveFileDialog
        {
            Title = "Guardar copia de seguridad",
            Filter = "Copia de seguridad de SQL Server (*.bak)|*.bak",
            DefaultExt = "bak",
            AddExtension = true,
            OverwritePrompt = true,
            InitialDirectory = Directory.Exists(txtCarpeta.Text)
                ? txtCarpeta.Text
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            FileName = AsegurarExtensionBak(txtNombreArchivo.Text.Trim())
        };

        if (dialogo.ShowDialog(ObtenerVentanaPropietaria()) == DialogResult.OK)
        {
            txtCarpeta.Text = Path.GetDirectoryName(dialogo.FileName)
                ?? string.Empty;
            txtNombreArchivo.Text = Path.GetFileName(dialogo.FileName);
            lblEstado.Text = "Ubicación y nombre del respaldo seleccionados.";
        }
    }

    private void BtnCrearRespaldo_Click(object? sender, EventArgs e)
    {
        if (!ValidarDatosRespaldo()) return;

        string archivo = AsegurarExtensionBak(txtNombreArchivo.Text.Trim());
        string rutaCompleta = Path.Combine(txtCarpeta.Text.Trim(), archivo);

        DialogResult respuesta = MessageBox.Show(
            this,
            $"¿Desea simular la creación del respaldo?\n\n{rutaCompleta}",
            "Crear respaldo",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes) return;

        AgregarHistorial(DateTime.Now, "Respaldo", archivo, "Administrador actual", "Correcto");
        lblUltimoRespaldo.Text = $"Último respaldo: {DateTime.Now:dd/MM/yyyy HH:mm}";
        lblEstado.Text = "Respaldo simulado correctamente. No se creó un archivo físico.";
        txtNombreArchivo.Text = GenerarNombreSugerido();

        MessageBox.Show(this, "La creación del respaldo fue simulada correctamente.", "Respaldo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private bool ValidarDatosRespaldo()
    {
        if (string.IsNullOrWhiteSpace(txtCarpeta.Text))
        {
            MostrarAdvertencia("Seleccione una carpeta de destino.", btnExaminarCarpeta);
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtNombreArchivo.Text))
        {
            MostrarAdvertencia("Ingrese un nombre para el archivo de respaldo.", txtNombreArchivo);
            return false;
        }
        if (txtNombreArchivo.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            MostrarAdvertencia("El nombre contiene caracteres no permitidos.", txtNombreArchivo);
            return false;
        }
        return true;
    }

    private static string AsegurarExtensionBak(string nombre)
    {
        return nombre.EndsWith(".bak", StringComparison.OrdinalIgnoreCase) ? nombre : $"{nombre}.bak";
    }

    private void BtnExaminarArchivo_Click(object? sender, EventArgs e)
    {
        using var dialogo = new OpenFileDialog
        {
            Title = "Seleccionar copia de seguridad",
            Filter = "Copias de seguridad de SQL Server (*.bak)|*.bak|Todos los archivos (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false
        };

        if (dialogo.ShowDialog(ObtenerVentanaPropietaria()) != DialogResult.OK) return;

        txtArchivoRestaurar.Text = dialogo.FileName;
        FileInfo archivo = new(dialogo.FileName);
        lblDetalleArchivo.Text = $"{archivo.Name} · {FormatearTamano(archivo.Length)} · Modificado {archivo.LastWriteTime:dd/MM/yyyy HH:mm}";
        _archivoValidado = false;
        lblEstado.Text = "Archivo seleccionado. Valídelo antes de restaurar.";
        ActualizarAccionesRestauracion();
    }

    private void BtnValidarArchivo_Click(object? sender, EventArgs e)
    {
        string ruta = txtArchivoRestaurar.Text.Trim();
        if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta))
        {
            MostrarAdvertencia("Seleccione un archivo de respaldo existente.", btnExaminarArchivo);
            return;
        }
        if (!string.Equals(Path.GetExtension(ruta), ".bak", StringComparison.OrdinalIgnoreCase))
        {
            MostrarAdvertencia("El archivo seleccionado debe tener extensión .bak.", btnExaminarArchivo);
            return;
        }

        _archivoValidado = true;
        lblEstado.Text = "Archivo validado para la simulación de restauración.";
        ActualizarAccionesRestauracion();
        MessageBox.Show(this, "El archivo posee un formato válido para la simulación.", "Archivo validado", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnRestaurar_Click(object? sender, EventArgs e)
    {
        if (!_archivoValidado) return;

        DialogResult respuesta = MessageBox.Show(
            this,
            "La restauración reemplazaría la información actual de la base de datos.\n\n¿Confirma que desea continuar con la simulación?",
            "Confirmar restauración",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes) return;

        string nombre = Path.GetFileName(txtArchivoRestaurar.Text);
        AgregarHistorial(DateTime.Now, "Restauración", nombre, "Administrador actual", "Correcto");
        lblEstado.Text = "Restauración simulada correctamente. La base de datos no fue modificada.";
        _archivoValidado = false;
        ActualizarAccionesRestauracion();

        MessageBox.Show(this, "La restauración fue simulada correctamente.", "Restauración", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ActualizarAccionesRestauracion()
    {
        btnValidarArchivo.Enabled = !string.IsNullOrWhiteSpace(txtArchivoRestaurar.Text);
        btnRestaurar.Enabled = _archivoValidado;
    }

    private void AgregarHistorial(DateTime fecha, string tipo, string archivo, string usuario, string resultado)
    {
        dgvHistorial.Rows.Insert(0, fecha.ToString("dd/MM/yyyy HH:mm"), tipo, archivo, usuario, resultado);
        DataGridViewCell celda = dgvHistorial.Rows[0].Cells[colResultado.Index];
        celda.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        celda.Style.ForeColor = resultado == "Correcto" ? Color.FromArgb(21, 128, 61) : Color.FromArgb(185, 28, 28);
        dgvHistorial.ClearSelection();
    }

    private static string FormatearTamano(long bytes)
    {
        return bytes >= 1024 * 1024 ? $"{bytes / 1024d / 1024d:0.##} MB" : $"{Math.Max(1, bytes / 1024d):0.##} KB";
    }

    private void MostrarAdvertencia(string mensaje, Control control)
    {
        MessageBox.Show(this, mensaje, "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        control.Focus();
    }

    private IWin32Window ObtenerVentanaPropietaria()
    {
        return Form.ActiveForm ?? this;
    }

    private void AplicarEstilos()
    {
        EstilosInterfaz.AplicarBotonSecundario(btnExaminarCarpeta);
        EstilosInterfaz.AplicarBotonPrimario(btnCrearRespaldo);
        EstilosInterfaz.AplicarBotonSecundario(btnExaminarArchivo);
        EstilosInterfaz.AplicarBotonSecundario(btnValidarArchivo);
        EstilosInterfaz.AplicarBotonPeligro(btnRestaurar);
        EstilosInterfaz.AplicarGrilla(dgvHistorial);
    }
}

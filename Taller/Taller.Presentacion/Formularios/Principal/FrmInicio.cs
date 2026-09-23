using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Formularios.Principal;

/// <summary>
/// Representa la pantalla inicial informativa mostrada dentro
/// del formulario principal.
/// </summary>
public partial class FrmInicio : Form
{
    private readonly SesionUsuario _sesionUsuario;
    private readonly System.Windows.Forms.Timer _reloj = new();
    private readonly Label _lblHora = new();
    private readonly Label _lblFecha = new();
    private bool _interfazConstruida;

    public FrmInicio(SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        Font = new Font("Segoe UI", 9F);
        AutoScaleMode = AutoScaleMode.Dpi;
        MinimumSize = new Size(760, 540);

        _sesionUsuario = sesionUsuario
            ?? throw new ArgumentNullException(nameof(sesionUsuario));
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        MostrarBienvenida();

        if (!_interfazConstruida)
        {
            ConstruirResumen();
            _interfazConstruida = true;
        }

        ConfigurarReloj();
    }

    private void MostrarBienvenida()
    {
        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            lblBienvenida.Text = "Bienvenido";
            lblRolActual.Text = "Sesión no disponible";
            return;
        }

        lblBienvenida.Text = $"Bienvenido, {usuario.Nombre}";
        lblRolActual.Text = $"Rol: {usuario.Rol.Nombre}";
    }

    private void ConstruirResumen()
    {
        Controls.Clear();
        AutoScroll = true;
        AutoSize = false;

        var principal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(28),
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Color.FromArgb(244, 247, 251)
        };

        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 105));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        principal.Controls.Add(CrearEncabezado(), 0, 0);
        principal.Controls.Add(CrearIndicadores(), 0, 1);
        principal.Controls.Add(CrearSeccionInferior(), 0, 2);

        Controls.Add(principal);
    }

    private Control CrearEncabezado()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(20, 12, 20, 10),
            Margin = new Padding(0, 0, 0, 10)
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));

        var bienvenida = new Panel { Dock = DockStyle.Fill };

        lblBienvenida.AutoSize = true;
        lblBienvenida.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        lblBienvenida.ForeColor = Color.FromArgb(23, 32, 51);
        lblBienvenida.Location = new Point(0, 2);

        lblDescripcionInicio.AutoSize = true;
        lblDescripcionInicio.Font = new Font("Segoe UI", 9.5F);
        lblDescripcionInicio.ForeColor = Color.FromArgb(100, 116, 139);
        lblDescripcionInicio.Text =
            "Resumen informativo de la actividad reciente del taller.";
        lblDescripcionInicio.Location = new Point(2, 42);

        lblRolActual.AutoSize = true;
        lblRolActual.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblRolActual.ForeColor = Color.FromArgb(46, 125, 222);
        lblRolActual.Location = new Point(2, 66);

        bienvenida.Controls.Add(lblBienvenida);
        bienvenida.Controls.Add(lblDescripcionInicio);
        bienvenida.Controls.Add(lblRolActual);

        var fechaHora = new Panel { Dock = DockStyle.Fill };

        _lblHora.AutoSize = true;
        _lblHora.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _lblHora.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        _lblHora.ForeColor = Color.FromArgb(30, 64, 175);
        _lblHora.TextAlign = ContentAlignment.MiddleRight;
        _lblHora.Location = new Point(20, 5);

        _lblFecha.AutoSize = true;
        _lblFecha.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _lblFecha.Font = new Font("Segoe UI", 9.5F);
        _lblFecha.ForeColor = Color.FromArgb(100, 116, 139);
        _lblFecha.TextAlign = ContentAlignment.MiddleRight;
        _lblFecha.Location = new Point(20, 48);

        fechaHora.Controls.Add(_lblHora);
        fechaHora.Controls.Add(_lblFecha);
        fechaHora.Resize += (_, _) =>
        {
            _lblHora.Left = Math.Max(0, fechaHora.ClientSize.Width - _lblHora.Width);
            _lblFecha.Left = Math.Max(0, fechaHora.ClientSize.Width - _lblFecha.Width);
        };

        panel.Controls.Add(bienvenida, 0, 0);
        panel.Controls.Add(fechaHora, 1, 0);

        return panel;
    }

    private Control CrearIndicadores()
    {
        string rol = ObtenerRolNormalizado();
        var indicadores = ObtenerIndicadores(rol);

        var tabla = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = indicadores.Count,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 10)
        };

        for (int indice = 0; indice < indicadores.Count; indice++)
        {
            tabla.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F / indicadores.Count));

            Indicador indicador = indicadores[indice];
            tabla.Controls.Add(
                CrearTarjeta(indicador.Titulo, indicador.Valor, indicador.Detalle),
                indice,
                0);
        }

        return tabla;
    }

    private static Control CrearTarjeta(
        string titulo,
        string valor,
        string detalle)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 10, 0),
            Padding = new Padding(18)
        };

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(100, 116, 139),
            Text = titulo.ToUpperInvariant(),
            Location = new Point(18, 16)
        });

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 64, 175),
            Text = valor,
            Location = new Point(16, 42)
        });

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 8.5F),
            ForeColor = Color.FromArgb(100, 116, 139),
            Text = detalle,
            Location = new Point(19, 92)
        });

        return panel;
    }

    private Control CrearSeccionInferior()
    {
        var tabla = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };

        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        tabla.Controls.Add(CrearCalendario(), 0, 0);
        tabla.Controls.Add(CrearActividadReciente(), 1, 0);

        return tabla;
    }

    private Control CrearCalendario()
    {
        var tabla = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 10, 0),
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 2
        };

        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        tabla.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Text = "Calendario",
            Margin = new Padding(6, 4, 0, 0)
        }, 0, 0);

        var contenedor = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };

        var calendario = new MonthCalendar
        {
            MaxSelectionCount = 1,
            ShowToday = true,
            ShowTodayCircle = true,
            Font = new Font("Segoe UI", 9F)
        };

        contenedor.Controls.Add(calendario);
        contenedor.Resize += (_, _) =>
        {
            calendario.Left = Math.Max(
                0,
                (contenedor.ClientSize.Width - calendario.Width) / 2);
            calendario.Top = Math.Max(
                0,
                (contenedor.ClientSize.Height - calendario.Height) / 2);
        };

        tabla.Controls.Add(contenedor, 0, 1);
        return tabla;
    }

    private Control CrearActividadReciente()
    {
        var tabla = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0),
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 2
        };

        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        tabla.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Text = "Actividad reciente según el rol",
            Margin = new Padding(6, 4, 0, 0)
        }, 0, 0);

        var grilla = new DataGridView
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(6, 0, 6, 6),
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            MultiSelect = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            Font = new Font("Segoe UI", 9F),
            ColumnHeadersHeight = 34,
            ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            RowTemplate = { Height = 32 }
        };

        grilla.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        grilla.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        grilla.ColumnHeadersDefaultCellStyle.Font =
            new Font("Segoe UI", 9F, FontStyle.Bold);

        grilla.Columns.Add("Fecha", "Fecha");
        grilla.Columns.Add("Modulo", "Módulo");
        grilla.Columns.Add("Descripcion", "Descripción");
        grilla.Columns[0].FillWeight = 65;
        grilla.Columns[1].FillWeight = 80;
        grilla.Columns[2].FillWeight = 190;

        foreach (Actividad actividad in ObtenerActividad(ObtenerRolNormalizado()))
        {
            grilla.Rows.Add(
                actividad.Fecha,
                actividad.Modulo,
                actividad.Descripcion);
        }

        grilla.ClearSelection();
        grilla.CurrentCell = null;

        tabla.Controls.Add(grilla, 0, 1);
        return tabla;
    }

    private static Panel CrearPanelSeccion(string titulo)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 10, 0)
        };

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Text = titulo,
            Location = new Point(18, 17)
        });

        return panel;
    }

    private void ConfigurarReloj()
    {
        ActualizarFechaHora();

        _reloj.Interval = 1000;
        _reloj.Tick -= Reloj_Tick;
        _reloj.Tick += Reloj_Tick;
        _reloj.Start();

        Disposed -= FrmInicio_Disposed;
        Disposed += FrmInicio_Disposed;
    }

    private void Reloj_Tick(object? sender, EventArgs e) =>
        ActualizarFechaHora();

    private void FrmInicio_Disposed(object? sender, EventArgs e) =>
        _reloj.Stop();

    private void ActualizarFechaHora()
    {
        DateTime ahora = DateTime.Now;
        _lblHora.Text = ahora.ToString("HH:mm:ss");
        _lblFecha.Text = ahora.ToString(
            "dddd, dd 'de' MMMM 'de' yyyy",
            new System.Globalization.CultureInfo("es-AR"));

        if (_lblHora.Parent is not null)
            _lblHora.Left = Math.Max(0, _lblHora.Parent.ClientSize.Width - _lblHora.Width);

        if (_lblFecha.Parent is not null)
            _lblFecha.Left = Math.Max(0, _lblFecha.Parent.ClientSize.Width - _lblFecha.Width);
    }

    private string ObtenerRolNormalizado()
    {
        return _sesionUsuario.UsuarioActual?.Rol?.Nombre
            .Trim()
            .ToLowerInvariant()
            ?? "administrador";
    }

    private static List<Indicador> ObtenerIndicadores(string rol)
    {
        if (rol.Contains("técnico") || rol.Contains("tecnico"))
        {
            return
            [
                new("Atenciones asignadas", "6", "2 requieren diagnóstico"),
                new("Trabajos en proceso", "3", "Actividad demostrativa"),
                new("Último reporte", "RT-024", "Trabajos técnicos del día"),
                new("Presupuestos recientes", "4", "2 pendientes de decisión")
            ];
        }

        if (rol.Contains("administrativo"))
        {
            return
            [
                new("Atenciones pendientes", "8", "3 esperan asignación"),
                new("Pendientes de entrega", "4", "Actividad demostrativa"),
                new("Último reporte", "RO-018", "Comprobantes y pagos"),
                new("Pagos registrados", "7", "Durante el día")
            ];
        }

        return
        [
            new("Atenciones pendientes", "8", "Resumen general"),
            new("Usuarios activos", "12", "Actividad demostrativa"),
            new("Último reporte", "RG-031", "Ingresos por período"),
            new("Ingresos del mes", "$ 1,2 M", "Valor demostrativo")
        ];
    }

    private static List<Actividad> ObtenerActividad(string rol)
    {
        if (rol.Contains("técnico") || rol.Contains("tecnico"))
        {
            return
            [
                new("Hoy 10:25", "Diagnósticos", "Diagnóstico D-024 registrado"),
                new("Hoy 09:40", "Presupuestos", "Presupuesto P-038 confeccionado"),
                new("Ayer 17:10", "Atenciones", "Atención A-104 finalizada")
            ];
        }

        if (rol.Contains("administrativo"))
        {
            return
            [
                new("Hoy 11:05", "Pagos", "Pago de comprobante C-092 registrado"),
                new("Hoy 10:15", "Comprobantes", "Comprobante C-094 emitido"),
                new("Ayer 16:30", "Entregas", "Vehículo AB345CD entregado")
            ];
        }

        return
        [
            new("Hoy 11:30", "Reportes", "Reporte RG-031 generado"),
            new("Hoy 09:20", "Usuarios", "Usuario administrativo habilitado"),
            new("Ayer 18:00", "Respaldos", "Copia de seguridad simulada")
        ];
    }

    private sealed record Indicador(
        string Titulo,
        string Valor,
        string Detalle);

    private sealed record Actividad(
        string Fecha,
        string Modulo,
        string Descripcion);
}

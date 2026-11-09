using System.Diagnostics;

try
{
    // Sin argumentos, el lanzador conserva el modo de prueba existente.
    var crearBase = args.Length == 1 && args[0] == "--instalacion-inicial";
    if (args.Length != 0 && !crearBase)
    {
        throw new ArgumentException("Uso: SGTM.Setup.exe [--instalacion-inicial]");
    }

    var modoBase = crearBase ? "--crear-base" : "--usar-base-existente";
    var nombreBase = crearBase ? "SGTM_DB" : "SGTM_DB_InstalacionPrueba";

    var paquete = Path.Combine(AppContext.BaseDirectory, "SGTM.Instalador.msi");
    var destino = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
        "SGTM");
    var aplicacion = Path.Combine(destino, "Taller.Presentacion.exe");
    var configuracion = Path.Combine(destino, "appsettings.Local.json");

    if (crearBase && File.Exists(configuracion))
    {
        throw new InvalidOperationException(
            "Este destino ya está configurado. Usá un entorno limpio para la instalación inicial.");
    }

    var claveInicial = crearBase ? LeerClaveInicial() : null;

    // El MSI se distribuye en la misma carpeta que el lanzador.
    if (!File.Exists(paquete))
        throw new FileNotFoundException("Colocá SGTM.Instalador.msi junto a SGTM.Setup.exe.", paquete);

    // Para esta prueba podemos continuar desde la instalación que ya hiciste.
    if (!File.Exists(aplicacion))
    {
        Console.WriteLine($"Instalando SGTM. Conservá esta carpeta en el asistente: {destino}");
        var msiexec = Path.Combine(Environment.SystemDirectory, "msiexec.exe");

        var inicioMsi = new ProcessStartInfo(msiexec)
        {
            UseShellExecute = false,
            Arguments = $"/i \"{paquete}\" TARGETDIR=\"{destino}\" DISABLEADVTSHORTCUTS=1 /norestart"
        };

        using var procesoMsi = Process.Start(inicioMsi)
            ?? throw new InvalidOperationException("No se pudo iniciar Windows Installer.");

        procesoMsi.WaitForExit();
        var codigo = procesoMsi.ExitCode;

        if (codigo == 3010)
            throw new InvalidOperationException("Windows pidió reiniciar. Reiniciá y volvé a abrir el lanzador.");

        if (codigo != 0)
            throw new InvalidOperationException($"El MSI terminó con código {codigo}.");
    }

    if (!File.Exists(aplicacion))
        throw new FileNotFoundException("El ejecutable no quedó en la carpeta indicada.", aplicacion);

    // Conserva la conexión si la instalación ya fue configurada.
    if (File.Exists(configuracion))
    {
        Console.WriteLine($"Ya existe una configuración local en {destino}.");
    }
    else
    {
        // El proceso hereda la cuenta de Windows que ejecuta este lanzador.
        var codigo = Ejecutar(aplicacion, "--preparar-base", modoBase, nombreBase);

        if (codigo != 0 || !File.Exists(configuracion))
            throw new InvalidOperationException($"La preparación de la conexión falló. Código: {codigo}.");

        Console.WriteLine($"Conexión a {nombreBase} configurada correctamente.");
    }

    if (claveInicial is not null)
    {
        var primerInicio = new ProcessStartInfo(aplicacion)
        {
            UseShellExecute = false,
            WorkingDirectory = destino
        };

        // Usa la conexión del JSON recién generado, aunque la consola tenga
        // una cadena SGTM_CONN de desarrollo.
        primerInicio.Environment.Remove("SGTM_CONN");
        primerInicio.Environment.Remove("ConnectionStrings__Default");
        primerInicio.Environment["SGTM_ADMIN_INICIAL_PASSWORD"] = claveInicial;

        using var proceso = Process.Start(primerInicio)
            ?? throw new InvalidOperationException("No se pudo abrir SGTM.");

        Console.WriteLine("SGTM se abrió. Ingresá como admin con la contraseña que elegiste.");
    }
    else
    {
        Console.WriteLine($"Abrí Taller.Presentacion.exe desde {destino} para ingresar.");
    }

    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}

static int Ejecutar(string ejecutable, params string[] argumentos)
{
    var inicio = new ProcessStartInfo(ejecutable) { UseShellExecute = false };

    foreach (var argumento in argumentos)
        inicio.ArgumentList.Add(argumento);

    using var proceso = Process.Start(inicio)
        ?? throw new InvalidOperationException($"No se pudo iniciar {ejecutable}.");

    proceso.WaitForExit();
    return proceso.ExitCode;
}

static string LeerClaveInicial()
{
    Console.Write("Contraseña inicial de admin: ");
    var clave = new System.Text.StringBuilder();

    while (true)
    {
        var tecla = Console.ReadKey(intercept: true);

        if (tecla.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            break;
        }

        if (tecla.Key == ConsoleKey.Backspace)
        {
            if (clave.Length > 0)
                clave.Length--;

            continue;
        }

        if (!char.IsControl(tecla.KeyChar))
            clave.Append(tecla.KeyChar);
    }

    var resultado = clave.ToString();
    if (string.IsNullOrWhiteSpace(resultado))
        throw new ArgumentException("Ingresá una contraseña inicial para admin.");

    return resultado;
}
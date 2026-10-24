using Microsoft.Extensions.Configuration;
using Taller.Aplicacion.DTOs.Reportes;
namespace Taller.Presentacion.Impresion;
/// <summary>Resuelve la identidad configurada y la dibuja sin inventar datos de contacto.</summary>
internal static class EncabezadoEmpresa
{
    public static EmpresaDocumentoDto Cargar(IConfiguration? configuracion)
    {
        using var memoria = new MemoryStream();
        Properties.Resources.LogoTallerMecanico.Save(memoria, System.Drawing.Imaging.ImageFormat.Png);
        string Leer(string clave, int maximo)
        {
            string valor = (configuracion?["Empresa:" + clave] ?? "").Trim();
            if (valor.Length > maximo)
                throw new InvalidOperationException("Revise la longitud de Empresa:" + clave + " en la configuración.");
            return valor;
        }
        var nombre = Leer("Nombre", 100);
        return new EmpresaDocumentoDto
        {
            Nombre = nombre.Length == 0 ? "Taller mecánico" : nombre,
            Direccion = Leer("Direccion", 200), Telefono = Leer("Telefono", 80),
            Correo = Leer("Correo", 150), IdentificacionFiscal = Leer("IdentificacionFiscal", 80),
            LogoPngBase64 = Convert.ToBase64String(memoria.ToArray())
        };
    }

    public static int Dibujar(Graphics g, Rectangle area, EmpresaDocumentoDto empresa)
    {
        using var fuente = new Font("Segoe UI", 9);
        using var nombre = new Font("Segoe UI", 12, FontStyle.Bold);
        const int anchoLogo = 80;
        if (!string.IsNullOrWhiteSpace(empresa.LogoPngBase64))
        {
            using var memoria = new MemoryStream(Convert.FromBase64String(empresa.LogoPngBase64));
            using var logo = Image.FromStream(memoria);
            float escala = Math.Min(72f / logo.Width, 72f / logo.Height);
            g.DrawImage(logo, area.X, area.Y, logo.Width * escala, logo.Height * escala);
        }
        var texto = new RectangleF(area.X + anchoLogo, area.Y, area.Width - anchoLogo, 150);
        float altoNombre = g.MeasureString(empresa.Nombre, nombre, (int)texto.Width).Height;
        g.DrawString(empresa.Nombre, nombre, Brushes.Black, new RectangleF(texto.X, texto.Y, texto.Width, altoNombre));
        float altoContacto = g.MeasureString(empresa.Contacto, fuente, (int)texto.Width).Height;
        g.DrawString(empresa.Contacto, fuente, Brushes.DimGray, new RectangleF(texto.X, texto.Y + altoNombre + 4, texto.Width, altoContacto));
        int alto = (int)Math.Ceiling(Math.Max(76, altoNombre + altoContacto + 12));
        g.DrawLine(Pens.LightGray, area.X, area.Y + alto, area.Right, area.Y + alto);
        return alto + 10;
    }
}

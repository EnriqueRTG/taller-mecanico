using System.Net.Mail;
using System.Text.RegularExpressions;
using Taller.Aplicacion.Excepciones;

namespace Taller.Aplicacion.Validaciones;
/// <summary>Reglas compartidas por alta y edición; los límites coinciden con las columnas de persistencia.</summary>
public static partial class DatosMaestros
{
    public static string Texto(string? valor, int maximo, string campo, bool obligatorio = true)
    {
        var texto = (valor ?? "").Trim();
        if (texto.Length > maximo || obligatorio && texto.Length == 0 || texto.Any(char.IsControl))
            throw new ValidacionException($"{campo}: ingrese {(obligatorio ? "un valor de " : "hasta ")}{maximo} caracteres como máximo, sin caracteres de control.");
        return texto;
    }

    public static string? Opcional(string? valor, int maximo, string campo)
    {
        var t = Texto(valor, maximo, campo, false);
        return t.Length == 0 ? null : t;
    }

    public static string TipoDocumento(string? tipo)
    {
        var t = Texto(tipo, 20, "Tipo de documento").ToUpperInvariant();
        if (t is not ("DNI" or "CUIT" or "CUIL" or "PASAPORTE"))
            throw new ValidacionException("Seleccione DNI, CUIT, CUIL o Pasaporte.");
        return t;
    }

    public static string Documento(string tipo, string? valor)
    {
        var d = Texto(valor, 20, "Documento").ToUpperInvariant();
        if (tipo == "DNI" && (d.Length is < 7 or > 8 || !d.All(char.IsAsciiDigit)))
            throw new ValidacionException("El DNI debe contener 7 u 8 dígitos.");
        if (tipo is "CUIT" or "CUIL")
        {
            d = d.Replace("-", "");
            int[] pesos = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
            if (d.Length != 11 || !d.All(char.IsAsciiDigit))
                throw new ValidacionException("CUIT/CUIL debe contener 11 dígitos.");
            int digito = 11 - pesos.Select((p, i) => p * (d[i] - '0')).Sum() % 11;
            if (digito == 11)
                digito = 0;
            else if (digito == 10)
                digito = 9;
            if (digito != d[10] - '0')
                throw new ValidacionException("El dígito verificador del CUIT/CUIL no es válido.");
        }

        if (tipo == "PASAPORTE" && !d.All(char.IsAsciiLetterOrDigit))
            throw new ValidacionException("El pasaporte admite letras y números.");
        return d;
    }

    /// <summary>Valida un contacto de 7 a 15 dígitos, conservando su presentación y el prefijo internacional opcional.</summary>
    public static string Telefono(string? valor)
    {
        var telefono = Texto(valor, 30, "Teléfono");
        int digitos = telefono.Count(char.IsAsciiDigit);
        if (digitos is < 7 or > 15 ||
            telefono.Any(c => !char.IsAsciiDigit(c) && !" +()-".Contains(c)) ||
            telefono.Count(c => c == '+') > 1 ||
            telefono.Contains('+') && telefono[0] != '+')
            throw new ValidacionException("Ingrese un teléfono de contacto de 7 a 15 dígitos. Puede incluir espacios, guiones, paréntesis y un + inicial.", "telefono");
        return telefono;
    }

    public static string? Email(string? valor)
    {
        var t = Opcional(valor, 120, "Correo");
        if (t != null && (!MailAddress.TryCreate(t, out var a) || a.Address != t))
            throw new ValidacionException("Ingrese un correo válido o déjelo vacío.");
        return t;
    }

    public static string Dominio(string? valor)
    {
        var d = Texto(valor, 10, "Dominio").ToUpperInvariant();
        if (!d.All(char.IsAsciiLetterOrDigit))
            throw new ValidacionException("Ingrese el dominio sin espacios ni guiones, solo letras y números.");
        return d;
    }

    public static void Id(int id)
    {
        if (id <= 0)
            throw new ValidacionException("Seleccione un registro válido.");
    }
}

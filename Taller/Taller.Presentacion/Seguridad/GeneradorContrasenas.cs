using System.Security.Cryptography;
namespace Taller.Presentacion.Seguridad;
/// <summary>Genera claves aleatorias con todas las familias requeridas, sin registrarlas ni copiarlas al portapapeles.</summary>
internal static class GeneradorContrasenas
{
    public static string Generar()
    {
        string[] grupos = ["ABCDEFGHJKLMNPQRSTUVWXYZ", "abcdefghijkmnopqrstuvwxyz", "23456789", "!@#$%"];
        string todos = string.Concat(grupos);
        char[] clave = new char[16];
        for (int i = 0; i < clave.Length; i++)
        {
            string fuente = i < grupos.Length ? grupos[i] : todos;
            clave[i] = fuente[RandomNumberGenerator.GetInt32(fuente.Length)];
        }
        RandomNumberGenerator.Shuffle(clave.AsSpan());
        return new string(clave);
    }
}

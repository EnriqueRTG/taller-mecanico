using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Seguridad;
/// <summary>Cuenta únicamente entrada del usuario en esta aplicación; relojes y consultas automáticas no renuevan la sesión.</summary>
internal sealed class ActividadSesion(SesionUsuario sesion) : IMessageFilter
{
    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg is 0x100 or 0x101 or 0x201 or 0x204 or 0x20A)
            sesion.RegistrarActividad();
        return false;
    }
}

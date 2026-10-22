namespace Taller.Presentacion.Navegacion;
/// <summary>Impide abandonar una pantalla mientras confirma una operación asincrónica.</summary>
public interface IFormularioConOperacion
{
    bool OperacionEnCurso { get; }
}

using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Catalogo;
/// <summary>Filtros combinables: disponibilidad 0=todos, 1=habilitados, 2=no disponibles, 3=servicios de ingreso.</summary>
public sealed record FiltroCatalogoDto(string Busqueda = "", int IdCategoria = 0, TipoConcepto? Tipo = null, int Disponibilidad = 0);

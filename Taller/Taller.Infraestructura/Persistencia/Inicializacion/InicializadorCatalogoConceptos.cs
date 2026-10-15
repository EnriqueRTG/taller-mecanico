using Microsoft.EntityFrameworkCore;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Infraestructura.Persistencia.Inicializacion;

/// <summary>
/// Carga las categorías y los conceptos del catálogo inicial.
/// </summary>
/// <remarks>
/// Agrega únicamente registros faltantes.
/// Conserva precios, descripciones y estados de los registros existentes.
///
/// Debe ejecutarse durante la preparación de la base de datos,
/// bajo el mecanismo de inicialización de EF Core.
/// </remarks>
public static class InicializadorCatalogoConceptos
{
    /// <summary>
    /// Incorpora el catálogo inicial mediante un único guardado.
    /// </summary>
    public static async Task InicializarAsync(
        TallerDbContext contexto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contexto);

        await AgregarCategoriaAsync(
            contexto,
            "Diagnóstico e inspección",
            "Servicios de ingreso, inspección y evaluación técnica.",
            [
                (TipoConcepto.Servicio,
                    "Ingreso e inspección diagnóstica inicial",
                    "Servicio mínimo de ingreso e inspección inicial. " +
                    "No autoriza reparaciones."),

                (TipoConcepto.Servicio,
                    "Diagnóstico electrónico",
                    "Evaluación mediante herramientas de diagnóstico electrónico."),

                (TipoConcepto.Servicio,
                    "Evaluación de falla adicional",
                    "Evaluación adicional cuyo cobro requiere autorización expresa."),

                (TipoConcepto.Servicio,
                    "Inspección del sistema de frenos",
                    "Evaluación del sistema de frenos. No incluye reparaciones."),

                (TipoConcepto.Servicio,
                    "Inspección del sistema de refrigeración",
                    "Evaluación del sistema de refrigeración. No incluye reparaciones.")
            ],
            cancellationToken);

        await AgregarCategoriaAsync(
            contexto,
            "Mantenimiento y lubricación",
            "Servicios y materiales de mantenimiento periódico.",
            [
                (TipoConcepto.Servicio,
                    "Cambio de aceite y filtro",
                    "Mano de obra. No incluye aceite ni filtro."),

                (TipoConcepto.Servicio,
                    "Reemplazo de filtro de aire",
                    "Mano de obra. No incluye el filtro."),

                (TipoConcepto.Repuesto,
                    "Filtro de aceite",
                    "Por unidad. Especificar aplicación en el presupuesto."),

                (TipoConcepto.Repuesto,
                    "Filtro de aire",
                    "Por unidad. Especificar aplicación en el presupuesto."),

                (TipoConcepto.Consumible,
                    "Aceite de motor",
                    "Por litro. Especificar viscosidad y especificación " +
                    "en el presupuesto."),

                (TipoConcepto.Servicio,
                    "Reemplazo de filtro de combustible",
                    "Mano de obra. No incluye filtro."),

                (TipoConcepto.Servicio,
                    "Reemplazo de filtro de habitáculo",
                    "Mano de obra. No incluye filtro."),

                (TipoConcepto.Repuesto,
                    "Filtro de combustible",
                    "Por unidad. Especificar aplicación en el presupuesto."),

                (TipoConcepto.Repuesto,
                    "Filtro de habitáculo",
                    "Por unidad. Especificar aplicación en el presupuesto.")
            ],
            cancellationToken);

        await AgregarCategoriaAsync(
            contexto,
            "Frenos",
            "Servicios, repuestos y consumibles del sistema de frenos.",
            [
                (TipoConcepto.Servicio,
                    "Reemplazo de pastillas delanteras",
                    "Mano de obra por eje delantero. No incluye repuestos."),

                (TipoConcepto.Servicio,
                    "Reemplazo de discos delanteros",
                    "Mano de obra por eje delantero. No incluye repuestos."),

                (TipoConcepto.Servicio,
                    "Purgado de frenos",
                    "Mano de obra. No incluye líquido de frenos."),

                (TipoConcepto.Repuesto,
                    "Juego de pastillas delanteras",
                    "Por juego para el eje delantero. Especificar aplicación."),

                (TipoConcepto.Repuesto,
                    "Disco de freno delantero",
                    "Por unidad. Especificar aplicación."),

                (TipoConcepto.Consumible,
                    "Líquido de frenos",
                    "Por litro. Especificar clasificación en el presupuesto."),

                (TipoConcepto.Servicio,
                    "Reemplazo de zapatas traseras",
                    "Mano de obra por eje trasero. No incluye repuestos."),

                (TipoConcepto.Repuesto,
                    "Juego de zapatas traseras",
                    "Por juego para el eje trasero. Especificar aplicación."),

                (TipoConcepto.Consumible,
                    "Limpiador de frenos",
                    "Por envase. Especificar presentación; presupuestar " +
                    "la fracción prevista y registrar la realmente utilizada.")
            ],
            cancellationToken);

        await AgregarCategoriaAsync(
            contexto,
            "Refrigeración",
            "Servicios y materiales del sistema de refrigeración.",
            [
                (TipoConcepto.Servicio,
                    "Reemplazo de refrigerante",
                    "Mano de obra. No incluye refrigerante."),

                (TipoConcepto.Servicio,
                    "Reemplazo de termostato",
                    "Mano de obra. No incluye repuestos ni refrigerante."),

                (TipoConcepto.Repuesto,
                    "Termostato",
                    "Por unidad. Especificar aplicación."),

                (TipoConcepto.Repuesto,
                    "Manguera de refrigeración",
                    "Por unidad. Especificar ubicación y aplicación."),

                (TipoConcepto.Consumible,
                    "Refrigerante preparado",
                    "Por litro de mezcla lista para utilizar. " +
                    "Especificar producto y concentración."),

                (TipoConcepto.Servicio,
                    "Reemplazo de bomba de agua",
                    "Mano de obra. No incluye repuestos ni refrigerante."),

                (TipoConcepto.Repuesto,
                    "Bomba de agua",
                    "Por unidad. Especificar aplicación."),

                (TipoConcepto.Repuesto,
                    "Abrazadera de refrigeración",
                    "Por unidad. Especificar medida y aplicación.")
            ],
            cancellationToken);

        await AgregarCategoriaAsync(
            contexto,
            "Suspensión y dirección",
            "Servicios y repuestos de suspensión y dirección.",
            [
                (TipoConcepto.Servicio,
                    "Reemplazo de amortiguadores delanteros",
                    "Mano de obra por par delantero. No incluye repuestos " +
                    "ni alineación."),

                (TipoConcepto.Servicio,
                    "Reemplazo de extremo de dirección",
                    "Mano de obra por unidad. No incluye repuesto ni alineación."),

                (TipoConcepto.Repuesto,
                    "Amortiguador delantero",
                    "Por unidad. Especificar lado y aplicación."),

                (TipoConcepto.Repuesto,
                    "Extremo de dirección",
                    "Por unidad. Especificar lado y aplicación."),

                (TipoConcepto.Servicio,
                    "Reemplazo de bieleta",
                    "Mano de obra por unidad. No incluye repuesto."),

                (TipoConcepto.Repuesto,
                    "Bieleta",
                    "Por unidad. Especificar lado y aplicación."),

                (TipoConcepto.Repuesto,
                    "Cazoleta de amortiguador",
                    "Por unidad. Especificar lado y aplicación.")
            ],
            cancellationToken);

        await AgregarCategoriaAsync(
            contexto,
            "Electricidad y encendido",
            "Servicios y repuestos eléctricos y de encendido.",
            [
                (TipoConcepto.Servicio,
                    "Reemplazo de batería",
                    "Mano de obra. No incluye batería."),

                (TipoConcepto.Servicio,
                    "Reemplazo de bujías",
                    "Mano de obra por juego del motor. No incluye bujías."),

                (TipoConcepto.Repuesto,
                    "Batería",
                    "Por unidad. Especificar capacidad y aplicación."),

                (TipoConcepto.Repuesto,
                    "Bujía",
                    "Por unidad. Especificar referencia y aplicación."),

                (TipoConcepto.Servicio,
                    "Reemplazo de bobina de encendido",
                    "Mano de obra por unidad. No incluye repuesto."),

                (TipoConcepto.Repuesto,
                    "Bobina de encendido",
                    "Por unidad. Especificar referencia y aplicación."),

                (TipoConcepto.Repuesto,
                    "Lámpara de faro",
                    "Por unidad. Especificar tipo, potencia y aplicación.")
            ],
            cancellationToken);

        await AgregarCategoriaAsync(
            contexto,
            "Transmisión y embrague",
            "Servicios y repuestos de transmisión y embrague.",
            [
                (TipoConcepto.Servicio,
                    "Reemplazo de kit de embrague",
                    "Mano de obra. No incluye kit ni otros materiales."),

                (TipoConcepto.Repuesto,
                    "Kit de embrague",
                    "Por kit. Especificar componentes incluidos y aplicación."),

                (TipoConcepto.Servicio,
                    "Reemplazo de aceite de transmisión",
                    "Mano de obra. No incluye lubricante."),

                (TipoConcepto.Consumible,
                    "Aceite de transmisión",
                    "Por litro. Especificar viscosidad y especificación " +
                    "en el presupuesto.")
            ],
            cancellationToken);

        // EF Core guarda las categorías y sus conceptos relacionados.
        // En SQL Server, este guardado se ejecuta en una transacción.
        await contexto.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Busca una categoría e incorpora los conceptos que todavía no existen.
    /// </summary>
    private static async Task AgregarCategoriaAsync(
        TallerDbContext contexto,
        string nombre,
        string descripcion,
        (TipoConcepto Tipo, string Nombre, string Descripcion)[] conceptos,
        CancellationToken cancellationToken)
    {
        var categoria = await contexto.Categorias
            .SingleOrDefaultAsync(
                c => c.Nombre == nombre,
                cancellationToken);

        if (categoria is null)
        {
            categoria = new Categoria
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Activo = true
            };

            contexto.Categorias.Add(categoria);
        }

        foreach (var dato in conceptos)
        {
            // Una categoría nueva todavía no tiene registros persistidos.
            var existe = categoria.IdCategoria != 0
                && await contexto.Conceptos.AnyAsync(
                    c => c.IdCategoria == categoria.IdCategoria
                        && c.Tipo == dato.Tipo
                        && c.Nombre == dato.Nombre,
                    cancellationToken);

            if (existe)
            {
                continue;
            }

            contexto.Conceptos.Add(new Concepto
            {
                Categoria = categoria,
                Tipo = dato.Tipo,
                Nombre = dato.Nombre,
                Descripcion = dato.Descripcion,
                PrecioReferencia = null,
                Activo = true
            });
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura.Persistencia;
namespace Taller.Demostracion;
/// <summary>Catálogo académico 2026-10: referencias ARS orientativas, sin stock ni compatibilidad automática.</summary>
public static class CatalogoDemostracion
{
    public sealed record Entrada(string Categoria, TipoConcepto Tipo, string Nombre, decimal Precio, string Unidad);
    public static IReadOnlyList<Entrada> Entradas { get; } = new Entrada[]
    {
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Diagnóstico electrónico", 45000m, "por inspección"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Evaluación de falla adicional", 35000m, "por evaluación autorizada"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Inspección mecánica con desarme autorizado", 90000m, "por intervención y alcance acordado"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Prueba de compresión de motor", 55000m, "por prueba"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Prueba de presión del circuito de refrigeración", 35000m, "por prueba"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Inspección del sistema de frenos", 25000m, "por inspección"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Inspección del sistema de refrigeración", 25000m, "por inspección"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Revisión del tren delantero", 25000m, "por revisión"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Diagnóstico de arranque y carga", 35000m, "por diagnóstico"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Inspección de pérdidas de fluidos", 30000m, "por inspección"),
        new("Diagnóstico e inspección", TipoConcepto.Servicio, "Control preventivo de niveles y luces", 25000m, "por control"),
        new("Mantenimiento y lubricación", TipoConcepto.Servicio, "Cambio de aceite y filtro", 25000m, "mano de obra; no incluye materiales"),
        new("Mantenimiento y lubricación", TipoConcepto.Servicio, "Service completo de filtros y lubricación", 55000m, "mano de obra; aceite y filtros separados; no acumular con servicios individuales equivalentes"),
        new("Mantenimiento y lubricación", TipoConcepto.Servicio, "Reemplazo de filtro de aire", 10000m, "mano de obra por filtro"),
        new("Mantenimiento y lubricación", TipoConcepto.Servicio, "Reemplazo de filtro de combustible", 18000m, "mano de obra por filtro"),
        new("Mantenimiento y lubricación", TipoConcepto.Servicio, "Reemplazo de filtro de habitáculo", 12000m, "mano de obra por filtro"),
        new("Mantenimiento y lubricación", TipoConcepto.Servicio, "Cambio de aceite de caja manual", 35000m, "mano de obra; lubricante separado"),
        new("Mantenimiento y lubricación", TipoConcepto.Servicio, "Cambio de aceite de diferencial", 30000m, "mano de obra; lubricante separado"),
        new("Mantenimiento y lubricación", TipoConcepto.Repuesto, "Filtro de aceite", 12000m, "por unidad; indicar motor y aplicación"),
        new("Mantenimiento y lubricación", TipoConcepto.Repuesto, "Filtro de aire", 18000m, "por unidad; indicar motor y aplicación"),
        new("Mantenimiento y lubricación", TipoConcepto.Repuesto, "Filtro de combustible", 22000m, "por unidad; indicar motor y aplicación"),
        new("Mantenimiento y lubricación", TipoConcepto.Repuesto, "Filtro de habitáculo", 18000m, "por unidad; indicar aplicación"),
        new("Mantenimiento y lubricación", TipoConcepto.Repuesto, "Filtro de combustible diésel", 48000m, "por unidad; indicar motor y aplicación"),
        new("Mantenimiento y lubricación", TipoConcepto.Repuesto, "Arandela de tapón de cárter", 1800m, "por unidad"),
        new("Mantenimiento y lubricación", TipoConcepto.Consumible, "Aceite de motor", 14000m, "por litro; indicar viscosidad y norma"),
        new("Mantenimiento y lubricación", TipoConcepto.Consumible, "Aceite de motor semisintético 10W40", 12500m, "por litro; verificar especificación del fabricante"),
        new("Mantenimiento y lubricación", TipoConcepto.Consumible, "Aceite de motor sintético 5W30", 20000m, "por litro; verificar norma del fabricante"),
        new("Mantenimiento y lubricación", TipoConcepto.Consumible, "Aceite de motor sintético 5W40", 19000m, "por litro; verificar norma del fabricante"),
        new("Mantenimiento y lubricación", TipoConcepto.Consumible, "Aceite de transmisión manual 75W80", 24000m, "por litro; verificar especificación"),
        new("Mantenimiento y lubricación", TipoConcepto.Consumible, "Aceite de diferencial 80W90", 20000m, "por litro; verificar especificación"),
        new("Mantenimiento y lubricación", TipoConcepto.Consumible, "Fluido ATF", 28000m, "por litro; verificar especificación de la transmisión"),
        new("Mantenimiento y lubricación", TipoConcepto.Consumible, "Grasa multipropósito", 14000m, "por envase de 400 gramos"),
        new("Frenos", TipoConcepto.Servicio, "Reemplazo de discos delanteros", 65000m, "mano de obra por eje; discos separados"),
        new("Frenos", TipoConcepto.Servicio, "Reemplazo de discos traseros", 70000m, "mano de obra por eje; verificar que el vehículo equipe discos"),
        new("Frenos", TipoConcepto.Servicio, "Reemplazo de pastillas delanteras", 35000m, "mano de obra por eje"),
        new("Frenos", TipoConcepto.Servicio, "Reemplazo de pastillas traseras", 40000m, "mano de obra por eje"),
        new("Frenos", TipoConcepto.Servicio, "Reemplazo de zapatas traseras", 55000m, "mano de obra por eje"),
        new("Frenos", TipoConcepto.Servicio, "Reemplazo de campanas traseras", 50000m, "mano de obra por eje"),
        new("Frenos", TipoConcepto.Servicio, "Reemplazo de cilindro de rueda", 35000m, "mano de obra por rueda; purgado separado si corresponde"),
        new("Frenos", TipoConcepto.Servicio, "Reemplazo de flexible de freno", 30000m, "mano de obra por flexible"),
        new("Frenos", TipoConcepto.Servicio, "Renovación y purgado de líquido de frenos", 40000m, "mano de obra; líquido separado"),
        new("Frenos", TipoConcepto.Servicio, "Reparación de cáliper de freno", 65000m, "mano de obra por cáliper; kit separado"),
        new("Frenos", TipoConcepto.Servicio, "Ajuste de freno de estacionamiento", 25000m, "mano de obra por intervención"),
        new("Frenos", TipoConcepto.Repuesto, "Disco de freno delantero", 65000m, "por unidad; indicar diámetro y aplicación"),
        new("Frenos", TipoConcepto.Repuesto, "Disco de freno trasero", 75000m, "por unidad; verificar compatibilidad y tipo de eje"),
        new("Frenos", TipoConcepto.Repuesto, "Juego de pastillas delanteras", 60000m, "por juego para un eje"),
        new("Frenos", TipoConcepto.Repuesto, "Juego de pastillas traseras", 65000m, "por juego para un eje"),
        new("Frenos", TipoConcepto.Repuesto, "Juego de zapatas traseras", 55000m, "por juego para un eje"),
        new("Frenos", TipoConcepto.Repuesto, "Campana de freno trasera", 85000m, "por unidad"),
        new("Frenos", TipoConcepto.Repuesto, "Cilindro de rueda trasero", 30000m, "por unidad"),
        new("Frenos", TipoConcepto.Repuesto, "Flexible de freno delantero", 26000m, "por unidad"),
        new("Frenos", TipoConcepto.Repuesto, "Flexible de freno trasero", 24000m, "por unidad"),
        new("Frenos", TipoConcepto.Repuesto, "Kit de reparación de cáliper", 35000m, "por cáliper"),
        new("Frenos", TipoConcepto.Repuesto, "Cable de freno de estacionamiento", 42000m, "por unidad"),
        new("Frenos", TipoConcepto.Consumible, "Líquido de frenos DOT 4", 16000m, "por envase de 500 ml; verificar especificación"),
        new("Frenos", TipoConcepto.Consumible, "Limpiador de frenos", 14000m, "por aerosol de 500 ml"),
        new("Refrigeración", TipoConcepto.Servicio, "Reemplazo de termostato", 45000m, "mano de obra; repuesto separado"),
        new("Refrigeración", TipoConcepto.Servicio, "Reemplazo de radiador", 80000m, "mano de obra; repuesto y refrigerante separados"),
        new("Refrigeración", TipoConcepto.Servicio, "Reemplazo de bomba de agua", 110000m, "mano de obra; no duplicar si integra distribución"),
        new("Refrigeración", TipoConcepto.Servicio, "Reemplazo de manguera de refrigeración", 30000m, "mano de obra por manguera"),
        new("Refrigeración", TipoConcepto.Servicio, "Limpieza y renovación de refrigerante", 50000m, "mano de obra; líquido separado"),
        new("Refrigeración", TipoConcepto.Servicio, "Reemplazo de electroventilador", 55000m, "mano de obra"),
        new("Refrigeración", TipoConcepto.Repuesto, "Termostato", 45000m, "por unidad; indicar temperatura y aplicación"),
        new("Refrigeración", TipoConcepto.Repuesto, "Radiador de refrigeración", 220000m, "por unidad; indicar aplicación"),
        new("Refrigeración", TipoConcepto.Repuesto, "Bomba de agua", 85000m, "por unidad"),
        new("Refrigeración", TipoConcepto.Repuesto, "Manguera superior de radiador", 28000m, "por unidad"),
        new("Refrigeración", TipoConcepto.Repuesto, "Manguera inferior de radiador", 30000m, "por unidad"),
        new("Refrigeración", TipoConcepto.Repuesto, "Depósito de expansión", 55000m, "por unidad"),
        new("Refrigeración", TipoConcepto.Repuesto, "Tapa de depósito de refrigerante", 18000m, "por unidad; verificar presión"),
        new("Refrigeración", TipoConcepto.Repuesto, "Electroventilador", 160000m, "por conjunto; verificar aplicación"),
        new("Refrigeración", TipoConcepto.Consumible, "Refrigerante listo para usar", 9500m, "por litro; verificar tecnología y concentración"),
        new("Refrigeración", TipoConcepto.Consumible, "Refrigerante concentrado", 22000m, "por litro; diluir según fabricante"),
        new("Refrigeración", TipoConcepto.Consumible, "Agua desmineralizada", 2500m, "por litro"),
        new("Refrigeración", TipoConcepto.Consumible, "Abrazadera para manguera", 2500m, "por unidad"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Reemplazo de batería", 20000m, "mano de obra; batería separada"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Reemplazo de bujías", 35000m, "mano de obra por juego; bujías separadas"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Reemplazo de bobina de encendido", 30000m, "mano de obra por bobina"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Reemplazo de cables de bujías", 25000m, "mano de obra por juego"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Reparación de conexiones eléctricas", 35000m, "mano de obra por alcance acordado"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Reemplazo de motor de arranque", 65000m, "mano de obra"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Reemplazo de alternador", 70000m, "mano de obra"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Reemplazo de lámpara", 10000m, "mano de obra por lámpara"),
        new("Encendido y electricidad", TipoConcepto.Servicio, "Ajuste y comprobación adicional de conjunto", 25000m, "mano de obra por adicional autorizado"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Batería 12V 60Ah", 185000m, "por unidad; verificar polaridad y dimensiones"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Batería 12V 75Ah", 240000m, "por unidad; verificar polaridad y dimensiones"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Batería 12V 90Ah", 300000m, "por unidad; verificar sistema de carga"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Bujía convencional", 12000m, "por unidad; verificar motor y grado térmico"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Bujía de iridio", 30000m, "por unidad; verificar motor y grado térmico"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Juego de cables de bujías", 55000m, "por juego y aplicación"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Bobina de encendido", 80000m, "por unidad"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Motor de arranque", 260000m, "por unidad; verificar aplicación"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Alternador", 320000m, "por unidad; verificar amperaje"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Lámpara halógena H4", 10000m, "por unidad; verificar potencia"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Lámpara halógena H7", 12000m, "por unidad; verificar potencia"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Terminal de batería", 6000m, "por unidad"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Fusible automotor", 1200m, "por unidad; verificar amperaje"),
        new("Encendido y electricidad", TipoConcepto.Repuesto, "Relé automotor", 6500m, "por unidad; verificar circuito"),
        new("Suspensión y dirección", TipoConcepto.Servicio, "Reemplazo de amortiguadores delanteros", 95000m, "mano de obra por eje; alineación separada"),
        new("Suspensión y dirección", TipoConcepto.Servicio, "Reemplazo de amortiguadores traseros", 65000m, "mano de obra por eje"),
        new("Suspensión y dirección", TipoConcepto.Servicio, "Reemplazo de bieleta", 30000m, "mano de obra por unidad"),
        new("Suspensión y dirección", TipoConcepto.Servicio, "Reemplazo de extremo de dirección", 35000m, "mano de obra por unidad"),
        new("Suspensión y dirección", TipoConcepto.Servicio, "Reemplazo de rótula de suspensión", 40000m, "mano de obra por unidad"),
        new("Suspensión y dirección", TipoConcepto.Servicio, "Reemplazo de bujes de parrilla", 60000m, "mano de obra por lado"),
        new("Suspensión y dirección", TipoConcepto.Servicio, "Reemplazo de rodamiento de rueda", 65000m, "mano de obra por rueda"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Amortiguador delantero", 110000m, "por unidad; indicar lado y aplicación"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Amortiguador trasero", 85000m, "por unidad"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Cazoleta de amortiguador", 45000m, "por unidad"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Bieleta de suspensión", 30000m, "por unidad"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Extremo de dirección", 35000m, "por unidad"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Rótula de suspensión", 40000m, "por unidad"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Buje de parrilla", 20000m, "por unidad"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Rodamiento de rueda delantero", 65000m, "por unidad"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Rodamiento de rueda trasero", 55000m, "por unidad"),
        new("Suspensión y dirección", TipoConcepto.Repuesto, "Fuelle de dirección", 20000m, "por unidad"),
        new("Embrague y transmisión", TipoConcepto.Servicio, "Reemplazo de kit de embrague", 260000m, "mano de obra; kit y fluidos separados"),
        new("Embrague y transmisión", TipoConcepto.Servicio, "Reemplazo de bomba de embrague", 65000m, "mano de obra"),
        new("Embrague y transmisión", TipoConcepto.Servicio, "Reemplazo de cilindro auxiliar de embrague", 65000m, "mano de obra"),
        new("Embrague y transmisión", TipoConcepto.Servicio, "Purgado de circuito de embrague", 35000m, "mano de obra; líquido separado"),
        new("Embrague y transmisión", TipoConcepto.Servicio, "Reemplazo de semieje", 85000m, "mano de obra por lado"),
        new("Embrague y transmisión", TipoConcepto.Servicio, "Reemplazo de fuelle de semieje", 65000m, "mano de obra por lado"),
        new("Embrague y transmisión", TipoConcepto.Repuesto, "Kit de embrague", 260000m, "por kit; indicar motor y caja"),
        new("Embrague y transmisión", TipoConcepto.Repuesto, "Bomba de embrague", 95000m, "por unidad"),
        new("Embrague y transmisión", TipoConcepto.Repuesto, "Cilindro auxiliar de embrague", 105000m, "por unidad"),
        new("Embrague y transmisión", TipoConcepto.Repuesto, "Semieje", 210000m, "por unidad y lado"),
        new("Embrague y transmisión", TipoConcepto.Repuesto, "Fuelle de semieje", 22000m, "por unidad"),
        new("Embrague y transmisión", TipoConcepto.Repuesto, "Soporte de caja de cambios", 80000m, "por unidad"),
        new("Distribución y motor", TipoConcepto.Servicio, "Reemplazo de kit de distribución", 220000m, "mano de obra; definir si incluye bomba de agua"),
        new("Distribución y motor", TipoConcepto.Servicio, "Reemplazo de correa auxiliar", 45000m, "mano de obra"),
        new("Distribución y motor", TipoConcepto.Servicio, "Reemplazo de junta de tapa de válvulas", 65000m, "mano de obra"),
        new("Distribución y motor", TipoConcepto.Servicio, "Reemplazo de soporte de motor", 55000m, "mano de obra por soporte"),
        new("Distribución y motor", TipoConcepto.Servicio, "Limpieza de cuerpo de aceleración", 65000m, "mano de obra; adaptación según aplicación"),
        new("Distribución y motor", TipoConcepto.Repuesto, "Kit de distribución", 180000m, "por kit; verificar motor"),
        new("Distribución y motor", TipoConcepto.Repuesto, "Kit de distribución con bomba de agua", 260000m, "por kit; verificar motor"),
        new("Distribución y motor", TipoConcepto.Repuesto, "Correa auxiliar", 40000m, "por unidad"),
        new("Distribución y motor", TipoConcepto.Repuesto, "Tensor de correa auxiliar", 85000m, "por unidad"),
        new("Distribución y motor", TipoConcepto.Repuesto, "Junta de tapa de válvulas", 35000m, "por unidad"),
        new("Distribución y motor", TipoConcepto.Repuesto, "Soporte de motor", 90000m, "por unidad"),
        new("Distribución y motor", TipoConcepto.Repuesto, "Sensor de temperatura de refrigerante", 35000m, "por unidad"),
        new("Distribución y motor", TipoConcepto.Consumible, "Sellador automotor", 18000m, "por envase; verificar compatibilidad"),
        new("Ruedas y neumáticos", TipoConcepto.Servicio, "Alineación de dirección", 45000m, "por vehículo"),
        new("Ruedas y neumáticos", TipoConcepto.Servicio, "Balanceo de rueda", 10000m, "por rueda"),
        new("Ruedas y neumáticos", TipoConcepto.Servicio, "Montaje de neumático", 8000m, "por rueda"),
        new("Ruedas y neumáticos", TipoConcepto.Servicio, "Rotación de neumáticos", 20000m, "por vehículo"),
        new("Ruedas y neumáticos", TipoConcepto.Servicio, "Reparación de pinchadura", 18000m, "por rueda; sujeta a aptitud del neumático"),
        new("Ruedas y neumáticos", TipoConcepto.Repuesto, "Válvula de neumático", 3500m, "por unidad"),
        new("Ruedas y neumáticos", TipoConcepto.Repuesto, "Neumático 185/65 R15", 170000m, "por unidad; verificar índice de carga y velocidad"),
        new("Ruedas y neumáticos", TipoConcepto.Repuesto, "Neumático 195/55 R16", 220000m, "por unidad; verificar índice de carga y velocidad"),
        new("Ruedas y neumáticos", TipoConcepto.Repuesto, "Neumático 205/55 R16", 230000m, "por unidad; verificar homologación"),
        new("Ruedas y neumáticos", TipoConcepto.Consumible, "Contrapesos de balanceo", 4000m, "por rueda"),
        new("Climatización y accesorios", TipoConcepto.Servicio, "Control de aire acondicionado", 35000m, "por control; carga no incluida"),
        new("Climatización y accesorios", TipoConcepto.Servicio, "Carga de aire acondicionado R134a", 140000m, "servicio con carga según equipo y alcance acordado; no duplicar refrigerante"),
        new("Climatización y accesorios", TipoConcepto.Servicio, "Reemplazo de escobillas limpiaparabrisas", 10000m, "mano de obra por juego"),
        new("Climatización y accesorios", TipoConcepto.Servicio, "Reemplazo de bomba lavaparabrisas", 25000m, "mano de obra"),
        new("Climatización y accesorios", TipoConcepto.Repuesto, "Juego de escobillas delanteras", 35000m, "por juego; verificar medidas"),
        new("Climatización y accesorios", TipoConcepto.Repuesto, "Escobilla trasera", 20000m, "por unidad"),
        new("Climatización y accesorios", TipoConcepto.Repuesto, "Bomba lavaparabrisas", 25000m, "por unidad"),
        new("Climatización y accesorios", TipoConcepto.Consumible, "Líquido lavaparabrisas", 5000m, "por litro"),
    };
    public static async Task CargarAsync(TallerDbContext db)
    {
        if (!db.Database.IsSqlServer() || !db.Database.GetDbConnection().Database.EndsWith("_Demo", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("El catálogo académico solo se carga en una base SQL de demostración terminada en _Demo.");
        var categorias = await db.Categorias.ToDictionaryAsync(c => c.Nombre);
        var conceptos = await db.Conceptos.ToListAsync();
        // El catálogo básico preexistente conserva su identidad; solo el entorno Demo recibe referencias iniciales.
        foreach (var c in conceptos)
            c.PrecioReferencia = c.Tipo == TipoConcepto.Servicio ? 35000m : c.Tipo == TipoConcepto.Repuesto ? 45000m : 12000m;
        foreach (var e in Entradas)
        {
            if (!categorias.TryGetValue(e.Categoria, out var categoria))
            {
                categoria = new Categoria { Nombre = e.Categoria, Descripcion = "Catálogo de demostración: " + e.Categoria, Activo = true };
                db.Categorias.Add(categoria); categorias.Add(e.Categoria, categoria);
            }
            var c = conceptos.FirstOrDefault(c => c.Tipo == e.Tipo && c.Nombre == e.Nombre);
            if (c == null)
            {
                c = new Concepto { Categoria = categoria, Nombre = e.Nombre, Tipo = e.Tipo, Activo = true };
                db.Conceptos.Add(c); conceptos.Add(c);
            }
            c.PrecioReferencia = e.Precio;
            c.Descripcion = $"Referencia académica ARS (2026-10), {e.Unidad}. Confirmar aplicación y precio antes de presupuestar. No acredita existencias.";
        }
        await db.SaveChangesAsync(); db.ChangeTracker.Clear();
    }
}

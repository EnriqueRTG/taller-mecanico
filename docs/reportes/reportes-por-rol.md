# Reportes de gestión y revisión de consistencia

La pantalla **Reportes** genera un documento para tomar decisiones: título, usuario, fecha de generación, alcance, filtros, indicadores, gráfico, interpretación, metodología y registros que sustentan los resultados. Todos se calculan a partir del mismo conjunto filtrado. No contiene datos de ejemplo ni estima importes inexistentes.

## Qué puede consultar cada rol

| Reporte | Administrador | Administrativo | Técnico | Uso y fecha de referencia |
| --- | --- | --- | --- | --- |
| Atenciones recibidas y situación actual | Taller | Taller | — | Cohorte por fecha de apertura; estado actual. Ayuda a revisar el ingreso y las atenciones todavía activas. |
| Decisiones de presupuestos vigentes | Taller | Taller | Autoría propia | Cohorte por creación de presupuestos vigentes, incluidas inspecciones y adicionales; excluye versiones reemplazadas. Aceptación entre las ya decididas, incluyendo aceptación parcial. |
| Cobros y anulaciones por medio de pago | Taller | Taller | — | Cada medio del cobro suma en la fecha del recibo y resta en la fecha de anulación. Ayuda a conciliar caja/banco, no calcula utilidad. |
| Saldos pendientes actuales | Taller | Taller | — | Todas las liquidaciones con saldo positivo, también las antiguas. No usa fechas. Antigüedad desde emisión, no días de mora. |
| Carga técnica actual y trabajo disponible | Taller | — | Reservas propias y disponibles | Todas las evaluaciones y ejecuciones actuales; incluye pendientes de fechas anteriores y excluye reservas ajenas para el técnico. No usa fechas. |
| Actividad técnica registrada | Taller | — | Eventos propios | Acciones técnicas del historial por fecha de evento. Varias acciones pueden corresponder al mismo trabajo. |
| Conceptos realizados registrados | Taller | — | Último registro propio | Ítems con cantidad ejecutada positiva, por fecha de última actualización. Cantidad acumulada y valor a precio acordado; no producción incremental ni ingresos cobrados. |

Los permisos se revalidan contra el usuario activo de la base en cada generación, no solamente al mostrar el menú. Los reportes técnicos no exponen caja ni saldos. La carga disponible no incluye cerradas ni canceladas. La actividad se identifica por la acción histórica, de modo que un cambio posterior de rol no borre el trabajo registrado.

No se dispone de horas, costos de compra, vencimientos ni distribución de colaboración; por eso no se informa rentabilidad, morosidad ni ranking de productividad. No se suman cantidades de unidades diferentes. Los precios de referencia actuales no alteran los importes históricos. Los presupuestos se redondean por ítem igual que el documento original.

Los estados son **actuales al generar**, no una reconstrucción histórica al último día del período. Las propuestas pendientes no entran en el denominador de la tasa de aceptación. Si se filtra por estado o búsqueda, indicadores y gráfico describen ese subconjunto. No se deben comparar tasas calculadas con filtros distintos.

## Pantalla y documentos

Se conserva un módulo Reportes. Arriba están tipo, período, estado y búsqueda; debajo, alcance, indicadores/interpretación, gráfico y detalle. Las acciones quedan al pie: Generar, Limpiar, Guardar documento HTML, Exportar detalle CSV e Imprimir/PDF. Los filtros se acomodan en filas y disponen de desplazamiento cuando falta espacio. Las columnas permiten redimensionarse y desplazarse horizontalmente. Al cambiar filtros se invalida el documento anterior para evitar exportar resultados con un título o período diferente.

Los reportes de situación actual deshabilitan las dos fechas existentes y explican su alcance; no descartan saldos ni trabajos antiguos. Los demás toman el mes actual inicialmente y requieren Generar. El gráfico muestra hasta siete categorías principales y agrupa el resto en Otras categorías, conservando el total. Las anulaciones netas negativas se dibujan en rojo. Los saldos se ordenan desde la emisión más antigua; la carga técnica prioriza reservas y, dentro de ellas o de los disponibles, la apertura más antigua.

- **HTML:** documento completo, autocontenido y portable, con gráfico SVG; no necesita conexión, scripts ni recursos externos. Se abre en un navegador y también se puede imprimir desde él.
- **CSV:** detalle tabular, separado por punto y coma, UTF-8 con BOM para Excel. Es un complemento del documento; no contiene el gráfico. Se neutralizan fórmulas en campos de texto.
- **Impresión/PDF:** vista previa paginada, con el gráfico en la primera página y todo el detalle. Para PDF se selecciona una impresora PDF instalada en Windows. No exige una librería PDF externa.

El documento contiene datos de clientes y del taller; quien exporta es responsable de su archivo. Todos los textos ingresados se codifican al generar HTML/SVG. Estos documentos son informes de gestión internos, sin validez fiscal.

## Consistencia corregida

- Inicio utiliza las mismas definiciones de los reportes, con datos reales y alcance visible. No suma resultados de consultas diferentes ni muestra un último reporte inventado.
- Clientes y Vehículos leen registros reales; alta, edición, baja/reactivación y actualización vuelven a consultar los servicios. Los inactivos van al final y los más recientes primero dentro de cada grupo.
- Los historiales muestran atenciones reales; un cliente de una atención no se presenta como propietario permanente del vehículo. El historial del cliente incluye los vehículos atendidos para él, no un padrón de propiedad.
- Las consultas de clientes y vehículos no conservan entidades rastreadas entre operaciones. Guardar un vehículo modifica solo el vehículo, no el grafo de marca/modelo.
- Las ediciones preservan identificador, fecha de alta y estado; estos últimos se gestionan desde su acción específica. En vehículo, marca y modelo se conservan como catálogo de consulta en esta edición; no se pretende cambiar su identidad escribiendo texto libre.
- Cancelar apertura es una operación real para Recepción: requiere motivo, versión vigente, estado EnEvaluacionTecnica y ausencia de evaluación/diagnóstico/presupuestos. Conserva fecha y evento de cancelación. Después de iniciar actividad técnica se resuelve el circuito, no se descartan cargos mediante este botón.
- Respaldo/restauración dejó de simular operaciones exitosas. Su operación real requiere instalación técnica y queda documentada en [Respaldos y restauración](../respaldos/operacion.md).

## Instalación y pruebas

Usar main y el esquema EsquemaInicialCircuitoActual siguiendo [instalación](../instalacion/entornos.md). La Demo v5 usa los tres estados. Prepararla siguiendo la guía de actualización. No aplicar migraciones de ramas retiradas.

```powershell
dotnet build Taller/Taller.sln --configuration Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj --configuration Release --no-build
```

Verificaciones automáticas: servicios/repo reales con EF InMemory, permisos y revocación, reservas propias/disponibles, rangos inclusivos, documentos vacíos, cobros mixtos con anulación de otro período, filtros coherentes, redondeo por ítem, escape HTML y protección CSV, cancelación segura, lecturas y guardados de catálogos. Las consultas de reportes se traducen con el proveedor SQL Server antes de interceptar y evitar la apertura de conexión. Esto comprueba traducción, no ejecución ni rendimiento en una base real.

Pruebas manuales antes de fusionar:

1. Abrir Inicio y Reportes con cada rol; comparar con registros reales. Crear un usuario sin actividad: sus reportes propios deben quedar vacíos.
2. Generar un presupuesto vigente aceptado parcialmente, uno rechazado y una revisión reemplazada. Verificar tasa, importe autorizado y ausencia de duplicación de revisiones.
3. Registrar un cobro mixto y anular un recibo de otro día. Consultar ambos días separadamente y el período completo; comprobar conciliación por medio. Un neto diario puede ser negativo y no representa una salida bancaria confirmada.
4. Consultar saldos y carga técnica con registros antiguos; no deben desaparecer por la fecha inicial del mes. Como técnico, confirmar que una reserva de otro técnico no se ofrece como propia.
5. Buscar una atención/vehículo y filtrar por estado; comprobar igualdad entre filas, indicadores, gráfico, HTML, CSV e impresión. Cambiar filtros sin generar: exportación debe quedar deshabilitada.
6. Generar sin datos; exportar el documento vacío. Probar textos con tildes, comillas y signos HTML, y abrir CSV en Excel.
7. Imprimir varias páginas, incluyendo una descripción larga y una anulación. Verificar PDF con una impresora instalada.
8. Probar 1366×768 y 1920×1080 con escalado 100 %, 125 % y 150 %: filtros accesibles, gráfico legible, desplazamiento horizontal y botones al pie.
9. Crear/editar/desactivar/reactivar clientes y vehículos; reiniciar y corroborar persistencia. Confirmar que los historiales no muestran filas de ejemplo ni inventan documentos.
10. Cancelar una apertura nueva y verificar su historial. Intentarlo después del ingreso técnico: debe bloquearse.

Las pruebas automáticas no reemplazan la inspección visual de Windows Forms, la impresión ni la ejecución de consultas/migraciones en SQL Server.

El gráfico conserva siete categorías principales y resume el resto cuando supera ocho. La metodología identifica cada categoría resumida con su valor; el detalle conserva todos los registros. No es un estado ni un filtro del dominio.

## Filtros disponibles antes de generar

Cada tipo ofrece todas sus opciones válidas al abrir o cambiar el reporte. No se deducen del resultado de una consulta: fechas, búsquedas y ausencia de registros no reducen el desplegable. Limpiar restablece «Todos» y conserva las opciones del tipo seleccionado. Una opción sin coincidencias genera un documento sin registros, nunca datos de ejemplo. En actividad técnica el filtro se identifica como «Acción».

Comprobación: entrar como administrador y administrativo, elegir Atenciones sin generar, revisar los estados; elegir Presupuestos y comprobar sus opciones; buscar un texto inexistente, generar y limpiar; confirmar que siguen disponibles todos los estados. Repetir en los tipos autorizados para el técnico.

## Documentos económicos

La impresión/PDF de presupuestos y liquidaciones utiliza columnas de concepto, cantidad, precio unitario y subtotal; importes alineados a la derecha y totales separados. El recibo identifica el cobro concreto, sus medios y referencias, y el saldo histórico al registrarlo. No confundir ese saldo con el saldo actual de la liquidación. Conservan identidad de empresa, logo, atención, cliente y vehículo; son documentos internos sin validez fiscal.

Comprobación visual en Windows: imprimir un presupuesto con más de una página y descripciones extensas, una liquidación con varios conceptos, un recibo con dos medios y un recibo anulado. Revisar repetición del encabezado, continuidad de filas sin perder texto, importes a la derecha y diferencias entre total propuesto, liquidado y cobrado. No se agregan impuestos, descuentos ni subtotales ficticios.

## Presentación con lotes extensos

El gráfico de pantalla tiene un lienzo desplazable: aumentar las series no comprime todas las etiquetas en el espacio de una notebook. La grilla omite magnitudes completamente ausentes y conserva cantidades a tres decimales y dinero a dos. Búsqueda/estado/período afectan el mismo conjunto de indicadores, gráfico y detalle.

HTML e impresión comparten el detalle tabular: referencia/fecha, descripción, responsable, estado y las magnitudes presentes. La impresión usa A4 horizontal, resumen y gráfico al inicio, cabeceras de tabla repetidas, ajuste de texto, importes a la derecha y metodología al final. Una fila extensa se continúa sin descartar texto. El CSV mantiene su esquema estable y valores numéricos para análisis. Ninguna exportación corta el detalle a los primeros registros.

Los precios del lote son ficticios y los reportes no calculan stock, utilidad ni rentabilidad que no estén respaldados por datos del sistema. Ampliar el período a doce meses para recorrer el lote completo.

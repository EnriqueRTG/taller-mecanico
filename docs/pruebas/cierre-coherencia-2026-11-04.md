# Cierre de coherencia del SGTM — 04/11/2026

La revisión se entrega en el [PR 75](https://github.com/EnriqueRTG/taller-mecanico/pull/75), rama `mejora/rendimiento-operaciones`, sobre `main` 27ae24a. El commit que contiene este documento identifica la entrega documental; la evidencia ejecutada del primer bloque de correcciones corresponde a 8d0693b. Las comprobaciones finales se consultan en los checks del PR.

## Correcciones del recorrido

| Hallazgo | Resolución | Comprobación |
|---|---|---|
| INC-01 Corrección de adicional incompatible con CHECK SQL | Se crea un Borrador enlazado sin presentación ni decisión artificial. El anterior queda histórico. El técnico usa Corregir adicional y emite otra versión para presentar y aceptar. No se relajan las restricciones de contacto. | CircuitoCobrosPruebas y recorrido IntegracionSqlPruebas con corrección antes de presentar y después de aceptar. |
| INC-02 Verificación de interfaz interrumpida | Tamaño mínimo de botones de filtros; mensajes separados para tamaño insuficiente, recorte y superposición. | Job Windows `formularios`, más ampliación de pruebas de filtros, solicitudes y materiales. |
| INC-03 Caso de revisión cubierto solo con InMemory | El recorrido SQL conserva CHECK, FK y rowversion reales y comprueba que el borrador no autorice ejecución. Continúa hasta material recibido, resolución de pausa, ejecución y liquidación. | Integración SQL real, sin omitir casos en ese job. |
| INC-04 Estados de presupuesto incompletos | Se agregan Aceptados, Aceptados parcialmente y Rechazados; se compara la opción por nombre, sin depender de índices. | Pruebas de interfaz de filtros por estado. |
| INC-05 Históricos mezclados en pendientes | Los filtros de estado muestran versiones vigentes; Históricos muestra las sustituidas. Todos conserva la consulta completa. | Pruebas de interfaz con versiones vigentes e históricas del mismo estado. |
| INC-06 Búsqueda desigual con tildes | Las bandejas y ambos buscadores de catálogo usan BusquedaTexto, sin distinguir mayúsculas ni tildes. No se altera identidad de cuentas ni credenciales. | Pruebas existentes de BusquedaTexto y selector de catálogo con Suspensión/suspension. |
| INC-07 Identidad del ítem divergente | El editor de propuesta permite asociar solicitud y distingue el par concepto/solicitud; muestra esa relación en la grilla. La consulta reutiliza las solicitudes ya cargadas. | Prueba de interfaz con un concepto en dos solicitudes; validación de pertenencia en el servicio. |
| INC-08 Importe visual incorrecto de material del cliente | Cambiar concepto conserva precio cero si aporta el cliente, tanto en evaluación como en propuesta. | Prueba de editor y reglas existentes de materiales. |
| INC-09 Dirección de alta distinta de edición | Ambos admiten hasta 200 caracteres opcionales. En alta, la altura es opcional y el total concatenado se valida antes de guardar. | Revisión de controles, servicio y columna SQL; compilación de formularios. |
| INC-10 Compatibilidad de base detectada tarde | Se verifica el historial de migraciones una vez al iniciar y se informa específicamente una incompatibilidad. No se migran bases compartidas desde cada cliente. | SQL real: versión actual, migración faltante y versión desconocida; no modifica datos de negocio. |
| INC-11 Documentación desactualizada | ERS, casos de uso, diccionario y guías describen paquetes de Google Drive, agente de sesión, importación y límites de verificación remota. Diagrama nuevo distingue corrección administrativa y decisión parcial real. | DOCX renderizados, lectura posterior y revisión de enlaces/XML/SVG. |
| INC-12 Comentarios de estados contradictorios | PendienteRevisionTecnica describe selección parcial real; Aceptado no presupone autorización automática de inspección. | Revisión de comentarios frente a servicios y pruebas. |

## Evidencia y límites

En el commit 8d0693b los cuatro checks finalizaron correctamente:

- [Windows / formularios](https://github.com/EnriqueRTG/taller-mecanico/actions/runs/41889169285/job/126781625264): construcción y disposición, filtros y fechas inclusivas en ambos roles, informes existentes y acciones de recepción, selector de catálogo, revisión parcial, ejecución, cobros y documentos.
- [SQL Server real](https://github.com/EnriqueRTG/taller-mecanico/actions/runs/41889169299/job/126781626069): integración de circuito, concurrencia, compatibilidad de inicio y respaldos/recuperaciones.
- [Compilación y reglas](https://github.com/EnriqueRTG/taller-mecanico/actions/runs/41889169299/job/126781626613): solución completa y pruebas locales.
- GitGuardian: sin secretos detectados.

La ejecución local sobre Linux aprobó 185 pruebas y omitió explícitamente las 32 de SQL; estas últimas se ejecutan por separado con SQL Server real en CI. Compilar Windows en Linux no equivale a ejecutar interfaz: el job `formularios` usa Windows. La revisión de filtros cubre anchos 820, 1020, 1366 y 1690; los demás ensayos de interfaz conservan su cobertura de navegación y disposición. Los tests de interfaz no se conectan a una base de producción.

La compatibilidad inicial compara migraciones aplicadas y esperadas. No sustituye una auditoría de alteraciones manuales de columnas realizadas por fuera de las migraciones.

Las pruebas no certifican la sincronización de la cuenta de Google Drive del usuario, los controladores de impresión ni todas las combinaciones de monitor/DPI. Esos puntos se verifican en el equipo de exposición con el [recorrido de Drive](../respaldos/google-drive.md) y el [plan de aceptación](aceptacion-version-actual.md). La colaboración entre dos puestos sigue siendo una comprobación separada del despliegue.

## Documentos de entrega

- [ERS v2.0](../ERS/ERS-SGTM-v2.0.docx): requisitos, búsqueda/filtros, correcciones, compatibilidad inicial y Google Drive.
- [Apéndice A](../ERS/Apendice-A-Casos-de-Uso-v2.0.docx): CU06 y CU10 acordes con las acciones reales y las autorizaciones.
- [Apéndice C](../ERS/Apendice-C-Diccionario-de-Datos-v2.0.docx): contratos JSON vigentes de respaldo, dirección y semántica del borrador.
- [Diagramas](../diagramas/README.md): nueve vistas de CU/flujo/arquitectura y ocho del DER; se incorpora la corrección de adicionales.

Permanecen seis migraciones, 24 tablas EF, 205 columnas y 43 FK. Esta corrección no requiere crear migraciones, recrear bases ni recargar el lote Demo. Los índices y restricciones de decisión, concurrencia y material del cliente permanecen activos.

Las copias nativas anteriores en Google Docs no se actualizan por modificar estos DOCX; cotejarlas con esta entrega antes de usarlas. Los manuales finales de usuario e instalación pueden redactarse sobre esta versión, después de la aceptación en el equipo de exposición. Las guías técnicas actuales siguen siendo la referencia de preparación y operación.

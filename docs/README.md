# Documentación de SGTM

Este índice es el punto de entrada. Cada guía tiene una ubicación única; no es necesario abrir todos los documentos para instalar o demostrar el sistema.

| Necesidad | Documento |
|---|---|
| Replicar el sistema y el lote en otra PC | [Instructivo TXT](instalacion/replicar-demo.txt) |
| Distinguir instalación real y Demo | [Entornos y base de datos](instalacion/entornos.md) |
| Referencias entre capas y DI | [Organización](arquitectura/estructura-solucion.md) |
| Tablas y claves del esquema actual | [Modelo relacional](arquitectura/modelo-relacional.md) |
| Estados reales y filtros agrupados | [Estados](arquitectura/estados-circuito-actual.md) |
| Recorrido habitual de recepción a entrega | [Circuito principal](operacion/circuito-principal.md) |
| Selección parcial, hallazgos, pausas y materiales | [Alternativas](operacion/alternativas.md) |
| Clientes, vehículos y catálogo consultable | [Datos maestros](operacion/catalogo-clientes-vehiculos.md) |
| Qué significa cada reporte | [Reportes por rol](reportes/reportes-por-rol.md) |
| Logo, CSV, impresión y documentos | [Documentos](operacion/documentos.md) |
| Guion y casos preparados | [Coloquio](pruebas/coloquio.md) |
| Comprobar el recorrido principal | [Pruebas principales](pruebas/circuito-principal.md) |
| Comprobar decisiones, cobros y alternativas | [Pruebas alternativas](pruebas/alternativas-cobros.md) |
| Contenido y precios del lote | [Catálogo Demo](pruebas/catalogo-demo.md) |
| Límites y reglas implementadas | [Alcance](especificacion/alcance-implementado.md) |
| Fechas y zona horaria | [Fechas](arquitectura/fechas-y-zona-horaria.md) |
| Credenciales y configuración segura | [Seguridad](instalacion/seguridad.md) |
| Preparar y operar el agente | [Respaldos y restauración](respaldos/operacion.md) |
| Configurar almacenamiento Azure | [Azure](respaldos/azure.md) |
| Trabajo colaborativo con Git | [Git](desarrollo/git.md) |

## Material académico editable

Los tres DOCX de `ERS` (ERS v1.0, Apéndice A de casos de uso y Apéndice C de diccionario) se conservan sin modificar. Requieren revisión formal para alinearse por completo con la implementación actual. `ERS/Notas-de-revision.txt` es material de trabajo para esa revisión, no una lista de funciones prometidas o pendientes del programa.

Se retiraron el PDF v0.9 y los diagramas PNG anteriores: repetían entregas previas y describían columnas/relaciones o alcances ya reemplazados. El modelo relacional enlaza la migración/configuraciones actuales; no sustituye todavía la edición académica final del DER y los casos de uso.

## Pruebas y revisión

`Taller.Pruebas/Arquitectura` protege el sentido de referencias; `ReglasYComponentes` comprueba servicios, validaciones, fechas y documentos (varios casos usan EF InMemory); `IntegracionSql` comprueba restricciones, transacciones, rowversion, recuperación y el lote en SQL real. Son verificaciones complementarias, no copias redundantes. `Taller.Pruebas.Interfaz` comprueba controles y construcción Windows/STA; no reemplaza la revisión visual manual.

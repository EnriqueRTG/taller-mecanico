# Documentación de SGTM

Este índice es el punto de entrada. Cada guía tiene una ubicación única; no es necesario abrir todos los documentos para instalar o demostrar el sistema.

| Necesidad | Documento |
|---|---|
| Aplicar esta actualización y sus migraciones | [Actualización](instalacion/actualizacion-coloquio.md) |
| Compartir una base entre dos notebooks | [Red privada](instalacion/red-privada.md) |
| Comprobar respaldo y recuperación en Demo | [Prueba local](instalacion/respaldos-locales-demo.md) |
| Estudiar recorridos entre capas | [Defensa del circuito](pruebas/defensa-circuito.md) |
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

La versión 2.0 está sincronizada con el circuito actual y las seis migraciones vigentes:

- [ERS](ERS/ERS-SGTM-v2.0.docx)
- [Apéndice A de casos de uso](ERS/Apendice-A-Casos-de-Uso-v2.0.docx)
- [Apéndice C de diccionario](ERS/Apendice-C-Diccionario-de-Datos-v2.0.docx)
- [Diagramas editables y vistas](diagramas/README.md)
- [Trazabilidad de requisitos, servicios y pruebas](especificacion/trazabilidad.md)
- [Plan integral de aceptación](pruebas/aceptacion-version-actual.md)
- [Evidencia y límites de verificación](pruebas/evidencia-cierre.md)

Los originales de Google Docs/diagrams.net conservan sus identificadores. Los DOCX y diagramas del repositorio son la instantánea de entrega; deben actualizarse conjuntamente si cambian reglas o esquema. Se retiraron las notas de diseño previas que proponían entidades o estados ya descartados.

- [Primera revisión de consistencia](pruebas/revision-integral-1.md)
- [Segunda revisión y aceptación pendiente](pruebas/revision-integral-2.md)
- [Tercera revisión y evidencia de entrega](pruebas/revision-integral-3.md)

## Pruebas y revisión

`Taller.Pruebas/Arquitectura` protege el sentido de referencias; `ReglasYComponentes` comprueba servicios, validaciones, fechas y documentos (varios casos usan EF InMemory); `IntegracionSql` comprueba restricciones, transacciones, rowversion, recuperación y el lote en SQL real. Son verificaciones complementarias, no copias redundantes. `Taller.Pruebas.Interfaz` comprueba controles y construcción Windows/STA; no reemplaza la revisión visual manual.

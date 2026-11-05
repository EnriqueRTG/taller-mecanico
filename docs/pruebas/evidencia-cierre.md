> Registro histórico de la etapa indicada. Estado vigente y alcance de las pruebas: [cierre de coherencia del 04/11/2026](cierre-coherencia-2026-11-04.md).

> Evidencia histórica de la etapa descrita en este documento. Para el estado más reciente, consultar [tercera revisión](revision-integral-3.md) y las comprobaciones del commit entregado.

# Evidencia de revisión de cierre

## Evidencia de entrega — 05/11/2026

Referencia de código: `ef2b2e844f85188f3065e34572d84179b9eca2a5` (integración del PR #75).

| Recorrido | Evidencia observada | Límite |
|---|---|---|
| Atención 244 (CU04–CU08) | Alta, propuesta aceptada, tres ítems ejecutados, liquidación por $94.000, dos pagos y entrega con saldo cero. El expediente quedó cerrado. | Servicio definido; este recorrido no ejercitó los caminos alternativos. |
| Cobros (CU09) | Filtro por atención 244: dos registros, $40.000 en efectivo y $54.000 por transferencia. Se revisó la vista previa y el ajuste de paginación. | Evidencia manual de este reporte. |
| Respaldo manual (CU10) | Copia completa verificada, exportada a Drive y visible en web; paquete descargado e importado; ensayo en base temporal con integridad y esquema correctos. | La base activa no se reemplazó. La programación automática queda fuera de la validación manual de entrega. |

Los resultados de las revisiones anteriores se conservan a continuación como antecedentes.


Fecha: 29/10/2026. Base de código: main 253e7d6 y cambios de esta rama de cierre documental. La versión exacta se identifica con el commit que contiene este documento; no extrapolar resultados a una modificación posterior.

## Comprobaciones realizadas

- Compilación Release de la solución completa con .NET SDK 10.0.400: cero errores y cero advertencias. En Linux se habilita EnableWindowsTargeting para compilar los proyectos Windows; no ejecuta sus formularios.
- Pruebas de reglas/componentes y arquitectura: 149 aprobadas. Las pruebas de SQL real requieren instancia desechable y quedan explícitamente omitidas al no configurarla. No se informa como aprobado lo omitido.
- Modelo SQL configurado frente a snapshot: sin cambios pendientes. La prueba ModeloPersistenciaPruebas genera el DDL sin conectarse y comprueba relaciones e inventario de migraciones.
- EF CLI 10.0.11: has-pending-model-changes sin diferencias; generado script SQL de la migración de limpieza sin conectarse ni modificar bases de usuarios.
- Documentos: lectura posterior de originales, exportación DOCX y renderizado para revisar disposición. Diagramas: XML editable y vistas SVG/PNG revisados.
- Referencias internas: revisión de rutas locales y enlaces entre guías, requisitos, servicios y pruebas.

## Comprobaciones que pertenecen al despliegue

Ejecutar [plan integral](aceptacion-version-actual.md) en Windows y SQL Server del entorno de demostración: operación entre dos puestos, concurrencia, DPI, vista previa/impresión, lote, informes frente a SQL y configuración opcional del agente según el entorno. Probar recuperación en una copia descartable y conservar respaldo independiente. No se modificó la base real del usuario durante esta revisión.

Las pruebas SQL de CI crean sus propias bases temporales y requieren SGTM_SQL_PRUEBAS, SGTM_PERMITIR_PRUEBAS_SQL y una carpeta accesible por el servicio SQL. Su resultado se consulta en el PR correspondiente; este documento no sustituye ese resultado ni afirma aprobación futura.

## Limpieza y elementos conservados

Se retira IdPresupuestoCambio del dominio/configuración/snapshot mediante migración nueva. Su guardia impide descartar valores antiguos no nulos. Las migraciones previas permanecen porque ya pudieron aplicarse; constituyen el historial ejecutable para crear/actualizar bases, no rutas de negocio obsoletas.

Demostración prepara el lote explícito; Respaldos ejecuta calendario y control; ambos proyectos de pruebas verifican capacidades distintas. Tienen uso y las capas no dependen de ellos. No se eliminan archivos por carecer de un botón directo cuando sirven a migración, composición, pruebas o herramientas.

Se retiran las notas de análisis anteriores y los DOCX del repositorio reemplazados por v2.0. Las guías vigentes describen el mismo alcance. No se afirma ausencia universal de fallos ni se reemplaza la aceptación manual del taller por esta revisión.

## Verificación remota del código revisado

El commit 5cadd901e2f86d51eda7992cbada1262ad50927d del PR #59 obtuvo resultado satisfactorio en los tres trabajos de CI: validar, respaldos-sql y formularios. El segundo ejecutó pruebas de circuito, migraciones y recuperación sobre SQL Server 2022 desechable; el tercero construyó solución/filtro y formularios reales en Windows/STA. Los ajustes posteriores de esta revisión son documentales; comprobar también el resultado del último head del PR antes de integrar.

- [Compilación, reglas y SQL](https://github.com/EnriqueRTG/taller-mecanico/actions/runs/40827280626)
- [Construcción WinForms y scripts](https://github.com/EnriqueRTG/taller-mecanico/actions/runs/40827280697)

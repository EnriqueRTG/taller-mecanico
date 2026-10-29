# Primera revisión de consistencia

## Alcance

Base de revisión: main 85c44c7. Esta iteración corrige hallazgos confirmados sin modificar el circuito de atención, las decisiones del cliente ni las reglas de cobro y entrega.

## Cambios aplicados

- Cliente: teléfono obligatorio de 7 a 15 dígitos en aplicación y persistencia. Se conserva el formato legible. Formulario, DTO, entidad, configuración EF, demostración, pruebas y documentación quedan alineados.
- Migración ExigirTelefonoContactoCliente: valida contactos existentes antes de imponer NOT NULL y CHECK. Error 51142 exige corrección de datos reales; no rellena valores ni elimina clientes. No se modifican migraciones aplicadas.
- Usuarios: alta y cambio de contraseña mantienen la misma validación de 8 a 100 caracteres y muestran los mismos límites. Las pruebas verifican ambos extremos y el rechazo fuera de rango.
- Autorización de usuarios: se conserva la revalidación persistente y la comprobación de identidad del ejecutor; se eliminan consultas repetidas que verificaban nuevamente la misma cuenta y rol.
- Interfaz de usuarios: los errores inesperados utilizan el tratamiento central, con referencia y registro técnico, sin publicar detalles SQL.
- Reportes: las acciones técnicas compartidas pasan a Consultas/ActividadTecnicaReporte; Infraestructura deja de depender del servicio concreto. Se retiran tres opciones históricas que ningún servicio del circuito actual emite: ActividadTomada, ActividadLiberada y DiagnosticoRegistrado. El informe definitivo actual queda representado por EvaluacionFinalizada.
- Legibilidad: consultas de reportes distribuidas en líneas, documentación XML corregida y comentarios de atención alineados con la exclusión del cierre con interrupción.

## Comprobaciones de estructura

Referencias de proyecto verificadas: Dominio sin referencias internas; Aplicación depende de Dominio; Infraestructura depende de Aplicación y Dominio; Presentación depende de Aplicación e Infraestructura para la composición. Las herramientas y las pruebas no son dependencias de las capas operativas.

Los siete estados de atención, los seis de presupuesto, los tres de ejecución y las tres actividades técnicas tienen referencias explícitas en servicios o consultas de Aplicación. No se eliminan valores por un simple recuento textual: tipos de concepto, orígenes y otras enumeraciones también participan mediante valores predeterminados, persistencia, catálogo y selección dinámica.

No se encontraron NotImplementedException ni NotSupportedException en el código fuente consultado. Esto no demuestra cobertura completa ni ausencia de código sin uso.

## Verificación y límites

Compilación completa de la solución en Release con Windows targeting: sin errores ni advertencias. Pruebas locales de reglas, componentes, arquitectura y correspondencia modelo/snapshot ejecutadas. Las pruebas SQL real requieren una instancia aislada y habilitación explícita; no deben confundirse con las pruebas InMemory.

Las comprobaciones visuales reales de WinForms y el funcionamiento entre dos puestos Windows se completan en Windows. La compilación cruzada no las sustituye. No se modificó la base de datos de una instalación de usuario.

## Continuación de la revisión

Revisar referencias semánticas de métodos, sobrecargas y registros DI; autorización y atomicidad por operación; casos concurrentes sobre SQL; coherencia de manuales y diagramas. Eliminar solamente código cuyo desuso esté demostrado, considerando eventos, reflexión, migraciones y ejecutables auxiliares. Mantener evidencia separada para cada iteración.

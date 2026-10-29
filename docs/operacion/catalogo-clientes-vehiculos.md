# Catálogo, clientes y vehículos

## Catálogo vigente

Administrativo y técnico consultan conceptos persistidos por categoría, tipo, disponibilidad y texto. Disponible significa que concepto/categoría están habilitados; no afirma existencias. Se puede actualizar, limpiar filtros y leer la descripción/unidad. Un precio sin definir se muestra A cotizar. El catálogo no ofrece altas, edición, bajas ni inventario desde los formularios actuales.

Concepto distingue Servicio, Repuesto y Consumible. El precio de referencia ayuda a preparar la propuesta; los presupuestos y comprobantes guardan sus propios valores. El índice único de categoría/tipo/nombre protege la identidad dentro de esa combinación. No existe un código comercial de concepto ni control de stock. La ampliación de gestión/inventario requerirá un caso de uso propio, no se considera implementada por disponer de una tabla Conceptos.

## Clientes y vehículos

Recepción registra, edita, deshabilita y reactiva con usuario/rol habilitados. El técnico consulta el historial según sus permisos. Los listados incluyen búsquedas y filtros; las altas validan datos obligatorios, formatos, longitudes y duplicados. El cliente se identifica por tipo/número de documento; el vehículo por dominio y modelo/marca reales.

Los editores conservan la rowversion leída. Una modificación concurrente informa conflicto en vez de sobrescribir. Los índices únicos de SQL resuelven también el intento de dos altas simultáneas. Las bajas son lógicas; no eliminan documentos asociados. El historial de la atención conserva la vinculación cliente/vehículo de la operación.

El teléfono del cliente es obligatorio: entre 7 y 15 dígitos; admite espacios, guiones, paréntesis y un signo + inicial. El correo y la dirección continúan siendo opcionales. Alta y edición aplican la misma regla de aplicación, respaldada por NOT NULL y una restricción CHECK en SQL.

## Responsabilidades

Los formularios presentan datos y llaman a ClienteServicio, VehiculoServicio y ConceptoServicio. Aplicación valida y autoriza; Infraestructura consulta/persiste y traduce conflictos conocidos; configuraciones EF definen relaciones, índices y restricciones. Las categorías se consultan mediante la proyección del catálogo y su inicialización, sin un servicio de categorías sin consumidores.

La preparación aplica todas las migraciones del esquema actual, según [instalación](../instalacion/entornos.md). No se copian archivos o migraciones de ramas anteriores. Para comprobar concurrencia, abrir el mismo registro en dos instancias, guardar en una y confirmar que la otra rechaza su versión desactualizada.

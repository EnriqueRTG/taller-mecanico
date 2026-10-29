# Lote ampliado de demostración v5

Este lote permite probar presupuestos, búsquedas, filtros, estados y reportes sin incorporar datos comerciales ficticios al arranque normal de la aplicación. Solo se carga explícitamente sobre una base SQL vacía terminada en _Demo. Actualizar Git no recarga ni elimina datos.

## Volumen y referencias

| Conjunto | Contenido |
|---|---|
| Usuarios | 11: dos administradores, tres administrativos, cinco técnicos y una cuenta técnica inactiva |
| Clientes | 88: 80 activos y 8 inactivos, con teléfono obligatorio, documentos distintos y contactos ficticios |
| Vehículos | 256: 240 vinculados a atenciones y 16 libres para nuevas aperturas; 2 inactivos con antecedentes cerrados |
| Marcas/modelos | 30 marcas y 208 modelos activos, más una marca/modelo históricos inactivos; 31 y 209 en total |
| Categorías/conceptos | 16 categorías de oferta y una inactiva; 353 conceptos activos y uno inactivo |
| Atenciones | 240, marcadores DEMO-001 a DEMO-240; conocidas, síntomas y mixtas, distribuidas en hasta 360 días |
| Operaciones | Propuestas, revisiones, adicionales, ejecución, liquidaciones, pagos parciales/mixtos, anulaciones y entregas creados mediante los servicios reales |

Los primeros 64 casos cubren los puntos de operación del guion; el resto aporta más historia de trabajos terminados y cobros para que los reportes no se limiten a una lista de pendientes. La consola informa los conteos efectivos de propuestas, liquidaciones y cobros al confirmar la carga; no hay registros de pago inventados con independencia de su liquidación.

Marcas/modelos y perfiles de vehículos se definen en VehiculosDemostracion. Se incluyen vehículos habituales y referencias de distintas marcas; los perfiles del lote utilizan años definidos y ciertos casos acotan su selección a referencias pertinentes. Esto no crea una matriz automática de compatibilidad: la aplicación no registra una ficha técnica completa de motor, versión o VIN para deducir repuestos.

## Catálogo y presupuestos

Las familias incluyen diagnóstico/inspección, mantenimiento/lubricación, frenos, refrigeración, encendido/electricidad, suspensión/dirección, embrague/transmisión, distribución/motor, ruedas, climatización, alimentación/inyección, escape/emisiones, motor interno, diésel, iluminación/visibilidad y fijaciones/protección.

Cada entrada declara Servicio, Repuesto o Consumible, descripción de su unidad/alcance y un importe ficticio en ARS. No son cotizaciones de mercado, costos de compra ni existencias. El técnico confirma aplicación, unidad y precio antes de enviar un presupuesto. No se agregan cantidades como si todos los conceptos midieran lo mismo.

El service completo declara solo mano de obra: aceite y filtros son líneas separadas. No acumular servicios individuales equivalentes. La carga de aire acondicionado declara que incorpora la carga para no duplicarla como material. Los kits indican revisar piezas incluidas para no cobrarlas dos veces. Discos/campanas delanteros/traseros están diferenciados; no se presume que todos los vehículos llevan el mismo sistema.

Los casos operativos utilizan 16 familias de trabajo, con diferentes servicios, materiales y cantidades. El montaje de neumáticos se cotiza por rueda; los fluidos por litro y los juegos como conjunto. El material aportado por el cliente conserva precio de venta cero y recepción/conformidad antes de su utilización.

## Carga y seguridad de la demostración

PreparadorDemostracion es el mismo punto de entrada utilizado por consola y pruebas SQL. Aplica las seis migraciones existentes; después inicializa y carga los datos dentro de una transacción Serializable con bloqueo de carga. Los componentes reutilizan esa transacción cuando existe. Si falla un paso, no confirma parcialmente los datos del lote. Las migraciones del esquema se aplican antes de la transacción comercial.

Una segunda carga sobre una base poblada se rechaza sin duplicar ni reemplazar sus datos. Una Demo v4 existente sigue siendo una base operativa válida tras actualizar su esquema, pero no se transforma en v5 al actualizar el código. Para usar todo el lote v5, conservar un respaldo, cerrar las aplicaciones y reemplazar exclusivamente la Demo cuando se haya confirmado que sus datos son descartables. Ver [réplica](../instalacion/replicar-demo.txt) y [actualización](../instalacion/actualizacion-coloquio.md).

La auditoría de acceso no se simula: aparece al autenticar usuarios. Los respaldos y su auditoría requieren operaciones reales y no se crean durante la carga. Las fechas se retrotraen solo dentro de esta herramienta, conservando el orden interno de cada atención; los maestros/autores son anteriores a sus operaciones.

## Aceptación

La prueba SQL carga el mismo preparador de consola, comprueba conteos, restricciones, referencias, saldo/entrega, variantes y rechaza repetir la carga. Concilia los siete reportes con consultas SQL y verifica el conjunto representado por sus gráficos. Las pruebas de catálogo comprueban claves distintas, nombres, unidades, precios y perfiles vehiculares existentes.

Para observar toda la historia del lote, consultar doce meses; un reporte del mes actual debe reflejar únicamente su cohorte o movimientos según la metodología de ese reporte.

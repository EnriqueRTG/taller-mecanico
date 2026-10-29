# Verificación del lote Demo v5 y sus reportes

## Alcance

La ampliación conserva las seis migraciones vigentes y no modifica el modelo relacional ni los estados de negocio. Agrega datos mediante los servicios existentes y mejora la representación tabular de reportes en pantalla, HTML e impresión. Las bases de los equipos del usuario y su cuenta Azure no se modifican desde el repositorio.

## Datos y consistencia

- 31 marcas y 209 modelos: 30 marcas y 208 modelos activos, más una pareja inactiva para filtros.
- 17 categorías y 354 conceptos: 16 categorías y 353 conceptos activos, más registros inactivos de prueba.
- 88 clientes, 256 vehículos y 240 atenciones distribuidas en doce meses.
- 11 usuarios de los tres roles, incluido un usuario deshabilitado.
- Presupuestos, liquidaciones y pagos asociados mediante servicios; las cantidades exactas se imprimen al cargar. Las comprobaciones exigen más de 200 presupuestos, 100 liquidaciones y 150 pagos.
- Precios ficticios en ARS, unidades y cantidades fraccionarias explícitas. No implica control de existencias ni compatibilidad automática de una autoparte con cualquier vehículo.

El preparador usado por la consola es también el que ejecuta la prueba SQL. La carga comercial es transaccional y rechaza mezclar el lote con una base poblada. Los repositorios reutilizan una transacción existente y solo confirman la que les pertenece.

## Verificación automatizada

En el cambio de implementación 40f78b1, GitHub Actions completó 172 pruebas generales y las 30 pruebas específicas de SQL Server. Las siete conciliaciones de reportes usan el lote ampliado. La construcción WinForms y su comprobación de interfaz en Windows también finalizaron correctamente. El cambio final de documentación y presentación debe conservar estos resultados antes de integrarse.

La compilación Release local finalizó sin advertencias ni errores. Se inspeccionaron exportaciones HTML de 40 registros con descripción extensa, importes y cantidades, convertidas a A4 horizontal para verificar paginación y cabeceras repetidas. Esa inspección no sustituye la comprobación manual del controlador de impresión de Windows, las escalas de pantalla o la configuración Azure del usuario.

## Comprobación pendiente en los equipos

1. Actualizar código y preparar voluntariamente una Demo descartable v5 según la guía de réplica. No se amplía una Demo anterior al iniciar la aplicación.
2. Revisar catálogo, marcas/modelos y filtros; armar un presupuesto con servicios, repuestos y consumibles.
3. Ejecutar el circuito principal con administrativo y técnico; contrastar reportes con sus consultas SQL de control.
4. Comprobar pantalla HD/Full HD, desplazamiento del gráfico, vista previa e impresión/PDF de varias páginas.
5. Configurar y comprobar completa, diferencial y réplica Azure antes de ensayar una restauración.

No se declara validada una restauración real ni una conexión LAN hasta realizarlas con la instalación y las identidades definitivas.

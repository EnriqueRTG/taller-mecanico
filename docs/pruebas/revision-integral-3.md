# Tercera revisión de consistencia

## Alcance y versión

Base: main 3f5eda3. Código comprobado: cb2735e, integrado en el PR 62 junto con su documentación. Esta revisión mantiene el circuito comercial, las cuatro capas y el lote Demo v4. No agrega inventario ni nuevos estados de negocio. Los documentos y el código deben entregarse desde el mismo commit final del PR.

## Correcciones

- InicializadorDatos coordina los arranques con sp_getapplock de transacción. Reconsulta antes de insertar administrador, marcas y modelos; confirma conjuntamente y descarta seguimiento al terminar. No aplica migraciones automáticamente.
- UnidadDeTrabajo reconoce UX_Diagnosticos_Atencion y traduce el duplicado a una validación. Se elimina la referencia al índice retirado de diagnóstico inicial.
- Clientes y vehículos muestran la identidad y la acción concreta al cambiar disponibilidad; No es la respuesta predeterminada.
- La configuración publicada de Presentación usa AppContext.BaseDirectory. Los fallos iniciales conservan excepción y referencia técnica en el registro local; la pantalla conserva mensajes sanitizados.
- ExigirContactoDecisionPresupuesto agrega CK_Presupuestos_ContactoDecision. Aceptado, Rechazado y PendienteRevisionTecnica requieren persona y canal válidos. La migración se detiene con 51144 si existen decisiones incompletas; no rellena datos ni elimina propuestas. El esquema sigue teniendo 24 tablas, 205 columnas y 43 FK.
- Las consultas compuestas de lista/cuenta de cobros usan LeerConsistenteAsync con Serializable durante la consulta. La transacción termina antes de mostrar el resultado; no se mantienen bloqueos durante la interacción del usuario. No se cambia el aislamiento general de la aplicación.
- ReporteServicio rechaza estados ajenos al tipo de reporte antes de consultar sus datos. Una selección inválida no se presenta como un resultado vacío válido.
- Se retiran TextoDocumentos y las representaciones textuales de liquidación/recibo sin uso operativo; se conserva y prueba el documento estructurado utilizado por la interfaz. Se retiran textos de compatibilidad de decisiones nuevas.
- Azure verifica mediante lectura y SHA-256 el archivo principal, el auxiliar de control disponible y el manifiesto. Los .bak no se sobrescriben. La grilla distingue negocio/control y el catálogo JSON conserva el estado/hash/tamaño del auxiliar. Un auxiliar ausente no se declara verificado; una falla conserva el archivo local y deja el intento pendiente.
- Guías de instalación, actualización y réplica indican las seis migraciones. ERS, Apéndices A/C, DER y originales editables se actualizan conjuntamente. Los documentos históricos de revisión conservan su alcance y evidencia original. Los originales de Apéndices A/C se configuran en modo Páginas para que su exportación PDF respete la paginación y márgenes.

## Evidencia automatizada

Sobre cb2735e pasaron los trabajos de GitHub Actions de compilación y pruebas generales y de SQL Server aislado: 169 pruebas generales y 30 pruebas SQL, sin fallos. Las pruebas SQL incluyen inicialización simultánea, rechazo de una migración con contacto incompleto, traducción del diagnóstico duplicado, aislamiento/liberación de una lectura de cuenta y el circuito de cobros/entrega. El lote SQL concilia los siete reportes con consultas independientes, importes y totales gráficos. El trabajo Windows comprueba construcción de formularios en STA, no su aceptación visual.

Los resultados del commit final y su integración se consultan en [PR 62](https://github.com/EnriqueRTG/taller-mecanico/pull/62). No se declara aprobada una cuenta Azure real ni un ensayo en los equipos del usuario por estos resultados de CI.

## Actualización

1. Guardar cambios locales, respaldar y cerrar todos los puestos y el agente.
2. Actualizar el código y configurar la conexión al destino confirmado.
3. Aplicar todas las migraciones existentes una sola vez. Ante 51142 corregir contactos de clientes; ante 51144 revisar la persona/canal de decisiones con evidencia real y supervisión técnica.
4. Verificar seis migraciones, actualizar todos los ejecutables y probar el circuito. No generar migraciones distintas en cada PC ni recargar un lote poblado.
5. Crear una completa nueva tras comprobar la actualización. Conservar copias anteriores identificadas; el ensayo del sistema rechaza esquemas incompatibles.

## Aceptación pendiente de instalación

- Dos puestos Windows sobre una sola base: flujo principal, selección parcial exacta/reformulada, rechazo, hallazgo y material aportado.
- Concurrencia operativa, pérdida de conexión y reintentos en los equipos reales.
- Pantallas HD/Full HD y escalado 100/125/150 %; impresión/PDF multipágina y datos reales de empresa.
- Conciliación manual de reportes con SQL y filtros del coloquio.
- Identidades, permisos, carpeta, espacio, agente y ejecución automática con la interfaz cerrada.
- Restauración de una copia descartable y réplica Azure con identidad/contenedor reales.

No se modificó la base real del usuario durante esta revisión. La instalación de migraciones y el ensayo de recuperación siguen siendo acciones explícitas del despliegue.

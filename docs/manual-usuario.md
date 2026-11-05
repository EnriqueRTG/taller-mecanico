# Guía de operación del SGTM

Material de base para el manual definitivo. Describe el circuito, las acciones por rol y sus excepciones; la instalación y los permisos técnicos se consultan en la [referencia técnica](../REFERENCIA-TECNICA.md).

- [Atención, selección parcial, hallazgos y materiales del cliente](#operacion-alternativas)
- [Catálogo, clientes y vehículos](#operacion-catalogo-clientes-vehiculos)
- [Circuito principal de una atención](#operacion-circuito-principal)
- [Identidad, exportación y vista previa de documentos](#operacion-documentos)
- [Consulta y filtros de las bandejas](#operacion-filtros-listados)
- [Reportes de gestión y revisión de consistencia](#reportes-reportes-por-rol)

<a id="operacion-alternativas"></a>

## Atención, selección parcial, hallazgos y materiales del cliente

### Alcance

Una atención conserva la solicitud, evaluación, autorizaciones, ejecución y entrega del mismo vehículo. Una solicitud definida puede presupuestarse sin diagnóstico; un síntoma requiere informe técnico. El técnico confirma la viabilidad incluso cuando el cliente pide una tarea concreta. Registrar una propuesta no autoriza ejecutarla.

### Responsabilidades

| Operación | Administrativo | Técnico |
| --- | --- | --- |
| Abrir atención, registrar solicitud y comunicar propuestas | Sí | Consulta |
| Preparar informe, propuesta y adicionales | No | Sí |
| Registrar selección, aceptación o rechazo del cliente | Sí | Consulta |
| Validar una selección parcial y proponer alternativa viable | No | Sí |
| Registrar hallazgos | Consulta | Sí |
| Pausar por solicitud del cliente u otra causa | Sí | Sí |
| Resolver una pausa y determinar condiciones de continuidad | No | Sí |
| Recibir y conformar técnicamente material del cliente | Consulta | Sí |
| Registrar ejecución y finalización técnica | No | Sí |
| Liquidar, cobrar y confirmar entrega con conformidad | Sí | No |

Los servicios revalidan usuario/rol activos contra la base. La interfaz no sustituye esos controles. El administrador conserva sus funciones administrativas de seguridad, reportes y respaldos; no obtiene permisos operativos nuevos.

### Selección parcial

1. Recepción informa y registra que el cliente autoriza los ítems elegidos, a cantidades/precios ofrecidos, condicionado a viabilidad técnica. Se conserva persona que decide, medio y usuario que registró.
2. La propuesta queda PendienteRevisionTecnica: aún no puede ejecutarse.
3. El técnico abre Revisar selección parcial. Si conserva exactamente conceptos, descripciones, cantidades, precios, origen de material y solicitudes, usa Validar selección del cliente. Queda Aceptado con AlcanceValidado; no se pide otra aceptación y se registra el técnico en el historial. Los ítems excluidos siguen conservados y no se ejecutan ni cobran.
4. Si modifica la selección, la acción cambia a Enviar nueva propuesta a recepción. Se crea otra versión enlazada, con justificación, pendiente de presentación y decisión. Tampoco se acepta automáticamente cuando el técnico cambió el origen a material del cliente.
5. Una corrección técnica no es una decisión del cliente. Las solicitudes de corrección sin selección parcial real siempre producen otra propuesta para presentar y aceptar.

La viabilidad es una decisión del técnico registrada explícitamente; no se pretende deducir automáticamente dependencias mecánicas desde nombres de conceptos.

### Hallazgos, pausas y adicionales

Desde **Atenciones → Ver detalle → Ver progreso y seguimiento**, o desde la evaluación/ejecución, se consulta el expediente con cuatro pestañas:

- **Avance:** consulta de trabajos autorizados y cantidades realizadas; la edición se hace en ejecución.

- **Hallazgos e informes:** descripción y recomendación obligatorias, fecha y autor. Un hallazgo no genera deuda. Si impide continuar, crea una pausa de la tarea seleccionada o de toda la intervención.
- **Pausas y continuidad:** espera de repuestos, decisión pendiente, solicitud de detener u otra causa. Solo el técnico resuelve una pausa, dejando su motivo y responsabilidad. Resolver no ejecuta ni autoriza trabajos.
- **Materiales del cliente:** requeridos y recibidos, conformidad técnica y observaciones.

La acción **Informe de intervención** permite previsualizar e imprimir el diagnóstico disponible, los trabajos autorizados y su ejecución, los hallazgos/recomendaciones, las pausas y los materiales aportados. También se puede guardar como PDF mediante una impresora PDF de Windows. Es una consulta del expediente, no una nueva autorización ni un recibo.

Las tareas independientes pueden continuar con otra tarea pausada. Para finalizar se resuelven pausas y propuestas pendientes y se completan todos los ítems autorizados. No existe cierre con interrupción en esta entrega; no se simula una tarea completada ni una entrega para resolver un faltante.

Durante la ejecución, el técnico puede preparar un presupuesto adicional vinculado al hallazgo seleccionado. Se incluyen **solo conceptos nuevos**, sin duplicar lo ya autorizado. Recepción comunica y registra su decisión de manera independiente. Aceptarlo agrega sus ítems a la ejecución; rechazarlo conserva los presupuestos anteriores. La pausa no se resuelve automáticamente al aceptar: el técnico debe verificar que efectivamente puede continuar.

Un adicional sin ejecutar también puede ser enviado a revisión por recepción. Se conserva su documento anterior; el técnico prepara la alternativa desde Presupuestos. Los ítems ya ejecutados no se reescriben ni se eliminan.

Si se necesita más inspección/desarme con costo, desde la evaluación técnica se prepara un **adicional de inspección**, limitado a servicios. Se presenta y acepta antes de que aparezca habilitado para registrar su ejecución en Inspecciones autorizadas. Rechazar la reparación posterior no borra ese servicio autorizado y realizado.

### Materiales aportados por el cliente

Al preparar la propuesta, seleccionar **Material del cliente / Aporta cliente** para un repuesto o consumible. La cantidad y descripción expresan lo requerido; su precio de venta del taller es cero. Los servicios no pueden tener ese origen.

El técnico registra cantidad recibida acumulada, conformidad y observación (incluyendo descripción/especificación y reemplazo de una pieza no apta). No se permite disminuir lo recibido ni invalidar material ya utilizado. No se admite consumir más de lo recibido ni utilizar material no conforme. Si el material falta y afecta otras tareas, se pausa explícitamente ese alcance.

No se incorpora inventario, compras ni deuda del taller con el proveedor del cliente. El costo que el cliente pague por su cuenta queda fuera del cobro del taller. La mano de obra y materiales del taller mantienen sus precios autorizados. El registro de materiales permanece en el expediente aunque se excluya de la liquidación.

### Ejecución, cobro y cierre

La ejecución reúne todos los presupuestos de reparación vigentes y aceptados; los servicios de inspección se registran durante la evaluación. Las propuestas pendientes o rechazadas no aparecen como trabajo autorizado.

Se liquida únicamente cantidad efectivamente realizada de ítems autorizados, excluyendo materiales aportados por el cliente. No se cobra lo pendiente. La liquidación normal se habilita tras completar el alcance autorizado. Los pagos parciales/mixtos y comprobantes internos conservan su funcionamiento. La entrega exige liquidación, pagos consistentes, saldo cero y conformidad registrada por recepción. **Finalizar trabajo** registra la conformidad del técnico; no equivale a entregar el vehículo.

### Integridad y concurrencia

Cada mutación comparte la versión de Atención y una única confirmación transaccional con su historial. Las decisiones/revisiones también verifican la versión del presupuesto. Una reserva ajena impide intervenciones técnicas concurrentes; el administrativo puede registrar una pausa solicitada por el cliente. Las referencias a tareas/hallazgos se validan contra la atención correspondiente.

SQL conserva claves externas sin borrado en cascada, restricciones de textos, cantidades, origen de materiales, precio cero de aportes y coherencia de decisiones. El esquema se crea desde cero mediante EsquemaInicialCircuitoActual; no se convierte el lote anterior.

### Verificación

Preparar la base siguiendo [entornos](../REFERENCIA-TECNICA.md#instalacion-entornos). Las pruebas SQL ejercitan el recorrido selección parcial → hallazgo → adicional → material del cliente → resolución → ejecución → liquidación con rowversion nativa. Las pruebas de reglas verifican aceptación/autorización, permisos, pausas y cantidades. Repetir manualmente con dos roles, cierre/reapertura y documentos de varias páginas; comprobar que la pausa administrativa se avisa también directamente al taller.

Si cambian condiciones del acuerdo aunque los ítems sean iguales, marcar Se modifican condiciones del acuerdo y describirlas en la justificación: la propuesta vuelve a recepción para otra aceptación. La justificación de viabilidad sin cambios solo documenta la revisión técnica.

---

<a id="operacion-catalogo-clientes-vehiculos"></a>

## Catálogo, clientes y vehículos

### Catálogo vigente

Administrativo y técnico consultan conceptos persistidos por categoría, tipo, disponibilidad y texto. Disponible significa que concepto/categoría están habilitados; no afirma existencias. Se puede actualizar, limpiar filtros y leer la descripción/unidad. Un precio sin definir se muestra A cotizar. El catálogo no ofrece altas, edición, bajas ni inventario desde los formularios actuales.

Concepto distingue Servicio, Repuesto y Consumible. El precio de referencia ayuda a preparar la propuesta; los presupuestos y comprobantes guardan sus propios valores. El índice único de categoría/tipo/nombre protege la identidad dentro de esa combinación. No existe un código comercial de concepto ni control de stock. La ampliación de gestión/inventario requerirá un caso de uso propio, no se considera implementada por disponer de una tabla Conceptos.

### Clientes y vehículos

Recepción registra, edita, deshabilita y reactiva con usuario/rol habilitados. El técnico consulta el historial según sus permisos. Los listados incluyen búsquedas y filtros; las altas validan datos obligatorios, formatos, longitudes y duplicados. El cliente se identifica por tipo/número de documento; el vehículo por dominio y modelo/marca reales.

Los editores conservan la rowversion leída. Una modificación concurrente informa conflicto en vez de sobrescribir. Los índices únicos de SQL resuelven también el intento de dos altas simultáneas. Las bajas son lógicas; no eliminan documentos asociados. El historial de la atención conserva la vinculación cliente/vehículo de la operación.

El teléfono del cliente es obligatorio: entre 7 y 15 dígitos; admite espacios, guiones, paréntesis y un signo + inicial. El correo y la dirección continúan siendo opcionales. Alta y edición aplican la misma regla de aplicación, respaldada por NOT NULL y una restricción CHECK en SQL.

### Responsabilidades

Los formularios presentan datos y llaman a ClienteServicio, VehiculoServicio y ConceptoServicio. Aplicación valida y autoriza; Infraestructura consulta/persiste y traduce conflictos conocidos; configuraciones EF definen relaciones, índices y restricciones. Las categorías se consultan mediante la proyección del catálogo y su inicialización, sin un servicio de categorías sin consumidores.

La preparación aplica todas las migraciones del esquema actual, según [instalación](../REFERENCIA-TECNICA.md#instalacion-entornos). No se copian archivos o migraciones de ramas anteriores. Para comprobar concurrencia, abrir el mismo registro en dos instancias, guardar en una y confirmar que la otra rechaza su versión desactualizada.

---

<a id="operacion-circuito-principal"></a>

## Circuito principal de una atención

### Alcance y responsabilidades

Se supone disponibilidad del taller y de materiales, aceptación íntegra del presupuesto y finalización de los trabajos. Recepción registra los pedidos y comunica la propuesta; el técnico prepara el alcance, registra el diagnóstico cuando corresponde, presupuesta, ejecuta y comprueba el resultado. El administrador no interviene obligatoriamente en el circuito.

1. **Atenciones → Nueva atención (recepción).** Seleccionar cliente y vehículo. Agregar cada trabajo solicitado y cada síntoma como una solicitud independiente. Registrar kilometraje si se conoce y observaciones de ingreso. Registrar ingreso deja la atención en preparación técnica, sin generar cargos ni autorizar reparaciones.
2. **Preparación técnica (técnico).** Tomar la atención. Para pedidos definidos se abre Presupuesto. Para síntomas se incorpora el informe técnico. Una atención puede contener ambos; no se selecciona una modalidad mixta global. Guardar borrador conserva los avances y la reserva.
3. **Preparar presupuesto (técnico).** Seleccionar la solicitud a la que responde cada concepto y agregar cantidad y precio. El presupuesto debe cubrir todos los pedidos originales antes de enviarse a recepción. La preparación habitual de este circuito es sin cargo. No presupuestar un diagnóstico ficticio para poder continuar.
4. **Presupuestos → Informar al cliente (recepción).** Comunicar alcance y total. Registrar la decisión completa, la persona y el medio. Cerrar sin decidir conserva la comunicación y deja pendiente la respuesta; no autoriza ni rechaza automáticamente.
5. **Atenciones → Ejecución (técnico).** Iniciar ejecución. Solo aparecen ítems autorizados. Registrar cantidades reales y estados; Completar fila seleccionada facilita la carga de una tarea realmente terminada, sin guardar automáticamente. Guardar avance persiste lo registrado. Comprobar el resultado y marcar conformidad técnica antes de finalizar. La atención todavía no se cierra.
6. **Comprobantes y pagos (recepción).** Liquidar cantidades realmente realizadas a los precios acordados. Emitir la liquidación interna. Registrar uno o varios pagos, incluso con medios distintos; cada pago genera su recibo y reduce saldo. El presupuesto no se utiliza como si fuera una ejecución.
7. **Entrega (recepción).** Con saldo cero y trabajo terminado, identificar a quien retira y registrar la conformidad de entrega. Solo entonces se cierra la atención.

### Persistencia y actualización

La recepción y sus relaciones forman parte del esquema inicial vigente. No se convierten motivos o presupuestos del lote anterior.

Antes de actualizar una base utilizada, conservar un respaldo y probar sobre una copia. Con la solución actualizada, aplicar `Update-Database -Project Taller.Infraestructura -StartupProject Taller.Presentacion -Context TallerDbContext` apuntando a la base de prueba configurada. No es necesario volver a cargar la demostración ni eliminar la base.

### Comprobación manual con dos usuarios

Usar recepción y técnico sobre la misma base de demostración y la misma versión del programa. Probar una atención nueva por cada caso: solo trabajo conocido; solo síntoma; trabajo y síntoma en la misma visita. Anotar número de atención, propuesta, cantidades, liquidación y recibos. Actualizar las bandejas al cambiar de rol.

Verificar que el técnico recibe todos los pedidos; que el caso conocido no obliga a generar diagnóstico; que el mixto exige cubrir ambos pedidos; que no se puede ejecutar sin aceptación; que la conformidad técnica no equivale al cierre; que un pago parcial impide entregar; y que el segundo pago permite cerrar mediante entrega. Comprobar historial y persistencia al volver a abrir cada pantalla.

### Alternativas conservadas

Las revisiones, selecciones parciales, hallazgos, pausas, adicionales y materiales del cliente se conservan como alternativas. Una selección exacta validada por el técnico habilita ejecución; reformularla exige otra aceptación. El cierre con interrupción queda fuera de esta entrega. La autorización de inspecciones cobrables requiere acuerdo previo.

Los documentos económicos del sistema son internos; no constituyen integración de facturación fiscal.

### Legibilidad, borradores y documentos

- Las acciones exclusivas de otro rol se ocultan. Las propias que dependen del estado se conservan deshabilitadas. Los servicios vuelven a validar permisos, versiones y condiciones reales.
- Recepción puede **Solicitar revisión** si la propuesta sigue vigente, no tiene ejecución ni liquidación y no está reservada. Registra la solicitud del cliente; no cambia importes ni autoriza reparaciones. **Revisar selección parcial** corresponde al técnico, que valida la selección exacta sin nueva aceptación o reformula una versión para volver a consultar al cliente.
- En preparación, elija solicitud y concepto, especifique descripción, cantidad y precio, y agregue la fila. Cantidad y precio se pueden corregir en la lista antes de guardar. Material del cliente conserva precio cero.
- **Guardar borrador** conserva la pestaña y la lista enlazada; permite desplazarse y seguir editando sin agregar filas nuevas. **Enviar a recepción** finaliza la preparación; no autoriza ejecución.
- Informe y presupuesto cuentan con **Exportar HTML** e **Imprimir / PDF**, logo y contacto configurados en `Empresa`. HTML conserva una tabla de importes y secciones del informe; impresión usa el encabezado común en cada página. El nombre sugerido identifica tipo, número y fecha de elaboración de la copia. El diálogo de la impresora PDF de Windows puede requerir confirmar el nombre.

Prueba de aceptación: en una notebook, abrir presupuesto con 15 filas, bajar hasta la última, guardar borrador, comprobar que permanece la pestaña y volver a recorrer/editar/quitar filas. Probar a 1366×768 al 100 % y 1920×1080 al 125 %. Comprobar recepción sin acciones de validación técnica; técnico sin botones de decisión del cliente. Exportar ambas clases de documento, revisar caracteres especiales, importes, logo, contacto y varias páginas.

### Ejecución y consulta de avance

**Trabajos en curso**, después de Presupuestos, muestra atenciones con propuestas vigentes aceptadas que están en ejecución. El técnico encuentra primero sus reservas y luego las disponibles; recepción consulta la misma bandeja. Busca por atención, cliente, dominio o motivo; filtra disponibles, asignadas o con pausas. Actualizar consulta avances guardados por otros puestos; no es una vista de edición simultánea en tiempo real.

- **Gestionar ejecución**: exclusivo del técnico, disponible si no hay reserva o es propia. Iniciar ejecución toma la reserva; cerrar conserva la reserva; liberar conserva avances guardados y permite que otro técnico continúe.
- **En curso**: puede tener cantidad cero mientras se trabaja. **Completado** aplica la cantidad autorizada; al ingresar la cantidad autorizada también queda completado. La sincronización de pantalla no sustituye las validaciones de permisos, concurrencia, pausas y materiales recibidos de la capa de aplicación.
- Los estados son No iniciado, En curso y Completado. Una cantidad parcial permanece En curso. No se permite reducir cantidades realizadas ni finalizar con ítems pendientes.
- **Guardar avance** persiste y permite consultar el progreso desde recepción. **Finalizar trabajo** aparece tras completar todos los ítems autorizados y confirmar la comprobación técnica. No hay cierre con interrupción.
- **Ver progreso y seguimiento**, disponible también en Atenciones y su detalle, abre primero el avance de cada tarea: autorizado, realizado, estado y registro técnico. Hallazgos, pausas y materiales son otras pestañas. Recepción no redacta ni registra hallazgos; puede consultar e imprimir/exportar información con la identidad de la empresa.
- Un nuevo hallazgo se registra por el técnico en su pestaña. Marcar que impide continuar bloquea el alcance afectado; una propuesta adicional debe comunicarse y aceptarse antes de ejecutarla. El informe de avance no equivale a comprobante ni pago.

Prueba con dos roles: técnico inicia y guarda una tarea En curso; recepción actualiza y verifica ese estado y cantidad. Técnico marca Completado y verifica la cantidad automática; guarda, y recepción vuelve a actualizar. Finalmente el técnico comprueba el resultado, confirma la tilde y finaliza; la atención sale de Trabajos en curso y conserva su progreso desde Atenciones para liquidación y entrega.

### Recorrido habitual y excepciones (revisión de navegación)

1. El alta registra solicitudes e ingreso y deja la atención en preparación técnica. Disponible/reservada se expresa por la reserva; no se inventa otro estado. El esquema nuevo admite solo los siete estados actuales; no conserva las rutas de apertura, diagnóstico pendiente ni pausa general anteriores. Las pausas actuales se registran por alcance para no detener tareas independientes.
2. En Atenciones se abre Detalle; allí están Ingreso y solicitudes, el expediente consolidado y el acceso a progreso. El expediente reúne informes, versiones de presupuestos, ejecución, hallazgos e historial. Recepción también consulta liquidación y medios de cobro; el técnico no recibe funciones financieras. Trabajos en curso conserva su acceso directo al progreso.
3. En presupuesto, Marcar como informado registra la comunicación ya efectuada fuera del sistema; no envía mensajes. Después ofrece registrar respuesta ahora o dejarla pendiente. La confirmación distingue aceptación total, selección parcial y rechazo. Pedir corrección/cambios queda como acción secundaria; revisar un acuerdo aceptado es una excepción que exige nueva preparación y nueva aceptación y no admite ejecución/reserva/liquidación previas.
4. Iniciar ejecución se presenta encima de la lista. Antes de tomar, la grilla es de consulta; después se edita y Guardar avance es la acción habitual. Con todas las tareas completadas se habilita conformidad y Finalizar trabajo. Otras acciones agrupa hallazgos, pausas, materiales y relevo; no modifica la reserva ni finaliza por abrirlo. Cada opción abre su pestaña.
5. La ampliación de inspección abre Hallazgos. Debe registrarse el motivo y prepararse un adicional de servicios de inspección, distinto de la reparación; requiere comunicación/aceptación antes de ejecutarse. Los problemas identificados mediante la revisión habitual se documentan directamente en el informe, sin cargos adicionales automáticos.
6. La cuenta muestra Emitir liquidación antes del documento, Registrar cobro mientras haya saldo y Registrar entrega cuando esté saldada. Se ocultan operaciones anteriores. Anular pago es una corrección administrativa destacada como sensible, conserva el original y no efectúa devoluciones bancarias.
7. Usar restante completa saldo menos los medios preparados en ese cobro; no redondea, no cobra ni omite referencias. La referencia electrónica identifica la operación de pago (transferencia/billetera o cupón/autorización de tarjeta), no una factura ni datos completos de tarjeta. Efectivo puede dejarla vacía.
8. La entrega pide solo persona que retira (prefijada con el cliente), conformidad y observación opcional. El resultado procede del cierre técnico. El servicio valida responsable, conformidad, estado, versión y saldo; el registro queda en el historial de la atención sin duplicar tablas. Después se confirma el cierre y se conserva una cuenta de consulta e impresión con datos de entrega.

El cargador crea Demo v5 íntegramente mediante el circuito vigente en una base vacía. Los ejemplos cancelados corresponden a cancelación actual previa a actividad, no a aperturas de un circuito retirado. Una base Demo ya cargada no se mezcla ni se vuelve a sembrar desde el arranque de la aplicación.

### Seguimiento contextual y detenciones

El selector no filtra grillas: indica el alcance del próximo hallazgo o pausa técnica. Solo aparece en esas dos pestañas, con una etiqueta específica. Avance y materiales no lo muestran. Preparar adicional requiere seleccionar el hallazgo; resolver pausa requiere seleccionar una pausa activa; recibir materiales requiere seleccionar una fila acordada a cargo del cliente.

Recepción registra una **detención solicitada por el cliente**, general y con motivo. No puede resolverla ni crear pausas de una tarea específica. Debe avisar también directamente al taller. La ejecución consulta pausas con un contexto independiente cada diez segundos: muestra aviso, bloquea la edición del alcance afectado y conserva las ediciones locales. No es comunicación instantánea garantizada ni un control físico de la reparación. Si no se puede comprobar la continuidad, se bloquea la edición hasta recuperar la consulta. El servicio mantiene validación de versión y pausas al guardar, incluso entre consultas. El técnico documenta la resolución; aceptar un adicional no levanta pausas automáticamente. Ante ediciones anteriores a una detención, consultar/resolver y volver a abrir con los datos actuales antes de registrar continuidad; no se fusionan cambios obsoletos automáticamente.

Las inspecciones extraordinarias aceptadas se registran desde preparación técnica, pestaña **Inspecciones autorizadas**, como No iniciado, En curso o Completado; todas deben completarse antes de finalizar la evaluación. La reparación se presupuesta aparte. La prueba de circuito comprueba que rechazar después la reparación no elimina el cobro de la inspección autorizada y realizada.

Para materiales del cliente, recepción registra la solicitud de cambio y el técnico prepara una propuesta viable que conserva el material con origen Cliente y precio cero. Se comunica y acepta nuevamente. El técnico registra cantidad recibida y aptitud en Materiales del cliente antes de utilizarlo; no se retira silenciosamente del acuerdo ni se convierte su recepción en una venta del taller.

### Datos de decisión y lectura de documentos

La persona que decide es el cliente o su representante (por ejemplo, responsable de una flota), no el recepcionista. El usuario que registra se obtiene de la sesión y queda asociado a la decisión e historial. El medio describe cómo llegó la respuesta; no implica enviar mensajes desde el sistema. El documento de presupuesto para el cliente conserva identidad del taller, atención, cliente, vehículo, alcance, importes, estado y condiciones útiles. Omite los metadatos internos de comunicación y secciones vacías; esos datos no se eliminan de los registros internos. El informe técnico adjunto identifica a su técnico responsable; no se infiere un autor técnico del usuario creador de una revisión administrativa.

Adjuntar informe técnico en un pedido definido permite documentar una comprobación; no agrega un cargo ni autoriza reparaciones. Cambiar el concepto seleccionado reinicia la cantidad de nueva carga en uno sin modificar filas existentes. Las cabeceras de operación utilizan un estilo compartido de Presentación y los mensajes de cierre técnico/ingreso identifican la atención y vehículo para facilitar comunicaciones internas externas a la aplicación.

El informe de seguimiento es una copia opcional para comunicar avance: ofrece impresión/PDF y HTML secundario, no forma parte obligatoria de la ejecución ni acredita pagos. En vista previa, hacer clic sobre la hoja para usar flechas y rueda; con zoom ampliado desplazan la lectura, RePág/AvPág cambian página, Inicio/Fin van a extremos y Ctrl+rueda o +/− ajustan zoom. Comprobar en Windows también el desplazamiento con 100 % y 150 % y documentos de varias páginas.

### Cobrar y entregar en momentos distintos

La liquidación detalla lo autorizado y efectivamente realizado y determina la deuda; puede imprimirse antes del primer cobro para informar qué debe abonarse. No acredita un pago. Cada recibo corresponde a un cobro concreto, identifica sus medios/referencias y conserva el saldo histórico al registrarlo. Un recibo anulado se conserva marcado y no computa como cobro vigente. Los documentos económicos son internos, sin integración fiscal.

La cuenta mantiene acciones ordenadas y una selección inicial de recibo aunque no se haya abierto su pestaña. Cambiar entre conceptos y recibos no debe hacer aparecer funciones por primera vez. Las funciones de anulación siguen dependiendo de una cuenta abierta y un pago no anulado; imprimir un recibo anulado conserva su identificación.

El pago total no entrega ni cierra automáticamente. Recepción puede cerrar la ventana y registrar el retiro más tarde por dos accesos equivalentes: Atenciones → filtro **Pendientes de entrega** → **Registrar entrega** (directo), o Comprobantes y pagos → cuenta → Registrar entrega. El acceso directo consulta otra vez la cuenta y usa el mismo formulario y servicio de entrega; no pasa por el detalle ni duplica reglas. El filtro incluye solo cierre técnico con liquidación existente y saldo cero; el servicio vuelve a validar estado, reserva, versión y saldo real antes de cerrar. La entrega continúa requiriendo responsable y conformidad explícita.

Prueba: pagar parcialmente (entrega bloqueada), saldar, cerrar cuenta, volver a Atenciones y filtrar pendientes, registrar entrega y comprobar que desaparece de pendientes pero permanece en el historial. Abrir la misma cuenta desde Atenciones y desde Comprobantes y comparar botones, selección e impresión antes y después de cambiar de pestaña. Los mensajes de decisiones, cobros y entrega identifican atención y dominio para facilitar el aviso interno por los medios del taller; el sistema no envía mensajes externos automáticamente.

---

<a id="operacion-documentos"></a>

## Identidad, exportación y vista previa de documentos

### Identidad del taller
El logo de la aplicación se incorpora al HTML y a la impresión de reportes, liquidaciones, recibos e informes de intervención. No convierte estos documentos internos en comprobantes fiscales.
En `Taller/Taller.Presentacion/appsettings.Local.json`, conservar la conexión y agregar o combinar esta sección con los datos reales:

```json
{
  "Empresa": {
    "Nombre": "Taller mecánico",
    "Direccion": "",
    "Telefono": "",
    "Correo": "",
    "IdentificacionFiscal": ""
  }
}
```

Los campos vacíos no se imprimen como datos ficticios. Reiniciar la aplicación después de configurarlos. El nombre es de hasta 100 caracteres; dirección 200, teléfono 80, correo 150 e identificación fiscal 80. La identidad se comparte mediante inyección de dependencias. El HTML embebe el logo PNG; puede abrirse sin conexión.

### Nombres
Los reportes proponen `SGTM-Atenciones-recibidas-y-situacion-actual_20261024_105039.html` (o `.csv`). La fecha corresponde a la elaboración del reporte en horario del taller; repetir una exportación del mismo documento conserva el nombre y el diálogo solicita confirmar una sobrescritura. No es un consecutivo fiscal ni una numeración persistente.
Las liquidaciones y recibos incluyen su número y fecha de emisión. El nombre del trabajo de impresión usa el mismo criterio. El controlador de PDF de Windows puede solicitar elegir nuevamente la ubicación o el nombre.

### CSV para Excel
Incluye todas las filas del detalle y agrega contexto por fila: reporte, generación, emisor, alcance, filtros, método, unidad del gráfico e identidad del taller. Conserva importes numéricos y valores ausentes vacíos. No incluye dibujos, logo, tarjetas ni gráficos: para el documento visual completo utilizar HTML o impresión/PDF.
Usa UTF-8 con BOM, separador punto y coma, comillas escapadas y protección contra fórmulas de texto. La primera línea `sep=;` indica el separador a Excel. En otros lectores CSV, omitir esta primera línea al importar. Si Excel no reconoce importes decimales, importar mediante Datos → Desde texto/CSV, separador punto y coma y configuración regional Español (Argentina). No utilizar la coma como separador: también forma parte de nombres y decimales.
Sin resultados, el CSV conserva las cabeceras y no inventa filas.

### Vista previa
La vista incluye Anterior/Siguiente, selector Página, cantidad total, zoom y botón Imprimir/PDF. La rueda del mouse sobre la vista cambia de página. El zoom permite leer el contenido sin alterar lo impreso. La impresión vuelve a generar todas las páginas desde el inicio; los filtros del reporte generado no cambian. Se utiliza el diálogo de selección de impresora antes de imprimir.
Comprobar con un reporte de 100 registros: ir a página 2 y última, volver a primera, cambiar zoom, imprimir el documento completo dos veces y comparar cantidad y contenido. Logo/contacto y pie numerado deben aparecer en cada página; no debe faltar texto en los saltos de página.

---

<a id="operacion-filtros-listados"></a>

## Consulta y filtros de las bandejas

Los filtros de texto, situación y fechas se combinan sobre los registros ya cargados. Cambiarlos no consulta otra vez SQL; **Actualizar** obtiene los datos actuales. **Limpiar filtros** vuelve a todas las situaciones, elimina el texto y desactiva ambos extremos de fecha. No queda una fila seleccionada automáticamente.

### Preparación técnica: informe y reserva

- **Ver informe técnico** consulta el diagnóstico definitivo de la atención seleccionada. Solo aparece cuando existe ese informe. Recepción puede leerlo; no puede escribir ni reservar una evaluación.
- Una preparación de servicio solicitado puede producir una propuesta sin informe de diagnóstico. Que esté finalizada no implica que deba existir un informe ni que el vehículo esté entregado.
- **Mis actividades** corresponde exclusivamente al técnico y a sus reservas pendientes. Recepción dispone de Todas las actividades, Disponibles (sin reserva), Reservadas y Finalizadas.
- Los botones para tomar, continuar o liberar la preparación siguen reservados al técnico, con autorización nuevamente comprobada por los servicios.

### Fechas opcionales

Activar la casilla del selector **Desde** o **Hasta** para aplicar ese extremo. Se pueden usar de forma independiente. El filtro incluye el día completo, también las horas finales del día Hasta. Un rango invertido muestra una indicación y no devuelve filas; corregirlo o limpiar los filtros.

| Listado | Fecha que se filtra: la columna Último movimiento |
|---|---|
| Preparación técnica | Último evento del historial de la atención; si no tiene eventos, su apertura. |
| Presupuestos | Fecha de decisión; si no existe, fecha de presentación; si tampoco existe, fecha de creación. |
| Trabajos en curso | Último evento del historial de la atención; si no tiene eventos, su apertura. |
| Comprobantes y pagos | Último movimiento económico del comprobante (emisión, cobro o anulación). Si todavía no se liquidó, apertura de la atención. |

La fecha acota qué filas se muestran; los estados, avances y saldos son los actuales. No reconstruye una situación histórica al día elegido ni sustituye los reportes de movimientos por período. Las fechas empiezan desactivadas para no esconder pendientes antiguos.

Catálogo no usa este filtro temporal: se consulta por texto, categoría, tipo y disponibilidad, sin confundir disponibilidad comercial con stock físico.

### Distribución

Los cinco listados usan campos etiquetados que aprovechan el ancho de la pantalla y acciones en una fila independiente. En una ventana angosta los campos se distribuyen en dos filas. La adaptación visual no cambia permisos ni reglas del circuito.

---

<a id="reportes-reportes-por-rol"></a>

## Reportes de gestión y revisión de consistencia

La pantalla **Reportes** genera un documento para tomar decisiones: título, usuario, fecha de generación, alcance, filtros, indicadores, gráfico, interpretación, metodología y registros que sustentan los resultados. Todos se calculan a partir del mismo conjunto filtrado. No contiene datos de ejemplo ni estima importes inexistentes.

### Qué puede consultar cada rol

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

### Pantalla y documentos

Se conserva un módulo Reportes. Arriba están tipo, período, estado y búsqueda; debajo, alcance, indicadores/interpretación, gráfico y detalle. Las acciones quedan al pie: Generar, Limpiar, Guardar documento HTML, Exportar detalle CSV e Imprimir/PDF. Los filtros se acomodan en filas y disponen de desplazamiento cuando falta espacio. Las columnas permiten redimensionarse y desplazarse horizontalmente. Al cambiar filtros se invalida el documento anterior para evitar exportar resultados con un título o período diferente.

Los reportes de situación actual deshabilitan las dos fechas existentes y explican su alcance; no descartan saldos ni trabajos antiguos. Los demás toman el mes actual inicialmente y requieren Generar. El gráfico muestra hasta siete categorías principales y agrupa el resto en Otras categorías, conservando el total. Las anulaciones netas negativas se dibujan en rojo. Los saldos se ordenan desde la emisión más antigua; la carga técnica prioriza reservas y, dentro de ellas o de los disponibles, la apertura más antigua.

- **HTML:** documento completo, autocontenido y portable, con gráfico SVG; no necesita conexión, scripts ni recursos externos. Se abre en un navegador y también se puede imprimir desde él.
- **CSV:** detalle tabular, separado por punto y coma, UTF-8 con BOM para Excel. Es un complemento del documento; no contiene el gráfico. Se neutralizan fórmulas en campos de texto.
- **Impresión/PDF:** vista previa paginada, con el gráfico en la primera página y todo el detalle. Para PDF se selecciona una impresora PDF instalada en Windows. No exige una librería PDF externa.

El documento contiene datos de clientes y del taller; quien exporta es responsable de su archivo. Todos los textos ingresados se codifican al generar HTML/SVG. Estos documentos son informes de gestión internos, sin validez fiscal.

### Consistencia corregida

- Inicio utiliza las mismas definiciones de los reportes, con datos reales y alcance visible. No suma resultados de consultas diferentes ni muestra un último reporte inventado.
- Clientes y Vehículos leen registros reales; alta, edición, baja/reactivación y actualización vuelven a consultar los servicios. Los inactivos van al final y los más recientes primero dentro de cada grupo.
- Los historiales muestran atenciones reales; un cliente de una atención no se presenta como propietario permanente del vehículo. El historial del cliente incluye los vehículos atendidos para él, no un padrón de propiedad.
- Las consultas de clientes y vehículos no conservan entidades rastreadas entre operaciones. Guardar un vehículo modifica solo el vehículo, no el grafo de marca/modelo.
- Las ediciones preservan identificador, fecha de alta y estado; estos últimos se gestionan desde su acción específica. En vehículo, marca y modelo se conservan como catálogo de consulta en esta edición; no se pretende cambiar su identidad escribiendo texto libre.
- Cancelar apertura es una operación real para Recepción: requiere motivo, versión vigente, estado EnEvaluacionTecnica y ausencia de evaluación/diagnóstico/presupuestos. Conserva fecha y evento de cancelación. Después de iniciar actividad técnica se resuelve el circuito, no se descartan cargos mediante este botón.
- Respaldo/restauración dejó de simular operaciones exitosas. Su operación real requiere instalación técnica y queda documentada en [Respaldos y restauración](../REFERENCIA-TECNICA.md#respaldos-operacion).

### Instalación y pruebas

Usar main y el esquema EsquemaInicialCircuitoActual siguiendo [instalación](../REFERENCIA-TECNICA.md#instalacion-entornos). La Demo v5 usa los tres estados. Prepararla siguiendo la guía de actualización. No aplicar migraciones de ramas retiradas.

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

### Filtros disponibles antes de generar

Cada tipo ofrece todas sus opciones válidas al abrir o cambiar el reporte. No se deducen del resultado de una consulta: fechas, búsquedas y ausencia de registros no reducen el desplegable. Limpiar restablece «Todos» y conserva las opciones del tipo seleccionado. Una opción sin coincidencias genera un documento sin registros, nunca datos de ejemplo. En actividad técnica el filtro se identifica como «Acción».

Comprobación: entrar como administrador y administrativo, elegir Atenciones sin generar, revisar los estados; elegir Presupuestos y comprobar sus opciones; buscar un texto inexistente, generar y limpiar; confirmar que siguen disponibles todos los estados. Repetir en los tipos autorizados para el técnico.

### Documentos económicos

La impresión/PDF de presupuestos y liquidaciones utiliza columnas de concepto, cantidad, precio unitario y subtotal; importes alineados a la derecha y totales separados. El recibo identifica el cobro concreto, sus medios y referencias, y el saldo histórico al registrarlo. No confundir ese saldo con el saldo actual de la liquidación. Conservan identidad de empresa, logo, atención, cliente y vehículo; son documentos internos sin validez fiscal.

Comprobación visual en Windows: imprimir un presupuesto con más de una página y descripciones extensas, una liquidación con varios conceptos, un recibo con dos medios y un recibo anulado. Revisar repetición del encabezado, continuidad de filas sin perder texto, importes a la derecha y diferencias entre total propuesto, liquidado y cobrado. No se agregan impuestos, descuentos ni subtotales ficticios.

### Presentación con lotes extensos

El gráfico de pantalla tiene un lienzo desplazable: aumentar las series no comprime todas las etiquetas en el espacio de una notebook. La grilla omite magnitudes completamente ausentes y conserva cantidades a tres decimales y dinero a dos. Búsqueda/estado/período afectan el mismo conjunto de indicadores, gráfico y detalle.

HTML e impresión comparten el detalle tabular: referencia/fecha, descripción, responsable, estado y las magnitudes presentes. La impresión usa A4 horizontal, resumen y gráfico al inicio, cabeceras de tabla repetidas, ajuste de texto, importes a la derecha y metodología al final. Una fila extensa se continúa sin descartar texto. El CSV mantiene su esquema estable y valores numéricos para análisis. Ninguna exportación corta el detalle a los primeros registros.

Los precios del lote son ficticios y los reportes no calculan stock, utilidad ni rentabilidad que no estén respaldados por datos del sistema. Ampliar el período a doce meses para recorrer el lote completo.

### Filtro simplificado de actividad técnica

El desplegable Acción ofrece cuatro hitos del recorrido principal: Evaluación tomada, Evaluación finalizada, Ejecución tomada y Trabajo finalizado. Conserva tres alternativas habituales: Hallazgo registrado, Selección del cliente validada y Selección reformulada por el técnico.

La reducción afecta solamente las opciones de selección individual del reporte, para todos los roles autorizados a consultarlo. «Todos» mantiene el conjunto seleccionado de hitos técnicos: también borradores/avances, liberaciones, resolución de pausas, materiales y preparación de adicionales. Estos eventos siguen presentes en el detalle, los indicadores, el gráfico y las exportaciones cuando pertenecen al alcance consultado. No se eliminan estados, operaciones, historial ni datos, ni se modifica el circuito de negocio. Los demás tipos de reporte conservan sus filtros.

### Filtro simplificado de carga técnica

El desplegable Estado ofrece «Todos», «Disponible para tomar» y «Reservada». Se omite la selección individual «Con pausas pendientes» para centrar la consulta en la asignación habitual. «Todos» conserva las atenciones con pausas y las identifica en el detalle; no modifica sus restricciones de operación ni las clasifica como disponibles.

«Con pausas pendientes» es una categoría calculada cuando existe al menos una PausaTrabajo sin FechaResolucion en la atención. Tiene prioridad sobre su reserva: no es un valor de EstadoAtencion ni EstadoEjecucionDetalle y no implica necesariamente que todas sus tareas estén detenidas. Puede provenir de un hallazgo que impide continuar o una detención registrada, general o por tarea según permisos. Los filtros Disponible/Reservada conservan su semántica actual y no incorporan las filas categorizadas con pausas. Los datos, el gráfico y las exportaciones de «Todos» mantienen el alcance completo.

El reporte de actividad técnica no es una auditoría exhaustiva ni una medida de productividad. La preparación de una corrección de adicional permanece en el historial de la atención; no agrega otra opción al filtro ni altera los reportes económicos. Todas las búsquedas de texto libre de reportes y bandejas ignoran mayúsculas y tildes usando el mismo criterio; no se modifican credenciales ni valores persistidos.

# Circuito principal de una atención

## Alcance y responsabilidades

Se supone disponibilidad del taller y de materiales, aceptación íntegra del presupuesto y finalización de los trabajos. Recepción registra los pedidos y comunica la propuesta; el técnico prepara el alcance, registra el diagnóstico cuando corresponde, presupuesta, ejecuta y comprueba el resultado. El administrador no interviene obligatoriamente en el circuito.

1. **Atenciones → Nueva atención (recepción).** Seleccionar cliente y vehículo. Agregar cada trabajo solicitado y cada síntoma como una solicitud independiente. Registrar kilometraje si se conoce y observaciones de ingreso. Registrar ingreso deja la atención en preparación técnica, sin generar cargos ni autorizar reparaciones.
2. **Preparación técnica (técnico).** Tomar la atención. Para pedidos definidos se abre Presupuesto. Para síntomas se incorpora el informe técnico. Una atención puede contener ambos; no se selecciona una modalidad mixta global. Guardar borrador conserva los avances y la reserva.
3. **Preparar presupuesto (técnico).** Seleccionar la solicitud a la que responde cada concepto y agregar cantidad y precio. El presupuesto debe cubrir todos los pedidos originales antes de enviarse a recepción. La preparación habitual de este circuito es sin cargo. No presupuestar un diagnóstico ficticio para poder continuar.
4. **Presupuestos → Informar al cliente (recepción).** Comunicar alcance y total. Registrar la decisión completa, la persona y el medio. Cerrar sin decidir conserva la comunicación y deja pendiente la respuesta; no autoriza ni rechaza automáticamente.
5. **Atenciones → Ejecución (técnico).** Iniciar ejecución. Solo aparecen ítems autorizados. Registrar cantidades reales y estados; Completar fila seleccionada facilita la carga de una tarea realmente terminada, sin guardar automáticamente. Guardar avance persiste lo registrado. Comprobar el resultado y marcar conformidad técnica antes de finalizar. La atención todavía no se cierra.
6. **Comprobantes y pagos (recepción).** Liquidar cantidades realmente realizadas a los precios acordados. Emitir la liquidación interna. Registrar uno o varios pagos, incluso con medios distintos; cada pago genera su recibo y reduce saldo. El presupuesto no se utiliza como si fuera una ejecución.
7. **Entrega (recepción).** Con saldo cero y trabajo terminado, identificar a quien retira y registrar la conformidad de entrega. Solo entonces se cierra la atención.

## Persistencia y actualización

La recepción y sus relaciones forman parte del esquema inicial vigente. No se convierten motivos o presupuestos del lote anterior.

Antes de actualizar una base utilizada, conservar un respaldo y probar sobre una copia. Con la solución actualizada, aplicar `Update-Database -Project Taller.Infraestructura -StartupProject Taller.Presentacion -Context TallerDbContext` apuntando a la base de prueba configurada. No es necesario volver a cargar la demostración ni eliminar la base.

## Comprobación manual con dos usuarios

Usar recepción y técnico sobre la misma base de demostración y la misma versión del programa. Probar una atención nueva por cada caso: solo trabajo conocido; solo síntoma; trabajo y síntoma en la misma visita. Anotar número de atención, propuesta, cantidades, liquidación y recibos. Actualizar las bandejas al cambiar de rol.

Verificar que el técnico recibe todos los pedidos; que el caso conocido no obliga a generar diagnóstico; que el mixto exige cubrir ambos pedidos; que no se puede ejecutar sin aceptación; que la conformidad técnica no equivale al cierre; que un pago parcial impide entregar; y que el segundo pago permite cerrar mediante entrega. Comprobar historial y persistencia al volver a abrir cada pantalla.

## Alternativas conservadas

Las revisiones, selecciones parciales, hallazgos, pausas, adicionales y materiales del cliente se conservan como alternativas. Una selección exacta validada por el técnico habilita ejecución; reformularla exige otra aceptación. El cierre con interrupción queda fuera de esta entrega. La autorización de inspecciones cobrables requiere acuerdo previo.

Los documentos económicos del sistema son internos; no constituyen integración de facturación fiscal.

## Legibilidad, borradores y documentos

- Las acciones exclusivas de otro rol se ocultan. Las propias que dependen del estado se conservan deshabilitadas. Los servicios vuelven a validar permisos, versiones y condiciones reales.
- Recepción puede **Solicitar revisión** si la propuesta sigue vigente, no tiene ejecución ni liquidación y no está reservada. Registra la solicitud del cliente; no cambia importes ni autoriza reparaciones. **Revisar selección parcial** corresponde al técnico, que valida la selección exacta sin nueva aceptación o reformula una versión para volver a consultar al cliente.
- En preparación, elija solicitud y concepto, especifique descripción, cantidad y precio, y agregue la fila. Cantidad y precio se pueden corregir en la lista antes de guardar. Material del cliente conserva precio cero.
- **Guardar borrador** conserva la pestaña y la lista enlazada; permite desplazarse y seguir editando sin agregar filas nuevas. **Enviar a recepción** finaliza la preparación; no autoriza ejecución.
- Informe y presupuesto cuentan con **Exportar HTML** e **Imprimir / PDF**, logo y contacto configurados en `Empresa`. HTML conserva una tabla de importes y secciones del informe; impresión usa el encabezado común en cada página. El nombre sugerido identifica tipo, número y fecha de elaboración de la copia. El diálogo de la impresora PDF de Windows puede requerir confirmar el nombre.

Prueba de aceptación: en una notebook, abrir presupuesto con 15 filas, bajar hasta la última, guardar borrador, comprobar que permanece la pestaña y volver a recorrer/editar/quitar filas. Probar a 1366×768 al 100 % y 1920×1080 al 125 %. Comprobar recepción sin acciones de validación técnica; técnico sin botones de decisión del cliente. Exportar ambas clases de documento, revisar caracteres especiales, importes, logo, contacto y varias páginas.

## Ejecución y consulta de avance

**Trabajos en curso**, después de Presupuestos, muestra atenciones con propuestas vigentes aceptadas que están en ejecución. El técnico encuentra primero sus reservas y luego las disponibles; recepción consulta la misma bandeja. Busca por atención, cliente, dominio o motivo; filtra disponibles, asignadas o con pausas. Actualizar consulta avances guardados por otros puestos; no es una vista de edición simultánea en tiempo real.

- **Gestionar ejecución**: exclusivo del técnico, disponible si no hay reserva o es propia. Iniciar ejecución toma la reserva; cerrar conserva la reserva; liberar conserva avances guardados y permite que otro técnico continúe.
- **En curso**: puede tener cantidad cero mientras se trabaja. **Completado** aplica la cantidad autorizada; al ingresar la cantidad autorizada también queda completado. La sincronización de pantalla no sustituye las validaciones de permisos, concurrencia, pausas y materiales recibidos de la capa de aplicación.
- Los estados son No iniciado, En curso y Completado. Una cantidad parcial permanece En curso. No se permite reducir cantidades realizadas ni finalizar con ítems pendientes.
- **Guardar avance** persiste y permite consultar el progreso desde recepción. **Finalizar trabajo** aparece tras completar todos los ítems autorizados y confirmar la comprobación técnica. No hay cierre con interrupción.
- **Ver progreso y seguimiento**, disponible también en Atenciones y su detalle, abre primero el avance de cada tarea: autorizado, realizado, estado y registro técnico. Hallazgos, pausas y materiales son otras pestañas. Recepción no redacta ni registra hallazgos; puede consultar e imprimir/exportar información con la identidad de la empresa.
- Un nuevo hallazgo se registra por el técnico en su pestaña. Marcar que impide continuar bloquea el alcance afectado; una propuesta adicional debe comunicarse y aceptarse antes de ejecutarla. El informe de avance no equivale a comprobante ni pago.

Prueba con dos roles: técnico inicia y guarda una tarea En curso; recepción actualiza y verifica ese estado y cantidad. Técnico marca Completado y verifica la cantidad automática; guarda, y recepción vuelve a actualizar. Finalmente el técnico comprueba el resultado, confirma la tilde y finaliza; la atención sale de Trabajos en curso y conserva su progreso desde Atenciones para liquidación y entrega.

## Recorrido habitual y excepciones (revisión de navegación)

1. El alta registra solicitudes e ingreso y deja la atención en preparación técnica. Disponible/reservada se expresa por la reserva; no se inventa otro estado. El esquema nuevo admite solo los siete estados actuales; no conserva las rutas de apertura, diagnóstico pendiente ni pausa general anteriores. Las pausas actuales se registran por alcance para no detener tareas independientes.
2. En Atenciones se abre Detalle; allí están Ingreso y solicitudes, el expediente consolidado y el acceso a progreso. El expediente reúne informes, versiones de presupuestos, ejecución, hallazgos e historial. Recepción también consulta liquidación y medios de cobro; el técnico no recibe funciones financieras. Trabajos en curso conserva su acceso directo al progreso.
3. En presupuesto, Marcar como informado registra la comunicación ya efectuada fuera del sistema; no envía mensajes. Después ofrece registrar respuesta ahora o dejarla pendiente. La confirmación distingue aceptación total, selección parcial y rechazo. Pedir corrección/cambios queda como acción secundaria; revisar un acuerdo aceptado es una excepción que exige nueva preparación y nueva aceptación y no admite ejecución/reserva/liquidación previas.
4. Iniciar ejecución se presenta encima de la lista. Antes de tomar, la grilla es de consulta; después se edita y Guardar avance es la acción habitual. Con todas las tareas completadas se habilita conformidad y Finalizar trabajo. Otras acciones agrupa hallazgos, pausas, materiales y relevo; no modifica la reserva ni finaliza por abrirlo. Cada opción abre su pestaña.
5. La ampliación de inspección abre Hallazgos. Debe registrarse el motivo y prepararse un adicional de servicios de inspección, distinto de la reparación; requiere comunicación/aceptación antes de ejecutarse. Los problemas identificados mediante la revisión habitual se documentan directamente en el informe, sin cargos adicionales automáticos.
6. La cuenta muestra Emitir liquidación antes del documento, Registrar cobro mientras haya saldo y Registrar entrega cuando esté saldada. Se ocultan operaciones anteriores. Anular pago es una corrección administrativa destacada como sensible, conserva el original y no efectúa devoluciones bancarias.
7. Usar restante completa saldo menos los medios preparados en ese cobro; no redondea, no cobra ni omite referencias. La referencia electrónica identifica la operación de pago (transferencia/billetera o cupón/autorización de tarjeta), no una factura ni datos completos de tarjeta. Efectivo puede dejarla vacía.
8. La entrega pide solo persona que retira (prefijada con el cliente), conformidad y observación opcional. El resultado procede del cierre técnico. El servicio valida responsable, conformidad, estado, versión y saldo; el registro queda en el historial de la atención sin duplicar tablas. Después se confirma el cierre y se conserva una cuenta de consulta e impresión con datos de entrega.

El cargador crea Demo v5 íntegramente mediante el circuito vigente en una base vacía. Los ejemplos cancelados corresponden a cancelación actual previa a actividad, no a aperturas de un circuito retirado. Una base Demo ya cargada no se mezcla ni se vuelve a sembrar desde el arranque de la aplicación.

## Seguimiento contextual y detenciones

El selector no filtra grillas: indica el alcance del próximo hallazgo o pausa técnica. Solo aparece en esas dos pestañas, con una etiqueta específica. Avance y materiales no lo muestran. Preparar adicional requiere seleccionar el hallazgo; resolver pausa requiere seleccionar una pausa activa; recibir materiales requiere seleccionar una fila acordada a cargo del cliente.

Recepción registra una **detención solicitada por el cliente**, general y con motivo. No puede resolverla ni crear pausas de una tarea específica. Debe avisar también directamente al taller. La ejecución consulta pausas con un contexto independiente cada diez segundos: muestra aviso, bloquea la edición del alcance afectado y conserva las ediciones locales. No es comunicación instantánea garantizada ni un control físico de la reparación. Si no se puede comprobar la continuidad, se bloquea la edición hasta recuperar la consulta. El servicio mantiene validación de versión y pausas al guardar, incluso entre consultas. El técnico documenta la resolución; aceptar un adicional no levanta pausas automáticamente. Ante ediciones anteriores a una detención, consultar/resolver y volver a abrir con los datos actuales antes de registrar continuidad; no se fusionan cambios obsoletos automáticamente.

Las inspecciones extraordinarias aceptadas se registran desde preparación técnica, pestaña **Inspecciones autorizadas**, como No iniciado, En curso o Completado; todas deben completarse antes de finalizar la evaluación. La reparación se presupuesta aparte. La prueba de circuito comprueba que rechazar después la reparación no elimina el cobro de la inspección autorizada y realizada.

Para materiales del cliente, recepción registra la solicitud de cambio y el técnico prepara una propuesta viable que conserva el material con origen Cliente y precio cero. Se comunica y acepta nuevamente. El técnico registra cantidad recibida y aptitud en Materiales del cliente antes de utilizarlo; no se retira silenciosamente del acuerdo ni se convierte su recepción en una venta del taller.

## Datos de decisión y lectura de documentos

La persona que decide es el cliente o su representante (por ejemplo, responsable de una flota), no el recepcionista. El usuario que registra se obtiene de la sesión y queda asociado a la decisión e historial. El medio describe cómo llegó la respuesta; no implica enviar mensajes desde el sistema. El documento de presupuesto para el cliente conserva identidad del taller, atención, cliente, vehículo, alcance, importes, estado y condiciones útiles. Omite los metadatos internos de comunicación y secciones vacías; esos datos no se eliminan de los registros internos. El informe técnico adjunto identifica a su técnico responsable; no se infiere un autor técnico del usuario creador de una revisión administrativa.

Adjuntar informe técnico en un pedido definido permite documentar una comprobación; no agrega un cargo ni autoriza reparaciones. Cambiar el concepto seleccionado reinicia la cantidad de nueva carga en uno sin modificar filas existentes. Las cabeceras de operación utilizan un estilo compartido de Presentación y los mensajes de cierre técnico/ingreso identifican la atención y vehículo para facilitar comunicaciones internas externas a la aplicación.

El informe de seguimiento es una copia opcional para comunicar avance: ofrece impresión/PDF y HTML secundario, no forma parte obligatoria de la ejecución ni acredita pagos. En vista previa, hacer clic sobre la hoja para usar flechas y rueda; con zoom ampliado desplazan la lectura, RePág/AvPág cambian página, Inicio/Fin van a extremos y Ctrl+rueda o +/− ajustan zoom. Comprobar en Windows también el desplazamiento con 100 % y 150 % y documentos de varias páginas.

## Cobrar y entregar en momentos distintos

La liquidación detalla lo autorizado y efectivamente realizado y determina la deuda; puede imprimirse antes del primer cobro para informar qué debe abonarse. No acredita un pago. Cada recibo corresponde a un cobro concreto, identifica sus medios/referencias y conserva el saldo histórico al registrarlo. Un recibo anulado se conserva marcado y no computa como cobro vigente. Los documentos económicos son internos, sin integración fiscal.

La cuenta mantiene acciones ordenadas y una selección inicial de recibo aunque no se haya abierto su pestaña. Cambiar entre conceptos y recibos no debe hacer aparecer funciones por primera vez. Las funciones de anulación siguen dependiendo de una cuenta abierta y un pago no anulado; imprimir un recibo anulado conserva su identificación.

El pago total no entrega ni cierra automáticamente. Recepción puede cerrar la ventana y registrar el retiro más tarde por dos accesos equivalentes: Atenciones → filtro **Pendientes de entrega** → **Registrar entrega** (directo), o Comprobantes y pagos → cuenta → Registrar entrega. El acceso directo consulta otra vez la cuenta y usa el mismo formulario y servicio de entrega; no pasa por el detalle ni duplica reglas. El filtro incluye solo cierre técnico con liquidación existente y saldo cero; el servicio vuelve a validar estado, reserva, versión y saldo real antes de cerrar. La entrega continúa requiriendo responsable y conformidad explícita.

Prueba: pagar parcialmente (entrega bloqueada), saldar, cerrar cuenta, volver a Atenciones y filtrar pendientes, registrar entrega y comprobar que desaparece de pendientes pero permanece en el historial. Abrir la misma cuenta desde Atenciones y desde Comprobantes y comparar botones, selección e impresión antes y después de cambiar de pestaña. Los mensajes de decisiones, cobros y entrega identifican atención y dominio para facilitar el aviso interno por los medios del taller; el sistema no envía mensajes externos automáticamente.

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

La migración `SolicitudesIndividualesRecepcion` agrega pedidos individuales, datos de recepción, vínculo del detalle presupuestado con el pedido y datos de comunicación de la decisión. No reemplaza comprobantes ni altera cantidades ejecutadas. Las atenciones anteriores conservan su motivo como una solicitud única; las mixtas históricas no se dividen mediante inferencias automáticas. Los presupuestos históricos pueden conservar el vínculo de solicitud vacío.

Antes de actualizar una base utilizada, conservar un respaldo y probar sobre una copia. Con la solución actualizada, aplicar `Update-Database -Project Taller.Infraestructura -StartupProject Taller.Presentacion -Context TallerDbContext` apuntando a la base de prueba configurada. No es necesario volver a cargar la demostración ni eliminar la base.

## Comprobación manual con dos usuarios

Usar recepción y técnico sobre la misma base de demostración y la misma versión del programa. Probar una atención nueva por cada caso: solo trabajo conocido; solo síntoma; trabajo y síntoma en la misma visita. Anotar número de atención, propuesta, cantidades, liquidación y recibos. Actualizar las bandejas al cambiar de rol.

Verificar que el técnico recibe todos los pedidos; que el caso conocido no obliga a generar diagnóstico; que el mixto exige cubrir ambos pedidos; que no se puede ejecutar sin aceptación; que la conformidad técnica no equivale al cierre; que un pago parcial impide entregar; y que el segundo pago permite cerrar mediante entrega. Comprobar historial y persistencia al volver a abrir cada pantalla.

## Alternativas conservadas

Las revisiones, decisiones parciales, hallazgos, pausas, adicionales, materiales del cliente e interrupciones existentes se conservan. Se accede a ellas mediante acciones específicas. No forman pasos obligatorios del recorrido principal. Una interrupción justificada permite liquidar cantidades parciales registradas y omitir lo no realizado. Lo autorizado no ejecutado no se cobra. La autorización de una inspección cobrable requiere su propio acuerdo previo; no surge automáticamente por registrar un síntoma.

Los documentos económicos del sistema son internos; no constituyen integración de facturación fiscal.

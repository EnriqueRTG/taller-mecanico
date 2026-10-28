# Probar decisiones, ejecución, cobros y entrega

Usar [actualización y Demo v4](../instalacion/actualizacion-coloquio.md). Las cuentas de SGTM son distintas de la identidad SQL de cada estación.

| Caso | Comprobación |
|---|---|
| Principal | Recepción abre, técnico propone, recepción informa/acepta, técnico toma y completa, recepción liquida/cobra/entrega. |
| Selección parcial exacta | Recepción desmarca y registra autorización condicionada. Ejecución bloqueada hasta validar. Técnico no cambia contenido y valida: mismo presupuesto aceptado, sin otra decisión. Solo se ejecutan los elegidos; el historial conserva ambas intervenciones. |
| Reformulación | Cambiar concepto, descripción, cantidad, precio, solicitud u origen: propuesta nueva pendiente de presentación. No ejecutar antes de la nueva aceptación. |
| Corrección técnica | Solicitar corrección de propuesta emitida. El técnico reformula y vuelve a recepción; no interpretar como una autorización parcial del cliente. |
| Permisos/versión | Recepción no valida técnicamente. Dos técnicos sobre una versión: la segunda operación debe actualizarse. No repetir la validación ya aplicada. |
| Rechazo | No autorizar la reparación. Cobrar solo inspecciones aceptadas y completadas; si no hay cargos, liquidación cero con motivo y sin pago ficticio. |
| Tres estados | No iniciado exige cero; En curso admite avance menor al total; Completado exige el total. Estados retirados se rechazan también en SQL. |
| Finalización | Un ítem En curso/No iniciado bloquea finalizar aunque se marque conformidad. Todos completos sin conformidad también bloquean. Todos completos y conformidad permiten terminar. |
| Continuidad | Guardar avance, liberar y continuar con otro técnico conserva cantidad/autorización. No disminuir lo realizado ni editar una reserva ajena. |
| Hallazgo y pausa | Técnico registra informe. Si bloquea, pausa el alcance; otros trabajos independientes pueden continuar. Adicional requiere aceptación y resolver la pausa antes de retomar. |
| Material del cliente | Origen cliente a precio cero exige recepción conforme antes de consumo. Cambiar el origen durante la revisión requiere nueva aceptación. |
| Cobros | Registrar parciales/mixtos, usar restante, comprobar suma y referencias. Reintentar el mismo cobro no duplica recibo. No pagar más que saldo. |
| Concurrencia de cobros | Dos estaciones con la misma versión: una confirma, la otra actualiza; no sobrepago. |
| Anulación | Motivo, recibo conservado como anulado y saldo repuesto; no equivale a devolución bancaria. No anular tras entrega. |
| Entrega | Bloquear con deuda; con saldo cero registrar quién retira y conformidad. Puede hacerse después del cobro desde Pendientes de entrega. |
| Visibilidad | Sin selección o etapa aplicable no mostrar acciones operativas. Seleccionar otra fila/actualizar debe recalcularlas. Comprobar recepción/técnico/admin y formularios a 1366×768 con DPI 100/125/150 %. |
| Reportes | Comparar detalle, indicadores, gráfico, HTML/CSV/PDF. Si hay Resto, su composición/valores aparecen en metodología. |

El cierre con interrupción y el inventario no forman parte de esta entrega. Los cargos se calculan desde cantidad efectivamente ejecutada y precio acordado, no desde el precio actual del catálogo. No se afirma que la compilación sustituya la aceptación visual ni la prueba en dos equipos.

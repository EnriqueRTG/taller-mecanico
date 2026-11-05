# Alcance implementado

Este documento describe el comportamiento vigente. La ERS, los apéndices A/C y los diagramas v2.0 describen este mismo alcance; la matriz de trazabilidad enlaza sus requisitos con los servicios y pruebas.

## Flujo principal

1. Recepción identifica cliente/vehículo y registra solicitudes individuales conocidas o síntomas. La atención entra en preparación técnica sin servicios cobrables automáticos.
2. El técnico toma la actividad y prepara el presupuesto; los síntomas requieren informe técnico. Un trabajo definido puede presupuestarse sin inventar diagnóstico.
3. Recepción comunica la propuesta y registra persona que decide, medio e ítems. El usuario que registra procede de la sesión.
4. La aceptación completa permite tomar ejecución. El técnico registra cantidades realmente realizadas, estados y conformidad final.
5. Recepción emite liquidación de lo autorizado y realizado; registra cobros parciales/mixtos y recibos.
6. La entrega requiere que la atención esté habilitada para cobros, que exista una liquidación incluso si el importe es cero, y que el saldo sea exactamente cero y coincida con los pagos vigentes. Recepción registra quién retira y su conformidad; la atención pasa a Cerrada.

## Alternativas

- Selección parcial: queda pendiente de revisión técnica. Si el técnico valida exactamente lo elegido, queda aceptada sin otra comunicación obligatoria. Si cambia las condiciones, los conceptos, las solicitudes asociadas, la descripción, la cantidad, el precio o quién aporta el material, se genera una nueva versión por presentar y se requiere otra decisión del cliente. La selección por sí sola no habilita ejecución.
- Corrección de propuesta: recepción solicita corrección sobre una propuesta formalizada, vigente y sin ejecución, con las restricciones de reserva/liquidación. No equivale a editar importes ni a aceptar por el cliente. Si se corrige un adicional, la nueva versión comienza como borrador sin decisión registrada. Al terminar la corrección, la versión resultante queda por presentar y requiere aceptación incluso si conserva los mismos ítems.
- Rechazo: no autoriza tareas rechazadas. Una inspección previamente autorizada y efectivamente realizada puede tener cargo; sin cargos se justifica liquidación cero, sin pago ficticio.
- Inspección extraordinaria: tiene su propio presupuesto limitado a servicios y aceptación previa; se registra en Inspecciones autorizadas antes de la propuesta de reparación.
- Hallazgo en ejecución: informe puntual, pausa por alcance si impide continuar y presupuesto adicional independiente cuando corresponde. No reescribe lo ya aceptado. Aceptar un adicional no resuelve automáticamente una pausa.
- Material del cliente: repuesto/consumible a precio de venta cero, recepción y conformidad técnica antes de utilizarse. Los servicios los ofrece el taller. No supone compras/inventario del negocio.
- Liberar actividad conserva avances guardados. Una cantidad parcial sigue EnCurso. No se permite el cierre definitivo con trabajos incompletos.
- Anular un cobro conserva su recibo marcado, usuario/motivo y recompone el saldo antes de la entrega. No devuelve dinero por un banco.

## Documentos, seguridad y consultas

Presupuestos conservan revisiones; la liquidación conserva sus conceptos/importes y cada cobro su recibo con medios y saldo histórico. La liquidación informa deuda y puede emitirse antes de cobrar; un recibo acredita un registro de cobro. Son documentos internos en ARS, sin validez fiscal.

Los siete reportes aplican alcance por rol, filtros, indicadores, gráficos y detalle. No estiman costos de compra, utilidad ni horas que no se registran. Sesión hasta cierre explícito, autorización persistida por operación, contraseñas con hash y protección de intentos. Restauración mantiene doble aprobación, validación de copia, ensayo y mantenimiento técnico.

## Respaldo y recuperación en la entrega

El administrador puede crear y verificar una copia manual. Puede ensayar la recuperación en una base temporal, con comprobación de integridad y esquema, sin modificar la base activa. La restauración efectiva es una operación separada: exige dos cuentas distintas y una ventana de mantenimiento; genera una copia protectora y reemplaza los datos posteriores al punto seleccionado.

En la instalación de demostración se comprobó la copia completa local y su recorrido manual por Google Drive: exportación, sincronización visible en la web, descarga, importación y ensayo satisfactorio del paquete recuperado. No se ejecutó una restauración efectiva de la base activa en ese recorrido. El agente y la programación automática están implementados como opciones, pero quedan fuera de la validación manual de la entrega.

## Fuera del alcance actual

Cierre con interrupción, ABM de catálogo desde formularios, inventario, compras/proveedores, reposición por mínimos, turnos/capacidad, integración fiscal/bancaria, anticipos, financiación y devolución bancaria automática. Los respaldos reales requieren configuración local/SQL y, para nube, carpeta de Google Drive para escritorio y sesión Windows activa; disponer del formulario no significa que ese entorno ya esté instalado.

## Esquema y evidencia

Las seis migraciones vigentes crean y ajustan el modelo, los tres estados de ejecución, el teléfono obligatorio, la concurrencia de usuarios y el contacto de las decisiones. Ver la guía de actualización para aplicarlas completas. No existe una cadena de conversión de bases anteriores. Atención es el expediente principal; no se agrega una orden de trabajo paralela. Se preservan claves/rowversion y documentos asociados; las pausas se representan por alcance, no por el antiguo estado general Pausada.

Las pruebas de reglas/componentes, SQL aislado y Windows/STA están en los proyectos de pruebas. La revisión visual del circuito y de impresión se realiza en la instalación de presentación. Ver [estados](../arquitectura/estados-circuito-actual.md), [alternativas](../operacion/alternativas.md) y [reportes](../reportes/reportes-por-rol.md).

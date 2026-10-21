# Atención, evaluación y propuesta

Este documento describe la etapa de apertura, evaluación y propuesta. La extensión ya implementada de decisiones por ítem, ejecución, liquidación, pagos parciales y entrega se documenta en [la adenda funcional v1.0](ERS/ADENDA-v1.0-presupuestos-cobros.md) y [su guía de pruebas](probar-decision-ejecucion-cobros.md). Esa adenda prevalece sobre las referencias a trabajo futuro de esta primera etapa.

## Circuito y decisiones

| Solicitud en recepción | Intervención técnica | Documento resultante |
| --- | --- | --- |
| Síntoma o problema | Registrar conclusión y trabajo recomendado; preparar ítems si corresponde | Diagnóstico e ítems de propuesta, o informe sin propuesta con motivo |
| Servicio definido | Verificar alcance y preparar ítems; diagnóstico opcional si aparece una falla que requiere informe | Presupuesto vinculado a la atención, con o sin diagnóstico |
| Mixta | Evaluar el problema y el servicio solicitado en el mismo espacio de trabajo | Diagnóstico y propuesta |

Cliente y vehículo se identifican antes de abrir la atención. La atención es el expediente que enlaza el motivo original, reservas, informe, propuesta e historial. El tipo de solicitud describe lo pedido; no implica que el cliente haya autorizado reparar.

Desde **Detalle de atención**, Recepción puede solicitar evaluación sin cargos iniciales, o preparar uno o más servicios disponibles para ingreso y registrar que el cliente autorizó sus cantidades e importes. Es válido un precio cero. La selección preparada se pierde si se cierra sin confirmar; no se crea un cargo automáticamente por abrir una atención.

Los servicios iniciales se guardan como un documento aceptado de tipo `Diagnostico` (nombre histórico del enum, conservado para compatibilidad). Representa servicios iniciales autorizados, no un diagnóstico ya terminado, una ejecución ni un cobro. Se mantiene separado de la propuesta de reparación para impedir duplicarlos. Solo los efectivamente ejecutados deben considerarse al liquidar en la siguiente entrega; omitir un servicio requiere motivo. Un presupuesto de reparación rechazado no anula por sí solo servicios iniciales efectivamente autorizados y realizados.

**Evaluaciones técnicas** reemplaza la antigua alta aislada de diagnóstico. Tomar una atención reserva la actividad y abre el mismo espacio de trabajo con tres pestañas: informe/conclusión, propuesta y servicios iniciales. El técnico que conoce el hallazgo prepara los ítems sin volver a transcribir el informe en otro módulo.

- **Guardar avance** persiste textos, ítems y ejecución de servicios iniciales; conserva la reserva. No publica un informe definitivo.
- **Cerrar** no cambia estados ni libera la reserva; avisa si hay modificaciones sin guardar.
- **Liberar evaluación** en la bandeja permite que otro técnico continúe los avances guardados. Conserva el historial de quién guardó y quién finalizó; el autor del informe definitivo es quien lo finaliza.
- **Finalizar evaluación y propuesta** exige informe para problema/mixta, resuelve servicios iniciales y guarda todo en una operación atómica. El informe queda histórico y la propuesta pendiente de presentación. No autoriza reparar.
- **Finalizar sin propuesta** exige motivo y ningún ítem propuesto; elimina únicamente el borrador de propuesta no presentado, conserva el informe y servicios iniciales, y deja la atención pendiente de liquidación/revisión de cargos. Esto no confirma ningún cobro ni que deba cobrarse.

Recepción consulta la propuesta en **Presupuestos**. **Registrar comunicación al cliente** confirma que informó alcance e importes y pasa a pendiente de decisión. No representa aceptación. Los borradores se continúan desde Evaluaciones técnicas. Las revisiones preservan el presupuesto presentado y producen una nueva versión antes de ejecutar. La decisión puede excluir ítems sin borrarlos del documento original; véase la adenda para el circuito completo.

## Organización

- Dominio: `TipoSolicitudAtencion`, `EvaluacionAtencion` y presupuesto siempre vinculado a atención; diagnóstico opcional para propuestas de servicio definido.
- Aplicación: DTOs explícitos, `EvaluacionServicio` para coordinar el caso de uso, `DiagnosticoServicio` para consultar informes históricos, interfaces de persistencia y unidad de trabajo.
- Infraestructura: `EvaluacionRepositorio` carga el expediente de trabajo con seguimiento; no confirma cambios. La unidad de trabajo confirma conjuntamente atención, avance, informe, detalles e historial. Se conservan los repositorios existentes para recepción y consultas de diagnóstico.
- Presentación: controles, estructura, columnas y eventos en archivos `.Designer.cs`; datos, validaciones de interacción y llamadas a servicios en el otro archivo parcial. No se suscriben eventos duplicados en constructores.

Un DTO es un contrato de transferencia de datos, no por sí solo una arquitectura ni un patrón completo. La estructura combina servicios de aplicación, repositorios y unidad de trabajo. No se agregó un framework de CQRS ni un repositorio genérico.

## Integridad y listados

Cada guardado de evaluación usa la versión de Atención leída por el formulario, y fuerza su actualización incluso al guardar un borrador sin cambiar la etapa. SQL Server comprueba `rowversion`; otro técnico/otra reserva invalida la operación. Los avances tienen además su propia versión. El informe, presupuesto y estado se guardan en el mismo `SaveChangesAsync`, con la transacción de EF.

La FK compuesta impide asociar a un presupuesto un diagnóstico de otra atención. Hay un único borrador de reparación y un único documento de servicios iniciales por atención. Importes, duplicados, actividad, rol activo y disponibilidad de conceptos se comprueban en Aplicación además de las restricciones SQL.

Bandeja técnica: mis actividades, disponibles, actividades ajenas y finalizadas. Las disponibles se atienden por antigüedad del último movimiento; finalizadas por movimiento más reciente. `Último movimiento` surge del historial, no de la sesión en línea del usuario. Recepción mantiene su listado de atenciones con filtros de fecha opcionales y cerradas/canceladas al final. Presupuestos: por presentar, borradores, esperando decisión y otros documentos; dentro de cada grupo, recientes primero. Cerradas/canceladas no se ofrecen como actividades técnicas.

La aplicación conserva el contexto scoped existente; se impide cambiar de módulo/cerrar sesión durante las cargas y guardados de estas bandejas. Cada operación termina descartando seguimiento para que la siguiente lea datos actuales. Para una futura aplicación con trabajo en segundo plano simultáneo debe evaluarse crear un contexto por operación.

Los formularios nuevos usan tablas con relleno, columnas manuales que ocupan el ancho disponible y permiten redimensionado. La base de prueba visual es 1366×768 y 1920×1080; el tamaño físico en pulgadas no determina el espacio útil. Es necesario verificar también escalado de Windows, especialmente 125 % y 150 %.

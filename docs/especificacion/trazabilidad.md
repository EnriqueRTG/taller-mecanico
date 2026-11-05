# Trazabilidad de requisitos e implementación

Versión documental 2.0. Referencia de código para el cierre: `ef2b2e8` (PR #75). CU01 a CU10 son comunes a la ERS, al Apéndice A y a los diagramas. Los enlaces apuntan a clases reales; las pruebas automatizadas complementan la aceptación manual. No representan aprobación de una instalación aún no probada.

| CU | RF | Servicio de Aplicación | Datos principales | Evidencia automatizada |
|---|---|---|---|---|
| CU01 Iniciar y cerrar sesión | RF-01, RF-02, RF-03, RF-04, RF-05 | [AutenticacionServicio](../../Taller/Taller.Aplicacion/Servicios/AutenticacionServicio.cs) | Usuarios, Roles, ControlesAcceso, EventosAcceso | [SeguridadInicioPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/SeguridadInicioPruebas.cs) |
| CU02 Gestionar usuarios | RF-06, RF-07, RF-08, RF-09, RF-10, RF-11, RF-73 | [UsuarioServicio](../../Taller/Taller.Aplicacion/Servicios/UsuarioServicio.cs), [AuditoriaAccesoServicio](../../Taller/Taller.Aplicacion/Servicios/AuditoriaAccesoServicio.cs) | Usuarios, Roles | [UsuariosValidacionPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/UsuariosValidacionPruebas.cs) |
| CU03 Gestionar clientes y vehículos | RF-12, RF-13, RF-14, RF-15, RF-16, RF-17, RF-18, RF-19, RF-20, RF-65 | [ClienteServicio](../../Taller/Taller.Aplicacion/Servicios/ClienteServicio.cs), [VehiculoServicio](../../Taller/Taller.Aplicacion/Servicios/VehiculoServicio.cs), [ConceptoServicio](../../Taller/Taller.Aplicacion/Servicios/ConceptoServicio.cs) | Clientes, Vehiculos, Modelos, Marcas | [MaestrosPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/MaestrosPruebas.cs) |
| CU04 Gestionar atención y entrega | RF-21, RF-22, RF-23, RF-24, RF-25, RF-27 | [AtencionServicio](../../Taller/Taller.Aplicacion/Servicios/AtencionServicio.cs), [EvaluacionServicio](../../Taller/Taller.Aplicacion/Servicios/EvaluacionServicio.cs), [CobroServicio](../../Taller/Taller.Aplicacion/Servicios/CobroServicio.cs) | Atenciones, SolicitudesAtencion, HistorialAtenciones | [CircuitoCobrosPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/CircuitoCobrosPruebas.cs) |
| CU05 Preparar evaluación e informe técnico | RF-28, RF-29, RF-30, RF-31, RF-32 | [EvaluacionServicio](../../Taller/Taller.Aplicacion/Servicios/EvaluacionServicio.cs), [DiagnosticoServicio](../../Taller/Taller.Aplicacion/Servicios/DiagnosticoServicio.cs) | EvaluacionesAtencion, Diagnosticos, Presupuestos | [EvaluacionServicioPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/EvaluacionServicioPruebas.cs) |
| CU06 Gestionar propuestas y decisiones | RF-33, RF-34, RF-35, RF-36, RF-37, RF-38, RF-39, RF-40, RF-41, RF-42, RF-69 | [EvaluacionServicio](../../Taller/Taller.Aplicacion/Servicios/EvaluacionServicio.cs), [PresupuestoServicio](../../Taller/Taller.Aplicacion/Servicios/PresupuestoServicio.cs), [SeguimientoTrabajoServicio](../../Taller/Taller.Aplicacion/Servicios/SeguimientoTrabajoServicio.cs) | Presupuestos, PresupuestoDetalles, HistorialAtenciones | [CircuitoCobrosPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/CircuitoCobrosPruebas.cs) |
| CU07 Ejecutar y seguir trabajos autorizados | RF-26, RF-66, RF-67, RF-68, RF-70 | [EjecucionServicio](../../Taller/Taller.Aplicacion/Servicios/EjecucionServicio.cs), [SeguimientoTrabajoServicio](../../Taller/Taller.Aplicacion/Servicios/SeguimientoTrabajoServicio.cs) | PresupuestoDetalles, HallazgosAtencion, PausasTrabajo | [CircuitoCobrosPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/CircuitoCobrosPruebas.cs) |
| CU08 Liquidar y registrar cobros | RF-43, RF-44, RF-45, RF-46, RF-47, RF-48, RF-49, RF-50, RF-51, RF-71 | [CobroServicio](../../Taller/Taller.Aplicacion/Servicios/CobroServicio.cs) | Comprobantes, ComprobanteDetalles, Pagos, PagoMedios, MediosPago | [CircuitoCobrosPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/CircuitoCobrosPruebas.cs) |
| CU09 Generar reportes por rol | RF-52, RF-53, RF-54, RF-55, RF-56, RF-57 | [ReporteServicio](../../Taller/Taller.Aplicacion/Servicios/ReporteServicio.cs) | Consultas de negocio e historial; sin tabla de reporte | [ReportesPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/ReportesPruebas.cs) |
| CU10 Gestionar respaldos y recuperación | RF-58, RF-59, RF-60, RF-61, RF-62, RF-63, RF-64, RF-72, RF-73 | [RespaldoServicio](../../Taller/Taller.Aplicacion/Servicios/RespaldoServicio.cs) | Base de negocio y control externo EstadoSGTM, ProgramaSGTM, CopiasSGTM, EventosSGTM | [RespaldosPruebas](../../Taller/Taller.Pruebas/ReglasYComponentes/RespaldosPruebas.cs), [RespaldosSqlPruebas](../../Taller/Taller.Pruebas/IntegracionSql/RespaldosSqlPruebas.cs) |

## Reglas de transición para leer los diagramas

El estado de la atención, el estado del presupuesto y el avance de cada ítem representan momentos distintos del expediente. El [estado de atención](../../Taller/Taller.Dominio/Enumeraciones/EstadoAtencion.cs) gobierna la etapa general; el [estado de presupuesto](../../Taller/Taller.Dominio/Enumeraciones/EstadoPresupuesto.cs) registra la situación de cada propuesta. Una pausa tiene alcance propio y se registra aparte.

| Regla | Diagrama | Implementación |
|---|---|---|
| CU06: una selección parcial idéntica puede validarse técnicamente y conservar la decisión original. Cualquier cambio requiere presentar una nueva versión. | [Vista 5](../diagramas/vista-5.svg) | `SeguimientoTrabajoServicio.RevisarSeleccionAsync` |
| CU06: una corrección de adicional empieza como borrador y requiere nueva decisión aun si conserva los ítems. | [Vista 9](../diagramas/vista-9.svg) | `PresupuestoServicio` y `SeguimientoTrabajoServicio.RevisarSeleccionAsync` |
| CU04/CU08: la entrega requiere liquidación, saldo cero consistente con los pagos vigentes, persona que retira y conformidad. | [Vistas 4](../diagramas/vista-4.svg) y [6](../diagramas/vista-6.svg) | `CobroServicio.EntregarAsync` |

## Recorrido entre capas

Formulario → servicio de Aplicación → autorización y validaciones → contrato repositorio/unidad de trabajo → implementación Infraestructura → SQL Server. DTOs transportan datos; las entidades pertenecen a Dominio. La composición DI está en los registros de cada capa y Program de Presentación.

EvaluacionServicio.GuardarAsync coordina avance, borrador, informe definitivo e historial. Los detalles editables del borrador se reemplazan; no son versiones inmutables por cada pulsación. Tras formalizar, las revisiones conservan documentos previos. SeguimientoTrabajoServicio.RevisarSeleccionAsync distingue validación exacta de reformulación.

CobroServicio.RegistrarPagoAsync usa IdOperacion único e idempotencia; conserva Pago, PagoMedios y suma de Comprobante. UnidadDeTrabajo.GuardarCambiosAsync confirma con SaveChangesAsync: EF crea la transacción SQL implícita de esos cambios. El lote Demo usa BeginTransactionAsync explícito. Una consulta de reporte no necesita una transacción que cubra toda la atención.

RespaldoServicio aplica permisos y doble aprobación. RespaldoSqlServer ejecuta copias/ensayo/recuperación con conexiones SQL independientes; RegistroRespaldosSql conserva el control fuera de la base restaurable. El mantenimiento y los bloqueos de aplicación no equivalen a una transacción única durante toda la recuperación.

## Esquema y evolución

Migraciones, snapshot y configuraciones deben coincidir. RetirarVinculoCambioSinUso elimina una columna nullable, índice, FK y CHECK sin uso. Se bloquea si encuentra datos no nulos. Las migraciones anteriores siguen en el repositorio porque reconstruyen bases y documentan cambios aplicados; no son código ejecutado por la operación normal.

Los archivos de diagramas derivan del mismo modelo. Las tablas ref. repetidas en vistas del DER no son tablas nuevas. Los proyectos auxiliares y pruebas tienen responsabilidades activas y no se eliminan como si fueran capas adicionales.

## Verificación reproducible

La [evidencia manual de entrega](../pruebas.md#pruebas-evidencia-cierre) identifica los casos y recorridos observados, y separa los límites de esa prueba de la cobertura automatizada.

Consultar [plan integral](../pruebas.md#pruebas-aceptacion-version-actual), [SQL de contraste](../../scripts/recursos/verificacion-reportes.sql) y [estado de verificación](../pruebas.md#pruebas-evidencia-cierre). Para CU10, el [alcance implementado](../../REFERENCIA-TECNICA.md#especificacion-alcance-implementado) distingue la evidencia manual en la instalación de demostración de las pruebas automatizadas y de la restauración efectiva.

La segunda revisión agrega Usuarios.Version y guardias de edición en sus SP (CU02/RNF-05), unifica la entrega estructurada (CU04/RF-27) y revalida maestros del alta en transacción breve (CU04/RNF-04). Evidencia: Usuarios_SPRechazanVersionObsoletaEnDatosCredencialesYEstado, AltaAtencion_RevalidaMaestrosAntesDeGuardarYPermiteReintentarSinResiduos y AtencionPersistenciaPruebas. Véase [segunda revisión](https://github.com/EnriqueRTG/taller-mecanico/blob/4404dd1/docs/pruebas/revision-integral-2.md).

## Tercera revisión

- Decisión con persona y medio: PresupuestoServicio y CK_Presupuestos_ContactoDecision; prueba SQL DecisionPersistida_ExigeContactoYLaMigracionNoInventaDatos.
- Arranque concurrente: InicializadorDatos, bloqueo SQL de transacción; InicializacionConcurrente_CreaUnaSolaCuentaYCatalogoSinDuplicados.
- Lectura de cuenta: CobroServicio consulta mediante IUnidadDeTrabajo.LeerConsistenteAsync; aislamiento Serializable limitado a la consulta, sin conservarlo mientras se opera la ventana.
- Reportes: estados validados por ReporteServicio; conciliación de los siete tipos sobre el lote SQL en DemostracionSqlPruebas.
- Unicidad del diagnóstico: UX_Diagnosticos_Atencion y traducción en UnidadDeTrabajo; prueba SQL de duplicado sin residuos.

## Revisión vigente de simplicidad y capas — 04/11/2026

- Búsqueda común: BusquedaTexto en reportes y bandejas. ReportesPruebas compara los siete tipos con/sin tildes y mayúsculas y conserva indicadores y gráficos.
- Actividad técnica: selección de hitos; las correcciones adicionales quedan en el expediente sin un filtro nuevo. No equivale a auditoría completa ni productividad.
- Recepción: AtencionServicio normaliza una vez las solicitudes y deriva el tipo/motivo; conserva la revalidación transaccional de maestros y el índice de vehículo activo.
- Fallos de maestros: los repositorios conservan la pila de la excepción técnica desconocida. ErroresPersistenciaPruebas cubre altas y ediciones; no se expone el detalle SQL al operador.
- Paquete Drive: manifiesto incompleto o tipo desconocido se rechaza antes de importar datos. PaqueteRespaldoPruebas comprueba mensajes y ausencia de modificaciones.
- [Inventario por capas, evidencia y oportunidades](https://github.com/EnriqueRTG/taller-mecanico/blob/4404dd1/docs/pruebas/revision-por-capas-2026-11-04.md).

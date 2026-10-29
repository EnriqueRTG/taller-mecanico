# Trazabilidad de requisitos e implementación

Versión documental 2.0. CU01 a CU10 son comunes a la ERS, al Apéndice A y a los diagramas. Los enlaces apuntan a clases reales; las pruebas automatizadas complementan la aceptación manual. No representan aprobación de una instalación aún no probada.

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

## Recorrido entre capas

Formulario → servicio de Aplicación → autorización y validaciones → contrato repositorio/unidad de trabajo → implementación Infraestructura → SQL Server. DTOs transportan datos; las entidades pertenecen a Dominio. La composición DI está en los registros de cada capa y Program de Presentación.

EvaluacionServicio.GuardarAsync coordina avance, borrador, informe definitivo e historial. Los detalles editables del borrador se reemplazan; no son versiones inmutables por cada pulsación. Tras formalizar, las revisiones conservan documentos previos. SeguimientoTrabajoServicio.RevisarSeleccionAsync distingue validación exacta de reformulación.

CobroServicio.RegistrarPagoAsync usa IdOperacion único e idempotencia; conserva Pago, PagoMedios y suma de Comprobante. UnidadDeTrabajo.GuardarCambiosAsync confirma con SaveChangesAsync: EF crea la transacción SQL implícita de esos cambios. El lote Demo usa BeginTransactionAsync explícito. Una consulta de reporte no necesita una transacción que cubra toda la atención.

RespaldoServicio aplica permisos y doble aprobación. RespaldoSqlServer ejecuta copias/ensayo/recuperación con conexiones SQL independientes; RegistroRespaldosSql conserva el control fuera de la base restaurable. El mantenimiento y los bloqueos de aplicación no equivalen a una transacción única durante toda la recuperación.

## Esquema y evolución

Migraciones, snapshot y configuraciones deben coincidir. RetirarVinculoCambioSinUso elimina una columna nullable, índice, FK y CHECK sin uso. Se bloquea si encuentra datos no nulos. Las migraciones anteriores siguen en el repositorio porque reconstruyen bases y documentan cambios aplicados; no son código ejecutado por la operación normal.

Los archivos de diagramas derivan del mismo modelo. Las tablas ref. repetidas en vistas del DER no son tablas nuevas. Los proyectos auxiliares y pruebas tienen responsabilidades activas y no se eliminan como si fueran capas adicionales.

## Verificación reproducible

Consultar [plan integral](../pruebas/aceptacion-version-actual.md), [SQL de contraste](../reportes/verificacion-reportes.sql) y [estado de verificación](../pruebas/evidencia-cierre.md). Ninguna guía debe afirmar que SQL real o Azure se probaron si no hay evidencia de esa ejecución.

La segunda revisión agrega Usuarios.Version y guardias de edición en sus SP (CU02/RNF-05), unifica la entrega estructurada (CU04/RF-27) y revalida maestros del alta en transacción breve (CU04/RNF-04). Evidencia: Usuarios_SPRechazanVersionObsoletaEnDatosCredencialesYEstado, AltaAtencion_RevalidaMaestrosAntesDeGuardarYPermiteReintentarSinResiduos y AtencionPersistenciaPruebas. Véase [segunda revisión](../pruebas/revision-integral-2.md).

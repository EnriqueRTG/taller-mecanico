# Modelo relacional vigente

Referencia: ERS, Apéndices A/C y DER v2.0. El snapshot EF y las cinco migraciones son la evidencia física. El esquema final tiene 24 tablas, 205 columnas y 43 FK; el control técnico agrega cuatro tablas y 24 columnas fuera de EF. No se cuentan como entidades de negocio los metadatos __EFMigrationsHistory.

| Tabla | PK | FK declaradas |
|---|---|---|
| Atenciones | IdAtencion | (IdCliente) → Clientes(IdCliente); (IdTecnicoEnActividad) → Usuarios(Id) opcional; (IdUsuarioRecepcion) → Usuarios(Id); (IdVehiculo) → Vehiculos(IdVehiculo) |
| Categorias | IdCategoria | — |
| Clientes | IdCliente | — |
| Comprobantes | IdComprobante | (IdAtencion) → Atenciones(IdAtencion); (IdUsuarioEmision) → Usuarios(Id) |
| ComprobanteDetalles | IdComprobanteDetalle | (IdComprobante) → Comprobantes(IdComprobante); (IdPresupuestoDetalle) → PresupuestoDetalles(IdPresupuestoDetalle) |
| Conceptos | IdConcepto | (IdCategoria) → Categorias(IdCategoria) |
| ControlesAcceso | Clave | — |
| Diagnosticos | IdDiagnostico | (IdAtencion) → Atenciones(IdAtencion); (IdTecnicoAutor) → Usuarios(Id) |
| EvaluacionesAtencion | IdAtencion | (IdAtencion) → Atenciones(IdAtencion); (IdTecnicoUltimaEdicion) → Usuarios(Id) |
| EventosAcceso | Id | — |
| HallazgosAtencion | IdHallazgoAtencion | (IdAtencion) → Atenciones(IdAtencion); (IdDetalleAfectado) → PresupuestoDetalles(IdPresupuestoDetalle) opcional; (IdTecnico) → Usuarios(Id) |
| HistorialAtenciones | IdHistorialAtencion | (IdAtencion) → Atenciones(IdAtencion); (IdDiagnostico) → Diagnosticos(IdDiagnostico) opcional; (IdUsuario) → Usuarios(Id) |
| Marcas | IdMarca | — |
| MediosPago | IdMedioPago | — |
| Modelos | IdModelo | (IdMarca) → Marcas(IdMarca) |
| Pagos | IdPago | (IdComprobante) → Comprobantes(IdComprobante); (IdUsuarioAnulacion) → Usuarios(Id) opcional; (IdUsuarioRegistro) → Usuarios(Id) |
| PagoMedios | IdPagoMedio | (IdMedioPago) → MediosPago(IdMedioPago); (IdPago) → Pagos(IdPago) |
| PausasTrabajo | IdPausaTrabajo | (IdAtencion) → Atenciones(IdAtencion); (IdDetalle) → PresupuestoDetalles(IdPresupuestoDetalle) opcional; (IdTecnicoResolucion) → Usuarios(Id) opcional; (IdUsuario) → Usuarios(Id) |
| Presupuestos | IdPresupuesto | (IdAtencion) → Atenciones(IdAtencion); (IdHallazgoAtencion) → HallazgosAtencion(IdHallazgoAtencion) opcional; (IdPresupuestoAnterior) → Presupuestos(IdPresupuesto) opcional; (IdUsuarioCreacion) → Usuarios(Id); (IdUsuarioDecision) → Usuarios(Id) opcional; (IdDiagnostico, IdAtencion) → Diagnosticos(IdDiagnostico, IdAtencion) opcional |
| PresupuestoDetalles | IdPresupuestoDetalle | (IdConcepto) → Conceptos(IdConcepto); (IdPresupuesto) → Presupuestos(IdPresupuesto); (IdSolicitudAtencion) → SolicitudesAtencion(IdSolicitudAtencion) opcional; (IdTecnicoMaterial) → Usuarios(Id) opcional; (IdTecnicoRegistro) → Usuarios(Id) opcional |
| Roles | Id | — |
| SolicitudesAtencion | IdSolicitudAtencion | (IdAtencion) → Atenciones(IdAtencion) |
| Usuarios | Id | (RolId) → Roles(Id) |
| Vehiculos | IdVehiculo | (IdModelo) → Modelos(IdModelo) |

## Integridad y cardinalidades

Atención tiene como máximo un diagnóstico definitivo, una evaluación y una liquidación por PK/índices únicos. Un borrador puede carecer de detalles; al presentar se exigen condiciones operativas. La FK diagnóstico/presupuesto es compuesta para impedir diagnóstico de otra atención. Todas las FK usan Restrict/NO ACTION. Los mínimos operativos, permisos, sumas y reservas requieren además validación de Aplicación.

La revisión usa Presupuesto.IdPresupuestoAnterior y Vigente. IdPresupuestoCambio se retiró del modelo activo por no tener uso; aparece únicamente en migraciones históricas y su retirada. No se reescriben migraciones ya aplicadas.

No existe FK Cliente–Vehículo: se relacionan en la Atención. No existe OrdenTrabajo, stock ni tabla Entrega; esta última se documenta en Atención e historial. Saldo actual, números LIQ/REC y total propuesto son valores de consulta; Comprobante.Total y Pago.SaldoPosterior sí se almacenan.

## Control de respaldos

EstadoSGTM, ProgramaSGTM, CopiasSGTM y EventosSGTM se crean por RegistroRespaldosSql en la base de control separada. No tienen FK cruzada con Usuarios ni entre documentos JSON. La recuperación de negocio no debe borrar aprobaciones, catálogo ni calendario.

[DER editable](../diagramas/DER-SGTM-v2.0.drawio) · [Diccionario](../ERS/Apendice-C-Diccionario-de-Datos-v2.0.docx) · [Configuraciones](../../Taller/Taller.Infraestructura/Persistencia/Configuraciones) · [Migraciones](../../Taller/Taller.Infraestructura/Persistencia/Migraciones)

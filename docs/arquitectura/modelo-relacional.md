# Modelo relacional de referencia

Derivado de la migración EsquemaInicialCircuitoActual. Es una referencia de tablas y claves del negocio, no la exportación del DER académico editable. Las restricciones completas están en las configuraciones de Infraestructura y en la migración. La base técnica de respaldos se instala aparte y no se incluye en este esquema de negocio.

| Tabla | Clave primaria | Claves externas |
|---|---|---|
| Categorias | IdCategoria | — |
| Clientes | IdCliente | — |
| ControlesAcceso | Clave | — |
| EventosAcceso | Id | — |
| Marcas | IdMarca | — |
| MediosPago | IdMedioPago | — |
| Roles | Id | — |
| Conceptos | IdConcepto | IdCategoria → Categorias.IdCategoria |
| Modelos | IdModelo | IdMarca → Marcas.IdMarca |
| Usuarios | Id | RolId → Roles.Id |
| Vehiculos | IdVehiculo | IdModelo → Modelos.IdModelo |
| Atenciones | IdAtencion | IdCliente → Clientes.IdCliente; IdTecnicoEnActividad → Usuarios.Id; IdUsuarioRecepcion → Usuarios.Id; IdVehiculo → Vehiculos.IdVehiculo |
| Comprobantes | IdComprobante | IdAtencion → Atenciones.IdAtencion; IdUsuarioEmision → Usuarios.Id |
| Diagnosticos | IdDiagnostico | IdAtencion → Atenciones.IdAtencion; IdTecnicoAutor → Usuarios.Id |
| EvaluacionesAtencion | IdAtencion | IdAtencion → Atenciones.IdAtencion; IdTecnicoUltimaEdicion → Usuarios.Id |
| SolicitudesAtencion | IdSolicitudAtencion | IdAtencion → Atenciones.IdAtencion |
| Pagos | IdPago | IdComprobante → Comprobantes.IdComprobante; IdUsuarioAnulacion → Usuarios.Id; IdUsuarioRegistro → Usuarios.Id |
| HistorialAtenciones | IdHistorialAtencion | IdAtencion → Atenciones.IdAtencion; IdDiagnostico → Diagnosticos.IdDiagnostico; IdUsuario → Usuarios.Id |
| PagoMedios | IdPagoMedio | IdMedioPago → MediosPago.IdMedioPago; IdPago → Pagos.IdPago |
| ComprobanteDetalles | IdComprobanteDetalle | IdComprobante → Comprobantes.IdComprobante |
| HallazgosAtencion | IdHallazgoAtencion | IdAtencion → Atenciones.IdAtencion; IdTecnico → Usuarios.Id |
| Presupuestos | IdPresupuesto | IdAtencion → Atenciones.IdAtencion; IdHallazgoAtencion → HallazgosAtencion.IdHallazgoAtencion; IdPresupuestoAnterior → Presupuestos.IdPresupuesto; IdUsuarioCreacion → Usuarios.Id; IdUsuarioDecision → Usuarios.Id |
| PresupuestoDetalles | IdPresupuestoDetalle | IdConcepto → Conceptos.IdConcepto; IdPresupuesto → Presupuestos.IdPresupuesto; IdPresupuestoCambio → Presupuestos.IdPresupuesto; IdSolicitudAtencion → SolicitudesAtencion.IdSolicitudAtencion; IdTecnicoMaterial → Usuarios.Id; IdTecnicoRegistro → Usuarios.Id |
| PausasTrabajo | IdPausaTrabajo | IdAtencion → Atenciones.IdAtencion; IdDetalle → PresupuestoDetalles.IdPresupuestoDetalle; IdTecnicoResolucion → Usuarios.Id; IdUsuario → Usuarios.Id |

Fuente: [configuraciones EF](../../Taller/Taller.Infraestructura/Persistencia/Configuraciones) y [migración](../../Taller/Taller.Infraestructura/Persistencia/Migraciones). La edición final del DER y la sincronización de los DOCX de requisitos permanecen como trabajo académico pendiente.

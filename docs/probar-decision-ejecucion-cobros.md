# Probar decisiones, ejecución, cobros y entrega

Esta rama extiende `feature/circuito-atencion-evaluacion` y su migración previa. No es necesario fusionarla a `main` para descargarla y probarla. La adenda funcional está en `docs/ERS/ADENDA-v1.0-presupuestos-cobros.md`.

## Instalar

1. Guardar los cambios locales y descargar `feature/decision-ejecucion-cobros`.
2. Configurar la conexión propia en `Taller/Taller.Presentacion/appsettings.Local.json`. No reemplazarla por una conexión de otra máquina.
3. Respaldar la base y probar la actualización sobre una copia. La nueva migración `DecisionesEjecucionLiquidacionYCobros` agrega las tablas económicas y las decisiones por ítem, interpreta aceptaciones históricas como aceptación completa y conserva los estados/cantidades anteriores. No convierte autorización en ejecución.
4. Desde la raíz del repositorio:

```powershell
dotnet build Taller/Taller.sln -c Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj -c Release --no-build
dotnet ef database update --project Taller/Taller.Infraestructura --startup-project Taller/Taller.Infraestructura
```

Usar dotnet-ef 10.0.11 y SDK compatible con `global.json`. La fábrica toma la conexión local o `SGTM_CONN`. En Visual Studio también puede usarse `Update-Database -Project Taller.Infraestructura -StartupProject Taller.Presentacion`.

La nueva migración impide retroceder mediante Down si ya hay decisiones nuevas, revisiones o movimientos económicos que se perderían. En ese caso se restaura el respaldo previo. Se inicializan Efectivo, Transferencia, Tarjeta de débito, Tarjeta de crédito y Billetera virtual; los medios inactivos no se ofrecen para nuevos cobros, pero permanecen en el historial.

## Recorrido por rol

| Rol y pantalla | Acción |
| --- | --- |
| Administrativo — Presupuestos | Seleccionar propuesta vigente, registrar comunicación al cliente y luego Registrar decisión. Desmarcar ítems no aceptados y justificar exclusiones. |
| Administrativo — Presupuestos | Solicitar revisión si se necesita modificar el alcance antes de ejecutar: indicar motivo. No editar el documento anterior. |
| Técnico — Evaluaciones técnicas | Continuar una revisión como borrador; preparar sus ítems y finalizar la nueva propuesta. El informe anterior se conserva. |
| Técnico — Atenciones | Seleccionar una atención en ejecución y usar Gestionar ejecución. Revisar lo aceptado, indicar la viabilidad si hubo exclusiones y Validar alcance y tomar. |
| Técnico — Ejecución | Guardar cantidades realizadas y estados; justificar parciales/omisiones. Guardar avance conserva reserva; Liberar permite continuar a otro técnico. Cerrar no libera. |
| Técnico — Ejecución | Finalizar trabajo si está resuelto, o Terminar con interrupción con explicación y cantidades parciales/omisiones ya registradas. |
| Administrativo — Liquidaciones/cobros | Abrir la atención, revisar cantidades efectivamente realizadas y Emitir liquidación. Si total es cero, indicar motivo. |
| Administrativo — Cuenta de la atención | Registrar cobro. Agregar una o varias filas de medios e importes y confirmar el conjunto. Un cobro puede ser parcial respecto del saldo. |
| Administrativo — Cuenta de la atención | Imprimir liquidación o seleccionar un pago e Imprimir recibo. La liquidación no acredita pago. |
| Administrativo — Cuenta de la atención | Registrar entrega solo con saldo cero, dejando condición y persona que retira. |

La cuenta también puede abrirse desde Atenciones mediante Liquidación y cobros. El rol Administrador conserva sus tareas de administración del sistema; no opera la caja. La bandeja económica prioriza saldos pendientes, liquidaciones pendientes y entregas listas; entregadas al final.

## Casos manuales antes de fusionar

- Aceptar todo y ejecutar; comparar precio/cantidad autorizada con lo efectivamente liquidado.
- Excluir un ítem y verificar que no aparezca como ejecutable ni se cobre. El presupuesto original debe seguir mostrándolo como no aceptado.
- Rechazar todo después de una inspección inicial realizada: solo se liquida esa inspección. Rechazar sin servicios realizados: liquidación cero con motivo, sin pago ficticio.
- Solicitar revisión, cambiar/quitar ítems como técnico y volver a presentar/decidir. Debe conservarse el enlace al presupuesto anterior y su decisión.
- Intentar revisar después de ejecutar o con reserva ajena: debe bloquearse. Para trabajo retirado antes de realizarlo, registrar omisión técnica con motivo.
- Guardar ejecución parcial y liberar; continuar con otro técnico. No debe permitirse disminuir lo ya registrado, superar lo autorizado ni ejecutar un ítem rechazado.
- Interrumpir con parte realizada: liquidar esa cantidad, no la cantidad completa presupuestada.
- Cobrar una parte en efectivo y otra por transferencia en el mismo recibo. Verificar saldo, referencias y total de los medios.
- Reintentar el mismo cobro ante una respuesta perdida: un solo recibo y un solo movimiento. Intentar pagar más que el saldo o con referencia obligatoria vacía: ningún movimiento.
- Dos instancias cobran sobre la misma versión: la segunda debe actualizarse; no debe existir sobrepago ni un saldo perdido.
- Anular un registro con motivo: conservar el original anulado y reponer saldo. No permitir anular ni cobrar después de la entrega.
- Intentar entregar con saldo mayor a cero: bloquear. Completar saldo y entregar: atención cerrada, fecha y condición en historial.
- Reiniciar y consultar: mantener cantidades, decisiones, revisiones, saldos y documentos. Probar impresión de varias páginas y a PDF con una impresora instalada.
- Verificar a 1366×768 y 1920×1080, y escalados de Windows 100 %, 125 % y 150 %. Las grillas deben llenar su área, permitir redimensionar columnas y no dejar botones inaccesibles.

## Verificación automática y límites

Las pruebas usan servicios y repositorios reales con EF InMemory para validar el circuito y sus errores. No reemplazan la ejecución de migraciones, índices, CHECK, transacciones ni rowversion real en SQL Server. La compilación y los tests de GitHub Actions en Windows tampoco constituyen prueba visual de Windows Forms ni prueba con impresora.

El alcance no incluye anticipos antes de liquidar, financiación, crédito con vehículo entregado, integración fiscal, devolución bancaria ni presupuestos adicionales después de haber iniciado trabajos. No se modificaron las funciones de catálogo de otra rama.

Los indicadores de ejemplo del panel Inicio y los reportes generales no fueron conectados a este circuito en esta entrega. Para verificar importes use las bandejas y documentos económicos reales descritos arriba.

# Aceptación integral de la versión actual

Usar el mismo commit y una única base de prueba compartida cuando se prueben dos puestos. Registrar fecha, commit, instancia/base, usuario/rol, datos iniciales, pasos, resultado esperado/obtenido y evidencia. No borrar datos reales ni restaurar la base operativa para probar.

| Prueba | Procedimiento | Resultado esperado |
|---|---|---|
| P01 Acceso | Usuario válido, inválido e inhabilitado; revocar con sesión abierta | Permisos reales revalidados, sin secretos en mensajes |
| P02 Maestros | Alta/edición/inactivación/reactivación y duplicados de cliente/dominio/usuario | Persistencia real, unicidad y conflictos tratados |
| P03 Principal definido | Recepción con trabajo conocido, tomar preparación, presupuesto sin diagnóstico, informar/aceptar, tomar ejecución, completar/conformar, liquidar, cobros mixtos, entregar | Atención Cerrada, saldo cero, documentos coherentes |
| P04 Principal problema | Repetir con síntoma y diagnóstico | Informe definitivo único, relacionado con propuesta de la misma atención |
| P05 Mixto | Solicitudes conocidas y problemas | Propuesta responde a cada solicitud y genera informe cuando corresponde |
| P06 Selección parcial exacta | Cliente elige parte; técnico valida sin cambiar términos | Misma propuesta aceptada, sin segunda aceptación; excluidos no se ejecutan ni cobran |
| P07 Reformulación | Técnico cambia alcance o condiciones tras selección | Versión anterior conservada; nueva exige presentación/decisión |
| P08 Corrección | Intentar antes de propuesta, tras propuesta y con ejecución iniciada | Solo habilitada cuando reglas lo permiten; no modifica histórico |
| P09 Rechazo | Sin cargos; repetir con inspección previamente autorizada y realizada | Liquidación cero justificada en primero; solo inspección realizada se cobra en segundo |
| P10 Hallazgo | Informe no bloqueante y luego bloqueante por alcance | No genera deuda automática; pausa bloquea solo alcance indicado |
| P11 Adicional | Presupuestar hallazgo, informar, aceptar/rechazar | No borra principal; aceptar no resuelve pausa automáticamente |
| P12 Pausa administrativa | Recepción solicita detención general, avisar al taller, actualizar técnico | Impide avance bloqueado; solo técnico resuelve continuidad |
| P13 Material cliente | Propuesta con aporte cero, recepción parcial/no conforme/conforme | No consume más de lo recibido ni material no conforme; servicio no se marca aporte |
| P14 Avance | Guardar parcial, intentar disminuir y finalizar incompleto | EnCurso, rechaza disminución/cierre; normal solo todos completados |
| P15 Concurrencia | Dos técnicos toman mismo trabajo; dos ventanas guardan misma versión | Uno confirma; otro recibe conflicto, sin sobrescritura silenciosa |
| P16 Cobros | Parciales/mixtos, restante, referencia ausente, sobrepago, reintento y anulación | Sumas correctas; IdOperacion evita duplicar; anulación conserva y recompone saldo |
| P17 Entrega diferida | Saldar, cerrar pantalla; abrir atención luego para entregar | Disponible con saldo cero; cerrar con persona/conformidad e historial |
| P18 Reportes | Cada rol/tipo, opciones antes de generar, vacío, limpiar, SQL y exportaciones | Detalle/indicadores/gráfico coherentes, permisos y filtros completos |
| P19 Documentos | Presupuesto, informe, liquidación y recibo de varias páginas | Logo/contacto, datos reales, cantidades/importes históricos, impresión legible |
| P20 Completa/diferencial | Configurar Demo, completa, cambiar datos, diferencial | Copias verificadas y diferencial vinculada a completa |
| P21 Programación | Agente activo, horario/prueba, local y réplica si configurada | Resultado real en catálogo; fallo Azure no simula réplica exitosa |
| P22 Recuperación | Ensayo en copia, plan vencido, dos usuarios, mantenimiento | Validaciones bloquean intentos inválidos; recuperación probada solo en entorno descartable |
| P23 Limpieza | Aplicar cinco migraciones en base vacía y actualizar copia actual | Modelo sin cambios pendientes y sin IdPresupuestoCambio; datos conservados |

Para P18 usar [consultas SQL](../reportes/verificacion-reportes.sql) y la metodología del reporte. Para P20–P22 seguir [operación de respaldos](../respaldos/operacion.md), [prueba local](../instalacion/respaldos-locales-demo.md) y [Azure](../respaldos/azure.md). No prometer notificación instantánea ni restauración sin interrupción.

La comprobación visual se hace en Windows a 1366×768/1920×1080 y DPI 100/125/150 %. Probar con teclado, rueda y varias páginas. Un resultado automático aprobado no reemplaza estas comprobaciones.

## Comprobaciones de la segunda revisión

- Abrir el mismo usuario en dos ventanas: guardar datos en una e intentar editar credenciales o estado con la versión anterior. Rechazar el segundo cambio, conservar lo confirmado y solicitar recarga. Probar también dos administradores distintos.
- La entrega exige persona y conformidad desde la única operación pública; intentar confirmar sin cada campo y comprobar que no cambia la atención.
- La decisión de presupuesto exige persona y medio en aceptación, selección parcial y rechazo. No admitir una excepción por ausencia de solicitudes.
- Interrumpir técnicamente un alta de atención: no deben quedar solicitudes ni historial pendientes de otro guardado.
- Inhabilitar cliente/vehículo después de seleccionarlo y antes del alta: rechazar. Rehabilitar y comprobar una nueva apertura válida.
- Aplicar la quinta migración sobre una copia de la base existente y verificar usuarios, roles, contraseñas, circuito y reportes.

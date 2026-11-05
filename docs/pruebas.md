# Plan y evidencia de pruebas

- [Aceptación integral de la versión actual](#pruebas-aceptacion-version-actual)
- [Evidencia de entrega — 05/11/2026](#pruebas-evidencia-cierre)
- [Instalación y validación del circuito](#pruebas-circuito-principal)
- [Probar decisiones, ejecución, cobros y entrega](#pruebas-alternativas-cobros)

<a id="pruebas-aceptacion-version-actual"></a>

## Aceptación integral de la versión actual

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
| P21 Programación | Agente activo, horario/prueba, local y réplica si configurada | Resultado real en catálogo; Drive no disponible conserva copia local y reintenta exportación |
| P22 Recuperación | Ensayo en copia, plan vencido, dos usuarios, mantenimiento | Validaciones bloquean intentos inválidos; recuperación probada solo en entorno descartable |
| P23 Limpieza | Aplicar seis migraciones en base vacía y actualizar copia actual | Modelo sin cambios pendientes y sin IdPresupuestoCambio; datos conservados |

Para P18 usar [consultas SQL](../scripts/recursos/verificacion-reportes.sql) y la metodología del reporte. Para P20–P22 seguir [operación de respaldos](../REFERENCIA-TECNICA.md#respaldos-operacion), [prueba local](../REFERENCIA-TECNICA.md#instalacion-respaldos-locales-demo) y [Google Drive](../REFERENCIA-TECNICA.md#respaldos-google-drive). No prometer notificación instantánea ni restauración sin interrupción.

La comprobación visual se hace en Windows a 1366×768/1920×1080 y DPI 100/125/150 %. Probar con teclado, rueda y varias páginas. Un resultado automático aprobado no reemplaza estas comprobaciones.

### Comprobaciones de la segunda revisión

- Abrir el mismo usuario en dos ventanas: guardar datos en una e intentar editar credenciales o estado con la versión anterior. Rechazar el segundo cambio, conservar lo confirmado y solicitar recarga. Probar también dos administradores distintos.
- La entrega exige persona y conformidad desde la única operación pública; intentar confirmar sin cada campo y comprobar que no cambia la atención.
- La decisión de presupuesto exige persona y medio en aceptación, selección parcial y rechazo. No admitir una excepción por ausencia de solicitudes.
- Interrumpir técnicamente un alta de atención: no deben quedar solicitudes ni historial pendientes de otro guardado.
- Inhabilitar cliente/vehículo después de seleccionarlo y antes del alta: rechazar. Rehabilitar y comprobar una nueva apertura válida.
- Aplicar la quinta migración sobre una copia de la base existente y verificar usuarios, roles, contraseñas, circuito y reportes.

### Comprobaciones de la tercera revisión

- Verificar seis migraciones y repetir la preparación sobre una base ya actualizada sin recrearla.
- Iniciar dos puestos simultáneamente sobre una instalación nueva autorizada; comprobar una sola cuenta inicial y catálogos sin duplicados.
- Comprobar que al deshabilitar cliente/vehículo aparece su identidad y No es la opción predeterminada.
- Ejecutar la publicación desde un acceso directo con otra carpeta de trabajo; debe leer su configuración local.
- Cambiar el estado de un reporte por un valor ajeno mediante una llamada de servicio: rechazar, no producir una consulta vacía engañosa.
- Exportar paquetes a Drive; comprobar sincronización externa e importación. Probar destino no disponible y reintento sin recrear el respaldo SQL.

---

<a id="pruebas-evidencia-cierre"></a>

### Evidencia de entrega — 05/11/2026

Referencia de código: `ef2b2e844f85188f3065e34572d84179b9eca2a5` (integración del PR #75).

| Recorrido | Evidencia observada | Límite |
|---|---|---|
| Atención 244 (CU04–CU08) | Alta, propuesta aceptada, tres ítems ejecutados, liquidación por $94.000, dos pagos y entrega con saldo cero. El expediente quedó cerrado. | Servicio definido; este recorrido no ejercitó los caminos alternativos. |
| Cobros (CU09) | Filtro por atención 244: dos registros, $40.000 en efectivo y $54.000 por transferencia. Se revisó la vista previa y el ajuste de paginación. | Evidencia manual de este reporte. |
| Respaldo manual (CU10) | Copia completa verificada, exportada a Drive y visible en web; paquete descargado e importado; ensayo en base temporal con integridad y esquema correctos. | La base activa no se reemplazó. La programación automática queda fuera de la validación manual de entrega. |

En el [PR #77](https://github.com/EnriqueRTG/taller-mecanico/pull/77) finalizaron correctamente los workflows de construcción WinForms y compilación/pruebas, incluida la integración SQL aislada. El análisis de PowerShell verifica sintaxis; no acredita la ejecución de los asistentes en el equipo de presentación.

---

<a id="pruebas-circuito-principal"></a>

## Instalación y validación del circuito

### Preparar la rama en Windows

1. Guardar cambios locales, descargar la versión actual y abrir `Taller/Taller.sln` con el SDK indicado por global.json.
2. Preparar una base nueva siguiendo [entornos e instalación](../REFERENCIA-TECNICA.md#instalacion-entornos). No actualizar ni restaurar sobre ella el esquema anterior. El esquema incluye `EsquemaInicialCircuitoActual` y `SimplificarEjecucionTresEstados`.
3. Para el coloquio ejecutar el cargador Demo antes de abrir la aplicación. Para instalación real usar inicialización mínima y cuentas propias.
4. Configurar la misma conexión en Visual Studio y en las herramientas. Reconstruir y ejecutar.

```powershell
dotnet build Taller/Taller.sln -c Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj -c Release --no-build
```

No existe un segundo trámite de ingreso ni un catálogo de servicios obligatorios de ingreso. Solo una inspección extraordinaria presupuestada y aceptada por el cliente permite registrar su ejecución cobrable.

### Recorrido manual para validar el entorno de presentación

Usar cuentas activas de Administrativo y Técnico. El rol Administrador administra el sistema; no suplanta al operativo.

| Prueba | Resultado esperado |
| --- | --- |
| Abrir atención por síntoma y solicitar evaluación sin servicios | Atención en evaluación; ningún servicio inicial ni cargo inventado |
| Abrir atención por servicio definido | Tipo conservado; técnico puede proponer sin registrar diagnóstico |
| Técnico solicita inspección extraordinaria con costo; recepción informa y registra aceptación | Solo después de aceptar aparece en Inspecciones autorizadas; cantidad realizada inicial cero |
| Intentar ejecutar una inspección todavía no aceptada | No habilita su registro ni su liquidación |
| Tomar desde Atenciones o Evaluaciones técnicas | Se abre el espacio de informe y propuesta con datos reales de esa atención |
| Cerrar sin guardar un cambio | Pide descartar; no elimina el avance anterior ni libera la reserva |
| Guardar avance, cerrar y continuar | Recupera textos e ítems; reserva a nombre del mismo técnico |
| Liberar y tomar con otra cuenta técnica | Continúa el avance; historial y autoría final identifican a cada participante |
| Intentar tomar/guardar una actividad ajena | Rechaza la operación |
| Finalizar problema sin conclusión o sin ítems/motivo | No finaliza |
| Finalizar con inspecciones autorizadas todavía pendientes | Exige completar todas las inspecciones autorizadas |
| Finalizar servicio definido con ítems y sin diagnóstico | Presupuesto real relacionado a atención, sin diagnóstico ficticio |
| Finalizar sin propuesta y con motivo | Conserva informe, elimina solo borrador sin presentar y pasa a pendiente de liquidación |
| Recepción consulta y registra comunicación | Propuesta pasa a pendiente de decisión; no autoriza ejecución ni registra aceptación |
| Dos instancias intentan trabajar sobre la misma versión | La segunda operación se rechaza; actualizar/reabrir para leer datos actuales |
| Reiniciar la aplicación | Persisten reservas, avances, informe y presupuesto |
| Grillas a 1366×768 y 1920×1080; Windows 100 %, 125 % y 150 % | Campos y acciones accesibles; columnas ajustables; sin datos ficticios en este recorrido |

### Qué cubren las pruebas automáticas

`Taller.Pruebas` ejecuta reglas del circuito con repositorios reales y el proveedor EF InMemory: informe opcional/obligatorio, reserva y liberación, avances recuperables, versión obsoleta, permisos, conceptos activos/no duplicados, inspecciones autorizadas y comunicación de la propuesta. InMemory no reproduce transacciones, restricciones SQL ni generación real de rowversion: los casos de dos instancias y la migración sobre una copia de SQL Server deben verificarse en Windows/SQL Server.

La compilación cruzada comprueba C#, recursos y referencias; la construcción de formularios en STA corre en Windows en CI. La visualización, navegación, escalado e impresión deben comprobarse manualmente en Windows. Las pruebas SQL se ejecutan en bases temporales aisladas y no modifican la base del usuario.

---

<a id="pruebas-alternativas-cobros"></a>

## Probar decisiones, ejecución, cobros y entrega

Usar [actualización y Demo v5](../REFERENCIA-TECNICA.md#instalacion-actualizacion-coloquio). Las cuentas de SGTM son distintas de la identidad SQL de cada estación.

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

Si cambian condiciones del acuerdo aunque los ítems sean iguales, marcar Se modifican condiciones del acuerdo y describirlas en la justificación: la propuesta vuelve a recepción para otra aceptación. La justificación de viabilidad sin cambios solo documenta la revisión técnica.

# Instalación y validación del circuito

## Preparar la rama en Windows

1. Guardar cambios locales, descargar la versión actual y abrir `Taller/Taller.sln` con el SDK indicado por global.json.
2. Preparar una base nueva siguiendo [entornos e instalación](../instalacion/entornos.md). No actualizar ni restaurar sobre ella el esquema anterior. El esquema incluye `EsquemaInicialCircuitoActual` y `SimplificarEjecucionTresEstados`.
3. Para el coloquio ejecutar el cargador Demo antes de abrir la aplicación. Para instalación real usar inicialización mínima y cuentas propias.
4. Configurar la misma conexión en Visual Studio y en las herramientas. Reconstruir y ejecutar.

```powershell
dotnet build Taller/Taller.sln -c Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj -c Release --no-build
```

No existe un segundo trámite de ingreso ni un catálogo de servicios obligatorios de ingreso. Solo una inspección extraordinaria presupuestada y aceptada por el cliente permite registrar su ejecución cobrable.

## Recorrido manual obligatorio antes de fusionar

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

## Qué cubren las pruebas automáticas

`Taller.Pruebas` ejecuta reglas del circuito con repositorios reales y el proveedor EF InMemory: informe opcional/obligatorio, reserva y liberación, avances recuperables, versión obsoleta, permisos, conceptos activos/no duplicados, inspecciones autorizadas y comunicación de la propuesta. InMemory no reproduce transacciones, restricciones SQL ni generación real de rowversion: los casos de dos instancias y la migración sobre una copia de SQL Server deben verificarse en Windows/SQL Server.

La compilación cruzada comprueba C#, recursos y referencias; la construcción de formularios en STA corre en Windows en CI. La visualización, navegación, escalado e impresión deben comprobarse manualmente en Windows. Las pruebas SQL se ejecutan en bases temporales aisladas y no modifican la base del usuario.

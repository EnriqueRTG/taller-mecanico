# Instalación y validación del circuito

## Preparar la rama en Windows

1. Guardar el trabajo local y descargar `feature/circuito-atencion-evaluacion`. La rama parte del checkpoint `3fb5908` de `feature/presupuesto-reparacion`. No reemplaza automáticamente `main`.
2. Disponer del SDK .NET 10 compatible con `global.json` y SQL Server. Abrir `Taller/Taller.sln`.
3. Mantener la conexión propia en `Taller/Taller.Presentacion/appsettings.Local.json`, o usar `SGTM_CONN` para las herramientas. El archivo local es opcional para compilar y no se versiona. La conexión efectiva sí es necesaria al ejecutar.
4. Respaldar la base que se utilice para pruebas. La migración nueva no borra atenciones: recupera la atención de presupuestos históricos a través de su diagnóstico, clasifica solicitudes previas como `Problema` y reconoce propuestas históricas ya preparadas. Si detecta un presupuesto sin atención recuperable, detiene la migración.
5. Desde la raíz del repositorio, con `dotnet-ef` 10.0.11 instalado:

```powershell
dotnet build Taller/Taller.sln -c Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj -c Release --no-build
dotnet ef database update --project Taller/Taller.Infraestructura --startup-project Taller/Taller.Infraestructura
```

En la consola de paquetes de Visual Studio se puede usar `Update-Database -Project Taller.Infraestructura -StartupProject Taller.Presentacion`. La última migración es `CircuitoAtencionEvaluacionPropuesta`.

No es necesario configurar `IngresoDiagnostico:IdConcepto`: fue retirado. El catálogo usa `DisponibleParaIngreso` y puede ofrecer varios servicios; un presupuesto técnico usa conceptos activos de las categorías activas.

La reversión mediante `Down` se detiene si ya existen avances, clasificaciones o propuestas que el esquema anterior no representa. En ese caso se restaura el respaldo previo en lugar de perder esos datos silenciosamente.

## Recorrido manual obligatorio antes de fusionar

Usar cuentas activas de Administrativo y Técnico. El rol Administrador administra el sistema; no suplanta al operativo.

| Prueba | Resultado esperado |
| --- | --- |
| Abrir atención por síntoma y solicitar evaluación sin servicios | Atención en evaluación; ningún servicio inicial ni cargo inventado |
| Abrir atención por servicio definido | Tipo conservado; técnico puede proponer sin registrar diagnóstico |
| Agregar dos servicios iniciales, uno sin cargo, autorizar y confirmar | Documento inicial aceptado, detalles no iniciados y cantidad ejecutada cero |
| Intentar confirmar con servicios sin autorización | No confirma |
| Tomar desde Atenciones o Evaluaciones técnicas | Se abre el espacio de informe y propuesta con datos reales de esa atención |
| Cerrar sin guardar un cambio | Pide descartar; no elimina el avance anterior ni libera la reserva |
| Guardar avance, cerrar y continuar | Recupera textos e ítems; reserva a nombre del mismo técnico |
| Liberar y tomar con otra cuenta técnica | Continúa el avance; historial y autoría final identifican a cada participante |
| Intentar tomar/guardar una actividad ajena | Rechaza la operación |
| Finalizar problema sin conclusión o sin ítems/motivo | No finaliza |
| Finalizar con servicios iniciales todavía pendientes | Exige indicar realizados u omitidos con justificación |
| Finalizar servicio definido con ítems y sin diagnóstico | Presupuesto real relacionado a atención, sin diagnóstico ficticio |
| Finalizar sin propuesta y con motivo | Conserva informe, elimina solo borrador sin presentar y pasa a pendiente de liquidación |
| Recepción consulta y registra comunicación | Propuesta pasa a pendiente de decisión; no autoriza ejecución ni registra aceptación |
| Dos instancias intentan trabajar sobre la misma versión | La segunda operación se rechaza; actualizar/reabrir para leer datos actuales |
| Reiniciar la aplicación | Persisten reservas, avances, informe y presupuesto |
| Grillas a 1366×768 y 1920×1080; Windows 100 %, 125 % y 150 % | Campos y acciones accesibles; columnas ajustables; sin datos ficticios en este recorrido |

Para una atención histórica con informe ya registrado, se conserva ese informe sin edición y se permite preparar su propuesta si todavía no existe una confirmada.

## Qué cubren las pruebas automáticas

`Taller.Pruebas` ejecuta reglas del circuito con repositorios reales y el proveedor EF InMemory: informe opcional/obligatorio, reserva y liberación, avances recuperables, versión obsoleta, permisos, conceptos activos/no duplicados, servicios iniciales y comunicación de la propuesta. InMemory no reproduce transacciones, restricciones SQL ni generación real de rowversion: los casos de dos instancias y la migración sobre una copia de SQL Server deben verificarse en Windows/SQL Server.

La compilación cruzada desde Linux comprueba C#, recursos y referencias de Windows Forms, pero no ejecuta la interfaz de Windows. El PR queda en borrador hasta completar esas verificaciones. Las advertencias previas de módulos ajenos a este circuito no se consideran funcionalidades implementadas.

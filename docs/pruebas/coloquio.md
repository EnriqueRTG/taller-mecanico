# Lote reproducible para el coloquio

## Preparación separada de la instalación real

Requiere una base nueva con EsquemaInicialCircuitoActual; no se utiliza la base ni los respaldos de la versión anterior. Consulte `entornos-demo-e-instalacion.md`.

El cargador es `Taller.Demostracion`, una herramienta de consola que utiliza los servicios de Aplicación y repositorios reales. No forma parte del arranque de Windows Forms y no se ejecuta al instalar una base comercial.

Solo admite una base terminada en `_Demo`. Exige que no existan usuarios, clientes, vehículos ni atenciones. No borra datos ni mezcla otro lote. Una segunda ejecución rechaza la carga. Las migraciones y catálogos de referencia se preparan primero; el lote comercial se carga en una transacción con bloqueo exclusivo: si falla o se interrumpe, se revierte la carga. No debe haber clientes conectados durante su preparación. Los identificadores autonuméricos pueden tener saltos si se revierte una ejecución.

En PowerShell, desde la raíz del repositorio:

```powershell
$env:SGTM_CONN = 'Server=localhost\SQLEXPRESS;Database=SGTM_DB_Demo;Integrated Security=True;Encrypt=True;TrustServerCertificate=True'
$claveDemo = Read-Host 'Contraseña para las cuentas de demostración' -AsSecureString
$env:SGTM_DEMO_PASSWORD = [System.Net.NetworkCredential]::new('', $claveDemo).Password
dotnet run --project Taller/Taller.Demostracion -- --cargar-demo
Remove-Item Env:SGTM_DEMO_PASSWORD
```

Elegir mínimo 12 caracteres con mayúsculas, minúsculas, número y símbolo, hasta 100. La credencial se usa únicamente en este entorno académico. No se imprime ni se incluye en Git. Si existe una variable `SGTM_ADMIN_INICIAL_PASSWORD` de una instalación anterior, retirarla antes de cargar para que el administrador inicial de esta base utilice la contraseña del lote.

El cargador debe terminar indicando `Lote DEMO v4 completo`. Si falla, no continúe operando con un lote incompleto; capture el error. El comando puede crear la base si la identidad SQL tiene permiso. No use el nombre de la base real ni renombre una copia real para pasar esta protección.

## Contenido

- Roles y catálogos de referencia del modelo.
- Ocho usuarios: `admin`, `demo.admin`, `demo.recepcion`, `demo.recepciondos`, `demo.tecnico`, `demo.tecnicodos`, `demo.tecnicotres` y `demo.inactivo` (deshabilitado, para probar filtros y rechazo de acceso).
- 19 clientes ficticios (tres inactivos), 48 vehículos (dos inactivos con atenciones históricas cerradas), marcas/modelos y conceptos de distintas categorías y tipos.
- 48 atenciones del circuito actual de problema, servicio definido o solicitudes mixtas, con fechas distribuidas en seis meses. Cada una tiene un marcador buscable `DEMO-001` a `DEMO-048` en el motivo.
- Evaluaciones, diagnósticos cuando corresponden, propuestas, decisiones, selección parcial y nuevas versiones aceptadas, ejecución, hallazgos, adicionales, pausas por material del cliente, recepciones conformes, liquidaciones, pagos mixtos por los cinco medios, anulaciones justificadas y entregas.
- Los precios son académicos, no una lista de precios real. Los datos de nombres/documentos/dominios son ficticios. La retrodatación es exclusiva de los fixtures y conserva la secuencia de eventos de cada atención; no existe esa operación en la interfaz normal.
- Auditoría de acceso y de respaldos NO se inventa: se genera al iniciar sesión y al ejecutar respaldos reales. El historial comercial sí procede de las operaciones invocadas durante la carga.

## Casos preparados

Los primeros 16 marcadores se repiten en grupos posteriores, con diferentes usuarios, vehículos, fechas e importes. Desde el tercer grupo hay además selecciones parciales ya revisadas y aceptadas nuevamente, materiales del cliente ya recibidos y cobros anulados y registrados correctamente. Busque por el marcador, no dependa del ID SQL:

| Motivo | Punto de partida | Qué demostrar |
|---|---|---|
| DEMO-001 | Ingreso actual, disponible para preparación | Consultar solicitudes e iniciar preparación |
| DEMO-002 | Preparación disponible | Tomar como técnico |
| DEMO-003 | Avance de evaluación | Continuar borrador |
| DEMO-004 | Propuesta con inspección extraordinaria ya autorizada y realizada | Comunicación al cliente |
| DEMO-005 | Presupuesto presentado | Registrar aceptación o rechazo |
| DEMO-006 | Presupuesto rechazado tras inspección autorizada | Cobrar solo inspección realizada, no reparación rechazada |
| DEMO-007 | Selección parcial | Revisión técnica: validar selección exacta o reformular para otra aceptación |
| DEMO-008 | Alcance aceptado | Iniciar ejecución |
| DEMO-009 | Ejecución en curso | Registrar avances y completar |
| DEMO-010 | Hallazgo bloqueante y adicional | Presentar adicional y resolver causa de pausa |
| DEMO-011 | Material a cargo del cliente | Recibir y conformar material, resolver pausa |
| DEMO-012 | Trabajo finalizado | Emitir liquidación |
| DEMO-013 | Liquidación sin pagos | Registrar cobro |
| DEMO-014 | Pago parcial | Completar con otro medio |
| DEMO-015 | Cuenta saldada pendiente de entrega | Retiro posterior al pago; DEMO-031 muestra una ya entregada |
| DEMO-016 | Ingreso actual cancelado antes de intervenir | Historial y filtros |

La asignación a técnicos rota: use el técnico indicado en la bandeja. Los servicios vuelven a comprobar la asignación; no habilitarán modificar una actividad ajena para facilitar una demostración.

## Guion sugerido de 20 minutos

1. **Minutos 0–3:** recepción: inicio y pendientes, buscar cliente/vehículo, abrir atención o continuar DEMO-001. Explicar problema frente a servicio definido sin cargos automáticos de ingreso.
2. **Minutos 3–8:** técnico: tomar evaluación y preparar informe/propuesta. Alternativa breve: continuar DEMO-003 con el técnico asignado.
3. **Minutos 8–11:** recepción: presentar propuesta y registrar aceptación. Mostrar DEMO-007 para explicar por qué una selección parcial vuelve al técnico.
4. **Minutos 11–15:** técnico: registrar trabajo sobre un alcance aceptado. Mostrar DEMO-010/011 como alternativas sin recorrer todo el circuito adicional.
5. **Minutos 15–18:** recepción: continuar DEMO-014, registrar el saldo por otro medio y entregar solo cuando sea cero. Consultar comprobante.
6. **Minutos 18–20:** reportes: ampliar período a seis meses para comparar estados, medios y conceptos realizados. Administrador: explicar auditoría de acceso y respaldos reales ya preparados.

No es necesario reconstruir todo el historial en vivo. Los casos preparados permiten demostrar decisiones y excepciones sin exceder el tiempo.

## Abrir y recuperar el punto de partida

Configure el perfil F5 con `SGTM_CONN` hacia `SGTM_DB_Demo`, mantenimiento hacia la misma base y respaldos deshabilitados durante la prueba del negocio. No basta con cambiar una variable en otra terminal si Visual Studio ya estaba abierto. Para probar respaldos, prepare su base de control Demo y permisos siguiendo las otras guías.

Tras verificar el lote, cree un respaldo completo de `SGTM_DB_Demo` desde SSMS y consérvelo como punto de partida. Antes de restaurarlo para otra práctica, cierre la aplicación y confirme explícitamente el destino Demo. No restaure ese archivo sobre su base real. Conserve además el código y el identificador de la migración para saber qué versión corresponde al respaldo.

## Lote v4 y precios

El lote v4 se construye íntegramente en una base nueva y usa solicitudes individuales, conceptos coherentes con cada motivo y precios de referencia del catálogo. Incluye inspecciones autorizadas realizadas, un rechazo posterior, ejecución en curso, selección validada sin cambios y trabajos completados y un pago total aún pendiente de entrega. No crea atenciones actuales en Abierta, PendienteDiagnostico ni Pausada. Ver [estados](../arquitectura/estados-circuito-actual.md), [catálogo Demo](catalogo-demo.md) y [entornos](../instalacion/entornos.md). Los importes y resultados cuantitativos de reportes del lote anterior ya no son valores de aceptación del lote nuevo. La base existente no se modifica al actualizar main.

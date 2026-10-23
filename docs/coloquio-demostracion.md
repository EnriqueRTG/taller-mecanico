# Lote reproducible para el coloquio

## Preparación separada de la instalación real

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

El cargador debe terminar indicando `Lote DEMO completo`. Si falla, no continúe operando con un lote incompleto; capture el error. El comando puede crear la base si la identidad SQL tiene permiso. No use el nombre de la base real ni renombre una copia real para pasar esta protección.

## Contenido

- Roles y catálogos de referencia del modelo.
- Siete usuarios: `admin`, `demo.admin`, `demo.recepcion`, `demo.recepciondos`, `demo.tecnico`, `demo.tecnicodos`, `demo.tecnicotres`.
- 45 clientes ficticios (cinco inactivos), 100 vehículos, marcas/modelos y conceptos de distintas categorías y tipos.
- 100 atenciones de problema o servicio definido, con fechas distribuidas en seis meses. Cada una tiene un marcador buscable `DEMO-001` a `DEMO-100` en el motivo.
- Evaluaciones, diagnósticos cuando corresponden, propuestas, decisiones, selección parcial, ejecución, hallazgos, adicionales, pausas por material del cliente, liquidaciones, pagos y entregas.
- Los precios son académicos, no una lista de precios real. Los datos de nombres/documentos/dominios son ficticios. La retrodatación es exclusiva de los fixtures y conserva la secuencia de eventos de cada atención; no existe esa operación en la interfaz normal.
- Auditoría de acceso y de respaldos NO se inventa: se genera al iniciar sesión y al ejecutar respaldos reales. El historial comercial sí procede de las operaciones invocadas durante la carga.

## Casos preparados

Los primeros 16 marcadores se repiten en grupos posteriores, con diferentes usuarios, vehículos, fechas e importes. Busque por el marcador, no dependa del ID SQL:

| Motivo | Punto de partida | Qué demostrar |
|---|---|---|
| DEMO-001 | Recepción abierta | Edición e ingreso al circuito |
| DEMO-002 | Pendiente de evaluación | Tomar como técnico |
| DEMO-003 | Avance de evaluación | Continuar borrador |
| DEMO-004 | Propuesta pendiente de presentar | Comunicación al cliente |
| DEMO-005 | Presupuesto presentado | Registrar aceptación o rechazo |
| DEMO-006 | Presupuesto rechazado | Liquidación según lo realizado, sin cobrar reparación |
| DEMO-007 | Selección parcial | Revisión técnica antes de nueva aceptación |
| DEMO-008 | Alcance aceptado | Iniciar ejecución |
| DEMO-009 | Ejecución parcial | Registrar avances y completar |
| DEMO-010 | Hallazgo bloqueante y adicional | Presentar adicional y resolver causa de pausa |
| DEMO-011 | Material a cargo del cliente | Recibir y conformar material, resolver pausa |
| DEMO-012 | Trabajo finalizado | Emitir liquidación |
| DEMO-013 | Liquidación sin pagos | Registrar cobro |
| DEMO-014 | Pago parcial | Completar con otro medio |
| DEMO-015 | Entregada, saldo cero | Consulta histórica y documentos |
| DEMO-016 | Cancelada antes de intervenir | Historial y filtros |

La asignación a técnicos rota: use el técnico indicado en la bandeja. Los servicios vuelven a comprobar la asignación; no habilitarán modificar una actividad ajena para facilitar una demostración.

## Guion sugerido de 20 minutos

1. **Minutos 0–3:** recepción: inicio y pendientes, buscar cliente/vehículo, abrir atención o continuar DEMO-001. Explicar problema frente a servicio definido y autorización inicial.
2. **Minutos 3–8:** técnico: tomar evaluación y preparar informe/propuesta. Alternativa breve: continuar DEMO-003 con el técnico asignado.
3. **Minutos 8–11:** recepción: presentar propuesta y registrar aceptación. Mostrar DEMO-007 para explicar por qué una selección parcial vuelve al técnico.
4. **Minutos 11–15:** técnico: registrar trabajo sobre un alcance aceptado. Mostrar DEMO-010/011 como alternativas sin recorrer todo el circuito adicional.
5. **Minutos 15–18:** recepción: continuar DEMO-014, registrar el saldo por otro medio y entregar solo cuando sea cero. Consultar comprobante.
6. **Minutos 18–20:** reportes: ampliar período a seis meses para comparar estados, medios y conceptos realizados. Administrador: explicar auditoría de acceso y respaldos reales ya preparados.

No es necesario reconstruir todo el historial en vivo. Los casos preparados permiten demostrar decisiones y excepciones sin exceder el tiempo.

## Abrir y recuperar el punto de partida

Configure el perfil F5 con `SGTM_CONN` hacia `SGTM_DB_Demo`, mantenimiento hacia la misma base y respaldos deshabilitados durante la prueba del negocio. No basta con cambiar una variable en otra terminal si Visual Studio ya estaba abierto. Para probar respaldos, prepare su base de control Demo y permisos siguiendo las otras guías.

Tras verificar el lote, cree un respaldo completo de `SGTM_DB_Demo` desde SSMS y consérvelo como punto de partida. Antes de restaurarlo para otra práctica, cierre la aplicación y confirme explícitamente el destino Demo. No restaure ese archivo sobre su base real. Conserve además el código y el identificador de la migración para saber qué versión corresponde al respaldo.

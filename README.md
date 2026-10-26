# Sistema de Gestión de Taller Mecánico — SGTM

Aplicación de escritorio para gestionar la atención de vehículos en un taller mecánico. Proyecto académico de **Taller de Programación II**, **Licenciatura en Sistemas de Información, UNNE**.

## Circuito implementado en esta rama

1. Identificar cliente y vehículo y abrir una atención por síntoma, servicio definido o solicitud mixta.
2. La recepción con solicitudes individuales pasa directamente a preparación técnica. No agrega servicios cobrables automáticamente; una inspección extraordinaria requiere presupuesto y autorización específicos.
3. Reservar una evaluación, guardar avances, registrar un diagnóstico cuando sea necesario y preparar la propuesta económica.
4. Comunicar el presupuesto y registrar aceptación total, aceptación por ítems o rechazo. Las revisiones previas a la ejecución conservan el documento anterior.
5. Validar el alcance y registrar cantidades efectivamente realizadas, omisiones justificadas o interrupción del trabajo.
6. Emitir una liquidación sobre lo autorizado y realizado, registrar pagos parciales con uno o varios medios y conservar recibos/anulaciones.
7. Registrar la entrega únicamente con actividad técnica terminada y saldo cero.

Los listados, ediciones e historiales de clientes y vehículos utilizan persistencia real. Inicio muestra indicadores reales. Reportes genera documentos con indicadores, gráficos y detalle, según los permisos del rol.

La implementación dispone de pruebas automatizadas del circuito, reportes y recuperación, incluyendo pruebas nativas en SQL Server aislado. Todavía requiere validación manual de Windows Forms, impresión y consultas/migraciones en una copia de SQL Server antes de fusionar como versión estable. Las pruebas con EF InMemory no sustituyen las restricciones, transacciones ni rowversion de SQL Server.

## Roles

| Rol | Responsabilidades |
| --- | --- |
| Administrador | Usuarios, reportes de gestión, copias completas/diferenciales manuales o programadas y recuperación con doble aprobación. |
| Administrativo | Clientes, vehículos, recepción, comunicación y decisión de presupuestos, liquidación, cobros, entrega y reportes operativos/económicos. |
| Técnico | Reserva de actividades, evaluación/informe, propuestas, ejecución y reportes propios; consulta de actividades disponibles. |

Los servicios de reportes y las operaciones del circuito revalidan el usuario activo y su rol en la base. El administrador no opera la caja por el hecho de consultar sus reportes.

## Reportes

Documentos útiles para revisar ingresos de atenciones, decisiones de presupuestos, cobros/anulaciones por medio, saldos pendientes, carga técnica, actividad registrada y conceptos realizados. Distinguen importes presupuestados, autorizados, realizados y cobrados; no estiman rentabilidad ni productividad sin costos u horas registrados.

Se pueden guardar como **HTML autocontenido con gráfico SVG**, exportar el detalle **CSV** o utilizar la vista de **impresión/PDF** con una impresora instalada. [Definiciones, alcance por rol y pruebas](docs/reportes-por-rol.md).

## Arquitectura

| Proyecto | Responsabilidad |
| --- | --- |
| `Taller.Dominio` | Entidades, estados y constantes del negocio. |
| `Taller.Aplicacion` | Casos de uso en servicios, validaciones, autorización, DTOs y contratos de persistencia. |
| `Taller.Infraestructura` | EF Core, configuraciones, migraciones, repositorios, unidad de trabajo, seguridad e inicialización. |
| `Taller.Presentacion` | Windows Forms, controles y eventos estáticos en Designer, navegación y documentos imprimibles. |
| `Taller.Pruebas` | Pruebas xUnit del circuito, reportes, autorizaciones, errores y traducción de consultas. |

Reportes utiliza un modelo de lectura: `IReporteRepositorio` proyecta datos sin seguimiento a DTOs; `ReporteServicio` aplica alcance, filtros y cálculos; el formulario presenta el documento y sus exportadores. No necesita modificar entidades ni confirmar una unidad de trabajo para consultar.

Tecnologías: C#, .NET 10, Windows Forms, SQL Server, EF Core 10, inyección de dependencias, configuración JSON, PasswordHasher y xUnit. El SDK se determina en `global.json`.

## Preparar y ejecutar

Se requiere Windows para ejecutar la interfaz, SDK .NET compatible con `global.json`, SQL Server y dotnet-ef 10.0.11.

1. Guardar los cambios locales y descargar la rama que se quiere probar.
2. Configurar `ConnectionStrings:Default` en `Taller/Taller.Presentacion/appsettings.Local.json` o la variable `SGTM_CONN`. No publicar credenciales ni copiar una conexión ajena.
3. Realizar y verificar un respaldo mediante SQL Server Management Studio. Si el módulo SGTM todavía no está instalado y validado, realizar esta copia mediante SQL Server Management Studio.
4. Preparar una base nueva con `EsquemaInicialCircuitoActual`; no actualizar bases de versiones anteriores. Seguir [la guía de entornos e instalación](docs/entornos-demo-e-instalacion.md).

Desde la raíz:

```powershell
dotnet build Taller/Taller.sln --configuration Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj --configuration Release --no-build
dotnet ef database update --project Taller/Taller.Infraestructura --startup-project Taller/Taller.Infraestructura
dotnet run --project Taller/Taller.Presentacion
```

La fábrica de contexto utiliza la configuración local o `SGTM_CONN`. Los datos iniciales incluyen roles, administrador, marcas/modelos y catálogo de categorías/conceptos. La semilla de catálogo es repetible y no debe sobrescribir indiscriminadamente datos administrados por el usuario.

## Alcance y pendientes

- Documentos económicos internos en ARS, sin integración ni validez fiscal.
- Cobros después de liquidar; no incluye anticipos, financiación ni devolución bancaria automática.
- Las revisiones formales del presupuesto se realizan antes de comenzar sus trabajos. Los presupuestos adicionales después de iniciar ejecución quedan pendientes de ampliar.
- Respaldos/restauración requiere instalación técnica explícita de control, permisos y agente. Está deshabilitado por defecto hasta completar esa preparación. La recuperación usa mantenimiento y dos identidades; no ofrece recuperación sin ventana ni backups de log.
- Los reportes muestran estados actuales o movimientos según su fecha explícita; no reconstruyen una situación histórica completa ni calculan costos/rentabilidad.

## Documentación del circuito

- [Atención, evaluación y propuesta](docs/circuito-atencion-evaluacion.md).
- [Adenda de presupuestos, ejecución y cobros](docs/ERS/ADENDA-v1.0-presupuestos-cobros.md).
- [Probar decisiones, ejecución, cobros y entrega](docs/probar-decision-ejecucion-cobros.md).
- [Reportes por rol y revisión de consistencia](docs/reportes-por-rol.md).
- [Instalar y operar respaldos y restauración](docs/respaldos-restauracion.md).

El esquema actual parte de una única migración inicial y no conserva compatibilidad con la base anterior. El lote Demo v3 se carga explícitamente; la instalación comercial no incluye atenciones ficticias. Respaldos utiliza una base de control instalada explícitamente mediante el agente, según su guía.

La integración compatible de catálogo, clientes y vehículos se describe en [la guía de integración](docs/integracion-catalogo-maestros.md). Incluye la migración no destructiva de versiones de clientes/vehículos; no aplicar la migración `AjustarCatalogoActual` de la rama paralela sobre el circuito vigente.

- [Hallazgos, adicionales, aceptación parcial y materiales del cliente](docs/hallazgos-adicionales-materiales.md): circuito operativo, responsabilidades, migración y pruebas de aceptación.

## Instalación segura, nube y demostración

- [Seguridad e instalación](docs/seguridad-instalacion.md)
- [Respaldos en Azure](docs/azure-respaldos.md)
- [Lote y guion de coloquio](docs/coloquio-demostracion.md)

El lote se carga explícitamente con `scripts/Cargar-Demostracion.ps1`; no se ejecuta al iniciar Windows Forms. Las instalaciones nuevas requieren una contraseña inicial suministrada por el instalador.

- [Criterio de fechas y zona horaria del taller](docs/fechas-y-zona-horaria.md).

Estados y decisiones de limpieza: [estados del circuito actual](docs/estados-circuito-actual.md).

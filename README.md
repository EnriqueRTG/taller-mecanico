# SGTM — Sistema de Gestión de Taller Mecánico

Proyecto académico de Taller de Programación II, Licenciatura en Sistemas de Información, UNNE. Aplicación Windows Forms con SQL Server para recepción, preparación técnica, presupuestos, ejecución, liquidación, cobros y entrega.

## Ejecutar la demostración

1. Descargar main y compilar `Taller/Taller.sln` con el SDK indicado en `global.json`.
2. Seguir el [instructivo breve de instalación Demo](docs/instalacion/replicar-demo.txt). La base acordada es **SGTM_DB_Demo** y el cargador se ejecuta expresamente antes del primer inicio.
3. Seleccionar **Taller.Presentacion** como proyecto de inicio. El perfil local debe apuntar a la misma base que se cargó.

El esquema vigente parte de **EsquemaInicialCircuitoActual**. No aplicar esta migración inicial sobre tablas de versiones anteriores. Una base ya creada con este esquema no necesita recrearse por una reorganización de archivos. La instalación real usa SGTM_DB y no carga el lote académico.

## Abrir y recorrer el código

`Taller/Taller.sln` agrupa Sistema (cuatro capas), Herramientas, Pruebas y Documentación. `Taller/Taller.Sistema.slnf` abre solo las cuatro capas sin duplicar archivos. Las herramientas de demostración y pruebas no se ejecutan al abrir la aplicación; el agente de respaldos es independiente.

| Proyecto | Responsabilidad |
|---|---|
| Taller.Dominio | Entidades, enumeraciones y constantes de negocio. |
| Taller.Aplicacion | Servicios de casos de uso, validaciones, autorizaciones, DTOs y contratos. |
| Taller.Infraestructura | Implementaciones EF/SQL, configuraciones, migración, unidad de trabajo, seguridad y respaldos. |
| Taller.Presentacion | Formularios, navegación, composición de DI e impresión. |
| Taller.Demostracion | Lote académico generado mediante servicios reales; carga explícita en base _Demo vacía. |
| Taller.Respaldos | Agente para copias programadas e instalación del control de recuperación. |
| Taller.Pruebas | Reglas/componentes, arquitectura e integración SQL aislada. |
| Taller.Pruebas.Interfaz | Construcción de formularios y validación de DI en Windows/STA. |

[Índice de documentación](docs/README.md) · [Organización y dependencias](docs/arquitectura/estructura-solucion.md)

## Circuito y roles

Recepción identifica cliente/vehículo y registra pedidos conocidos o síntomas, sin cargos automáticos. El técnico toma la preparación, documenta diagnóstico cuando corresponde y presupuesta. Recepción comunica y registra la decisión del cliente. La aceptación total habilita ejecución; la selección parcial exige validación técnica; sin cambios habilita ejecución, mientras que una reformulación requiere nueva aceptación. Se registran cantidades realmente ejecutadas y conformidad técnica; luego se liquida, se cobra y se entrega con saldo cero.

- **Administrador:** usuarios, auditoría, reportes de gestión, respaldos y recuperación con las autorizaciones correspondientes.
- **Administrativo:** clientes/vehículos, recepción, comunicación y decisión de presupuestos, liquidación, cobros, entrega y reportes de su rol.
- **Técnico:** informes/propuestas, revisión técnica, ejecución, hallazgos, pausas y materiales del cliente; consulta y reportes de su rol.

La sesión permanece hasta cerrar sesión o salir. Las operaciones revalidan usuario/rol habilitados contra persistencia. Los controles visuales no reemplazan autorización ni concurrencia.

## Alcance implementado y límites

- Catálogo de consulta con categorías, tipos, disponibilidad, búsqueda y precios de referencia; **no incluye ABM de conceptos, stock, compras ni inventario**. Servicios no representan unidades almacenadas.
- Hallazgos y presupuestos adicionales independientes, inspecciones extraordinarias previamente autorizadas, pausas por alcance y materiales aportados por el cliente.
- Revisiones conservan documentos anteriores; no reescriben ítems ya ejecutados. Liquidación sobre cantidades autorizadas y efectivamente realizadas.
- Pagos parciales y varios medios por cobro; referencias de operaciones electrónicas, recibos y anulaciones justificadas. Entrega explícita con saldo cero.
- Documentos económicos internos en ARS, **sin validez fiscal**. Sin anticipos, financiación, crédito posterior a la entrega ni devolución bancaria automática.
- Siete reportes con filtros, gráficos, HTML autocontenido, CSV e impresión/PDF. No calculan rentabilidad ni horas de productividad, porque no se registran esos costos/horas.
- Respaldos completos/diferenciales, manuales/programados y réplica opcional Azure. Requieren configurar permisos, control y agente; permanecen deshabilitados hasta instalar ese entorno. Recuperación con mantenimiento y doble aprobación; sin promesa de operación sin interrupción ni copias de log.

## Verificaciones

Desde la raíz, en un equipo con el SDK correspondiente:

```powershell
dotnet build Taller/Taller.sln --configuration Release
dotnet build Taller/Taller.Sistema.slnf --configuration Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj --configuration Release --no-build
```

Las pruebas SQL requieren una instancia desechable explícitamente autorizada; nunca se dirigen a la base de trabajo. CI construye la solución/filtro en Windows, comprueba formularios con DI validada y ejecuta las pruebas SQL en bases temporales. La comprobación visual del recorrido, escalado e impresión sigue siendo necesaria en la PC/notebook de presentación.

Los editables de requisitos y casos de uso en `docs/ERS` se preservan para actualización académica. No se presentan los diagramas/exportaciones anteriores como descripción del esquema actual. El alcance operativo vigente y los límites se consultan en [la especificación implementada](docs/especificacion/alcance-implementado.md).

Esta versión incorpora SimplificarEjecucionTresEstados y Demo v4. Antes de abrir una base existente, seguir [actualización](docs/instalacion/actualizacion-coloquio.md). Para dos puestos, [red privada](docs/instalacion/red-privada.md); para copias, [prueba local de recuperación](docs/instalacion/respaldos-locales-demo.md).

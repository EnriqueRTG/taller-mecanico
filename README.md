# SGTM — Sistema de Gestión de Taller Mecánico

Proyecto académico de Taller de Programación II, Licenciatura en Sistemas de Información, UNNE. Aplicación Windows Forms con SQL Server para recepción, preparación técnica, presupuestos, ejecución, liquidación, cobros y entrega.

## Elegir el entorno

| Objetivo | Punto de partida |
|---|---|
| Presentar el circuito con datos de ejemplo | [Demo v5](scripts/recursos/replicar-demo.txt) en `SGTM_DB_Demo`. |
| Instalar en Windows con una base nueva | [Manual de instalación](docs/manuales/Manual-de-instalacion-y-configuracion-SGTM.docx) y paquete publicado para inicializar `SGTM_DB`. |
| Actualizar una base existente | [Procedimiento de actualización](REFERENCIA-TECNICA.md#instalacion-actualizacion-coloquio). |
| Consultar la entrega académica | [Índice de documentación](docs/README.md). |

## Instalar SGTM en un equipo nuevo

En Windows x64, prepará una instancia de SQL Server accesible y colocá `SGTM.Setup.exe` junto a `SGTM.Instalador.msi`. Abrí PowerShell con permisos de instalación y ejecutá `.\SGTM.Setup.exe --instalacion-inicial`. El asistente solicita la contraseña de `admin`, inicializa `SGTM_DB` con el catálogo y genera `appsettings.Local.json`. Si la instancia difiere de `localhost\SQLEXPRESS`, definí `SGTM_SQL_SERVIDOR` antes de iniciar.

Consultá el [manual de instalación y configuración](docs/manuales/Manual-de-instalacion-y-configuracion-SGTM.docx) para permisos, primera apertura y respaldo. Para una base existente seguí el procedimiento de actualización de la referencia técnica.

## Ejecutar la demostración

1. Descargar `main` y compilar [Taller.sln](Taller/Taller.sln) en Windows con el SDK .NET 10 indicado en [global.json](global.json) y SQL Server accesible.
2. Seguir el [instructivo breve de instalación Demo](scripts/recursos/replicar-demo.txt). La base acordada es **SGTM_DB_Demo** y el cargador se ejecuta expresamente antes del primer inicio.
3. Seleccionar **Taller.Presentacion** como proyecto de inicio. El perfil local debe apuntar a la misma base que se cargó.

El esquema vigente parte de **EsquemaInicialCircuitoActual**. No aplicar esta migración inicial sobre tablas de versiones anteriores. Una base ya creada con este esquema no necesita recrearse por una reorganización de archivos. La instalación real usa SGTM_DB y no carga el lote académico.

## Abrir y recorrer el código

`Taller/Taller.sln` agrupa Sistema (cuatro capas), Herramientas, Pruebas y Documentación. `Taller/Taller.Sistema.slnf` abre solo las cuatro capas sin duplicar archivos. Las herramientas de demostración y pruebas no se ejecutan al abrir la aplicación; el agente de respaldos es independiente.

| Proyecto | Responsabilidad |
|---|---|
| SGTM.Setup | Lanzador del paquete de instalación e inicialización. |
| SGTM.Instalador | Proyecto MSI que empaqueta la aplicación y los recursos de preparación. |
| Taller.Dominio | Entidades, enumeraciones y constantes de negocio. |
| Taller.Aplicacion | Servicios de casos de uso, validaciones, autorizaciones, DTOs y contratos. |
| Taller.Infraestructura | Implementaciones EF/SQL, configuraciones, migración, unidad de trabajo, seguridad y respaldos. |
| Taller.Presentacion | Formularios, navegación, composición de DI e impresión. |
| Taller.Demostracion | Lote académico generado mediante servicios reales; carga explícita en base _Demo vacía. |
| Taller.Respaldos | Agente para copias programadas e instalación del control de recuperación. |
| Taller.Pruebas | Reglas/componentes, arquitectura e integración SQL aislada. |
| Taller.Pruebas.Interfaz | Construcción de formularios y validación de DI en Windows/STA. |

[Entrega académica](docs/README.md) · [Referencia técnica](REFERENCIA-TECNICA.md) · [Scripts](scripts/README.md)

## Circuito y roles

Recepción identifica cliente/vehículo y registra pedidos conocidos o síntomas, sin cargos automáticos. El técnico toma la preparación, documenta diagnóstico cuando corresponde y presupuesta. Recepción comunica y registra la decisión del cliente. La aceptación total habilita ejecución; la selección parcial exige validación técnica; sin cambios habilita ejecución, mientras que una reformulación requiere nueva aceptación. Se registran cantidades realmente ejecutadas y conformidad técnica; luego se liquida, se cobra y se entrega con saldo cero.

- **Administrador:** usuarios, auditoría, reportes de todo el taller, respaldos y recuperación con las autorizaciones correspondientes.
- **Administrativo:** clientes/vehículos, recepción, comunicación y decisión de presupuestos, liquidación, cobros, entrega y reportes de sus intervenciones, decisiones y movimientos. En saldos consulta el saldo total de las atenciones en que intervino.
- **Técnico:** informes/propuestas, revisión técnica, ejecución, hallazgos, pausas y materiales del cliente; consulta y reportes de su rol.

La sesión permanece hasta cerrar sesión o salir. Las operaciones revalidan usuario/rol habilitados contra persistencia. Los controles visuales no reemplazan autorización ni concurrencia.

Consultá la [guía de operación](docs/manual-usuario.md) o la [ayuda de la aplicación](Taller/Taller.Presentacion/Ayuda/index.html) para el recorrido por rol.

## Alcance implementado y límites

- Catálogo de consulta con categorías, tipos, disponibilidad, búsqueda y precios de referencia; **no incluye ABM de conceptos, stock, compras ni inventario**. Servicios no representan unidades almacenadas.
- Hallazgos y presupuestos adicionales independientes, inspecciones extraordinarias previamente autorizadas, pausas por alcance y materiales aportados por el cliente.
- Revisiones conservan documentos anteriores; no reescriben ítems ya ejecutados. Liquidación sobre cantidades autorizadas y efectivamente realizadas.
- Pagos parciales y varios medios por cobro; referencias de operaciones electrónicas, recibos y anulaciones justificadas. Entrega explícita con saldo cero.
- Documentos económicos internos en ARS, **sin validez fiscal**. Sin anticipos, financiación, crédito posterior a la entrega ni devolución bancaria automática.
- Siete reportes con filtros, gráficos, HTML autocontenido, CSV e impresión/PDF. No calculan rentabilidad ni horas de productividad, porque no se registran esos costos/horas.
- Respaldos completos/diferenciales, manuales/programados y paquetes para Google Drive. El respaldo manual requiere permisos SQL, carpeta y base de control. El agente se usa para la programación automática; Google Drive es opcional. Recuperación con mantenimiento y doble aprobación; sin promesa de operación sin interrupción ni copias de log.

## Verificaciones

Desde la raíz, en un equipo con el SDK correspondiente:

```powershell
dotnet build Taller/Taller.sln --configuration Release
dotnet build Taller/Taller.Sistema.slnf --configuration Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj --configuration Release --no-build
```

Las pruebas SQL requieren una instancia desechable explícitamente autorizada; nunca se dirigen a la base de trabajo. CI construye la solución/filtro en Windows, comprueba formularios con DI validada y ejecuta las pruebas SQL en bases temporales. La comprobación visual del recorrido, escalado e impresión sigue siendo necesaria en la PC/notebook de presentación.

La [documentación v2.0](docs/README.md) incluye ERS, casos de uso, diccionario, DER, diagramas y [trazabilidad](docs/especificacion/trazabilidad.md) alineados con el circuito vigente.

Esta versión utiliza las seis migraciones del circuito actual y el lote ampliado Demo v5. Antes de abrir una base existente, seguir [actualización](REFERENCIA-TECNICA.md#instalacion-actualizacion-coloquio). Para dos puestos, [red privada](REFERENCIA-TECNICA.md#instalacion-red-privada); para copias, [prueba local de recuperación](REFERENCIA-TECNICA.md#instalacion-respaldos-locales-demo).

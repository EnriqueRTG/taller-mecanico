# Revisión por capas y simplicidad — 04/11/2026

Entrega: [PR 75](https://github.com/EnriqueRTG/taller-mecanico/pull/75), rama `mejora/rendimiento-operaciones`. La revisión parte de 93826c6; 47937a6 contiene la primera corrección de búsquedas, alcance de actividad y guía Drive. El commit que contiene este informe reúne las simplificaciones posteriores. Los checks del PR identifican la revisión ejecutada; no se debe suponer que el PR ya está integrado a main.

## Alcance y método

Se recorrieron los ocho proyectos y el inventario de 334 archivos C# de fuente, excluyendo bin/obj y migraciones generadas del conteo. Se contrastaron responsabilidades, referencias, registros DI, contratos y llamadas, validaciones, consultas y guardados, excepciones, rutas visuales y cobertura de pruebas. Hay 17 archivos de servicios, 55 archivos DTO, 15 contratos de persistencia, 13 repositorios y 40 archivos de formularios sin Designer. El inventario y la búsqueda de referencias orientan la revisión; no equivalen a ejecutar cada sentencia ni a demostrar ausencia universal de errores.

Se revisaron también configuraciones JSON, referencias entre proyectos, XML de diagramas, enlaces locales de documentación y scripts de preparación. Los DOCX académicos permanecen como la instantánea del cierre anterior; este cambio no modifica tablas, estados de dominio ni migraciones.

## Responsabilidades y dependencias

| Parte | Responsabilidad observada | Resultado |
|---|---|---|
| Dominio | Entidades, estados, constantes y hora del taller. | No depende de Presentación, Aplicación, EF ni herramientas. |
| Aplicación | Autoriza y coordina reglas, repositorios abstractos y DTO. | No ejecuta SQL ni depende de WinForms o del DbContext. |
| Infraestructura | EF, SP de usuarios, repositorios, hash, archivos y motor de respaldos. | Implementa contratos de Aplicación; no decide acciones visuales. |
| Presentación | Eventos, controles, navegación, documentos y mensajes. | Invoca servicios; no guarda entidades mediante SaveChanges ni ejecuta SQL. Program compone el contenedor. |
| Demostración | Carga académica controlada mediante los servicios. | Separada de la aplicación normal; no debe ejecutarse sobre datos reales. |
| Agente | Ejecución de agenda fuera del formulario. | Un solo motor técnico, cancelación y registro de fallos; no inicia una sesión visual. |
| Pruebas | Reglas, SQL real, arquitectura e interfaz Windows. | Verifican niveles distintos; no son código operativo duplicado. |

DI/IoC mantiene sesión y coordinación técnica como Singleton; contexto, repositorios y servicios de negocio como Scoped; formularios como Transient. No se incorporan nuevas capas, mediadores, buses, repositorios genéricos ni una interfaz para cada clase. Una rutina pura como BusquedaTexto puede ser estática sin contradecir DI.

Los DTO se utilizan donde separan datos de pantalla, decisiones, documentos y servicios técnicos. Los CRUD simples todavía devuelven algunas entidades sin seguimiento: no es una inversión de dependencias ni habilita escritura directa desde formularios. Convertir cada retorno a otro DTO no aporta por sí mismo una mejora de rendimiento.

## Revisión de los servicios

| Servicio | Secuencia y controles revisados |
|---|---|
| AutenticacionServicio | Datos admisibles, bloqueo compartido, consulta de cuenta/rol, verificación de hash y registro del intento. Credenciales inválidas no revelan la causa; fallos técnicos se propagan. La contraseña no usa la comparación cultural de búsquedas. |
| SesionUsuario | Mantiene identidad en memoria y permite cierre explícito; no autentica ni consulta SQL. Los servicios revalidan habilitación al operar. |
| UsuarioServicio | Administrador actual, restricciones de cuenta, versión, rol activo y protección del último administrador. Escrituras por SP parametrizados. |
| ClienteServicio | Autoriza recepción; normaliza datos; exige documento único y versión; conserva baja lógica e historial relacionado. |
| VehiculoServicio | Autoriza; valida dominio/modelo/marca y año; aplica concurrencia. Las consultas de modelos filtran en SQL. |
| AtencionServicio | Autoriza una vez, valida y normaliza las solicitudes una vez, deriva tipo/motivo, comprueba maestros y atención activa, confirma expediente e historial. Se retiraron dos auxiliares privados que solo repetían esa preparación. |
| ConceptoServicio | Consulta catálogo y disponibilidad; no presenta precio de referencia como precio histórico ni inventa stock. |
| DiagnosticoServicio | Consulta un informe definitivo por DTO y rol; el registro coordinado pertenece a EvaluacionServicio. |
| EvaluacionServicio | Reserva, avance, solicitudes, diagnóstico opcional según motivo y propuesta. Finalizar exige resolver inspecciones/adicionales y pausas. Confirma cambios relacionados e historial juntos. |
| PresupuestoServicio | Presentación/decisión por ítem, contacto real, vigencia y versión. Corrección administrativa no se confunde con consentimiento del cliente. |
| SeguimientoTrabajoServicio | Hallazgos, pausas, materiales y adicionales; verifica atención, autor, reserva y pertenencia. Selección parcial exacta conserva consentimiento; reformular requiere nueva aceptación. |
| EjecucionServicio | Solo ítems aceptados, cantidades monotónicas, estados coherentes y materiales conformes. Finalización requiere completar el alcance y resolver impedimentos. |
| CobroServicio | Liquidación de lo autorizado y realizado; pago idempotente, sumas exactas, anulación histórica y entrega con saldo cero. El detalle económico conserva lectura transaccional breve para no mezclar datos concurrentes. |
| ReporteServicio | Revalida rol; aplica período/estado/búsqueda al mismo conjunto que alimenta indicadores, gráfico y exportaciones. La actividad técnica es una selección de hitos, no una auditoría completa ni productividad. |
| InicioServicio | Autoriza una vez y reutiliza alcance para tarjetas y pendientes; no genera todos los reportes completos para mostrar tarjetas. |
| AuditoriaAccesoServicio | Restringida a administrador, con rango válido de hasta un año y sin fechas futuras. |
| RespaldoServicio | Administrador, rutas/plan y doble aprobación para sustituir datos; el motor ejecuta SQL y archivos. Las copias manuales no requieren el agente; la agenda sí. |

## Persistencia, validaciones y errores

Se conservan dos tipos de confirmación adecuados a su operación: repositorio/SP para un CRUD independiente y unidad de trabajo para cambios coordinados. No es necesario imponer una transacción de toda la sesión o de toda la atención.

La comprobación anticipada de un documento duplicado ayuda al operador; el índice único SQL resuelve la carrera entre dos puestos. La comprobación de cliente/vehículo antes del alta permite informar el problema; la revalidación dentro de la transacción protege frente a cambios posteriores. Estas repeticiones tienen propósitos diferentes y no se retiraron para ahorrar consultas.

Las lecturas de bandeja usan AsNoTracking o proyecciones; las mutaciones cargan entidades con seguimiento y versión. Los servicios coordinados descartan seguimiento después de completar o fallar. Las conexiones administradas por EF y las conexiones nativas con using/await using conservan su disposición. El interceptor de mantenimiento libera el applock y estaciona sesiones del pool en master; sus callbacks síncronos siguen las firmas del framework y sus rutinas asíncronas evitan capturar el contexto visual.

Los fallos esperados se traducen a ValidacionException o ConflictoConcurrenciaException. Los fallos técnicos desconocidos se propagan hasta la presentación y se registran sin mostrar SQL al operador. Se corrigieron cuatro relanzamientos de clientes/vehículos que podían perder la pila original del fallo al lanzar nuevamente el mismo objeto de excepción. Las traducciones de duplicidad y concurrencia permanecen.

## Cambios concretos de esta revisión

1. Búsquedas de los siete reportes, Usuarios, Atenciones y Comprobantes alineadas con BusquedaTexto. Mayúsculas y tildes se ignoran al consultar; no cambia autenticación, claves únicas ni contenido guardado.
2. CorreccionAdicionalPreparada conserva su registro en el expediente sin agregar otra opción al reporte. Se explicita el alcance selectivo del reporte tanto en el documento generado como en la guía.
3. Atención deja de copiar/validar solicitudes y derivar el tipo dos veces. Se elimina el método privado de registro intermediario y la normalización de un motivo ya derivado de solicitudes válidas.
4. Se retira de FrmInicio un campo y parámetro ReporteServicio sin uso; InicioServicio conserva la coordinación del resumen. El aviso de revisión pendiente usa un texto que también contempla correcciones de adicionales.
5. Clientes y vehículos conservan excepción y pila originales de fallos técnicos desconocidos, manteniendo el descarte de cambios fallidos.
6. Un manifiesto Drive incompleto tiene un mensaje orientado a completar la descarga; un tipo de copia desconocido se rechaza antes de interpretar/importar su cadena. No se abre una conexión ni se modifica negocio para comprobar ese formato.
7. La guía auxiliar de replicación utiliza Google Drive y carpeta SQL local; se actualizan índice y trazabilidad para distinguir evidencia actual de registros históricos.

## Archivos y código que se conservan con motivo

No se identificaron contratos de persistencia sin llamadas directas en el código operativo. Las clases de configuración EF se descubren por ensamblado; la fábrica DbContext se utiliza mediante herramientas; los callbacks se invocan desde el framework. No se consideran muertos por no tener una llamada escrita con su nombre de clase.

Se conservan las seis migraciones y su snapshot: son parte de la reproducción y actualización del esquema. Se conservan Designer/resx, exportadores de documentos y sus variantes tabulares, porque tienen consumidores distintos. El instalador del servicio de respaldos locales y la tarea de sesión para Drive tienen entornos diferentes; no se debe instalar ambos para el mismo plan. Los informes anteriores conservan evidencia enlazada y están identificados como históricos, no como guías vigentes.

No se retiraron paquetes únicamente por ausencia de un using directo: una referencia puede fijar una dependencia transitiva. Una retirada adicional debe contrastarse con el grafo restaurado de NuGet y sus advertencias, no con el nombre del paquete.

## Oportunidades restantes y límites

| Prioridad | Observación | Qué demostrar antes de cambiar |
|---|---|---|
| Media, crecimiento de datos | ListarPresupuestosAsync todavía carga detalles y relaciones para la bandeja; ObtenerAtencionAsync comparte una carga de expediente amplia entre varios comandos. Hay margen para proyecciones/cargas específicas. | Medir comandos, filas y latencia en el equipo objetivo. No dividir automáticamente una operación coordinada en consultas que mezclen versiones. |
| Media, sesiones largas | FrmPrincipal resuelve formularios Transient descartables desde el scope de sesión. El contenedor puede conservar referencias a instancias descartadas hasta terminar ese scope. Es un riesgo de retención, no una fuga cuantificada en esta revisión. | Medir memoria con navegación repetida. Un scope por módulo exige probar navegación, formularios hijos, errores y cierre antes de adoptarlo. |
| Baja, volumen futuro | Varias bandejas cargan el conjunto y filtran en memoria; no tienen paginación. El lote pequeño funciona, pero el coste crecerá con años de datos. | Medir volumen real y definir filtros/paginación SQL si hace falta; no agregar complejidad solo por previsión. |
| Instalación | La sincronización remota de Drive y la impresión dependen del equipo/cuenta/controlador. | Crear completa manual, exportar y comprobar en Drive web; importar/ensayar en copia de prueba. Verificar señal del agente y horario real si se programa. |

No se detectó una nueva ruptura de dependencias entre las cuatro capas ni un cambio necesario de arquitectura. Las oportunidades anteriores no prueban una regresión actual de rendimiento; requieren medición y pruebas propias antes de reestructurar funciones ya validadas.

## Evidencia de verificación

El commit 47937a6 aprobó compilación, 187 pruebas de reglas/componentes, 32 pruebas SQL sin omisiones, verificación Windows y GitGuardian. Ese bloque incluye búsquedas comunes de los siete reportes y conservación del historial sin un nuevo hito.

Las pruebas nuevas posteriores comprueban cuatro rutas de fallo de maestros y dos rechazos de manifiesto, además del recorrido de recepción existente para trabajo, problema y solicitud mixta. Consultar los checks del commit final para su resultado. Las pruebas SQL concilian los siete reportes contra consultas del lote, prueban cobros/entrega, selección parcial y adicional, y ejercitan completa/diferencial/restauración con protección ante fallos. No se modificó ninguna base del usuario.

[Plan de aceptación](aceptacion-version-actual.md) · [Cierre de coherencia anterior](cierre-coherencia-2026-11-04.md) · [Respaldo y Google Drive](../respaldos/google-drive.md).

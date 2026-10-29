# Organización de la solución

Abrir `Taller/Taller.sln` para ver el proyecto completo. Las carpetas de solución separan responsabilidades, sin añadir capas:

| Carpeta de Visual Studio | Proyectos | Propósito |
|---|---|---|
| 01 Sistema (cuatro capas) | Taller.Dominio, Taller.Aplicacion, Taller.Infraestructura, Taller.Presentacion | Aplicación de negocio y sus cuatro capas. |
| 02 Herramientas | Taller.Demostracion, Taller.Respaldos | Preparar el lote académico y ejecutar copias programadas independientemente de la interfaz. |
| 03 Pruebas | Taller.Pruebas, Taller.Pruebas.Interfaz | Verificar reglas, SQL, documentos y construcción de formularios. No se distribuyen al usuario. |
| 04 Documentacion | Índice de documentación, instructivo TXT y guía de organización | Acceso al circuito, estados, preparación de la base y demostración. |

Para ver solo las cuatro capas abrir `Taller/Taller.Sistema.slnf`. Es un filtro de la misma solución: usa los mismos archivos y proyectos. No duplica código ni elimina herramientas. Para volver a verlas, abrir Taller.sln. En Visual Studio elegir Taller.Presentacion como proyecto de inicio para F5; esta selección es local a cada equipo.

## Carpetas físicas

Los ocho proyectos continúan en directorios identificados por su nombre dentro de Taller. No se movieron: esto conserva rutas de referencias, comandos, migraciones y CI. Las carpetas numeradas son agrupaciones virtuales de Visual Studio; no son directorios adicionales de Windows. En el explorador se distinguen por los nombres Taller.Demostracion, Taller.Pruebas, Taller.Pruebas.Interfaz y Taller.Respaldos.

## Referencias e inyección de dependencias

- Dominio no referencia otros proyectos del sistema.
- Aplicación referencia Dominio. Define contratos y coordina operaciones; no referencia EF, Infraestructura ni formularios.
- Infraestructura referencia Aplicación y Dominio e implementa los contratos con EF/SQL y servicios técnicos.
- Presentación referencia Aplicación e Infraestructura. Program es el punto de composición: registra ambas mediante AddAplicacion, AddInfraestructura y AddPresentacion. Los formularios invocan servicios; no ejecutan consultas SQL. El tratamiento visual de errores técnicos conserva mensajes sanitizados.
- Las herramientas y pruebas utilizan esas capas. Las cuatro capas no dependen de ellas.

La sesión es Singleton y se comparte entre formularios. Servicios, repositorios y DbContext son Scoped; formularios son Transient según su registro. AplicacionContexto gestiona los alcances de acceso y de la sesión visual. La construcción de interfaz en Windows verifica los registros con ValidateOnBuild y ValidateScopes, además de crear formularios. Las pruebas de arquitectura comprueban que Dominio, Aplicación e Infraestructura no incorporen referencias invertidas.

## Sesión y autorizaciones

La sesión no vence por tiempo ni inactividad. Termina por Cerrar sesión o salir de la aplicación. El tiempo sin clientes no obliga a autenticarse nuevamente. La identidad se mantiene en memoria; los servicios revalidan la cuenta activa y el rol contra persistencia al autorizar una operación. Deshabilitar un usuario no le permite seguir operando por conservar una ventana abierta. Las aprobaciones y vencimientos de planes de restauración son independientes y se mantienen.

## Preparación para el coloquio

1. Descargar la misma versión de main en ambos equipos.
2. Preparar la base nueva con la guía de entornos y cargar Demo v5 antes de abrir la aplicación. No aplicar el esquema inicial sobre la base anterior.
3. Configurar en cada equipo la conexión al destino elegido y seleccionar Taller.Presentacion como inicio.
4. Ejecutar el recorrido principal con cuentas de recepción y técnico. Si ambas máquinas colaboran sobre el mismo circuito, deben usar la misma instancia/base accesible; localhost de cada equipo representa bases diferentes.
5. Las configuraciones de Azure y del agente se realizan después. La versión actual requiere aplicar todas las migraciones, incluida AgregarConcurrenciaUsuarios, con los puestos cerrados. Después actualizar todos los ejecutables antes de reanudar operaciones.

La instalación del puesto incluye Presentación y sus dependencias. Demostración se utiliza para preparar datos académicos, no al iniciar una instalación real. El agente de respaldos se despliega por separado en un equipo disponible para ejecutar las tareas programadas.

Los archivos de pruebas están agrupados en Arquitectura, ReglasYComponentes e IntegracionSql dentro del mismo proyecto. Los namespaces y filtros de CI se conservan; mover los archivos no cambia las pruebas ejecutadas. Los editables v2.0 de docs/ERS y los diagramas de docs/diagramas están vinculados en la matriz de trazabilidad.

# Segunda revisión de consistencia

## Versión y alcance

Base: main 9cfe090 y cambios del PR de segunda revisión. El commit de entrega contiene conjuntamente código, migraciones y documentos. Esta revisión no agrega inventario, órdenes de trabajo ni nuevos flujos comerciales.

## Correcciones aplicadas

- Entrega: una única operación pública recibe RegistrarEntregaDto y exige persona y conformidad. La confirmación interna es privada; se conservan autorización, versión, liquidación, saldo cero e historial.
- Presupuesto: toda decisión exige persona y medio. Evaluación rechaza atenciones sin solicitudes, en vez de habilitar reglas de compatibilidad distintas. Los datos de prueba se adecuan al circuito actual.
- Usuarios: rowversion, versión esperada en datos/credenciales/estado y guardia SQL 51005. Los SP conservan su función y la protección del último administrador. Las ventanas y el listado transportan la versión leída; no obtienen una nueva versión para permitir un guardado obsoleto.
- Apertura: precondiciones de cliente y vehículo habilitados revalidadas por el repositorio en una transacción RepeatableRead breve; alta de atención, solicitudes e historial confirmada conjuntamente.
- Limpieza de fallos: se comprobó que AtencionRepositorio ya desasocia cabecera e hijos. Se mantiene esa implementación y se agrega una prueba de fallo seguido de otro guardado.
- Autorización: AtencionServicio y DiagnosticoServicio reciben AutorizacionOperativa mediante DI. Las validaciones operativas esperadas de Usuarios usan ValidacionException; los fallos técnicos mantienen registro central y mensajes sanitizados.
- Continuidad: un fallo de la consulta periódica bloquea edición y se registra una vez por episodio, sin ventanas periódicas ni repetición ilimitada del mismo error.
- Código sin consumidores: retirados IMarcaRepositorio, MarcaRepositorio y su registro; ListarActivosPorMarcaAsync; ListarInactivosAsync de clientes y vehículos. Se mantienen entidades, catálogo, consultas utilizadas y bajas lógicas.
- Documentación: quinta migración, 205 columnas, concurrencia de usuarios, entrada de entrega, SQL de reportes, instrucciones de actualización y trazabilidad. ERS, Apéndices A/C y DER del repositorio y originales editables se revisan conjuntamente.

## Evidencia automatizada

La solución completa compila en Release. Las pruebas locales de reglas, componentes y arquitectura suman 166 casos correctos. Las 26 pruebas SQL se ejecutan en CI sobre bases temporales aisladas; consultar el resultado del último commit del PR antes de integrar, no resultados de commits previos.

Las nuevas pruebas comprueban versiones obsoletas contra los tres SP, revalidación de maestros del alta y limpieza completa del grafo tras un fallo. El modelo final debe coincidir con snapshot y cinco migraciones. Los trabajos Windows construyen solución, filtro de cuatro capas y formularios reales en STA.

## Actualización de una instalación

Respaldar, cerrar todos los puestos y aplicar todas las migraciones desde una estación de mantenimiento. ExigirTelefonoContactoCliente se detiene si hay contactos inválidos; completarlos con información real. AgregarConcurrenciaUsuarios conserva usuarios y cambia las firmas internas de los SP. Actualizar después todos los clientes; no mezclar versiones. No se modificó la base del usuario durante esta revisión.

## Aceptación pendiente del entorno

- Recorrer el flujo principal y alternativas acordadas en dos puestos Windows conectados a la misma base.
- Verificar HD/Full HD, escalado y documentos multipágina.
- Contrastar reportes con SQL y los mismos filtros.
- Configurar permisos, carpeta, control y agente reales de respaldos; comprobar copias manuales y programadas.
- Ensayar restauración en una copia descartable y verificar réplica Azure con una cuenta real cuando forme parte del despliegue.

Estas comprobaciones manuales no se consideran aprobadas por compilar ni por ejecutar los tests de CI. La tercera revisión conserva ese objetivo y el cierre formal de la entrega.

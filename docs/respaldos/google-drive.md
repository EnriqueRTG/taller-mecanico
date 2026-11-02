# Respaldo transportable con Google Drive

No requiere Azure ni claves API. Google Drive para escritorio sincroniza los paquetes generados por SGTM. SGTM verifica el archivo escrito y su integridad, pero no consulta el estado remoto de Google. Compruebe que Drive finalizó la sincronización y que el paquete aparece en la web antes de considerarlo una copia externa disponible.

## Preparación

En el equipo Windows que ejecuta SQL Server:

1. Instalar Google Drive para escritorio e iniciar sesión. No hace falta sincronizar Documentos, Escritorio ni Descargas.
2. Crear `SGTM-Respaldos` dentro de Mi unidad. La ruta depende del equipo; ejemplo: `G:\My Drive\SGTM-Respaldos`.
3. Preparar los respaldos locales de SGTM según [operación](operacion.md). SQL escribe primero en una carpeta local con permisos, por ejemplo `C:\SGTM\Respaldos`. No usar la unidad virtual de Drive como destino de BACKUP/RESTORE SQL.

## Copiar a Drive

Como administrador de SGTM, crear y verificar una copia local. Seleccionarla en Copias manuales. En **Drive / copia transportable**, pulsar **Exportar copia seleccionada a Drive** y elegir la carpeta SGTM-Respaldos.

Se genera una carpeta única con `sgtm-respaldo.json` y uno o dos `.bak`. Una diferencial incluye su completa base. Las carpetas `.pendiente` no son paquetes terminados. No se borran originales ni paquetes anteriores automáticamente. Revise espacio disponible en la cuenta.

Espere a que Drive indique sincronización completada. La confirmación de SGTM solo acredita la generación e integridad local del paquete.

## Recuperar la misma instalación

1. Descargar desde Drive la carpeta completa del paquete, o hacerla disponible sin conexión. Mantener juntos manifiesto y archivos.
2. En **Recuperar paquete desde Drive**, elegir esa carpeta y una carpeta local con permisos de lectura para SQL Server.
3. SGTM comprueba tamaño, SHA-256, familia, versión, cabeceras SQL, CHECKSUM y dependencia completa/diferencial. Registra la cadena en el catálogo sin reemplazar datos activos.
4. En Restaurar, ensayar la copia seleccionada. El ensayo crea una base temporal y ejecuta CHECKDB.
5. Si corresponde, autorizar la restauración mediante el circuito existente de dos aprobadores y copia protectora. Reiniciar los puestos después.

## Alcance actual

La exportación/importación es manual y requiere ejecutar SGTM junto a SQL Server en Windows. La programación del agente conserva sus destinos existentes; no exporta automáticamente a la unidad Drive del usuario, que puede no estar montada para un servicio Windows.

Se admiten paquetes de la misma familia de base y versión. La reconstrucción tras perder toda la instalación (base y control) sigue siendo un procedimiento técnico separado; esta pantalla no adopta arbitrariamente la familia de un archivo externo. El paquete no incluye la base auxiliar de control. No se debe presentar esta función como recuperación automática de un equipo perdido.

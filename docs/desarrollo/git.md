# Trabajo con Git

El código compartido está en https://github.com/EnriqueRTG/taller-mecanico. Cada equipo conserva su configuración local y su base; descargar código no copia datos SQL ni credenciales.

## Actualizar una copia

Desde la raíz, cerrar la aplicación y revisar primero:

```powershell
git status
git fetch origin
git switch main
git pull --ff-only origin main
git rev-parse --short HEAD
dotnet build Taller/Taller.sln
```

Si existen cambios, guardarlos antes en una rama propia. Revisar los archivos antes de agregarlos; no incluir contraseñas, conexiones privadas, respaldos ni datos personales. El perfil local launchSettings.json está excluido. No usar reset --hard para resolver una actualización pendiente.

```powershell
git switch -c trabajo/mis-cambios
git add ruta/del/archivo
git commit -m "Describir el cambio concreto"
git push -u origin trabajo/mis-cambios
```

Si pull --ff-only se detiene, no forzarlo: revisar las diferencias y conservar los commits propios antes de decidir cómo integrarlos. Para replicar la Demo, seguir [el instructivo](../instalacion/replicar-demo.txt).

## Desarrollar e integrar

1. Partir de main actualizado, sin cambios pendientes.
2. Crear una rama con git switch -c tipo/descripcion.
3. Implementar un cambio coherente, actualizar documentación afectada y revisar git diff.
4. Compilar y ejecutar las pruebas adecuadas; comprobar manualmente el recorrido modificado en Windows.
5. Guardar commits y subir la rama con git push -u origin tipo/descripcion.
6. Abrir un pull request hacia main explicando el resultado, validación y limitaciones.
7. Esperar los controles de GitHub Actions y revisar antes de fusionar. Un PR abierto todavía no actualiza main.
8. Tras fusionar, actualizar main en cada equipo y comparar el identificador del commit.

Las pruebas SQL crean bases temporales aisladas; las de interfaz necesitan Windows. La compilación no sustituye la comprobación de navegación, impresión ni la configuración real de Azure. No añadir migraciones vacías por una reorganización de archivos. Los cambios de esquema se revisan y documentan por separado.

## Comprobaciones locales

```powershell
dotnet build Taller/Taller.sln --configuration Release
dotnet test Taller/Taller.Pruebas/Taller.Pruebas.csproj --configuration Release --no-build
dotnet run --project Taller/Taller.Pruebas.Interfaz --configuration Release --no-build
```

Sin conexión de pruebas SQL configurada, esos casos se omiten y deben validarse mediante el trabajo SQL de CI. Conservar los documentos académicos editables y actualizar sus versiones deliberadamente; no reemplazarlos por capturas antiguas.

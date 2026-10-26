# Preparación de SQL Server para el esquema actual

Esta versión requiere una base nueva. No ejecutar la migración inicial sobre tablas de la versión anterior. La guía completa del lote, las credenciales iniciales y la separación de entornos está en [entornos e instalación](entornos-demo-e-instalacion.md).

## Demostración

El cargador aplica la migración y carga el lote en una base vacía terminada en _Demo. No ejecutar antes la aplicación, porque el cargador exige que todavía no haya usuarios.

## Instalación sin lote

Desde PowerShell en la raíz del repositorio, con SQL Server disponible:

```powershell
$env:SGTM_CONN = 'Server=localhost\SQLEXPRESS;Database=SGTM_DB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True'
dotnet ef database update --project Taller/Taller.Infraestructura --startup-project Taller/Taller.Infraestructura
```

La alternativa desde Package Manager Console de Visual Studio es:

```powershell
Update-Database -Project Taller.Infraestructura -StartupProject Taller.Infraestructura
```

Debe existir únicamente la migración terminada en EsquemaInicialCircuitoActual. Ya está incluida en el repositorio; no crear otra migración local. Las herramientas EF deben corresponder a la versión del proyecto. Para desarrollo local se admite confiar explícitamente en el certificado; en despliegue instalar un certificado de servidor válido y verificarlo.

Configurar la misma conexión en el proceso que inicia Visual Studio o en appsettings.Local.json (no versionar secretos). Suministrar la contraseña inicial del administrador según la guía de instalación, ejecutar y retirarla después. No mezclar SGTM_DB con SGTM_DB_Demo. Dos puestos deben apuntar a la misma instancia SQL, no al localhost de cada PC.

Un respaldo del esquema anterior no es compatible con esta versión. La configuración de Azure y la prueba de recuperación se realizan después de verificar la base nueva y el circuito completo.

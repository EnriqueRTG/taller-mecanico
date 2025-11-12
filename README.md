# 🧰 Sistema de Gestión de Taller Mecánico
Aplicación de escritorio desarrollada en **C# (.NET 8)** con **SQL Server**, siguiendo una arquitectura limpia:
- 🧩 `Taller.App`: capa de presentación (Windows Forms)
- ⚙️ `Taller.Domain`: entidades y lógica de negocio
- 🗄️ `Taller.Infrastructure`: acceso a datos con EF Core
- 🔗 `Taller.Shared`: utilidades y componentes compartidos
- 🧪 `Taller.Tests`: pruebas unitarias y de integración


---

## ⚙️ Tecnologías

- **.NET 8 / C#**
- **Entity Framework Core 8**
  - SqlServer
  - Tools
  - Design
- **Serilog** (logging a consola y archivo)
- **SQL Server** (SQLEXPRESS o LocalDB)
- **Visual Studio 2022 (v17.8 o superior)**

---

## 🧱 Requisitos Previos

- Tener instalado:
  - [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download)
  - [SQL Server Express o LocalDB](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - [Visual Studio 2022](https://visualstudio.microsoft.com/es/vs/)
- Git configurado para clonar el repositorio

---

## 🚀 Configuración del Entorno

### 1️⃣ Clonar el proyecto

git clone https://github.com/EnriqueRTG/taller-mecanico.git
cd taller-mecanico

### 2️⃣ Configurar la conexión a la base de datos

El archivo Taller.App/appsettings.json contiene:
"ConnectionStrings": {
  "Default": "Server=.\\SQLEXPRESS;Database=SGTM_DB;Trusted_Connection=True;TrustServerCertificate=True"
}

## 🧩 Migraciones y Seeders

Al ejecutar la aplicación por primera vez:

Se aplican automáticamente las migraciones pendientes (InitialCreate).

Se ejecutan los seeders idempotentes:

ServicioSeeder → carga servicios base (MO-ACEITE, MO-FRENOS, etc.)

RepuestoSeeder → carga repuestos base (RP-FILT-ACE, RP-BUJ-NGK, etc.)

Todo el proceso queda registrado en los logs:
Migraciones aplicadas correctamente (si existían pendientes).
Seeding completado exitosamente.

## 🗃️ Base de Datos

Nombre: SGTM_DB

Migraciones automáticas: Database.MigrateAsync() en DatabaseSeeder

Seeders idempotentes: no duplican datos ya existentes

Tablas principales:

Clientes

Vehiculos

Ordenes

Servicios

Repuestos

Pagos

Usuarios

## 🪵 Logging con Serilog

Configurado para emitir logs a:

Consola / Output de Visual Studio

Archivo diario: logs/app-YYYY-MM-DD.log

Ejemplo de salida:
[2025-11-12 16:10:20 INF] Migraciones aplicadas correctamente.
[2025-11-12 16:10:20 INF] Seeder Repuestos: sin cambios (todos los códigos ya existen).

## 🧭 Flujo de Trabajo en Git
Ramas

main → rama estable

feature/* → ramas de desarrollo

Ejemplo: feature/infra-db-efcore, feature/ui-base

Commits

Usá conventional commits:

feat: nueva funcionalidad

fix: corrección de bug

chore: tarea de mantenimiento/config

docs: documentación

refactor: mejora de código sin cambio funcional

Pull Requests

Crear desde cada feature/* hacia main

Incluir descripción detallada de los cambios

Eliminar rama local y remota luego del merge

## 🧪 Pruebas (pendiente)

El proyecto incluye una capa Taller.Tests donde se agregarán:

Pruebas de entidades (validaciones de dominio)

Pruebas de persistencia (repos y contextos EF)

## 🔧 Troubleshooting
Problema	Causa probable	Solución
Sin logs en Output	No está seleccionada la vista “Debug”	Menú: View → Output → Debug
Error de conexión a DB	Cadena inválida o SQL Server detenido	Verificar en appsettings.Local.json
Incompatibilidad de paquetes	Versiones distintas de EF Core / .NET	Usar EF Core 8.x para .NET 8
Migraciones no aplican	Contexto no compiló o DB creada con cambios	Ejecutar Clean Solution + re-run
Seeder no carga	Datos ya existen	Es idempotente, revisar logs (sin cambios)

## 🗺️ Roadmap

 Configuración de infraestructura y EF Core

 Migraciones iniciales + seeders base

 Implementación de UI base (listado de Servicios y Repuestos)

 Gestión de Clientes y Vehículos

 Módulo de Órdenes de Trabajo

 Facturación y Reportes

 Pruebas unitarias

## 📄 Licencia

Este proyecto está bajo la licencia MIT.
Podés usarlo, modificarlo y redistribuirlo libremente, siempre con atribución al autor original.

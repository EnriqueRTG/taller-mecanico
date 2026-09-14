# Sistema de Gestión de Taller Mecánico — SGTM

Aplicación de escritorio desarrollada para administrar el proceso de atención de vehículos en un taller mecánico.

El sistema corresponde a un proyecto académico de la asignatura **Taller de Programación II**, perteneciente a la **Licenciatura en Sistemas de Información de la Universidad Nacional del Nordeste (UNNE)**.

> **Estado del proyecto:** en desarrollo. Actualmente se encuentran implementadas la autenticación, la gestión de usuarios, la navegación según roles y las operaciones iniciales relacionadas con clientes, vehículos y atenciones.

---

## Objetivo del sistema

El Sistema de Gestión de Taller Mecánico busca centralizar la información necesaria para administrar el flujo de trabajo principal del taller:

1. Registrar el ingreso y la atención del vehículo.
2. Identificar al cliente y al vehículo involucrados.
3. Registrar el diagnóstico técnico.
4. Elaborar y gestionar el presupuesto.
5. Registrar la finalización del trabajo.
6. Emitir el comprobante y registrar sus pagos.
7. Registrar la entrega del vehículo.

El sistema permitirá conservar el historial de las atenciones realizadas y facilitar el trabajo coordinado entre el personal administrativo, técnico y de administración general.

---

## Estado actual

### Funcionalidades implementadas

- Inicio de sesión mediante nombre de usuario y contraseña.
- Verificación segura de contraseñas mediante `PasswordHasher`.
- Control de usuarios activos e inactivos.
- Gestión de la sesión del usuario autenticado.
- Ventana principal común para todos los usuarios.
- Navegación y opciones visibles según el rol.
- Cierre de sesión y regreso al formulario de acceso.
- Gestión inicial de usuarios:
  - listado;
  - búsqueda;
  - alta;
  - modificación;
  - activación y desactivación lógica;
  - asignación de roles.
- Alta de clientes.
- Alta de vehículos.
- Registro de nuevas atenciones.
- Búsqueda y selección de clientes y vehículos durante una atención.
- Alta de un cliente o vehículo desde el flujo de atención.
- Validaciones de negocio para:
  - usuarios;
  - clientes;
  - vehículos;
  - atenciones.
- Persistencia en SQL Server mediante Entity Framework Core.
- Migraciones de la base de datos.
- Inicialización del usuario administrador.
- Inicialización de un catálogo básico de marcas y modelos.

### Funcionalidades en desarrollo

Las siguientes secciones poseen formularios iniciales o acceso desde la navegación principal, pero todavía no cuentan con una implementación funcional completa:

- Gestión completa de clientes.
- Gestión completa de vehículos.
- Consulta y seguimiento de atenciones.
- Diagnósticos.
- Presupuestos.
- Comprobantes y pagos parciales.
- Entrega de vehículos.
- Reportes.
- Respaldo y restauración de la base de datos.
- Procedimientos almacenados y funciones.
- Pruebas automatizadas.

La existencia de una opción en el menú o de un formulario navegable no implica necesariamente que el módulo esté finalizado.

---

## Roles del sistema

El sistema contempla tres roles principales:

| Rol | Responsabilidades |
|---|---|
| **Administrador** | Gestionar usuarios, consultar reportes generales y administrar respaldos del sistema |
| **Administrativo** | Gestionar clientes, vehículos, atenciones, decisiones sobre presupuestos, comprobantes y pagos |
| **Técnico** | Consultar atenciones asignadas, registrar diagnósticos, elaborar presupuestos técnicos y registrar la finalización del trabajo |

Los permisos se aplican en la interfaz para que cada usuario visualice únicamente las funciones correspondientes a su rol.

---

## Tecnologías utilizadas

- **C#**
- **.NET 10**
- **Windows Forms**
- **SQL Server**
- **Entity Framework Core 10**
- **Microsoft.Extensions.Hosting**
- **Microsoft.Extensions.DependencyInjection**
- **Microsoft.Extensions.Configuration**
- **ASP.NET Core Identity PasswordHasher**
- **Serilog**
- **xUnit**
- **FluentAssertions**
- **Git y GitHub**

---

## Arquitectura

La solución se organiza mediante una arquitectura en capas:

```text
Taller/
├── Taller.Dominio/
├── Taller.Aplicacion/
├── Taller.Infraestructura/
├── Taller.Presentacion/
├── Taller.Pruebas/
└── Taller.sln
```

### Taller.Dominio

Contiene las entidades, enumeraciones y constantes principales del negocio.

Entidades implementadas actualmente:

- `Usuario`
- `Rol`
- `Cliente`
- `Vehiculo`
- `Marca`
- `Modelo`
- `Atencion`

También contiene:

- los identificadores constantes de los roles;
- los estados posibles de una atención.

Esta capa no depende de la infraestructura ni de la interfaz gráfica.

### Taller.Aplicacion

Contiene los servicios de aplicación y las abstracciones utilizadas por las demás capas.

Incluye:

- `AutenticacionServicio`
- `UsuarioServicio`
- `ClienteServicio`
- `VehiculoServicio`
- `AtencionServicio`
- `SesionUsuario`
- interfaces de repositorios;
- abstracción para el hash de contraseñas;
- validaciones y reglas de negocio.

### Taller.Infraestructura

Implementa el acceso a SQL Server mediante Entity Framework Core.

Incluye:

- `TallerDbContext`;
- configuraciones Fluent API;
- repositorios;
- migraciones;
- inicialización de datos;
- implementación del hash de contraseñas;
- registro de dependencias de infraestructura.

### Taller.Presentacion

Contiene la aplicación Windows Forms y es responsable de:

- mostrar el formulario de inicio de sesión;
- administrar la ventana principal;
- aplicar la navegación según el rol;
- mostrar los formularios dentro del área principal;
- gestionar usuarios;
- registrar clientes y vehículos;
- iniciar el flujo de una nueva atención;
- proporcionar acceso a los módulos todavía en desarrollo.

### Taller.Pruebas

Proyecto destinado a las pruebas unitarias y de integración.

Actualmente posee las dependencias necesarias para trabajar con:

- xUnit;
- FluentAssertions.

La implementación de la suite de pruebas todavía se encuentra pendiente.

---

## Base de datos

El sistema utiliza **SQL Server** y Entity Framework Core mediante el contexto:

```text
TallerDbContext
```

### Tablas implementadas actualmente

- `Roles`
- `Usuarios`
- `Clientes`
- `Marcas`
- `Modelos`
- `Vehiculos`
- `Atenciones`

### Migraciones existentes

- `InicialUsuariosRoles`
- `AgregarRolesIniciales`
- `AgregarClientes`
- `AgregarMarcasModelosVehiculos`
- `AgregarAtenciones`

Las migraciones deben aplicarse antes de ejecutar la aplicación por primera vez sobre una base de datos nueva.

---

## Datos iniciales

Cuando se inicia el sistema se comprueba la existencia de los datos mínimos necesarios.

### Roles iniciales

| Id | Rol |
|---:|---|
| 1 | Administrador |
| 2 | Administrativo |
| 3 | Técnico |

### Usuario administrador

Si todavía no existe, el sistema crea el siguiente usuario:

```text
Usuario: admin
Contraseña: Admin123!
```

Estas credenciales se utilizan únicamente para el entorno académico y de desarrollo.

### Catálogo de vehículos

También se inicializa un catálogo básico de marcas y modelos para permitir el registro inicial de vehículos.

La inicialización es idempotente: los registros existentes no vuelven a crearse.

---

## Requisitos previos

Para compilar y ejecutar el proyecto se necesita:

- Windows 10 u 11.
- .NET SDK compatible con el archivo `global.json`.
- Visual Studio 2022 con la carga de trabajo **Desarrollo de escritorio con .NET**.
- SQL Server Express, Developer o una instancia compatible.
- Git.
- Entity Framework Core CLI para ejecutar migraciones desde la terminal.

Puede comprobarse la instalación de .NET con:

```powershell
dotnet --version
dotnet --list-sdks
```

El repositorio utiliza el SDK indicado en:

```text
global.json
```

---

## Instalación y configuración

### 1. Clonar el repositorio

```powershell
git clone https://github.com/EnriqueRTG/taller-mecanico.git
cd taller-mecanico
```

### 2. Restaurar las dependencias

Desde la raíz del repositorio:

```powershell
dotnet restore Taller/Taller.sln
```

### 3. Instalar Entity Framework Core CLI

Si la herramienta todavía no está instalada:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.11
```

Si ya se encuentra instalada y necesita actualizarse:

```powershell
dotnet tool update --global dotnet-ef --version 10.0.11
```

Para verificarla:

```powershell
dotnet ef --version
```

---

## Configuración de la conexión

El archivo versionado:

```text
Taller/Taller.Presentacion/appsettings.json
```

no contiene una cadena de conexión real.

Para el entorno local se recomienda crear el archivo:

```text
Taller/Taller.Presentacion/appsettings.Local.json
```

con el siguiente contenido:

```json
{
  "ConnectionStrings": {
    "Default": "Server=.\\SQLEXPRESS;Database=SGTM_DB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Si la instancia de SQL Server posee otro nombre, debe reemplazarse:

```text
.\SQLEXPRESS
```

por el servidor correspondiente.

### Variable de entorno

Como alternativa puede utilizarse la variable de entorno:

```text
SGTM_CONN
```

Ejemplo para la sesión actual de PowerShell:

```powershell
$env:SGTM_CONN = "Server=.\SQLEXPRESS;Database=SGTM_DB;Trusted_Connection=True;TrustServerCertificate=True"
```

Para comprobar su contenido:

```powershell
$env:SGTM_CONN
```

La variable `SGTM_CONN` tiene prioridad sobre la cadena definida en los archivos de configuración.

> Las cadenas de conexión locales, credenciales y contraseñas privadas no deben subirse al repositorio.

---

## Aplicar las migraciones

Las migraciones no deben confundirse con la inicialización de datos. Primero debe existir la estructura de la base de datos y luego la aplicación podrá comprobar los datos iniciales.

### Desde PowerShell

Ejecutar desde la raíz del repositorio:

```powershell
dotnet ef database update `
  --project Taller/Taller.Infraestructura `
  --startup-project Taller/Taller.Presentacion
```

### Desde Git Bash

```bash
dotnet ef database update --project Taller/Taller.Infraestructura --startup-project Taller/Taller.Presentacion
```

Este comando crea o actualiza la base de datos utilizando las migraciones existentes.

---

## Compilar el proyecto

Desde la raíz del repositorio:

```powershell
dotnet build Taller/Taller.sln
```

La compilación debería finalizar sin errores antes de intentar ejecutar la aplicación.

---

## Ejecutar la aplicación

Desde la terminal:

```powershell
dotnet run --project Taller/Taller.Presentacion
```

También puede abrirse:

```text
Taller/Taller.sln
```

en Visual Studio y establecer `Taller.Presentacion` como proyecto de inicio.

---

## Documentación

La carpeta [`docs`](docs) contiene la documentación académica y técnica del proyecto:

```text
docs/
├── Diagramas/
│   ├── DER/
│   └── casos-de-uso/
├── ERS/
├── flujo-trabajo-git.md
├── guia-configuracion-bd-y-migraciones.md
└── Sugerencias TPII.txt
```

Entre los documentos disponibles se encuentran:

- Especificación de Requisitos de Software.
- Diagrama Entidad–Relación.
- Diagramas de casos de uso.
- Guía de configuración de la base de datos.
- Guía para aplicar migraciones.
- Flujo de trabajo colaborativo con Git.

---

## Flujo de trabajo con Git

La rama:

```text
main
```

representa la versión estable e integrada del proyecto.

Las nuevas funcionalidades se desarrollan en ramas separadas:

```text
feature/*
```

Las correcciones puntuales pueden realizarse en ramas:

```text
fix/*
```

### Crear una rama de trabajo

```powershell
git switch main
git pull origin main
git switch -c feature/nombre-funcionalidad
```

### Guardar los cambios

```powershell
git status
git add .
git commit -m "feat: describir funcionalidad"
```

### Publicar una rama por primera vez

```powershell
git push -u origin feature/nombre-funcionalidad
```

La integración a `main` debe realizarse mediante un Pull Request después de revisar y probar los cambios.

La guía completa se encuentra en:

```text
docs/flujo-trabajo-git.md
```

---

## Convención de commits

Se recomienda utilizar mensajes breves y descriptivos:

| Prefijo | Uso |
|---|---|
| `feat` | Nueva funcionalidad |
| `fix` | Corrección de un error |
| `docs` | Cambios de documentación |
| `refactor` | Reorganización del código sin cambiar su comportamiento |
| `test` | Incorporación o modificación de pruebas |
| `chore` | Configuración o mantenimiento |

Ejemplos:

```text
feat: agregar navegación principal por roles
fix: corregir asignación del rol técnico
docs: actualizar README del proyecto
```

---

## Próximos objetivos

- Completar la gestión de clientes.
- Completar la gestión de vehículos.
- Completar el seguimiento de atenciones.
- Implementar el módulo de diagnóstico.
- Implementar la elaboración y decisión del presupuesto.
- Implementar la finalización técnica del trabajo.
- Implementar comprobantes y pagos parciales.
- Implementar la entrega del vehículo.
- Incorporar procedimientos almacenados y funciones.
- Implementar al menos tres reportes.
- Incorporar respaldo y restauración de la base de datos.
- Agregar pruebas unitarias y de integración.
- Actualizar la ERS, los casos de uso y el DER.
- Elaborar los manuales de instalación y usuario.

---

## Alcance académico

Este repositorio contiene una versión académica en desarrollo.

Algunos módulos pueden disponer de una pantalla navegable antes de tener implementadas todas sus reglas de negocio, operaciones de persistencia y validaciones. El estado real del proyecto debe interpretarse según las secciones **Funcionalidades implementadas** y **Funcionalidades en desarrollo**.

---

## Autores

Proyecto desarrollado por estudiantes de la **Licenciatura en Sistemas de Información de la Universidad Nacional del Nordeste**, para la asignatura **Taller de Programación II**.
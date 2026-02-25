
# Guía rápida: configurar BD local y aplicar migraciones (EF Core + SQL Server)

## 1. Objetivo

Dejar **lista la base de datos local** para desarrollar:

- Conectarse a SQL Server local (`.\\SQLEXPRESS` o `(localdb)\\MSSQLLocalDB`).
- Configurar la **cadena de conexión** en `appsettings.json`.
- Ejecutar las **migraciones de Entity Framework Core**.
- Evitar errores típicos (certificado SSL, pegar JSON en la consola, etc.).

---

## 2. Configurar conexión en SQL Server Management Studio (SSMS)

1. Abrir **SSMS**.
2. En la ventana de conexión:
   - **Servidor**:  
     - Para instancia Express: `.\\SQLEXPRESS`  
     - Para LocalDB: `(localdb)\\MSSQLLocalDB`
   - **Autenticación**: `Autenticación de Windows`.
3. Pulsar el botón **Opciones >>**.
4. Ir a la pestaña **Propiedades de conexión**:
   - En **Cifrar** → seleccionar:
     - **“No”**  
       **o** marcar **“Confiar en el certificado de servidor”**.
5. Aceptar y conectar.

> 💡 Si aparece el error “La cadena de certificación fue emitida por una entidad en la que no se confía”, es normal en local. La solución es justamente marcar **“Confiar en el certificado de servidor”** o usar `TrustServerCertificate=True` en la cadena de conexión.

---

## 3. Configurar `appsettings.json`

En el proyecto de inicio (ej: `Taller.Presentacion`), abrir `appsettings.json` y definir:

```json
{
  "ConnectionStrings": {
    "Default": "Server=.\\SQLEXPRESS;Database=SGTM_DB;Trusted_Connection=True;TrustServerCertificate=True"
  },

  "Auth": {
    "MaxLoginAttempts": 5
  },

  "Serilog": {
    "Using": [ "Serilog.Sinks.File", "Serilog.Sinks.Debug" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "Enrich": [ "FromLogContext" ],
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "logs/app-.log",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 7,
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:l}{NewLine}{Exception}"
        }
      },
      {
        "Name": "Debug",
        "Args": {
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:l}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

Puntos clave:

- `Server=.\\SQLEXPRESS` → instancia local de SQL Server Express.
- `Database=SGTM_DB` → nombre de la BD (se creará al correr las migraciones).
- `Trusted_Connection=True` → usa las credenciales de Windows.
- `TrustServerCertificate=True` → acepta el certificado auto-firmado en local.

---

## 4. Aplicar migraciones con **Package Manager Console**

> Método recomendado si se trabaja dentro de Visual Studio.

1. En Visual Studio:
   - Menú **Tools > NuGet Package Manager > Package Manager Console**.
2. En la consola:
   - En **Default project** seleccionar: `Taller.Infraestructura` (o el proyecto de infraestructura).
3. Asegurarse de que el **proyecto de inicio** (Startup Project) sea `Taller.Presentacion`.
4. Ejecutar:

```powershell
Update-Database
```

Esto:

- Usa la cadena de conexión de `appsettings.json`.
- Crea la BD `SGTM_DB` si no existe.
- Aplica todas las migraciones pendientes (crea tablas, etc.).

---

## 5. Aplicar migraciones con **dotnet ef** (CLI)

> Útil si se trabaja desde Git Bash / PowerShell sin Package Manager Console.

1. Abrir consola en la carpeta raíz del repo (donde está el `.sln`):

2. Ejecutar:

```powershell
dotnet ef database update `
  --project Taller/Taller.Infraestructura `
  --startup-project Taller/Taller.Presentacion
```

- `--project` → proyecto donde están las migraciones (Infraestructura).
- `--startup-project` → proyecto que tiene `Program.cs` y `appsettings.json` (Presentación).

---

## 6. Verificar la base de datos

En SSMS:

1. Conectarse al servidor (`.\\SQLEXPRESS`).
2. Expandir **Bases de datos**.
3. Verificar que exista **`SGTM_DB`** con sus tablas (ej. `dbo.Clientes`, `dbo.Ordenes`, etc.).

---

## 7. Errores frecuentes y soluciones

### 7.1. Error de certificado SSL en SSMS

> *“La cadena de certificación fue emitida por una entidad en la que no se confía…”*

**Causa:** el servidor usa un certificado auto-firmado (típico en local).

**Solución:**

- En SSMS: marcar **“Confiar en el certificado de servidor”** o desactivar cifrado.
- En la app: incluir `TrustServerCertificate=True` en la cadena de conexión.

---

### 7.2. Error en consola: `token ':' inesperado en la expresión o la instrucción`

**Causa más común:** pegar texto JSON de `appsettings.json` directamente en la consola de PowerShell / Git Bash.  
La consola intenta interpretarlo como comandos y falla.

**Solución:**

- El JSON va **solo** en `appsettings.json`.
- En la consola **solo** se ejecutan comandos, por ejemplo:

  ```powershell
  Update-Database        # Package Manager Console
  ```

  o

  ```powershell
  dotnet ef database update --project ... --startup-project ...
  ```

---

### 7.3. `Cannot open database "SGTM_DB" requested by the login`

**Causa:** la BD aún no existe / no se aplicaron migraciones.

**Solución:**

- Ejecutar `Update-Database` (o `dotnet ef database update`) como se indica arriba.

---

## 8. Resumen ultra corto

1. **Conectar SSMS** a `.\\SQLEXPRESS` y marcar **“Confiar en el certificado de servidor”**.
2. **Configurar `appsettings.json`** con:

   ```json
   "ConnectionStrings": {
     "Default": "Server=.\\SQLEXPRESS;Database=SGTM_DB;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

3. En Visual Studio → **Package Manager Console**:

   ```powershell
   Update-Database
   ```

4. Verificar en SSMS que `SGTM_DB` fue creada y tiene las tablas.

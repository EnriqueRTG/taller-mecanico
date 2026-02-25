# Guía de Flujo de Trabajo con Git para el Proyecto de Taller Mecánico

## 1. Objetivo del documento

Este documento define **cómo vamos a trabajar con Git** en el proyecto de Sistema de Gestión de Taller Mecánico.

Busca:

- Mantener el proyecto **estable**, **seguro** y **trazable**.  
- Facilitar el trabajo desde **varios dispositivos** (PC de escritorio, notebook, etc.).  
- Permitir colaborar en equipo de forma ordenada.

La idea es que cualquier persona del grupo pueda leer esto y saber **qué hacer en cada situación**.

---

## 2. Conceptos básicos y comandos clave

> Estos son los comandos principales que se usan en los ejemplos.  
> *Solo se explican la primera vez*.

- `git clone URL`  
  Clona un repositorio remoto (GitHub) en tu máquina.

- `git status`  
  Muestra el estado actual: rama, archivos modificados, listos para commit, etc.

- `git fetch origin`  
  Trae al local la información más nueva del remoto (nuevos commits, ramas), **sin mezclarla aún**.

- `git checkout rama`  
  Cambia la rama actual.  
  Con `git checkout -b rama-nueva` crea una nueva rama y se cambia a ella.

- `git pull --ff-only`  
  Trae los cambios del remoto y los aplica sobre tu rama, **solo si se puede avanzar sin merge raro**.  
  Es la forma más segura de evitar merges confusos.

- `git add .` / `git add -A`  
  Marca archivos modificados para incluirlos en el próximo commit.

- `git commit -m "mensaje"`  
  Crea un commit con los cambios marcados (versión “fotografiada” del código).

- `git push`  
  Sube los commits de tu rama local al repositorio remoto (GitHub).

---

## 3. Convenciones del proyecto

### 3.1. Ramas principales

- `main`  
  - Contiene **solo código estable**: compila y funciona.  
  - Es la rama desde la que parte cualquier nueva funcionalidad.  
  - Nunca se desarrolla directamente en `main`, siempre se usan ramas de feature.

### 3.2. Ramas de trabajo

Se utilizan ramas específicas para cada tarea:

- **Feature (nuevas funcionalidades):**

  ```text
  feature/refactor-arquitectura-taller
  feature/clientes-mejoras-ux
  feature/vehiculos-crud
  ```

- **Hotfix (correcciones urgentes sobre producción / main):**

  ```text
  hotfix/bug-clientes-paginacion
  ```

- **Chore / docs (tareas de mantenimiento, documentación):**

  ```text
  chore/actualizar-readme
  docs/guia-flujo-git
  ```

**Regla:** una rama = una tarea/feature clara.

### 3.3. Mensajes de commit

Formato recomendado:

```text
tipo: descripción corta en presente
```

Tipos comunes:

- `feat:` → nueva funcionalidad  
- `fix:` → corrección de bug  
- `refactor:` → cambios internos sin modificar funcionalidad visible  
- `chore:` → mantenimiento, configuración, scripts, etc.  
- `docs:` → documentación

Ejemplos:

```text
feat: mejora UX en CRUD de clientes (seleccion y recarga)
fix: corrige validacion de email en alta de cliente
refactor: renombra proyectos y agrega capa Aplicacion
```

---

## 4. Flujo estándar para una nueva funcionalidad

Ejemplo: implementar CRUD de vehículos.

### Paso 1 – Partir de `main` actualizado

En la máquina donde vayas a empezar:

```bash
git fetch origin
git checkout main
git pull --ff-only
```

- `fetch` trae la información del remoto.  
- `checkout main` te ubica en la rama estable.  
- `pull --ff-only` asegura que tu `main` local está al día con GitHub.

### Paso 2 – Crear rama de feature

```bash
git checkout -b feature/vehiculos-crud
```

Esta rama será el “laboratorio” de esa funcionalidad.

### Paso 3 – Trabajar iterativamente

Ciclo normal de desarrollo:

```bash
# ver cambios actuales
git status

# marcar cambios para commit
git add .

# crear commit con mensaje descriptivo
git commit -m "feat: agrega entidad Vehiculo y DbSet"

git add .
git commit -m "feat: agrega formulario FrmVehiculos y listado inicial"

git add .
git commit -m "feat: completa CRUD de vehiculos con validaciones"
```

### Paso 4 – Subir trabajo al remoto regularmente

```bash
git push -u origin feature/vehiculos-crud
```

Luego, basta con:

```bash
git push
```

**Buena práctica:** hacer push frecuentemente.  
Así el código queda a salvo si se rompe algo en la máquina local.

---

## 5. Trabajar desde dos dispositivos (desktop + notebook)

Regla de oro:

> **Nunca empieces a trabajar en un dispositivo sin antes hacer `pull`  
> y nunca dejes un dispositivo sin antes hacer `push`.**

### 5.1. Antes de trabajar en un dispositivo

1. Abrir terminal en la carpeta del repo.
2. Asegurarse de que no hay cambios sin commitear:

   ```bash
   git status
   ```

3. Traer lo último de la rama de trabajo:

   ```bash
   git fetch origin
   git checkout feature/vehiculos-crud   # o la rama que corresponda
   git pull --ff-only
   ```

Ahora esa máquina tiene la misma versión que la otra.

### 5.2. Después de trabajar en un dispositivo

1. Ver estado y agregar cambios:

   ```bash
   git status
   git add -A
   ```

2. Crear commit:

   ```bash
   git commit -m "feat: mejora validacion de vehiculos"
   ```

3. Subir al remoto:

   ```bash
   git push
   ```

Cuando pases al otro dispositivo, repetirás la sección 5.1.

---

## 6. Finalizar una feature e integrarla a `main`

Una rama de feature está lista para integrarse cuando:

- Compila sin errores.  
- La funcionalidad hace lo que debe.  
- No rompe funcionalidades existentes (al menos lo probado).

### 6.1. Crear Pull Request (PR)

Desde GitHub:

1. Ir al repo → pestaña **Pull requests** → **New pull request**.
2. Elegir:
   - **base**: `main`
   - **compare**: `feature/vehiculos-crud` (ejemplo)
3. Revisar el diff:
   - Qué archivos se agregan, modifican o eliminan.
   - Comentarios o dudas.

Si estuvieras trabajando con otra persona, esa persona revisaría el PR.  
Aunque estés solo, el PR te sirve como **documentación y revisión final**.

### 6.2. Hacer el merge a `main`

Cuando el PR está aprobado:

- Usar el botón **Merge** en GitHub (merge commit o squash, según preferencia).
- La rama `main` en remoto ahora incluye toda la feature.

### 6.3. Actualizar `main` en cada dispositivo

En cada máquina:

```bash
git checkout main
git pull --ff-only
```

### 6.4. Limpiar ramas de feature

Opcional pero recomendado para mantener orden:

- En remoto (GitHub lo ofrece al cerrar el PR) o manualmente:

  ```bash
  git push origin --delete feature/vehiculos-crud
  ```

- En local:

  ```bash
  git branch -d feature/vehiculos-crud
  ```

---

## 7. Modificar una funcionalidad ya integrada

Ejemplo: mejorar nuevamente el CRUD de clientes más adelante.

**No se reabre la rama vieja.**  
Se crea una **nueva rama desde `main`**:

```bash
git fetch origin
git checkout main
git pull --ff-only

git checkout -b feature/clientes-mejoras-ux-2
```

Se trabaja normalmente:

```bash
git add .
git commit -m "feat: agrega mensajes en status bar para clientes"
git push
```

Y se hace un nuevo PR → `main`.

Ventajas:

- Cada ola de cambios queda registrada en su rama y PR.  
- Es fácil ver el historial de mejoras por funcionalidad.

---

## 8. Manejo básico de errores frecuentes

### 8.1. Intento commitear y Git pide nombre/email

Si aparece:

```text
Please tell me who you are.
```

Configurar identidad en esa máquina:

```bash
git config --global user.name "EnriqueRTG-Desktop"
git config --global user.email "enriqueramontg@gmail.com"
```

> Mismo email en todas las máquinas para que GitHub lo reconozca como el mismo usuario;  
> nombres distintos si se quiere distinguir `Desktop` / `Notebook`.

### 8.2. Hay cambios locales y quiero descartar todo

**Cuidado:** esto borra cambios sin commitear.

```bash
git restore .
```

Si además querés borrar archivos nuevos sin trackear:

```bash
git clean -fd
```

Solo usar si esas modificaciones realmente no se quieren guardar.

---

## 9. Buenas prácticas generales

- **Commits pequeños y frecuentes.**  
  Evitar “mega commits” que mezclan muchas cosas distintas.

- **Un commit debe representar una unidad lógica.**  
  Ejemplo: “agrega validación de email”, no “mil cosas a la vez”.

- **Nunca trabajar directo en `main`.**  
  Siempre usar ramas de feature/hotfix.

- **Siempre hacer `pull` antes de empezar a tocar código, y `push` al terminar** en cada dispositivo.

- **No commitear archivos sensibles o específicos de máquina**:
  - cadenas de conexión con credenciales,
  - archivos de configuración local (`*.local.json`),
  - archivos generados por Visual Studio (`bin/`, `obj/`, etc.).

  Asegurarse de que `.gitignore` esté configurado correctamente.

- **Nombrar ramas de forma clara y corta.**  
  Facilita leer historial y entender qué se hizo dónde.

---

## 10. Checklist rápido (modo “chuleta”)

### Nueva funcionalidad

```bash
git fetch origin
git checkout main
git pull --ff-only
git checkout -b feature/nombre-claro
# ... trabajar, hacer commits ...
git push    # subir cambios
```

### Cambiar de dispositivo

```bash
git fetch origin
git checkout feature/nombre-claro
git pull --ff-only
# ... trabajar ...
git add -A
git commit -m "feat: descripcion"
git push
```

### Terminar la feature

1. Probar la funcionalidad.
2. Crear PR `feature/nombre-claro` → `main`.
3. Merge en GitHub.
4. En cada máquina:

   ```bash
   git checkout main
   git pull --ff-only
   git branch -d feature/nombre-claro        # opcional
   git push origin --delete feature/nombre-claro  # opcional
   ```

---

Este documento puede guardarse, por ejemplo, como:

- `docs/flujo-trabajo-git.md`  
- O imprimirse / compartirse en PDF para el equipo.

Si más adelante definimos reglas adicionales (por ejemplo, linters, tests obligatorios antes de merge, etc.), se pueden agregar nuevas secciones siguiendo esta misma estructura.

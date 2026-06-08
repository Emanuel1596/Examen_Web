# AppClean - Actividad SPA Frontend + API Backend

Proyecto base para la actividad de equipo donde se trabajará con una API backend en .NET y un frontend tipo SPA con HTML/JavaScript, Angular o la tecnología indicada por el profesor.

El proyecto está preparado para trabajar con GitHub Codespaces, de forma que cada integrante tenga su propio entorno sin instalar herramientas localmente.

---

## Integrantes

| Nombre completo | Usuario de GitHub |
|---|---|
-
-
-
-
| Villanueva Garcia Emanuel | [@Emanuel1596](https://github.com/Emanuel1596) |

---

## Objetivo del proyecto

El objetivo es usar una plantilla AppClean con backend funcional para después conectarla con un frontend SPA.

El backend ya incluye:

- API en .NET
- Clean Architecture
- Autenticación con JWT
- Roles de usuario
- SQLite
- Endpoints protegidos
- Pruebas iniciales con script y colección Bruno

---

## Tecnologías usadas

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT
- GitHub Codespaces
- Bruno
- HTML, JavaScript o Angular para el frontend

---

## Estructura del proyecto

```text
MiApp.API
MiApp.Application
MiApp.Domain
MiApp.Infrastructure
bruno
scripts
.devcontainer
MiApp.slnx
```

---

## Descripción de carpetas

### MiApp.API

Contiene la API, los controladores, la configuración del proyecto y el archivo `Program.cs`.

Aquí están rutas como:

```text
/api/auth/login
/api/admin/users
/api/admin/dashboard
/api/reader/content
/api/reader/profile
```

### MiApp.Application

Contiene la lógica de aplicación, como los casos de uso y comandos.

### MiApp.Domain

Contiene las entidades principales, interfaces y roles del sistema.

### MiApp.Infrastructure

Contiene la conexión con SQLite, repositorios, servicios de JWT y hasheo de contraseñas.

### bruno

Contiene la colección para probar la API con Bruno.

### scripts

Contiene scripts para confirmar que la API funciona correctamente.

### .devcontainer

Contiene la configuración para que cada integrante pueda crear su propio Codespace con el entorno preparado.

---

## Trabajo con GitHub Codespaces

Cada integrante debe crear su propio Codespace desde el repositorio.

Ruta en GitHub:

```text
Code > Codespaces > Create codespace on main
```

No se debe configurar cada Codespace a mano. Las herramientas necesarias deben venir desde:

```text
.devcontainer/devcontainer.json
```

Cada integrante tendrá su propio entorno, su propia terminal y su propia base SQLite local.

---

## Reglas del equipo

- No trabajar directo en `main`.
- Cada integrante debe crear su propia rama.
- No subir `bin`, `obj`, `node_modules` ni bases SQLite locales.
- No subir archivos `.zip`.
- Primero se confirma que la API funciona.
- Después se trabaja el frontend.
- Todo cambio importante debe ir en commit.
- Cada rama debe subirse con `push`.
- La integración debe hacerse mediante Pull Request.

---

## Crear una rama de trabajo

Cada integrante debe crear su rama con este formato:

```bash
git checkout -b <nombre-integrante>/<tarea>
```

Ejemplo:

```bash
git checkout -b <nombre-integrante>/frontend-login
```

---

## Guardar cambios

Para revisar cambios:

```bash
git status
```

Para agregar archivos:

```bash
git add .
```

Para crear commit:

```bash
git commit -m "Agrega avance de <tarea>"
```

Para subir la rama:

```bash
git push -u origin <nombre-integrante>/<tarea>
```

---

## Compilar el proyecto

Desde la raíz del proyecto:

```bash
dotnet build MiApp.slnx
```

Resultado esperado:

```text
Build succeeded.
```

---

## Correr la API

Desde la raíz del proyecto:

```bash
dotnet run --project MiApp.API/MiApp.API.csproj
```

La API debe iniciar en:

```text
http://localhost:5223
```

Resultado esperado:

```text
Now listening on: http://localhost:5223
```

La terminal donde corre la API debe quedarse abierta.

---

## Confirmar que la API funciona

Con la API corriendo, abrir otra terminal y ejecutar:

```bash
./scripts/check-api.sh
```

Resultado esperado:

```text
API confirmada correctamente
```

---

## Qué valida el script de confirmación

El script revisa que:

- El login de Admin funcione.
- El login de Reader funcione.
- Un login incorrecto responda `401`.
- Admin pueda entrar a rutas Admin.
- Reader pueda entrar a rutas Reader.
- Reader no pueda entrar a rutas Admin.

---

## Usuarios iniciales

### Usuario Admin

```text
Correo: admin@miapp.com
Contraseña: Admin123!
Rol: Admin
```

### Usuario Reader

```text
Correo: reader@miapp.com
Contraseña: Reader123!
Rol: Reader
```

---

## Endpoints principales

### Auth

```text
POST /api/auth/login
```

### Admin

```text
GET /api/admin/users
GET /api/admin/dashboard
```

### Reader

```text
GET /api/reader/content
GET /api/reader/profile
```

---

## Códigos HTTP importantes

```text
200 = funcionó correctamente
401 = credenciales incorrectas o falta autenticación
403 = usuario autenticado, pero sin permiso
500 = error interno del servidor
```

---

## Frontend

El frontend se agregará según lo indique el profesor.

Puede ser:

- HTML + JavaScript
- SPA con JavaScript
- Angular

Puertos preparados:

```text
5223 = API Backend
3000 = Frontend JS
3001 = Frontend alternativo
4200 = Angular
```

En Codespaces, los puertos deben abrirse desde la pestaña:

```text
PUERTOS
```

---

## Estado esperado antes de trabajar el frontend

Antes de empezar el frontend, se debe confirmar esto:

```text
Build succeeded.
Now listening on: http://localhost:5223
API confirmada correctamente
```

---

## Comandos rápidos

Compilar:

```bash
dotnet build MiApp.slnx
```

Correr API:

```bash
dotnet run --project MiApp.API/MiApp.API.csproj
```

Probar API:

```bash
./scripts/check-api.sh
```

Ver cambios:

```bash
git status
```

Guardar cambios:

```bash
git add .
git commit -m "Agrega avance de <tarea>"
git push
```

---

## Nota importante

Este proyecto está pensado para que la configuración viva en el repositorio. Si un integrante crea un Codespace nuevo desde este repositorio, debe poder compilar, correr la API y ejecutar las pruebas iniciales sin instalar herramientas manualmente.

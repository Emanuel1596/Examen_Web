# AppClean - Actividad SPA Frontend + API Backend

Proyecto base para la actividad de equipo donde se trabajará con una API backend en .NET y un frontend tipo SPA con HTML/JavaScript, Angular o la tecnología indicada por el profesor.

El proyecto está preparado para trabajar con GitHub Codespaces, de forma que cada integrante tenga su propio entorno sin instalar herramientas localmente.

---

## Integrantes

| Nombre completo                | Usuario de GitHub                                              |
| ------------------------------ | -------------------------------------------------------------- |
| Contreras Gonzalez Andre Yahir | [@XxTiny2099xX](https://github.com/XxTiny2099xX)               |
| Huerta Ruiz Diego Rafael       | [@o0rafahuerta0o](https://github.com/o0rafahuerta0o)           |
| Martinez Mejia Edgar           | [@martinedgar711-lgtm](https://github.com/martinedgar711-lgtm) |
| Romero Palacios Randy          | [@RandyPalacios02](https://github.com/RandyPalacios02)         |
| Villanueva Garcia Emanuel      | [@Emanuel1596](https://github.com/Emanuel1596)                 |

---

## Objetivo del proyecto

El objetivo es usar una plantilla AppClean con backend funcional para después conectarla con un frontend SPA.

El sistema desarrollado permite administrar eventos y vender boletos para conciertos, conferencias y espectáculos.

El backend incluye:

* API en .NET
* Clean Architecture
* Autenticación con JWT
* Roles de usuario
* SQLite
* Endpoints protegidos
* Pruebas iniciales con script y colección Bruno

---

## Tecnologías usadas

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* JWT
* GitHub Codespaces
* Bruno
* HTML, CSS y JavaScript para el frontend

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
frontend
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
/api/events
/api/events/active
/api/tickets/purchase
/api/dashboard/sales
```

### MiApp.Application

Contiene la lógica de aplicación, como los casos de uso, comandos, consultas y validaciones.

### MiApp.Domain

Contiene las entidades principales, interfaces, roles y enums del sistema.

### MiApp.Infrastructure

Contiene la conexión con SQLite, repositorios, servicios de JWT, hasheo de contraseñas y datos iniciales del sistema.

### bruno

Contiene la colección para probar la API con Bruno.

### scripts

Contiene scripts para confirmar que la API funciona correctamente.

### .devcontainer

Contiene la configuración para que cada integrante pueda crear su propio Codespace con el entorno preparado.

### frontend

Contiene el frontend SPA realizado con HTML, CSS y JavaScript.

---

## Trabajo con GitHub Codespaces

Cada integrante puede crear su propio Codespace desde el repositorio.

Ruta en GitHub:

```text
Code > Codespaces > Create codespace on main
```

No se necesita instalar .NET, Node.js ni Visual Studio Code localmente, porque el entorno se prepara desde:

```text
.devcontainer/devcontainer.json
```

Cada Codespace tendrá su propio entorno, su propia terminal y su propia base SQLite local.

---

## Datos iniciales en Codespaces nuevos

Cada Codespace tiene su propia base de datos SQLite local.

Por eso, los datos creados en un Codespace no aparecen automáticamente en otro Codespace.

Para que el proyecto tenga información de prueba al abrirse en un Codespace nuevo, el sistema usa `DbSeeder`.

Al correr la API por primera vez en un Codespace nuevo, se crean automáticamente:

* Usuario Admin
* Usuario Reader
* Eventos demo
* Zonas VIP, Preferente y General
* Una compra demo para mostrar información en el dashboard

No se debe subir el archivo `miapp.db` al repositorio, porque es una base local generada por cada Codespace.

---

## Cómo ejecutar el proyecto en Codespaces

Abrir el repositorio en GitHub Codespaces:

```text
Code > Codespaces > Create codespace on main
```

Esperar a que termine de preparar el entorno.

---

## Correr la API

Desde la raíz del proyecto, abrir una terminal y ejecutar:

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

## Correr el frontend

Abrir una segunda terminal y ejecutar:

```bash
cd frontend
```

```bash
npx --yes serve -l 3000
```

La terminal donde corre el frontend debe quedarse abierta.

---

## Configurar puertos en Codespaces

Ir a la pestaña:

```text
PUERTOS
```

En el puerto `5223`, abrir el menú de tres puntos y cambiar la visibilidad a:

```text
Public
```

En el puerto `3000`, abrir el menú de tres puntos y cambiar la visibilidad a:

```text
Public
```

Después abrir el puerto `3000` en el navegador.

---

## Datos para iniciar sesión como administrador

```text
Correo: admin@miapp.com
Contraseña: Admin123!
Rol: Admin
```

---

## Usuario Reader inicial

```text
Correo: reader@miapp.com
Contraseña: Reader123!
Rol: Reader
```

---

## Qué probar en el navegador

### Portal Público

* Ver eventos disponibles.
* Buscar eventos.
* Filtrar eventos por lugar y fecha.
* Seleccionar zona de boleto.
* Seleccionar cantidad.
* Confirmar que el total se calcula correctamente.
* Comprar boletos.
* Confirmar que aparece el aviso de compra completada.

### Administración

* Iniciar sesión como administrador.
* Crear evento.
* Editar evento.
* Cancelar evento.
* Filtrar eventos por nombre, lugar, estado y fecha.
* Ver dashboard de ventas.

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

* El login de Admin funcione.
* El login de Reader funcione.
* Un login incorrecto responda `401`.
* Admin pueda entrar a rutas Admin.
* Reader pueda entrar a rutas Reader.
* Reader no pueda entrar a rutas Admin.

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

### Eventos

```text
GET /api/events
GET /api/events/active
GET /api/events/{id}
POST /api/events
PUT /api/events/{id}
PATCH /api/events/{id}/cancel
```

### Boletos

```text
POST /api/tickets/purchase
```

### Dashboard

```text
GET /api/dashboard/sales
```

---

## Códigos HTTP importantes

```text
200 = funcionó correctamente
201 = recurso creado correctamente
400 = datos inválidos
401 = credenciales incorrectas o falta autenticación
403 = usuario autenticado, pero sin permiso
404 = recurso no encontrado
500 = error interno del servidor
```

---

## Funcionalidades principales

El proyecto permite:

* Crear eventos.
* Editar eventos.
* Cancelar eventos.
* Consultar eventos.
* Consultar eventos activos.
* Configurar zonas VIP, Preferente y General.
* Asignar precios por zona.
* Comprar boletos.
* Calcular total de compra.
* Mostrar dashboard de ventas.
* Proteger la administración con JWT.

---

## Extras implementados

El proyecto incluye:

* Validaciones en backend.
* Validaciones en frontend.
* Búsqueda de eventos.
* Filtros por lugar.
* Filtros por fecha.
* Filtros por estado en administración.
* Dashboard de ventas.
* Autenticación JWT.
* Protección de controladores administrativos.
* Aviso visual de compra completada.

---

## Puertos usados

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

## Apagar el proyecto

En la terminal donde corre la API presionar:

```text
Ctrl + C
```

En la terminal donde corre el frontend presionar:

```text
Ctrl + C
```

---

## Comandos rápidos

Correr API:

```bash
dotnet run --project MiApp.API/MiApp.API.csproj
```

Correr frontend:

```bash
cd frontend
npx --yes serve -l 3000
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
git commit -m "Actualiza proyecto"
git push
```

---

## Nota importante

Este proyecto está pensado para que la configuración viva en el repositorio. Si un integrante crea un Codespace nuevo desde este repositorio, debe poder correr la API, ejecutar el frontend y probar el sistema sin instalar herramientas manualmente.

Cada Codespace genera su propia base SQLite local. Los datos demo se crean automáticamente mediante el seeder al iniciar la API por primera vez.

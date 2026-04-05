# 🏘️ G03 — Sistema de Condominios

> A web-based condominium management system built with ASP.NET MVC and SQL Server.

---

## 🇺🇸 English

### About

G03 Sistema Condominios is a full-stack web application for managing residential condominium complexes. It handles property (house) registration, resident accounts, service management, and billing/charges — all through a secure, login-protected interface.

### ✨ Features

- 🔐 **Authentication** — Login system to protect all management views
- 🏠 **House Management** — Register, view, and administer condominium units
- 💰 **Billing & Charges** — Create, view, and manage resident payment records
- 🔧 **Services** — Track and manage shared condominium services
- 👤 **User Management** — Manage resident and admin user accounts

### 🛠️ Tech Stack

| Technology | Description |
|---|---|
| [ASP.NET MVC 5](https://dotnet.microsoft.com/apps/aspnet/mvc) | Web framework (C#, .NET 4.7.2) |
| [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) | Primary programming language |
| [SQL Server](https://www.microsoft.com/en-us/sql-server) | Relational database |
| [linq2db](https://linq2db.github.io/) | ORM / database access layer |
| [Bootstrap 5](https://getbootstrap.com/) | Responsive UI framework |
| [jQuery 3.7](https://jquery.com/) | Client-side scripting |
| [Font Awesome 4.7](https://fontawesome.com/) | Icon library |
| [Newtonsoft.Json](https://www.newtonsoft.com/json) | JSON serialization |
| [Razor Views](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor) | Server-side HTML templating |

### 📁 Project Structure

```
G03-Sistema-Condominios/
├── App_Start/
│   ├── BundleConfig.cs       # CSS/JS bundles
│   ├── FilterConfig.cs       # Global MVC filters
│   └── RouteConfig.cs        # URL routing
├── Controllers/
│   ├── HomeController.cs
│   ├── LoginController.cs
│   ├── CasasController.cs    # House management
│   ├── CobroController.cs    # Billing management
│   └── ServiciosController.cs # Services management
├── Models/
│   ├── ModelCasa.cs
│   ├── ModelCobro.cs
│   ├── ModelServicio.cs
│   ├── ModelUsuario.cs
│   └── ModelLogin.cs
├── Views/
│   ├── Shared/               # Layout & shared partials
│   ├── Home/
│   ├── Login/
│   ├── Casas/
│   ├── Cobro/
│   └── Servicios/
├── Content/                  # CSS files
├── Scripts/                  # JavaScript files
├── Web.config
└── Global.asax
```

### ⚙️ Prerequisites

- **Visual Studio 2019 or later** (with ASP.NET and web development workload)
- **.NET Framework 4.7.2**
- **SQL Server** (local or remote instance)
- **IIS Express** (included with Visual Studio)

### 🚀 Getting Started

```bash
# 1. Clone the repository
git clone https://github.com/LUHECU/G03-Sistema-Condominios.git

# 2. Open the solution in Visual Studio
# Open: G03 Sistema Condominios.sln

# 3. Restore NuGet packages
# In Visual Studio: Tools > NuGet Package Manager > Restore packages
# Or right-click the solution > Restore NuGet Packages

# 4. Configure the database connection
# Edit Web.config and update the connection string with your SQL Server details

# 5. Run the project
# Press F5 or click the green "IIS Express" button in Visual Studio
```

The application will launch at `https://localhost:44390/`.

### 🗄️ Database Configuration

Open `Web.config` and update the connection string to point to your SQL Server instance:

```xml
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Server=YOUR_SERVER;Database=G03_Condominios;Trusted_Connection=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

---

## 🇪🇸 Español

### Acerca del proyecto

G03 Sistema Condominios es una aplicación web completa para la gestión de complejos residenciales. Permite administrar propiedades (casas), usuarios residentes, servicios compartidos y cobros, todo a través de una interfaz protegida por inicio de sesión.

### ✨ Funcionalidades

- 🔐 **Autenticación** — Sistema de login para proteger todas las vistas de gestión
- 🏠 **Gestión de Casas** — Registrar, visualizar y administrar unidades del condominio
- 💰 **Cobros** — Crear, consultar y gestionar registros de pago de residentes
- 🔧 **Servicios** — Administrar los servicios compartidos del condominio
- 👤 **Usuarios** — Gestión de cuentas de residentes y administradores

### 🛠️ Stack Tecnológico

| Tecnología | Descripción |
|---|---|
| [ASP.NET MVC 5](https://dotnet.microsoft.com/apps/aspnet/mvc) | Framework web (C#, .NET 4.7.2) |
| [C#](https://learn.microsoft.com/es-es/dotnet/csharp/) | Lenguaje de programación principal |
| [SQL Server](https://www.microsoft.com/es-es/sql-server) | Base de datos relacional |
| [linq2db](https://linq2db.github.io/) | ORM / capa de acceso a datos |
| [Bootstrap 5](https://getbootstrap.com/) | Framework de UI responsivo |
| [jQuery 3.7](https://jquery.com/) | Scripting del lado cliente |
| [Font Awesome 4.7](https://fontawesome.com/) | Librería de iconos |
| [Newtonsoft.Json](https://www.newtonsoft.com/json) | Serialización JSON |
| [Razor Views](https://learn.microsoft.com/es-es/aspnet/core/mvc/views/razor) | Plantillas HTML del lado servidor |

### 📁 Estructura del Proyecto

```
G03-Sistema-Condominios/
├── App_Start/
│   ├── BundleConfig.cs       # Bundles de CSS/JS
│   ├── FilterConfig.cs       # Filtros globales de MVC
│   └── RouteConfig.cs        # Configuración de rutas URL
├── Controllers/
│   ├── HomeController.cs
│   ├── LoginController.cs
│   ├── CasasController.cs    # Gestión de casas
│   ├── CobroController.cs    # Gestión de cobros
│   └── ServiciosController.cs # Gestión de servicios
├── Models/
│   ├── ModelCasa.cs
│   ├── ModelCobro.cs
│   ├── ModelServicio.cs
│   ├── ModelUsuario.cs
│   └── ModelLogin.cs
├── Views/
│   ├── Shared/               # Layout y partiales compartidos
│   ├── Home/
│   ├── Login/
│   ├── Casas/
│   ├── Cobro/
│   └── Servicios/
├── Content/                  # Archivos CSS
├── Scripts/                  # Archivos JavaScript
├── Web.config
└── Global.asax
```

### ⚙️ Requisitos Previos

- **Visual Studio 2019 o superior** (con carga de trabajo de ASP.NET y desarrollo web)
- **.NET Framework 4.7.2**
- **SQL Server** (instancia local o remota)
- **IIS Express** (incluido con Visual Studio)

### 🚀 Primeros Pasos

```bash
# 1. Clonar el repositorio
git clone https://github.com/LUHECU/G03-Sistema-Condominios.git

# 2. Abrir la solución en Visual Studio
# Abrir: G03 Sistema Condominios.sln

# 3. Restaurar paquetes NuGet
# En Visual Studio: Herramientas > Administrador de paquetes NuGet > Restaurar
# O clic derecho en la solución > Restaurar paquetes NuGet

# 4. Configurar la conexión a la base de datos
# Editar Web.config y actualizar el connection string con los datos de SQL Server

# 5. Ejecutar el proyecto
# Presionar F5 o el botón verde "IIS Express" en Visual Studio
```

La aplicación se ejecutará en `https://localhost:44390/`.

### 🗄️ Configuración de la Base de Datos

Abrir `Web.config` y actualizar el connection string con los datos de tu instancia de SQL Server:

```xml
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Server=TU_SERVIDOR;Database=G03_Condominios;Trusted_Connection=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

---

<div align="center">
  <sub>Built with ❤️ by <a href="https://github.com/LUHECU">LUHECU</a></sub>
</div>

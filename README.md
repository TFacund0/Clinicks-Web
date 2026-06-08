<div align="center">
  <h1 align="center">Clinicks</h1>
  <p align="center">
    <strong>Sistema Integrado de Gestión Hospitalaria y Expediente Clínico Electrónico (EHR)</strong>
    <br />
    <br />
    <a href="#sobre-el-proyecto"><strong>Explorar la documentación »</strong></a>
    <br />
  </p>
</div>

<div align="center">
  
  [![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
  [![React](https://img.shields.io/badge/React-18-61DAFB?style=for-the-badge&logo=react&logoColor=black)](https://reactjs.org/)
  [![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14+-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
  [![TailwindCSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)](https://tailwindcss.com/)
  [![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](#licencia)

</div>

<br />

## Sobre el Proyecto

**Clinicks** es una solución tecnológica diseñada para digitalizar, organizar y optimizar las operaciones clínicas y administrativas diarias en centros de salud modernos. A través de una plataforma centralizada y altamente intuitiva, los profesionales médicos pueden gestionar su agenda de turnos, registrar atenciones, documentar procedimientos y acceder al historial clínico electrónico completo de sus pacientes en tiempo real.

El proyecto ha sido desarrollado bajo un fuerte compromiso con las mejores prácticas de la Ingeniería de Software (Arquitectura N-Tier, Patrones de Diseño, Principios SOLID, SoC y DRY), garantizando así un sistema resiliente, mantenible y preparado para alta escalabilidad.

---

## Características Principales

* **Agenda Médica Inteligente**: Calendario interactivo con vistas diarias, semanales y mensuales. Flujo optimizado para reducir la sobrecarga de red con consultas filtradas temporalmente O(1).
* **Expediente Clínico Electrónico (EHR)**: Línea de tiempo unificada con el historial completo de consultas y procedimientos del paciente.
* **Búsqueda Dinámica**: Búsqueda delegada al servidor (`.Contains()` en PostgreSQL) con implementación de algoritmo *Debounce* en el frontend para evitar cuellos de botella en la API.
* **Dashboard Analítico**: Panel principal con métricas reactivas, próximos turnos y atajos para agilizar el flujo de trabajo médico.
* **Seguridad RBAC y Autenticación**: Gestión de acceso basada en roles mediante **JSON Web Tokens (JWT)** y almacenamiento seguro de contraseñas con cifrado **BCrypt**.
* **Gestión de Estados (State Pattern)**: Transiciones de turnos (Pendiente, Confirmado, Atendido, Cancelado) manejadas de forma orientada a objetos, eliminando complejidad y condicionales anidados.

---

## Arquitectura y Tecnologías

El sistema adopta una **Arquitectura Cliente-Servidor Desacoplada**, separando completamente la interfaz de usuario de la capa de acceso a datos y reglas de negocio.

### Backend (API RESTful)
Desarrollado bajo el marco **ASP.NET Core Web API (C#)** utilizando Arquitectura por Capas:
- **Framework:** .NET 8
- **ORM:** Entity Framework Core (Code-First)
- **Base de Datos:** PostgreSQL
- **Patrones:** Repository Pattern, Dependency Injection (DI), State Pattern, Factory Pattern.
- **Testing:** xUnit + Moq (Cobertura exhaustiva en la Capa de Negocio).

### Frontend (SPA)
Interfaz fluida (Single Page Application) enfocada en la experiencia del usuario (UX) médico:
- **Librería Core:** React 18 (Vite)
- **Estilos:** Tailwind CSS (Diseño Responsivo y utilitario)
- **Ruteo:** React Router DOM
- **Cliente HTTP:** Axios (Con interceptores para inyección automática de Token JWT)
- **Estado:** Custom Hooks y Context API.

---

## Manual de Usuario (Flujo Médico)

El sistema ha sido diseñado priorizando la eficiencia en la atención y reduciendo la curva de aprendizaje. A continuación, se detalla el flujo básico operativo para un usuario con rol **Médico**:

1. **Autenticación (Login)**: 
   - Accede a la pantalla principal e ingresa tus credenciales (Usuario/DNI y Contraseña).
   - El sistema validará tu rol y te otorgará acceso seguro.

2. **Dashboard Principal**: 
   - Al ingresar, visualizarás el panel de control con un resumen rápido de tu actividad.
   - Podrás ver métricas clave como "Turnos Atendidos" y accesos directos a "Nueva Consulta".

3. **Mi Agenda**: 
   - Desde el menú lateral, dirígete a `Mi Agenda`. 
   - Aquí encontrarás un listado interactivo con todos tus turnos programados del día, ordenados cronológicamente.
   - Selecciona un turno para desplegar una ventana con su detalle completo (datos filiatorios del paciente, motivo, estado).
   - Haz clic en `Iniciar Consulta` para ser redirigido automáticamente al formulario clínico.

4. **Registrar Consulta / Procedimiento**: 
   - Completa los datos médicos (Diagnóstico Presuntivo, Tratamiento, Recomendaciones, Observaciones).
   - Al confirmar y guardar, el sistema cambiará automáticamente el estado del turno a "Atendido" y la consulta quedará persistida de forma segura en la base de datos.

5. **Historial Clínico**: 
   - Desde el apartado de "Pacientes", podrás buscar rápidamente a las personas que atendiste.
   - Selecciona "Ver Historial" para acceder al expediente completo, donde verás como línea de tiempo cada evolución, consulta y práctica médica documentada, sin importar qué médico lo haya atendido previamente.

---

## Empezando (Configuración y Despliegue)

Sigue estas instrucciones detalladas para configurar el entorno de desarrollo, levantar la base de datos y correr el proyecto en tu máquina local.

### Requisitos Previos

Asegúrate de contar con el siguiente software instalado y configurado en tu variables de entorno:
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) - Para compilar y correr la API.
* [Node.js](https://nodejs.org/) (v18 o superior) - Para ejecutar el servidor de desarrollo de Vite.
* [PostgreSQL](https://www.postgresql.org/) (v14 o superior) - Motor de base de datos relacional.

### Instalación Paso a Paso

1. **Clonar el repositorio:**
   Descarga el código fuente a tu equipo local.
   ```bash
   git clone https://github.com/TFacund0/Clinicks-Web.git
   cd Clinicks-Web
   ```

2. **Configuración de la Base de Datos:**
   Abre el archivo `appsettings.Development.json` ubicado dentro de la carpeta `ClinicksApi` y actualiza la cadena de conexión con tus credenciales locales de PostgreSQL.
   ```json
   "ConnectionStrings": {
     "ClinicksDb": "Host=localhost;Database=ClinicksDb;Username=postgres;Password=tu_password"
   }
   ```

3. **Aplicar Migraciones (Backend):**
   Este paso creará automáticamente las tablas y procedimientos almacenados en tu base de datos mediante Entity Framework Core.
   ```bash
   cd ClinicksApi
   dotnet ef database update
   ```

4. **Ejecutar el Backend (API):**
   Levanta el servidor .NET.
   ```bash
   dotnet run
   ```
   *La API iniciará y escuchará peticiones típicamente en `https://localhost:7198` o `http://localhost:5198`.*

5. **Instalar Dependencias y Ejecutar el Frontend:**
   Abre una nueva terminal en paralelo (dejando la API corriendo) y ejecuta:
   ```bash
   cd ClinicksWeb
   npm install
   npm run dev
   ```
   *La aplicación web estará disponible y lista para usar en tu navegador entrando a `http://localhost:5173`.*

---

## Testing

El proyecto cuenta con una robusta suite de **Pruebas Unitarias** enfocadas en la Capa de Negocio (`Services`) utilizando `xUnit`. 

Para ejecutar los tests, sitúate en el directorio raíz o en `ClinicksApi.Tests` y ejecuta:

```bash
dotnet test
```

*Se han validado los flujos de `PacienteService`, `ConsultaService`, `ProcesoService`, `TurnoService` y las transiciones del `TurnoStateFactory`.*

---

## Estructura del Proyecto

```text
Clinicks/
├── Clinicks.sln                 # Solución Global de .NET
├── ClinicksApi/                 # Proyecto Backend (ASP.NET Core)
│   ├── Business/                # Capa de Negocio (Interfaces, DTOs, Services, States)
│   ├── Controllers/             # Capa de Aplicación (API Endpoints)
│   ├── Data/                    # Capa de Datos (Entities, Repositories, DbContext)
│   ├── Workers/                 # Background Services (BackgroundTasks)
│   └── appsettings.json         # Configuración y Connection Strings
├── ClinicksApi.Tests/           # Suite de Pruebas Unitarias (Espejo de Business/)
├── ClinicksWeb/                 # Proyecto Frontend (React)
│   ├── src/
│   │   ├── components/          # Componentes reutilizables
│   │   ├── pages/               # Vistas principales (Dashboard, Agenda, etc.)
│   │   └── hooks/               # Custom Hooks de React
└── README.md                    # Documentación
```

---

## Licencia

Distribuido bajo la Licencia MIT. Ver `LICENSE` para más información.

<p align="center">Desarrollado con pasión por el equipo de Clinicks.</p>

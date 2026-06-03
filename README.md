# Clinicks - Sistema de Gestión Médica

## Descripción General
Clinicks es un Sistema de Gestión Médica integral diseñado para digitalizar y optimizar las operaciones diarias de los centros de salud modernos. La plataforma ofrece un entorno robusto, escalable y seguro para que los profesionales médicos administren sus agendas, realicen consultas y mantengan expedientes clínicos electrónicos (EHR) de manera eficiente.

Este proyecto fue desarrollado con un fuerte enfoque en arquitectura limpia, separación de responsabilidades (SoC) y alto rendimiento, utilizando una arquitectura desacoplada cliente-servidor.

## Arquitectura y Stack Tecnológico

La aplicación se basa en una arquitectura desacoplada, lo que garantiza la escalabilidad independiente de las aplicaciones cliente y servidor.

### Backend (API RESTful)
* **Framework:** .NET 8 (C#) / ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Base de datos:** PostgreSQL
* **Seguridad:** Autenticación JWT (JSON Web Tokens), Hash de contraseñas con BCrypt y Control de Acceso Basado en Roles (RBAC).
* **Patrones de Diseño:** Patrón Repositorio (Repository Pattern), Inyección de Dependencias (Dependency Injection) y Objetos de Transferencia de Datos (DTOs).

### Frontend (Single Page Application)
* **Librería:** React 18 (Vite)
* **Estilos:** Tailwind CSS
* **Enrutamiento:** React Router DOM
* **Cliente HTTP:** Axios (configurado con Interceptores de Peticiones para Cabeceras de Autorización)
* **Gestión de Estado:** Hooks personalizados de React y Context API.

## Características Principales

### 1. Agenda Médica Avanzada
* Calendario interactivo con vistas diarias, semanales y mensuales.
* Seguimiento en tiempo real del estado de las citas (Pendiente, Confirmada, En Progreso, Atendida, Cancelada).
* Obtención de datos optimizada: El sistema solo descarga el rango de tiempo visible en pantalla, garantizando una complejidad de red $O(1)$ independientemente del tamaño total de la base de datos.

### 2. Expediente Clínico Electrónico (EHR)
* Vista histórica completa de las consultas médicas y procedimientos de cada paciente.
* **Búsqueda en el Servidor y Debouncing:** La funcionalidad de búsqueda se delega al motor de PostgreSQL a través de EF Core (`.Contains()`). El frontend implementa un algoritmo de *Debounce* de 500ms para evitar la saturación de la API, asegurando una alta escalabilidad incluso con miles de registros históricos.

### 3. Panel de Control y Analíticas (Dashboard)
* Panel dinámico que ofrece un resumen en tiempo real de la actividad diaria del médico.
* Métricas reactivas y widgets de acceso rápido para una atención inmediata al paciente.

### 4. Seguridad y Buenas Prácticas
* Las contraseñas nunca se almacenan en texto plano; el sistema utiliza BCrypt para el hashing criptográfico.
* Gestión de sesiones sin estado (stateless) segura mediante JWT.
* Arquitectura en capas profundamente estructurada (Controladores -> Servicios -> Repositorios -> Base de datos) que evita la contaminación cruzada de lógica y hace cumplir el Principio de Responsabilidad Única.

## Configuración del Entorno Local

### Requisitos Previos
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Node.js](https://nodejs.org/) (v18 o superior)
* [PostgreSQL](https://www.postgresql.org/) (v14 o superior)

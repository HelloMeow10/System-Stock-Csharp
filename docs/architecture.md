# Arquitectura del Sistema (Service-Oriented)

Este documento describe la arquitectura del sistema de gestión de usuarios, que ha sido refactorizada para seguir un enfoque orientado a servicios (Service-Oriented Architecture - SOA).

## Resumen Ejecutivo

La arquitectura del sistema ha sido reestructurada para desacoplar completamente el cliente de la lógica de negocio, utilizando una API REST como intermediario. Anteriormente, la aplicación de escritorio (`Presentation`) tenía una dependencia directa de la capa de `BusinessLogic`, lo que creaba un acoplamiento no deseado.

La nueva arquitectura es **robusta, escalable y sigue las mejores prácticas de SOA**, con una clara separación entre el cliente (frontend) y el servidor (backend).

En resumen:
- **La arquitectura ahora es estrictamente orientada a servicios.**
- **Se ha eliminado la dependencia directa de `Presentation` a `BusinessLogic`.**
- **Toda la comunicación entre el cliente y el servidor se realiza a través de la API REST.**
- **El cliente (`Presentation`) utiliza un `ApiClient` para consumir los servicios expuestos por la API.**

## Diagrama de Arquitectura

A continuación se muestra un diagrama que ilustra la nueva estructura del sistema:

```
+--------------------------------+
|      Presentation (UI)         |
| (App de Escritorio WinForms)   |
| - Contiene ApiClient           |
+--------------------------------+
               |
               v
+--------------------------------+
|         Services (API)         |
|   (Punto de entrada REST)      |
+--------------------------------+
               |
               v
+--------------------------------+
|            Session             |
|     (Gestión de Tokens JWT)    |
+--------------------------------+
               |
               v
+--------------------------------+
|      BusinessLogic (Core)      |
| (Lógica de Negocio y Servicios)|
+--------------------------------+
               |
               v
+--------------------------------+
|      DataAccess (DAL)          |
|    (Acceso a Base de Datos)    |
+--------------------------------+
```

**Flujo de Dependencias:**
`Presentation` -> `Services` (implícitamente, a través de HTTP)
`Services` -> `Session`
`Services` -> `BusinessLogic`
`BusinessLogic` -> `DataAccess`

**Nota:** La dependencia de `Presentation` a `Contracts` y `SharedKernel` existe para poder reutilizar los DTOs y modelos compartidos, lo cual es una práctica aceptable.

## Análisis por Capa

### 1. Capa de Presentación (`Presentation`)
- **Propósito:** Ser la interfaz de usuario para la aplicación de escritorio.
- **Análisis:** **Correcto.** Esta capa ahora está completamente desacoplada de la lógica de negocio. Utiliza un `ApiClient` para todas las operaciones, enviando solicitudes HTTP a la capa de `Services`. No tiene conocimiento de la implementación interna del servidor.
- **Veredicto:** :white_check_mark: **Arquitectura Correcta y Desacoplada.**

### 2. Capa de Servicios (`Services`)
- **Propósito:** Exponer la lógica de negocio como una API RESTful.
- **Análisis:** **Correcto.** Actúa como la única puerta de entrada para los clientes. Recibe solicitudes HTTP, las traduce en llamadas a la `BusinessLogic` y devuelve respuestas estandarizadas (DTOs o respuestas de error).
- **Veredicto:** :white_check_mark: **Correcto.**

### 3. Capa de Lógica de Negocio (`BusinessLogic`)
- **Propósito:** Contener la lógica de negocio central.
- **Análisis:** **Correcto.** No ha sufrido cambios significativos, pero ahora su único consumidor es la capa de `Services`, lo que garantiza que las reglas de negocio se apliquen de manera consistente.
- **Veredicto:** :white_check_mark: **Correcto.**

### 4. Capa de Acceso a Datos (`DataAccess`)
- **Propósito:** Encapsular toda la comunicación con la base de datos.
- **Análisis:** **Correcto.** Sin cambios. Sigue siendo responsable únicamente de la persistencia de datos.
- **Veredicto:** :white_check_mark: **Correcto.**

## Conclusión

La arquitectura del sistema ha sido migrada con éxito a un modelo orientado a servicios. El cliente y el servidor están completamente desacoplados, lo que mejora la mantenibilidad, permite la escalabilidad (por ejemplo, añadiendo un cliente web en el futuro) y se alinea con las mejores prácticas de desarrollo de software moderno.

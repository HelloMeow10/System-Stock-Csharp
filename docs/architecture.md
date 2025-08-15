# Análisis de la Arquitectura del Sistema (Post-Refactorización)

Este documento describe la arquitectura refactorizada del sistema de gestión de usuarios, asegurando que cumple estrictamente con los principios de una arquitectura de n-capas.

## Resumen Ejecutivo

La arquitectura del sistema ha sido refactorizada para corregir violaciones de dependencias y alinearla con un modelo estricto de n-capas. Anteriormente, existían dependencias incorrectas, como la capa de `Presentation` accediendo directamente a `DataAccess`. Estos problemas han sido resueltos.

La estructura actual es **limpia, robusta y sigue las mejores prácticas**, garantizando una separación de responsabilidades clara y un flujo de dependencias unidireccional.

En resumen:
- **La arquitectura ahora es estrictamente correcta.**
- **Se han eliminado todas las violaciones de dependencias** entre capas.
- **Se ha eliminado el proyecto `Common`** que no se utilizaba.
- **La gestión de dependencias se realiza con el contenedor de DI de .NET** (`Microsoft.Extensions.DependencyInjection`), configurado en la capa de `Presentation`.

## Diagrama de Arquitectura Corregido

A continuación se muestra un diagrama que ilustra la nueva estructura del sistema:

```
+--------------------------------+      +--------------------------------+
|      Presentation (UI)         |      |         Services (API)         |
| (App de Escritorio WinForms)   |      |   (Punto de entrada para Web)  |
+--------------------------------+      +--------------------------------+
               |                                      |
               |                                      v
               |                      +--------------------------------+
               |                      |            Session             |
               |                      |     (Gestión de Tokens JWT)    |
               |                      +--------------------------------+
               |                                      |
               +-----------------+--------------------+
                                 |
                                 v
+--------------------------------------------------------------------------+
|                 BusinessLogic (Capa de Lógica de Negocio)                  |
| (Contiene servicios, DTOs, lógica de negocio y un método de extensión para DI) |
+--------------------------------------------------------------------------+
                                 |
                                 v
+--------------------------------------------------------------------------+
|                   DataAccess (Capa de Acceso a Datos)                      |
|      (Repositorios, consultas a la BBDD, mapeo de entidades)             |
+--------------------------------------------------------------------------+
```

**Flujo de Dependencias:**
`Presentation` -> `BusinessLogic`
`Services` -> `BusinessLogic`
`Services` -> `Session`
`BusinessLogic` -> `DataAccess`

## Análisis por Capa (Post-Refactorización)

### 1. Capa de Presentación (`Presentation`)
- **Propósito:** Ser la interfaz de usuario para la aplicación de escritorio (Windows Forms).
- **Análisis:** **Correcto.** Esta capa ahora depende únicamente de `BusinessLogic`. La inicialización de servicios se realiza en `Program.cs` usando el **Host Genérico de .NET**, que configura el contenedor de inyección de dependencias. Los formularios reciben sus dependencias (como servicios de la capa de negocio) a través de **inyección por constructor**.
- **Veredicto:** :white_check_mark: **Corregido y Correcto.**

### 2. Capa de Lógica de Negocio (`BusinessLogic`)
- **Propósito:** Contener la lógica de negocio central y actuar como el único punto de contacto con la capa de datos.
- **Análisis:** **Correcto.** Esta capa sigue siendo el corazón de la aplicación. Proporciona un **método de extensión de `IServiceCollection` (`AddInfrastructure`)** que permite a las capas de presentación registrar todos los servicios y repositorios necesarios de forma encapsulada, sin necesidad de conocer los detalles de implementación. Esto promueve un bajo acoplamiento.
- **Veredicto:** :white_check_mark: **Correcto.**

### 3. Capa de Acceso a Datos (`DataAccess`)
- **Propósito:** Encapsular toda la comunicación con la base de datos.
- **Análisis:** **Correcto.** Esta capa no ha cambiado, ya que su diseño original era correcto. No tiene dependencias de otras capas y su única responsabilidad es la persistencia de datos. Se ha refactorizado `SqlUserRepository` para que el método `GetAllUsers` no devuelva datos de `Persona`, respetando el principio de Responsabilidad Única.
- **Veredicto:** :white_check_mark: **Correcto.**

### 4. Capa de Servicios (`Services`)
- **Propósito:** Exponer la lógica de negocio como una API web (HTTP).
- **Análisis:** **Correcto.** Actúa como una capa de presentación para clientes web. Depende de `BusinessLogic` para ejecutar las operaciones y de `Session` para la generación de tokens, lo cual es una separación de responsabilidades adecuada.
- **Veredicto:** :white_check_mark: **Correcto.**

### 5. Capa de Sesión (`Session`)
- **Propósito:** Gestionar la creación y validación de tokens de sesión (JWT).
- **Análisis:** **Correcto.** Se ha corregido la dependencia que tenía con `BusinessLogic`. Ahora es una capa independiente y cohesiva que se enfoca únicamente en la gestión de tokens, utilizada por la capa de `Services`.
- **Veredicto:** :white_check_mark: **Corregido y Correcto.**

### 6. Capa Común (`Common`)
- **Análisis:** Este proyecto estaba vacío y no cumplía ninguna función. **Ha sido eliminado** para limpiar la estructura del proyecto. Las referencias innecesarias desde las otras capas también se han eliminado.
- **Veredicto:** :heavy_check_mark: **Eliminado.**

## Conclusión

La arquitectura del sistema ahora cumple estrictamente con los principios de diseño de n-capas. Las dependencias fluyen en una única dirección (`Presentación` -> `Lógica` -> `Datos`), la separación de responsabilidades es clara y el código es más mantenible y robusto.

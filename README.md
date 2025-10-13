# Sistema de Gestión de Usuarios

## 1. Descripción  
Sistema de escritorio (.NET 8, Windows Forms) con arquitectura N-Capas para ABM de usuarios, autenticación SHA-256 y políticas de seguridad.

## 2. Tecnologías  
- .NET 8, C#
- ADO.NET (SqlClient) + SQL Server
- JWT para sesión  
- xUnit para pruebas
- GitHub Actions para CI
- Servidor SMTP (para envío de correos como recuperación de contraseña y 2FA)

## 3. Estructura del repositorio
- `src/Presentation`: Interfaz de usuario (Windows Forms)
- `src/BusinessLogic`: Lógica de negocio
- `src/DataAccess`: Acceso a datos (ADO.NET) y entidades
- `src/Services`: API REST que expone la lógica de negocio. Es el backend para todos los clientes.
- `src/Session`: Gestión de sesión y tokens JWT

## 4. Cómo ejecutar localmente (Windows)  

Requisitos previos:
- Windows 10/11, .NET SDK 8 (y .NET 9 para Blazor, ya incluido en el repo), SQL Server (LocalDB o instancia local).
- PowerShell (v5+). Los comandos aquí mostrados están adaptados para PowerShell.

1) Clonar y restaurar dependencias

```powershell
git clone <URL_DEL_REPOSITORIO>
cd System-Stock-Csharp
dotnet restore .\UserManagementSystem.sln
```

2) Base de datos (SQL Server)
- El backend está configurado por defecto para usar Windows Authentication:
   `Server=localhost;Database=login2;Trusted_Connection=True;TrustServerCertificate=True;`
- Crea la BD ejecutando el script del repositorio:

```powershell
# Con SSMS puedes abrir y ejecutar create_database_login2_corrected_v2.sql
# O con sqlcmd (si usas SQL Auth, ajusta -U/-P)
sqlcmd -S localhost -E -i .\create_database_login2_corrected_v2.sql
```

3) Configuración rápida
- Backend API (`src/Services/appsettings.json`):
   - ConnectionStrings: ya apunta a `login2` con Trusted_Connection=True.
   - Jwt: clave larga ya configurada. No necesitas cambiarla para desarrollo.
- WinForms (`src/Presentation/appsettings.json`):
   - ConnectionStrings: igual que el backend (para operaciones locales si aplica).
   - ApiSettings: `BaseUrl` debe apuntar al backend: `http://localhost:5000`.
   - SmtpSettings: opcional para recuperación de contraseña/2FA. Puedes dejar datos dummy en dev.
- Blazor (`blazor-server/AgileStockPro.Web`):
   - Usa `ApiOptions.BaseUrl = "http://localhost:5000/"` (valor por defecto en código).
   - Se levanta en `http://localhost:5173` por defecto.

4) Ejecutar el backend (API REST)

Desde la raíz del repo:

```powershell
dotnet run --project .\src\Services\Services.csproj
```

Deberías ver: `Now listening on: http://localhost:5000`. Deja esta ventana abierta.

5) Ejecutar la app de escritorio (WinForms)

En otra consola PowerShell, desde la raíz:

```powershell
dotnet run --project .\src\Presentation\Presentation.csproj
```

Notas:
- El `appsettings.json` de WinForms se resuelve desde la carpeta de salida (ya está ajustado con `AppContext.BaseDirectory`).
- Login por defecto (si fue sembrado en tu BD): usuario `admin`, clave `admin123`.

6) Ejecutar la app Blazor Server (UI Web)

En otra consola PowerShell, desde la raíz:

```powershell
dotnet run --project .\blazor-server\AgileStockPro.Web\AgileStockPro.Web.csproj
```

Abre `http://localhost:5173` en el navegador. Para que Blazor funcione, el backend (paso 4) debe estar corriendo en `http://localhost:5000`.

Notas de UI (Blazor):
- Usuarios: confirmación antes de eliminar y paginación simple (Anterior/Siguiente).
- Personas: confirmación antes de eliminar, paginación simple y combos en cascada Provincia → Partido → Localidad, además de Género y Tipo Doc.

Detalles recientes (Blazor):
- Filtros y paginación server-side en Usuarios y Personas.
- Manejo de errores uniforme: el cliente parsea ProblemDetails del backend para mostrar errores de validación inline y via toasts (crear/editar/eliminar/listar).

7) Flujo recomendado de pruebas end-to-end
- Backend arriba (http://localhost:5000)
- Blazor arriba (http://localhost:5173)
- Inicia sesión en Blazor con tu usuario:
   - Autenticación con JWT (el token se envía en el header Authorization). Si tu política habilita 2FA, al loguear se te pedirá el código.
- Visita “Usuarios” y “Personas” si eres Admin:
   - Listar/crear/editar/eliminar usuarios y personas usando las APIs.
- WinForms: prueba login y “Mi Perfil”, que consulta `api/v1/users/me` del backend.

8) Variables y configuración relevantes
- `src/Services/appsettings.json` (backend)
   - ConnectionStrings: SQL Server local con Windows Auth.
   - Jwt.Key: cadena >=256-bit para HS256.
   - Authentication.ApiKey: solo para escenarios de API-Key (opcional).
- `src/Presentation/appsettings.json` (WinForms)
   - ApiSettings.BaseUrl: URL del backend.
   - SmtpSettings: servidor SMTP para recuperación/2FA por email (opcional en dev).
- `blazor-server/AgileStockPro.Web/Services/Api/ApiOptions.cs` (Blazor)
   - BaseUrl del backend (por defecto `http://localhost:5000/`).

9) Errores comunes y soluciones
- “No se puede conectar a SQL Server”:
   - Verifica que SQL Server (o LocalDB) está en ejecución y que la BD `login2` existe.
   - Re-ejecuta el script de creación, revisa permisos del usuario Windows actual.
- “Error de firma JWT / longitud de clave”:
   - Usa una Key de al menos 32 bytes (ya provista en appsettings.json del backend).
- “403 Forbidden” al consumir APIs desde UI:
   - Asegúrate de iniciar sesión y que el usuario tenga rol Admin para endpoints administrativos (e.g. usuarios, personas POST/PUT/DELETE).
- “Respuesta vacía / ProblemDetails en cliente”:
   - Ya se manejan errores con mensajes; revisa la salida para ver el detalle y corrige datos requeridos.

10) Playbook rápido (pasos mínimos)

```powershell
# 1) Backend
dotnet run --project .\src\Services\Services.csproj

# 2) Blazor (en otra consola)
dotnet run --project .\blazor-server\AgileStockPro.Web\AgileStockPro.Web.csproj

# 3) WinForms (opcional, en otra consola)
dotnet run --project .\src\Presentation\Presentation.csproj
```

Tip: también podés usar las tareas de VS Code incluidas (.vscode/tasks.json) para levantar Backend, Blazor y WinForms con un par de clics.

## 5. Branching model  
- **main**: siempre verde, releases en producción.  
- **develop**: integración de features, base de los sprints.  
- **feature/xxx**: ramitas para cada historia de usuario.  
- **release/x.y**: preparación de versión.  
- **hotfix/x.y.z**: corrección urgente desde main.

## 6. Contribuir  
- Abrir PRs contra `develop`.  
- Asignar reviewers.  
- Referenciar Issue: “Closes #12”.  

## 7. CI/CD  
El workflow `.github/workflows/dotnet-ci.yml` compila, testea y publica artefactos.

## 8. Licencia  
MIT (u otra que prefiráis)

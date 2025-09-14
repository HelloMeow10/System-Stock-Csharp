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

## 4. Cómo ejecutar localmente  
1. **Clonar el repositorio:**
   ```bash
   git clone <URL_DEL_REPOSITORIO>
   cd <NOMBRE_DEL_DIRECTORIO>
   ```
2. **Restaurar dependencias de .NET:**
   ```bash
   dotnet restore UserManagementSystem.sln
   ```
3. **Crear la base de datos:**
   - Asegúrate de tener una instancia de SQL Server en ejecución.
   - Ejecuta el script `create_database_login2_corrected_v2.sql` en tu instancia de SQL Server. Puedes usar una herramienta como SQL Server Management Studio (SSMS) o el comando `sqlcmd`:
     ```bash
     sqlcmd -S localhost -U sa -P <tu_contraseña> -i create_database_login2_corrected_v2.sql
     ```
     *Nota: Ajusta los parámetros `-S`, `-U`, y `-P` según la configuración de tu servidor.*

4. **Configurar la aplicación:**
   - Abre el archivo `src/Presentation/appsettings.json`.
   - Modifica la `ConnectionStrings:DefaultConnection` para que apunte a tu base de datos.
   - Configura las `SmtpSettings` con los datos de tu servidor de correo para que funcionen la recuperación de contraseña y la autenticación de dos factores (2FA).

5. **Ejecutar la aplicación:**
   - Abre la solución `UserManagementSystem.sln` en Visual Studio 2022 (o superior).
   - Establece el proyecto `Presentation` como proyecto de inicio.
   - Inicia la depuración (F5). El usuario administrador por defecto es `admin` con contraseña `admin123`.

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

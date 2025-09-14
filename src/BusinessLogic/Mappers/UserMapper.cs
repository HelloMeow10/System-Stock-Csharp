using BusinessLogic.Models;
using DataAccess.Entities;

namespace BusinessLogic.Mappers
{
    public static class UserMapper
    {
        public static UserDto? MapToUserDto(Usuario? u)
        {
            if (u == null) return null;
            return new UserDto
            {
                IdUsuario = u.IdUsuario,
                Username = u.UsuarioNombre,
                Rol = u.Rol?.Nombre,
                IdRol = u.IdRol,
                IdPersona = u.IdPersona,
                CambioContrasenaObligatorio = u.CambioContrasenaObligatorio,
                FechaExpiracion = u.FechaExpiracion,
                Habilitado = u.FechaBloqueo == null || u.FechaBloqueo > System.DateTime.Now
            };
        }

        public static UpdateUserRequest MapToUpdateUserRequest(Usuario u, Persona p)
        {
            return new UpdateUserRequest
            {
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Correo = p.Correo,
                IdRol = u.IdRol,
                CambioContrasenaObligatorio = u.CambioContrasenaObligatorio,
                FechaExpiracion = u.FechaExpiracion,
                Habilitado = u.FechaBloqueo == null || u.FechaBloqueo > System.DateTime.Now
            };
        }
    }
}

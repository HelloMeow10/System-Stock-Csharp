// src/BusinessLogic/Models/UserResponse.cs
namespace Contracts
{
    public class UserResponse
    {
        public string Username { get; set; } = null!;
        public string? Rol { get; set; }
        public bool CambioContrasenaObligatorio { get; set; }
        public int IdPersona { get; set; }
    }
}
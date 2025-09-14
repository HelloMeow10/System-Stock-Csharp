// src/BusinessLogic/Models/UserRequest.cs
namespace Contracts
{
    public class UserRequest
    {
        public string PersonaId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
namespace Contracts
{
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class Validate2faRequest
    {
        public string Username { get; set; } = null!;
        public string Code { get; set; } = null!;
    }

    public class LoginResponse
    {
        public bool Requires2fa { get; set; }
        public string? Username { get; set; }
        public string? Rol { get; set; }
        public string? Token { get; set; }
    }
}

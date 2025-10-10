// src/BusinessLogic/Models/AuthenticationResult.cs
namespace Contracts
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public bool Requires2fa { get; set; }
        public UserResponse? User { get; set; }
        public string? ErrorMessage { get; set; }

        public static AuthenticationResult Succeeded(UserResponse user) =>
            new AuthenticationResult { Success = true, User = user };

        public static AuthenticationResult Failed(string message) =>
            new AuthenticationResult { Success = false, ErrorMessage = message };

        public static AuthenticationResult TwoFactorRequired() =>
            new AuthenticationResult { Success = true, Requires2fa = true };
    }
}

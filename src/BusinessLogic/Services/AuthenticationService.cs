using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Contracts;
using BusinessLogic.Security;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISecurityPolicyService _securityPolicyService;

        private const string AdminRole = "Administrador";

        public AuthenticationService(
            IUserRepository userRepository,
            IPersonaRepository personaRepository,
            IEmailService emailService,
            ILogger<AuthenticationService> logger,
            IPasswordHasher passwordHasher,
            ISecurityPolicyService securityPolicyService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _securityPolicyService = securityPolicyService ?? throw new ArgumentNullException(nameof(securityPolicyService));
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string username, string password)
        {
            return await ExecuteServiceOperationAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    return AuthenticationResult.Failed("Usuario o contraseña no pueden estar vacíos.");

                var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username);

                var validationResult = ValidateCredentials(usuario, username, password) ?? CheckAccountStatus(usuario!);
                if (validationResult != null)
                {
                    return validationResult;
                }

                var politica = await _securityPolicyService.GetPoliticaSeguridadAsync();
                if (politica?.Autenticacion2FA ?? false)
                {
                    return await HandleTwoFactorAuthentication(username, usuario!.IdPersona);
                }

                return CreateSuccessfulAuthenticationResult(usuario!);
            }, "authenticating user");
        }

        private AuthenticationResult? ValidateCredentials(DataAccess.Entities.Usuario? usuario, string username, string password)
        {
            if (usuario == null)
                return AuthenticationResult.Failed("Usuario o contraseña incorrectos.");

            var hash = _passwordHasher.Hash(username, password);
            if (!hash.SequenceEqual(usuario.ContrasenaScript))
                return AuthenticationResult.Failed("Usuario o contraseña incorrectos.");

            return null; // Credentials are valid
        }

        private AuthenticationResult? CheckAccountStatus(DataAccess.Entities.Usuario usuario)
        {
            if (usuario.FechaBloqueo < DateTime.Now)
            {
                return AuthenticationResult.Failed("La cuenta se encuentra deshabilitada.");
            }

            if (usuario.FechaExpiracion.HasValue && usuario.FechaExpiracion.Value < DateTime.Now)
            {
                return AuthenticationResult.Failed("La cuenta ha expirado.");
            }

            return null; // Account status is valid
        }

        private async Task<AuthenticationResult> HandleTwoFactorAuthentication(string username, int personaId)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(personaId);
            if (persona == null || string.IsNullOrWhiteSpace(persona.Correo))
            {
                return AuthenticationResult.Failed("No se puede usar 2FA sin un correo configurado.");
            }

            var code = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(5);

            await _userRepository.Set2faCodeAsync(username, code, expiry);

            await _emailService.Send2faCodeEmailAsync(persona.Correo, code);

            return AuthenticationResult.TwoFactorRequired();
        }

        private AuthenticationResult CreateSuccessfulAuthenticationResult(DataAccess.Entities.Usuario usuario)
        {
            var userResponse = new UserResponse
            {
                Username = usuario.UsuarioNombre,
                Rol = usuario.Rol?.Nombre,
                CambioContrasenaObligatorio = usuario.CambioContrasenaObligatorio,
                IdPersona = usuario.IdPersona
            };

            var nextAction = DetermineNextAction(userResponse);
            return AuthenticationResult.Succeeded(userResponse, nextAction);
        }

        public async Task<AuthenticationResult> Validate2faAsync(string username, string code)
        {
            return await ExecuteServiceOperationAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(code))
                {
                    return AuthenticationResult.Failed("El código 2FA es requerido.");
                }

                var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username);
                if (usuario == null || usuario.Codigo2FA != code || usuario.Codigo2FAExpiracion < DateTime.UtcNow)
                {
                    return AuthenticationResult.Failed("El código 2FA es inválido o ha expirado.");
                }

                usuario.SetTwoFactorCode(null, null);
                await _userRepository.UpdateUsuarioAsync(usuario);

                var userResponse = new UserResponse
                {
                    Username = usuario.UsuarioNombre,
                    Rol = usuario.Rol?.Nombre,
                    CambioContrasenaObligatorio = usuario.CambioContrasenaObligatorio,
                    IdPersona = usuario.IdPersona
                };

                var nextAction = DetermineNextAction(userResponse);
                return AuthenticationResult.Succeeded(userResponse, nextAction);
            }, "validating 2FA");
        }

        private PostLoginAction DetermineNextAction(UserResponse user)
        {
            if (user.CambioContrasenaObligatorio)
            {
                return PostLoginAction.ChangePassword;
            }
            if (user.Rol == AdminRole)
            {
                return PostLoginAction.ShowAdminDashboard;
            }
            return PostLoginAction.ShowUserDashboard;
        }

        private async Task<T> ExecuteServiceOperationAsync<T>(Func<Task<T>> operation, string operationName)
        {
            try
            {
                return await operation();
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during {OperationName}", operationName);
                throw new BusinessLogicException($"An unexpected error occurred during {operationName}.", ex);
            }
        }
    }
}

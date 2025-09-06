using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using BusinessLogic.Models;
using BusinessLogic.Security;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly ISecurityRepository _securityRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<PasswordService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordPolicyValidator _passwordPolicyValidator;

        public PasswordService(
            IUserRepository userRepository,
            IPersonaRepository personaRepository,
            ISecurityRepository securityRepository,
            IEmailService emailService,
            ILogger<PasswordService> logger,
            IPasswordHasher passwordHasher,
            IPasswordPolicyValidator passwordPolicyValidator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _passwordPolicyValidator = passwordPolicyValidator ?? throw new ArgumentNullException(nameof(passwordPolicyValidator));
        }

        public async Task RecoverPasswordAsync(string username, Dictionary<int, string> answers) => await ExecuteServiceOperationAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ValidationException("Username is required");

            var politica = _securityRepository.GetPoliticaSeguridad() ?? throw new BusinessLogicException("Security policy not configured.");
            if (answers == null || answers.Count != politica.CantPreguntas || answers.Any(r => string.IsNullOrWhiteSpace(r.Value)))
                throw new ValidationException($"Se requieren {politica.CantPreguntas} respuestas de seguridad.");

            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username)
                ?? throw new ValidationException($"Usuario '{username}' not found");

            var persona = _personaRepository.GetPersonaById(usuario.IdPersona)
                ?? throw new ValidationException("Persona no encontrada");

            var respuestasGuardadas = _securityRepository.GetRespuestasSeguridadByUsuarioId(usuario.IdUsuario)
                ?? throw new ValidationException("No se han configurado las preguntas de seguridad.");

            if (respuestasGuardadas.Count != politica.CantPreguntas)
                throw new ValidationException("La cantidad de respuestas guardadas no coincide con la política de seguridad.");

            var respuestasGuardadasDict = respuestasGuardadas.ToDictionary(r => r.IdPregunta, r => r.Respuesta);

            foreach (var answer in answers)
            {
                if (!respuestasGuardadasDict.TryGetValue(answer.Key, out var respuestaGuardada) || respuestaGuardada != answer.Value)
                {
                    throw new ValidationException("Una o más respuestas de seguridad son incorrectas.");
                }
            }

            var newPassword = GenerateRandomPassword(username, persona);
            var newPasswordHash = _passwordHasher.Hash(username, newPassword);

            usuario.ChangePassword(newPasswordHash);
            usuario.ForcePasswordChange(true);
            await _userRepository.UpdateUsuarioAsync(usuario);

            if (string.IsNullOrEmpty(persona.Correo))
            {
                throw new ValidationException("El usuario no tiene una dirección de correo electrónico configurada.");
            }

            await _emailService.SendPasswordResetEmailAsync(persona.Correo, newPassword);
        }, "recovering password");

        public async Task ChangePasswordAsync(string username, string newPassword, string oldPassword) => await ExecuteServiceOperationAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(oldPassword))
                throw new ValidationException("Todos los campos son requeridos.");

            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username)
                ?? throw new ValidationException($"Usuario '{username}' not found");

            var oldPasswordHash = _passwordHasher.Hash(username, oldPassword);
            if (!oldPasswordHash.SequenceEqual(usuario.ContrasenaScript))
            {
                throw new ValidationException("La contraseña actual es incorrecta. Por favor, intente de nuevo.");
            }

            var persona = _personaRepository.GetPersonaById(usuario.IdPersona)
                ?? throw new ValidationException("Persona no encontrada");

            ValidatePasswordPolicy(newPassword, username, persona);

            var newPasswordHash = _passwordHasher.Hash(username, newPassword);

            var politica = _securityRepository.GetPoliticaSeguridad();
            if (politica?.NoRepetirAnteriores ?? false)
            {
                var historial = await _userRepository.GetHistorialContrasenasByUsuarioIdAsync(usuario.IdUsuario);
                if (historial.Any(h => h.ContrasenaScript.SequenceEqual(newPasswordHash)))
                {
                    throw new ValidationException("La nueva contraseña no puede ser igual a ninguna de las contraseñas anteriores.");
                }
            }

            var currentPasswordHash = usuario.ContrasenaScript;
            await _userRepository.AddHistorialContrasenaAsync(new HistorialContrasena
            {
                IdUsuario = usuario.IdUsuario,
                ContrasenaScript = currentPasswordHash,
                FechaCambio = DateTime.Now
            });

            usuario.ChangePassword(newPasswordHash);
            await _userRepository.UpdateUsuarioAsync(usuario);
        }, "changing password");

        private void ValidatePasswordPolicy(string password, string username, Persona persona)
        {
            var politica = _securityRepository.GetPoliticaSeguridad();
            if (politica == null) return;

            if (password.Length < politica.MinCaracteres)
                throw new ValidationException($"La contraseña debe tener al menos {politica.MinCaracteres} caracteres.");

            if (politica.MayusYMinus && (!password.Any(char.IsUpper) || !password.Any(char.IsLower)))
                throw new ValidationException("La contraseña debe contener mayúsculas y minúsculas.");

            if (politica.LetrasYNumeros && (!password.Any(char.IsLetter) || !password.Any(char.IsDigit)))
                throw new ValidationException("La contraseña debe contener letras y números.");

            if (politica.CaracterEspecial && !password.Any(c => !char.IsLetterOrDigit(c)))
                throw new ValidationException("La contraseña debe contener caracteres especiales.");

            if (politica.SinDatosPersonales)
            {
                if (password.Contains(username, StringComparison.OrdinalIgnoreCase) ||
                    password.Contains(persona.Nombre, StringComparison.OrdinalIgnoreCase) ||
                    password.Contains(persona.Apellido, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ValidationException("La contraseña no debe contener datos personales (nombre de usuario, nombre o apellido).");
                }

                if (persona.FechaNacimiento.HasValue)
                {
                    string[] dateFormats = { "ddMMyyyy", "yyyyMMdd", "ddMM", "MMdd" };
                    foreach (var format in dateFormats)
                    {
                        if (password.Contains(persona.FechaNacimiento.Value.ToString(format)))
                        {
                            throw new ValidationException("La contraseña no debe contener su fecha de nacimiento.");
                        }
                    }
                }
            }
        }

        private string GenerateRandomPassword(string? username = null, Persona? persona = null)
        {
            var politica = _securityRepository.GetPoliticaSeguridad() ?? throw new BusinessLogicException("Security policy not configured.");
            var random = new Random();

            while (true)
            {
                var minLength = politica.MinCaracteres > 0 ? politica.MinCaracteres : 12;
                var passwordChars = new List<char>();
                const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string lower = "abcdefghijklmnopqrstuvwxyz";
                const string digits = "0123456789";
                const string specials = "!@#$%^&*()";
                var allChars = new StringBuilder(upper).Append(lower).Append(digits).Append(specials).ToString();

                if (politica.MayusYMinus)
                {
                    passwordChars.Add(upper[random.Next(upper.Length)]);
                    passwordChars.Add(lower[random.Next(lower.Length)]);
                }
                if (politica.LetrasYNumeros)
                {
                    passwordChars.Add(digits[random.Next(digits.Length)]);
                }
                if (politica.CaracterEspecial)
                {
                    passwordChars.Add(specials[random.Next(specials.Length)]);
                }

                while (passwordChars.Count < minLength)
                {
                    passwordChars.Add(allChars[random.Next(allChars.Length)]);
                }

                var password = new string(passwordChars.OrderBy(c => random.Next()).ToArray());

                if (politica.SinDatosPersonales && username != null && persona != null)
                {
                    try
                    {
                        _passwordPolicyValidator.Validate(password, username, persona, politica);
                    }
                    catch (ValidationException)
                    {
                        continue;
                    }
                }
                return password;
            }
        }

        private async Task ExecuteServiceOperationAsync(Func<Task> operation, string operationName)
        {
            try
            {
                await operation();
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

        private void ExecuteServiceOperation(Action operation, string operationName)
        {
            try
            {
                operation();
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

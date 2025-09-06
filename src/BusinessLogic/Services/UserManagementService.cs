using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using BusinessLogic.Exceptions;
using BusinessLogic.Security;
using BusinessLogic.Factories;
using BusinessLogic.Mappers;

namespace BusinessLogic.Services
{
    public class UserManagementService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserManagementService> _logger;
        private readonly IUsuarioFactory _usuarioFactory;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPersonaService _personaService;


        public UserManagementService(
            IUserRepository userRepository,
            IPersonaRepository personaRepository,
            IEmailService emailService,
            ILogger<UserManagementService> logger,
            IUsuarioFactory usuarioFactory,
            IPasswordHasher passwordHasher,
            IPersonaService personaService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _usuarioFactory = usuarioFactory ?? throw new ArgumentNullException(nameof(usuarioFactory));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _personaService = personaService ?? throw new ArgumentNullException(nameof(personaService));
        }

        private T ExecuteServiceOperation<T>(Func<T> operation, string operationName)
        {
            try
            {
                return operation();
            }
            catch (ValidationException)
            {
                throw;
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
        }

        public async Task<UserDto> CreateUserAsync(UserRequest request) => await ExecuteServiceOperationAsync(async () =>
        {
            var (usuario, plainPassword) = _usuarioFactory.Create(request);

            await _userRepository.AddUsuarioAsync(usuario);

            var persona = _personaRepository.GetPersonaById(usuario.IdPersona)!;

            await _emailService.SendWelcomeEmailAsync(persona.Correo!, usuario.UsuarioNombre, plainPassword);

            return UserMapper.MapToUserDto(usuario)!;
        }, "creating a user");

        public async Task<List<UserDto>> GetAllUsersAsync() => await ExecuteServiceOperationAsync(async () =>
        {
            var usuarios = await _userRepository.GetAllUsersAsync();
            var personas = (await _personaService.GetPersonasAsync()).ToDictionary(p => p.IdPersona);

            return usuarios.Select(u =>
            {
                var userDto = UserMapper.MapToUserDto(u)!;
                if (personas.TryGetValue(u.IdPersona, out var persona))
                {
                    userDto.Nombre = persona.Nombre;
                    userDto.Apellido = persona.Apellido;
                    userDto.Correo = persona.Correo;
                }
                return userDto;
            }).ToList();
        }, "getting all users");

        public async Task UpdateUserAsync(UserDto userDto) => await ExecuteServiceOperationAsync(async () =>
        {
            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(userDto.Username)
                ?? throw new ValidationException($"Usuario '{userDto.Username}' not found");

            // The admin username should come from the current session context in a real app
            const string adminUsername = "Admin";

            usuario.ChangeRole(userDto.IdRol);
            usuario.SetExpiration(userDto.FechaExpiracion);
            usuario.ForcePasswordChange(userDto.CambioContrasenaObligatorio);

            if (userDto.Habilitado)
            {
                usuario.Habilitar();
            }
            else
            {
                usuario.Deshabilitar(adminUsername);
            }

            await _userRepository.UpdateUsuarioAsync(usuario);
        }, "updating user");

        public async Task DeleteUserAsync(int userId) => await ExecuteServiceOperationAsync(async () =>
            await _userRepository.DeleteUsuarioAsync(userId),
            "deleting user");

        public async Task<UserDto?> GetUserByUsernameAsync(string username) => await ExecuteServiceOperationAsync(async () =>
        {
            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username);
            return UserMapper.MapToUserDto(usuario);
        }, "getting user by username");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using SharedKernel;
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
            var (usuario, plainPassword) = await _usuarioFactory.Create(request);

            await _userRepository.AddUsuarioAsync(usuario);

            var persona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona)!;

            await _emailService.SendWelcomeEmailAsync(persona.Correo!, usuario.UsuarioNombre, plainPassword);

            return UserMapper.MapToUserDto(usuario)!;
        }, "creating a user");

        public async Task<PagedList<UserDto>> GetUsersAsync(PaginationParams paginationParams) => await ExecuteServiceOperationAsync(async () =>
        {
            var pagedUsers = await _userRepository.GetUsersAsync(paginationParams);

            // Get Personas only for the users in the current page for efficiency
            var personaIds = pagedUsers.Items.Select(u => u.IdPersona).Distinct();
            var personas = new Dictionary<int, PersonaDto>();
            foreach (var id in personaIds)
            {
                var persona = await _personaService.GetPersonaByIdAsync(id);
                if (persona != null)
                {
                    personas[id] = persona;
                }
            }

            var userDtos = pagedUsers.Items.Select(u =>
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

            return new PagedList<UserDto>(userDtos, pagedUsers.TotalCount, pagedUsers.CurrentPage, pagedUsers.PageSize);
        }, "getting all users");

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserRequest request) => await ExecuteServiceOperationAsync(async () =>
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                throw new BusinessLogicException($"User with ID {id} not found.");
            }

            var persona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona == null)
            {
                throw new BusinessLogicException($"Persona not found for user ID: {id}.");
            }

            // Update Persona entity
            persona.Update(
                persona.Legajo,
                request.Nombre ?? persona.Nombre,
                request.Apellido ?? persona.Apellido,
                persona.IdTipoDoc,
                persona.NumDoc,
                persona.FechaNacimiento,
                persona.Cuil,
                persona.Calle,
                persona.Altura,
                persona.IdLocalidad,
                persona.IdGenero,
                request.Correo ?? persona.Correo,
                persona.Celular,
                persona.FechaIngreso
            );
            await _personaRepository.UpdatePersonaAsync(persona);

            // The admin username should come from the current session context in a real app
            const string adminUsername = "Admin";

            // Apply updates to the User entity
            usuario.ChangeRole(request.IdRol);
            usuario.SetExpiration(request.FechaExpiracion);
            usuario.ForcePasswordChange(request.CambioContrasenaObligatorio);

            if (request.Habilitado)
            {
                usuario.Habilitar();
            }
            else
            {
                usuario.Deshabilitar(adminUsername);
            }

            await _userRepository.UpdateUsuarioAsync(usuario);

            var updatedDto = UserMapper.MapToUserDto(usuario);
            var updatedPersona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona);
            updatedDto.Nombre = updatedPersona.Nombre;
            updatedDto.Apellido = updatedPersona.Apellido;
            updatedDto.Correo = updatedPersona.Correo;

            return updatedDto;
        }, "updating user");

        public async Task DeleteUserAsync(int userId) => await ExecuteServiceOperationAsync(async () =>
            await _userRepository.DeleteUsuarioAsync(userId),
            "deleting user");

        public async Task<UserDto?> GetUserByUsernameAsync(string username) => await ExecuteServiceOperationAsync(async () =>
        {
            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username);
            return UserMapper.MapToUserDto(usuario);
        }, "getting user by username");

        public async Task<UserDto?> GetUserByIdAsync(int id) => await ExecuteServiceOperationAsync(async () =>
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            return UserMapper.MapToUserDto(usuario);
        }, "getting user by id");

        public async Task<UpdateUserRequest?> GetUserForUpdateAsync(int id) => await ExecuteServiceOperationAsync(async () =>
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            if (usuario == null) return null;

            var persona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona == null) return null;

            return UserMapper.MapToUpdateUserRequest(usuario, persona);

        }, "getting user for update");
    }
}

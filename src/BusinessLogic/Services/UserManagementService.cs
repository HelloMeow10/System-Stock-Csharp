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
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;

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
        private readonly IMapper _mapper;


        public UserManagementService(
            IUserRepository userRepository,
            IPersonaRepository personaRepository,
            IEmailService emailService,
            ILogger<UserManagementService> logger,
            IUsuarioFactory usuarioFactory,
            IPasswordHasher passwordHasher,
            IPersonaService personaService,
            IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _usuarioFactory = usuarioFactory ?? throw new ArgumentNullException(nameof(usuarioFactory));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _personaService = personaService ?? throw new ArgumentNullException(nameof(personaService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<UserDto> CreateUserAsync(UserRequest request)
        {
            var (usuario, plainPassword) = await _usuarioFactory.Create(request);

            await _userRepository.AddUsuarioAsync(usuario);

            var persona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona is null)
            {
                throw new NotFoundException($"Persona with ID {usuario.IdPersona} not found after user creation.");
            }

            await _emailService.SendWelcomeEmailAsync(persona.Correo!, usuario.UsuarioNombre, plainPassword);

            var personaDto = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);

            return _mapper.Map<UserDto>((usuario, personaDto));
        }

        public async Task<UserDtoV2> CreateUserAsyncV2(UserRequestV2 request)
        {
            var (usuario, plainPassword) = await _usuarioFactory.CreateV2(request);

            // The factory already adds the persona, so we just need to add the user
            await _userRepository.AddUsuarioAsync(usuario);

            var persona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona is null)
            {
                throw new NotFoundException($"Persona with ID {usuario.IdPersona} not found after user creation.");
            }

            await _emailService.SendWelcomeEmailAsync(persona.Correo!, usuario.UsuarioNombre, plainPassword);

            var personaDto = await _personaService.GetPersonaByIdAsync(persona.IdPersona);

            return _mapper.Map<UserDtoV2>((usuario, personaDto));
        }

        public async Task<PagedResponse<T>> GetUsersAsync<T>(UserQueryParameters queryParameters) where T : class
        {
            var pagedUsers = await _userRepository.GetUsersAsync(queryParameters);

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
                personas.TryGetValue(u.IdPersona, out var persona);
                return _mapper.Map<T>((u, persona));
            }).ToList();

            return pagedUsers.ToPagedResponse(userDtos);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserRequest updateUserRequest)
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                throw new NotFoundException($"User with ID {id} not found.");
            }

            var persona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona == null)
            {
                throw new NotFoundException($"Persona not found for user ID: {id}.");
            }

            persona.Update(
                persona.Legajo,
                updateUserRequest.Nombre ?? persona.Nombre,
                updateUserRequest.Apellido ?? persona.Apellido,
                persona.IdTipoDoc,
                persona.NumDoc,
                persona.FechaNacimiento,
                persona.Cuil,
                persona.Calle,
                persona.Altura,
                persona.IdLocalidad,
                persona.IdGenero,
                updateUserRequest.Correo ?? persona.Correo,
                persona.Celular,
                persona.FechaIngreso
            );
            await _personaRepository.UpdatePersonaAsync(persona);

            const string adminUsername = "Admin";

            usuario.ChangeRole(updateUserRequest.IdRol);
            usuario.SetExpiration(updateUserRequest.FechaExpiracion);
            usuario.ForcePasswordChange(updateUserRequest.CambioContrasenaObligatorio);

            if (updateUserRequest.Habilitado)
            {
                usuario.Habilitar();
            }
            else
            {
                usuario.Deshabilitar(adminUsername);
            }

            await _userRepository.UpdateUsuarioAsync(usuario);

            var updatedPersonaDto = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);

            return _mapper.Map<UserDto>((usuario, updatedPersonaDto));
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetUsuarioByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }
            await _userRepository.DeleteUsuarioAsync(userId);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username);
            if (usuario == null)
            {
                throw new NotFoundException($"User with username '{username}' not found.");
            }

            var persona = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);

            return _mapper.Map<UserDto>((usuario, persona));
        }

        public async Task<T> GetUserByIdAsync<T>(int id) where T : class
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                throw new NotFoundException($"User with ID {id} not found.");
            }

            var persona = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona == null)
            {
                // Depending on requirements, you might throw or just proceed with a null persona
                _logger.LogWarning("Persona not found for user ID: {UserId}", id);
            }

            return _mapper.Map<T>((usuario, persona));
        }

        public async Task<UserDtoV2> UpdateUserAsyncV2(int id, UpdateUserRequestV2 updateUserRequest)
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                throw new NotFoundException($"User with ID {id} not found.");
            }

            var persona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona == null)
            {
                throw new NotFoundException($"Persona not found for user ID: {id}.");
            }

            var nameParts = updateUserRequest.FullName?.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            var firstName = nameParts?.Length > 0 ? nameParts[0] : persona.Nombre;
            var lastName = nameParts?.Length > 1 ? nameParts[1] : persona.Apellido;

            persona.Update(
                persona.Legajo,
                firstName,
                lastName,
                persona.IdTipoDoc,
                persona.NumDoc,
                persona.FechaNacimiento,
                persona.Cuil,
                persona.Calle,
                persona.Altura,
                persona.IdLocalidad,
                persona.IdGenero,
                updateUserRequest.Correo ?? persona.Correo,
                persona.Celular,
                persona.FechaIngreso
            );
            await _personaRepository.UpdatePersonaAsync(persona);

            const string adminUsername = "Admin";

            usuario.ChangeRole(updateUserRequest.IdRol);
            usuario.SetExpiration(updateUserRequest.FechaExpiracion);
            usuario.ForcePasswordChange(updateUserRequest.CambioContrasenaObligatorio);

            if (updateUserRequest.Habilitado)
            {
                usuario.Habilitar();
            }
            else
            {
                usuario.Deshabilitar(adminUsername);
            }

            await _userRepository.UpdateUsuarioAsync(usuario);

            return await GetUserByIdAsync<UserDtoV2>(id);
        }
    }
}
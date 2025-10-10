using AutoMapper;
using BusinessLogic.Exceptions;
using BusinessLogic.Factories;
using BusinessLogic.Mappers;
using BusinessLogic.Services;
using Contracts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserManagementService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserManagementService> _logger;
        private readonly IUsuarioFactory _usuarioFactory;
        private readonly IPersonaService _personaService;
        private readonly IMapper _mapper;

        public UserManagementService(
            IUserRepository userRepository,
            IPersonaRepository personaRepository,
            IEmailService emailService,
            ILogger<UserManagementService> logger,
            IUsuarioFactory usuarioFactory,
            IPersonaService personaService,
            IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _usuarioFactory = usuarioFactory ?? throw new ArgumentNullException(nameof(usuarioFactory));
            _personaService = personaService ?? throw new ArgumentNullException(nameof(personaService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TResponse> CreateUserAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            var (usuario, plainPassword) = await _usuarioFactory.CreateAsync(request);

            await _userRepository.AddUsuarioAsync(usuario);

            var persona = await _personaRepository.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona is null)
            {
                throw new BusinessLogicException($"Persona with ID {usuario.IdPersona} not found after user creation.");
            }

            await _emailService.SendWelcomeEmailAsync(persona.Correo!, usuario.UsuarioNombre, plainPassword);

            var personaDto = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);

            return _mapper.Map<TResponse>((usuario, personaDto));
        }

        public async Task<TResponse> UpdateUserAsync<TRequest, TResponse>(int id, TRequest request)
            where TRequest : class
            where TResponse : class
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

            // Map the update request to the existing entities
            _mapper.Map(request, usuario);
            _mapper.Map(request, persona);

            await _personaRepository.UpdatePersonaAsync(persona);
            await _userRepository.UpdateUsuarioAsync(usuario);

            var updatedPersonaDto = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);

            return _mapper.Map<TResponse>((usuario, updatedPersonaDto));
        }

        public async Task<TResponse> PatchUserAsync<TRequest, TResponse>(int id, JsonPatchDocument<TRequest> patchDoc)
            where TRequest : class
            where TResponse : class
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

            var userToPatch = _mapper.Map<TRequest>((usuario, persona));

            patchDoc.ApplyTo(userToPatch);

            var validationContext = new ValidationContext(userToPatch, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(userToPatch, validationContext, validationResults, validateAllProperties: true))
            {
                var errors = validationResults.Select(r => r.ErrorMessage ?? "Validation error");
                throw new BusinessLogic.Exceptions.ValidationException(errors);
            }

            _mapper.Map(userToPatch, usuario);
            _mapper.Map(userToPatch, persona);

            await _personaRepository.UpdatePersonaAsync(persona);
            await _userRepository.UpdateUsuarioAsync(usuario);

            var updatedPersonaDto = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);

            return _mapper.Map<TResponse>((usuario, updatedPersonaDto));
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

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetUsuarioByIdAsync(userId);
            if (user == null)
            {
                throw new BusinessLogicException($"User with ID {userId} not found.");
            }
            await _userRepository.DeleteUsuarioAsync(userId);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username);
            if (usuario == null)
            {
                throw new BusinessLogicException($"User with username '{username}' not found.");
            }

            var persona = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);

            return _mapper.Map<UserDto>((usuario, persona));
        }

        public async Task<T> GetUserByIdAsync<T>(int id) where T : class
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                throw new BusinessLogicException($"User with ID {id} not found.");
            }

            var persona = await _personaService.GetPersonaByIdAsync(usuario.IdPersona);
            if (persona == null)
            {
                _logger.LogWarning("Persona not found for user ID: {UserId}", id);
            }

            return _mapper.Map<T>((usuario, persona));
        }
    }
}
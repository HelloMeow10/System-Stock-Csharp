using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Exceptions;
using BusinessLogic.Factories;
using BusinessLogic.Mappers;
using Contracts;
using SharedKernel;
using DataAccess.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly ILogger<PersonaService> _logger;
        private readonly IPersonaFactory _personaFactory;
        private readonly IMapper _mapper;

        public PersonaService(
            IPersonaRepository personaRepository,
            ILogger<PersonaService> logger,
            IPersonaFactory personaFactory,
            IMapper mapper)
        {
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _personaFactory = personaFactory ?? throw new ArgumentNullException(nameof(personaFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PersonaDto> CreatePersonaAsync(PersonaRequest request)
        {
            _logger.LogInformation("Iniciando la creación de la persona con legajo: {Legajo}", request.Legajo);
            var persona = await _personaFactory.CreateAsync(request);
            _logger.LogInformation("Llamando a AddPersonaAsync en el repositorio.");
            await _personaRepository.AddPersonaAsync(persona);
            _logger.LogInformation("Persona creada con éxito en el repositorio.");
            return _mapper.Map<PersonaDto>(persona);
        }

        public async Task<PersonaDto> UpdatePersonaAsync(int id, UpdatePersonaRequest request)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID: {PersonaId} para actualizar.", id);
                throw new NotFoundException($"Persona with ID {id} not found.");
            }

            _mapper.Map(request, persona);

            await _personaRepository.UpdatePersonaAsync(persona);

            return _mapper.Map<PersonaDto>(persona);
        }

        public async Task<PersonaDto> PatchPersonaAsync(int id, JsonPatchDocument<UpdatePersonaRequest> patchDoc)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                throw new NotFoundException($"Persona with ID {id} not found.");
            }

            var personaToPatch = _mapper.Map<UpdatePersonaRequest>(persona);
            patchDoc.ApplyTo(personaToPatch);

            var validationContext = new ValidationContext(personaToPatch, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(personaToPatch, validationContext, validationResults, validateAllProperties: true))
            {
                var errors = validationResults.Select(r => r.ErrorMessage ?? "Validation error");
                throw new BusinessLogic.Exceptions.ValidationException(errors);
            }

            _mapper.Map(personaToPatch, persona);

            await _personaRepository.UpdatePersonaAsync(persona);

            return _mapper.Map<PersonaDto>(persona);
        }

        public async Task DeletePersonaAsync(int personaId)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(personaId);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID: {PersonaId} para eliminar.", personaId);
                throw new NotFoundException($"Persona with ID {personaId} not found.");
            }
            await _personaRepository.DeletePersonaAsync(personaId);
        }

        public async Task<PagedResponse<PersonaDto>> GetPersonasAsync(PaginationParams paginationParams)
        {
            var pagedPersonas = await _personaRepository.GetPersonasAsync(paginationParams);
            var personaDtos = _mapper.Map<List<PersonaDto>>(pagedPersonas.Items);
            return pagedPersonas.ToPagedResponse(personaDtos);
        }

        public async Task<PersonaDto> GetPersonaByIdAsync(int personaId)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(personaId);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID: {PersonaId}.", personaId);
                throw new NotFoundException($"Persona with ID {personaId} not found.");
            }
            return _mapper.Map<PersonaDto>(persona);
        }
    }
}

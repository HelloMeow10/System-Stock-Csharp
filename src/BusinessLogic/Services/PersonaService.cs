using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using BusinessLogic.Factories;
using BusinessLogic.Mappers;
using Contracts;
using SharedKernel;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly ILogger<PersonaService> _logger;
        private readonly IPersonaFactory _personaFactory;

        public PersonaService(
            IPersonaRepository personaRepository,
            ILogger<PersonaService> logger,
            IPersonaFactory personaFactory)
        {
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _personaFactory = personaFactory ?? throw new ArgumentNullException(nameof(personaFactory));
        }

        public async Task<PersonaDto> CreatePersonaAsync(PersonaRequest request)
        {
            _logger.LogInformation("Iniciando la creación de la persona con legajo: {Legajo}", request.Legajo);
            var persona = _personaFactory.Create(request);
            _logger.LogInformation("Llamando a AddPersonaAsync en el repositorio.");
            await _personaRepository.AddPersonaAsync(persona);
            _logger.LogInformation("Persona creada con éxito en el repositorio.");
            return PersonaMapper.MapToPersonaDto(persona)!;
        }

        public async Task<PersonaDto> UpdatePersonaAsync(int id, UpdatePersonaRequest personaDto)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID: {PersonaId} para actualizar.", id);
                throw new BusinessLogicException($"Resource not found.");
            }

            // Legajo and FechaIngreso are not part of UpdatePersonaRequest, so we keep the existing ones.
            // FechaNacimiento and Cuil are nullable in UpdatePersonaRequest, so we use existing if not provided.
            persona.Update(
                persona.Legajo,
                personaDto.Nombre,
                personaDto.Apellido,
                personaDto.IdTipoDoc,
                personaDto.NumDoc,
                personaDto.FechaNacimiento ?? persona.FechaNacimiento,
                personaDto.Cuil ?? persona.Cuil,
                personaDto.Calle ?? persona.Calle,
                personaDto.Altura ?? persona.Altura,
                personaDto.IdLocalidad,
                personaDto.IdGenero,
                personaDto.Correo ?? persona.Correo,
                personaDto.Celular ?? persona.Celular,
                persona.FechaIngreso
            );

            await _personaRepository.UpdatePersonaAsync(persona);

            return PersonaMapper.MapToPersonaDto(persona)!;
        }

        public async Task DeletePersonaAsync(int personaId)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(personaId);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID: {PersonaId} para eliminar.", personaId);
                throw new BusinessLogicException($"Resource not found.");
            }
            await _personaRepository.DeletePersonaAsync(personaId);
        }

        public async Task<PagedList<PersonaDto>> GetPersonasAsync(PaginationParams paginationParams)
        {
            var pagedPersonas = await _personaRepository.GetPersonasAsync(paginationParams);
            var personaDtos = pagedPersonas.Items.Select(p => PersonaMapper.MapToPersonaDto(p)!).ToList();
            return new PagedList<PersonaDto>(personaDtos, pagedPersonas.TotalCount, pagedPersonas.CurrentPage, pagedPersonas.PageSize);
        }

        public async Task<PersonaDto> GetPersonaByIdAsync(int personaId)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(personaId);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID: {PersonaId}.", personaId);
                throw new BusinessLogicException("Resource not found");
            }
            return PersonaMapper.MapToPersonaDto(persona)!;
        }

        public async Task<PersonaDto> PatchPersonaAsync(int id, JsonPatchDocument<UpdatePersonaRequest> patchDoc)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID: {PersonaId} para actualizar.", id);
                throw new BusinessLogicException($"Resource not found.");
            }

            var personaToPatch = PersonaMapper.MapToUpdatePersonaRequest(persona);

            patchDoc.ApplyTo(personaToPatch);

            var validationResults = new System.Collections.Generic.List<ValidationResult>();
            var validationContext = new ValidationContext(personaToPatch, null, null);
            if (!Validator.TryValidateObject(personaToPatch, validationContext, validationResults, true))
            {
                var errors = validationResults.Select(r => r.ErrorMessage!);
                throw new BusinessLogic.Exceptions.ValidationException(errors);
            }

            return await UpdatePersonaAsync(id, personaToPatch);
        }
    }
}

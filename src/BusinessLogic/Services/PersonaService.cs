using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Factories;
using BusinessLogic.Mappers;
using BusinessLogic.Models;
using SharedKernel;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;

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

        public async Task<PersonaDto> UpdatePersonaAsync(int id, UpdatePersonaRequest request)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID: {PersonaId} para actualizar.", id);
                throw new KeyNotFoundException($"Persona with ID {id} not found.");
            }

            // Use the entity's Update method
            persona.Update(
                persona.Legajo, // Legajo is not updatable
                request.Nombre,
                request.Apellido,
                request.IdTipoDoc,
                request.NumDoc,
                request.FechaNacimiento,
                request.Cuil,
                request.Calle,
                request.Altura,
                request.IdLocalidad,
                request.IdGenero,
                request.Correo,
                request.Celular,
                persona.FechaIngreso // FechaIngreso is not updatable
            );

            await _personaRepository.UpdatePersonaAsync(persona);

            return PersonaMapper.MapToPersonaDto(persona)!;
        }

        public async Task DeletePersonaAsync(int personaId)
        {
            await _personaRepository.DeletePersonaAsync(personaId);
        }

        public async Task<PagedList<PersonaDto>> GetPersonasAsync(PaginationParams paginationParams)
        {
            var pagedPersonas = await _personaRepository.GetPersonasAsync(paginationParams);
            var personaDtos = pagedPersonas.Items.Select(p => PersonaMapper.MapToPersonaDto(p)!).ToList();
            return new PagedList<PersonaDto>(personaDtos, pagedPersonas.TotalCount, pagedPersonas.CurrentPage, pagedPersonas.PageSize);
        }

        public async Task<PersonaDto?> GetPersonaByIdAsync(int personaId)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(personaId);
            return PersonaMapper.MapToPersonaDto(persona);
        }

        public async Task<UpdatePersonaRequest?> GetPersonaForUpdateAsync(int id)
        {
            var persona = await _personaRepository.GetPersonaByIdAsync(id);
            if (persona == null) return null;
            return PersonaMapper.MapToUpdatePersonaRequest(persona);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using BusinessLogic.Factories;
using BusinessLogic.Mappers;
using BusinessLogic.Models;
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

        public Task CrearPersonaAsync(PersonaRequest request)
        {
            ExecuteServiceOperation(() =>
            {
                _logger.LogInformation("Iniciando la creación de la persona con legajo: {Legajo}", request.Legajo);
                var persona = _personaFactory.Create(request);
                _logger.LogInformation("Llamando a AddPersona en el repositorio.");
                _personaRepository.AddPersona(persona);
                _logger.LogInformation("Persona creada con éxito en el repositorio.");
            }, "creating a person");
            return Task.CompletedTask;
        }

        public Task UpdatePersonaAsync(PersonaDto personaDto)
        {
            ExecuteServiceOperation(() =>
            {
                var persona = _personaRepository.GetPersonaById(personaDto.IdPersona)
                    ?? throw new ValidationException($"Persona with id {personaDto.IdPersona} not found");

                persona.Update(personaDto.Legajo, personaDto.Nombre, personaDto.Apellido, personaDto.IdTipoDoc, personaDto.NumDoc, personaDto.FechaNacimiento, personaDto.Cuil, personaDto.Calle, personaDto.Altura, personaDto.IdLocalidad, personaDto.IdGenero, personaDto.Correo, personaDto.Celular, personaDto.FechaIngreso);

                _personaRepository.UpdatePersona(persona);
            }, "updating persona");
            return Task.CompletedTask;
        }

        public Task DeletePersonaAsync(int personaId)
        {
            ExecuteServiceOperation(() => _personaRepository.DeletePersona(personaId), "deleting persona");
            return Task.CompletedTask;
        }

        public Task<List<PersonaDto>> GetPersonasAsync()
        {
            return Task.FromResult(ExecuteServiceOperation(() =>
                _personaRepository.GetAllPersonas().Select(p => PersonaMapper.MapToPersonaDto(p)!).ToList(),
                "getting all people"));
        }

        public Task<PersonaDto?> GetPersonaByIdAsync(int personaId)
        {
            return Task.FromResult(ExecuteServiceOperation(() =>
            {
                var persona = _personaRepository.GetPersonaById(personaId);
                return PersonaMapper.MapToPersonaDto(persona);
            }, "getting persona by id"));
        }
    }
}

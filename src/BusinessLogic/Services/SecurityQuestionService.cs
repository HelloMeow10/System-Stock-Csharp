using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Contracts;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityRepository _securityRepository;
        private readonly ILogger<SecurityQuestionService> _logger;

        public SecurityQuestionService(
            IUserRepository userRepository,
            ISecurityRepository securityRepository,
            ILogger<SecurityQuestionService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SaveSecurityAnswersAsync(string username, Dictionary<int, string> answers) => await ExecuteServiceOperationAsync(async () =>
        {
            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username)
                ?? throw new ValidationException($"Usuario '{username}' not found");

            var politica = await _securityRepository.GetPoliticaSeguridadAsync() ?? throw new BusinessLogicException("Security policy not configured.");
            if (answers.Count != politica.CantPreguntas)
                throw new ValidationException($"Se requieren exactamente {politica.CantPreguntas} respuestas de seguridad.");

            await _securityRepository.DeleteRespuestasSeguridadByUsuarioIdAsync(usuario.IdUsuario);

            foreach (var pair in answers)
            {
                var respuesta = new RespuestaSeguridad
                {
                    IdUsuario = usuario.IdUsuario,
                    IdPregunta = pair.Key,
                    Respuesta = pair.Value
                };
                await _securityRepository.AddRespuestaSeguridadAsync(respuesta);
            }
        }, "saving security answers");

        public async Task<List<PreguntaSeguridadDto>> GetSecurityQuestionsAsync() => await ExecuteServiceOperationAsync(async () =>
        {
            var questions = await _securityRepository.GetPreguntasSeguridadAsync();
            return questions.Select(p => new PreguntaSeguridadDto { IdPregunta = p.IdPregunta, Pregunta = p.Pregunta }).ToList();
        }, "getting security questions");

        public async Task<List<PreguntaSeguridadDto>> GetUserSecurityQuestionsAsync(string username) => await ExecuteServiceOperationAsync(async () =>
        {
            var usuario = await _userRepository.GetUsuarioByNombreUsuarioAsync(username)
                ?? throw new ValidationException($"Usuario '{username}' not found");

            var respuestas = await _securityRepository.GetRespuestasSeguridadByUsuarioIdAsync(usuario.IdUsuario)
                ?? throw new ValidationException("No se han configurado las preguntas de seguridad.");

            if (!respuestas.Any())
                throw new ValidationException("No se han configurado las preguntas de seguridad.");

            var idPreguntas = respuestas.Select(r => r.IdPregunta).ToList();
            var preguntas = await _securityRepository.GetPreguntasSeguridadByIdsAsync(idPreguntas);

            return preguntas.Select(p => new PreguntaSeguridadDto { IdPregunta = p.IdPregunta, Pregunta = p.Pregunta }).ToList();
        }, "getting user security questions");

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
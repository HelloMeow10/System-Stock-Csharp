using System;
using BusinessLogic.Exceptions;
using BusinessLogic.Mappers;
using BusinessLogic.Models;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class SecurityPolicyService : ISecurityPolicyService
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly ILogger<SecurityPolicyService> _logger;

        public SecurityPolicyService(ISecurityRepository securityRepository, ILogger<SecurityPolicyService> logger)
        {
            _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        public async Task<PoliticaSeguridadDto?> GetPoliticaSeguridadAsync() => await ExecuteServiceOperationAsync(async () =>
        {
            var politica = await _securityRepository.GetPoliticaSeguridadAsync();
            return PoliticaSeguridadMapper.MapToPoliticaSeguridadDto(politica);
        }, "getting security policy");

        public async Task UpdatePoliticaSeguridadAsync(UpdatePoliticaSeguridadRequest request) => await ExecuteServiceOperationAsync(async () =>
        {
            var politica = await _securityRepository.GetPoliticaSeguridadAsync()
                ?? throw new ValidationException("No se encontró la política de seguridad para actualizar.");

            politica.Update(request.MayusYMinus, request.LetrasYNumeros, request.CaracterEspecial, request.Autenticacion2FA, request.NoRepetirAnteriores, request.SinDatosPersonales, request.MinCaracteres, request.CantPreguntas);

            await _securityRepository.UpdatePoliticaSeguridadAsync(politica);

        }, "updating security policy");
    }
}

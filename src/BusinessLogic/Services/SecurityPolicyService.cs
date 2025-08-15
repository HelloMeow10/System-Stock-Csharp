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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during {OperationName}", operationName);
                throw new BusinessLogicException($"An unexpected error occurred during {operationName}.", ex);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during {OperationName}", operationName);
                throw new BusinessLogicException($"An unexpected error occurred during {operationName}.", ex);
            }
        }

        public PoliticaSeguridadDto? GetPoliticaSeguridad() => ExecuteServiceOperation(() =>
        {
            var politica = _securityRepository.GetPoliticaSeguridad();
            return PoliticaSeguridadMapper.MapToPoliticaSeguridadDto(politica);
        }, "getting security policy");

        public void UpdatePoliticaSeguridad(PoliticaSeguridadDto politicaDto) => ExecuteServiceOperation(() =>
        {
            var politica = _securityRepository.GetPoliticaSeguridad()
                ?? throw new ValidationException("No se encontró la política de seguridad para actualizar.");

            politica.Update(politicaDto.MayusYMinus, politicaDto.LetrasYNumeros, politicaDto.CaracterEspecial, politicaDto.Autenticacion2FA, politicaDto.NoRepetirAnteriores, politicaDto.SinDatosPersonales, politicaDto.MinCaracteres, politicaDto.CantPreguntas);

            _securityRepository.UpdatePoliticaSeguridad(politica);
        }, "updating security policy");
    }
}

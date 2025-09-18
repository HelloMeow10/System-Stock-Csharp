using System;
using BusinessLogic.Exceptions;
using BusinessLogic.Mappers;
using Contracts;
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


        public async Task<PoliticaSeguridadDto> GetPoliticaSeguridadAsync()
        {
            var politica = await _securityRepository.GetPoliticaSeguridadAsync();
            if (politica == null)
            {
                _logger.LogWarning("Security policy not found.");
                throw new NotFoundException("Security policy not found.");
            }
            return PoliticaSeguridadMapper.MapToPoliticaSeguridadDto(politica)!;
        }

        public async Task<PoliticaSeguridadDto> UpdatePoliticaSeguridadAsync(UpdatePoliticaSeguridadRequest request)
        {
            var politica = await _securityRepository.GetPoliticaSeguridadAsync()
                ?? throw new NotFoundException("Security policy not found.");

            politica.Update(request.MayusYMinus, request.LetrasYNumeros, request.CaracterEspecial, request.Autenticacion2FA, request.NoRepetirAnteriores, request.SinDatosPersonales, request.MinCaracteres, request.CantPreguntas);

            await _securityRepository.UpdatePoliticaSeguridadAsync(politica);

            return PoliticaSeguridadMapper.MapToPoliticaSeguridadDto(politica)!;

        }
    }
}

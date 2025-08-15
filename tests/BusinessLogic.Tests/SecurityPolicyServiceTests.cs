using Xunit;
using Moq;
using BusinessLogic.Services;
using BusinessLogic.Models;
using DataAccess.Repositories;
using DataAccess.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Tests
{
    public class SecurityPolicyServiceTests
    {
        private readonly Mock<ISecurityRepository> _securityRepositoryMock;
        private readonly Mock<ILogger<SecurityPolicyService>> _loggerMock;
        private readonly SecurityPolicyService _sut;

        public SecurityPolicyServiceTests()
        {
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _loggerMock = new Mock<ILogger<SecurityPolicyService>>();
            _sut = new SecurityPolicyService(_securityRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void UpdatePoliticaSeguridad_WhenPolicyExists_CallsRepository()
        {
            // Arrange
            var politicaDto = new PoliticaSeguridadDto { IdPolitica = 1, MinCaracteres = 10, CantPreguntas = 3 };
            var politica = new PoliticaSeguridad(1, true, true, true, true, true, true, 8, 3);
            _securityRepositoryMock.Setup(r => r.GetPoliticaSeguridad()).Returns(politica);

            // Act
            _sut.UpdatePoliticaSeguridad(politicaDto);

            // Assert
            _securityRepositoryMock.Verify(r => r.UpdatePoliticaSeguridad(It.Is<PoliticaSeguridad>(p => p.MinCaracteres == 10)), Times.Once);
        }
    }
}

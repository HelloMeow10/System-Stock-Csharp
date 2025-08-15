using Xunit;
using Moq;
using System.Threading.Tasks;
using BusinessLogic.Services;
using BusinessLogic.Models;
using BusinessLogic.Factories;
using DataAccess.Repositories;
using DataAccess.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Tests
{
    public class PersonaServiceTests
    {
        private readonly Mock<IPersonaRepository> _personaRepositoryMock;
        private readonly Mock<ILogger<PersonaService>> _loggerMock;
        private readonly Mock<IPersonaFactory> _personaFactoryMock;
        private readonly PersonaService _sut;

        public PersonaServiceTests()
        {
            _personaRepositoryMock = new Mock<IPersonaRepository>();
            _loggerMock = new Mock<ILogger<PersonaService>>();
            _personaFactoryMock = new Mock<IPersonaFactory>();
            _sut = new PersonaService(
                _personaRepositoryMock.Object,
                _loggerMock.Object,
                _personaFactoryMock.Object
            );
        }

        [Fact]
        public async Task CrearPersonaAsync_WithValidData_CallsFactoryAndRepository()
        {
            // Arrange
            var personaRequest = new PersonaRequest { Nombre = "Test", Apellido = "Person" };
            var persona = new Persona(1, "Test", "Person", 1, "12345678", System.DateTime.Now, "20123456789", "Test Street", "123", 1, 1, "test@example.com", "1234567890", System.DateTime.Now);
            _personaFactoryMock.Setup(f => f.Create(personaRequest)).Returns(persona);
            _personaRepositoryMock.Setup(r => r.AddPersona(persona));

            // Act
            await _sut.CrearPersonaAsync(personaRequest);

            // Assert
            _personaFactoryMock.Verify(f => f.Create(personaRequest), Times.Once);
            _personaRepositoryMock.Verify(r => r.AddPersona(persona), Times.Once);
        }
    }
}

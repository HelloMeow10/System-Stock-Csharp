using BusinessLogic.Services;
using BusinessLogic.Security;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Contracts;
using SharedKernel;
using DataAccess.Entities;
using System.Threading.Tasks;
using BusinessLogic.Factories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Tests
{
    public class UserManagementServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPersonaRepository> _personaRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ILogger<UserManagementService>> _loggerMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IUsuarioFactory> _usuarioFactoryMock;
        private readonly Mock<IPersonaService> _personaServiceMock;
        private readonly UserManagementService _sut; // System Under Test

        public UserManagementServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _personaRepositoryMock = new Mock<IPersonaRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _loggerMock = new Mock<ILogger<UserManagementService>>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _usuarioFactoryMock = new Mock<IUsuarioFactory>();
            _personaServiceMock = new Mock<IPersonaService>();

            _sut = new UserManagementService(
                _userRepositoryMock.Object,
                _personaRepositoryMock.Object,
                _emailServiceMock.Object,
                _loggerMock.Object,
                _usuarioFactoryMock.Object,
                _passwordHasherMock.Object,
                _personaServiceMock.Object
            );
        }

        [Fact]
        public async Task CrearUsuarioAsync_WithValidData_ShouldCallAddUsuarioAndSendEmail()
        {
            // Arrange
            var userRequest = new UserRequest { PersonaId = "1", Username = "testuser", Rol = "Usuario" };
            var persona = new Persona(1, "Test", "User", 1, "12345678", System.DateTime.Now, "20123456789", "Test Street", "123", 1, 1, "test@example.com", "1234567890", System.DateTime.Now);
            var usuario = new Usuario("testuser", new byte[0], 1, 1, 1);
            var plainPassword = "plainPassword123";

            _usuarioFactoryMock.Setup(f => f.Create(userRequest)).ReturnsAsync((usuario, plainPassword));
            _personaRepositoryMock.Setup(r => r.GetPersonaByIdAsync(1)).ReturnsAsync(persona);
            _userRepositoryMock.Setup(r => r.AddUsuarioAsync(usuario)).Returns(Task.CompletedTask);
            _emailServiceMock.Setup(s => s.SendWelcomeEmailAsync(persona.Correo!, usuario.UsuarioNombre, plainPassword)).Returns(Task.CompletedTask);

            // Act
            await _sut.CreateUserAsync(userRequest);

            // Assert
            _userRepositoryMock.Verify(r => r.AddUsuarioAsync(usuario), Times.Once);
            _emailServiceMock.Verify(s => s.SendWelcomeEmailAsync(persona.Correo!, usuario.UsuarioNombre, plainPassword), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserExists_ShouldUpdateAndReturnDto()
        {
            // Arrange
            var userId = 100;
            var personaId = 1;
            var request = new UpdateUserRequest
            {
                Nombre = "New",
                Apellido = "Name",
                Correo = "new@test.com",
                IdRol = 2,
                Habilitado = false,
                CambioContrasenaObligatorio = true,
                FechaExpiracion = new DateTime(2025, 1, 1)
            };
            var rol = new Rol { IdRol = 1, Nombre = "User" };
            var usuario = new Usuario(userId, "testuser", new byte[0], personaId, DateTime.MaxValue, null, DateTime.Now, 1, 1, false, null, null, null, rol);
            var persona = new Persona(personaId, "Old", "Name", 1, "123", null, null, null, null, 1, 1, "old@test.com", null, DateTime.Now);

            _userRepositoryMock.Setup(r => r.GetUsuarioByIdAsync(userId)).ReturnsAsync(usuario);
            _personaRepositoryMock.Setup(r => r.GetPersonaByIdAsync(personaId)).ReturnsAsync(persona);
            _userRepositoryMock.Setup(r => r.UpdateUsuarioAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
            _personaRepositoryMock.Setup(r => r.UpdatePersonaAsync(It.IsAny<Persona>())).Returns(Task.CompletedTask);
            _personaRepositoryMock.Setup(r => r.GetPersonaByIdAsync(personaId)).ReturnsAsync(persona);


            // Act
            var result = await _sut.UpdateUserAsync(userId, request);

            // Assert
            _userRepositoryMock.Verify(r => r.UpdateUsuarioAsync(It.Is<Usuario>(u =>
                u.IdUsuario == userId &&
                u.IdRol == request.IdRol &&
                u.CambioContrasenaObligatorio == request.CambioContrasenaObligatorio &&
                u.FechaExpiracion == request.FechaExpiracion
            )), Times.Once);

            _personaRepositoryMock.Verify(r => r.UpdatePersonaAsync(It.Is<Persona>(p =>
                p.Nombre == request.Nombre &&
                p.Apellido == request.Apellido &&
                p.Correo == request.Correo
            )), Times.Once);

            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var userId = 999;
            var request = new UpdateUserRequest();
            _userRepositoryMock.Setup(r => r.GetUsuarioByIdAsync(userId)).ReturnsAsync((Usuario?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessLogicException>(() => _sut.UpdateUserAsync(userId, request));
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserExists_ShouldCallRepository()
        {
            // Arrange
            int userId = 1;
            var user = new Usuario(userId, "test", new byte[0], 1, DateTime.Now, null, DateTime.Now, 1, 1, false, null, null, null, new Rol());
            _userRepositoryMock.Setup(r => r.GetUsuarioByIdAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.DeleteUsuarioAsync(userId)).Returns(Task.CompletedTask);

            // Act
            await _sut.DeleteUserAsync(userId);

            // Assert
            _userRepositoryMock.Verify(r => r.DeleteUsuarioAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_WhenUsersAndPersonasExist_ReturnsCorrectlyMappedAndPagedDtos()
        {
            // Arrange
            var queryParameters = new UserQueryParameters { PageNumber = 1, PageSize = 10 };
            var rol = new Rol { IdRol = 1, Nombre = "User" };
            var users = new List<Usuario>
            {
                new Usuario(1, "user1", new byte[0], 1, DateTime.Now.AddDays(1), null, DateTime.Now, 1, 1, false, null, null, null, rol),
                new Usuario(2, "user2", new byte[0], 2, DateTime.Now.AddDays(1), null, DateTime.Now, 1, 1, false, null, null, null, rol)
            };
            var pagedUsers = new PagedList<Usuario>(users, users.Count, queryParameters.PageNumber, queryParameters.PageSize);

            var persona1 = new PersonaDto { IdPersona = 1, Nombre = "John", Apellido = "Doe", Correo = "john.doe@test.com" };
            var persona2 = new PersonaDto { IdPersona = 2, Nombre = "Jane", Apellido = "Doe", Correo = "jane.doe@test.com" };

            _userRepositoryMock.Setup(r => r.GetUsersAsync(queryParameters)).ReturnsAsync(pagedUsers);
            _personaServiceMock.Setup(s => s.GetPersonaByIdAsync(1)).ReturnsAsync(persona1);
            _personaServiceMock.Setup(s => s.GetPersonaByIdAsync(2)).ReturnsAsync(persona2);

            // Act
            var result = await _sut.GetUsersAsync(queryParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.CurrentPage);

            var user1 = result.Items.First(u => u.Username == "user1");
            Assert.Equal("John", user1.Nombre);
            Assert.Equal("Doe", user1.Apellido);
            Assert.Equal("john.doe@test.com", user1.Correo);

            var user2 = result.Items.First(u => u.Username == "user2");
            Assert.Equal("Jane", user2.Nombre);
            Assert.Equal("Doe", user2.Apellido);
            Assert.Equal("jane.doe@test.com", user2.Correo);
        }

    }
}

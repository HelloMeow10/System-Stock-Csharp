using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Moq;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;
using Contracts;
using SharedKernel;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;

namespace Services.Tests
{
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_User_By_Id_Returns_NotFound_When_Service_Throws_BusinessLogicException()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetUserByIdAsync<UserDto>(It.IsAny<int>()))
                .ThrowsAsync(new BusinessLogicException("Resource not found"));

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(mockUserService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1.0/users/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Users_Returns_Ok_With_Data()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var users = new List<UserDto> { new UserDto { IdUsuario = 1, Username = "test" } };
            var pagedResponse = new PagedResponse<UserDto>(users, 1, 10, 1);
            mockUserService.Setup(s => s.GetUsersAsync<UserDto>(It.IsAny<UserQueryParameters>()))
                .ReturnsAsync(pagedResponse);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(mockUserService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1.0/users");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
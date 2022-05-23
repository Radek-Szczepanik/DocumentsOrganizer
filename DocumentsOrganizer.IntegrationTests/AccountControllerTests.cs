using DocumentsOrganizer.Entities;
using DocumentsOrganizer.IntegrationTests.Helpers;
using DocumentsOrganizer.Models;
using DocumentsOrganizer.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DocumentsOrganizer.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient client;
        private Mock<IAccountService> accountServiceMock = new Mock<IAccountService>();

        public AccountControllerTests(WebApplicationFactory<Startup> factory)
        {
            this.client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DocumentsOrganizerDbContext>));

                        services.Remove(dbContextOptions);
                        services.AddSingleton<IAccountService>(this.accountServiceMock.Object);
                        services.AddDbContext<DocumentsOrganizerDbContext>(options => options.UseInMemoryDatabase("DocumentsDb"));
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOkResult()
        {
            // arrange

            var registerUser = new RegisterUserDto()
            {
                Email = "test@test.pl",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            // act

            var response = await this.client.PostAsync("/api/account/register", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequestResult()
        {
            // arrange

            var registerUser = new RegisterUserDto()
            {
                Password = "password123",
                ConfirmPassword = "password"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            // act

            var response = await this.client.PostAsync("/api/account/register", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOkResult()
        {
            // arrange

            this.accountServiceMock
                .Setup(x => x.GenerateJwt(It.IsAny<LoginDto>()))
                .Returns("jwt");

            var loginDto = new LoginDto()
            {
                Email = "test@test.pl",
                Password = "password123"
            };

            var httpContent = loginDto.ToJsonHttpContent();

            // act

            var response = await this.client.PostAsync("/api/account/login", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}

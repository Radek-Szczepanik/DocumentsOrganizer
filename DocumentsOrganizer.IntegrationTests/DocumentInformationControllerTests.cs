using DocumentsOrganizer.Entities;
using DocumentsOrganizer.IntegrationTests.Helpers;
using DocumentsOrganizer.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DocumentsOrganizer.IntegrationTests
{
    public class DocumentInformationControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient client;

        public DocumentInformationControllerTests(WebApplicationFactory<Startup> factory)
        {
            client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DocumentsOrganizerDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddDbContext<DocumentsOrganizerDbContext>(options => options.UseInMemoryDatabase("DocumentsDb"));
                    });
                })
                .CreateClient();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task GetAll_WithExistingDocument_ReturnsOkResult(string documentId)
        {
            // act
            var response = await this.client.GetAsync($"api/document/{documentId}/information");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("29")]
        [InlineData("53")]
        public async Task GetAll_WithNonExistingDocument_ReturnsNotFoundResult(string documentId)
        {
            // act
            var response = await this.client.GetAsync($"api/document/{documentId}/information");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("1", "2")]
        public async Task GetById_WithExistingDocumentAndExistingInformation_ReturnsOkResult(string documentId, string informationId)
        {
            // act
            var response = await this.client.GetAsync($"api/document/{documentId}/information/{informationId}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("1", "12")]
        public async Task GetById_WithExistingDocumentAndNotExistingInformation_ReturnsNotFoundResult(string documentId, string informationId)
        {
            // act
            var response = await this.client.GetAsync($"api/document/{documentId}/information/{informationId}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("8", "7")]
        public async Task GetById_WithNonExistingDocumentAndNotExistingInformation_ReturnsNotFoundResult(string documentId, string informationId)
        {
            // act
            var response = await this.client.GetAsync($"api/document/{documentId}/information/{informationId}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("1")]
        public async Task CreateInformation_WithValidModelAndExistingDocument_ReturnsCreatedResult(string documentId)
        {
            // arrange
            var model = new DocumentInformationDto()
            {
                Description = "test"
            };

            var httpContent = model.ToJsonHttpContent();

            // act
            var response = await this.client.PostAsync($"/api/document/{documentId}/information", httpContent);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Theory]
        [InlineData("1")]
        public async Task CreateInformation_WithInvalidModelAndExistingDocument_ReturnsCreatedResult(string documentId)
        {
            // arrange
            var model = new DocumentInformationDto()
            {
                
            };

            var httpContent = model.ToJsonHttpContent();

            // act
            var response = await this.client.PostAsync($"/api/document/{documentId}/information", httpContent);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("1", "2")]
        public async Task Delete_ForExistingInformation_ReturnsNoContentResult(string documentId, string informationId)
        {
            // act
            var response = await this.client.DeleteAsync($"/api/document/{documentId}/information/{informationId}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("1", "7")]
        public async Task Delete_ForNonExistingInformation_ReturnsNotFoundResult(string documentId, string informationId)
        {
            // act
            var response = await this.client.DeleteAsync($"/api/document/{documentId}/information/{informationId}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}

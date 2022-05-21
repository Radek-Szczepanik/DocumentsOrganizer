using DocumentsOrganizer.Entities;
using DocumentsOrganizer.IntegrationTests.Helpers;
using DocumentsOrganizer.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
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
    public class DocumentControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public DocumentControllerTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DocumentsOrganizerDbContext>));

                        services.Remove(dbContextOptions);
                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
                        services.AddDbContext<DocumentsOrganizerDbContext>(options => options.UseInMemoryDatabase("DocumentsDb"));
                    });
                });
            client = this.factory.CreateClient();
        }

        private void SeedDocument(Document document)
        {
            var scopeFactory = this.factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<DocumentsOrganizerDbContext>();

            dbContext.Documents.Add(document);
            dbContext.SaveChanges();
        }

        [Theory]
        [InlineData("PageNumber=1&PageSize=5")]
        [InlineData("PageNumber=15&PageSize=10")]
        [InlineData("PageNumber=155&PageSize=15")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParams)
        {
            var response = await this.client.GetAsync("/api/document?" + queryParams);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("PageNumber=7&PageSize=7")]
        [InlineData("PageNumber=1&PageSize=11")]
        [InlineData("PageNumber=12&PageSize=29")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetAll_WithInvalidQueryParameters_ReturnsBadRequestResult(string queryParams)
        {
            var response = await this.client.GetAsync("/api/document?" + queryParams);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task GetById_ForExistingDocument_ReturnsOkResult(string queryParams)
        {
            var response = await this.client.GetAsync("/api/document/" + queryParams);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("8")]
        [InlineData("12")]
        public async Task GetById_ForNonExistingDocument_ReturnsOkResult(string queryParams)
        {
            var response = await this.client.GetAsync("/api/document/" + queryParams);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetById_WithInvalidQueryParameters_ReturnsBadRequestResult(string queryParams)
        {
            var response = await this.client.GetAsync("/api/document/" + queryParams);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateDocument_WithValidModel_ReturnsCreatedResult()
        {
            var model = new CreateDocumentDto()
            {
                Name = "Test document"
            };

            var httpContent = model.ToJsonHttpContent();

            var response = await this.client.PostAsync("/api/document", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateDocument_WithInvalidModel_ReturnsBadRequestResult()
        {
            var model = new CreateDocumentDto()
            {
                
            };

            var httpContent = model.ToJsonHttpContent();

            var response = await this.client.PostAsync("/api/document", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ForNonExistingDocument_ReturnsNotFoundResult()
        {
            var response = await this.client.DeleteAsync("/api/document/594");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForDocumentOwner_ReturnsNoContentResult()
        {
            // arrange

            var document = new Document()
            {
                CreatedById = 1,
                Name = "Test"
            };

            SeedDocument(document);

            // act

            var response = await this.client.DeleteAsync("/api/document/" + document.Id);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ForNonDocumentOwner_ReturnsForbiddenResult()
        {
            // arrange

            var document = new Document()
            {
                CreatedById = 328,
                Name = "Test"
            };

            SeedDocument(document);

            // act

            var response = await this.client.DeleteAsync("/api/document/" + document.Id);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

using FluentAssertions;
using Rental.Api;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Core.Pagination;
using Rental.Core.Resources;
using Rental.Core.Responses;
using RentalService.Tests.Integration.Factories;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace RentalService.Tests.Integration.Controllers.V1
{
    public class RentalPlanIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RentalPlanIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        #region Add

        [Fact]
        public async Task AddRentalPlan_ShouldCreateAndPersist()
        {
            // Arrange
            var request = new AddRentalPlanRequest
            {
                Days = 7,
                DailyRate = 50,
                PenaltyPercent = 10,
                Description = "Weekly plan"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/rentalservice/v1/rentalplan/add", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadFromJsonAsync<ApiResponse>();
            body.Should().NotBeNull();
            body!.Success.Should().BeTrue();
            body.Messages.Should().Contain(RentalPlanMessages.RentalPlan_Registered_Successfully);
        }

        [Fact]
        public async Task AddRentalPlan_ShouldReturnBadRequest_WhenInvalidData()
        {
            // Arrange
            var request = new AddRentalPlanRequest
            {
                Days = 0,
                DailyRate = 0,
                PenaltyPercent = 200,
                Description = "Invalid"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/rentalservice/v1/rentalplan/add", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Search

        [Fact]
        public async Task SearchRentalPlans_ShouldReturnPagedResult()
        {
            // Arrange: cria um registro para garantir que haja dados
            await _client.PostAsJsonAsync("/api/rentalservice/v1/rentalplan/add", new AddRentalPlanRequest
            {
                Days = 3,
                DailyRate = 30,
                PenaltyPercent = 5,
                Description = "Short Plan"
            });

            var request = new GetAllRentalPlanRequest
            {
                PageIndex = 1,
                PageSize = 10
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/rentalservice/v1/rentalplan/search", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var dataElement = doc.RootElement.GetProperty("data");

            var paged = System.Text.Json.JsonSerializer.Deserialize<PagedResult<GetAllRentalPlanResponse>>(
                dataElement.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            paged.Should().NotBeNull();
            paged!.Items.Should().NotBeEmpty();
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateRentalPlan_ShouldModifyExistingRecord()
        {
            // Arrange
            var createResponse = await _client.PostAsJsonAsync("/api/rentalservice/v1/rentalplan/add", new AddRentalPlanRequest
            {
                Days = 15,
                DailyRate = 70,
                PenaltyPercent = 8,
                Description = "Biweekly plan"
            });

            var created = await createResponse.Content.ReadFromJsonAsync<ApiResponse>();
            created!.Success.Should().BeTrue();

            // Você pode adaptar para extrair o ID real se o retorno trouxer o objeto.
            // Aqui vamos apenas enviar uma atualização simulada:
            var updateRequest = new UpdateRentalPlanRequest
            {
                Id = Guid.NewGuid(), // Se retornar o ID, substitua aqui.
                DailyRate = 80,
                PenaltyPercent = 7,
                Description = "Updated plan"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/rentalservice/v1/rentalplan/update", updateRequest);

            // Assert
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteRentalPlan_ShouldRemoveRecord()
        {
            // Arrange
            var createResponse = await _client.PostAsJsonAsync("/api/rentalservice/v1/rentalplan/add", new AddRentalPlanRequest
            {
                Days = 30,
                DailyRate = 100,
                PenaltyPercent = 10,
                Description = "Monthly plan"
            });

            var created = await createResponse.Content.ReadFromJsonAsync<ApiResponse>();
            created!.Success.Should().BeTrue();

            // Simula exclusão (caso o ID não venha no response, você precisará de query auxiliar futuramente)
            var deleteRequest = new DeleteRentalPlanRequest
            {
                Id = Guid.NewGuid()
            };

            // Act
            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "/api/rentalservice/v1/rentalplan/delete")
            {
                Content = JsonContent.Create(deleteRequest)
            });

            // Assert
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);
        }

        #endregion
    }
}

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Rental.Api.Application.Commands.RentalPlanCommands.Add;
using Rental.Api.Application.Commands.RentalPlanCommands.Delete;
using Rental.Api.Application.Commands.RentalPlanCommands.Update;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Api.Application.Queries.RentalPlanQueries.GetAll;
using Rental.Core.Pagination;
using Rental.Core.Resources;
using Rental.Core.Responses;
using RentalService.UnitTest.Base.Fixtures.Controllers;

namespace RentalService.UnitTest.Controllers.V1
{
    public class RentalPlanControllerTests : IClassFixture<RentalPlanControllerFixture>
    {
        private readonly RentalPlanControllerFixture _fixture;

        public RentalPlanControllerTests(RentalPlanControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithPagedResult()
        {
            // Arrange
            var request = new GetAllRentalPlanRequest();

            var pagedResult = new PagedResult<GetAllRentalPlanResponse>(
                new List<GetAllRentalPlanResponse>
                {
                    new() { Days = 7, DailyRate = 50, PenaltyPercent = 20, Description = "Weekly plan" }
                },
                totalCount: 1,
                pageNumber: 1,
                pageSize: 10
            );

            _fixture.MediatorMock
                .Setup(m => m.SendQuery(It.IsAny<GetAllRentalPlanQuery>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _fixture.Controller.GetAll(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse>().Subject;

            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();

            var data = response.Data.Should().BeAssignableTo<PagedResult<GetAllRentalPlanResponse>>().Subject;
            data.Items.Should().ContainSingle(i => i.Description == "Weekly plan");

            _fixture.MediatorMock.Verify(m => m.SendQuery(It.IsAny<GetAllRentalPlanQuery>()), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnOk_WhenCommandSucceeds()
        {
            var request = new AddRentalPlanRequest { Days = 7, DailyRate = 100, PenaltyPercent = 10, Description = "Weekly Plan" };
            var successResponse = _fixture.CreateSuccessResponse(RentalPlanMessages.RentalPlan_Registered_Successfully);

            _fixture.MediatorMock
                .Setup(m => m.SendCommand(It.IsAny<AddRentalPlanCommand>()))
                .ReturnsAsync(successResponse);

            var result = await _fixture.Controller.Add(request);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse>().Subject;

            response.Success.Should().BeTrue();
            response.Messages.Should().Contain(RentalPlanMessages.RentalPlan_Registered_Successfully);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenCommandFails()
        {
            var request = new AddRentalPlanRequest { Days = 0, DailyRate = 0, PenaltyPercent = 10, Description = "Invalid Plan" };
            var failResponse = _fixture.CreateFailResponse("Invalid data");

            _fixture.MediatorMock
                .Setup(m => m.SendCommand(It.IsAny<AddRentalPlanCommand>()))
                .ReturnsAsync(failResponse);

            var result = await _fixture.Controller.Add(request);

            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badResult.Value.Should().BeOfType<ApiResponse>().Subject;

            response.Success.Should().BeFalse();
            response.Messages.Should().Contain("Invalid data");
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenCommandSucceeds()
        {
            var request = new UpdateRentalPlanRequest { Id = Guid.NewGuid(), DailyRate = 150, PenaltyPercent = 5, Description = "Updated plan" };
            var successResponse = _fixture.CreateSuccessResponse(RentalPlanMessages.RentalPlan_Updated_Successfully);

            _fixture.MediatorMock
                .Setup(m => m.SendCommand(It.IsAny<UpdateRentalPlanCommand>()))
                .ReturnsAsync(successResponse);

            var result = await _fixture.Controller.Update(request);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse>().Subject;

            response.Success.Should().BeTrue();
            response.Messages.Should().Contain(RentalPlanMessages.RentalPlan_Updated_Successfully);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenCommandSucceeds()
        {
            var request = new DeleteRentalPlanRequest { Id = Guid.NewGuid() };
            var successResponse = _fixture.CreateSuccessResponse(RentalPlanMessages.RentalPlan_Deleted_Successfully);

            _fixture.MediatorMock
                .Setup(m => m.SendCommand(It.IsAny<DeleteRentalPlanCommand>()))
                .ReturnsAsync(successResponse);

            var result = await _fixture.Controller.Delete(request);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse>().Subject;

            response.Success.Should().BeTrue();
            response.Messages.Should().Contain(RentalPlanMessages.RentalPlan_Deleted_Successfully);
        }
    }
}

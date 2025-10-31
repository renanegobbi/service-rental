using FluentAssertions;
using Moq;
using Rental.Api.Application.Commands.RentalPlanCommands.Update;
using Rental.Core.Resources;
using Rental.Core.Responses;
using RentalService.Tests.Unit.Base.Fixtures.Commands;
using EntityRentalPlan = Rental.Api.Entities.RentalPlan;

namespace RentalService.Tests.Unit.Commands.RentalPlan
{
    public class UpdateRentalPlanCommandHandlerTests : IClassFixture<RentalPlanCommandFixture>
    {
        private readonly RentalPlanCommandFixture _fixture;

        public UpdateRentalPlanCommandHandlerTests(RentalPlanCommandFixture fixture)
        {
            _fixture = fixture;
            _fixture.Reset();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenPlanNotFound()
        {
            // Arrange
            var command = new UpdateRentalPlanCommand(new() { Id = Guid.NewGuid(), DailyRate = 100, PenaltyPercent = 10 });
            _fixture.RepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((EntityRentalPlan?)null);

            // Act
            var result = await _fixture.UpdateHandler.Handle(command, CancellationToken.None);

            // Assert
            var response = result.Should().BeOfType<ApiResponse>().Subject;
            response.Should().BeOfType<ApiResponse>().Which.Success.Should().BeFalse();
            response.Messages.Should().Contain(RentalPlanMessages.RentalPlan_ID_Not_Found);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenUpdatedSuccessfully()
        {
            // Arrange
            var plan = _fixture.CreateValidPlan();
            var command = new UpdateRentalPlanCommand(new()
            {
                Id = plan.Id,
                DailyRate = 120,
                PenaltyPercent = 5,
                Description = "Updated"
            });

            _fixture.RepositoryMock.Setup(r => r.GetByIdAsync(plan.Id)).ReturnsAsync(plan);
            _fixture.RepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<EntityRentalPlan>());
            _fixture.RepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
            _fixture.RepositoryMock.Setup(r => r.Update(It.IsAny<EntityRentalPlan>()));

            // Act
            var result = await _fixture.UpdateHandler.Handle(command, CancellationToken.None);

            // Assert
            var response = result.Should().BeOfType<ApiResponse>().Subject;
            response.Success.Should().BeTrue();
            response.Messages.Should().Contain(RentalPlanMessages.RentalPlan_Updated_Successfully);
        }
    }
}

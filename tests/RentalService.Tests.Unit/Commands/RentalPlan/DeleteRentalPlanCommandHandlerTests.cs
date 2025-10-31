using FluentAssertions;
using Moq;
using Rental.Api.Application.Commands.RentalPlanCommands.Delete;
using Rental.Core.Resources;
using Rental.Core.Responses;
using RentalService.Tests.Unit.Base.Fixtures.Commands;
using EntityRentalPlan = Rental.Api.Entities.RentalPlan;

namespace RentalService.Tests.Unit.Commands.RentalPlan
{
    public class DeleteRentalPlanCommandHandlerTests : IClassFixture<RentalPlanCommandFixture>
    {
        private readonly RentalPlanCommandFixture _fixture;

        public DeleteRentalPlanCommandHandlerTests(RentalPlanCommandFixture fixture)
        {
            _fixture = fixture;
            _fixture.Reset();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenPlanNotFound()
        {
            // Arrange
            var command = new DeleteRentalPlanCommand(Guid.NewGuid());
            _fixture.RepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((EntityRentalPlan?)null);

            // Act
            var result = await _fixture.DeleteHandler.Handle(command, CancellationToken.None);

            // Assert
            var response = result.Should().BeOfType<ApiResponse>().Subject;
            response.Should().BeOfType<ApiResponse>().Which.Success.Should().BeFalse();
            response.Messages.Should().Contain(RentalPlanMessages.RentalPlan_ID_Not_Found);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenPlanDeleted()
        {
            // Arrange
            var plan = _fixture.CreateValidPlan();
            var command = new DeleteRentalPlanCommand(plan.Id);

            _fixture.RepositoryMock.Setup(r => r.GetByIdAsync(plan.Id)).ReturnsAsync(plan);
            _fixture.RepositoryMock.Setup(r => r.Delete(It.IsAny<EntityRentalPlan>()));
            _fixture.RepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _fixture.DeleteHandler.Handle(command, CancellationToken.None);

            // Assert
            var response = result.Should().BeOfType<ApiResponse>().Subject;
            response.Should().BeOfType<ApiResponse>().Which.Success.Should().BeTrue();
            response.Messages.Should().Contain(RentalPlanMessages.RentalPlan_Deleted_Successfully);
        }
    }
}

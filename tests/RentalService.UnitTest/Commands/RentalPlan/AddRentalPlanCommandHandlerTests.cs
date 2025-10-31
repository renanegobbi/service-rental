using FluentAssertions;
using Moq;
using Rental.Api.Application.Commands.RentalPlanCommands.Add;
using Rental.Core.Resources;
using Rental.Core.Responses;
using RentalService.UnitTest.Base.Fixtures.Commands;
using EntityRentalPlan = Rental.Api.Entities.RentalPlan;

namespace RentalService.UnitTest.Commands.RentalPlan
{
    public class AddRentalPlanCommandHandlerTests : IClassFixture<RentalPlanCommandFixture>
    {
        private readonly RentalPlanCommandFixture _fixture;

        public AddRentalPlanCommandHandlerTests(RentalPlanCommandFixture fixture)
        {
            _fixture = fixture;
            _fixture.Reset();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new AddRentalPlanCommand(0, 0, 10, "Invalid");

            // Act
            var result = await _fixture.AddHandler.Handle(command, CancellationToken.None);

            // Assert
            var response = result.Should().BeOfType<ApiResponse>().Subject;
            response.Success.Should().BeFalse();

            _fixture.RepositoryMock.Verify(r => r.Add(It.IsAny<EntityRentalPlan>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenPlanAlreadyExists()
        {
            var command = new AddRentalPlanCommand(7, 100, 10, "Weekly");

            _fixture.RepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(() => new List<EntityRentalPlan> { _fixture.CreateValidPlan() });

            var result = await _fixture.AddHandler.Handle(command, CancellationToken.None);

            var response = result.Should().BeOfType<ApiResponse>().Subject;
            response.Success.Should().BeFalse();
            response.Messages.Should().Contain(m => m.Contains("already exists"));
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenValidAndPersisted()
        {
            // Arrange
            var command = new AddRentalPlanCommand(7, 100, 10, "Weekly");

            _fixture.RepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<EntityRentalPlan>());
            _fixture.RepositoryMock.Setup(r => r.Add(It.IsAny<EntityRentalPlan>()));
            _fixture.RepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _fixture.AddHandler.Handle(command, CancellationToken.None);

            // Assert
            var response = result.Should().BeOfType<ApiResponse>().Subject;
            response.Success.Should().BeTrue();
            response.Messages.Should().Contain(RentalPlanMessages.RentalPlan_Registered_Successfully);
        }
    }
}

using FluentAssertions;
using Rental.Api.Application.Commands.RentalPlanCommands.Delete;

namespace RentalService.Tests.Unit.Commands.RentalPlan
{
    public class DeleteRentalPlanCommandTests
    {
        [Fact]
        public void IsValid_ShouldReturnTrue_WhenIdIsProvided()
        {
            // Arrange
            var command = new DeleteRentalPlanCommand(Guid.NewGuid());

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenIdIsEmpty()
        {
            // Arrange
            var command = new DeleteRentalPlanCommand(Guid.Empty);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should()
                .Contain(e => e.PropertyName == "Id")
                .And.Contain(e => e.ErrorMessage == "The ID must be provided.");
        }
    }
}

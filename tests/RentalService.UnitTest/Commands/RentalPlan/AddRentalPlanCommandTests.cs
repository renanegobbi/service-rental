using FluentAssertions;
using Rental.Api.Application.Commands.RentalPlanCommands.Add;

namespace RentalService.UnitTest.Commands.RentalPlan
{
    public class AddRentalPlanCommandTests
    {
        [Fact]
        public void IsValid_ShouldReturnTrue_WhenCommandHasValidData()
        {
            // Arrange
            var command = new AddRentalPlanCommand(
                days: 7,
                dailyRate: 100,
                penaltyPercent: 10,
                description: "Weekly plan"
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenDaysIsZero()
        {
            // Arrange
            var command = new AddRentalPlanCommand(
                days: 0,
                dailyRate: 100,
                penaltyPercent: 10,
                description: "Invalid plan"
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should()
                .Contain(e => e.PropertyName == "Days");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenDailyRateIsZero()
        {
            // Arrange
            var command = new AddRentalPlanCommand(
                days: 7,
                dailyRate: 0,
                penaltyPercent: 10,
                description: "Invalid plan"
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should()
                .Contain(e => e.PropertyName == "DailyRate");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenDescriptionIsTooLong()
        {
            // Arrange
            var longDescription = new string('A', 101);
            var command = new AddRentalPlanCommand(
                days: 7,
                dailyRate: 50,
                penaltyPercent: 5,
                description: longDescription
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should()
                .Contain(e => e.PropertyName == "Description");
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenPenaltyPercentIsZero()
        {
            // Arrange
            var command = new AddRentalPlanCommand(
                days: 7,
                dailyRate: 100,
                penaltyPercent: 0,
                description: "No penalty plan"
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }
    }
}

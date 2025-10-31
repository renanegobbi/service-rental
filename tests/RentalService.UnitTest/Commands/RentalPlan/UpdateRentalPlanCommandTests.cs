using FluentAssertions;
using Rental.Api.Application.Commands.RentalPlanCommands.Update;
using Rental.Api.Application.DTOs.RentalPlan;
using System;

namespace RentalService.UnitTests.UnitTest.Commands.RentalPlan
{
    public class UpdateRentalPlanCommandTests
    {
        [Fact]
        public void IsValid_ShouldReturnTrue_WhenCommandHasValidData()
        {
            // Arrange
            var command = new UpdateRentalPlanCommand(new UpdateRentalPlanRequest
            {
                Id = Guid.NewGuid(),
                DailyRate = 100,
                PenaltyPercent = 10,
                Description = "Updated plan"
            });

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
            var command = new UpdateRentalPlanCommand(new UpdateRentalPlanRequest
            {
                Id = Guid.Empty,
                DailyRate = 100,
                PenaltyPercent = 10,
                Description = "Invalid"
            });

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should()
                .Contain(e => e.PropertyName == "Id");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenDailyRateIsZero()
        {
            // Arrange
            var command = new UpdateRentalPlanCommand(new UpdateRentalPlanRequest
            {
                Id = Guid.NewGuid(),
                DailyRate = 0,
                PenaltyPercent = 10,
                Description = "Invalid"
            });

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should()
                .Contain(e => e.PropertyName == "DailyRate");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenPenaltyPercentIsOutOfRange()
        {
            // Arrange
            var command = new UpdateRentalPlanCommand(new UpdateRentalPlanRequest
            {
                Id = Guid.NewGuid(),
                DailyRate = 100,
                PenaltyPercent = 150,
                Description = "Invalid"
            });

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should()
                .Contain(e => e.PropertyName == "PenaltyPercent");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenDescriptionIsTooLong()
        {
            // Arrange
            var command = new UpdateRentalPlanCommand(new UpdateRentalPlanRequest
            {
                Id = Guid.NewGuid(),
                DailyRate = 100,
                PenaltyPercent = 10,
                Description = new string('A', 101)
            });

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should()
                .Contain(e => e.PropertyName == "Description");
        }
    }
}

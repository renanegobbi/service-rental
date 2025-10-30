using FluentValidation;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Core.Messages;

namespace Rental.Api.Application.Commands.RentalPlanCommands.Add
{
    public class AddRentalPlanCommand : Command
    {
        public int Days { get; private set; }
        public decimal DailyRate { get; private set; }
        public decimal PenaltyPercent { get; private set; }
        public string? Description { get; private set; }

        public AddRentalPlanCommand(int days, decimal dailyRate, decimal penaltyPercent, string? description)
        {
            Days = days;
            DailyRate = dailyRate;
            PenaltyPercent = penaltyPercent;
            Description = description;
        }

        public AddRentalPlanCommand(AddRentalPlanRequest request)
        {
            Days = request.Days;
            DailyRate = request.DailyRate;
            PenaltyPercent = request.PenaltyPercent;
            Description = request.Description;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddRentalPlanValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddRentalPlanValidation : AbstractValidator<AddRentalPlanCommand>
        {
            public AddRentalPlanValidation()
            {
                RuleFor(c => c.Days).GreaterThan(0);
                RuleFor(c => c.DailyRate).GreaterThan(0);
                RuleFor(c => c.Description).MaximumLength(100);
            }
        }
    }
}

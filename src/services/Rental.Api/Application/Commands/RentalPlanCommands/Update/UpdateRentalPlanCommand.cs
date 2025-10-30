using FluentValidation;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Core.Messages;
using System;

namespace Rental.Api.Application.Commands.RentalPlanCommands.Update
{
    public class UpdateRentalPlanCommand : Command
    {
        public Guid Id { get; private set; }
        public int Days { get; private set; }
        public decimal DailyRate { get; private set; }
        public decimal PenaltyPercent { get; private set; }
        public string? Description { get; private set; }

        public UpdateRentalPlanCommand(UpdateRentalPlanRequest request)
        {
            Id = request.Id;
            DailyRate = request.DailyRate;
            PenaltyPercent = request.PenaltyPercent;
            Description = request.Description;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateRentalPlanValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class UpdateRentalPlanValidation : AbstractValidator<UpdateRentalPlanCommand>
        {
            public UpdateRentalPlanValidation()
            {
                RuleFor(c => c.Id)
                    .NotEmpty().WithMessage("The plan ID is required.");

                RuleFor(c => c.DailyRate)
                    .GreaterThan(0).WithMessage("Daily rate must be greater than zero.");

                RuleFor(c => c.PenaltyPercent)
                    .InclusiveBetween(0, 100).WithMessage("Penalty percent must be between 0 and 100.");

                RuleFor(c => c.Description)
                    .MaximumLength(100).WithMessage("Description must have up to 100 characters.");
            }
        }
    }
}

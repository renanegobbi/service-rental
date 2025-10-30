using FluentValidation;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Core.Messages;
using System;

namespace Rental.Api.Application.Commands.RentalPlanCommands.Delete
{
    public class DeleteRentalPlanCommand : Command
    {
        public Guid Id { get; private set; }

        public DeleteRentalPlanCommand(Guid id)
        {
            Id = id;
        }

        public DeleteRentalPlanCommand(DeleteRentalPlanRequest request)
        {
            Id = request.Id;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteRentalPlanValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class DeleteRentalPlanValidation : AbstractValidator<DeleteRentalPlanCommand>
        {
            public DeleteRentalPlanValidation()
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .WithMessage("The ID must be provided.");
            }
        }
    }
}

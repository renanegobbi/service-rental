using FluentValidation;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Core.Messages;
using System;

namespace Rental.Api.Application.Commands.MotorcycleCommands.Delete
{
    public class DeleteMotorcycleCommand : Command
    {
        public Guid Id { get; private set; }

        public DeleteMotorcycleCommand(Guid id)
        {
            Id = id;
        }

        public DeleteMotorcycleCommand(DeleteMotorcycleRequest request)
        {
            Id = request.Id;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteMotorcycleValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class DeleteMotorcycleValidation : AbstractValidator<DeleteMotorcycleCommand>
        {
            public DeleteMotorcycleValidation()
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .WithMessage("The ID must be provided.");
            }
        }
    }
}

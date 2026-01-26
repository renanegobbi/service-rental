using FluentValidation;
using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Core.Messages;
using System;

namespace Rental.Api.Application.Commands.MotocycleCommands.Update
{
    public class UpdateMotorcycleCommand : Command
    {
        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }

        public UpdateMotorcycleCommand(UpdateMotorcycleRequest request)
        {
            Id = request.Id;
            Year = request.Year;
            Model = request.Model;
            Plate = request.Plate;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateMotorcycleValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class UpdateMotorcycleValidation : AbstractValidator<UpdateMotorcycleCommand>
        {
            public UpdateMotorcycleValidation()
            {
                RuleFor(c => c.Id)
                    .NotEmpty().WithMessage("The motorcycle ID is required.");

                RuleFor(c => c.Year)
                    .GreaterThan(2000);

                RuleFor(c => c.Model)
                    .NotEmpty()
                    .MaximumLength(50);

                RuleFor(c => c.Plate)
                    .NotEmpty()
                    .MaximumLength(10);
            }
        }
    }
}

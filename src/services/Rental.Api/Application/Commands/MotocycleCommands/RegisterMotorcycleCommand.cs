using FluentValidation;
using Rental.Core.Messages;

namespace Rental.Api.Application.Commands.MotocycleCommands
{
    public class RegisterMotorcycleCommand : Command
    {
        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }

        public RegisterMotorcycleCommand(Guid id, int year, string model, string plate)
        {
            AggregateId = id;
            Id = id;
            Year = year;
            Model = model;
            Plate = plate;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterMotorcycleValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RegisterMotorcycleValidation : AbstractValidator<RegisterMotorcycleCommand>
        {
            public RegisterMotorcycleValidation()
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid motorcycle Id.");

                RuleFor(c => c.Year)
                    .GreaterThan(2000)
                    .WithMessage("The motorcycle year must be greater than 2000.");

                RuleFor(c => c.Model)
                    .NotEmpty()
                    .WithMessage("The motorcycle model must be provided.")
                    .MaximumLength(50)
                    .WithMessage("The motorcycle model cannot exceed 50 characters.");

                RuleFor(c => c.Plate)
                    .NotEmpty()
                    .WithMessage("The motorcycle plate must be provided.")
                    .MaximumLength(10)
                    .WithMessage("The motorcycle plate cannot exceed 10 characters.");
            }
        }
    }
}

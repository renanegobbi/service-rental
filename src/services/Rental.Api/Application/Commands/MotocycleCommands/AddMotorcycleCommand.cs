using FluentValidation;
using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Core.Messages;

namespace Rental.Api.Application.Commands.MotorcycleCommands.Add
{
    public class AddMotorcycleCommand : Command
    {
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }

        public AddMotorcycleCommand(int year, string model, string plate)
        {
            Year = year;
            Model = model;
            Plate = plate;
        }

        public AddMotorcycleCommand(AddMotorcycleRequest request)
        {
            Year = request.Year;
            Model = request.Model;
            Plate = request.Plate;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddMotorcycleValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddMotorcycleValidation : AbstractValidator<AddMotorcycleCommand>
        {
            public AddMotorcycleValidation()
            {
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

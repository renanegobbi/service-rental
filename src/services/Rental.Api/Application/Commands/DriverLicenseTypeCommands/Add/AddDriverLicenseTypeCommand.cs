using FluentValidation;
using FluentValidation.Results;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Core.Messages;

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Add
{
    public class AddDriverLicenseTypeCommand : Command
    {
        public string Code { get; private set; }
        public string Description { get; private set; }

        public AddDriverLicenseTypeCommand(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public AddDriverLicenseTypeCommand(AddDriverLicenseTypeRequest request)
        {
            Code = request.Code;
            Description = request.Description;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddDriverLicenseTypeValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddDriverLicenseTypeValidation : AbstractValidator<AddDriverLicenseTypeCommand>
        {
            public AddDriverLicenseTypeValidation()
            {
                RuleFor(x => x.Code)
                    .NotEmpty().WithMessage("The license code is required.")
                    .MaximumLength(5).WithMessage("The license code must have up to 5 characters.")
                    .Matches("^[a-zA-Z]{1,5}$").WithMessage("The license code must contain only letters.");

                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("The description is required.")
                    .MaximumLength(100).WithMessage("The description must have up to 100 characters.");
            }
        }
    }
}

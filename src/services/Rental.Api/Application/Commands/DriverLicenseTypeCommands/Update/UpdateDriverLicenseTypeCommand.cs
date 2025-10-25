using FluentValidation;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Core.Messages;
using System;

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Update
{
    public class UpdateDriverLicenseTypeCommand : Command
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }

        public UpdateDriverLicenseTypeCommand(UpdateDriverLicenseTypeRequest request)
        {
            Id = request.Id;
            Code = request.Code;
            Description = request.Description;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateDriverLicenseTypeValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class UpdateDriverLicenseTypeValidation : AbstractValidator<UpdateDriverLicenseTypeCommand>
        {
            public UpdateDriverLicenseTypeValidation()
            {
                RuleFor(dlt => dlt.Id)
                    .NotEmpty()
                    .WithMessage("The ID must be provided.");

                RuleFor(dlt => dlt.Code)
                    .NotEmpty()
                    .WithMessage("The Code must be provided.")
                    .MaximumLength(2)
                    .WithMessage("The code cannot exceed 2 characters.");

                RuleFor(dlt => dlt.Description)
                    .NotEmpty()
                    .WithMessage("The description must be provided.")
                    .MaximumLength(100)
                    .WithMessage("The description cannot exceed 100 characters.");
            }
        }
    }
}

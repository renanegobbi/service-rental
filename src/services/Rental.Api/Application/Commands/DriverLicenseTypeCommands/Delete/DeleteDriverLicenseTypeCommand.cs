using FluentValidation;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Core.Messages;
using System;

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Delete
{
    public class DeleteDriverLicenseTypeCommand : Command
    {
        public Guid Id { get; private set; }

        public DeleteDriverLicenseTypeCommand(Guid id)
        {
            Id = id;
        }

        public DeleteDriverLicenseTypeCommand(DeleteDriverLicenseTypeRequest request)
        {
            Id = request.Id;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteDriverLicenseTypeValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class DeleteDriverLicenseTypeValidation : AbstractValidator<DeleteDriverLicenseTypeCommand>
        {
            public DeleteDriverLicenseTypeValidation()
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .WithMessage("The ID must be provided.");
            }
        }
    }
}

using FluentValidation;
using Rental.Api.Application.DTOs.Courier;
using Rental.Core.Messages;

namespace Rental.Api.Application.Commands.CourierCommands
{
    public class RegisterCourierCommand : Command
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string DriverLicenseNumber { get; private set; }
        public string DriverLicenseType { get; private set; }
        public string? DriverLicenseImageUrl { get; private set; }

        public RegisterCourierCommand(
            Guid id,
            string fullName,
            string cnpj,
            DateTime birthDate,
            string driverLicenseNumber,
            string driverLicenseType//,
            /*string? driverLicenseImageUrl = null*/)
        {
            AggregateId = id;
            Id = id;
            FullName = fullName;
            Cnpj = cnpj;
            BirthDate = birthDate;
            DriverLicenseNumber = driverLicenseNumber;
            DriverLicenseType = driverLicenseType;
            //DriverLicenseImageUrl = driverLicenseImageUrl;
        }

        public RegisterCourierCommand(RegisterCourierRequest registerCourierRequest)
        {
            var newId = Guid.NewGuid();
            AggregateId = newId;
            Id = newId;
            FullName = registerCourierRequest.FullName;
            Cnpj = registerCourierRequest.Cnpj;
            BirthDate = registerCourierRequest.BirthDate;
            DriverLicenseNumber = registerCourierRequest.DriverLicenseNumber;
            DriverLicenseType = registerCourierRequest.DriverLicenseType;
            //DriverLicenseImageUrl = registerCourierRequest.DriverLicenseImageUrl;
            //null;
            DriverLicenseImageUrl = null; // valor inicial padrão
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterCourierValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RegisterCourierValidation : AbstractValidator<RegisterCourierCommand>
        {
            public RegisterCourierValidation()
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid courier Id.");

                RuleFor(c => c.FullName)
                    .NotEmpty().WithMessage("Courier full name must be provided.")
                    .MaximumLength(100).WithMessage("Courier full name cannot exceed 100 characters.");

                RuleFor(c => c.Cnpj)
                    .NotEmpty().WithMessage("Courier CNPJ must be provided.");

                RuleFor(c => c.BirthDate)
                    .LessThan(DateTime.UtcNow)
                    .WithMessage("Birth date must be in the past.");

                RuleFor(c => c.DriverLicenseNumber)
                    .NotEmpty().WithMessage("Driver license number must be provided.")
                    .MaximumLength(20).WithMessage("Driver license number cannot exceed 20 characters.");

                RuleFor(c => c.DriverLicenseType)
                    .NotEmpty().WithMessage("Driver license type must be provided.")
                    .Must(t => t == "A" || t == "B" || t == "AB")
                    .WithMessage("Driver license type must be 'A', 'B', or 'AB'.");
            }
        }
    }
}

using FluentValidation;
using Rental.Api.Application.DTOs.Auth;
using Rental.Core.Messages;

namespace Rental.Api.Application.Commands.AuthCommands.Register
{
    public class UserRegisterCommand : Command
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string PasswordConfirmation { get; private set; }
        public UserRegisterCommand(UserRegisterRequest request)
        {
            Email = request.Email;
            Password = request.Password;
            PasswordConfirmation = request.PasswordConfirmation;
        }

        public override bool IsValid()
        {
            ValidationResult = new UserRegisterValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class UserRegisterValidation : AbstractValidator<UserRegisterCommand>
        {
            public UserRegisterValidation()
            {
                RuleFor(c => c.Email)
                    .NotEmpty().WithMessage("Email is required")
                    .EmailAddress().WithMessage("Invalid email");

                RuleFor(c => c.Password)
                    .NotEmpty().WithMessage("Password is required")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters");

                RuleFor(c => c.PasswordConfirmation)
                    .Equal(c => c.Password)
                    .WithMessage("Password confirmation does not match");

            }
        }
    }
}

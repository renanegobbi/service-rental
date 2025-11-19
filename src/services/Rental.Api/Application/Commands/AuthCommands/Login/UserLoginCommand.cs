using FluentValidation;
using Rental.Api.Application.DTOs.Auth;
using Rental.Core.Messages;

namespace Rental.Api.Application.Commands.AuthCommands.Login
{
    public class UserLoginCommand : Command
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public UserLoginCommand(UserLoginRequest request)
        {
            Email = request.Email;
            Password = request.Password;
        }

        public override bool IsValid()
        {
            ValidationResult = new UserLoginValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class UserLoginValidation : AbstractValidator<UserLoginCommand>
        {
            public UserLoginValidation()
            {
                RuleFor(c => c.Email)
                    .NotEmpty().WithMessage("E-mail is required")
                    .EmailAddress().WithMessage("Invalid e-mail");

                RuleFor(c => c.Password)
                    .NotEmpty().WithMessage("Password is required");

            }
        }
    }
}

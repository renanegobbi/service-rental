using FluentValidation;
using IdentityModel.Client;
using Rental.Core.Messages;
using System;
using RefreshTokenRequest = Rental.Api.Application.DTOs.Auth.RefreshTokenRequest;

namespace Rental.Api.Application.Commands.RefreshTokenCommands
{
    public class RefreshTokenCommand : Command
    {
        public string RefreshToken { get; private set; }

        public RefreshTokenCommand(RefreshTokenRequest request)
        {
            RefreshToken = request.RefreshToken;
        }

        public RefreshTokenCommand(string request)
        {
            RefreshToken = request;
        }

        public override bool IsValid()
        {
            ValidationResult = new RefreshTokenValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RefreshTokenValidation : AbstractValidator<RefreshTokenCommand>
        {
            public RefreshTokenValidation()
            {
                RuleFor(rt => rt.RefreshToken)
                    .NotEmpty().WithMessage("Refresh Token is required")
                    .Must(BeAValidGuid).WithMessage("Invalid refresh token");
            }

            private bool BeAValidGuid(string refreshToken)
            {
                try
                {
                    Guid.Parse(refreshToken);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}

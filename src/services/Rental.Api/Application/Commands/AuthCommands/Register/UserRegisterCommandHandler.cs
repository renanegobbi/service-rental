using MediatR;
using Microsoft.AspNetCore.Identity;
using Rental.Api.Application.Extensions;
using Rental.Api.Entities.Identity;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.AuthCommands.Register
{
    public class UserRegisterCommandHandler : CommandHandler,
        IRequestHandler<UserRegisterCommand, IResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRegisterCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResponse> Handle(UserRegisterCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting UserRegisterCommand: Email={Email}", command.Email);

            if (!command.IsValid())
            {
                Log.Information("Validation failed for UserRegisterCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }
                
            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
            {
                Log.Information("Business rule validation failed for UserRegisterCommand: {@Errors}", ValidationResult.Errors);
                return Response.Fail(ValidationResult);
            }
                
            var user = new ApplicationUser
            {
                UserName = command.Email,
                Email = command.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
            {
                Log.Information("{Error_Persisting_Data}", CommonMessages.Error_Persisting_Data);
                return Response.Fail(CommonMessages.Error_Persisting_Data);
            }

            Log.Information("{User_Created_Successfully}", AuthMessages.User_Created_Successfully);
            return Response.Ok(AuthMessages.User_Created_Successfully, user.ToUserRegisterResponse());
        }

        private async Task ValidateBusinessRulesAsync(UserRegisterCommand command)
        {
            var exists = await _userManager.FindByEmailAsync(command.Email);
            if (exists != null)
            {
                AddError("A user with this email already exists.");
            }
        }
    }
}

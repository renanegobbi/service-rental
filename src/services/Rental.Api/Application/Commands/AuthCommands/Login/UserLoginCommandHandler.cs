using MediatR;
using Microsoft.AspNetCore.Identity;
using Rental.Api.Application.Services.Auth;
using Rental.Api.Entities.Identity;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.AuthCommands.Login
{
    public class UserLoginCommandHandler : CommandHandler,
        IRequestHandler<UserLoginCommand, IResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly AuthenticationService _authService;
        public UserLoginCommandHandler(UserManager<ApplicationUser> userManager, 
                                       SignInManager<ApplicationUser> signInManager,
                                       AuthenticationService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        public async Task<IResponse> Handle(UserLoginCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return Response.Fail(AuthMessages.User_Or_Password_Incorrect);

            if (!user.EmailConfirmed)
                return Response.Fail(AuthMessages.User_Email_Not_Confirmed);

            var result = await _signInManager.PasswordSignInAsync(
                user, command.Password, false, lockoutOnFailure: true);

            if (result.Succeeded) 
            {
                var userLongReposnse = await _authService.GenerateJwt(user.Email);
                return Response.Ok(AuthMessages.User_Login_Success, userLongReposnse);
            }

            if (result.IsLockedOut)
                return Response.Fail(AuthMessages.User_Blocked_Invalid_Attempts);

            return Response.Fail(AuthMessages.User_Or_Password_Incorrect);
        }

        private async Task ValidateBusinessRulesAsync(UserLoginCommand command)
        {

        }
    }
}

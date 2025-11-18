using MediatR;
using Rental.Api.Application.Services.Auth;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.RefreshTokenCommands
{
    public class RefreshTokenCommandHandler : CommandHandler,
        IRequestHandler<RefreshTokenCommand, IResponse>
    {
        public readonly AuthenticationService _authenticationService;
        public RefreshTokenCommandHandler(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<IResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            ValidateBusinessRules(command);

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            Guid guid;
            try
            {
                guid = Guid.Parse(command.RefreshToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while executing {Command}", nameof(RefreshTokenCommand));
                return Response.Fail(AuthMessages.Refresh_Token_Invalid);
            }

            var refreshToken = await _authenticationService.GetRefreshToken(guid);

            if (refreshToken is null)
                return Response.Fail(AuthMessages.Refresh_Token_Invalid);

            if (refreshToken.ExpirationDate.ToLocalTime() <= DateTime.Now)
                return Response.Fail(AuthMessages.Refresh_Token_Expired);

            return Response.Ok(AuthMessages.User_Login_Success,
                await _authenticationService.GenerateJwt(refreshToken.Username));
        }

        private void ValidateBusinessRules(RefreshTokenCommand command)
        {
            if (string.IsNullOrEmpty(command.RefreshToken))
            {
                AddError(AuthMessages.Refresh_Token_Invalid);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rental.Api.Application.Commands.AuthCommands;
using Rental.Api.Application.Commands.AuthCommands.Login;
using Rental.Api.Application.DTOs.Auth;
using Rental.Api.Swagger.Examples;
using Rental.Core.Mediator;
using Rental.Core.Responses;
using Rental.Services.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Threading.Tasks;

namespace Rental.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InternalServerErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorResponseExample))]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundResponseExample))]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestResponseExample))]
    [SwaggerTag("Provides management and retrieval of authentication and personal data.")]
    public class AuthController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public AuthController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// Saves a new user in the database.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>Authentication is not required to access this endpoint.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/new-account")]
        [SwaggerRequestExample(typeof(UserRegisterRequest), typeof(UserRegisterRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UserRegisterResponseExamplo))]
        [ProducesResponseType(typeof(UserRegisterResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Register(UserRegisterRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new UserRegisterCommand(request));

            return ApiResponse(response);
        }

        /// <summary>
        /// Authenticates a user in the system.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>Authentication is not required to access this endpoint.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/login")]
        [SwaggerRequestExample(typeof(UserLoginRequest), typeof(UserLoginRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UserLoginResponseExamplo))]
        [ProducesResponseType(typeof(UserLoginResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Login(UserLoginRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new UserLoginCommand(request));

            return ApiResponse(response);
        }

    }
}

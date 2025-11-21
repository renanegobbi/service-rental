using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rental.Api.Application.Commands.MotorcycleCommands.Add;
using Rental.Api.Application.Commands.RentalPlanCommands.Add;
using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Api.Swagger.Examples;
using Rental.Core.Authorization;
using Rental.Core.Mediator;
using Rental.Core.Responses;
using Rental.Services.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Threading.Tasks;

namespace Rental.Api.Controllers.V1
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InternalServerErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorResponseExample))]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundResponseExample))]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestResponseExample))]
    [SwaggerTag("Provides management and retrieval of motorcycle data.")]
    public class MotorcycleController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public MotorcycleController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// Registers a new motorcycle.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>Authentication <b>is required</b> to access this endpoint.</li>
        ///     <li>Requires appropriate role permissions.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        //[Authorize(Roles = UserRoles.AdminOrManager)]
        [Route("add")]
        [SwaggerRequestExample(typeof(AddMotorcycleRequest), typeof(AddMotorcycleRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AddMotorcycleResponseExample))]
        [ProducesResponseType(typeof(AddMotorcycleResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody] AddMotorcycleRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new AddMotorcycleCommand(request));

            return ApiResponse(response);
        }
    }
}

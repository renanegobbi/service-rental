using Microsoft.AspNetCore.Mvc;
using Rental.Api.Application.Commands.MotocycleCommands.Add;
using Rental.Api.Application.Commands.MotocycleCommands.Update;
using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Api.Application.Queries.MotorcycleQueries.GetAll;
using Rental.Api.Swagger.Examples;
using Rental.Core.Mediator;
using Rental.Core.Pagination;
using Rental.Core.Resources;
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
        /// Retrieves motorcycles based on query parameters.
        /// </summary>
        /// <remarks>
        /// Notes:
        /// <ul>
        ///     <li>Authentication <b>is required</b> to access this endpoint.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        //[Authorize]
        [Route("search")]
        [SwaggerRequestExample(typeof(GetAllMotorcycleRequest), typeof(GetAllMotorcycleRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetAllMotorcycleResponseExample))]
        [ProducesResponseType(typeof(PagedResult<GetAllMotorcycleResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromBody] GetAllMotorcycleRequest request)
        {
            var query = new GetAllMotorcycleQuery(request);
            if (!query.IsValid()) return ApiResponse(query.ValidationResult);
            var response = await _mediatorHandler.SendQuery(query);

            return ApiResponse(response, CommonMessages.Query_Successful);
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

        /// <summary>
        /// Updates a motorcycle.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>Authentication <b>is required</b> to access this endpoint.</li>
        ///     <li>Requires appropriate role permissions.</li>
        /// </ul>
        /// </remarks>
        [HttpPut]
        //[Authorize(Roles = UserRoles.AdminOrManager)]
        [Route("update")]
        [SwaggerRequestExample(typeof(UpdateMotorcycleRequest), typeof(UpdateMotorcycleRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UpdateMotorcycleResponseExamplo))]
        [ProducesResponseType(typeof(UpdateMotorcycleResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateMotorcycleRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new UpdateMotorcycleCommand(request));

            return ApiResponse(response);
        }
    }
}

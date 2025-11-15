using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rental.Api.Application.Commands.RentalPlanCommands.Add;
using Rental.Api.Application.Commands.RentalPlanCommands.Delete;
using Rental.Api.Application.Commands.RentalPlanCommands.Update;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Api.Application.Queries.RentalPlanQueries.GetAll;
using Rental.Api.Extensions;
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
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InternalServerErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorResponseExample))]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundResponseExample))]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestResponseExample))]
    [SwaggerTag("Provides management and retrieval of rental plan data.")]
    public class RentalPlanController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public RentalPlanController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// Retrieves rental plans based on query parameters.
        /// </summary>
        /// <remarks>
        /// Notes:
        /// <ul>
        ///     <li>No authentication is required to access this endpoint.</li>
        ///     <li>Returns all rental plans.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [ClaimsAuthorize("RentalPlan","Read")]
        [Route("v{version:apiVersion}/[controller]/search")]
        [SwaggerRequestExample(typeof(GetAllRentalPlanRequest), typeof(GetAllRentalPlanRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetAllRentalPlanResponseExample))]
        [ProducesResponseType(typeof(PagedResult<GetAllRentalPlanResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromBody] GetAllRentalPlanRequest request)
        {
            var query = new GetAllRentalPlanQuery(request);
            if (!query.IsValid()) return ApiResponse(query.ValidationResult);

            var response = await _mediatorHandler.SendQuery(query);
            return ApiResponse(response, CommonMessages.Query_Successful);
        }

        /// <summary>
        /// Registers a new rental plan.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>No authentication is required to access this endpoint.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/add")]
        [SwaggerRequestExample(typeof(AddRentalPlanRequest), typeof(AddRentalPlanRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AddRentalPlanResponseExamplo))]
        [ProducesResponseType(typeof(AddRentalPlanResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody] AddRentalPlanRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new AddRentalPlanCommand(request));

            return ApiResponse(response);
        }

        /// <summary>
        /// Updates a rental plan.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/update")]
        [SwaggerRequestExample(typeof(UpdateRentalPlanRequest), typeof(UpdateRentalPlanRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UpdateRentalPlanResponseExamplo))]
        [ProducesResponseType(typeof(UpdateRentalPlanResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateRentalPlanRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new UpdateRentalPlanCommand(request));

            return ApiResponse(response);
        }

        /// <summary>
        /// Deletes a rental plan.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/delete")]
        [SwaggerRequestExample(typeof(DeleteRentalPlanRequest), typeof(DeleteRentalPlanRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DeleteRentalPlanResponseExamplo))]
        [ProducesResponseType(typeof(DeleteRentalPlanResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromBody] DeleteRentalPlanRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new DeleteRentalPlanCommand(request));

            return ApiResponse(response);
        }
    }
}

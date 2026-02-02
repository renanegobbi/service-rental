using Microsoft.AspNetCore.Mvc;
using Rental.Api.Application.DTOs.Courier;
using Rental.Api.Application.Queries.CourierQueries.GetAll;
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
    [SwaggerTag("Provides management and retrieval of courier data.")]
    public class CourierController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CourierController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// Retrieves couries based on query parameters.
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
        [SwaggerRequestExample(typeof(GetAllCourierRequest), typeof(GetAllCourierRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetAllCourierResponseExample))]
        [ProducesResponseType(typeof(PagedResult<GetAllCourierResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromBody] GetAllCourierRequest request)
        {
            var query = new GetAllCourierQuery(request);
            if (!query.IsValid()) return ApiResponse(query.ValidationResult);
            var response = await _mediatorHandler.SendQuery(query);

            return ApiResponse(response, CommonMessages.Query_Successful);
        }

    }
}

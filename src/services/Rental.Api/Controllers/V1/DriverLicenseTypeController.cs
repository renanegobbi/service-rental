using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rental.Api.Application.Commands.DriverLicenseTypeCommands.Add;
using Rental.Api.Application.Commands.DriverLicenseTypeCommands.Delete;
using Rental.Api.Application.Commands.DriverLicenseTypeCommands.Update;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Api.Application.Queries.DriverLicenseTypeQueries.GetAll;
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
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InternalServerErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorResponseExample))]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundResponseExample))]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestResponseExample))]
    [SwaggerTag("Provides management and retrieval of driver license type data.")]
    public class DriverLicenseTypeController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public DriverLicenseTypeController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// Retrieves driver license types based on query parameters.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>No authentication is required to access this endpoint.</li>
        ///     <li>Returns all driver license types.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/search")]
        [SwaggerRequestExample(typeof(GetAllDriverLicenseTypeRequest), typeof(GetAllDriverLicenseTypeRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetAllDriverLicenseTypeResponseExample))]
        [ProducesResponseType(typeof(PagedResult<GetAllDriverLicenseTypeResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromBody] GetAllDriverLicenseTypeRequest request)
        {
            var query = new GetAllDriverLicenseTypeQuery(request);
            if (!query.IsValid()) return ApiResponse(query.ValidationResult);
            var response = await _mediatorHandler.SendQuery(query);
            
            return ApiResponse(response, CommonMessages.Query_Successful);
        }

        /// <summary>
        /// Registers a new driver license type.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>No authentication is required to access this endpoint.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/add")]
        [SwaggerRequestExample(typeof(AddDriverLicenseTypeRequest), typeof(AddDriverLicenseTypeRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AddDriverLicenseTypeResponseExamplo))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody] AddDriverLicenseTypeRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new AddDriverLicenseTypeCommand(request));
            
            return ApiResponse(response);
        }

        /// <summary>
        /// Updates a driver license type.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/update")]
        [SwaggerRequestExample(typeof(UpdateDriverLicenseTypeRequest), typeof(UpdateDriverLicenseTypeRequestExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UpdateDriverLicenseTypeResponseExamplo))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateDriverLicenseTypeRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new UpdateDriverLicenseTypeCommand(request));

            return ApiResponse(response);
        }

        /// <summary>
        /// Deletes a driver license type.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/delete")]
        [SwaggerRequestExample(typeof(DeleteDriverLicenseTypeRequest), typeof(DeleteDriverLicenseTypeResponseExamplo))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DeleteDriverLicenseTypeResponseExamplo))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromBody] DeleteDriverLicenseTypeRequest request)
        {
            var response = await _mediatorHandler.SendCommand(new DeleteDriverLicenseTypeCommand(request));

            return ApiResponse(response);
        }
    }

}

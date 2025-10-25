using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rental.Core.Responses;
using System.Net;

namespace Rental.Api.Controllers
{
    public class FeedbackController : Controller
    {

        [AllowAnonymous]
        [Route("feedback/{httpStatusCode:int}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Feedback(HttpStatusCode httpStatusCode)
        {
            ApiResponse response;

            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                    var featureNotFound = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    response = new NotFoundResponse(featureNotFound?.OriginalPath);
                    break;
                case HttpStatusCode.BadRequest:
                    response = new BadRequestResponse();
                    break;
                case HttpStatusCode.Forbidden:
                    response = new ForbiddenResponse();
                    break;
                case HttpStatusCode.InternalServerError:
                    var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();
                    response = new InternalServerErrorResponse(ex.Error);
                    break;
                case HttpStatusCode.Unauthorized:
                    var authenticationFailureMessage = HttpContext?.Items["AuthenticationFailureMessage "]?.ToString();
                    response = new UnauthorizedResponse(authenticationFailureMessage);
                    break;
                default:
                    response = new ApiResponse(false, new[] { $"Error {(int)httpStatusCode}: {httpStatusCode.ToString()}" }, null);
                    break;
            }

            return new JsonResult(response);
        }
    }
}








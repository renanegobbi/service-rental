using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Rental.Core.Interfaces;
using Rental.Core.Responses;
using System.Collections.Generic;
using System.Linq;

namespace Rental.Services.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected ICollection<string> Errors = new List<string>();

        protected bool IsOperationValid()
        {
            return !Errors.Any();
        }

        protected void AddProcessingError(string error)
        {
            Errors.Add(error);
        }

        protected void ClearProcessingErrors()
        {
            Errors.Clear();
        }

        protected ActionResult ApiResponse<T>(T result = default, string message = null)
        {
            if (result is ApiResponse apiResponse)
            {
                if (!apiResponse.Success)
                    return BadRequest(apiResponse);

                return Ok(apiResponse);
            }

            if (IsOperationValid())
            {
                return Ok(new ApiResponse(
                                success: true,
                                messages: new List<string> { message ?? "Operation completed successfully." },
                                data: result
                            ));
            }

            return BadRequest(new ApiResponse(false, Errors, null));
        }

        protected ActionResult ApiResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            foreach (var error in errors) AddProcessingError(error);

            return ApiResponse<object>(default!);
        }

        protected ActionResult ApiResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                AddProcessingError(error.ErrorMessage);

            return ApiResponse<object>(default!);
        }
    }
}
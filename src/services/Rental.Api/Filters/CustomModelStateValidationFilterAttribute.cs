using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Rental.Core.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Net;

/// <summary>
/// Filter that extracts messages from ModelState and formats them according to the API's response standard.
/// </summary>
public class CustomModelStateValidationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var validationResult = new ValidationResultModel(context.ModelState);

            context.Result = new JsonResult(new ApiResponse(false, validationResult.Erros.Select(x => $"{x.Field}: {x.Message}"), null));
        }
    }
}

public class ValidationError
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Field { get; }

    public string Message { get; }

    public ValidationError(string field, string message)
    {
        Field = field != string.Empty ? field[0].ToString().ToLower() + field.Substring(1) : null;
        Message = message;
    }
}

public class ValidationResultModel
{
    public List<ValidationError> Erros { get; }

    public ValidationResultModel(ModelStateDictionary modelState)
    {
        Erros = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, string.IsNullOrEmpty(x.ErrorMessage) ? x.Exception?.Message : x.ErrorMessage)))
                .ToList();
    }
}



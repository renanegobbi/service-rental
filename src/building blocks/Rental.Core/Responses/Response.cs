using FluentValidation.Results;
using Rental.Core.Interfaces;

namespace Rental.Core.Responses
{
    /// <summary>
    /// Static factory to create standardized application responses (success or error).
    /// </summary>
    public static class Response
    {
        /// <summary>
        /// Returns a success response.
        /// </summary>
        public static IResponse Ok(string message, object? data = null)
        {
            return new ApiResponse(true, new[] { message }, data);
        }

        /// <summary>
        /// Returns a success response.
        /// </summary>
        public static IResponse Ok(IEnumerable<string> messages, object? data = null)
        {
            return new ApiResponse(true, messages, data);
        }

        /// <summary>
        /// Returns an error response.
        /// </summary>
        public static IResponse Fail(params string[] messages)
        {
            return new ApiResponse(false, messages?.Length > 0 ? messages : new[] { "An unexpected error occurred." }, null);
        }

        /// <summary>
        /// Returns a response indicating validation failure.
        /// </summary>
        public static IResponse Fail(ValidationResult validationResult)
        {
            var messages = validationResult?.Errors?.Select(e => e.ErrorMessage)
                             ?? new[] { "An unexpected validation error occurred." };
            return new ApiResponse(false, messages, null);
        }

        /// <summary>
        /// Returns an error response from an exception.
        /// </summary>
        public static IResponse FromException(Exception ex)
        {
            return new ApiResponse(false, new[] { $"Unexpected error: {ex.Message}" }, null);
        }

    }
}

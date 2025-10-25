namespace Rental.Core.Responses
{
    /// <summary>
    /// Standard API response for HTTP 400 (Bad Request)
    /// </summary>
    public class BadRequestResponse : ApiResponse
    {
        public BadRequestResponse()
            : base(false, new[] { "The field X is required and was not provided.", "The field Y is required and was not provided." }, null)
        {
        }

        public ApiResponse GetExamples()
        {
            return new BadRequestResponse();
        }
    }

    /// <summary>
    /// Standard API response for HTTP 401 (Unauthorized)
    /// </summary>
    public class UnauthorizedResponse : ApiResponse
    {
        public UnauthorizedResponse()
            : base(false, new[]
            {
                "Access denied. Make sure you are authenticated in the system."
            }, null)
        {
        }

        public UnauthorizedResponse(string message)
            : base(false, new[]
            {
                string.IsNullOrEmpty(message)
                    ? "Access denied. Make sure you are authenticated in the system."
                    : $"Access denied: {message}"
            }, null)
        {
        }
    }

    /// <summary>
    /// Standard API response for HTTP 403 (Forbidden)
    /// </summary>
    public class ForbiddenResponse : ApiResponse
    {
        public ForbiddenResponse()
            : base(false, new[]
            {
                "Access denied. You do not have permission for this operation."
            }, null)
        {
        }
    }

    /// <summary>
    /// Standard API response for HTTP 404 (Not Found)
    /// </summary>
    public class NotFoundResponse : ApiResponse
    {
        public NotFoundResponse()
            : base(false, new[]
            {
                "The requested resource was not found."
            }, null)
        {
        }

        public NotFoundResponse(string path)
            : base(false, new[]
            {
                $"The address at {path} was not found."
            }, null)
        {
        }
    }

    /// <summary>
    /// Standard API response for HTTP 500 (Internal Server Error)
    /// </summary>
    public class InternalServerErrorResponse : ApiResponse
    {
        public InternalServerErrorResponse()
            : base(false, new[] { "An unexpected error occurred." }, null)
        {
        }

        public InternalServerErrorResponse(Exception exception)
            : base(false, new[] { "An unexpected error occurred." },
                  new
                  {
                      Message = "Please try again later or contact support.",
                      Details = new
                      {
                          exception?.Message,
                          BaseException = exception?.GetBaseException().Message,
                          exception?.Source
                      }
                  })
        {
        }
    }
}


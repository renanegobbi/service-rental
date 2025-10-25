using Rental.Core.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Rental.Api.Swagger.Examples
{
    public class BadRequestResponseExample : IExamplesProvider<BadRequestResponse>
    {
        public BadRequestResponse GetExamples()
        {
            return new BadRequestResponse();
        }
    }

    public class NotFoundResponseExample : IExamplesProvider<NotFoundResponse>
    {
        public NotFoundResponse GetExamples()
        {
            return new NotFoundResponse("/api/v1/resource");
        }
    }

    public class InternalServerErrorResponseExample : IExamplesProvider<InternalServerErrorResponse>
    {
        //public InternalServerErrorResponse GetExamples()
        //{
        //    return new InternalServerErrorResponse()
        //    {
        //        Data = new
        //        {
        //            Message = "Please try again later or contact support.",
        //            Details = new
        //            {
        //                Message = "Unexpected server error.",
        //                BaseException = "NpgsqlException: Connection refused",
        //                Source = "Rental.Api"
        //            }
        //        }
        //    };
        //}

        public InternalServerErrorResponse GetExamples()
        {
            return new InternalServerErrorResponse(new Exception("NpgsqlException: Connection refused"));
        }
    }
}

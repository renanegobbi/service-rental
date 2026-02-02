using Rental.Api.Application.DTOs.Courier;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Pagination;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Rental.Api.Swagger.Examples
{
    #region GetAll
    public class GetAllCourierRequestExamplo : IExamplesProvider<GetAllCourierRequest>
    {
        public GetAllCourierRequest GetExamples() => new GetAllCourierRequest
        {
            FullName = "Carlos Henrique Silva",
            Cnpj = "12345678000101",
            BirthDate = new DateOnly(1992, 05, 18),
            DriverLicenseNumber = "CNH123456A",
            DriverLicenseType = Guid.Parse("42dc670e-39ef-4584-b104-b4ba82290e81"),
            CreatedAt = new DateTime(2025, 10, 15, 0, 0, 0),
            PageIndex = 1,
            PageSize = 25,
            OrderBy = CourierOrderBy.FullName,
            SortDirection = "ASC"
        };
    }

    public class CourierExample : GetAllCourierResponse
    {
        public static IEnumerable<GetAllCourierResponse> Example => new[]
        {
           new GetAllCourierResponse
                {
                    FullName = "Carlos Henrique Silva",
                    Cnpj = "12345678000101",
                    BirthDate = new DateOnly(1992, 05, 18),
                    DriverLicenseNumber = "CNH123456A",
                    DriverLicenseType = Guid.Parse("42dc670e-39ef-4584-b104-b4ba82290e81"),
                    DriverLicenseImageUrl = "https://cdn.rental.com/licenses/carlos_silva.jpg",
                    CreatedAt = new DateTime(2025, 10, 15, 0, 0, 0)
                },
            new GetAllCourierResponse
            {
                    FullName = "Carlos Henrique Silva",
                    Cnpj = "12345678000101",
                    BirthDate = new DateOnly(1992, 05, 18),
                    DriverLicenseNumber = "CNH123456A",
                    DriverLicenseType = Guid.Parse("42dc670e-39ef-4584-b104-b4ba82290e81"),
                    DriverLicenseImageUrl = "https://cdn.rental.com/licenses/carlos_silva.jpg",
                    CreatedAt = new DateTime(2025, 10, 15, 0, 0, 0)
            }
        };
    }

    public class QueryCourierExample : PagedResult<GetAllCourierResponse>
    {
        public QueryCourierExample()
            : base(
                items: CourierExample.Example,
                totalCount: 2,
                pageNumber: 1,
                pageSize: 25,
                orderBy: CourierOrderBy.FullName.ToString(),
                sortDirection: "ASC"
            )
        { }
    }

    public class GetAllCourierResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            var pagedExample = new QueryCourierExample();

            return new ApiResponse(
                success: true,
                messages: new[] { CommonMessages.Query_Successful },
                data: pagedExample
            );
        }
    }
    #endregion
}

using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Pagination;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Rental.Api.Swagger.Examples
{
    public class GetAllMotorcycleRequestExamplo : IExamplesProvider<GetAllMotorcycleRequest>
    {
        public GetAllMotorcycleRequest GetExamples() => new GetAllMotorcycleRequest
        {
            Year = 2022,
            Model = "Honda CG 160 Fan",
            Plate = "ABC1D23",
            StartDate = new DateTime(2025, 10, 15, 0, 0, 0),
            EndDate = new DateTime(2025, 10, 15, 23, 59, 59),
            PageIndex = 1,
            PageSize = 25,
            OrderBy = MotorcycleOrderBy.Year,
            SortDirection = "ASC"
        };
    }

    public class MotorcycleExample : GetAllMotorcycleResponse
    {
        public static IEnumerable<GetAllMotorcycleResponse> Example => new[]
        {
           new GetAllMotorcycleResponse
                {
                    Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9e"),
                    Year = 2022,
                    Model = "Honda CG 160",
                    Plate = "ABC1D23",                    
                    CreatedAt = new DateTime(2025, 10, 12, 20, 11, 36)
                },
                new GetAllMotorcycleResponse
                {
                    Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9f"),
                    Year = 2023,
                    Model = "Yamaha Fazer 250",
                    Plate = "XYZ9E87",
                    CreatedAt = new DateTime(2025, 10, 15, 22, 12, 35)
                }
        };
    }

    public class QueryMotorcycleExample : PagedResult<GetAllMotorcycleResponse>
    {
        public QueryMotorcycleExample()
            : base(
                items: MotorcycleExample.Example,
                totalCount: 2,
                pageNumber: 1,
                pageSize: 25,
                orderBy: MotorcycleOrderBy.Year.ToString(),
                sortDirection: "ASC"
            )
        { }
    }

    public class GetAllMotorcycleResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            var pagedExample = new QueryMotorcycleExample();

            return new ApiResponse(
                success: true,
                messages: new[] { CommonMessages.Query_Successful },
                data: pagedExample
            );
        }
    }

    public class AddMotorcycleRequestExample : IExamplesProvider<AddMotorcycleRequest>
    {
        public AddMotorcycleRequest GetExamples() => new AddMotorcycleRequest
        {
            Year = 2023,
            Model = "Honda CG 160 Fan",
            Plate = "ABC1D23"
        };
    }

    public class AddMotorcycleResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { "Motorcycle registered successfully." },
            data: new AddMotorcycleResponse
            {
                Id = Guid.Parse("b39b592b-2116-4843-8959-d4919c092a9e"),
                Year = 2023,
                Model = "Honda CG 160 Fan",
                Plate = "ABC1D23",
                CreatedAt = DateTime.Parse("2025-11-21T15:18:32.6753229Z")
            }
        );
    }
}

using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Pagination;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Rental.Api.Swagger.Examples
{
    public class GetAllDriverLicenseTypeRequestExamplo : IExamplesProvider<GetAllDriverLicenseTypeRequest>
    {
        public GetAllDriverLicenseTypeRequest GetExamples() => new GetAllDriverLicenseTypeRequest
        {
            Code = "A",
            Description = "Motorcycle",
            IsActive = true,
            StartDate = new DateTime(2025, 10, 15, 0, 0, 0),
            EndDate = new DateTime(2025, 10, 15, 23, 59, 59),
            PageIndex = 1,
            PageSize = 25,
            OrderBy = DriverLicenseTypeOrderBy.Code,
            SortDirection = "ASC"

        };
    }

    public class DriverLicenseTypeExample : GetAllDriverLicenseTypeResponse
    {
        public static IEnumerable<GetAllDriverLicenseTypeResponse> Example => new[]
        {
           new GetAllDriverLicenseTypeResponse
                {
                    Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9d"),
                    Code = "A",
                    Description = "Motorcycle",
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 10, 15, 22, 12, 35)
                },
                new GetAllDriverLicenseTypeResponse
                {
                    Id = Guid.Parse("8b9a3171-26c5-47ad-8362-6991aedac1ea"),
                    Code = "AB",
                    Description = "Motorcycle and Car",
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 10, 15, 22, 12, 35)
                }
        };
    }

    public class QueryDriverLicenseTypeExample : PagedResult<GetAllDriverLicenseTypeResponse>
    {
        public QueryDriverLicenseTypeExample()
            : base(
                items: DriverLicenseTypeExample.Example,
                totalCount: 2,
                pageNumber: 1,
                pageSize: 25,
                orderBy: DriverLicenseTypeOrderBy.Code.ToString(),
                sortDirection: "ASC"
            )
        { }
    }

    public class GetAllDriverLicenseTypeResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            var pagedExample = new QueryDriverLicenseTypeExample();

            return new ApiResponse(
                success: true,
                messages: new[] { CommonMessages.Query_Successful },
                data: pagedExample
            );
        }
    }

    //Add
    public class AddDriverLicenseTypeRequestExamplo : IExamplesProvider<AddDriverLicenseTypeRequest>
    {
        public AddDriverLicenseTypeRequest GetExamples() => new AddDriverLicenseTypeRequest
        {
            Code = "A",
            Description = "Motorcycle"
        };
    }

    public class AddDriverLicenseTypeResponseExamplo : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { DriverLicenseTypeMessages.DriverLicenseType_Registered_Successfully },
            data: new AddDriverLicenseTypeResponse()
            {
                Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9d"),
                Code = "A",
                Description = "Motorcycle",
                IsActive = false,
                CreatedAt = new DateTime(2025, 10, 15, 22, 12, 35)
            }
        );
    }

    //Update
    public class UpdateDriverLicenseTypeRequestExamplo : IExamplesProvider<UpdateDriverLicenseTypeRequest>
    {
        public UpdateDriverLicenseTypeRequest GetExamples() => new UpdateDriverLicenseTypeRequest
        {
            Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9d"),
            Code = "A",
            Description = "Motorcycle"
        };
    }

    public class UpdateDriverLicenseTypeResponseExamplo : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { DriverLicenseTypeMessages.DriverLicenseType_Updated_Successfully },
            data: new UpdateDriverLicenseTypeResponse()
            {
                Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9d"),
                Code = "A",
                Description = "Motorcycle",
                IsActive = false,
                CreatedAt = new DateTime(2025, 10, 15, 22, 12, 35)
            }
        );
    }

    //Delete
    public class DeleteDriverLicenseTypeRequestExamplo : IExamplesProvider<DeleteDriverLicenseTypeRequest>
    {
        public DeleteDriverLicenseTypeRequest GetExamples() => new DeleteDriverLicenseTypeRequest
        {
            Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9d")
        };
    }

    public class DeleteDriverLicenseTypeResponseExamplo : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { DriverLicenseTypeMessages.DriverLicenseType_Deleted_Successfully },
            data: new DeleteDriverLicenseTypeResponse()
            {
                Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9d"),
                Code = "A",
                Description = "Motorcycle",
                IsActive = false,
                CreatedAt = new DateTime(2025, 10, 15, 22, 12, 35)
            }
        );
    }

}

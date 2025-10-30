using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Pagination;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Rental.Api.Swagger.Examples
{
    public class GetAllRentalPlanRequestExamplo : IExamplesProvider<GetAllRentalPlanRequest>
    {
        public GetAllRentalPlanRequest GetExamples() => new GetAllRentalPlanRequest
        {
            Days = 7,
            MinDailyRate = 30m,
            MaxDailyRate = 50m,
            MinPenaltyPercent = 5m,
            MaxPenaltyPercent = 20m,
            Description = "Motorcycle",
            StartDate = new DateTime(2025, 10, 15, 0, 0, 0),
            EndDate = new DateTime(2025, 10, 15, 23, 59, 59),
            PageIndex = 1,
            PageSize = 25,
            OrderBy = RentalPlanOrderBy.Days,
            SortDirection = "ASC"

        };
    }

    public class RentalPlanExample : GetAllRentalPlanResponse
    {
        public static IEnumerable<GetAllRentalPlanResponse> Example => new[]
        {
           new GetAllRentalPlanResponse
                {
                    Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9e"),
                    Days = 100,
                    DailyRate = 30m,
                    PenaltyPercent = 50m,
                    Description = "100-day plan - 100 days",
                    CreatedAt = new DateTime(2025, 10, 15, 22, 12, 35)
                },
                new GetAllRentalPlanResponse
                {
                    Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9f"),
                    Days = 50,
                    DailyRate = 30m,
                    PenaltyPercent = 50m,
                    Description = "50-day plan - 50 days",
                    CreatedAt = new DateTime(2025, 10, 15, 22, 12, 35)
                }
        };
    }

    public class QueryRentalPlanExample : PagedResult<GetAllRentalPlanResponse>
    {
        public QueryRentalPlanExample()
            : base(
                items: RentalPlanExample.Example,
                totalCount: 2,
                pageNumber: 1,
                pageSize: 25,
                orderBy: RentalPlanOrderBy.Days.ToString(),
                sortDirection: "ASC"
            )
        { }
    }

    public class GetAllRentalPlanResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            var pagedExample = new QueryRentalPlanExample();

            return new ApiResponse(
                success: true,
                messages: new[] { CommonMessages.Query_Successful },
                data: pagedExample
            );
        }
    }

    //Add
    public class AddRentalPlanRequestExamplo : IExamplesProvider<AddRentalPlanRequest>
    {
        public AddRentalPlanRequest GetExamples() => new AddRentalPlanRequest
        {
            Days = 100,
            DailyRate = 100m,
            PenaltyPercent = 50m,
            Description = "50-day plan - teste"
        };
    }

    public class AddRentalPlanResponseExamplo : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { RentalPlanMessages.RentalPlan_Registered_Successfully },
            data: new AddRentalPlanResponse()
            {
                Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9e"),
                Days = 100,
                DailyRate = 30m,
                PenaltyPercent = 50m,
                Description = "100-day plan - 100 days"
            }
        );
    }

    //Update
    public class UpdateRentalPlanRequestExamplo : IExamplesProvider<UpdateRentalPlanRequest>
    {
        public UpdateRentalPlanRequest GetExamples() => new UpdateRentalPlanRequest
        {
            Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9e"),
            DailyRate = 40m,
            PenaltyPercent = 50m,
            Description = "100-day plan - 100 days"

        };
    }

    public class UpdateRentalPlanResponseExamplo : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { RentalPlanMessages.RentalPlan_Updated_Successfully },
            data: new UpdateRentalPlanResponse()
            {
                Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9e"),
                Days = 100,
                DailyRate = 40m,
                PenaltyPercent = 50m,
                Description = "100-day plan - 100 days",
            }
        );
    }

    ////Delete
    public class DeleteRentalPlanRequestExamplo : IExamplesProvider<DeleteRentalPlanRequest>
    {
        public DeleteRentalPlanRequest GetExamples() => new DeleteRentalPlanRequest
        {
            Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9e")
        };
    }

    public class DeleteRentalPlanResponseExamplo : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { RentalPlanMessages.RentalPlan_Deleted_Successfully },
            data: new DeleteRentalPlanResponse()
            {
                Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9e"),
                Days = 100,
                DailyRate = 40m,
                PenaltyPercent = 50m,
                Description = "100-day plan - 100 days"
            }
        );
    }

}

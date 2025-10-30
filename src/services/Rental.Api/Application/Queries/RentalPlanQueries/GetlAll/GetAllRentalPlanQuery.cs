using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Pagination;
using System;

namespace Rental.Api.Application.Queries.RentalPlanQueries.GetAll
{
    public class GetAllRentalPlanQuery : PaginatedQueryBase<PagedResult<GetAllRentalPlanResponse>, RentalPlanOrderBy>
    {
        public int? Days { get; }
        public string? Description { get; }
        public decimal? MinDailyRate { get; }
        public decimal? MaxDailyRate { get; }
        public decimal? MinPenaltyPercent { get; set; }
        public decimal? MaxPenaltyPercent { get; set; }
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }

        public GetAllRentalPlanQuery(GetAllRentalPlanRequest request)
            : base(
                request.OrderBy ?? RentalPlanOrderBy.Id,
                request.SortDirection,
                request.PageIndex,
                request.PageSize)
        {
            Days = request.Days;
            Description = request.Description;
            MinDailyRate = request.MinDailyRate;
            MaxDailyRate = request.MaxDailyRate;
            StartDate = request.StartDate;
            EndDate = request.EndDate;
        }
    }
}

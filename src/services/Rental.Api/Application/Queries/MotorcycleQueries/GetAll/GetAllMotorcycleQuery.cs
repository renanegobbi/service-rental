using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Pagination;
using System;

namespace Rental.Api.Application.Queries.MotorcycleQueries.GetAll
{
    public class GetAllMotorcycleQuery : PaginatedQueryBase<PagedResult<GetAllMotorcycleResponse>, MotorcycleOrderBy>
    {
        public int? Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set;  }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public GetAllMotorcycleQuery(GetAllMotorcycleRequest request)
            : base(
                request.OrderBy ?? MotorcycleOrderBy.Year,
                request.SortDirection,
                request.PageIndex,
                request.PageSize)
        {
            Year = request.Year;
            Model = request.Model;
            Plate = request.Plate;
            StartDate = request.StartDate;
            EndDate = request.EndDate;
        }
    }
}

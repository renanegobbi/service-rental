using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Pagination;
using System;

namespace Rental.Api.Application.Queries.DriverLicenseTypeQueries.GetAll
{
    public class GetAllDriverLicenseTypeQuery : PaginatedQueryBase<PagedResult<GetAllDriverLicenseTypeResponse>, DriverLicenseTypeOrderBy>
    {
        public string Code { get; }
        public string Description { get; }
        public bool? IsActive { get; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public GetAllDriverLicenseTypeQuery(GetAllDriverLicenseTypeRequest request) : base(
            request.OrderBy ?? DriverLicenseTypeOrderBy.Id,
            request.SortDirection,
            request.PageIndex,
            request.PageSize)
        {
            Code = request.Code;
            Description = request.Description;
            IsActive = request.IsActive;
            StartDate = request.StartDate;
            EndDate = request.EndDate;
        }
    }
}

using Rental.Api.Application.DTOs.Courier;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Pagination;
using System;

namespace Rental.Api.Application.Queries.CourierQueries.GetAll
{
    public class GetAllCourierQuery : PaginatedQueryBase<PagedResult<GetAllCourierResponse>, CourierOrderBy>
    {
        public string FullName { get; set; }
        public string Cnpj { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string DriverLicenseNumber { get; set; }
        public Guid? DriverLicenseType { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public GetAllCourierQuery(GetAllCourierRequest request)
            : base(
                request.OrderBy ?? CourierOrderBy.FullName,
                request.SortDirection,
                request.PageIndex,
                request.PageSize)
        {
            FullName = request.FullName;
            Cnpj = request.Cnpj;
            BirthDate = request.BirthDate;
            DriverLicenseNumber = request.DriverLicenseNumber;
            DriverLicenseType = request.DriverLicenseType;
            CreatedAt = request.CreatedAt;
        }
    }
}

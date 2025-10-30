using MediatR;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Core.Pagination;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Queries.DriverLicenseTypeQueries.GetAll
{
    public class GetAllRentalPlanQueryHandler : IRequestHandler<GetAllDriverLicenseTypeQuery, PagedResult<GetAllDriverLicenseTypeResponse>>
    {
        private readonly IDriverLicenseTypeRepository _repo;

        public GetAllRentalPlanQueryHandler(IDriverLicenseTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<GetAllDriverLicenseTypeResponse>> Handle(GetAllDriverLicenseTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetAllAsync(request);

            var items = result.Item1.Select(x => new GetAllDriverLicenseTypeResponse
            {
                Id = x.Id,
                Code = x.Code,
                Description = x.Description,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            });

            return new PagedResult<GetAllDriverLicenseTypeResponse>(
                items,
                (int)result.Item2,
                request.PageIndex ?? 1,
                request.PageSize ?? 10,
                request.OrderBy.ToString(),
                request.SortDirection
            );
        }
    }
}

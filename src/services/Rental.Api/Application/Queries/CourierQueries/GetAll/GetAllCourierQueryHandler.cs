using MediatR;
using Rental.Api.Application.DTOs.Courier;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Core.Pagination;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Queries.CourierQueries.GetAll
{
    public class GetAllCourierQueryHandler : IRequestHandler<GetAllCourierQuery, PagedResult<GetAllCourierResponse>>
    {
        private readonly ICourierRepository _repository;

        public GetAllCourierQueryHandler(ICourierRepository repository)
        {
            _repository = repository;
        }
        public async Task<PagedResult<GetAllCourierResponse>> Handle(GetAllCourierQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(request);

            var items = result.Item1.Select(x => new GetAllCourierResponse
            {
                FullName = x.FullName,
                Cnpj = x.Cnpj,
                BirthDate = x.BirthDate,
                DriverLicenseNumber = x.DriverLicenseNumber,
                DriverLicenseType = x.DriverLicenseType,
                DriverLicenseImageUrl = x.DriverLicenseImageUrl,
                CreatedAt = x.CreatedAt
            });

            return new PagedResult<GetAllCourierResponse>(
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

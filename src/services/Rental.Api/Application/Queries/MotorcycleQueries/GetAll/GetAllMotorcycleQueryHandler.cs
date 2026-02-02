using MediatR;
using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Api.Infrastructure.Repository;
using Rental.Core.Pagination;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Queries.MotorcycleQueries.GetAll
{
    public class GetAllMotorcycleQueryHandler : IRequestHandler<GetAllMotorcycleQuery, PagedResult<GetAllMotorcycleResponse>>
    {
        private readonly IMotorcycleRepository _repository;

        public GetAllMotorcycleQueryHandler(IMotorcycleRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<GetAllMotorcycleResponse>> Handle(GetAllMotorcycleQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(request);

            var items = result.Item1.Select(x => new GetAllMotorcycleResponse
            {
                Id = x.Id,
                Year = x.Year,
                Model = x.Model,
                Plate = x.Plate,
                CreatedAt = x.CreatedAt
            });

            return new PagedResult<GetAllMotorcycleResponse>(
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

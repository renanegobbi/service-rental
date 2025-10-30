using MediatR;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Api.Infrastructure.Repository;
using Rental.Core.Pagination;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Queries.RentalPlanQueries.GetAll
{
    public class GetAllRentalPlanQueryHandler : IRequestHandler<GetAllRentalPlanQuery, PagedResult<GetAllRentalPlanResponse>>
    {
        private readonly IRentalPlanRepository _repository;

        public GetAllRentalPlanQueryHandler(IRentalPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<GetAllRentalPlanResponse>> Handle(GetAllRentalPlanQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(request);

            var items = result.Item1.Select(x => new GetAllRentalPlanResponse
            {
                Id = x.Id,
                Days = x.Days,
                DailyRate = x.DailyRate,
                PenaltyPercent = x.PenaltyPercent,
                Description = x.Description,
                CreatedAt = x.CreatedAt
            });

            return new PagedResult<GetAllRentalPlanResponse>(
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

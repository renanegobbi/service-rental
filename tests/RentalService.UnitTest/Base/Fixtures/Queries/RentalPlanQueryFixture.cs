using Moq;
using Rental.Api.Application.Queries.RentalPlanQueries.GetAll;
using Rental.Api.Entities;
using Rental.Api.Infrastructure.Repository;

namespace RentalService.Tests.Unit.Base.Fixtures.Queries
{
    public class RentalPlanQueryFixture : FixtureBase
    {
        public Mock<IRentalPlanRepository> RepositoryMock { get; private set; } = null!;
        public GetAllRentalPlanQueryHandler Handler { get; private set; } = null!;

        public RentalPlanQueryFixture()
        {
            Reset();
        }

        public override void Reset()
        {
            RepositoryMock = CreateMock<IRentalPlanRepository>();

            Handler = new GetAllRentalPlanQueryHandler(RepositoryMock.Object);
        }

        public Tuple<RentalPlan[], double> CreateRentalPlansTuple(params RentalPlan[] plans)
            => new Tuple<RentalPlan[], double>(plans, plans.Length);

        public Tuple<RentalPlan[], double> CreateEmptyPlansTuple()
            => new Tuple<RentalPlan[], double>(Array.Empty<RentalPlan>(), 0);
    }
}

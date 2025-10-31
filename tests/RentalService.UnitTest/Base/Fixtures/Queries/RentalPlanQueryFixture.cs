using Moq;
using Rental.Api.Application.Queries.RentalPlanQueries.GetAll;
using Rental.Api.Entities;
using Rental.Api.Infrastructure.Repository;

namespace RentalService.UnitTest.Base.Fixtures.Queries
{
    public class RentalPlanQueryFixture : FixtureBase
    {
        public Mock<IRentalPlanRepository> RepositoryMock { get; }
        public GetAllRentalPlanQueryHandler Handler { get; }

        public RentalPlanQueryFixture()
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

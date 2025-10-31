using Moq;
using Rental.Api.Application.Commands.RentalPlanCommands.Add;
using Rental.Api.Application.Commands.RentalPlanCommands.Update;
using Rental.Api.Application.Commands.RentalPlanCommands.Delete;
using Rental.Api.Infrastructure.Repository;
using Rental.Api.Entities;

namespace RentalService.Tests.Unit.Base.Fixtures.Commands
{
    public class RentalPlanCommandFixture : FixtureBase
    {
        public Mock<IRentalPlanRepository> RepositoryMock { get; private set; }
        public AddRentalPlanCommandHandler AddHandler { get; private set; }
        public UpdateRentalPlanCommandHandler UpdateHandler { get; private set; }
        public DeleteRentalPlanCommandHandler DeleteHandler { get; private set; }

        public RentalPlanCommandFixture()
        {
            Reset();
        }

        public override void Reset()
        {
            RepositoryMock = CreateMock<IRentalPlanRepository>();
            AddHandler = new AddRentalPlanCommandHandler(RepositoryMock.Object);
            UpdateHandler = new UpdateRentalPlanCommandHandler(RepositoryMock.Object);
            DeleteHandler = new DeleteRentalPlanCommandHandler(RepositoryMock.Object);
        }

        public RentalPlan CreateValidPlan()
            => new RentalPlan(7, 50, 20, "Weekly plan");

        public RentalPlan CreateUpdatedPlan()
            => new RentalPlan(15, 45, 10, "Updated plan");
    }
}

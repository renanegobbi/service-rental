using Moq;
using Rental.Api.Controllers.V1;
using Rental.Core.Mediator;
using Rental.Core.Responses;

namespace RentalService.UnitTest.Base.Fixtures.Controllers
{
    public class RentalPlanControllerFixture : FixtureBase
    {
        public Mock<IMediatorHandler> MediatorMock { get; }
        public RentalPlanController Controller { get; }

        public RentalPlanControllerFixture()
        {
            MediatorMock = CreateMock<IMediatorHandler>();
            Controller = new RentalPlanController(MediatorMock.Object);
        }

        public ApiResponse CreateSuccessResponse(string message)
            => new ApiResponse(true, new[] { message }, null);

        public ApiResponse CreateFailResponse(string message)
            => new ApiResponse(false, new[] { message }, null);

    }
}

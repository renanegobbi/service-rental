using FluentAssertions;
using Moq;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Api.Application.Queries.RentalPlanQueries.GetAll;
using RentalService.Tests.Unit.Base.Fixtures.Queries;
using EntityRentalPlan = Rental.Api.Entities.RentalPlan;

namespace RentalService.Tests.Unit.Queries.RentalPlan
{
    public class GetAllRentalPlanQueryHandlerTests : IClassFixture<RentalPlanQueryFixture>
    {
        private readonly RentalPlanQueryFixture _fixture;

        public GetAllRentalPlanQueryHandlerTests(RentalPlanQueryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenRepositoryReturnsData()
        {
            _fixture.Reset();

            // Arrange
            var request = new GetAllRentalPlanRequest { PageIndex = 1, PageSize = 10 };
            var query = new GetAllRentalPlanQuery(request);

            var plans = new[]
            {
                new EntityRentalPlan(7, 50, 20, "Weekly"),
                new EntityRentalPlan(15, 45, 15, "Biweekly")
            };

            _fixture.RepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<GetAllRentalPlanQuery>()))
                .ReturnsAsync(_fixture.CreateRentalPlansTuple(plans));

            // Act
            var result = await _fixture.Handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.First().Description.Should().Be("Weekly");
            result.TotalCount.Should().Be(2);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);

            _fixture.RepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<GetAllRentalPlanQuery>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenNoDataFound()
        {
            _fixture.Reset();

            // Arrange
            var request = new GetAllRentalPlanRequest { PageIndex = 1, PageSize = 10 };
            var query = new GetAllRentalPlanQuery(request);

            _fixture.RepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<GetAllRentalPlanQuery>()))
                .ReturnsAsync(_fixture.CreateEmptyPlansTuple());

            // Act
            var result = await _fixture.Handler.Handle(query, CancellationToken.None);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
    }
}

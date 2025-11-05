using Moq;
using Rental.Api.Data.Cache;

namespace RentalService.Tests.Unit.Cache
{
    public class CacheInvalidationHelperTests
    {
        [Fact]
        public async Task InvalidateEntityAsync_ShouldDeleteId_Code_AndPrefix()
        {
            // Arrange
            var cacheMock = new Mock<ICacheService<object>>();
            var id = Guid.NewGuid();

            // Act
            await CacheInvalidationHelper.InvalidateEntityAsync(cacheMock.Object, "DriverLicenseType", id, "A");

            // Assert
            cacheMock.Verify(c => c.KeyDeleteAsync($"DriverLicenseType:Id:{id}"), Times.Once);
            cacheMock.Verify(c => c.KeyDeleteAsync($"DriverLicenseType:Code:A"), Times.Once);
            cacheMock.Verify(c => c.KeyDeleteByPrefixAsync("DriverLicenseType:GetAll"), Times.Once);
        }

        [Fact]
        public async Task InvalidateEntityAsync_ShouldSkipCode_WhenNull()
        {
            var cacheMock = new Mock<ICacheService<object>>();
            var id = Guid.NewGuid();

            await CacheInvalidationHelper.InvalidateEntityAsync(cacheMock.Object, "DriverLicenseType", id, null);

            cacheMock.Verify(c => c.KeyDeleteAsync(It.Is<string>(x => x.Contains(":Code:"))), Times.Never);
        }
    }
}

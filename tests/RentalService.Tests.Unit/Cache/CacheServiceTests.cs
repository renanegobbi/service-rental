using FluentAssertions;
using Moq;
using Rental.Api.Data.Cache;

namespace RentalService.Tests.Unit.Cache
{
    public class CacheServiceTests
    {
        [Fact]
        public async Task GetByKeyAsync_ShouldReturnCachedValue_WhenExists()
        {
            // Arrange
            var redisMock = new Mock<IRedisCacheService>();
            var expected = new List<string> { "cached" };

            redisMock.Setup(r => r.GetOrSetAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<List<string>>>>(),
                    null))
                .ReturnsAsync(expected);

            var cacheService = new CacheService<string>(redisMock.Object);

            // Act
            var result = await cacheService.GetByKeyAsync("Key", async () => new() { "new" });

            // Assert
            result.Should().BeEquivalentTo(expected);
            redisMock.Verify(r => r.GetOrSetAsync("Key", It.IsAny<Func<Task<List<string>>>>(), null), Times.Once);
        }

        [Fact]
        public async Task GetByKeyAsync_ShouldInvokeFactory_WhenCacheIsEmpty()
        {
            // Arrange
            var redisMock = new Mock<IRedisCacheService>();

            redisMock.Setup(r => r.GetOrSetAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<List<string>>>>(),
                    null))
                .Returns(async (string _, Func<Task<List<string>>> factory, TimeSpan? _) => await factory());

            var cacheService = new CacheService<string>(redisMock.Object);
            var factoryCalled = false;

            // Act
            var result = await cacheService.GetByKeyAsync("Key", async () =>
            {
                factoryCalled = true;
                return new() { "generated" };
            });

            // Assert
            factoryCalled.Should().BeTrue();
            result.Should().Contain("generated");
        }
    }
}

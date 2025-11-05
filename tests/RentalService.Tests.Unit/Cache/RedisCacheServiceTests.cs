using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Rental.Api.Data.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RentalService.Tests.Unit.Cache
{
    public class RedisCacheServiceTests
    {
        [Fact]
        public async Task StringSetAsync_ShouldSerializeAndStoreValue_WithoutChangingConstructor()
        {
            // Arrange
            var dbMock = new Mock<IDatabase>();
            dbMock.Setup(d => d.StringSetAsync(
                    It.IsAny<RedisKey>(),
                    It.IsAny<RedisValue>(),
                    null, false, When.Always, CommandFlags.None))
                  .ReturnsAsync(true);

            var logger = Mock.Of<ILogger<RedisCacheService>>();
            var config = new Mock<IConfiguration>();
            config.Setup(x => x["CacheSettings:Host"]).Returns("localhost");
            config.Setup(x => x["CacheSettings:Port"]).Returns("6379");

            var redis = new RedisCacheService(config.Object, logger);

            var dbField = typeof(RedisCacheService)
                .GetField("_db", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            dbField!.SetValue(redis, dbMock.Object);

            // Act
            var result = await redis.StringSetAsync("key", new { Name = "Test" });

            // Assert
            result.Should().BeTrue();
            dbMock.Verify(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                null, false, When.Always, CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task StringGetAsync_ShouldDeserializeObject()
        {
            // Arrange
            var expected = new { Name = "Renan" };
            var serialized = JsonSerializer.Serialize(expected);

            var dbMock = new Mock<IDatabase>();
            dbMock.Setup(d => d.StringGetAsync("key", CommandFlags.None))
                .ReturnsAsync(serialized);

            var connMock = new Mock<IConnectionMultiplexer>();
            connMock.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(dbMock.Object);

            var logger = Mock.Of<Microsoft.Extensions.Logging.ILogger<RedisCacheService>>();
            var config = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            config.Setup(x => x["CacheSettings:Host"]).Returns("localhost");

            var redis = new RedisCacheService(config.Object, logger);

            // Act
            var result = await redis.StringGetAsync<dynamic>("key");

            // Assert
            result?.Name.ToString().Should().Be("Renan");
        }
    }
}

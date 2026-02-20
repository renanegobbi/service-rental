using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rental.Api.Data.Cache
{
    /// <summary>
    /// Centralizes the logic for clearing entity caches in Redis.
    /// </summary>
    public static class CacheInvalidationHelper
    {
        /// <summary>
        /// Invalidates all cache keys related to a specific entity.
        /// </summary>
        public static async Task TryInvalidateEntityAsync<T>(
            ICacheService<T> cache,
            string entityName,
            ILogger logger,
            Guid? id = null,
            string? code = null)
            where T : class
        {
            try
            {
                if (id.HasValue)
                    await cache.KeyDeleteAsync($"{entityName}:Id:{id.Value}");

                if (!string.IsNullOrEmpty(code))
                    await cache.KeyDeleteAsync($"{entityName}:Code:{code}");

                await cache.KeyDeleteByPrefixAsync($"{entityName}:GetAll");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache invalidation failed for entity {Entity}", entityName);
            }
        }


        /// <summary>
        /// Invalidates all cache keys related to a specific entity.
        /// </summary>
        public static async Task InvalidateEntityAsync<T>(
            ICacheService<T> cache,
            string entityName)
            where T : class
        {
            // Clear all list caches (GetAll)
            await cache.KeyDeleteByPrefixAsync($"{entityName}:GetAll");
        }
    }
}

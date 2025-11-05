using System;
using System.Threading.Tasks;

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
        public static async Task InvalidateEntityAsync<T>(
            ICacheService<T> cache,
            string entityName,
            Guid id,
            string? code = null)
            where T : class
        {
            // Clear cache for the record by ID
            await cache.KeyDeleteAsync($"{entityName}:Id:{id}");

            // Clear cache for the record by code (if it exists)
            if (!string.IsNullOrEmpty(code))
                await cache.KeyDeleteAsync($"{entityName}:Code:{code}");

            // Clear all list caches (GetAll)
            await cache.KeyDeleteByPrefixAsync($"{entityName}:GetAll");
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

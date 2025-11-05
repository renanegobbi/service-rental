using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Rental.Api.Data.Cache
{
    public class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly IRedisCacheService _redis;

        public CacheService(IRedisCacheService redis)
        {
            _redis = redis;
        }

        public Task<List<T>> GetByKeyAsync(string key, Func<Task<List<T>>> getData, TimeSpan? expiry = null)
        {
            return _redis.GetOrSetAsync(key, getData, expiry);
        }

        public Task<bool> KeyDeleteAsync(string key)
           => _redis.KeyDeleteAsync(key);

        public Task KeyDeleteByPrefixAsync(string prefix)
            => _redis.KeyDeleteByPrefixAsync(prefix);
    }
}

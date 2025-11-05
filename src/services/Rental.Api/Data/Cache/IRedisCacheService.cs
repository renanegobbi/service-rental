using System.Threading.Tasks;
using System;

namespace Rental.Api.Data.Cache
{
    public interface IRedisCacheService
    {
        Task<T?> StringGetAsync<T>(string key);
        Task<bool> StringSetAsync(string key, object value, TimeSpan? expiry = null);
        Task<bool> KeyDeleteAsync(string key);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null);
        Task KeyDeleteByPrefixAsync(string prefix);
    }
}

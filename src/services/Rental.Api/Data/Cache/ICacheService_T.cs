using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Rental.Api.Data.Cache
{
    public interface ICacheService<T> where T : class
    {
        Task<List<T>> GetByKeyAsync(string key, Func<Task<List<T>>> getData, TimeSpan? expiry = null);
        Task<bool> KeyDeleteAsync(string key);
        Task KeyDeleteByPrefixAsync(string prefix);
    }
}

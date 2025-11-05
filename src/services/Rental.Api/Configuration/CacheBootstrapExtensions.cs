using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rental.Api.Data.Cache;
using System;

namespace Rental.Api.Configuration
{
    public static class CacheBootstrapExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));

            var ttlHours = configuration.GetSection("Cache")?.GetValue<int?>("DefaultTtlHours") ?? 24;
            services.AddSingleton(new CacheDefaults { DefaultTtl = TimeSpan.FromHours(ttlHours) });

            return services;
        }
    }

    public sealed class CacheDefaults
    {
        public TimeSpan DefaultTtl { get; init; }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RedisDatabase = StackExchange.Redis.IDatabase;

namespace Rental.Api.Data.Cache
{
    public class RedisCacheService : IRedisCacheService, IDisposable
    {
        private readonly ILogger<RedisCacheService> _logger;
        private readonly ConnectionMultiplexer _connection;
        private readonly RedisDatabase _db;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisCacheService(IConfiguration configuration, ILogger<RedisCacheService> logger)
        {
            _logger = logger;

            //  Lendo da seção CacheSettings
            var host = configuration["CacheSettings:Host"] ?? "localhost";
            var port = configuration["CacheSettings:Port"] ?? "6379";
            var password = configuration["CacheSettings:Password"];

            // Monta a string de conexão manualmente
            var connStr = string.IsNullOrEmpty(password)
                ? $"{host}:{port},abortConnect=false"
                : $"{host}:{port},password={password},abortConnect=false";

            var options = ConfigurationOptions.Parse(connStr, ignoreUnknown: true);

            _connection = ConnectionMultiplexer.Connect(options);
            _db = _connection.GetDatabase();

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        public async Task<T?> StringGetAsync<T>(string key)
        {
            try
            {
                var value = await _db.StringGetAsync(key);
                if (value.IsNullOrEmpty) return default;
                return JsonSerializer.Deserialize<T>(value!, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GET falhou para key {Key}", key);
                return default;
            }
        }

        public async Task<bool> StringSetAsync(string key, object value, TimeSpan? expiry = null)
        {
            try
            {
                var json = JsonSerializer.Serialize(value, _jsonOptions);
                return await _db.StringSetAsync(key, json, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis SET falhou para key {Key}", key);
                return false;
            }
        }

        public Task<bool> KeyDeleteAsync(string key) => _db.KeyDeleteAsync(key);

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
        {
            var cached = await StringGetAsync<T>(key);
            if (cached is not null && !cached.Equals(default(T))) return cached;

            var data = await factory();
            await StringSetAsync(key, data!, expiry);
            return data;
        }

        public async Task KeyDeleteByPrefixAsync(string prefix)
        {
            var endpoints = _connection.GetEndPoints();
            var server = _connection.GetServer(endpoints.First());
            foreach (var key in server.Keys(pattern: $"{prefix}*"))
            {
                await _db.KeyDeleteAsync(key);
            }
        }

        public void Dispose() => _connection?.Dispose();
    }
}

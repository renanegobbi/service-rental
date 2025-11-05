using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using Rental.Core.Enums;
using Rental.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Rental.Api.Configuration
{
    public static class HealthCheckConfig
    {
        private static string _postgresPort;
        private static string _rabbitPort;
        private static string _redisHost;
        private static string _redisPort;
        private static string _redisContainer;
        private static string _minioPort;

        public static void AddHealthCheckConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            _postgresPort = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection") ?? "").Port.ToString() ?? "5432";
            _rabbitPort = int.TryParse((new Uri(configuration["MessageQueueConnection:HealthCheck"] ?? "amqp://guest:guest@localhost:5672")).Port.ToString(), out var rPort) ? rPort.ToString() : "5672";
            _redisHost = configuration["CacheSettings:Host"] ?? "localhost";
            _redisPort = configuration["CacheSettings:Port"] ?? "6379";
            _redisContainer = configuration["CacheSettings:Container"] ?? "redis";
            _minioPort = int.TryParse(new Uri(configuration["Storage:Endpoint"] ?? "http://localhost:9000").Port.ToString(), out var mPort) ? mPort.ToString() : "9000";

            services.AddHealthChecks()

                .AddNpgSql(
                    connectionString: configuration.GetConnectionString("DefaultConnection"),
                    name: "PostgreSQL",
                    tags: new[] { "db", "postgres" })

                .AddRabbitMQ(
                    rabbitConnectionString: configuration["MessageQueueConnection:HealthCheck"],
                    name: "RabbitMQ",
                    tags: new[] { "mq", "rabbitmq" })

                .AddUrlGroup(
                        new Uri($"{configuration["Storage:Endpoint"]}/minio/health/live"),
                        name: "MinIO",
                        tags: new[] { "storage", "minio" })

                .AddRedis(
                        redisConnectionString: $"{configuration["CacheSettings:Host"]}:{configuration["CacheSettings:Port"]},password={configuration["CacheSettings:Password"]},abortConnect=false",
                        name: "Redis",
                        tags: new[] { "cache", "redis" })

                .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

            services.AddHealthChecksUI()
                    .AddInMemoryStorage();
        }

        public static void UseHealthCheckConfiguration(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var details = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = GetCustomDescription(entry)
                    });

                    var response = new
                    {
                        status = report.Status.ToString(),
                        message = report.Status switch
                        {
                            HealthStatus.Healthy => HealthCheckMessages.Status_AllOperational,
                            HealthStatus.Degraded => HealthCheckMessages.Status_PartiallyUnavailable,
                            HealthStatus.Unhealthy => HealthCheckMessages.Status_Unavailable,
                            _ => HealthCheckMessages.Status_Unknown
                        },
                        checkedAt = DateTime.UtcNow,
                        details
                    };

                    var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });

                    await context.Response.WriteAsync(json);
                }
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/health-ui";
            });
        }

        private static string GetCustomDescription(KeyValuePair<string, HealthReportEntry> entry)
        {
            var name = entry.Key;
            var status = entry.Value.Status;

            if (status == HealthStatus.Healthy)
                return $"{name} {HealthCheckMessages.Service_Healthy}";

            if (IsService(name, ServiceType.PostgreSQL))
                return string.Format(HealthCheckMessages.Service_PostgresDown, _postgresPort);

            if (IsService(name, ServiceType.RabbitMQ))
                return string.Format(HealthCheckMessages.Service_RabbitDown, _rabbitPort);

            if (IsService(name, ServiceType.Redis))
                return string.Format(HealthCheckMessages.Service_RedisDown, _redisContainer, _redisHost, _redisPort);

            if (IsService(name, ServiceType.MinIO))
                return string.Format(HealthCheckMessages.Service_MinIODown, _minioPort);

            if (IsService(name, ServiceType.Self))
                return string.Format(HealthCheckMessages.Service_ApiOk);

            return $"{name} is unavailable or degraded. {entry.Value.Exception?.Message ?? entry.Value.Description ?? "Check service logs."}";
        }

        private static bool IsService(string name, ServiceType service)
            => name.Contains(service.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}

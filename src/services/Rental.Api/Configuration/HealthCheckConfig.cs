using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rental.Api.Configuration
{
    public static class HealthCheckConfig
    {
        public static void AddHealthCheckConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
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

                .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

            services.AddHealthChecksUI()
                    .AddInMemoryStorage();
        }

        public static void UseHealthCheckConfiguration(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/health-ui";
            });
        }
    }
}

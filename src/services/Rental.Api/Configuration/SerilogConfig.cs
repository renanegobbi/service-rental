using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using System;

namespace Rental.Api.Configuration
{
    public static class SerilogConfig
    {
        public static void AddSerilogConfiguration(
            this IHostBuilder host, IConfiguration configuration, IHostEnvironment env)
        {
            var lokiUrl = configuration["Logging:Loki:Url"] ?? "http://localhost:3100";

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("🔍 [SERILOG DEBUG] " + msg);
                Console.ResetColor();
            });

            var lokiLabels = new[]
                {
                    new LokiLabel { Key = "app", Value = "rental-service" },
                    new LokiLabel { Key = "env", Value = env.EnvironmentName }
                };

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "RentalService.Api")
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .CreateLogger();
           
            host.UseSerilog();
        }
    }
}

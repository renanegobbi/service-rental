using Rental.Api.Configuration;
using Rental.Services.Storage;

namespace Rental.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            builder.Services.AddApiConfiguration(builder.Configuration);
            builder.Services.AddSwaggerConfiguration();
            builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
            builder.Services.RegisterServices();
            builder.Services.AddMessageBusConfiguration(builder.Configuration);
            builder.Services.AddStorage(builder.Configuration);

            var app = builder.Build();

            app.UseSwaggerConfiguration();
            app.UseApiConfiguration(app.Environment);

            app.Run();
        }
    }
}

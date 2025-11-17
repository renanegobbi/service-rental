using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rental.Api.Data;
using Rental.Api.Middlewares;
using System.Net;
using System.Text.Json.Serialization;
using Rental.Services.Identity;

namespace Rental.Api.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddRouting(options => options.LowercaseUrls = true);

            services
                .AddControllers(options => options.Filters.Add(typeof(CustomModelStateValidationFilterAttribute)))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true
            );

            services.AddDbContext<RentalContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });
        }
        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseExceptionHandler($"/feedback/{(int)HttpStatusCode.InternalServerError}");
            app.UseStatusCodePagesWithReExecute("/feedback/{0}");

            app.UseHttpsRedirection();

            app.UsePathBase("/api/rentalservice");

            app.UseResponseCompression();

            app.UseRouting();

            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseAuthConfiguration();

            app.UseCors("Total");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
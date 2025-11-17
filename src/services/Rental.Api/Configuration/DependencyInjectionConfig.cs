using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rental.Api.Application.Services.Audit;
using Rental.Api.Data;
using Rental.Api.Data.Repositories;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Infrastructure.Repository;
using Rental.Core.Mediator;
using Rental.Services.User;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using static Rental.Api.Configuration.SwaggerConfig;
using _AuthenticationService = Rental.Api.Application.Services.Auth.AuthenticationService;

namespace Rental.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSwaggerExamplesFromAssemblyOf<Program>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly()
                )
            );

            services.AddScoped<_AuthenticationService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IRentalPlanRepository, RentalPlanRepository>();
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<IDriverLicenseTypeRepository, DriverLicenseTypeRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            services.AddScoped<RentalContext>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}
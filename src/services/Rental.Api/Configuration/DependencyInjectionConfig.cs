using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rental.Api.Data;
using Rental.Api.Data.Repositories;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Core.Mediator;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using static Rental.Api.Configuration.SwaggerConfig;

namespace Rental.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSwaggerExamplesFromAssemblyOf<Program>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            //services.AddScoped<IRequestHandler<RegisterMotorcycleCommand, ValidationResult>, MotorcycleCommandHandler>();
            //services.AddScoped<IRequestHandler<RegisterCourierCommand, ValidationResult>, CourierCommandHandler>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly()
                )
            );

            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<IDriverLicenseTypeRepository, DriverLicenseTypeRepository>();
            services.AddScoped<RentalContext>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}
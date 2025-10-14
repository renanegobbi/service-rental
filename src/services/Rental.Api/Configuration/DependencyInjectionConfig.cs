using FluentValidation.Results;
using MediatR;
using Rental.Api.Application.Commands.CourierCommands;
using Rental.Api.Application.Commands.MotocycleCommands;
using Rental.Api.Data;
using Rental.Api.Data.Repositories;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Core.Mediator;

namespace Rental.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegisterMotorcycleCommand, ValidationResult>, MotorcycleCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterCourierCommand, ValidationResult>, CourierCommandHandler>();

            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<RentalContext>();
        }
    }
}
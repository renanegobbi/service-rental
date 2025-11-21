using Rental.Api.Application.Commands.MotorcycleCommands.Add;
using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Api.Entities;

namespace Rental.Api.Application.Extensions
{
    public static class MotorcycleExtensions
    {
        public static Motorcycle ToMotorcycle(this AddMotorcycleCommand command)
        {
            return new Motorcycle(
                year: command.Year,
                model: command.Model,
                plate: command.Plate
            );
        }

        public static AddMotorcycleResponse ToAddMotorcycleResponse(this Motorcycle motorcycle)
        {
            return new AddMotorcycleResponse
            {
                Id = motorcycle.Id,
                Year = motorcycle.Year,
                Model = motorcycle.Model,
                Plate = motorcycle.Plate,
                CreatedAt = motorcycle.CreatedAt
            };
        }
    }
}

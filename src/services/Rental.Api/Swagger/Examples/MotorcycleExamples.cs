using Microsoft.AspNetCore.Http.HttpResults;
using Rental.Api.Application.DTOs.Motorcycle;
using Rental.Core.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Rental.Api.Swagger.Examples
{
    public class AddMotorcycleRequestExample : IExamplesProvider<AddMotorcycleRequest>
    {
        public AddMotorcycleRequest GetExamples() => new AddMotorcycleRequest
        {
            Year = 2023,
            Model = "Honda CG 160 Fan",
            Plate = "ABC1D23"
        };
    }

    public class AddMotorcycleResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { "Motorcycle registered successfully." },
            data: new AddMotorcycleResponse
            {
                Id = Guid.Parse("b39b592b-2116-4843-8959-d4919c092a9e"),
                Year = 2023,
                Model = "Honda CG 160 Fan",
                Plate = "ABC1D23",
                CreatedAt = DateTime.Parse("2025-11-21T15:18:32.6753229Z")
            }
        );
    }
}

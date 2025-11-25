using Rental.Api.Swagger;
using System;

namespace Rental.Api.Application.DTOs.Motorcycle
{
    public class GetAllMotorcycleResponse : IExposeInSwagger
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

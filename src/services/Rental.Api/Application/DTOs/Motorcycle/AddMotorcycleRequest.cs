using Rental.Api.Swagger;

namespace Rental.Api.Application.DTOs.Motorcycle
{
    public class AddMotorcycleRequest : IExposeInSwagger
    {
        /// <summary>
        /// Manufacturing year of the motorcycle (must be greater than 2000).
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Motorcycle model (e.g., "Honda CG 160").
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// License plate (e.g., "ABC1D23").
        /// </summary>
        public string Plate { get; set; }
    }
}

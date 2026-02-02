using Rental.Api.Swagger;
using System;

namespace Rental.Api.Application.DTOs.Courier
{
    public class GetAllCourierResponse : IExposeInSwagger
    {
        public string FullName { get; set; }
        public string Cnpj { get; set; }
        public DateOnly BirthDate { get; set; }
        public string DriverLicenseNumber { get; set; }
        public Guid? DriverLicenseType { get; set; }
        public string? DriverLicenseImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

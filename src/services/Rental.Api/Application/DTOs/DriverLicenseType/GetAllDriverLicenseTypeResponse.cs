using Rental.Api.Swagger;
using System;

namespace Rental.Api.Application.DTOs.DriverLicenseType
{
    public class GetAllDriverLicenseTypeResponse : IExposeInSwagger
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using Rental.Api.Swagger;
using System;

namespace Rental.Api.Application.DTOs.DriverLicenseType
{
    public class DeleteDriverLicenseTypeRequest : IExposeInSwagger
    {
        public Guid Id { get; set; }
    }
}

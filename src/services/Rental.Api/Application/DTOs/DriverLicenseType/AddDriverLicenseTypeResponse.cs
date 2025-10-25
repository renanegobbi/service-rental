using Rental.Core.Interfaces;
using System;

namespace Rental.Api.Application.DTOs.DriverLicenseType
{
    public class AddDriverLicenseTypeResponse: IResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

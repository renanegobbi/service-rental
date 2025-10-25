using System;

namespace Rental.Api.Application.DTOs.Courier
{
    public class RegisterCourierRequest
    {
        public string FullName { get; set; }
        public string Cnpj { get; set; }
        public DateTime BirthDate { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string DriverLicenseType { get; set; }
    }
}

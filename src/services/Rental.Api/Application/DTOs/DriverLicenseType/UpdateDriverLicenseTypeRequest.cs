using System;

namespace Rental.Api.Application.DTOs.DriverLicenseType
{
    public class UpdateDriverLicenseTypeRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// License code (A, B, AB...)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Description of the license type
        /// </summary>
        public string Description { get; set; }
    }
}

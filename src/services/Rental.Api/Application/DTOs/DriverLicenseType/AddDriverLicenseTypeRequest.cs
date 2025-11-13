using Rental.Api.Swagger;

namespace Rental.Api.Application.DTOs.DriverLicenseType
{
    public class AddDriverLicenseTypeRequest : IExposeInSwagger
    {
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

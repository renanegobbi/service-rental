using Rental.Core.Messages.Integration.Common;

namespace Rental.Core.Messages.Integration.Rental
{
    public class CourierRegisteredIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string Cnpj { get; private set; }
        public string DriverLicenseNumber { get; private set; }
        public string DriverLicenseType { get; private set; }
        public string? DriverLicenseImageUrl { get; private set; }

        public CourierRegisteredIntegrationEvent(
            Guid id,
            string fullName,
            string cnpj,
            string driverLicenseNumber,
            string driverLicenseType,
            string? driverLicenseImageUrl = null)
        {
            AggregateId = id;
            Id = id;
            FullName = fullName;
            Cnpj = cnpj;
            DriverLicenseNumber = driverLicenseNumber;
            DriverLicenseType = driverLicenseType;
            DriverLicenseImageUrl = driverLicenseImageUrl;
        }
    }
}

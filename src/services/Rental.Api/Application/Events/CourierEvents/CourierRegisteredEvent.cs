using Rental.Core.Messages;

namespace Rental.Api.Application.Events.CourierEvent
{
    public class CourierRegisteredEvent : Event
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string DriverLicenseNumber { get; private set; }
        public string DriverLicenseType { get; private set; }
        public string? DriverLicenseImageUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public CourierRegisteredEvent(
            Guid id,
            string fullName,
            string cnpj,
            DateTime birthDate,
            string driverLicenseNumber,
            string driverLicenseType,
            string? driverLicenseImageUrl = null)
        {
            AggregateId = id;
            Id = id;
            FullName = fullName;
            Cnpj = cnpj;
            BirthDate = birthDate;
            DriverLicenseNumber = driverLicenseNumber;
            DriverLicenseType = driverLicenseType;
            DriverLicenseImageUrl = driverLicenseImageUrl;
        }
    }
}

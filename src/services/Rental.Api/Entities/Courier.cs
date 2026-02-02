using Rental.Core.DomainObjects;
using System;

namespace Rental.Api.Entities
{
    public class Courier : Entity, IAggregateRoot
    {
        public string FullName { get; private set; }
        public string Cnpj { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public string DriverLicenseNumber { get; private set; }
        public Guid? DriverLicenseType { get; private set; }
        public string? DriverLicenseImageUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected Courier() { }

        public Courier(string fullName,
                       string cnpj,
                       DateOnly birthDate,
                       string driverLicenseNumber,
                       Guid? driverLicenseType,
                       string? driverLicenseImageUrl = null)
        {
            FullName = fullName;
            Cnpj = cnpj;
            BirthDate = birthDate;
            DriverLicenseNumber = driverLicenseNumber;
            DriverLicenseType = driverLicenseType;
            DriverLicenseImageUrl = driverLicenseImageUrl;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateInfo(string fullName,
                               Guid? driverLicenseType,
                               string? driverLicenseImageUrl = null)
        {
            FullName = fullName;
            DriverLicenseType = driverLicenseType;
            DriverLicenseImageUrl = driverLicenseImageUrl;
        }

        public void UpdateDriverLicenseImage(string objectKey)
        {
            DriverLicenseImageUrl = objectKey;
        }

        public void RemoveDriverLicenseImage()
        {
            DriverLicenseImageUrl = null;
        }
    }
}

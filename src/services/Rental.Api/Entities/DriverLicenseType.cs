using Rental.Core.DomainObjects;
using System;

namespace Rental.Api.Entities
{
    public class DriverLicenseType : Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected DriverLicenseType() { }

        public DriverLicenseType(string code, string description, bool isActive, DateTime createdAt)
        {
            Code = code;
            Description = description;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        public void Update(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public void RemoveDriverLicenseType()
        {
            IsActive = false;
        }
    }
}
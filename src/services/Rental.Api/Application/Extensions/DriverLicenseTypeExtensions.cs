using Rental.Api.Application.Commands.DriverLicenseTypeCommands.Add;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Api.Entities;
using Rental.Core.DomainObjects;
using System;

namespace Rental.Api.Application.Extensions
{
    public static class DriverLicenseTypeExtensions
    {
        public static DriverLicenseType ToDriverLicenseType(this AddDriverLicenseTypeCommand command)
        {
            if (command == null) return null;

            return new DriverLicenseType(command.Code.Trim().ToUpper(), command.Description, isActive: true, createdAt: DateTime.Now);
        }

        public static AddDriverLicenseTypeResponse ToAddDriverLicenseTypeResponse(this DriverLicenseType driverLicenseType)
        {
            if (driverLicenseType == null) return null;

            return new AddDriverLicenseTypeResponse
            {
                Id = driverLicenseType.Id,
                Code = driverLicenseType.Code,
                Description = driverLicenseType.Description,
                IsActive = driverLicenseType.IsActive,
                CreatedAt = driverLicenseType.CreatedAt
            };
        }

        public static UpdateDriverLicenseTypeResponse ToUpdateDriverLicenseTypeResponse(this DriverLicenseType driverLicenseType)
        {
            if (driverLicenseType == null) return null;

            return new UpdateDriverLicenseTypeResponse
            {
                Id = driverLicenseType.Id,
                Code = driverLicenseType.Code,
                Description = driverLicenseType.Description,
                IsActive = driverLicenseType.IsActive,
                CreatedAt = driverLicenseType.CreatedAt
            };
        }

        public static DeleteDriverLicenseTypeResponse ToDeleteDriverLicenseTypeResponse(this DriverLicenseType driverLicenseType)
        {
            if (driverLicenseType == null) return null;

            return new DeleteDriverLicenseTypeResponse
            {
                Id = driverLicenseType.Id,
                Code = driverLicenseType.Code,
                Description = driverLicenseType.Description,
                IsActive = driverLicenseType.IsActive,
                CreatedAt = driverLicenseType.CreatedAt
            };
        }

    }
}
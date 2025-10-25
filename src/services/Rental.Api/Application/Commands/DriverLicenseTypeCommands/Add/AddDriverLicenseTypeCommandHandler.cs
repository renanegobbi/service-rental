using FluentValidation.Results;
using MediatR;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Entities;
using Rental.Core.DomainObjects.Enums;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Add
{
    public class AddDriverLicenseTypeCommandHandler : CommandHandler,
        IRequestHandler<AddDriverLicenseTypeCommand, IResponse>
    {
        private readonly IDriverLicenseTypeRepository _driverlicenseTypeRepository;

        public AddDriverLicenseTypeCommandHandler(IDriverLicenseTypeRepository driverlicenseTypeRepository)
        {
            _driverlicenseTypeRepository = driverlicenseTypeRepository;
        }

        public async Task<IResponse> Handle(AddDriverLicenseTypeCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid()) 
                return Response.Fail(command.ValidationResult);

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            var driverLicenseType = new DriverLicenseType(
                code: command.Code.Trim().ToUpper(), 
                description: command.Description, 
                isActive: true, 
                createdAt: DateTime.Now);

            _driverlicenseTypeRepository.Add(driverLicenseType);
            await _driverlicenseTypeRepository.UnitOfWork.Commit();

            var addDriverLicenseTypeResponse = new AddDriverLicenseTypeResponse
            {
                Id = driverLicenseType.Id,
                Code = driverLicenseType.Code,
                Description = driverLicenseType.Description,
                IsActive = driverLicenseType.IsActive,
                CreatedAt = driverLicenseType.CreatedAt
            };

            return Response.Ok(DriverLicenseTypeMessages.DriverLicenseType_Registered_Successfully, addDriverLicenseTypeResponse);
        }

        private async Task ValidateBusinessRulesAsync(AddDriverLicenseTypeCommand command)
        {
            var active = await _driverlicenseTypeRepository.GetActiveByCodeAsync(command.Code);
            if (active != null)
                AddError("A license type with this code is already active.");

            var validCodes = Enum.GetNames(typeof(DriverLicenseTypeCode));
            if (!validCodes.Contains(command.Code, StringComparer.OrdinalIgnoreCase))
            {
                AddError($"Invalid driver license code '{command.Code}'. Must be one of: {string.Join(", ", validCodes)}.");
            }
        }

    }
}
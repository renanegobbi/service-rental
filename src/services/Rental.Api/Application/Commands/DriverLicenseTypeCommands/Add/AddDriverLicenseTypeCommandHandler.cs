using FluentValidation.Results;
using MediatR;
using Rental.Api.Application.Extensions;
using Rental.Api.Application.Services.Audit;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Entities.Audit;
using Rental.Core.DomainObjects.Enums;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Serilog;
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
        private readonly IAuditService _audit;

        public AddDriverLicenseTypeCommandHandler(IDriverLicenseTypeRepository driverlicenseTypeRepository, IAuditService audit)
        {
            _driverlicenseTypeRepository = driverlicenseTypeRepository;
            _audit = audit;
        }

        public async Task<IResponse> Handle(AddDriverLicenseTypeCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting AddDriverLicenseTypeCommand: Code={Code}, Description={Description}",
                command.Code, command.Description);

            if (!command.IsValid())
            {
                Log.Information("Validation failed for AddDriverLicenseTypeCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
            {
                Log.Information("Business rule validation failed for AddDriverLicenseTypeCommand: {@Errors}", ValidationResult.Errors);
                return Response.Fail(ValidationResult);
            }

            var driverLicenseType = command.ToDriverLicenseType();

            _driverlicenseTypeRepository.UnitOfWork.BeginTransaction();

            try
            {
                _driverlicenseTypeRepository.Add(driverLicenseType);

                await _driverlicenseTypeRepository.UnitOfWork.SaveChangesAsync();

                Log.Information("DriverLicenseType created: Code={Code}, Description={Description}",
                    driverLicenseType.Code, driverLicenseType.Description);

                var driverLicenseTypeResponse = driverLicenseType.ToAddDriverLicenseTypeResponse();

                await _audit.AddAsync(AuditEventType.Created, $"The DriverLicenseType {command.Code} - {command.Description} has been registered.", 
                    null, driverLicenseTypeResponse);

                var success = await _driverlicenseTypeRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                return Response.Ok(DriverLicenseTypeMessages.DriverLicenseType_Registered_Successfully, driverLicenseType.ToAddDriverLicenseTypeResponse());
            }
            catch (Exception ex)
            {
                await _driverlicenseTypeRepository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(AddDriverLicenseTypeCommand));
                return Response.Fail(CommonMessages.Error_Persisting_Data);
            }
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
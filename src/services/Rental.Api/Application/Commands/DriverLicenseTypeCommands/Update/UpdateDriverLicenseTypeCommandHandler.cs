using FluentValidation.Results;
using MediatR;
using Rental.Api.Application.Extensions;
using Rental.Api.Application.Services.Audit;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Entities.Audit;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Update
{
    public class UpdateDriverLicenseTypeCommandHandler : CommandHandler,
        IRequestHandler<UpdateDriverLicenseTypeCommand, IResponse>
    {
        private readonly IDriverLicenseTypeRepository _driverlicenseTypeRepository;
        private readonly IAuditService _audit;

        public UpdateDriverLicenseTypeCommandHandler(IDriverLicenseTypeRepository driverlicenseTypeRepository, IAuditService audit)
        {
            _driverlicenseTypeRepository = driverlicenseTypeRepository;
            _audit = audit;
        }

        public async Task<IResponse> Handle(UpdateDriverLicenseTypeCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting UpdateDriverLicenseTypeCommand: Id={Id}, Code={Code}, Description={Description}",
                command.Id, command.Code, command.Description);

            if (!command.IsValid())
            {
                Log.Information("Validation failed for UpdateDriverLicenseTypeCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }

            var driverLicenseType = await _driverlicenseTypeRepository.GetByIdAsync(command.Id);
            if (driverLicenseType == null)
            {
                Log.Information("{RentalPlan_ID_Not_Found}: ", DriverLicenseTypeMessages.DriverLicenseType_ID_Not_Found);
                return Response.Fail(DriverLicenseTypeMessages.DriverLicenseType_ID_Not_Found);
            }

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
            {
                Log.Information("Business rule validation failed for UpdateDriverLicenseTypeCommand: {@Errors}", ValidationResult.Errors);
                return Response.Fail(ValidationResult);
            }

            var driverLicenseTypePlanBefore = driverLicenseType.ToUpdateDriverLicenseTypeResponse();

            _driverlicenseTypeRepository.UnitOfWork.BeginTransaction();

            try
            {
                driverLicenseType.Update(command.Code.Trim().ToUpper(), command.Description);

                _driverlicenseTypeRepository.Update(driverLicenseType);

                await _driverlicenseTypeRepository.UnitOfWork.SaveChangesAsync();

                var driverLicenseTypeResponse = driverLicenseType.ToUpdateDriverLicenseTypeResponse();

                await _audit.AddAsync(AuditEventType.Updated, $"The DriverLicenseType {driverLicenseTypePlanBefore.Id} - {driverLicenseTypePlanBefore.Code} has been updated.",
                    driverLicenseTypePlanBefore, driverLicenseTypeResponse);

                var success = await _driverlicenseTypeRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                Log.Information("DriverLicenseType updated: Id={Id}, Code={Code}, Description={Description}",
                    driverLicenseType.Id, driverLicenseType.Code, driverLicenseType.Description);

                return Response.Ok(DriverLicenseTypeMessages.DriverLicenseType_Updated_Successfully, driverLicenseType.ToUpdateDriverLicenseTypeResponse());
            }
            catch (System.Exception ex)
            {
                await _driverlicenseTypeRepository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(UpdateDriverLicenseTypeCommand));
                throw;
            }
        }

        private async Task ValidateBusinessRulesAsync(UpdateDriverLicenseTypeCommand command)
        {
            var active = await _driverlicenseTypeRepository.GetActiveByCodeAsync(command.Code);
            if (active != null)
                AddError("A license type with this code is already active.");
        }

    }
}
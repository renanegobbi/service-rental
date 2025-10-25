using FluentValidation.Results;
using MediatR;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Core.Data;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Update
{
    public class UpdateDriverLicenseTypeCommandHandler : CommandHandler,
        IRequestHandler<UpdateDriverLicenseTypeCommand, IResponse>
    {
        private readonly IDriverLicenseTypeRepository _driverlicenseTypeRepository;

        public UpdateDriverLicenseTypeCommandHandler(IDriverLicenseTypeRepository driverlicenseTypeRepository)
        {
            _driverlicenseTypeRepository = driverlicenseTypeRepository;
        }

        public async Task<IResponse> Handle(UpdateDriverLicenseTypeCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            var driverLicenseType = await _driverlicenseTypeRepository.GetByIdAsync(command.Id);

            if (driverLicenseType is null)
                AddError("The driver license type with the specified ID does not exist.");

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            driverLicenseType.Update(command.Code.Trim().ToUpper(), command.Description);
            _driverlicenseTypeRepository.Update(driverLicenseType);
            await _driverlicenseTypeRepository.UnitOfWork.Commit();

            var updateDriverLicenseTypeResponse = new UpdateDriverLicenseTypeResponse
            {
                Id = driverLicenseType.Id,
                Code = driverLicenseType.Code,
                Description = driverLicenseType.Description,
                IsActive = driverLicenseType.IsActive,
                CreatedAt = driverLicenseType.CreatedAt
            };

            return Response.Ok(DriverLicenseTypeMessages.DriverLicenseType_Updated_Successfully, updateDriverLicenseTypeResponse);
        }

        private async Task ValidateBusinessRulesAsync(UpdateDriverLicenseTypeCommand command)
        {
            var active = await _driverlicenseTypeRepository.GetActiveByCodeAsync(command.Code);
            if (active != null)
                AddError("A license type with this code is already active.");
        }

    }
}
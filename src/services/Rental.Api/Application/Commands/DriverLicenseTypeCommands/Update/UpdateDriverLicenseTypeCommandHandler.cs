using FluentValidation.Results;
using MediatR;
using Rental.Api.Application.Extensions;
using Rental.Api.Data.Repositories.Interfaces;
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

            var driverLicenseType = await _driverlicenseTypeRepository.GetByIdAsync(command.Id);
            if (driverLicenseType == null)
                return Response.Fail($"The driver license type with ID '{command.Id}' was not found.");

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            driverLicenseType.Update(command.Code.Trim().ToUpper(), command.Description);
            _driverlicenseTypeRepository.Update(driverLicenseType);
            var success = await _driverlicenseTypeRepository.UnitOfWork.Commit();

            if (!success)
                return Response.Fail(CommonMessages.Error_Persisting_Data);

            return Response.Ok(DriverLicenseTypeMessages.DriverLicenseType_Updated_Successfully, driverLicenseType.ToUpdateDriverLicenseTypeResponse());
        }

        private async Task ValidateBusinessRulesAsync(UpdateDriverLicenseTypeCommand command)
        {
            var active = await _driverlicenseTypeRepository.GetActiveByCodeAsync(command.Code);
            if (active != null)
                AddError("A license type with this code is already active.");
        }

    }
}
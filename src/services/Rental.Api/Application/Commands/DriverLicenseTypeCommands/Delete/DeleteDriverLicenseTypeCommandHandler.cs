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

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Delete
{
    public class DeleteRentalPlanCommandHandler : CommandHandler,
    IRequestHandler<DeleteDriverLicenseTypeCommand, IResponse>
    {
        private readonly IDriverLicenseTypeRepository _repository;
        private readonly IAuditService _audit;

        public DeleteRentalPlanCommandHandler(IDriverLicenseTypeRepository repository, IAuditService audit)
        {
            _repository = repository;
            _audit = audit;
        }

        public async Task<IResponse> Handle(DeleteDriverLicenseTypeCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting DeleteDriverLicenseTypeCommand: Id={Id}", command.Id);

            if (!command.IsValid())
            {
                Log.Warning("Validation failed for DeleteDriverLicenseTypeCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }

            var driverLicenseType = await _repository.GetByIdAsync(command.Id);

            if (driverLicenseType is null)
            {
                Log.Information("{DriverLicenseType_ID_Not_Found}: ", DriverLicenseTypeMessages.DriverLicenseType_ID_Not_Found);
                return Response.Fail(DriverLicenseTypeMessages.DriverLicenseType_ID_Not_Found);
            }

            var driverLicenseTypeBefore = driverLicenseType.ToDeleteDriverLicenseTypeResponse();

            _repository.UnitOfWork.BeginTransaction();

            try
            {
                driverLicenseType.RemoveDriverLicenseType();

                _repository.Update(driverLicenseType);

                await _repository.UnitOfWork.SaveChangesAsync();

                var driverLicenseTypeResponse = driverLicenseType.ToDeleteDriverLicenseTypeResponse();

                await _audit.AddAsync(AuditEventType.Deleted, $"The DriverLicenseType {driverLicenseTypeBefore.Id} - {driverLicenseTypeBefore.Code} has been deleted.",
                    driverLicenseTypeBefore, driverLicenseTypeResponse);

                var success = await _repository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                Log.Information("DriverLicenseType deleted: Id={Id}, Code={Code}, Description={Description}",
                    driverLicenseType.Id, driverLicenseType.Code, driverLicenseType.Description);

                return Response.Ok(DriverLicenseTypeMessages.DriverLicenseType_Deleted_Successfully, driverLicenseTypeResponse);
            }
            catch (System.Exception ex)
            {
                await _repository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(DeleteDriverLicenseTypeCommand));
                throw;
            }
        }
    }

}

using MediatR;
using Rental.Api.Application.Extensions;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Delete
{
    public class DeleteRentalPlanCommandHandler : CommandHandler,
    IRequestHandler<DeleteDriverLicenseTypeCommand, IResponse>
    {
        private readonly IDriverLicenseTypeRepository _repository;

        public DeleteRentalPlanCommandHandler(IDriverLicenseTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResponse> Handle(DeleteDriverLicenseTypeCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            var driverLicenseType = await _repository.GetByIdAsync(command.Id);

            if (driverLicenseType is null)
                return Response.Fail(DriverLicenseTypeMessages.DriverLicenseType_ID_Not_Found);

            driverLicenseType.RemoveDriverLicenseType();
            _repository.Update(driverLicenseType);
            var success = await _repository.UnitOfWork.CommitTransaction();

            if (!success)
                return Response.Fail(CommonMessages.Error_Persisting_Data);

            return Response.Ok(DriverLicenseTypeMessages.DriverLicenseType_Deleted_Successfully, driverLicenseType.ToDeleteDriverLicenseTypeResponse());
        }
    }

}

using MediatR;
using Rental.Api.Application.DTOs.DriverLicenseType;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.DriverLicenseTypeCommands.Delete
{
    public class DeleteDriverLicenseTypeCommandHandler : CommandHandler,
    IRequestHandler<DeleteDriverLicenseTypeCommand, IResponse>
    {
        private readonly IDriverLicenseTypeRepository _repository;

        public DeleteDriverLicenseTypeCommandHandler(IDriverLicenseTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResponse> Handle(DeleteDriverLicenseTypeCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            var entity = await _repository.GetByIdAsync(command.Id);

            if (entity is null)
                return Response.Fail(DriverLicenseTypeMessages.DriverLicenseType_ID_Not_Found);

            entity.RemoveDriverLicenseType();
            _repository.Update(entity);
            await _repository.UnitOfWork.Commit();

            var deleteDriverLicenseTypeResponse = new DeleteDriverLicenseTypeResponse
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt
            };

            return Response.Ok(message: DriverLicenseTypeMessages.DriverLicenseType_Deleted_Successfully, data: deleteDriverLicenseTypeResponse);
        }
    }

}

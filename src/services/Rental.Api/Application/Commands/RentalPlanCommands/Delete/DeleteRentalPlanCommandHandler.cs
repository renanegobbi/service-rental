using MediatR;
using Rental.Api.Application.Extensions;
using Rental.Api.Infrastructure.Repository;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.RentalPlanCommands.Delete
{
    public class DeleteRentalPlanCommandHandler : CommandHandler,
        IRequestHandler<DeleteRentalPlanCommand, IResponse>
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;

        public DeleteRentalPlanCommandHandler(IRentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }

        public async Task<IResponse> Handle(DeleteRentalPlanCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            var rentalPlan = await _rentalPlanRepository.GetByIdAsync(command.Id);

            if (rentalPlan is null)
                return Response.Fail(RentalPlanMessages.RentalPlan_ID_Not_Found);

            _rentalPlanRepository.Delete(rentalPlan);            
            var success = await _rentalPlanRepository.UnitOfWork.Commit();

            if (!success)
                return Response.Fail(CommonMessages.Error_Persisting_Data);

            return Response.Ok(RentalPlanMessages.RentalPlan_Deleted_Successfully, rentalPlan.ToDeleteRentalPlanResponse());
        }
    }
}

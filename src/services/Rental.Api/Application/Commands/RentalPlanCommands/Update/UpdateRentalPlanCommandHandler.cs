using MediatR;
using Rental.Api.Application.Extensions;
using Rental.Api.Infrastructure.Repository;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.RentalPlanCommands.Update
{
    public class UpdateRentalPlanCommandHandler : CommandHandler,
        IRequestHandler<UpdateRentalPlanCommand, IResponse>
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;

        public UpdateRentalPlanCommandHandler(IRentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }

        public async Task<IResponse> Handle(UpdateRentalPlanCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            var rentalPlan = await _rentalPlanRepository.GetByIdAsync(command.Id);
            if (rentalPlan == null)
                return Response.Fail($"Rental plan with ID '{command.Id}' was not found.");

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            rentalPlan.Update(command.DailyRate, command.PenaltyPercent, command.Description);
            _rentalPlanRepository.Update(rentalPlan);
            var success = await _rentalPlanRepository.UnitOfWork.Commit();

            if (!success)
                return Response.Fail(CommonMessages.Error_Persisting_Data);

            return Response.Ok(RentalPlanMessages.RentalPlan_Updated_Successfully, rentalPlan.ToUpdateRentalPlanResponse());
        }

        private async Task ValidateBusinessRulesAsync(UpdateRentalPlanCommand command)
        {
            var existingPlans = await _rentalPlanRepository.GetAllAsync();
            var duplicate = existingPlans.Any(p => p.Days == command.Days && p.Id != command.Id);

            if (duplicate)
                AddError($"Another rental plan with {command.Days} days already exists.");
        }
    }
}

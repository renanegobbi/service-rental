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

namespace Rental.Api.Application.Commands.RentalPlanCommands.Add
{
    public class AddRentalPlanCommandHandler : CommandHandler, 
        IRequestHandler<AddRentalPlanCommand, IResponse>
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;

        public AddRentalPlanCommandHandler(IRentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }

        public async Task<IResponse> Handle(AddRentalPlanCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            var rentalPlan = command.ToRentalPlan();

            _rentalPlanRepository.Add(rentalPlan);
            var success = await _rentalPlanRepository.UnitOfWork.CommitTransaction();

            if (!success)
                return Response.Fail(CommonMessages.Error_Persisting_Data);

            return Response.Ok(RentalPlanMessages.RentalPlan_Registered_Successfully, rentalPlan.ToAddRentalPlanResponse());
        }

        private async Task ValidateBusinessRulesAsync(AddRentalPlanCommand command)
        {
            var existingPlans = await _rentalPlanRepository.GetAllAsync();
            if (existingPlans.Any(p => p.Days == command.Days))
            {
                AddError($"A rental plan with {command.Days} days already exists.");
                return;
            }

            if (command.PenaltyPercent < 0 || command.PenaltyPercent > 100)
                AddError("Penalty percent must be between 0 and 100.");
        }
    }
}

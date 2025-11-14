using MediatR;
using Rental.Api.Application.Extensions;
using Rental.Api.Infrastructure.Repository;
using Rental.Core.Interfaces;
using Rental.Core.Messages;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Serilog;
using System;
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
            Log.Information("Starting UpdateRentalPlanCommand: Id={Id}, DailyRate={DailyRate}, PenaltyPercent={PenaltyPercent}, Description={Description}",
                command.Id, command.DailyRate, command.PenaltyPercent, command.Description);

            if (!command.IsValid())
                return Response.Fail(command.ValidationResult);

            var rentalPlan = await _rentalPlanRepository.GetByIdAsync(command.Id);
            if (rentalPlan == null)
                return Response.Fail(RentalPlanMessages.RentalPlan_ID_Not_Found);

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
                return Response.Fail(ValidationResult);

            _rentalPlanRepository.UnitOfWork.BeginTransaction();

            try
            {
                rentalPlan.Update(command.DailyRate, command.PenaltyPercent, command.Description);
                _rentalPlanRepository.Update(rentalPlan);
                var success = await _rentalPlanRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                Log.Information("RentalPlan updated: Id={Id}, Days={Days}, DailyRate={DailyRate}, PenaltyPercent={PenaltyPercent}, Description={Description}",
                    rentalPlan.Id, rentalPlan.Days, rentalPlan.DailyRate, rentalPlan.PenaltyPercent, rentalPlan.Description);

                return Response.Ok(RentalPlanMessages.RentalPlan_Updated_Successfully, rentalPlan.ToUpdateRentalPlanResponse());
            }
            catch (Exception ex)
            {
                _rentalPlanRepository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(UpdateRentalPlanCommand));
                throw;
            }
        }

        private async Task ValidateBusinessRulesAsync(UpdateRentalPlanCommand command)
        {
            var existingPlans = await _rentalPlanRepository.GetAllAsync();

            var plan = await _rentalPlanRepository.GetByIdAsync(command.Id);

            var duplicate = existingPlans.Any(p => p.Days == plan.Days && p.Id != plan.Id);

            if (duplicate)
                AddError($"Another rental plan with {plan.Days} days already exists.");
        }
    }
}

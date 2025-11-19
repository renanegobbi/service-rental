using MediatR;
using Rental.Api.Application.Extensions;
using Rental.Api.Application.Services.Audit;
using Rental.Api.Entities.Audit;
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
        private readonly IAuditService _audit;

        public UpdateRentalPlanCommandHandler(IRentalPlanRepository rentalPlanRepository, IAuditService audit)
        {
            _rentalPlanRepository = rentalPlanRepository;
            _audit = audit;
        }

        public async Task<IResponse> Handle(UpdateRentalPlanCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting UpdateRentalPlanCommand: Id={Id}, DailyRate={DailyRate}, PenaltyPercent={PenaltyPercent}, Description={Description}",
                command.Id, command.DailyRate, command.PenaltyPercent, command.Description);

            if (!command.IsValid())
            {
                Log.Information("Validation failed for UpdateRentalPlanCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }
                
            var rentalPlan = await _rentalPlanRepository.GetByIdAsync(command.Id);
            if (rentalPlan == null)
            {
                Log.Information("{RentalPlan_ID_Not_Found}: ", RentalPlanMessages.RentalPlan_ID_Not_Found);
                return Response.Fail(RentalPlanMessages.RentalPlan_ID_Not_Found);
            }

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
            {
                Log.Information("Business rule validation failed for UpdateRentalPlanCommand: {@Errors}", ValidationResult.Errors);
                return Response.Fail(ValidationResult);
            }

            var rentalPlanBefore = rentalPlan.ToUpdateRentalPlanResponse();

            _rentalPlanRepository.UnitOfWork.BeginTransaction();

            try
            {
                rentalPlan.Update(command.DailyRate, command.PenaltyPercent, command.Description);

                _rentalPlanRepository.Update(rentalPlan);

                await _rentalPlanRepository.UnitOfWork.SaveChangesAsync();

                var rentalPlanResponse = rentalPlan.ToUpdateRentalPlanResponse();

                await _audit.AddAsync(AuditEventType.Updated, $"The RentalPlan {rentalPlanBefore.Id} - {rentalPlanBefore.DailyRate} has been updated.", 
                    rentalPlanBefore, rentalPlanResponse);

                var success = await _rentalPlanRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                Log.Information("RentalPlan updated: Id={Id}, Days={Days}, DailyRate={DailyRate}, PenaltyPercent={PenaltyPercent}, Description={Description}",
                    rentalPlan.Id, rentalPlan.Days, rentalPlan.DailyRate, rentalPlan.PenaltyPercent, rentalPlan.Description);

                return Response.Ok(RentalPlanMessages.RentalPlan_Updated_Successfully, rentalPlanResponse);
            }
            catch (Exception ex)
            {
                await _rentalPlanRepository.UnitOfWork.RollbackTransaction();
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

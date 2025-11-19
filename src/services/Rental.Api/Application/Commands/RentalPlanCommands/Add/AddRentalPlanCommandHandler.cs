using MediatR;
using Rental.Api.Application.Commands.RentalPlanCommands.Update;
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

namespace Rental.Api.Application.Commands.RentalPlanCommands.Add
{
    public class AddRentalPlanCommandHandler : CommandHandler, 
        IRequestHandler<AddRentalPlanCommand, IResponse>
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;
        private readonly IAuditService _audit;

        public AddRentalPlanCommandHandler(IRentalPlanRepository rentalPlanRepository, IAuditService audit)
        {
            _rentalPlanRepository = rentalPlanRepository;
            _audit = audit;        
        }

        public async Task<IResponse> Handle(AddRentalPlanCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting AddRentalPlanCommand: DailyRate={DailyRate}, PenaltyPercent={PenaltyPercent}, Description={Description}",
                command.DailyRate, command.PenaltyPercent, command.Description);

            if (!command.IsValid())
            {
                Log.Information("Validation failed for AddRentalPlanCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
            {
                Log.Information("Business rule validation failed for AddRentalPlanCommand: {@Errors}", ValidationResult.Errors);
                return Response.Fail(ValidationResult);
            }

            var rentalPlan = command.ToRentalPlan();

            _rentalPlanRepository.UnitOfWork.BeginTransaction();

            try
            {
                _rentalPlanRepository.Add(rentalPlan);

                await _rentalPlanRepository.UnitOfWork.SaveChangesAsync();

                Log.Information("RentalPlan created: Days={Days}, DailyRate={DailyRate}, PenaltyPercent={PenaltyPercent}, Description={Description}",
                    rentalPlan.Days, rentalPlan.DailyRate, rentalPlan.PenaltyPercent, rentalPlan.Description);

                var rentalPlanResponse = rentalPlan.ToAddRentalPlanResponse();

                await _audit.AddAsync(AuditEventType.Created, $"The RentalPlan {command.DailyRate} has been registered.", null, rentalPlanResponse);

                var success = await _rentalPlanRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                return Response.Ok(RentalPlanMessages.RentalPlan_Registered_Successfully, rentalPlanResponse);
            }
            catch (Exception ex)
            {
                await _rentalPlanRepository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(UpdateRentalPlanCommand));
                return Response.Fail(CommonMessages.Error_Persisting_Data);
            }           
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

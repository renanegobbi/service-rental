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
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Commands.RentalPlanCommands.Delete
{
    public class DeleteRentalPlanCommandHandler : CommandHandler,
        IRequestHandler<DeleteRentalPlanCommand, IResponse>
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;
        private readonly IAuditService _audit;

        public DeleteRentalPlanCommandHandler(IRentalPlanRepository rentalPlanRepository, IAuditService audit)
        {
            _rentalPlanRepository = rentalPlanRepository;
            _audit = audit;
        }

        public async Task<IResponse> Handle(DeleteRentalPlanCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting DeleteRentalPlanCommand: Id={Id}", command.Id);

            if (!command.IsValid())
            {
                Log.Warning("Validation failed for DeleteRentalPlanCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }

            var rentalPlan = await _rentalPlanRepository.GetByIdAsync(command.Id);

            if (rentalPlan is null)
            {
                Log.Information("{RentalPlan_ID_Not_Found}: ", RentalPlanMessages.RentalPlan_ID_Not_Found);
                return Response.Fail(RentalPlanMessages.RentalPlan_ID_Not_Found);
            }

            var rentalPlanBefore = rentalPlan.ToDeleteRentalPlanResponse();

            _rentalPlanRepository.UnitOfWork.BeginTransaction();

            try
            {
                _rentalPlanRepository.Delete(rentalPlan);

                await _rentalPlanRepository.UnitOfWork.SaveChangesAsync();

                var rentalPlanResponse = rentalPlan.ToDeleteRentalPlanResponse();

                await _audit.AddAsync(AuditEventType.Deleted, $"The RentalPlan {rentalPlanBefore.Id} - {rentalPlanBefore.DailyRate} has been deleted.",
                    rentalPlanBefore, null);

                var success = await _rentalPlanRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                Log.Information("RentalPlan deleted: Id={Id}, Days={Days}, DailyRate={DailyRate}, PenaltyPercent={PenaltyPercent}, Description={Description}",
                    rentalPlan.Id, rentalPlan.Days, rentalPlan.DailyRate, rentalPlan.PenaltyPercent, rentalPlan.Description);

                return Response.Ok(RentalPlanMessages.RentalPlan_Deleted_Successfully, rentalPlanResponse);
            }
            catch (Exception ex)
            {
                await _rentalPlanRepository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(DeleteRentalPlanCommand));
                throw;
            }
            
        }
    }
}

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

namespace Rental.Api.Application.Commands.MotorcycleCommands.Delete
{
    public class DeleteMotorcycleCommandHandler : CommandHandler,
        IRequestHandler<DeleteMotorcycleCommand, IResponse>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IAuditService _audit;

        public DeleteMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IAuditService audit)
        {
            _motorcycleRepository = motorcycleRepository;
            _audit = audit;
        }

        public async Task<IResponse> Handle(DeleteMotorcycleCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting DeleteMotorcycleCommand: Id={Id}", command.Id);

            if (!command.IsValid())
            {
                Log.Warning("Validation failed for DeleteMotorcycleCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }

            var motorcycle = await _motorcycleRepository.GetByIdAsync(command.Id);

            if (motorcycle is null)
            {
                Log.Information("{Motorcycle_ID_Not_Found}: ", MotorcycleMessages.Motorcycle_ID_Not_Found);
                return Response.Fail(MotorcycleMessages.Motorcycle_ID_Not_Found);
            }

            var motorcyclenBefore = motorcycle.ToDeleteMotorcycleResponse();

            _motorcycleRepository.UnitOfWork.BeginTransaction();

            try
            {
                _motorcycleRepository.Delete(motorcycle);

                await _motorcycleRepository.UnitOfWork.SaveChangesAsync();

                var motorcycleResponse = motorcycle.ToDeleteMotorcycleResponse();

                await _audit.AddAsync(AuditEventType.Deleted, $"The Motorcycle {motorcyclenBefore.Id} - {motorcyclenBefore.Plate} has been deleted.",
                    motorcyclenBefore, motorcycleResponse);

                var success = await _motorcycleRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                Log.Information("Motorcycle deleted: Id={Id}, Year={Year}, Model={Model}, Plate={Plate}, CreatedAt={CreatedAt}",
                    motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate, motorcycle.CreatedAt);

                return Response.Ok(MotorcycleMessages.Motorcycle_Deleted_Successfully, motorcycleResponse);
            }
            catch (Exception ex)
            {
                await _motorcycleRepository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(DeleteMotorcycleCommand));
                throw;
            }
        }
    }
}

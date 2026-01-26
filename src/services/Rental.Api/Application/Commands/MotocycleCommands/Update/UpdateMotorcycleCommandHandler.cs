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

namespace Rental.Api.Application.Commands.MotocycleCommands.Update
{
    public class UpdateMotorcycleCommandHandler : CommandHandler,
        IRequestHandler<UpdateMotorcycleCommand, IResponse>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IAuditService _audit;

        public UpdateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IAuditService audit)
        {
            _motorcycleRepository = motorcycleRepository;
            _audit = audit;
        }

        public async Task<IResponse> Handle(UpdateMotorcycleCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting UpdateMotorcycleCommand: Year={Year}, Model={Model}, Plate={Plate}",
                command.Year, command.Model, command.Plate);

            if (!command.IsValid())
            {
                Log.Information("Validation failed for UpdateMotorcycleCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }

            var motorcycle = await _motorcycleRepository.GetByIdAsync(command.Id);
            if (motorcycle == null)
            {
                Log.Information("{Motorcycle_ID_Not_Found}: ", MotorcycleMessages.Motorcycle_ID_Not_Found);
                return Response.Fail(MotorcycleMessages.Motorcycle_ID_Not_Found);
            }

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
            {
                Log.Information("Business rule validation failed for UpdateMotorcycleCommand: {@Errors}", ValidationResult.Errors);
                return Response.Fail(ValidationResult);
            }

            var motorcycleBefore = motorcycle.ToUpdateMotorcycleResponse();

            _motorcycleRepository.UnitOfWork.BeginTransaction();

            try
            {
                motorcycle.Update(command.Year, command.Model, command.Plate);

                _motorcycleRepository.Update(motorcycle);

                await _motorcycleRepository.UnitOfWork.SaveChangesAsync();

                var motorcycleResponse = motorcycle.ToUpdateMotorcycleResponse();

                await _audit.AddAsync(AuditEventType.Updated, $"The Motorcycle {motorcycleBefore.Id} - {motorcycleBefore.Plate} has been updated.",
                    motorcycleBefore, motorcycleResponse);

                var success = await _motorcycleRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                Log.Information("Motorcycle updated: Id={Id}, Year={Year}, Model={Model}, Plate={Plate}",
                    motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate);

                return Response.Ok(MotorcycleMessages.Motorcycle_Updated_Successfully, motorcycleResponse);
            }
            catch (Exception ex)
            {
                await _motorcycleRepository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(UpdateMotorcycleCommand));
                throw;
            }
        }

        private async Task ValidateBusinessRulesAsync(UpdateMotorcycleCommand command)
        {
            var existingMotorcycle = await _motorcycleRepository.GetAllAsync();

            var motorcycle = await _motorcycleRepository.GetByIdAsync(command.Id);

            var duplicate = existingMotorcycle.Any(m => m.Year == motorcycle.Year &&
                                                   m.Model == motorcycle.Model &&
                                                   m.Plate == motorcycle.Plate && 
                                                   m.Id != motorcycle.Id);

            if (duplicate)
                AddError($"Another motorcycle with {motorcycle.Year}, {motorcycle.Model} and {motorcycle.Plate} already exists.");
        }
    }
}

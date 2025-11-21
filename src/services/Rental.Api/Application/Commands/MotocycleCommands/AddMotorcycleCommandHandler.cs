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

namespace Rental.Api.Application.Commands.MotorcycleCommands.Add
{
    public class AddMotorcycleCommandHandler : CommandHandler,
        IRequestHandler<AddMotorcycleCommand, IResponse>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IAuditService _audit;

        public AddMotorcycleCommandHandler(
            IMotorcycleRepository motorcycleRepository,
            IAuditService audit)
        {
            _motorcycleRepository = motorcycleRepository;
            _audit = audit;
        }

        public async Task<IResponse> Handle(AddMotorcycleCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Starting AddMotorcycleCommand: Year={Year}, Model={Model}, Plate={Plate}",
                command.Year, command.Model, command.Plate);

            if (!command.IsValid())
            {
                Log.Information("Validation failed for AddMotorcycleCommand: {@Errors}", command.ValidationResult.Errors);
                return Response.Fail(command.ValidationResult);
            }

            await ValidateBusinessRulesAsync(command);

            if (!ValidationResult.IsValid)
            {
                Log.Information("Business rule validation failed for AddMotorcycleCommand: {@Errors}", ValidationResult.Errors);
                return Response.Fail(ValidationResult);
            }

            var motorcycle = command.ToMotorcycle();

            _motorcycleRepository.UnitOfWork.BeginTransaction();

            try
            {
                _motorcycleRepository.Add(motorcycle);

                await _motorcycleRepository.UnitOfWork.SaveChangesAsync();

                Log.Information("Motorcycle created: Year={Year}, Model={Model}, Plate={Plate}",
                    motorcycle.Year, motorcycle.Model, motorcycle.Plate);

                var motorcycleResponse = motorcycle.ToAddMotorcycleResponse();

                await _audit.AddAsync(
                    eventType: AuditEventType.Created,
                    message: $"The Motorcycle {motorcycle.Plate} has been registered.",
                    beforeState: null,
                    afterState: motorcycleResponse);

                var success = await _motorcycleRepository.UnitOfWork.CommitTransaction();

                if (!success)
                    return Response.Fail(CommonMessages.Error_Persisting_Data);

                return Response.Ok("Motorcycle registered successfully.", motorcycleResponse);
            }
            catch (Exception ex)
            {
                await _motorcycleRepository.UnitOfWork.RollbackTransaction();
                Log.Error(ex, "Error while executing {Command}", nameof(AddMotorcycleCommand));
                return Response.Fail(CommonMessages.Error_Persisting_Data);
            }
        }

        private async Task ValidateBusinessRulesAsync(AddMotorcycleCommand command)
        {
            var existingByPlate = await _motorcycleRepository.GetByPlateAsync(command.Plate);
            if (existingByPlate != null)
            {
                AddError($"A motorcycle with plate '{command.Plate}' already exists.");
            }

            if (command.Year < 2001)
                AddError("Year must be greater than 2000.");
        }
    }
}

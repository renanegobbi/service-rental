using FluentValidation.Results;
using MediatR;
using Rental.Api.Application.Events;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Entities;
using Rental.Core.Messages;

namespace Rental.Api.Application.Commands.MotocycleCommands
{
    public class MotorcycleCommandHandler : CommandHandler,
        IRequestHandler<RegisterMotorcycleCommand, ValidationResult>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<ValidationResult> Handle(RegisterMotorcycleCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var motorcycle = new Motorcycle(message.Id, message.Year, message.Model, message.Plate);

            _motorcycleRepository.Add(motorcycle);

            motorcycle.AddEvent(new MotorcycleRegisteredEvent(message.Id, message.Year, message.Model, message.Plate));

            return await PersistData(_motorcycleRepository.UnitOfWork);
        }
    }
}
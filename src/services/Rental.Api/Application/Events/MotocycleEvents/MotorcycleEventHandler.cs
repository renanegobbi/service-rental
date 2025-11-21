using MediatR;
using Rental.Core.Messages.Integration.Rental;
using Rental.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Application.Events.MotocycleEvents
{
    public class MotorcycleEventHandler : INotificationHandler<MotorcycleRegisteredEvent>
    {
        private readonly IMessageBus _bus;
        public MotorcycleEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public Task Handle(MotorcycleRegisteredEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Handler] MotorcycleRegisteredEvent received: {notification.Model} ({DateTime.UtcNow:HH:mm:ss.fff})");

            var motorcycleRegistered = new MotorcycleRegisteredIntegrationEvent(
                notification.Id, notification.Year, notification.Model, notification.Plate);

            var success = _bus.PublishAsync(motorcycleRegistered);

            Console.WriteLine($"[Handler] MotorcycleRegisteredEvent received: {notification.Model} ({DateTime.UtcNow:HH:mm:ss.fff})");

            return Task.CompletedTask;
        }
    }
}
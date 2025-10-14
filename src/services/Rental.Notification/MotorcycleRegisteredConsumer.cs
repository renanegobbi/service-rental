using Rental.Core.Messages.Integration.Rental;
using Rental.MessageBus;

namespace Rental.Consumer.Worker.Services
{
    public class MotorcycleRegisteredConsumer
    {
        private readonly IMessageBus _bus;
        private readonly ILogger<MotorcycleRegisteredConsumer> _logger;

        public MotorcycleRegisteredConsumer(IMessageBus bus, ILogger<MotorcycleRegisteredConsumer> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Subscribing consumer to the MotorcycleRegisteredIntegrationEnvelope queue...");

            _bus.SubscribeAsync<MotorcycleRegisteredIntegrationEvent>(
                "rental-motorcycle-subscriber",
                HandleMessage);
        }

        private Task HandleMessage(MotorcycleRegisteredIntegrationEvent msg)
        {
            _logger.LogInformation("Message received:");
            _logger.LogInformation($"Id: {msg.Id}");
            _logger.LogInformation($"Model: {msg.Model}");
            _logger.LogInformation($"Plate: {msg.Plate}");
            _logger.LogInformation($"Year: {msg.Year}");

            return Task.CompletedTask;
        }
    }
}

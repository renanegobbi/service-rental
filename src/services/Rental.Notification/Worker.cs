using Rental.Core.Messages.Integration.Rental;
using Rental.MessageBus;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMessageBus _bus;

    public Worker(ILogger<Worker> logger, IMessageBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started. Waiting for messages...");

        _bus.SubscribeAsync<MotorcycleRegisteredIntegrationEvent>(
            subscriptionId: "rental.notification.worker.dev",
            onMessage: async msg =>
            {
                try
                {
                    _logger.LogInformation(
                        "Event received: {Model} - Plate {Plate} - Id {Id}",
                        msg.Model, msg.Plate, msg.Id);

                    await Task.Delay(50, stoppingToken);

                    _logger.LogInformation("Successfully processed: {Plate}", msg.Plate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message {Id}", msg.Id);
                }
            });

        return Task.CompletedTask;
    }
}


using Rental.Core.Messages.Integration.Rental;
using Rental.MessageBus;

namespace Rental.Notification.Services
{
    public class MotorcycleNotificationsHandler : BackgroundService
    {
        private readonly IMessageBus _bus; private readonly IServiceProvider _sp;
        public MotorcycleNotificationsHandler(IMessageBus bus, IServiceProvider sp)
        {
            _bus = bus; _sp = sp;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus.SubscribeAsync<MotorcycleRegisteredIntegrationEvent>("NotificationService",
                async evt =>
                {
                    if (evt.Year != 2024) return;

                    using var scope = _sp.CreateScope();
                });

            return Task.CompletedTask;
        }
    }
}

